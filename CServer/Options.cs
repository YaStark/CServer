using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CServer
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.AfkUserState = cBoxAfkUserState.Checked;
            Properties.Settings.Default.AfkUserTimeoutMs = (double)numAfkTimeoutSec.Value * 1000;
            Properties.Settings.Default.TimeoutSendRequestMs = (double)numRequestTimeoutMs.Value;
            Properties.Settings.Default.TimeoutServerOffMs = (double)numShutdownTimeoutSec.Value * 1000;
            Properties.Settings.Default.Save();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void Options_Load(object sender, EventArgs e)
        {
            cBoxAfkUserState.Checked = Properties.Settings.Default.AfkUserState;
            numAfkTimeoutSec.Value = (decimal)Properties.Settings.Default.AfkUserTimeoutMs / 1000;
            numRequestTimeoutMs.Value = (decimal)Properties.Settings.Default.TimeoutSendRequestMs;
            numShutdownTimeoutSec.Value = (decimal)Properties.Settings.Default.TimeoutServerOffMs / 1000;
        }
    }
}
