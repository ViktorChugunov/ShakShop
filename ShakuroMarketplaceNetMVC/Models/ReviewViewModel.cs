using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class ReviewViewModel
    {
        public double goodRating;
        public int[] reviewsNumberList;
        public List<Review> goodReviewsList;        
    }
}