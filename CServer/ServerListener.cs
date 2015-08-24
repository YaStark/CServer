using CCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CServer
{
    class ServerListener : Listener, IDisposable
    {
        /// <summary>
        /// Dictionary for all users
        /// </summary>
        public Dictionary<string, UserInfo> Users = new Dictionary<string, UserInfo>();

        public delegate void OnUserStateEventHandler(string Username, UserState State);
        public event OnUserStateEventHandler OnUserState;

        public delegate void OnTalkOccurEventHandler(bool Enabled, string UserA, string UserB);
        public event OnTalkOccurEventHandler OnTalkOccur;

        public int UserAfkCheckMs = 5000;

        public Dictionary<CustomData, string> CustomData = new Dictionary<CustomData, string>();

        bool afkUserState = true;
        public bool AfkUserState
        {
            get { return afkUserState; }
            set 
            {
                if (afkUserState != value)
                {
                    if (afkUserState) StopCheckUsers();
                    else StartCheckUsers(userAfkTimeoutMs);
                }
                afkUserState = value;
            }
        }

        System.Timers.Timer userAfkTimer = new System.Timers.Timer();
        double userAfkTimeoutMs = 5000;

        public ServerListener() 
        {
            OnConnect += ServerListener_OnConnect;
            userAfkTimer.Elapsed += timer_Elapsed;
            Name = "Server";
        }

        void ServerListener_OnConnect(IPEndPoint client, byte[] data)
        {
            try
            {
                TcpMessage header = new TcpMessage(data);
                Decrypt(header, client);
            }
            catch (Exception e)
            {
                Msg("Cannot read data from [" + client.ToString() + "]: " + e.Message);
            }
        }

        /// <summary>
        /// Decrypting a request and create right responses
        /// </summary>
        /// <param name="request"></param>
        /// <param name="endPoint"></param>
        private void Decrypt(TcpMessage request, IPEndPoint receiver)
        {
            /* * * * If request handler call "SendAnswer" before quit, it's OK  * * * */
            /* Otherwise socket should be disconnected and closed in handler directly */

            if ((CodeMessage)request.Code == CodeMessage.CConnect)
            {
                HConnect(request);
                return;
            }

            if (!Users.ContainsKey(request.From))
            {
                Msg("Message from unknown source: [" + receiver.ToString() + "]");
                return;
            }
            receiver = Users[request.From].EndPoint;

            switch ((CodeMessage)request.Code)
            {
                case CodeMessage.CQuit:
                    HQuit(request);
                    break;

                case CodeMessage.CUserList:
                    HUserList(request);
                    break;

                case CodeMessage.CWriteFor:
                    HWriteFor(request);
                    break;

                case CodeMessage.CATalkOffer:
                    HTalkOffer(request);
                    break;

                case CodeMessage.CTalkEnd:
                    HTalkEnd(request);
                    break;

                case CodeMessage.CATalkAnswer:
                    HTalkAnswer(request);
                    break;

                case CodeMessage.CARequestCustomData:
                    HRequestCustomData(request);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Send broadcast answers to all users
        /// </summary>
        /// <param name="resp"></param>
        public void SendBroadcast(TcpMessage resp)
        {
            foreach (var User in Users) SendRequest(User.Value.EndPoint, resp.GetBytes());
        }

        /// <summary>
        /// Direct kick handler
        /// </summary>
        public void RemoveClient(string Client)
        {
            IPEndPoint receiver = null;
            lock (Users)
            {
                receiver = Users[Client].EndPoint;
                Users.Remove(Client);
            }
            if (receiver != null)
            {
                SendRequest(receiver,
                    new ResponseServerState(false, "Directly kicked").GetBytes());
                SendBroadcast(new ResponseUser(Client, UserState.UserLogOut));
                Msg("User " + Client + " directly kicked from server");
            }
        }

        /// <summary>
        /// Sending to gently close client's connections
        /// </summary>
        /// <param name="TimeoutMs">Time between start notification and server's closing</param>
        public void StopListenAsync(string Cause, int TimeoutMs)
        {
            if (!IsBound) return;
            Msg(String.Format("Shutdown in {0:T}", TimeSpan.FromMilliseconds(TimeoutMs)));
            SendBroadcast(new ResponseServerState(false, Cause));

            Task.Factory.StartNew(() => Thread.Sleep(TimeoutMs))
                        .ContinueWith((task) => StopListenAsync());
        }

        #region User afk handle
        
        /// <summary>
        /// Check users on afk state according to UserAfkTimeoutSec value. Lock Users
        /// </summary>
        private void _checkUsers()
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            lock (Users)
            {
                foreach (var user in Users)
                {
                    if (now.Subtract(user.Value.LastResp).TotalMilliseconds > userAfkTimeoutMs)
                    {
                        ResponseUser resp = new ResponseUser(user.Key, UserState.UserLogOut);
                        ResponseConnect conn = new ResponseConnect(
                            false, "Timeout afk kick");
                        this.SendRequest(user.Value.EndPoint, conn.GetBytes());
                        HQuit(resp);
                    }
                }
            }
            
        }

        public void StartCheckUsers(double UserAfkTimeoutMs)
        {
            if(userAfkTimer == null) userAfkTimer = new System.Timers.Timer();
            userAfkTimeoutMs = UserAfkTimeoutMs;
            userAfkTimer.AutoReset = true;
            userAfkTimer.Interval = UserAfkCheckMs;
            userAfkTimer.Start();
            Msg(String.Format("Afk events checking enabled (Interval={0}, timeout={1})", 
                                TimeSpan.FromMilliseconds(UserAfkCheckMs), 
                                TimeSpan.FromMilliseconds(UserAfkTimeoutMs)));
        }

        public void StopCheckUsers()
        {
            userAfkTimer.Stop();
            userAfkTimer.Dispose();
            userAfkTimer = null;
            Msg("Afk events checking disabled");
        }

        /// <summary>
        /// Change timeout for user afk commit. Ignored if change is smaller than 100 ms
        /// </summary>
        public void CheckUsersSetTimeout(double UserAfkTimeoutMs)
        {
            if (Math.Abs(UserAfkTimeoutMs - userAfkTimeoutMs) < 100) return;
            if (AfkUserState)
            {
                userAfkTimer.Stop();
                userAfkTimeoutMs = UserAfkTimeoutMs;
                userAfkTimer.Start();
                Msg(String.Format(
                    "Afk events timeout interval changed to {0}", 
                    TimeSpan.FromMilliseconds(UserAfkTimeoutMs)));
            }
            else
            {
                userAfkTimeoutMs = UserAfkTimeoutMs;
            }
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _checkUsers();
        }

        #endregion

        /* Different connect handlers */
        #region Connect Handlers

        /* Handlers for decrypt call requests and transactions */
        #region Call handlers

        /// <summary>
        /// Transact talk answer message and begins the udp connection if required
        /// </summary>
        private void HTalkAnswer(TcpMessage Request)
        {
            if ((TalkState)Request.State == TalkState.Adopt)
            {
                if (OnTalkOccur != null) OnTalkOccur(true, Request.From, Request.To);
            }
            IPEndPoint receiver = Users[Request.To].EndPoint;
            SendRequest(receiver, Request.GetBytes());
            IPEndPoint sender = Users[Request.From].EndPoint;
            SendRequest(sender, 
                new ResponseServerInfo("Talk answer with " + Request.To + " OK").GetBytes());
        }

        /// <summary>
        /// User request to end current talk
        /// </summary>
        public void HTalkEnd(TcpMessage Request)
        {
            if (OnTalkOccur != null) OnTalkOccur(false, Request.From, Request.To);
            IPEndPoint sender = Users[Request.From].EndPoint;
            IPEndPoint receiver = Users[Request.To].EndPoint;
            SendRequest(receiver, Request.GetBytes());
            SendRequest(sender, new ResponseServerInfo("Talk end OK").GetBytes());
        }

        /// <summary>
        /// Offer to talk
        /// </summary>
        public void HTalkOffer(TcpMessage Request)
        {
            if (!Users.ContainsKey(Request.To))
            {
                // This user is unavailable, and sender should remove this name from his userlist
                SendRequest(Users[Request.From].EndPoint,
                    new ResponseUser(Request.To, UserState.UserLogOut).GetBytes());
            }

            IPEndPoint receiver = Users[Request.To].EndPoint;
            SendRequest(receiver, Request.GetBytes());

            IPEndPoint sender = Users[Request.From].EndPoint;
            SendRequest(sender,
                new ResponseServerInfo("Talk offer with " + Request.To + " is processing").GetBytes());
        }

        #endregion

        /* Describes a common handlers */
        #region Common handlers

        /// <summary>
        /// User was connected
        /// </summary>
        public void HConnect(TcpMessage request)
        {
            string name = request.From;
            IPEndPoint tcpEP = null, udpEP = null;
            try
            {
                tcpEP = RequestConnect.GetRemotePoint(request);
                udpEP = RequestConnect.GetUdpRemotePoint(request);
            }
            catch
            {
                Msg("Wrong message format");
                // Cannot answer to client because info about listener's socket is broken
                return;
            }

            // We should check if he in client list (it will be an error)
            if (Users.ContainsValue(tcpEP))
            {
                string realName = Users.GetKey(tcpEP);
                string cause = "Connection: user " + name + " already connected as " + realName;

                SendRequest(tcpEP, new ResponseConnect(false, cause).GetBytes());
                Msg(cause);
                return;
            }

            //  and has unique name (answer Cancel)
            if (Users.ContainsKey(name))
            {
                string cause = "Connection: username " + name + " is not available";
                SendRequest(tcpEP, new ResponseConnect(false, cause).GetBytes());
                Msg(cause);
                return;
            }

            SendRequest(tcpEP, new ResponseConnect(true, "OK").GetBytes());
            Users.Add(name, new UserInfo(tcpEP, udpEP.Port));
            Msg("User " + name + " [" + tcpEP.ToString() + "] added in user list");
            if (OnUserState != null) OnUserState(name, UserState.UserLogIn);
            SendBroadcast(new ResponseUser(name, UserState.UserLogIn));
        }


        /// <summary>
        /// User request some custom data, e.g. udp server port
        /// </summary>
        public void HRequestCustomData(TcpMessage Request)
        {
            CCommon.CustomData cData = (CCommon.CustomData)Request.State;
            if (!CustomData.ContainsKey(cData)) return;
            SendRequest(Users[Request.From].EndPoint,
                new RequestCustomData(Request.From, cData, CustomData[cData]).GetBytes());
        }

        /// <summary>
        /// User was gently logged off 
        /// </summary>
        public void HQuit(TcpMessage Request)
        {
            IPEndPoint ep = Users[Request.From].EndPoint;
            string deleted = Users.GetKey(ep);
            if (deleted == null) return;
            Users.Remove(deleted);
            SendBroadcast(new ResponseUser(deleted, UserState.UserLogOut));
            Msg("User " + deleted + " was logged off");
            if(OnUserState != null) OnUserState(Request.From, UserState.UserLogOut);
                // I'll try to send it, but he is offline...
            IPEndPoint sender = Users[Request.From].EndPoint;
            SendRequest(sender,
                new ResponseServerInfo("Quit OK").GetBytes());
        }

        /// <summary>
        /// User was requested a userlist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="endPoint"></param>
        public void HUserList(TcpMessage request)
        {
            IPEndPoint ep = Users[request.From].EndPoint;
            string usr = Users.GetKey(ep);
            Users[usr].Update();
            SendRequest(ep, new ResponseUserList(Users.Keys).GetBytes());
            Msg("User " + usr + " request a user list");
        }

        /// <summary>
        /// Handler for write message event
        /// </summary>
        public void HWriteFor(TcpMessage Request)
        {
            IPEndPoint ep = Users[Request.From].EndPoint;
            Users[Request.From].Update();
            // if receiver is not valid 
            if (!Users.ContainsKey(Request.To))
            {
                SendRequest(ep, new ResponseUser(Request.To, UserState.UserLogOut).GetBytes());
                Msg("Message resend: Receiver " + Request.To + " is not valid");
                return;
            }
            IPEndPoint receiver = Users[Request.To].EndPoint;
            string user = Users.GetKey(ep);
            SendRequest(receiver, 
                new RequestTextMessage(user, Request.To, Request.GetMessageString()).GetBytes());
            Msg("Message resend: from " + Request.From + " to " + Request.To);

            IPEndPoint sender = Users[Request.From].EndPoint;
            SendRequest(sender,
                new ResponseServerInfo("Message to " + Request.To + " OK").GetBytes());
        }

        #endregion

        #endregion

        /// <summary>
        /// Stops the timers and close current connection
        /// </summary>
        public new void Dispose()
        {
            userAfkTimer.Stop();
            userAfkTimer.Dispose();
            base.Dispose();
        }
            
    }




    public class UserInfo
    {
        public IPEndPoint EndPoint;
        public IPEndPoint UdpEndPoint;
        public TimeSpan LastResp;


        public UserInfo(IPEndPoint ep, int UdpPort)
        {
            EndPoint = ep;
            UdpEndPoint = new IPEndPoint(ep.Address, UdpPort);
            LastResp = DateTime.Now.TimeOfDay;
        }

        public void Update()
        {
            LastResp = DateTime.Now.TimeOfDay;
        }
    }

    /// <summary>
    /// Dictionary(string, UserName) extension
    /// </summary>
    public static class DictionaryExtension
    {
        public static string GetKey(this Dictionary<string, UserInfo> Dict, IPEndPoint value)
        {
            var keyvalue = Dict.First(
                (m) => 
                    (m.Value.EndPoint.Address == value.Address) 
                        && (m.Value.EndPoint.Port == value.Port)
                );
            return keyvalue.Key;
        }

        public static bool ContainsValue(this Dictionary<string, UserInfo> Dict, IPEndPoint value)
        {
            foreach (UserInfo val in Dict.Values)
            {
                if (val.EndPoint.Address == value.Address &&
                    val.EndPoint.Port == value.Port) return true;
            }
            return false;
        }   
    }
}
