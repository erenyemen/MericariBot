using MericariBot.Helper;
using MericariBot.Models;
using MericariBot.UserController;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace MericariBot.WinForms
{
    public partial class MainForm : Form
    {
        #region Properties

        Thread threadBackground;

        public User _user { get; set; }
        public string ChromeProfilePath { get; set; }

        #endregion Properties

        public MainForm(User user)
        {
            InitializeComponent();
            LoadUserInfo(user);
            LoadAdvert();

            ChromeProfilePath = $@"--user-data-dir=C:\Users\{Environment.UserName}\AppData\Local\Google\Chrome\User Data\Default";

            if (user.Role != UserRole.Admin)
            {
                tsmUserManagament.Visible = false;
                advertToolStripMenuItem.Visible = false;
            }

            threadBackground = new Thread(() => BackgroundJob(user));
            threadBackground.Start();
        }

        #region Events

        private void tsmUserManagament_Click(object sender, EventArgs e)
        {
            OpenNewTabPageForWinForm("User List");
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

        private void tsmAdd_Click(object sender, EventArgs e)
        {
            if (BrowserTabControl.SelectedTab == null) return;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ucBrowser browser = (ucBrowser)BrowserTabControl.SelectedTab.Controls[0];

                var result = browser.GetProduct();

                SaveImagesToTempFolder(result);
                //return;
                var SellUrl = AddProductToProduct(result, browser._commerceType, "https://www.mercari.com/jp/sell");

                OpenNewTabPageForProductAdd(result, ECommerceType.MercariSell, "mercari.com | Sell", SellUrl, 3);
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}\n\n{(exp.InnerException == null ? string.Empty : exp.InnerException.Message)}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }

        private void tsmReAdd_Click(object sender, EventArgs e)
        {
            if (BrowserTabControl.SelectedTab == null) return;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ucBrowser browser = (ucBrowser)BrowserTabControl.SelectedTab.Controls[0];

                var result = browser.GetProduct();

                SaveImagesToTempFolder(result);

                //TODO: Ürünü kaldır butonuna tıklanacak.
                var elements = browser.geckoWebBrowser1.Document.GetElementsByTagName("button");

                bool isButtonClick = false;
                foreach (var item in elements)
                {
                    if (item.ClassName == "btn-default btn-gray")
                    {
                        if (item.TextContent == "出品を一旦停止する")
                        {
                            item.Click();
                            isButtonClick = true;
                            break;
                        }
                    }
                }

                if (isButtonClick)
                {
                    //TODO: Ürünü draft olarak değil gerçekten kaydedecek.
                    var SellUrl = AddProductToProduct(result, browser._commerceType, "https://www.mercari.com/jp/sell");

                    OpenNewTabPageForProductAdd(result, ECommerceType.MercariSell, "mercari.com | Sell", SellUrl, 3);
                }
                else
                {
                    MessageBox.Show("The product could not be temporarily stopped", "Warning", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}\n\n{(exp.InnerException == null ? string.Empty : exp.InnerException.Message)}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }

        private void tsmGoogleChrome_Click(object sender, EventArgs e)
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();
            options.AddArguments(ChromeProfilePath);

            IWebDriver driver = new ChromeDriver(chromeDriverService, options);
            driver.Navigate().GoToUrl("https://www.mercari.com/jp/sell");
        }

        private void BrowserTabControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                TabControl obj = (TabControl)sender;

                if (obj.SelectedTab.Text != "User List")
                {
                    ucBrowser de = (ucBrowser)obj.SelectedTab.Controls[0];
                    de.Dispose();
                    de = null;
                }

                obj.SelectedTab.Controls.Clear();

                int indexOfTabPage = obj.TabPages.IndexOf(obj.SelectedTab);
                ((TabControl)sender).TabPages.Remove(((TabControl)sender).SelectedTab);

                if (indexOfTabPage != 0)
                    obj.SelectTab(indexOfTabPage - 1);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var processList = Process.GetProcesses().Where(x => x.ProcessName.Equals("chromedriver")).ToList();

                foreach (var item in processList)
                {
                    item.Kill();
                }

                threadBackground.Abort();
            }
            catch { }


            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void advertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdvert frm = new frmAdvert(pictureBox1);
            frm.ShowDialog();

            LoadAdvert();
        }

        #endregion Events

        #region Methods

        private void LoadUserInfo(User user)
        {
            _user = user;

            stslblUserName.Text = _user.UserName;
            stslblIpAddress.Text = _user.IPAddress;
            stslblUserRole.Text = _user.Role.ToString();
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

        private void OpenNewTabPageForWinForm(string title, int imageIndex = 0)
        {
            TabPage tp = new TabPage(title);
            tp.ImageIndex = imageIndex;

            frmUserList frm = new frmUserList();
            frm.TopLevel = false;
            tp.Controls.Add(frm);
            frm.Dock = DockStyle.Fill;
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //frm.Dock = DockStyle.Fill;
            frm.Show();






            BrowserTabControl.TabPages.Add(tp);
            BrowserTabControl.SelectedTab = tp;

        }

        private void OpenNewTabPageForProductAdd(Product product, ECommerceType commerceType, string title, string SellUrl, int imageIndex = 0)
        {
            TabPage tp = new TabPage(title);
            ucBrowser uc = new ucBrowser(product, commerceType, SellUrl);
            tp.ImageIndex = imageIndex;
            tp.Controls.Add(uc);
            BrowserTabControl.TabPages.Add(tp);
            BrowserTabControl.SelectedTab = tp;
            uc.Initialize();
            uc.Dock = DockStyle.Fill;
            uc.OpenPage();
        }

        private string AddProductToProduct(Product product, ECommerceType commerceType, string url)
        {
            string resultUrl = string.Empty;

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("headless");
            options.AddArguments(ChromeProfilePath);

            IWebDriver driver = new ChromeDriver(chromeDriverService, options);
            driver.Navigate().GoToUrl(url);

            //Images
            IWebElement upload_file = driver.FindElement(By.XPath("//input[@type='file']"));

            int count = 1;
            foreach (var item in product.ImagesPath)
            {
                upload_file.SendKeys(item);

                if (count >= 5) break;

                count = +1;
            }

            //Product Title
            driver.FindElements(By.Name("name"))[1].SendKeys(product.Title);

            //Product Description
            driver.FindElements(By.Name("description"))[2].SendKeys(product.Description);

            //Category
            var education = driver.FindElements(By.Name("categoryId"))[1];
            SelectElement selectElement = new SelectElement(education);

            if (commerceType == ECommerceType.Mercari)
                selectElement.SelectByText(product.Category.Name);
            else
                selectElement.SelectByIndex(ConfigHelper.GetConfigByKey("Default_CategoryIndex"));

            //Sub Category 1
            education = driver.FindElements(By.Name("categoryId"))[2];
            selectElement = new SelectElement(education);

            if (commerceType == ECommerceType.Mercari)
                selectElement.SelectByText(product.SubCategory1.Name);
            else
                selectElement.SelectByIndex(ConfigHelper.GetConfigByKey("Default_SubCategory1Index"));

            //Sub Category 2
            education = driver.FindElements(By.Name("categoryId"))[3];
            selectElement = new SelectElement(education);

            if (commerceType == ECommerceType.Mercari)
                selectElement.SelectByText(product.SubCategory2.Name);
            else
                selectElement.SelectByIndex(ConfigHelper.GetConfigByKey("Default_SubCategory2Index"));

            //Brand Name
            if (!string.IsNullOrEmpty(product.Brand))
                driver.FindElements(By.Name("brandName"))[0].SendKeys(product.Brand);
            else
                driver.FindElements(By.Name("brandName"))[0].SendKeys("Empty");

            //Condition
            education = driver.FindElements(By.Name("itemCondition"))[1];
            selectElement = new SelectElement(education);

            if (commerceType == ECommerceType.Mercari)
                selectElement.SelectByText(product.Condition);
            else
                selectElement.SelectByIndex(ConfigHelper.GetConfigByKey("Default_ConditionIndex"));

            //shippingPayer
            education = driver.FindElements(By.Name("shippingPayer"))[1];
            selectElement = new SelectElement(education);

            if (commerceType == ECommerceType.Mercari)
                selectElement.SelectByText(product.ShippingCharges);
            else
                selectElement.SelectByIndex(ConfigHelper.GetConfigByKey("Default_shippingPayerIndex"));

            //shippingFromArea
            education = driver.FindElements(By.Name("shippingFromArea"))[1];
            selectElement = new SelectElement(education);

            if (commerceType == ECommerceType.Mercari)
                selectElement.SelectByText(product.ShippingArea);
            else
                selectElement.SelectByIndex(ConfigHelper.GetConfigByKey("Default_shippingFromAreaIndex"));

            //shippingDuration
            education = driver.FindElements(By.Name("shippingDuration"))[1];
            selectElement = new SelectElement(education);

            if (commerceType == ECommerceType.Mercari)
                selectElement.SelectByText(product.DaysToShip);
            else
                selectElement.SelectByIndex(ConfigHelper.GetConfigByKey("Default_shippingDurationIndex"));

            if (commerceType == ECommerceType.Mercari)
            {
                driver.FindElements(By.Name("price"))[1].SendKeys(product.SellingPrice);
            }

            //kaydet
            education = driver.FindElements(By.CssSelector("button[class='style_button__3yWFH common_fontFamily__3-3Si style_primary__Mg3zL style_medium__3wTQ5 style_fluid__3mdYA style_legacy__2D0U0']"))[0];
            //education.Click();

            //Taslak olarak kaydet
            education = driver.FindElements(By.CssSelector("button[class='style_button__3yWFH common_fontFamily__3-3Si style_defaultIntent__22709 style_medium__3wTQ5 style_fluid__3mdYA style_legacy__2D0U0']"))[0];
            education.Click();


            //Sayfanın yüklenmesini bekle #begin
            Thread.Sleep(3000);
            driver.Manage().Timeouts().PageLoad = new TimeSpan(TimeSpan.TicksPerSecond * 5);

            bool flag = true;
            do
            {
                var list = driver.FindElements(By.CssSelector("a[class='style_listlink__2YdMK sc-cfWELz jRintt']"));

                if (list.Count > 0)
                {
                    flag = false;
                }

            } while (flag);
            //Sayfanın yüklenmesini bekle #end


            //son eklenen draft ürüne tıkla
            education = driver.FindElements(By.CssSelector("a[class='style_listlink__2YdMK sc-cfWELz jRintt']"))[0];
            education.Click();

            //Sayfanın yüklenmesini bekle #begin
            driver.Manage().Timeouts().PageLoad = new TimeSpan(TimeSpan.TicksPerSecond * 5);
            Thread.Sleep(3000);
            //education = driver.FindElements(By.CssSelector("a[class='style_list__FdlpK common_fontFamily__3-3Si']"))[0].FindElements(By.TagName("li"))[0].FindElements(By.TagName("a"))[0];
            //education.Click();
            //Sayfanın yüklenmesini bekle #end

            resultUrl = driver.Url;

            //test yapıldıktan sonra açılacak
            driver.Dispose();
            driver = null;

            return resultUrl;
        }

        private void SaveImagesToTempFolder(Product product)
        {
            string TempFolderPath = $"{Environment.CurrentDirectory}/temp";

            if (Directory.Exists(TempFolderPath))
                Directory.Delete(TempFolderPath, true);

            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
            d.CreateSubdirectory("temp");

            foreach (var item in product.ImagesUrl)
            {
                string imageName = item.Split('/').Last().Trim();

                if (imageName.Contains("?"))
                {
                    imageName = imageName.Split('?').First().Trim();
                }

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(item), $@"{TempFolderPath}\{imageName}");

                    product.ImagesPath.Add($@"{TempFolderPath}\{imageName}");
                }
            }
        }

        public void BackgroundJob(User user)
        {
            DataAccess da = new DataAccess();
            while (true)
            {
                var response = da.GetUserById(user.UserId);

                if (!response.IsActive)
                {
                    MessageBox.Show("Locked User", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    ApplicationExit();
                }

                Thread.Sleep(5000);
            }
        }

        private void BackgroundJobSuspend()
        {
            threadBackground.Suspend();
        }

        private void BackgroundJobResume()
        {
            threadBackground.Resume();
        }

        public void ApplicationExit()
        {
            threadBackground.Abort();
            Application.Exit();
        }

        public void LoadAdvert()
        {
            DataAccess da = new DataAccess();
            var res = da.GetAdverts();

            if (res.Count > 0)
            {
                pictureBox1.ImageLocation = DownloadAdvertImage(res[0].AdvertUrl);
                pictureBox1.SizeMode = res[0].ImageSizeMode;
            }
        }

        private string DownloadAdvertImage(string url)
        {
            string TempFolderPath = $"{Environment.CurrentDirectory}/Advert";

            if (Directory.Exists(TempFolderPath))
                Directory.Delete(TempFolderPath, true);

            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
            d.CreateSubdirectory("Advert");

            string imageName = url.Split('/').Last().Trim();

            if (imageName.Contains("?"))
            {
                imageName = imageName.Split('?').First().Trim();
            }

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(url.Trim()), $@"{TempFolderPath}\{imageName}");
            }

            return $@"{TempFolderPath}\{imageName}";
        }

        #endregion Methods
    }
}