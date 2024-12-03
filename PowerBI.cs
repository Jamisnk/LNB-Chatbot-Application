using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chatbot_Application
{
    public partial class PowerBI : Form
    {
        public PowerBI()
        {
            InitializeComponent();
        }

        private void PowerBI_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("www.PowerBI.com");
        }
    }
}
