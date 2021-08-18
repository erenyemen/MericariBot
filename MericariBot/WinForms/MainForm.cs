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
            OpenNewTabPage(ECommerceType.Amazon, "amazon.co.jp", 1);
        }

        private void tsmRakuten_Click(object sender, EventArgs e)
        {
            OpenNewTabPage(ECommerceType.Rakuten, "rakuten.co.jp", 2);
        }

        private void tsmMericari_Click(object sender, EventArgs e)
        {
            OpenNewTabPage(ECommerceType.Mercari, "mercari.com", 3);
        }

        private void OpenNewTabPage(ECommerceType commerceType, string title, int imageIndex = 0)
        {
            TabPage tp = new TabPage(title);
            ucBrowser uc = new ucBrowser(commerceType);
            tp.ImageIndex = imageIndex;
            tp.Controls.Add(uc);
            BrowserTabControl.TabPages.Add(tp);
            BrowserTabControl.SelectedTab = tp;
            uc.Initialize();
            uc.Dock = DockStyle.Fill;
            uc.OpenPage();
        }

        private void OpenNewTabPageForProductAdd(Product product, ECommerceType commerceType, string title, int imageIndex = 0)
        {
            TabPage tp = new TabPage(title);
            ucBrowser uc = new ucBrowser(product, commerceType);
            tp.ImageIndex = imageIndex;
            tp.Controls.Add(uc);
            BrowserTabControl.TabPages.Add(tp);
            BrowserTabControl.SelectedTab = tp;
            uc.Initialize();
            uc.Dock = DockStyle.Fill;
            uc.OpenPage();
            uc.AddProduct();
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

            OpenNewTabPageForProductAdd(result, ECommerceType.MercariSell, "mercari.com Sell", 3);
        }

        private void tsmReAdd_Click(object sender, EventArgs e)
        {
            ucBrowser browser = (ucBrowser)BrowserTabControl.SelectedTab.Controls[0];

            browser.AddProduct();
        }

        private void googleToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}
