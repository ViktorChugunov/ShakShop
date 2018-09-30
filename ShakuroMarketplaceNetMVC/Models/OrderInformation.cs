using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class OrderInformation
    {
        public int[] goodIdArray { get; set; }
        public string[] goodNameArray { get; set; }
        public string[] goodLinkArray { get; set; }
        public string[] goodImageUrlArray { get; set; }
        public int[] goodQuantityArray { get; set; }

        public double[] goodPriceArray { get; set; }
        public double[] goodTotalPriceArray { get; set; }
        public double goodsTotalPrice { get; set; }
        public double deliveryPrice { get; set; }
        
        public string customerName { get; set; }
        public string cardOwnerName { get; set; }
        public string cardNumber { get; set; }
        public string cardMonth { get; set; }
        public string cardYear { get; set; }
        public int cardCvv { get; set; }
        public string bankSystem { get; set; }
        public string addresseeFirstName { get; set; }
        public string addresseeSecondName { get; set; }
        public string addresseeCountry { get; set; }
        public string addresseeRegion { get; set; }
        public string addresseeCity { get; set; }
        public int addresseeIndex { get; set; }
        public string addresseeStreetAddress { get; set; }
        public string addresseePhoneNumber { get; set; }
        public string addresseeEmail { get; set; }
        public string deliveryMethods { get; set; }         
    }
}