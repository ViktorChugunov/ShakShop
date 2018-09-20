using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class Overview
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public virtual ICollection<Good> Goods { get; set; }
    }
}