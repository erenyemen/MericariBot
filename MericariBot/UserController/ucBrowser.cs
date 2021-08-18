﻿using Gecko;
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
using System.Net;
using System.IO;
using System.Threading;

namespace MericariBot.UserController
{
    public partial class ucBrowser : UserControl
    {
        private string _url { get { return GetUrl(); } set { textBox1.Text = value; } }
        private ECommerceType _commerceType { get; set; }
        private Product _product { get; set; }

        public bool IsPageLoaded = false;

        public ucBrowser(ECommerceType commerceType)
        {
            _commerceType = commerceType;
            PromptFactory.PromptServiceCreator = () => new FilteredPromptService();
        }

        public ucBrowser(Product product, ECommerceType commerceType)
        {
            _commerceType = commerceType;
            _product = product;
            PromptFactory.PromptServiceCreator = () => new FilteredPromptService();
        }

        public void Initialize()
        {
            InitializeComponent();
        }

        private string GetUrl()
        {
            switch (_commerceType)
            {
                case ECommerceType.Amazon: return "https://www.amazon.co.jp/";
                case ECommerceType.Rakuten: return "https://www.rakuten.co.jp/";
                case ECommerceType.Mercari: return "https://www.mercari.com/jp/";
                case ECommerceType.MercariSell: return "file:///D:/Users/ey000087/Desktop/mercari/mercari/mercari_com_jp_sell.html";
                default: return "";
            }
        }

        public void OpenPage()
        {
            geckoWebBrowser1.Navigate(_url);
        }

        #region Button Events

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
            geckoWebBrowser1.Navigate(textBox1.Text);
        }

        #endregion Button Events

        public Product GetProduct()
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(geckoWebBrowser1.Document.Body.OuterHtml);

