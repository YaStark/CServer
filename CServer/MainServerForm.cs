using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCommon;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Diagnostics;

namespace CServer
{
    public partial class MainServerForm : Form
    {
        ServerListener ChatServer = new ServerListener();
        UdpServerListener UdpChatServer = new UdpServerListener();

        int DefaultPortMsg = 6721;
        int DefaultPortVoice = 6722;

        public IPEndPoint Server;
        public IPEndPoint ServerVoice;
        public Options Options = new Options();

        public string EventLogName = "ChatServer App";
        
        public MainServerForm()
        {
            InitializeComponent();

            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            try
            {
                IPAddress ip = ips.Where(_ip => _ip.AddressFamily == AddressFamily.InterNetwork).First();
                labelServerIP.Text = ip.ToString();
            }
            catch
            {
                string msg = "Error: cannot get ipv4 address. Stop.";
                var res = MessageBox.Show(msg);
                EvtLog(msg, EventLogEntryType.Error);
                Application.Exit();
            }
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "Text file|*.txt";

            buttonSettingsDefault_Click(null, null);
            buttonSettingsApply_Click(null, null);
            settingsTextChanged(null, null);

            ChatServerInit();
        }

        ~MainServerForm()
        {
            ChatServer.Dispose();
        }

        void ChatServerInit()
        {
            lock (UdpChatServer)
            {
                UdpChatServer.OnLog += Log;
            }
            lock(ChatServer)
            {
                ChatServer.Users.Clear();
                ChatServer.OnLog += Log;
                ChatServer.OnUserState += ChatServer_OnUserState;
                ChatServer.OnTalkOccur += ChatServer_OnTalkOccur;
                ChatServer.OnClose += () =>
                {
                    EvtLog("Server was closed");
                    ChatServer = new ServerListener(); 
                    ChatServerInit();
                };
            }
        }

        void ChatServer_OnTalkOccur(bool Enabled, string UserA, string UserB)
        {
            if (Enabled)
            {
                UdpChatServer.AddConnect(ChatServer.Users[UserA].UdpEndPoint,
                                         ChatServer.Users[UserB].UdpEndPoint);
            }
            else
            {
                UdpChatServer.RemoveConnect(ChatServer.Users[UserA].UdpEndPoint);
            }
        }


        void ChatServer_OnUserState(string Username, UserState State)
        {
            if (State == UserState.UserLogOut)
            {
                IPEndPoint UserA = ChatServer.Users[Username].UdpEndPoint;
                if (UdpChatServer.DatagramGraph.ContainsKey(UserA))
                {
                    IPEndPoint UserB = UdpChatServer.DatagramGraph[UserA];
                    UdpChatServer.DatagramGraph.Remove(UserA);
                    UdpChatServer.DatagramGraph.Remove(UserB);
                }
            }
            listBoxUsers.Invoke(new Action(
                () =>
                {
                    listBoxUsers.Items.Clear();
                    listBoxUsers.Items.AddRange(ChatServer.Users.Keys.ToArray());
                }));
        }

        /// <summary>
        /// Create new record in windows event log
        /// </summary>
        private void EvtLog(string Text, EventLogEntryType Type = EventLogEntryType.Information)
        {
            string LogApp = "Application";
            if (!EventLog.SourceExists(EventLogName)) EventLog.CreateEventSource(EventLogName, LogApp);
            EventLog.WriteEntry(EventLogName, Text, Type);
        }

        /// <summary>
        /// Create new record in app log
        /// </summary>
        private void Log(string text)
        {
            try
            {
                textBoxLog.Invoke(new Action(() =>
                {
                    textBoxLog.AppendText(String.Format(
                                "[{0:T}] : {1} \r\n",
                                DateTime.Now,
                                text
                        ));
                }));
            }
            catch { }
        }

        private void buttonTSStart_Click(object sender, EventArgs e)
        {
            if (!ChatServer.IsBound)
            {
                if (Properties.Settings.Default.AfkUserState)
                {
                    ChatServer.StartCheckUsers(Properties.Settings.Default.AfkUserTimeoutMs);
                }
                ChatServer.StartListenAsync(Server.Address, Server.Port);
            }
            StartUdpServer();
            EvtLog(String.Format("Server was started on TCP:[{0}], UDP:[{1}]",
                ChatServer.ServerEndPoint, UdpChatServer.ServerEndPoint));
        }

        /// <summary>
        /// Bind udp socket and resave new port name
        /// </summary>
        void StartUdpServer()
        {
            if (!UdpChatServer.IsBound)
            {
                UdpChatServer.StartListenAsync(ServerVoice.Port);
                ChatServer.CustomData[CustomData.RequestUdpPort] = ServerVoice.Port.ToString();
            }

        }

