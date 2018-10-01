using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class GoodSearchResult
    {
        public string GoodName { get; set; }
        public string GoodBrand { get; set; }
        public string GoodImageUrl { get; set; }
        public string GoodPageLink { get; set; }
        public string GoodColor { get; set; }
        public double GoodPrice { get; set; }
    }
}