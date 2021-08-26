using MericariBot.Helper;
using MericariBot.Models;
using System;
using System.Windows.Forms;

namespace MericariBot.WinForms
{
    public partial class frmUser : Form
    {
        DataAccess da;
        User _user;

        public frmUser(User user = null)
        {
            InitializeComponent();
            da = new DataAccess();
            _user = user;

            if (_user != null)
            {
                FillUserComponents(user);
            }
            else
            {
                cmbUserRole.SelectedIndex = 1;
            }
        }

        private void FillUserComponents(User user)
        {
            txtUserId.Text = user.UserId.ToString();
            txtUserName.Text = user.UserName;
            txtPassword.Text = user.Password;
            txtMacAddress.Text = user.MACAddress;
            txtIpAddress.Text = user.IPAddress;
            chcIsActive.Checked = user.IsActive;
            cmbUserRole.SelectedItem = user.Role.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_user == null)
            {
                _user = new User();
            }

            _user.UserName = txtUserName.Text.Trim();
            _user.Password = txtPassword.Text.Trim();
            _user.MACAddress = txtMacAddress.Text.Trim();
            _user.IPAddress = txtIpAddress.Text.Trim();
            _user.Role = (UserRole)Enum.Parse(typeof(UserRole), cmbUserRole.SelectedItem.ToString());
            _user.IsActive = chcIsActive.Checked;

            var res = da.SaveUser(_user);

            if (res == -1)
            {
                MessageBox.Show("user could not be saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("user successfully saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}