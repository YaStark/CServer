using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCommon;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;

namespace CClient
{
    class ClientListener : Listener
    {
        #region Events and their delegates

        public delegate void OnServerChangeStateHandler(bool State, string Message);
        /// <summary>
        /// Raises when server change his state
        /// </summary>
        public event OnServerChangeStateHandler OnServerChangeState;

        public delegate void OnConnectStateEventHandler(bool Connected, string Message);
        /// <summary>
        /// Connect state event returns a connection state and message about it
        /// </summary>
        public event OnConnectStateEventHandler OnConnectState;

        public delegate void OnChatEventHandler(string Username, string Text);
        /// <summary>
        /// Chat message event handler
        /// </summary>
        public event OnChatEventHandler OnChat;

        public delegate void OnUserEventHandler(UserState State, string Username);
        /// <summary>
        /// User off message event. Message is a username
        /// </summary>
        public event OnUserEventHandler OnUser;

        public delegate void OnUserListEventHandler(string[] Users);
        /// <summary>
        /// Receive a user list as string array
        /// </summary>
        public event OnUserListEventHandler OnUserList;

        #endregion

        public delegate void OnTalkOfferEventHandler(string From);
        public event OnTalkOfferEventHandler OnTalkOffer;

        public delegate void OnTalkStateEventHandler(string From, TalkState TalkState);
        public event OnTalkStateEventHandler OnTalkState;

        public delegate void OnTalkEndEventHandler(string From);
        public event OnTalkEndEventHandler OnTalkEnd;

        public delegate void OnCustomDataEventHandler(CustomData customData, string Data);
        public event OnCustomDataEventHandler OnCustomData;

        bool TryConnected = false;
        
        /// <summary>
        /// Connection timeout in ms
        /// </summary>
        public int TryConnectionTimeout = 10000;

        public ClientListener()
        {
            Name = "Client";
            OnConnect += ClientListener_OnConnect;
        }

        void ClientListener_OnConnect(IPEndPoint Client, byte[] Data)
        {
            try
            {
                TcpMessage header = new TcpMessage(Data);
                Decrypt(header);
            }
            catch (Exception e)
            {
                Msg("Cannot read data: " + e.Message);
            }
        }

        /// <summary>
        /// Decrypting a request and create right responses
        /// </summary>
        private void Decrypt(TcpMessage Request)
        {
            switch ((CodeMessage)Request.Code)
            {
                case CodeMessage.AConnect:
                    HConnect(Request);
                    break;

                case CodeMessage.AUser:
                    HUser(Request);
                    break;

                case CodeMessage.AUserList:
                    HUserList(Request);
                    break;

                case CodeMessage.CWriteFor:
                    HWriteFor(Request);
                    break;

                case CodeMessage.AServerState:
                    HServerState(Request);
                    break;

                case CodeMessage.CATalkOffer:
                    HTalkOffer(Request);
                    break;

                case CodeMessage.CATalkAnswer:
                    HTalkState(Request);
                    break;

                case CodeMessage.CARequestCustomData:
                    HRequestCustomData(Request);
                    break;

                case CodeMessage.CTalkEnd:
                    HTalkEnd(Request);
                    break;

                case CodeMessage.AServerInfo:
                    HServerInfo(Request);
                    break;

                default:
                    Msg("Unrecognized/wrong data");
                    break;
            }
        }

        #region Responses from server

        void HTalkEnd(TcpMessage Request)
        {
            if (OnTalkEnd != null) OnTalkEnd(Request.From);
            Msg(Request.From + " is hang up");
        }

        /// <summary>
        /// Remote companion answer to talk offer
        /// </summary>
        void HTalkState(TcpMessage request)
        {
            int State = request.State;
            string translate = ResponseTalkState.TalkStateString((TalkState)State);
            Msg(String.Format("Talk status with {0} is {1}", request.From, translate));
            if (OnTalkState != null) OnTalkState(request.From, (TalkState)State);
        }

        void HTalkOffer(TcpMessage request)
        {
            Msg(request.From + " offer to talk");
            if (OnTalkOffer != null) OnTalkOffer(request.From);
        }

        void HServerInfo(TcpMessage Request)
        {
            Msg("Server: " + Request.GetMessageString());
        }

        void HServerState(TcpMessage Request)
        {
            if (Request.State == 0) Msg("Server is closing (" + Request.GetMessageString() + ")");
            else Msg("Server is on (" + Request.GetMessageString() + ")");
            OnServerChangeState(Request.State != 0, Request.GetMessageString());
        }