        private void buttonTSStop_Click(object sender, EventArgs e)
        {
            ChatServer.StopListenAsync("Stopped", (int)Properties.Settings.Default.TimeoutServerOffMs);
            UdpChatServer.StopListenAsync();
        }

        private void buttonTSSettings_Click(object sender, EventArgs e)
        {
            groupBoxSettings.Visible = !groupBoxSettings.Visible;
        }

        private void buttonTSUInfo_Click(object sender, EventArgs e)
        {
            groupBoxUInfo.Visible = !groupBoxUInfo.Visible;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            textBoxMsgPort.Value = Server.Port;
            textBoxVoicePort.Value = ServerVoice.Port;
        }

        private void settingsTextChanged(object sender, EventArgs e)
        {
            try
            {
                buttonSettingsApply.Enabled =
                                    textBoxMsgPort.Value != Server.Port ||
                                    textBoxVoicePort.Value != ServerVoice.Port;
            }
            catch { }
        }

        private void buttonSettingsDefault_Click(object sender, EventArgs e)
        {
            textBoxMsgPort.Value = DefaultPortMsg;
            textBoxVoicePort.Value = DefaultPortVoice;
        }

        private void buttonSettingsApply_Click(object sender, EventArgs e)
        {
            if (sender != null &&
                    MessageBox.Show("Confirm apply settings (restart required)",
                                    "Apply new settings",
                                    MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                buttonReset_Click(null, null);
                return;
            }
            
            if (textBoxMsgPort.Value == textBoxVoicePort.Value)
            {
                MessageBox.Show("Port values cannot intersects");
                return;
            }
            Server = new IPEndPoint(
                IPAddress.Parse(labelServerIP.Text), (int)textBoxMsgPort.Value);
            ServerVoice = new IPEndPoint(
                IPAddress.Parse(labelServerIP.Text), (int)textBoxVoicePort.Value);
            if(sender != null) Reboot();
        }

        /// <summary>
        /// Stop handler for rebooting
        /// </summary>
        public void CloseHandler()
        {
            Thread.Sleep(1000);
            ChatServer.OnClose -= CloseHandler;
            ChatServer = new ServerListener();
            UdpChatServer = new UdpServerListener();
            ChatServerInit();
            ChatServer.StartListenAsync(Server.Address, Server.Port);
            StartUdpServer();
        }

        /// <summary>
        /// Gently rebooting server
        /// </summary>
        public void Reboot()
        {
            ChatServer.OnClose += CloseHandler;
            ChatServer.StopListenAsync("Reboot", (int)Properties.Settings.Default.TimeoutServerOffMs);
            UdpChatServer.StopListenAsync((int)Properties.Settings.Default.TimeoutServerOffMs);
        }

        private void MainServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChatServer.StopListenAsync();
            UdpChatServer.StopListenAsync();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxLog.Invoke(new Action(() => textBoxLog.Text = ""));
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.FileName != "" 
                    || saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                try
                {
                    File.WriteAllText(saveFileDialog1.FileName, textBoxLog.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                try
                {
                    File.WriteAllText(saveFileDialog1.FileName, textBoxLog.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void listBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string currentUser = (string)listBoxUsers.SelectedItem;
            try
            {
                textBoxNick.Text = currentUser;
                if (currentUser != "")
                {
                    lock (ChatServer.Users[currentUser])
                    {
                        textBoxSocket.Text = ChatServer.Users[currentUser].EndPoint.ToString();
                        textBoxLastResp.Text = ChatServer.Users[currentUser].LastResp.ToString();
                        textBoxUdp.Text = ChatServer.Users[currentUser].UdpEndPoint.ToString();
                    }
                }
                else
                {
                    textBoxSocket.Text = "";
                    textBoxLastResp.Text = "";
                }
            }
            catch { }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (Options.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Request.TimeoutConnectionMs = Properties.Settings.Default.TimeoutSendRequestMs;
                ChatServer.AfkUserState = Properties.Settings.Default.AfkUserState;
                ChatServer.CheckUsersSetTimeout(Properties.Settings.Default.AfkUserTimeoutMs);
            }
        }

        private void buttonUsersKick_Click(object sender, EventArgs e)
        {
            string currentUsr = listBoxUsers.SelectedItem as string;
            if(currentUsr == null) return;

            IPEndPoint udpEP = ChatServer.Users[currentUsr].UdpEndPoint;
            UdpChatServer.RemoveConnect(udpEP);
            ChatServer.RemoveClient(currentUsr);
            listBoxUsers.Items.Remove(currentUsr);
        }
    }
}
