using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class GoodCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryUrl { get; set; }
        public string CategoryImageUrl { get; set; }

        public virtual ICollection<GoodSubcategory> GoodSubcategories { get; set; }
    }
}