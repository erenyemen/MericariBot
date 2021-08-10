using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MericariBot.Models
{
    public class Product
    {
        public List<string> ImagesPath { get; set; }
        public string Name { get; set; } //ProductTitle 40 karakter

        public string Description { get; set; }

        public Category ProductCategory { get; set; }

        public string Size { get; set; }

        public string Brand { get; set; }

        public string Condition { get; set; }

        public string ShippingCharges { get; set; }

        public string ShippingArea { get; set; }

        public string DaysToShip { get; set; }

        public string SellingPrice { get; set; }
    }

    public class Category
    {

    }
}
