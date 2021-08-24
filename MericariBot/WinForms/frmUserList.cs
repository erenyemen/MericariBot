using MericariBot.Helper;
using MericariBot.Models;
using System;
using System.Windows.Forms;

namespace MericariBot.WinForms
{
    public partial class frmUserList : Form
    {
        DataAccess da;
        public frmUserList()
        {
            InitializeComponent();

            da = new DataAccess();
        }

        private void LoadUsers()
        {
            gvUserList.DataSource = null;
            gvUserList.DataSource = da.GetUsers();
        }

        private void frmUserList_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void btnGetUsers_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUser frm = new frmUser();
            frm.ShowDialog();

            LoadUsers();
        }

        private void gvUserList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var select = (User)gvUserList.CurrentRow.DataBoundItem;

            frmUser frm = new frmUser(select);
            frm.ShowDialog();

            LoadUsers();
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            frmUser frm = new frmUser((User)gvUserList.CurrentRow.DataBoundItem);
            frm.ShowDialog();

            LoadUsers();
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("The user will be deleted, are you sure?", "Question", MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                User usr = (User)gvUserList.CurrentRow.DataBoundItem;
                var res = da.DeleteUser(usr.UserId);

                if (res != -1)
                {
                    MessageBox.Show("user successfully deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("user could not be deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}