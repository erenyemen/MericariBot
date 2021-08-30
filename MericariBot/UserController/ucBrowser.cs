using Gecko;
using HtmlAgilityPack;
using MericariBot.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace MericariBot.UserController
{
    public partial class ucBrowser : UserControl
    {
        #region Properties

        private string _url { get { return GetUrl(); } set { textBox1.Text = value; } }
        public ECommerceType _commerceType { get; private set; }
        private Product _product { get; set; }

        public bool IsPageLoaded = false;

        public string _mercariSellUrl { get; set; }

        #endregion Properties

        #region Constructor

        public ucBrowser(ECommerceType commerceType)
        {
            _commerceType = commerceType;
            PromptFactory.PromptServiceCreator = () => new FilteredPromptService();
        }

        public ucBrowser(Product product, ECommerceType commerceType, string mercariSellUrl = null)
        {
            _commerceType = commerceType;
            _product = product;
            _mercariSellUrl = mercariSellUrl;

            PromptFactory.PromptServiceCreator = () => new FilteredPromptService();
        }

        public void Initialize()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Methods

        private string GetUrl()
        {
            switch (_commerceType)
            {
                case ECommerceType.Amazon: return "https://www.amazon.co.jp/";
                case ECommerceType.Rakuten: return "https://www.rakuten.co.jp/";
                case ECommerceType.Mercari: return "https://www.mercari.com/jp/";
                case ECommerceType.MercariSell:
                    return string.IsNullOrEmpty(_mercariSellUrl) ? "https://www.mercari.com/jp/mypage/drafts" : _mercariSellUrl;
                default: return string.Empty;
            }
        }

        public void OpenPage()
        {
            geckoWebBrowser1.Navigate(_url);

            if (_commerceType == ECommerceType.MercariSell)
            {
                if (string.IsNullOrEmpty(_mercariSellUrl))
                {
                    WaitDocumentComplated();
                    
                    //RunJavaScript("document.getElementsByClassName('style_listlink__2YdMK sc-cfWELz jRintt')[0].click()");

                    var ele = geckoWebBrowser1.Document.GetElementsByTagName("a").FirstOrDefault(x => x.ClassName.Contains("style_listlink__2YdMK sc-"));
                    ele.Click();
                }
            }
        }

        public void ViewDraftProduct()
        {
            WaitDocumentComplated();

            //RunJavaScript("document.getElementsByClassName('style_listlink__2YdMK sc-cfWELz jRintt')[0].click()");

            //var ele = geckoWebBrowser1.Document.GetElementsByTagName("a").FirstOrDefault(x => x.ClassName == "style_listlink__2YdMK sc-cfWELz jRintt");

            var ele = geckoWebBrowser1.Document.GetElementsByTagName("a").FirstOrDefault(x => x.ClassName.Contains("style_listlink__2YdMK sc-"));

            ele.Click();
        }

        private void WaitDocumentComplated()
        {
            do
            {
                Application.DoEvents();
            } while (!IsPageLoaded);
        }

        public void RunJavaScript(string script)
        {
            geckoWebBrowser1.Navigate("javascript:void(" + script + ")");
            Application.DoEvents();
        }

        public bool ReaddClick()
        {
            bool isButtonClick = false;

            var elements = geckoWebBrowser1.Document.GetElementsByTagName("button");

            foreach (var item in elements)
            {
                if (item.ClassName == "btn-default btn-gray")
                {
                    if (item.TextContent.ToString().Trim() == "出品を一旦停止する")
                    {
                        item.Click();
                        isButtonClick = true;
                        break;
                    }
                }
            }

            return isButtonClick;
        }

        #region Get Product Methods

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
            int imageCount = SelectImagesFromAmazon();

            Product result = new Product()
            {
                ImagesUrl = GetImagesFromAmazon(doc, imageCount),
                Title = GetTitleFromAmazon(),
                Description = GetDescriptionFromAmazon(doc)
            };

            return result;
        }

        private int SelectImagesFromAmazon()
        {
            int result = 0;

            try
            {
                var imageElements = geckoWebBrowser1.Document.GetElementsByTagName("li").Where(x => x.ClassName == "a-spacing-small item imageThumbnail a-declarative");

                foreach (var item in imageElements)
                {
                    item.Click();
                }

                result = imageElements.Count();
            }
            catch (Exception ex)
            {
                //TODO: test edildikten sonra kaldırılacak.
            }

            return result;
        }

        private string GetTitleFromAmazon()
        {
            string result = geckoWebBrowser1.Document.GetHtmlElementById("productTitle").InnerHtml.Replace("\n", "");

            string res = string.Empty;
            for (int i = 0; i < result.Length; i++)
            {
                res = res + result[i];

                if (i >= 39) break;
            }

            return res;
        }

        private string GetDescriptionFromAmazon(HtmlAgilityPack.HtmlDocument doc)
        {
            string result = string.Empty;

            HtmlNode specificNode = doc.GetElementbyId("feature-bullets");

            if (!(specificNode.ChildNodes.Where(x => x.Name == "ul") == null || specificNode.ChildNodes.Where(x => x.Name == "ul").Count() == 0))
            {
                var nodes = specificNode.ChildNodes.Where(x => x.Name == "ul").First().ChildNodes.Where(x => x.Name == "li");

                StringBuilder sb = new StringBuilder();
                foreach (var item in nodes)
                {
                    if (string.IsNullOrEmpty(item.Id))
                        sb.Append(item.InnerText);
                }

                result = sb.ToString();


                if (result.Length > 1000)
                {
                    string res = string.Empty;
                    for (int i = 0; i < result.Length; i++)
                    {
                        res = res + result[i];

                        if (i >= 999) break;
                    }

                    return res;
                }
            }

            return result;
        }

        private List<string> GetImagesFromAmazon(HtmlAgilityPack.HtmlDocument doc, int count)
        {
            if (count == 0)
            {
                return new List<string>();
            }
            List<string> result = new List<string>();
            
            doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(geckoWebBrowser1.Document.Body.OuterHtml);

            HtmlNode specificNode = doc.GetElementbyId("main-image-container");
            var nodes = specificNode.ChildNodes.FirstOrDefault(x => x.Name == "ul").ChildNodes.Where(x => x.Name == "li");

            foreach (var nodeItem in nodes)
            {
                var attribute = nodeItem.Attributes["class"];

                if (attribute == null || !attribute.Value.Contains("image")) continue;

                var imageNode = nodeItem.ChildNodes.FirstOrDefault().ChildNodes["span"].ChildNodes["div"].ChildNodes["img"].Attributes["src"];

                result.Add(imageNode.Value);
            }

            if (result.Count() != count)
            {
                result = GetImagesFromAmazon(doc, count);
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
                ImagesUrl = GetImagesFromRakuten(doc)
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
            WaitDocumentComplated();

            SelectImagesFromMercari();

            Product result = new Product()
            {
                Title = GetTitleFromMercari(doc),
                Description = GetDescriptionFromMercari(doc),
                ImagesUrl = GetImagesFromMercari(doc),
                SellingPrice = GetSellingPriceFromMercari(doc)
            };

            GetCategoryFromMercari(doc, result);


            return result;
        }

        private void SelectImagesFromMercari()
        {
            try
            {
                var imageElements = geckoWebBrowser1.Document.GetElementsByTagName("div").Where(x => x.ClassName == "owl-dot");

                int count = imageElements.Count();

                foreach (var item in imageElements)
                {
                    item.Click();
                    //Application.DoEvents();
                }

                //bool flag = true;
                //int imagecount = 0;
                //do
                //{
                //    imagecount = 0;
                //    var outherHtml = new HtmlAgilityPack.HtmlDocument();
                //    outherHtml.LoadHtml(geckoWebBrowser1.Document.Body.OuterHtml);

                //    var nodes = outherHtml.DocumentNode.SelectNodes("//div[@class='owl-stage-outer']")[0].InnerHtml;

                //    HtmlAgilityPack.HtmlDocument imgDoc = new HtmlAgilityPack.HtmlDocument();
                //    imgDoc.LoadHtml(nodes);

                //    var imgNodes = imgDoc.DocumentNode.SelectNodes("//img");

                //    foreach (var item in imgNodes)
                //    {
                //        var isAttribute = item.Attributes["src"];

                //        if (isAttribute != null)
                //        {
                //            imagecount += 1;
                //        }
                //    }

                //    if (imagecount == count + 1)
                //    {
                //        flag = false;
                //    }

                //    Application.DoEvents();

                //} while (flag);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "SelectImagesFromAmazon");
            }
           
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
            doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(geckoWebBrowser1.Document.Body.OuterHtml);

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
                    product.SubCategory1.Name = itemValue[1].InnerText.Replace("\n", "").Trim();
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

        #endregion Get Product Methods

        #endregion Methods

        #region Events

        #region Button Events

        private void btnBackward_Click(object sender, EventArgs e)
        {
            geckoWebBrowser1.GoBack();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            geckoWebBrowser1.GoForward();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            geckoWebBrowser1.Refresh();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            geckoWebBrowser1.Navigate(_url);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            geckoWebBrowser1.Navigate(textBox1.Text);
        }

        #endregion Button Events

        #region WebBrowser Events

        private void geckoWebBrowser1_Navigating(object sender, Gecko.Events.GeckoNavigatingEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            _url = e.Uri.AbsoluteUri;
            IsPageLoaded = false;
        }

        private void geckoWebBrowser1_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            IsPageLoaded = true;
        }

        private void geckoWebBrowser1_CreateWindow(object sender, GeckoCreateWindowEventArgs e)
        {
            e.WebBrowser = geckoWebBrowser1;
        }

        #endregion WebBrowser Events

        #endregion Events
    }
}