using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCommon;

namespace CClient
{
    public partial class MainClientForm : Form
    {
        ClientListener ChatClient = new ClientListener();
        UdpClientListener UdpChatClient = new UdpClientListener();
        bool IsConnected = false;
        string Nickname = "Self";
        Dictionary<string, string> TextData = new Dictionary<string, string>();
        IPEndPoint Server { get; set; }
        IPEndPoint UdpServer { get; set; }

        string UpText = "Chat";
        int listBoxLastSelectedIndex = 0;

        public MainClientForm()
        {
            InitializeComponent();
            tabControl1.TabPages.Remove(tabPageChat);
            tabControl1.TabPages.Remove(tabPageCall);
            this.Text = UpText;

            ChatClient.OnCustomData += ChatClient_OnCustomData;
            ChatClient.OnLog += SetLogText;
            UdpChatClient.OnLog += SetLogText;
            ChatClient.OnUser += ChatClient_OnUser;
            ChatClient.OnUserList += ChatClient_OnUserList;
            ChatClient.OnChat += ChatClient_OnChat;
            ChatClient.OnConnectState += ChatClient_OnConnectState;
            ChatClient.OnServerChangeState += ChatClient_OnServerChangeState;

            CallIni();
        }

        /// <summary>
        /// Run action with control if required, catch ObjectDisposedException
        /// </summary>
        void InvokeIt(Control Control, Action act)
        {
            try
            {
                if (Control.InvokeRequired) Control.Invoke(act);
                else act();
            }
            catch(ObjectDisposedException) { /* Object disposed */ }
        }


        #region Common informational events

        /// <summary>
        /// Log message handler
        /// </summary>
        void SetLogText(string Text)
        {
            InvokeIt(textBoxLog, () =>
            {
                string msg = String.Format("[{0:T}]:{1}\r\n", DateTime.Now, Text);
                textBoxLog.AppendText(msg);
                toolStripLabelLog.Text = Text;
            });
        }

        /// <summary>
        /// Handle receiving custom data from server
        /// </summary>
        void ChatClient_OnCustomData(CCommon.CustomData customData, string Data)
        {
            switch (customData)
            {
                case CCommon.CustomData.RequestUdpPort:
                    try
                    {
                        int port = Int32.Parse(Data);
                        UdpServer = new IPEndPoint(Server.Address, port);
                    }
                    catch { }
                    break;

                // Add new cases of CustomData
            }
        }

        /// <summary>
        /// Handle server's stop
        /// </summary>
        void ChatClient_OnServerChangeState(bool State, string Message)
        {
            if (!State)
            {
                SetLogText(Message);
                Logout();
            }
        }

        #endregion


        #region Users events

        /// <summary>
        /// Event of receiving user list
        /// </summary>
        void ChatClient_OnUserList(string[] Users)
        {
            InvokeIt(buttonRefresh, () => buttonRefresh.Enabled = true);
            InvokeIt(listBoxUsers, () =>
            {
                listBoxUsers.Items.Clear();
                if (Users != null) listBoxUsers.Items.AddRange(Users);
            });
            UpdateChatField();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            if (Server != null)
            {
                ChatClient.RequestUserList(Nickname, Server);
                buttonRefresh.Enabled = false;
            }
        }

