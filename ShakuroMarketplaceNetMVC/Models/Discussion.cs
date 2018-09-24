using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class Discussion
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatarSrc { get; set; }
        public bool FirstDiscussionMessage { get; set; }
        public int DiscussionGroup { get; set; }

        public virtual ICollection<Good> Goods { get; set; }
    }
}