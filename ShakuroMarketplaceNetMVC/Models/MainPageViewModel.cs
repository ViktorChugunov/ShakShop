using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class MainPageViewModel
    {
        public List<GoodViewModel> InterestingGoodsList { get; set; }
        public List<GoodViewModel> ViewedGoodsList { get; set; }
    }
}