using System;
using System.Drawing;
using System.Windows.Forms;

namespace AutoCentral
{
    public partial class AutoCentral : Form
    {
        public AutoCentral()
        {
            InitializeComponent();
            Login.closeForm = (Login l) =>
            {
                l.Visible = false;
            };
            //MessageBox.Show();
        }

        private void AutoCentral_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConnectionSocketIO.Instance.unRegister();
            Application.Exit();
        }

        private void btnConnectSocket_Click(object sender, EventArgs e)
        {

        }

        private void AutoCentral_Load(object sender, EventArgs e)
        {

        }
    }
}
