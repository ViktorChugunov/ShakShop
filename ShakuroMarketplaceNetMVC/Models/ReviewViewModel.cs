using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class ReviewViewModel
    {
        public double GoodRating;
        public int[] ReviewsRationList;
        public List<Review> GoodReviewsList;        
    }
}