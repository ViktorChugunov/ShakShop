using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class DiscussionViewModel
    {
        public int todayReviewsNumber { get; set; }
        public int lastWeekReviewsNumber { get; set; }
        public int lastMonthReviewsNumber { get; set; }
        public int lastYearReviewsNumber { get; set; }
        public int allReviewsNumber { get; set; }
        public IEnumerable<IGrouping<int, Discussion>> discussions { get; set; }
    }
}