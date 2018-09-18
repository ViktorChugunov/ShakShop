using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class GoodSubcategory
    {
        public int Id { get; set; }
        public string SubcategoryName { get; set; }
        public string SubcategoryUrl { get; set; }
        public string SubcategoryImageUrl { get; set; }
        public int GoodCategoryId { get; set; }
        
        public virtual GoodCategory GoodCategory { get; set; }
        public virtual ICollection<Good> Goods { get; set; }
    }
}