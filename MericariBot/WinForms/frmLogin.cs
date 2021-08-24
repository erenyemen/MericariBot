using MericariBot.Helper;
using MericariBot.Models;
using System;
using System.Drawing;
using System.Management;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MericariBot.WinForms
{
    public partial class frmLogin : Form
    {
        DataAccess da;
        public frmLogin()
        {
            da = new DataAccess();
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == string.Empty || textBox2.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Username and password cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (textBox1.Text.Trim() == "username" || textBox2.Text.Trim() == "password")
            {
                MessageBox.Show("Username and password cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User user = new User()
            {
                UserName = textBox1.Text,
                Password = textBox2.Text,
                MACAddress = GetMacAddress(),
                IPAddress = GetIpAddress()
            };

           UserLoginResult loginedUser = da.LoginUser(user);

            if (loginedUser.IsError)
            {
                MessageBox.Show(loginedUser.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (loginedUser.IsWarning)
            {
                MessageBox.Show(loginedUser.ErrorMessage, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.Hide();
                MainForm frm = new MainForm(loginedUser.LoginedUser);
                frm.Show();
            }
        }

        private string GetIpAddress()
        {
            var webClient = new WebClient();

            string dnsString = webClient.DownloadString("http://checkip.dyndns.org");
            dnsString = (new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b")).Match(dnsString).Value;

            webClient.Dispose();
            return dnsString;
        }

        private string GetMacAddress()
        {
            ManagementClass manager = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject obj in manager.GetInstances())
            {
                if ((bool)obj["IPEnabled"])
                {
                    return obj["MacAddress"].ToString();
                }
            }
            return String.Empty;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == string.Empty)
            {
                textBox1.ForeColor = Color.Gray;
                textBox1.Text = "username";
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "username")
            {
                textBox1.Clear();
                textBox1.ForeColor = Color.Black;

            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == string.Empty)
            {
                textBox2.ForeColor = Color.Gray;
                textBox2.UseSystemPasswordChar = false;
                textBox2.Text = "password";
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "password")
            {
                textBox2.Clear();
                textBox2.ForeColor = Color.Black;
                textBox2.UseSystemPasswordChar = true;
            }
        }
    }
}
