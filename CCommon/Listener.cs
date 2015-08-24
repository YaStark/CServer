using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Open.Nat;

namespace CCommon
{
    public class Listener : Socket, IDisposable
    {
        #region Events and their delegats

        public delegate void OnCloseEventHandler();
        /// <summary>
        /// Event to occur server's closing
        /// </summary>
        public event OnCloseEventHandler OnClose;

        public delegate void OnLogEventHandler(string Message);
        /// <summary>
        /// Get the log information
        /// </summary>
        public event OnLogEventHandler OnLog;

        public delegate void OnConnectEventHandler(IPEndPoint client, byte[] data);
        /// <summary>
        /// Get data from connection event
        /// This event must disconnect and close socket after using
        /// </summary>
        public event OnConnectEventHandler OnConnect;

        #endregion

        public string Name = "Listener";

        /// <summary>
        /// Timeout for check if server was interrupted
        /// </summary>
        public static int AcceptCheckMs = 1000;

        /// <summary>
        /// Server interrupting state
        /// </summary>
        public bool Interrupt { get; set; }
        
        /// <summary>
        /// Delegate for Socket.Accept() method
        /// </summary>
        /// <returns></returns>
        delegate Socket AcceptDelegate();

        private bool IsDisposed = false;

        public IPEndPoint ServerEndPoint { get; set; }

        /// <summary>
        /// Default socket with IPv4, Stream and Tcp
        /// </summary>
        public Listener()
            : base(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp) { }

        /// <summary>
        /// Socket with custom protocol type
        /// </summary>
        public Listener(ProtocolType protocol, SocketType socketType)
            : base(AddressFamily.InterNetwork, socketType, protocol) { }

        /// <summary>
        /// Bind socket on port and begin listen any input connections
        /// </summary>
        public void StartListenAsync(int Port)
        {
            IPAddress[] ip = Dns.GetHostEntry(Dns.GetHostName()).
                AddressList.Where((addr)=>addr.AddressFamily == AddressFamily).
                ToArray();
            if (ip.Length > 0) StartListenAsync(ip[0], Port);
            else throw new Exception("AddressFamily as in this socket is unaviable.");
        }

        /// <summary>
        /// Bind socket on port and begin listen any input connections
        /// </summary>
        public void StartListenAsync(IPAddress ip, int Port)
        {
            try
            {
                Bind(new IPEndPoint(IPAddress.Any, Port));
                Port = (LocalEndPoint as IPEndPoint).Port;
                ServerEndPoint = new IPEndPoint(ip, Port);
                Listen(100);
                Msg(String.Format("Server was running on {0} : {1}", ip, Port));
                    // Become a listen 
                Thread thread = new Thread(new ThreadStart(_listen));
                thread.Start();
            }
            catch (Exception e)
            {
                Msg("Exception on start: " + e.Message);
            }
        }

        /// <summary>
        /// Stop the server async.
        /// </summary>
        public void StopListenAsync()
        {
            Interrupt = true;
            Msg("Query to stop...");
        }

        /// <summary>
        /// Method for handling connection from async call
        /// </summary>
        /// <param name="result">Some stuff</param>
        private void AsyncRes(IAsyncResult result)
        {
            Socket client = null;
            try
            {
                AsyncResult res = (AsyncResult)result;
                AcceptDelegate caller = (AcceptDelegate)res.AsyncDelegate;

                    // Get client' socket
                client = caller.EndInvoke(res);

                    // Get data for QueryEvent
                byte[] data = new byte[ReceiveBufferSize];
                int size = client.Receive(data);
                IPEndPoint clientEP = client.RemoteEndPoint as IPEndPoint;

                // Msg(String.Format("Received {0} byte(s) from ip: {1}.", size, clientEP.Address));

                if (OnConnect != null)
                {
                    OnConnect(clientEP, data.Take(size).ToArray());
                }
                else
                {
                    client.Disconnect(false);
                    client.Close();
                }
            }
            catch (Exception)
            {
                Msg("Server was closed");
                if (OnClose != null) OnClose();
                return;
            }
        }

        /// <summary>
        /// Main async accept method
        /// </summary>
        private void _listen()
        {
            while (!Interrupt)
            {
                    // Begin listening
                AcceptDelegate acceptDelegate = new AcceptDelegate(Accept);
                IAsyncResult result = acceptDelegate.BeginInvoke(AsyncRes, null);

                while (!result.AsyncWaitHandle.WaitOne(AcceptCheckMs))  // Keep alive and check to broke
                {
                    if (Interrupt)                         // In case of interrupt
                    {
                        if (Connected) Disconnect(false);
                        Close();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Sending request to a custom IPEndPoint
        /// </summary>
        /// <param name="Receiver"></param>
        /// <param name="Answer"></param>
        public void SendRequest(IPEndPoint Receiver, byte[] Answer)
        {
            Request request = new Request();
            request.ConnectStateEvent += (connected) =>
            {
                if (!connected)
                {
                    Msg("Cannot connect with [" + Receiver.ToString() + "]");
                    request.Dispose();
                    return;
                }
                request.SendStateEvent += (bytesSended) =>
                {
                    request.Dispose();
                };
                request.TrySend(Answer);
            };
            request.TryConnectAsync(Receiver);
        }

        /// <summary>
        /// Send single request
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="answer"></param>
        public void SendAnswer(Socket Receiver, TcpMessage Request)
        {
            this.TrySend(
                Request.GetBytes(),
                (bytesSended) =>
                {
                    Receiver.Disconnect(false);
                    Receiver.Close();
                });
        }

        /// <summary>
        /// Close the connection and base dispose call
        /// </summary>
        public new void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            if (Connected) Disconnect(false);
            Close();
            base.Dispose();
        }

        /// <summary>
        /// Information manager wrapper
        /// </summary>
        /// <param name="Text">Data</param>
        protected virtual void Msg(string Text)
        {
            if (OnLog == null) Console.WriteLine(Name + ": " + Text);
            else OnLog(Name + ": " + Text);
        }
    }
}
