using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CCommon
{
    public class Request : Socket, IDisposable 
    {
        public static double TimeoutConnectionMs = 15000;

        private IPEndPoint remEndPoint;

        public delegate void OnLogEventHandler(string Message);
        public event OnLogEventHandler OnLog;

        public delegate void ConnectStateEventHandler(bool IsConnected);
        public event ConnectStateEventHandler ConnectStateEvent;

        public delegate void SendStateEventHandler(int BytesSended);
        public event SendStateEventHandler SendStateEvent;

        delegate void ConnectDelegate(EndPoint RemoteEndPoint);

        #region Constructors
        /// <summary>
        /// Default: IPv4, Stream, Tcp socket
        /// </summary>
        public Request()
            : base(AddressFamily.InterNetwork,
                   SocketType.Stream,
                   ProtocolType.Tcp)
        {

        }

        /// <summary>
        /// Socket parameters set as input socket
        /// </summary>
        /// <param name="SocketRequest"></param>
        public Request(Socket SocketRequest)
            : base(SocketRequest.AddressFamily,
                SocketRequest.SocketType,
                SocketRequest.ProtocolType)
        { }

        #endregion

        #region Connect
        /// <summary>
        /// Async connect, result callback is ConnectStateEvent
        /// </summary>
        public void TryConnectAsync()
        {
            Thread th = new Thread(_conn);
            th.Start();
        }

        /// <summary>
        /// Connection function
        /// </summary>
        private void _conn()
        {
            IAsyncResult res = BeginConnect(remEndPoint, null, null);
            
            bool IsConnected = true;
            if (!res.AsyncWaitHandle.WaitOne((int)TimeoutConnectionMs))
            {
                // Timeout
                IsConnected = false;
            }
            res.AsyncWaitHandle.Dispose();
            if (ConnectStateEvent != null) ConnectStateEvent(IsConnected);
        }

        /// <summary>
        /// Async connect, result callback is ConnectStateEvent
        /// </summary>
        public void TryConnectAsync(IPEndPoint RemoteEndPoint)
        {
            remEndPoint = new IPEndPoint(RemoteEndPoint.Address, RemoteEndPoint.Port);
            TryConnectAsync();
        }

        #endregion

        #region Send

        /// <summary>
        /// Send data async
        /// </summary>
        /// <param name="Data"></param>
        public void TrySend(byte[] Data)
        {
            if (remEndPoint == null)
            {
                Msg("Try send: Server IPEndPoint is null");
                return;
            }
            if (!Connected)
            {
                Msg("Try send: Client doesn't connect with server");
                return;
            }
            Thread th = new Thread(_send);
            th.Start(Data);
        }

        private void _send(object data)
        {
            byte[] Data = data as byte[];
            int numBytes = Send(Data);
            if (SendStateEvent != null) SendStateEvent(numBytes);
        }

        #endregion

        public new void Dispose()
        {
            if (Connected) Disconnect(false);
            base.Dispose();
        }

        void Msg(string Text)
        {
            if (OnLog != null) OnLog(Text);
            else Console.WriteLine(Text);
        }
    }

    /// <summary>
    /// Extends the Socket class
    /// </summary>
    public static class SocketExt
    {

        /// <summary>
        /// Try send Data on opened socket, call AfterSend after it
        /// </summary>
        /// <param name="Socket"></param>
        /// <param name="Data"></param>
        /// <param name="AfterSend"></param>
        public static void TrySend(
            this Socket Socket, 
            byte[] Data, 
            Request.SendStateEventHandler AfterSend)
        {
            Thread th = new Thread((obj)=>
            {
                byte[] data = obj as byte[];
                int numBytes = Socket.Send(data);
                if (AfterSend != null) AfterSend(numBytes);
            });
            th.Start(Data);
        }
    }
}
