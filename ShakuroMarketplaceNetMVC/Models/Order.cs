using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string GoodIdArray { get; set; }
        public string GoodNameArray { get; set; }
        public string GoodLinkArray { get; set; }
        public string GoodImageUrlArray { get; set; }
        public string GoodQuantityArray { get; set; }
        public string GoodPriceArray { get; set; }
        public string GoodTotalPriceArray { get; set; }

        public double GoodsTotalPrice { get; set; }
        public double DeliveryPrice { get; set; }

        public string CustomerName { get; set; }
        public string CardOwnerName { get; set; }
        public string CardNumber { get; set; }
        public string CardMonth { get; set; }
        public string CardYear { get; set; }
        public int CardCvv { get; set; }
        public string BankSystem { get; set; }
        public string AddresseeFirstName { get; set; }
        public string AddresseeSecondName { get; set; }
        public string AddresseeCountry { get; set; }
        public string AddresseeRegion { get; set; }
        public string AddresseeCity { get; set; }
        public int AddresseeIndex { get; set; }
        public string AddresseeStreetAddress { get; set; }
        public string AddresseePhoneNumber { get; set; }
        public string AddresseeEmail { get; set; }
        public string DeliveryMethods { get; set; }
    }
}