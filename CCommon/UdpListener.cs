using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Open.Nat;

namespace CCommon
{
    public class UdpListener
    {
        UdpClient udpClient = new UdpClient();

        public bool IsBound { get; set; }

        static int CheckInterruptTimeout = 1000;

        public bool Interrupt { get; set; }

        public delegate void OnConnectEventHandler(IPEndPoint sender, byte[] data);
        public event OnConnectEventHandler OnConnect;

        public delegate void OnCloseEventHandler();
        public event OnCloseEventHandler OnClose;

        public delegate void OnLogEventHandler(string message);
        public event OnLogEventHandler OnLog;

        public IPEndPoint ServerEndPoint;

        public string Name { get; set; }

        public UdpListener()
        {
            Interrupt = false;
            IsBound = false;
        }

        public void StartListenAsync(int UdpPort)
        {
            IPAddress ip = Dns.GetHostEntry(Dns.GetHostName())
                              .AddressList
                              .Where(_ip => _ip.AddressFamily == AddressFamily.InterNetwork)
                              .First();
            try
            {
                Interrupt = false;
                udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, UdpPort));
                UdpPort = (udpClient.Client.LocalEndPoint as IPEndPoint).Port;
                ServerEndPoint = new IPEndPoint(ip, UdpPort);
                Thread thread = new Thread(_listen);
                thread.Start();
                IsBound = true;
                Msg(String.Format("Server start on [{0}]", (IPEndPoint)ServerEndPoint));
            }
            catch { Msg(String.Format("Cannot start on [:{0}]",UdpPort)); }
        }

        void _listen()
        {
            try
            {
                while (!Interrupt)
                {
                    IAsyncResult result = udpClient.BeginReceive(null, null);
                    while (!result.AsyncWaitHandle.WaitOne(CheckInterruptTimeout))
                    {
                        if (Interrupt)
                        {
                            udpClient.Close();
                            return;
                        }
                    }
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any, ServerEndPoint.Port);
                    byte[] data = udpClient.EndReceive(result, ref ep);
                    if (OnConnect != null) OnConnect(ep, data);
                }
            }
            catch (Exception ex)
            {
                Msg("Error on listening:" + ex.Message);
            }
            finally
            {
                Msg("Server closed");
                if (OnClose != null) OnClose();
            }
        }

        public void StopListenAsync()
        {
            Interrupt = true;
        }

        public async void Send(IPEndPoint RemoteEndPoint, byte[] Data)
        {
            var result = await udpClient.SendAsync(Data, Data.Length, RemoteEndPoint);
        }

        public void StopListenAsync(int Timeout)
        {
            Task.Factory.StartNew(() => Thread.Sleep(Timeout)).ContinueWith((task) => StopListenAsync());
        }

        public void Msg(string Text)
        {
            if (OnLog != null) OnLog(Name + ": " + Text);
            else Console.WriteLine(Name + ": " + Text);
        }
    }
}
