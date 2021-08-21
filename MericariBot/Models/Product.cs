using System.Collections.Generic;
using System.Drawing;

namespace MericariBot.Models
{
    public class Product
    {
        public Product()
        {
            Category = new Category();
            SubCategory1 = new Category();
            SubCategory2 = new Category();
            ImagesPath = new List<string>();
        }

        public List<Image> ImageList { get; set; }

        public List<string> ImagesUrl { get; set; }

        public List<string> ImagesPath { get; set; }

        public string Title { get; set; } //ProductTitle 40 karakter

        public string Description { get; set; }

        public Category Category { get; set; }

        public Category SubCategory1 { get; set; }

        public Category SubCategory2 { get; set; }

        public string Size { get; set; }

        public string Brand { get; set; }

        public string Condition { get; set; }

        public string ShippingCharges { get; set; }

        public string ShippingArea { get; set; }

        public string DaysToShip { get; set; }

        public string SellingPrice { get; set; }
    }
}
