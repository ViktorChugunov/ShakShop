using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class CartGoodViewModel
    {
        public int Id { get; set; }
        public string GoodName { get; set; }
        public string GoodBrand { get; set; }
        public string GoodCategoryUrl { get; set; }
        public string GoodSubcategoryUrl { get; set; }
        public string GoodUrl { get; set; }
        public string GoodColor { get; set; }
        public string GoodImagesUrls { get; set; }
        public int GoodPrice { get; set; }
        public bool SalesGood { get; set; }
    }
}