        /// <summary>
        /// Connection state
        /// </summary>
        /// <param name="request"></param>
        void HConnect(TcpMessage Request)
        {
            TryConnected = true;
            if (Request.State != 0)
            {
                Msg("Connect state OK");
                if (OnConnectState != null) OnConnectState(true, "");
            }
            else
            {
                string cause = Request.GetMessageString();
                Msg("Connect state failed because " + cause);
                if (OnConnectState != null) OnConnectState(false, cause);
            }
        }

        /// <summary>
        /// Some user change his state
        /// </summary>
        /// <param name="request"></param>
        void HUser(TcpMessage Request)
        {
            string translate = ResponseUser.UserStateString((UserState)Request.State);
            Msg("User " + Request.From + " has " + translate + " state now");
            if (OnUser != null) OnUser((UserState)Request.State, Request.From);
        }

        /// <summary>
        /// Get the user list from server
        /// </summary>
        /// <param name="request"></param>
        void HUserList(TcpMessage Request)
        {
            Msg("Get users list");
            if (OnUserList != null) OnUserList(ResponseUserList.GetUsers(Request));
        }

        /// <summary>
        /// Text message event
        /// </summary>
        /// <param name="request"></param>
        void HWriteFor(TcpMessage Request)
        {
            Msg("Get message from " + Request.From);
            if (OnChat != null) OnChat(Request.From, Request.GetMessageString());
        }
        
        #endregion

        #region Outer requests

        /// <summary>
        /// Send request to end current call
        /// </summary>
        public void RequestEndTalk(IPEndPoint Server, string Sender, string Receiver)
        {
            Msg("Talk with " + Receiver + " is exhausted");
            RequestTalkEnd request = new RequestTalkEnd(Sender, Receiver);
            SendRequest(Server, request.GetBytes());
        }

        /// <summary>
        /// Send request to begin new call
        /// </summary>
        public void RequestTalk(IPEndPoint Server, string Sender, string Receiver)
        {
            Msg("Send talk request to " + Receiver);
            RequestTalkOffer request = new RequestTalkOffer(Sender, Receiver);
            SendRequest(Server, request.GetBytes());
        }

        /// <summary>
        /// Send answer talk state
        /// </summary>
        public void RequestTalkState(IPEndPoint Server, string Sender, string Receiver, TalkState state)
        {
            Msg("Request talk is " + ResponseTalkState.TalkStateString(state));
            ResponseTalkState resp = new ResponseTalkState(Sender, Receiver, state);
            SendRequest(Server, resp.GetBytes());
        }

        /// <summary>
        /// Send login request to server
        /// </summary>
        public void RequestConnect(IPEndPoint Server, int UdpPort, string Nickname)
        {
            Msg("Try to login as " + Nickname);
            TryConnected = false;
            Thread thread = new Thread(() =>
            {
                Thread.Sleep(TryConnectionTimeout);
                if (!TryConnected)
                {
                    string msg = "Server [" + Server.ToString() + "] is unavailable: timeout";
                    OnConnectState(false, msg);
                    Msg(msg);
                }
            });
            SendRequest(Server,
                new RequestConnect(Nickname, ServerEndPoint, UdpPort).GetBytes());
            thread.Start();
        }

        /// <summary>
        /// Send quit message
        /// </summary>
        public void RequestQuit(IPEndPoint Server, string Nickname)
        {
            RequestQuit req = new RequestQuit(Nickname);
            Msg("Logout");
            SendRequest(Server, req.GetBytes());
        }

        /// <summary>
        /// Attempt to get user list
        /// </summary>
        public void RequestUserList(string Name, IPEndPoint Server)
        {
            RequestUserList req = new RequestUserList(Name);
            Msg("Attempt to get user list");
            SendRequest(Server, req.GetBytes());
        }

        /// <summary>
        /// Send text message to user
        /// </summary>
        public void RequestWrite(IPEndPoint Server, string From, string To, string Message)
        {
            RequestTextMessage text = new RequestTextMessage(From, To, Message);
            SendRequest(Server, text.GetBytes());
        }

        /// <summary>
        /// Server send some custom data
        /// </summary>
        public void HRequestCustomData(TcpMessage request)
        {
            Msg("Receive custom data");
            if (OnCustomData != null) OnCustomData((CustomData)request.State, request.GetMessageString());
        }

        #endregion

    }
}