        private void listBoxUsers_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedItem == null)
            {
                if (listBoxUsers.Items.Count <= listBoxLastSelectedIndex)
                    listBoxLastSelectedIndex = 0;
                listBoxUsers.SelectedIndex = listBoxLastSelectedIndex;
            }
            else listBoxLastSelectedIndex = listBoxUsers.SelectedIndex;
            UpdateChatField();
        }

        #endregion


        #region Text chat events

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string text = textBoxInput.Text;
            textBoxInput.Text = "";
            textBoxInput.Focus();
            string currentKey = (string)listBoxUsers.SelectedItem;
            string msg = String.Format("[{0:T}]:[{1}] - {2}\r\n", DateTime.Now, Nickname, text);

            if (!IsConnected)
            {
                textBoxChat.AppendText(msg);
                return;
            }
            if (currentKey == null) return;
            ChatClient.RequestWrite(Server, Nickname, currentKey, text);

            if (TextData.ContainsKey(currentKey)) TextData[currentKey] += msg;
            else TextData.Add(currentKey, msg);

            UpdateChatField();
        }

        private void textBoxInput_Key(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
            {
                string t = textBoxInput.Text;
                textBoxInput.Text = t.Remove(t.Length - 1);
                buttonSend.PerformClick();
            }
        }

        /// <summary>
        /// Gather info about current talking user and show dialog
        /// </summary>
        void UpdateChatField()
        {
            InvokeIt(listBoxUsers, () =>
                {
                    string currentKey = listBoxUsers.SelectedItem as string;
                    if (currentKey == null) return;
                    if (!TextData.ContainsKey(currentKey)) TextData.Add(currentKey, "");
                    textBoxChat.Text = TextData[currentKey];
                });
        }

        /// <summary>
        /// Event of new incoming text message
        /// </summary>
        void ChatClient_OnChat(string Username, string Text)
        {
            string msg = "";
            lock (TextData)
            {
                // Add data to database
                if (!TextData.ContainsKey(Username)) TextData.Add(Username, "");
                msg = String.Format("[{0:T}]:[{1}] - {2}\r\n", DateTime.Now, Username, Text);
                TextData[Username] += msg;

            }
            InvokeIt(textBoxChat, () =>
            {
                textBoxChat.AppendText(msg);
                int index = listBoxUsers.Items.IndexOf(Username);
            });
        }

        /// <summary>
        /// Some user change his state
        /// </summary>
        void ChatClient_OnUser(CCommon.UserState State, string Username)
        {
            InvokeIt(listBoxUsers, () =>
            {
                switch (State)
                {
                    case CCommon.UserState.UserLogIn:
                        if (!listBoxUsers.Items.Contains(Username)) listBoxUsers.Items.Add(Username);
                        break;

                    case CCommon.UserState.UserLogOut:
                        if (listBoxUsers.Items.Contains(Username)) listBoxUsers.Items.Remove(Username);
                        if (Username == callPanel1.TalkWith) callPanel1.OuterReject();
                        break;
                }
            });
        }

        #endregion


        #region Autentification

        void ChatClient_OnConnectState(bool Connected, string Message)
        {
            ChatClient.SendRequest(Server,
                new RequestCustomData(Nickname, CustomData.RequestUdpPort).GetBytes());
            InvokeIt(labelLoginState, () =>
            {
                if (IsConnected != Connected)
                {
                    if (Connected)  // Try connect
                    {
                        Login();
                        buttonRefresh.PerformClick();
                    }
                    else
                    {               // Try disconnect
                        Logout();
                    }
                }
                panelLogin.Enabled = true;   // Try connect failed
                labelLoginState.Text = Message;
                IsConnected = Connected;
            });
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            if (IsConnected) Logout();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                IPEndPoint _server = new IPEndPoint(
                    IPAddress.Parse(textBoxIP.Text),
                    Int32.Parse(textBoxPort.Text));
                Server = _server;
            }
            catch
            {
                labelLoginState.Text = "Wrong pair IP and Port";
                return;
            }
            if (!CCommon.RequestConnect.MatchName(textBoxNickname.Text))
            {
                labelLoginState.Text = "Wrong name. Name can contains rus/eng letters, digits " +
                                       "and '_' sign, and length between 3-30 symbols";
                textBoxNickname.Focus();
                return;
            }
            Nickname = textBoxNickname.Text;
            labelLoginState.Text = "Connecting...";
            panelLogin.Enabled = false;
            SendLoginRequest();
        }

        /// <summary>
        /// Start servers, some wait to bind and send login request
        /// </summary>
        private void SendLoginRequest()
        {
            if (!ChatClient.IsBound)
            {
                try
                {
                    ChatClient.StartListenAsync(0);
                    UdpChatClient.StartListenAsync(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return;
                }
            }
            ChatClient.RequestConnect(Server, UdpChatClient.ServerEndPoint.Port, textBoxNickname.Text);
        }

        #endregion


        #region Form lifetime

        /// <summary>
        /// Set up the app view as logged in
        /// </summary>
        void Login()
        {
            InvokeIt(tabControl1, () =>
            {
                tabControl1.TabPages.Add(tabPageChat);
                tabControl1.TabPages.Remove(tabPageLogin);
                tabControl1.SelectedTab = tabPageChat;
            });
            Text = UpText + ": " + Nickname;
        }

        /// <summary>
        /// Set up the app view as logged out and send bye message to server
        /// </summary>
        void Logout()
        {
            ChatClient.RequestQuit(Server, Nickname);
            InvokeIt(tabControl1, () =>
            {
                tabControl1.TabPages.Remove(tabPageChat);
                if (tabControl1.TabPages.Contains(tabPageCall))
                {
                    // if call panel is active, must close current talk
                    callPanel1.OuterReject();
                }
                tabControl1.TabPages.Add(tabPageLogin);
                tabControl1.SelectedTab = tabPageLogin;
                listBoxUsers.Items.Clear();
                Text = UpText;
            });
            IsConnected = false;
        }
        
        private void buttonQuit_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                Logout();
                ChatClient.Close(1000);
                UdpChatClient.StopListenAsync(1000);
            }
            this.Close();
        }
        /// <summary>
        /// Hard logout without re-state GUI 
        /// </summary>
        private void MainClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChatClient.StopListenAsync();
            UdpChatClient.StopListenAsync();
        }

        #endregion


        #region Call handlers

        void CallIni()
        {
            ChatClient.OnTalkOffer += new ClientListener.OnTalkOfferEventHandler(
                delegate(string From)
                {
                    InvokeIt(tabControl1, () =>
                    {
                        if (!tabControl1.TabPages.Contains(tabPageCall))
                        {
                            tabControl1.TabPages.Add(tabPageCall);
                            tabControl1.SelectedTab = tabPageCall;
                        }
                    });
                    callPanel1.OnTalkOffer(From);
                });

            ChatClient.OnTalkState += new ClientListener.OnTalkStateEventHandler(
                delegate(string from, TalkState state)
                {
                    if (state == TalkState.Adopt) UdpChatClient.Start(UdpServer);
                    callPanel1.OnTalkState(from, state);
                });

            ChatClient.OnTalkEnd += new ClientListener.OnTalkEndEventHandler(
                delegate(string from)
                {
                    UdpChatClient.Stop();
                    callPanel1.OnTalkEnd(from);
                });
                
        }

        private void buttonCall_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                string receiver = (string)listBoxUsers.SelectedItem;
                if (receiver == "") return;
                callPanel1.TalkWith = receiver;
                tabControl1.TabPages.Add(tabPageCall);
                tabControl1.SelectedTab = tabPageCall;
                buttonCall.Enabled = false;
            }
        }

        private void callPanel1_OnHide()
        {
            tabControl1.SelectedTab = tabPageChat;
            tabControl1.TabPages.Remove(tabPageCall);
            buttonCall.Enabled = true;
            UdpChatClient.Stop();
        }

        private void callPanel1_OnCall(string To)
        {
            ChatClient.RequestTalk(Server, Nickname, To);
            SetLogText("Calling " + To + "...");
        }

        private void callPanel1_OnOfferAnswer(string To, TalkState Answer)
        {
            ChatClient.RequestTalkState(Server, Nickname, To, Answer);
            if(Answer == TalkState.Adopt) UdpChatClient.Start(UdpServer);
            SetLogText("Call with " + To + " is " + ResponseTalkState.TalkStateString(Answer));
        }

        private void callPanel1_OnRejectCall(string To)
        {
            ChatClient.RequestEndTalk(Server, Nickname, To);
            SetLogText("Talk with " + To + " is rejected");
            UdpChatClient.Stop();
        }

        #endregion
    }
}
