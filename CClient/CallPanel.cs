using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCommon;

namespace CClient
{
    public partial class CallPanel : UserControl
    {
        public enum CallState { None, Talk, Answer }
        CallState _state = CallState.None;
        public CallState State 
        {
            get { return _state; }
            set
            {
                _state = value;
                SetState(_state);
            }
        }

        string _talkWith = "";
        public string TalkWith
        {
            get { return _talkWith; }
            set
            {
                try
                {
                    _talkWith = value;
                    Action setText = new Action(() => labelName.Text = _talkWith);
                    if (InvokeRequired) labelName.Invoke(setText);
                    else setText();
                }
                catch { }
            }
        }

        public string Status
        {
            get { return labelStatus.Text; }
            set
            {
                try
                {
                    Action act = new Action(() => labelStatus.Text = value);
                    if (InvokeRequired) labelStatus.Invoke(act);
                    else act();
                }
                catch { }
            }
        }

        public delegate void OnOfferAnswerEventHandler(string To, TalkState Answer);
        public event OnOfferAnswerEventHandler OnOfferAnswer;

        public delegate void OnHideEventHandler();
        public event OnHideEventHandler OnHide;

        public delegate void OnCallEventHandler(string To);
        public event OnCallEventHandler OnCall;

        public delegate void OnRejectCallEventHandler(string To);
        public event OnRejectCallEventHandler OnRejectCall;

        public CallPanel()
        {
            InitializeComponent();
        }

        void SetState(CallState state)
        {
            try
            {
                Action act = new Action(() =>
                        {
                            panelNone.Visible = state == CallState.None;
                            panelAnswer.Visible = state == CallState.Answer;
                            panelTalk.Visible = state == CallState.Talk;
                        });
                if (InvokeRequired) Invoke(act);
                else act();
            }
            catch { }
        }

        /// <summary>
        /// Event handler for OnTalkState event
        /// </summary>
        public void OnTalkState(string From, TalkState TalkState)
        {
            if (From != TalkWith) return; // Not allowed client 

            switch (TalkState)
            {
                case TalkState.Adopt:
                    State = CallState.Talk;
                    Status = "Connection OK";
                    break;

                case TalkState.Busy:
                    State = CallState.None;
                    Status = "User " + From + " is busy";
                    break;

                case TalkState.Reject:
                    State = CallState.None;
                    Status = "User " + From + " reject call offer";
                    break;
            }
        }

        /// <summary>
        /// Event handler for OnTalkOffer event
        /// </summary>
        public void OnTalkOffer(string From)
        {
            if (State != CallState.None)
            {
                // if CallState is call or waiting answer, then reject other incoming calls 
                if (OnOfferAnswer != null) OnOfferAnswer(From, TalkState.Busy);
                return;
            }
            Status = "Incoming call";
            TalkWith = From; 
            State = CallState.Answer;
        }

        /// <summary>
        /// Can occur when status is Talk and companion is hang up, or
        /// when status is Answer and companion stop waiting for a response
        /// </summary>
        public void OnTalkEnd(string From)
        {
            if (State != CallState.None && TalkWith == From)
            {
                Status = From + " is hang up";
                State = CallState.None;
            }
        }

        /// <summary>
        /// Forcibly closes the call connection
        /// </summary>
        public void OuterReject()
        {
            buttonHide_Click(null, null);
        }

        #region Buttons handlers

        private void buttonAnswerDecline_Click(object sender, EventArgs e)
        {
            if(OnOfferAnswer != null) OnOfferAnswer(TalkWith, TalkState.Reject);
            State = CallState.None;
            Status = "Rejected";
        }

        private void buttonNoneCall_Click(object sender, EventArgs e)
        {
            if (OnCall != null) OnCall(TalkWith);
            State = CallState.Talk;
            Status = "Calling...";
        }

        private void buttonTalkCancel_Click(object sender, EventArgs e)
        {
            if (OnRejectCall != null) OnRejectCall(TalkWith);
            State = CallState.None;
            Status = "Switched off";
        }

        private void buttonAnswerAccept_Click(object sender, EventArgs e)
        {
            if (OnOfferAnswer != null) OnOfferAnswer(TalkWith, TalkState.Adopt);
            State = CallState.Talk;
            Status = "";
        }

        private void buttonHide_Click(object sender, EventArgs e)
        {
            if (OnHide != null) OnHide();
            if (State == CallState.Talk)
            {
                if (OnRejectCall != null) OnRejectCall(TalkWith);
            }
            Status = "";
            State = CallState.None;
            TalkWith = "";
        }

        #endregion
    }
}
