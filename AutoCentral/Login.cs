using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoCentral
{
    public partial class Login : Form
    {
        public static Action<Login> closeForm;
        public Login()
        {
            InitializeComponent();
            bindComboBox(ConfigCenter.Instance.getConfig("get-list-server"));
            ConnectionSocketIO.Instance.LoginThatBai = (mess) =>
            {
                MessageBox.Show(mess);
            };
            ConnectionSocketIO.Instance.LoginThanhCong = () =>
            {
                 this.Invoke((MethodInvoker)delegate {
                     AutoCentral auto = new AutoCentral();
                     auto.Show();
                   closeForm.Invoke(this);
                   // Close();
                });
            };
        }

        

        private void bindComboBox(string url)
        {
            var tmp = RestSharpWrapper.Instance.get(url);
            List<ServerDTO> values = JsonConvert.DeserializeObject<List<ServerDTO>>(tmp.ToString());
            comboServer.DataSource = values;
            comboServer.DisplayMember = "name";
            comboServer.ValueMember = "id";
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            if
                (
                    txtUserName.Text.Equals("")
                    ||
                    txtPassword.Text.Equals("")
                    ||
                    comboServer.Text.Equals("")
                )
            {
                MessageBox.Show("Nhập thông tin đăng nhập");
            }
            else
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("server", comboServer.SelectedValue.ToString());
                dic.Add("user_name", txtUserName.Text.ToString());
                dic.Add("pass_word", txtPassword.Text.ToString());
                DataAutoIT.account = txtUserName.Text.ToString();
                DataAutoIT.server = comboServer.SelectedValue.ToString();
                ConnectionSocketIO.Instance.emit("register", JsonConvert.SerializeObject(dic));
            }
        }
    }
}
