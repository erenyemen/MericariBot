using Gecko;
using MericariBot.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using Gecko.DOM;

namespace MericariBot.UserController
{
    public partial class ucBrowser : UserControl
    {
        private string _url { get { return GetUrl(); } }
        private ECommerceType _commerceType { get; set; }

        public ucBrowser(ECommerceType commerceType)
        {
            _commerceType = commerceType;

            InitializeComponent();

            txtUrl.Text = _url;
            PromptFactory.PromptServiceCreator = () => new FilteredPromptService();

        }

        private string GetUrl()
        {
            switch (_commerceType)
            {
                case ECommerceType.Amazon: return "https://www.amazon.co.jp/";
                case ECommerceType.Rakuten: return "https://www.rakuten.co.jp/";
                case ECommerceType.Mercari: return "https://www.mercari.com/jp/";
                default: return "";
            }
        }

        public void OpenPage()
        {
            geckoWebBrowser1.Navigate(_url);
        }

        private void btnBackward_Click(object sender, EventArgs e)
        {
            geckoWebBrowser1.GoBack();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            geckoWebBrowser1.GoForward();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            geckoWebBrowser1.Navigate(txtUrl.Text);
        }

        public Product GetProduct()
        {
            var bodyHtml = geckoWebBrowser1.Document.Body.OuterHtml;
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(bodyHtml);

            switch (_commerceType)
            {
                case ECommerceType.Amazon: return GetAmazonProduct(doc);
                case ECommerceType.Rakuten: return GetRakutenProduct();
                case ECommerceType.Mercari: return GetMercariProduct();
                default: return null;
            }
        }

        public Product GetAmazonProduct(HtmlAgilityPack.HtmlDocument doc)
        {
            //geckoWebBrowser1.Document.GetHtmlElementById("add-to-cart-button").Click();

            Product result = new Product() 
            { 
                ImagesPath = GetImagesFromAmazon(doc),
                Name = geckoWebBrowser1.Document.GetHtmlElementById("productTitle").InnerHtml.Replace("\n", ""),
                Description = GetDescriptionFromAmazon(doc)
            };

            return result;
        }

        private string GetDescriptionFromAmazon(HtmlAgilityPack.HtmlDocument doc)
        {
            string result = string.Empty;

            HtmlNode specificNode = doc.GetElementbyId("feature-bullets");
            var nodes = specificNode.ChildNodes.Where(x => x.Name == "ul").First().ChildNodes.Where(x => x.Name == "li");

            StringBuilder sb = new StringBuilder();
            foreach (var item in nodes)
            {
                if (string.IsNullOrEmpty(item.Id))
                    sb.Append(item.InnerText);
            }

            result = sb.ToString();

            return result;

            //var res = geckoWebBrowser1.Document.GetHtmlElementById("feature-bullets").InnerHtml;

            //var dasd = geckoWebBrowser1.Document.GetHtmlElementById("feature-bullets").ChildNodes;

            //foreach (var item in dasd)
            //{
            //    var asdasd = item.NodeName;
            //}

            //geckoWebBrowser1.Document.GetHtmlElementById("productTitle").InnerHtml = "eren yemen";
        }

        private List<string> GetImagesFromAmazon(HtmlAgilityPack.HtmlDocument doc)
        {
            List<string> result = new List<string>();

            HtmlNode specificNode = doc.GetElementbyId("main-image-container");
            var nodes = specificNode.ChildNodes.FirstOrDefault(x => x.Name == "ul").ChildNodes.Where(x => x.Name == "li");

            foreach (var nodeItem in nodes)
            {
                var attribute = nodeItem.Attributes["class"];

                if (attribute == null || !attribute.Value.Contains("image")) continue;

                var imageNode = nodeItem.ChildNodes.FirstOrDefault().ChildNodes["span"].ChildNodes["div"].ChildNodes["img"].Attributes["src"];

                result.Add(imageNode.Value);

                //if (string.IsNullOrEmpty(nodeItem.Id))
                //{
                //}
            }

            return result;
        }

