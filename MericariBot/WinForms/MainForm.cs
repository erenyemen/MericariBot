using MericariBot.Models;
using MericariBot.UserController;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
            if (BrowserTabControl.SelectedTab == null) return;

            ucBrowser browser = (ucBrowser)BrowserTabControl.SelectedTab.Controls[0];

            var result = browser.GetProduct();

            SaveImagesToTempFolder(result);

            AddProductToProduct(result, browser._commerceType);

            OpenNewTabPageForProductAdd(result, ECommerceType.MercariSell, "mercari.com Sell", 3);
        }

        private void AddProductToProduct(Product product, ECommerceType commerceType)
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            string chromeProfilePath = $@"--user-data-dir=D:\Users\{Environment.UserName}\AppData\Local\Google\Chrome\User Data\Default";

            ChromeOptions options = new ChromeOptions();
            //options.AddArguments("headless");
            options.AddArguments(chromeProfilePath);

            IWebDriver driver = new ChromeDriver(chromeDriverService, options);
            //driver.Navigate().GoToUrl("https://www.n11.com/"); //Mercari

            driver.Navigate().GoToUrl("file:///D:/Users/ey000087/Desktop/mercari/sell/%E5%87%BA%E5%93%81%20-%20%E3%83%A1%E3%83%AB%E3%82%AB%E3%83%AA%20%E3%82%B9%E3%83%9E%E3%83%9B%E3%81%A7%E3%81%8B%E3%82%93%E3%81%9F%E3%82%93%20%E3%83%95%E3%83%AA%E3%83%9E%E3%82%A2%E3%83%97%E3%83%AA.html");

            //Images
            IWebElement upload_file = driver.FindElement(By.XPath("//input[@type='file']"));

            //upload_file.SendKeys(@"D:\Users\ey000087\Desktop\Signature Verified.PNG");

            foreach (var item in product.ImagesPath)
            {
                upload_file.SendKeys(item);
            }


            //Product Title
            driver.FindElements(By.Name("name"))[1].SendKeys(product.Title);

            //Product Description
            driver.FindElements(By.Name("description"))[2].SendKeys(product.Description);


            //Category
            var education = driver.FindElements(By.Name("categoryId"))[1];
            SelectElement selectElement = new SelectElement(education);

            if (commerceType == ECommerceType.Mercari)
            {
                selectElement.SelectByText(product.Category.Name);
            }
            else
            {
                selectElement.SelectByIndex(1);
            }


            //Sub Category 1
            education = driver.FindElements(By.Name("categoryId"))[2];
            selectElement = new SelectElement(education);

            if (commerceType == ECommerceType.Mercari)
            {
                selectElement.SelectByText(product.SubCategory1.Name);
            }
            else
            {
                selectElement.SelectByIndex(1);
            }

            //Sub Category 2
            education = driver.FindElements(By.Name("categoryId"))[3];
            selectElement = new SelectElement(education);
            if (commerceType == ECommerceType.Mercari)
            {
                selectElement.SelectByText(product.SubCategory2.Name);
            }
            else
            {
                selectElement.SelectByIndex(1);
            }

            //Brand Name
            if (!string.IsNullOrEmpty(product.Brand))
                driver.FindElements(By.Name("brandName"))[0].SendKeys(product.Brand);
            else
                driver.FindElements(By.Name("brandName"))[0].SendKeys("Empty");


            education = driver.FindElements(By.Name("itemCondition"))[1];
            selectElement = new SelectElement(education);
            if (commerceType == ECommerceType.Mercari)
            {
                selectElement.SelectByText(product.Condition);
            }
            else
            {
                selectElement.SelectByIndex(1);
            }

            //shippingPayer
            education = driver.FindElements(By.Name("shippingPayer"))[1];
            selectElement = new SelectElement(education);
            if (commerceType == ECommerceType.Mercari)
            {
                selectElement.SelectByText(product.ShippingCharges);
            }
            else
            {
                selectElement.SelectByIndex(1);
            }
            //selectElement.SelectByValue("2");

            //shippingFromArea
            education = driver.FindElements(By.Name("shippingFromArea"))[1];
            selectElement = new SelectElement(education);
            if (commerceType == ECommerceType.Mercari)
            {
                selectElement.SelectByText(product.ShippingArea);
            }
            else
            {
                selectElement.SelectByIndex(1);
            }

            //shippingDuration
            education = driver.FindElements(By.Name("shippingDuration"))[1];
            selectElement = new SelectElement(education);
            if (commerceType == ECommerceType.Mercari)
            {
                selectElement.SelectByText(product.DaysToShip);
            }
            else
            {
                selectElement.SelectByIndex(3);
            }

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


            driver.Dispose();
            driver = null;
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

        private void tsmReAdd_Click(object sender, EventArgs e)
        {
            ucBrowser browser = (ucBrowser)BrowserTabControl.SelectedTab.Controls[0];

            browser.AddProduct();
        }

        private void googleChromeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var obj = Environment.UserName;

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();

            string chromeProfilePath = $@"--user-data-dir=D:\Users\{Environment.UserName}\AppData\Local\Google\Chrome\User Data\Default";
            options.AddArguments(chromeProfilePath);

            IWebDriver driver = new ChromeDriver(chromeDriverService, options);
            driver.Navigate().GoToUrl("https://www.n11.com/"); //Mercari
        }
    }
}
