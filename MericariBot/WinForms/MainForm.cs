using MericariBot.Models;
using MericariBot.UserController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MericariBot.WinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void tsmAmazon_Click(object sender, EventArgs e)
        {
            OpenNewTabPage(ECommerceType.Amazon, "Amazon", 1);
        }

        private void tsmRakuten_Click(object sender, EventArgs e)
        {
            OpenNewTabPage(ECommerceType.Rakuten, "Rakuten", 2);
        }

        private void tsmMericari_Click(object sender, EventArgs e)
        {
            OpenNewTabPage(ECommerceType.Mercari, "Mercari", 3);
        }

        private void OpenNewTabPage(ECommerceType commerceType, string title, int imageIndex = 0)
        {
            TabPage tp = new TabPage(commerceType.ToString());
            ucBrowser uc = new ucBrowser(commerceType);
            tp.ImageIndex = imageIndex;
            tp.Controls.Add(uc);
            BrowserTabControl.TabPages.Add(tp);
            BrowserTabControl.SelectedTab = tp;
            uc.Initialize();
            uc.Dock = DockStyle.Fill;
            uc.OpenPage();
        }

        private void BrowserTabControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                TabControl obj = (TabControl)sender;

                ucBrowser de = (ucBrowser)obj.SelectedTab.Controls[0];
                de.Dispose();
                de = null;

                obj.SelectedTab.Controls.Clear();

                int indexOfTabPage = obj.TabPages.IndexOf(obj.SelectedTab);
                ((TabControl)sender).TabPages.Remove(((TabControl)sender).SelectedTab);

                if (indexOfTabPage != 0)
                    obj.SelectTab(indexOfTabPage - 1);
            }
        }

        private void tsmAdd_Click(object sender, EventArgs e)
        {
            ucBrowser browser = (ucBrowser)BrowserTabControl.SelectedTab.Controls[0];

            var result = browser.GetProduct();
        }
    }
}