        private void ThumpImageClickFromAmazon(HtmlAgilityPack.HtmlDocument doc)
        {
            var dddd = geckoWebBrowser1.Document.GetElementsByClassName("a-button a-button-thumbnail a-button-toggle a-button-selected a-button-focus");

            foreach (var item in dddd)
            {
                var res = item.ParentElement.DOMElement;

                
            }

            geckoWebBrowser1.Document.GetHtmlElementById("a-autoid-5").Click();
            geckoWebBrowser1.Document.GetHtmlElementById("a-autoid-6").Click();
            geckoWebBrowser1.Document.GetHtmlElementById("a-autoid-7").Click();

            //thump id : altImages > ul > li focus or click

            HtmlNode specificNode = doc.GetElementbyId("altImages");
            var nodes = specificNode.ChildNodes.Where(x => x.Name == "ul").First().ChildNodes.Where(x => x.Name == "li");

            foreach (var item in nodes)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    geckoWebBrowser1.Document.GetElementsByTagName("").First().Click();
                    var ddd = item.ChildNodes.First().ChildNodes;

                    foreach (var asda in ddd)
                    {

                    }
                    var image = item.ChildNodes.FirstOrDefault(x => x.Name == "img");
                    var dd = image.GetAttributes();
                }
            }
        }

        public Product GetRakutenProduct()
        {
            return null;
        }

        public Product GetMercariProduct()
        {
            return null;
        }

        private void geckoWebBrowser1_Navigating(object sender, Gecko.Events.GeckoNavigatingEventArgs e)
        {
            txtUrl.Text = e.Uri.AbsoluteUri;
        }

        private void geckoWebBrowser1_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {

        }

        private void geckoWebBrowser1_FrameNavigating(object sender, Gecko.Events.GeckoNavigatingEventArgs e)
        {

        }

        private void geckoWebBrowser1_CreateWindow(object sender, GeckoCreateWindowEventArgs e)
        {
            e.WebBrowser = geckoWebBrowser1;
        }

        private void geckoWebBrowser1_DomContentChanged(object sender, DomEventArgs e)
        {
            
        }

        private void geckoWebBrowser1_Load(object sender, DomEventArgs e)
        {

        }

        private void geckoWebBrowser1_NavigationError(object sender, Gecko.Events.GeckoNavigationErrorEventArgs e)
        {

        }

        private void geckoWebBrowser1_Navigated(object sender, GeckoNavigatedEventArgs e)
        {

        }

        private void geckoWebBrowser1_GeckoHandleCreated(object sender, EventArgs e)
        {

        }

        private void geckoWebBrowser1_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {

        }

        private void geckoWebBrowser1_NSSError(object sender, Gecko.Events.GeckoNSSErrorEventArgs e)
        {

        }

        private void geckoWebBrowser1_Validated(object sender, EventArgs e)
        {

        }
    }


    public class FilteredPromptService : nsIPrompt
    {
        public void Alert(string dialogTitle, string text)
        {
            //do nothing, 
        }

        public void AlertCheck(string dialogTitle, string text, string checkMsg, ref bool checkValue)
        {
            //throw new NotImplementedException();
        }

        public bool Confirm(string dialogTitle, string text)
        {
            return true;
            //throw new NotImplementedException();
        }

        public bool ConfirmCheck(string dialogTitle, string text, string checkMsg, ref bool checkValue)
        {
            return true;
            //throw new NotImplementedException();
        }

        public int ConfirmEx(string dialogTitle, string text, uint buttonFlags, string button0Title, string button1Title, string button2Title, string checkMsg, ref bool checkValue)
        {
            return 0;
            throw new NotImplementedException();
        }

        public bool Prompt(string dialogTitle, string text, ref string value, string checkMsg, ref bool checkValue)
        {
            return true;
            throw new NotImplementedException();
        }

        public bool PromptPassword(string dialogTitle, string text, ref string password, string checkMsg, ref bool checkValue)
        {
            return true;
            throw new NotImplementedException();
        }

        public bool PromptUsernameAndPassword(string dialogTitle, string text, ref string username, ref string password, string checkMsg, ref bool checkValue)
        {
            return true;
            throw new NotImplementedException();
        }

        public bool Select(string dialogTitle, string text, uint count, IntPtr[] selectList, ref int outSelection)
        {
            return true;
            throw new NotImplementedException();
        }
        //and so on for other alerts/prompts
    }
}
