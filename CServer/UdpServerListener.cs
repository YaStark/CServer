using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCommon;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace CServer
{
    public class UdpServerListener : UdpListener
    {
        public Dictionary<IPEndPoint, IPEndPoint> DatagramGraph = new Dictionary<IPEndPoint, IPEndPoint>();
        
        public UdpServerListener() : base() 
        {
            OnConnect += _connect; 
            Name = "UdpServer";
        }

        public bool AddConnect(IPEndPoint User1, IPEndPoint User2)
        {
            lock (DatagramGraph)
            {
                if (DatagramGraph.ContainsKey(User1) || DatagramGraph.ContainsKey(User2))
                {
                    Msg("Connection fail, already connected");
                    return false;
                }
                DatagramGraph.Add(User1, User2);
                DatagramGraph.Add(User2, User1);
            }
            Msg(String.Format("Begin connection between [{0}] and [{1}]", User1, User2 ));
            return true;
        }

        public void RemoveConnect(IPEndPoint User)
        {
            IPEndPoint User2 = DatagramGraph[User];
            lock (DatagramGraph)
            {
                DatagramGraph.Remove(User);
                DatagramGraph.Remove(User2);
            }
            Msg(String.Format("Close connection between [{0}] and [{1}]", User, User2));
        }

        void _connect(IPEndPoint client, byte[] data)
        {
            IPEndPoint ep = null;
            lock (DatagramGraph) if (DatagramGraph.ContainsKey(client)) ep = DatagramGraph[client];
            if (ep != null)
            {
                Send(ep, data);
            }
        }
    }
}
