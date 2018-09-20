using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class Good
    {
        public int Id { get; set; }
        public string GoodName { get; set; }
        public string GoodBrand { get; set; }
        public string GoodUrl { get; set; }
        public string GoodColor { get; set; }
        public string GoodImagesUrls { get; set; }
        public int GoodPrice { get; set; }    
        public int GoodSubcategoryId { get; set; }
        public bool NewGood { get; set; }
        public bool SalesGood { get; set; }
        public bool RecommendedGood { get; set; }
        public string Characteristics { get; set; }

        public virtual GoodSubcategory GoodSubcategory { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Discussion> Discussions { get; set; }
        public virtual ICollection<Overview> Overviews { get; set; }
    }
}