            switch (_commerceType)
            {
                case ECommerceType.Amazon: return GetProductFromAmazon(doc);
                case ECommerceType.Rakuten: return GetProductFromRakuten(doc);
                case ECommerceType.Mercari: return GetProductFromMercari(doc);
                default: return null;
            }
        }

        #region Get Product From Amazon

        public Product GetProductFromAmazon(HtmlAgilityPack.HtmlDocument doc)
        {
            //geckoWebBrowser1.Document.GetHtmlElementById("add-to-cart-button").Click();
            Product result = new Product() 
            { 
                ImagesPath = GetImagesFromAmazon(doc),
                Title = GetTitleFromAmazon(),
                Description = GetDescriptionFromAmazon(doc)
            };

            return result;
        }

        private string GetTitleFromAmazon()
        {
            return geckoWebBrowser1.Document.GetHtmlElementById("productTitle").InnerHtml.Replace("\n", "");
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
            }

            return result;
        }

        #endregion Get Product From Amazon

        #region Get Product From Rakuten

        public Product GetProductFromRakuten(HtmlAgilityPack.HtmlDocument doc)
        {
            Product result = new Product()
            {
                Title = GetTitleFromRakuten(doc),
                Description = GetDescriptionFromRakuten(doc),
                ImagesPath = GetImagesFromRakuten(doc)
            };

            return result;
        }

        private string GetTitleFromRakuten(HtmlAgilityPack.HtmlDocument doc)
        {
            var title = doc.DocumentNode.SelectNodes("//span[@class='item_name']")[0].InnerText;

            //geckoWebBrowser1.Document.GetElementsByClassName("")[0].TextContent = "";

            return title;
        }

        private List<string> GetImagesFromRakuten(HtmlAgilityPack.HtmlDocument doc)
        {
            //TODO: Bazı resimler gelmiyor, bakılacak.
            List<string> result = new List<string>();

            var newImageNodes = doc.DocumentNode.SelectNodes("//a[@class='rakutenLimitedId_ImageMain1-3']");

            foreach (var itemNew in newImageNodes)
            {
                var deneme = itemNew.Attributes["href"].Value;
                result.Add(deneme);
            }

            return result;

            var trTag = doc.DocumentNode.SelectNodes("//tr[@valign='top']")[0].InnerHtml;

            HtmlAgilityPack.HtmlDocument frameTag = new HtmlAgilityPack.HtmlDocument();
            frameTag.LoadHtml(trTag);

            var imgUrl = frameTag.DocumentNode.SelectNodes("//iframe")[0].Attributes["src"].Value;


            WebRequest req = HttpWebRequest.Create(imgUrl);
            WebResponse res = req.GetResponse();
            StreamReader read = new StreamReader(res.GetResponseStream());
            string iframeHtml = read.ReadToEnd();

            frameTag = new HtmlAgilityPack.HtmlDocument();
            frameTag.LoadHtml(iframeHtml);

            var imglist = frameTag.DocumentNode.SelectNodes("//img");
           
            var baseUrl = $"{req.RequestUri.Scheme}://{req.RequestUri.Host}";

            for (int i = 0; i < req.RequestUri.Segments.Count() - 1; i++)
            {
                baseUrl += req.RequestUri.Segments[i];
            }

            foreach (var item in imglist)
            {
                var Link = $"{baseUrl}{item.Attributes["src"].Value.Remove(0, 2)}";
                result.Add(Link);
            }

            return result;
        }

        private string GetDescriptionFromRakuten(HtmlAgilityPack.HtmlDocument doc)
        {
            var desc = doc.DocumentNode.SelectNodes("//span[@class='item_desc']")[0].InnerText.Remove(0, 10);
            return desc;
        }

        #endregion Get Product From Rakuten

        #region Get Product From Mercari

        public Product GetProductFromMercari(HtmlAgilityPack.HtmlDocument doc)
        {
            Product result = new Product()
            {
                Title = GetTitleFromMercari(doc),
                Description = GetDescriptionFromMercari(doc),
                ImagesPath = GetImagesFromMercari(doc),
                SellingPrice = GetSellingPriceFromMercari(doc)
            };

            GetCategoryFromMercari(doc, result);


            return result;
        }

        private string GetSellingPriceFromMercari(HtmlAgilityPack.HtmlDocument doc)
        {
            var price = doc.DocumentNode.SelectNodes("//span[@class='item-price bold']")[0].InnerText.Remove(0, 1);
            return price;
        }

        private string GetTitleFromMercari(HtmlAgilityPack.HtmlDocument doc)
        {
            var title = doc.DocumentNode.SelectNodes("//h1[@class='item-name']")[0].InnerText;
            return title;
        }

        private string GetDescriptionFromMercari(HtmlAgilityPack.HtmlDocument doc)
        {
            var desc = doc.DocumentNode.SelectNodes("//p[@class='item-description-inner']")[0].InnerText;
            return desc;
        }

        private List<string> GetImagesFromMercari(HtmlAgilityPack.HtmlDocument doc)
        {
            List<string> result = new List<string>();
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='owl-stage-outer']")[0].InnerHtml;

            HtmlAgilityPack.HtmlDocument imgDoc = new HtmlAgilityPack.HtmlDocument();
            imgDoc.LoadHtml(nodes);

            var imgNodes = imgDoc.DocumentNode.SelectNodes("//img");

            foreach (var item in imgNodes)
            {
                var isAttribute = item.Attributes["src"];

                if (isAttribute != null)
                {
                    var imgUrl = item.Attributes["src"].Value;
                    result.Add(imgUrl);
                }
            }
            return result;
        }

        private void GetCategoryFromMercari(HtmlAgilityPack.HtmlDocument doc, Product product)
        {
            var detail = doc.DocumentNode.SelectNodes("//table[@class='item-detail-table']")[0].InnerHtml;
            var trListDoc = new HtmlAgilityPack.HtmlDocument();
            trListDoc.LoadHtml(detail);

            var trNodes = trListDoc.DocumentNode.SelectNodes("//tr");

            foreach (var item in trNodes)
            {
                var temp = new HtmlAgilityPack.HtmlDocument();
                temp.LoadHtml(item.InnerHtml);

                var itemName = temp.DocumentNode.SelectNodes("//th")[0].InnerText;

                if (itemName == "カテゴリー") // Category
                {
                    var itemValue = temp.DocumentNode.SelectNodes("//td/a");

                    product.Category.Name = itemValue[0].InnerText.Replace("\n", "").Trim();
                    product.SubCategory1.Name = itemValue[1].InnerText.Replace("\n","").Trim();
                    product.SubCategory2.Name = itemValue[2].InnerText.Replace("\n", "").Trim();

                }
                else if (itemName == "ブランド") // Marka - Brand
                {
                    if (temp.DocumentNode.SelectNodes("//td/a") != null)
                        product.Brand = temp.DocumentNode.SelectNodes("//td/a")[0].InnerText.Replace("\n", "").Trim();
                }
                else if (itemName == "商品のサイズ")// Size
                {
                    if (temp.DocumentNode.SelectNodes("//td") != null)
                        product.Size = temp.DocumentNode.SelectNodes("//td")[0].InnerText.Replace("\n", "").Trim();
                }
                else if (itemName == "商品の状態") // ürün durumu - Condition
                {
                    if (temp.DocumentNode.SelectNodes("//td") != null)
                        product.Condition = temp.DocumentNode.SelectNodes("//td")[0].InnerText.Replace("\n", "").Trim();
                }
                else if (itemName == "配送料の負担") // nakliye ücretleri
                {
                    if (temp.DocumentNode.SelectNodes("//td") != null)
                        product.ShippingCharges = temp.DocumentNode.SelectNodes("//td")[0].InnerText.Replace("\n", "").Trim();
                }
                else if (itemName == "配送元地域") // Teslimat kaynak alanı
                {
                    if (temp.DocumentNode.SelectNodes("//td") != null)
                        product.ShippingArea = temp.DocumentNode.SelectNodes("//td")[0].InnerText.Replace("\n", "").Trim();
                }
                else if (itemName == "発送日の目安") // Tahmini gönderim tarihi
                {
                    if (temp.DocumentNode.SelectNodes("//td") != null)
                        product.DaysToShip = temp.DocumentNode.SelectNodes("//td")[0].InnerText.Replace("\n", "").Trim();
                }
            }
        }

        #endregion Get Product From Mercari

        public void AddProduct()
            {
            WaitDocumentComplated();

            //int sa = 0;
            //do
            //{
            //    Thread.Sleep(1000);
            //    Application.DoEvents();

            //    sa += 1;
            //} while (sa > 3);


            //geckoWebBrowser1.Navigate("javascript:void(document.getElementsByName('name')[1].value = 'eren yemen')");
            //SendKeys.Send("{TAB}");

            
            GeckoSelectElement selectElement2 = (GeckoSelectElement)geckoWebBrowser1.Document.GetElementsByName("categoryId")[1];
            selectElement2.Click();
            selectElement2.SelectedIndex = 4;
            return;
            //selectElement2.Focus();
           




            //selectElement2.SelectedIndex = 4;



            Application.DoEvents();
            return;
            SetImages();
            SetTitle(true);return;
            SetDescription(true); 
            SetCategory(true); return;
            SetProductDetails();
            SetDelivery();
            SetSellingPrice();
            ClickSell();

            return;

            var categoryList = geckoWebBrowser1.Document.GetElementsByName("categoryId");

            foreach (var item in categoryList)
            {
                var asd = item.ChildNodes;

                foreach (var itemSub in asd)
                {
                    var asdf = itemSub.NodeName;

                    var query = geckoWebBrowser1.Document.GetElementsByTagName("option");

                    foreach (var itemquery in query)
                    {

                    }

                    //GeckoSelectElement selectElement4 = (GeckoSelectElement)geckoWebBrowser1.Document.GetElementById("categoryId");

                    //selectElement4.SelectedIndex = 3;





                    //var optionElements = geckoWebBrowser1.Document.GetElementsByTagName("option");
                    //foreach (GeckoOptionElement optionElement in optionElements)
                    //{

                    //    if (optionElement.Value == "Foo")
                    //    {
                    //        optionElement.Selected = true;
                    //    }
                    //}

                    //GeckoInputElement asdasdr = new GeckoInputElement();

                    var wer = (GeckoOptionElement)geckoWebBrowser1.Document.GetElementsByTagName("option").FirstOrDefault(x => x.GetAttribute("label") == "レディース");

                    wer.Selected = true;

                    var parent = wer.ParentElement.GetAttribute("name"); // categoryId ise
                    var aria =wer.GetAttribute("value");
                    
                    var ariaaaa = wer.GetAttribute("aria-selected");
                    //foreach (var itemdf in wer)
                    //{
                    //    var att = itemdf.GetAttribute("label");
                    //}
                    //var name8 = itemSub.Attributes["label"];
                }



            }




            var objs = geckoWebBrowser1.Document.GetElementsByName("name");

            foreach (var item in objs)
            {
                var asds = item.OuterHtml;
            }


            var test = geckoWebBrowser1.Document.Body.InnerHtml;

            geckoWebBrowser1.Document.GetElementsByName("name")[1].RemoveAttribute("placeholder");

            var asdasd = geckoWebBrowser1.Document.GetHtmlElementById("name");

            var fdfd = geckoWebBrowser1.Document.GetElementsByName("name")[1].Attributes["value"].NodeValue;

            geckoWebBrowser1.Document.GetElementsByName("name")[1].SetAttribute("value", "attribute");


            geckoWebBrowser1.Document.GetElementsByName("name")[1].TextContent = "text";

            geckoWebBrowser1.Document.GetElementsByName("name")[1].InnerHtml = "InnerHtml";


            //var asdasd = geckoWebBrowser1.Document.GetElementsByName("name")[1].TextContent;

            //geckoWebBrowser1.Document.GetElementsByName("description")[1].Focus();

        }

        private void WaitDocumentComplated()
        {
            do
            {
                Application.DoEvents();

            } while (!IsPageLoaded);
        }

        private void SetImages(bool isWait = false)
        {

        }

        public void RunJavaScript(GeckoWebBrowser b, string script)
        {
            b.Navigate("javascript:void(" + script + ")");
            Application.DoEvents(); //review... is there a better way?  it seems that NavigationFinished isn't raised.

            geckoWebBrowser1.Navigate("javascript:void(document.getElementsByName('name')[1].value = 'eren'");
        }

        private void SetTitle(bool isWait = false)
        {
            
            GeckoInputElement input = (GeckoInputElement)geckoWebBrowser1.Document.GetElementsByName("name")[1];
            input.Focus();
            input.Click();
            input.Select();

            //Thread.Sleep(1000);

            //SendKeys.Send("eren");

            //input.Select();
            input.Value = "Set Title";
            Thread.Sleep(1000);
            input.Focus();
            input.Click();
            input.Select();

            

            nsAStringBase eventType = (nsAStringBase)new nsAString("keypress5");

            var ev = geckoWebBrowser1.Document.CreateEvent("HTMLEvents");
            ev.DomEvent.InitEvent(eventType, false, false);
            input.GetEventTarget().DispatchEvent(ev);
            //enquiryTypeCombo.GetEventTarget().DispatchEvent(ev);



            DomEventTarget events = input.GetEventTarget();
            

            //DomKeyEventArgs asd = DomKeyEventArgs.Create();

            //geckoWebBrowser1.Document.GetElementsByName("name")[1].SetAttribute("value", _product.Title);
            if (isWait) WaitDocumentComplated();
        }

        private void SetDescription(bool isWait = false)
        {
            GeckoTextAreaElement input = (GeckoTextAreaElement)geckoWebBrowser1.Document.GetElementsByName("description")[2];
            input.Focus();
            input.Value = "Set Description";

            SendKeys.Send(".");
            Thread.Sleep(1000);

            if (isWait) WaitDocumentComplated();
        }

        /// <summary>
        /// Category, SubCategory1, SubCategory2
        /// </summary>
        private void SetCategory(bool isWait = false)
        {
            GeckoSelectElement selectElement2 = (GeckoSelectElement)geckoWebBrowser1.Document.GetElementsByName("categoryId")[1];
            selectElement2.Focus();
            selectElement2.Click();
            Thread.Sleep(1000);


            var asdf = selectElement2.Options.Item(4);

            asdf.Click();

            //selectElement2.SelectedIndex = 4;

            if (isWait) WaitDocumentComplated();

            //if (_commerceType == ECommerceType.Mercari)
            //{

            //}
            //else
            //{
            //    var categoryList = geckoWebBrowser1.Document.GetElementsByName("categoryId");

            //    foreach (var item in categoryList)
            //    {
            //        var name = item.Attributes["label"].NodeName;
            //    }
            //}
        }

        /// <summary>
        /// Size, Brand, Product Condition
        /// </summary>
        private void SetProductDetails(bool isWait = false)
        {
            if (_commerceType == ECommerceType.Mercari)
            {
                //Brand
                GeckoInputElement objBrand = (GeckoInputElement)geckoWebBrowser1.Document.GetElementsByName("brandName")[0];
                objBrand.Value = "brand";
            }
            else
            {

               

                //Product Condition
                GeckoSelectElement objProductCondition = (GeckoSelectElement)geckoWebBrowser1.Document.GetElementsByName("itemCondition")[1];
                objProductCondition.SelectedIndex = 1;
            }
        }
        /// <summary>
        /// Shipping Charges, Shipping Area, Days to Ship
        /// </summary>
        private void SetDelivery(bool isWait = false)
        {
            if (_commerceType == ECommerceType.Mercari)
            {

            }
            else
            {
                //Shipping Charges
                GeckoSelectElement objShippingCharges = (GeckoSelectElement)geckoWebBrowser1.Document.GetElementsByName("shippingPayer")[1];
                objShippingCharges.SelectedIndex = 1;

                //Shipping Area
                GeckoSelectElement objShippingArea = (GeckoSelectElement)geckoWebBrowser1.Document.GetElementsByName("shippingFromArea")[1];
                objShippingArea.SelectedIndex = 1;

                //Days to Ship
                GeckoSelectElement objDays = (GeckoSelectElement)geckoWebBrowser1.Document.GetElementsByName("shippingDuration")[1];
                objDays.SelectedIndex = 3; // 4-7 day
            }
        }

        private void SetSellingPrice(bool isWait = false)
        {
            if (_commerceType == ECommerceType.Mercari)
            {

                GeckoInputElement ads = (GeckoInputElement)geckoWebBrowser1.Document.GetElementsByName("price")[1];
                ads.Value = "1231";
                
            }
        }

        private void ClickSell(bool isWait = false)
        {
            var className = "style_button__3yWFH common_fontFamily__3-3Si style_primary__Mg3zL style_medium__3wTQ5 style_fluid__3mdYA style_legacy__2D0U0";
            GeckoButtonElement obj = (GeckoButtonElement)geckoWebBrowser1.Document.GetElementsByClassName(className)[0];
            obj.Click();
        }

        #region WebBrowser Events

        private void geckoWebBrowser1_Navigating(object sender, Gecko.Events.GeckoNavigatingEventArgs e)
        {
            _url = e.Uri.AbsoluteUri;
            IsPageLoaded = false;
        }

        private void geckoWebBrowser1_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            IsPageLoaded = true;
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

        #endregion WebBrowser Events

        private void geckoWebBrowser1_DomKeyDown(object sender, DomKeyEventArgs e)
        {

        }

        private void geckoWebBrowser1_DomKeyPress(object sender, DomKeyEventArgs e)
        {

        }
    }
}