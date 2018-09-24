using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class DiscussionGood
    {
        public int Id { get; set; }
        public int Discussion_Id { get; set; }
        public int Good_Id { get; set; }        
    }
}