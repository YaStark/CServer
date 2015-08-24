using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CCommon
{
    public enum CodeMessage : int
    {
        /// <summary>
        /// Client logging in
        /// </summary>
        CConnect,

        /// <summary>
        /// Client send text message to other user
        /// </summary>
        CWriteFor,

        /// <summary>
        /// Client request userlist
        /// </summary>
        CUserList,

        /// <summary>
        /// Client logout
        /// </summary>
        CQuit,

        /// <summary>
        /// Client offer talk
        /// </summary>
        CATalkOffer,

        /// <summary>
        /// Client end current talk
        /// </summary>
        CTalkEnd,

        /// <summary>
        /// Client request some custom data or answer on this request
        /// </summary>
        CARequestCustomData,

        /// <summary>
        /// Client accept talk offer
        /// </summary>
        CATalkAnswer,

        /// <summary>
        /// Server inform that some user change his state
        /// </summary>
        AUser,

        /// <summary>
        /// Server answer on login request
        /// </summary>
        AConnect,

        /// <summary>
        /// Server answer on userlist request
        /// </summary>
        AUserList,

        /// <summary>
        /// Server change his state (On/off)
        /// </summary>
        AServerState,

        /// <summary>
        /// Server information
        /// </summary>
        AServerInfo
    }

    public enum UserState : int
    {
        UserLogIn, UserLogOut
    }

    public enum TalkState : int
    {
        Adopt, Busy, Reject
    }

    public enum CustomData : int
    {
        RequestUdpPort
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Serialized : System.Attribute { }

    /// <summary>
    /// General requests class
    /// </summary>
    public class TcpMessage 
    {
        /* To add a field that should be serialized, just add [Serializable] attribute to it
         *  Name of this field will be transform into text and added into message header
         *  This attributes only can be string or int types
         * */

        [Serialized]
        public int Code { get; set; }

        [Serialized]
        public string From { get; set; }

        [Serialized]
        public string To { get; set; }

        [Serialized]
        public int State { get; set; }
        
        public byte[] Msg { get; set; }
        
        public Encoding Encoding = Encoding.UTF8;

        public TcpMessage() { }

        /// <summary>
        /// Create object as bytes from socket
        /// </summary>
        public TcpMessage(byte[] Data) : this()
        {
            Deserialize(Data);
        }

        public byte[] GetBytes()
        {
            return Serialize();
        }

        public virtual string GetMessageString()
        {
            return Encoding.GetString(Msg);
        }

        #region Serialize

        /// <summary>
        /// Get fields list marked by [Serializable] attribute
        /// </summary>
        PropertyInfo[] GetSerializedProperties()
        {
            List<PropertyInfo> sFields = new List<PropertyInfo>();
            PropertyInfo[] props = this.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.GetCustomAttribute(typeof(Serialized)) != null)
                    sFields.Add(prop);
            }
            return sFields.ToArray();
        }

        /// <summary>
        /// Create RequestHeader from raw bytes
        /// </summary>
        /// <param name="InputData"></param>
        /// <returns></returns>
        void Deserialize(byte[] InputData)
        {
                // Get the msg bytes and split text header and byte data
            string sHead = Encoding.GetString(InputData);
            Msg = GetMsgValue(sHead, InputData);
                // offset is length of text header
            if (Msg != null)
            {
                int offset = InputData.Length - Msg.Length;
                sHead = Encoding.GetString(InputData, 0, offset);
            }

            PropertyInfo[] fields = GetSerializedProperties();
            foreach (PropertyInfo field in fields)
            {
                Type fieldType = field.PropertyType;
                string value = GetHeaderValue(sHead, field.Name);
                if(fieldType == typeof(string))
                        field.SetValue(this, value);
                else if(fieldType == typeof(int))
                        field.SetValue(this, int.Parse(value));
            }
        }

        /// <summary>
        /// Convert RequestHeaer into the raw bytes 
        /// </summary>
        /// <param name="Header"></param>
        /// <returns></returns>
        byte[] Serialize()
        {
            string header = "";
            PropertyInfo[] fields = GetSerializedProperties();

            foreach (PropertyInfo field in fields)
            {
                object value = field.GetValue(this);
                header += String.Format("{0}:{1};",
                    field.Name, value == null ? "" : value.ToString());
            }
            header += "Msg:";
            
            byte[] headerData = Encoding.GetBytes(header);

            if (Msg == null) return headerData;
            else return headerData.Concat(Msg).ToArray();
        }

        /// <summary>
        /// Get one of the headers
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Field"></param>
        /// <returns></returns>
        string GetHeaderValue(string Data, string Field)
        {
            return Regex.Match(Data, Field + @":[\s]*([^\;]*)[\s]*").Groups[1].Value;
        }

        /// <summary>
        /// Get byte array that represent msg as raw bytes, from "Msg:" to end
        /// </summary>
        /// <param name="StrData"></param>
        /// <param name="ByteData"></param>
        /// <param name="Encoding"></param>
        /// <returns></returns>
        byte[] GetMsgValue(string StrData, byte[] ByteData)
        {
            string pre = Regex.Match(StrData, @"([^\!]+?Msg:)").Groups[0].Value;
            int offset = Encoding.GetByteCount(pre);

            if (offset == ByteData.Length) return null;
            else if (offset > ByteData.Length)
            {
                throw new ArgumentException("ByteData argument is not valid - array out of bounds", "ByteData");
            }
            else return ByteData.Skip(offset).ToArray();
        }

        #endregion
    }

    /* *  Requests require some respondes on query  * */
    #region Requests

    /// <summary>
    /// Requesting some data which can be external for server.
    /// Amswer - self with filled "Msg" field
    /// </summary>
    public class RequestCustomData : TcpMessage
    {
        public RequestCustomData(string Username, CustomData dataType, string Data = "")
        {
            Code = (int)CodeMessage.CARequestCustomData;
            From = Username;
            State = (int)CustomData.RequestUdpPort;
            if (Data != "") Msg = Encoding.GetBytes(Data);
        }
    }

    /// <summary>
    /// Request to talk. Answer - ResponseTalkState or ResponseUser
    /// </summary>
    public class RequestTalkOffer : TcpMessage
    {
        public RequestTalkOffer(string Sender, string Receiver) : base()
        {
            Code = (int)CodeMessage.CATalkOffer;
            From = Sender;
            To = Receiver;
        }
    }

    /// <summary>
    /// Client logging in. Answer - ResponseConnect
    /// </summary>
    public class RequestConnect : TcpMessage
    {
        public RequestConnect(string Username, IPEndPoint LocalEndPoint, int UdpPort)
        {
            if (!MatchName(Username))
            {
                throw new ArgumentException("Username is not valid", "Username");
            }
            Code = (int)CodeMessage.CConnect;
            From = Username;
            Msg = Encoding.GetBytes(
                String.Format("[{0}]:{1} {2}", 
                    LocalEndPoint.Address.ToString(), 
                    LocalEndPoint.Port, 
                    UdpPort));                               
        }

        public static IPEndPoint GetUdpRemotePoint(TcpMessage Message)
        {
            try
            {
                string msg = Message.GetMessageString();
                GroupCollection gs = Regex.Match(msg, @"\[([^\[\]]*)\]:[0-9]+[\s]([0-9]+)").Groups;
                string ip = gs[1].Value;
                string port = gs[2].Value;
                return new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port));
            }
            catch
            { return null; } 
        }

        public static IPEndPoint GetRemotePoint(TcpMessage Message)
        {
            try
            {
                string msg = Message.GetMessageString();
                GroupCollection gs = Regex.Match(msg, @"\[([^\[\]]*)\]:([0-9]+)").Groups;
                string ip = gs[1].Value;
                string port = gs[2].Value;
                return new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port));
            }
            catch
            { return null; } 
        }

        /// <summary>
        /// Name can contain letters A-z, А-я, ё, Ё, digits, symbols '_' and
        /// have length from 3 to 20 chars
        /// </summary>
        public static bool MatchName(string Name)
        {
            return Regex.IsMatch(Name, @"^[A-zА-яёЁ0-9_^!]{3,20}$");
        }
    }

    /// <summary>
    /// Client request userlist. Answer - ResponseUserList
    /// </summary>
    public class RequestUserList : TcpMessage
    {
        public RequestUserList(string Username)
            : base()
        {
            Code = (int)CodeMessage.CUserList;
            From = Username;
        }
    }

    #endregion

    /* *  Answers don't need in response, just send  * */
    #region Answers

    /// <summary>
    /// Server send some information
    /// </summary>
    public class ResponseServerInfo : TcpMessage
    {
        public ResponseServerInfo(string Message)
            : base()
        {
            Code = (int)CodeMessage.AServerInfo;
            Msg = Encoding.GetBytes(Message);
        }
    }

    /// <summary>
    /// Client send text message to other user
    /// </summary>
    public class RequestTextMessage : TcpMessage
    {
        public RequestTextMessage(string SenderName, string CompanionName, string Message)
            : base()
        {
            Code = (int)CodeMessage.CWriteFor;
            From = SenderName;
            To = CompanionName;
            Msg = Encoding.GetBytes(Message);
        }
    }

    /// <summary>
    /// Client logout
    /// </summary>
    public class RequestQuit : TcpMessage
    {
        public RequestQuit(string Username)
            : base()
        {
            Code = (int)CodeMessage.CQuit;
            From = Username;
        }
    }

    /// <summary>
    /// Report to server about end current talk
    /// </summary>
    public class RequestTalkEnd : TcpMessage
    {
        public RequestTalkEnd(string Sender, string Receiver)
            : base()
        {
            Code = (int)CodeMessage.CTalkEnd;
            From = Sender;
            To = Receiver;
        }
    }

    /// <summary>
    /// Answer on call offer
    /// </summary>
    public class ResponseTalkState : TcpMessage
    {
        public ResponseTalkState(string Sender, string Receiver, TalkState TalkState)
            : base()
        {
            Code = (int)CodeMessage.CATalkAnswer;
            From = Sender;
            To = Receiver;
            State = (int)TalkState;
        }

        public static string TalkStateString(TalkState State)
        {
            switch (State)
            {
                case TalkState.Adopt: return "addopted";
                case TalkState.Busy: return "busy";
                case TalkState.Reject: return "reject";
                default: return "unknown";
            }
        }
    }

    /// <summary>
    /// Server answer on login request
    /// </summary>
    public class ResponseConnect : TcpMessage
    {
        public ResponseConnect(bool Connected, string Cause)
            : base()
        {
            Code = (int)CodeMessage.AConnect;
            State = Connected ? 1 : 0;
            Msg = Encoding.GetBytes(Cause);
        }
    }

    /// <summary>
    /// Server answer on userlist request
    /// </summary>
    public class ResponseUserList : TcpMessage
    {
        public ResponseUserList(IEnumerable<string> Users)
            : base()
        {
            Code = (int)CodeMessage.AUserList;
            string outstring = "";
            foreach (string user in Users)
            {
                outstring += user + ";";
            }
            Msg = Encoding.GetBytes(outstring);
        }

        /// <summary>
        /// return username list
        /// </summary>
        public static string[] GetUsers(TcpMessage Answer)
        {
            if (Answer.Msg == null) return null;
            string usernames = Answer.Encoding.GetString(Answer.Msg);
            return usernames.Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }
    }

    /// <summary>
    /// Server inform that some user change his state
    /// </summary>
    public class ResponseUser : TcpMessage
    {
        public ResponseUser(string Username, UserState State)
            : base()
        {
            Code = (int)CodeMessage.AUser;
            From = Username;
            this.State = (int)State;
        }

        public static string UserStateString(UserState State)
        {
            switch (State)
            {
                case UserState.UserLogIn: return "login";
                case UserState.UserLogOut: return "logout";
                default: return "unknown";
            }
        }
    }

    /// <summary>
    /// Server change his state
    /// </summary>
    public class ResponseServerState : TcpMessage
    {
        public ResponseServerState(bool State, string Message)
            : base()
        {
            Code = (int)CodeMessage.AServerState;
            this.State = State ? 1 : 0;
            Msg = Encoding.GetBytes(Message);
        }
    }

    #endregion

}
