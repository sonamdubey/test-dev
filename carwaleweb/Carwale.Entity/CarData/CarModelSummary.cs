using Carwale.Entity.Deals;
using Carwale.Entity.PriceQuote;
using System;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CarModelSummary
    {
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
        public double ModelRating { get; set; }
        public double FuelEconomy { get; set; }
        public double ValueForMoney { get; set; }
        public double Comfort { get; set; }
        public double Performance { get; set; }
        public double Looks { get; set; }
        public int ReviewCount { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public int MinAvgPrice { get; set; }
        public string ModelImage { get; set; }
        public string LargeImage { get; set; } // added by Sanjay
        public string XLargeImage { get; set; } // added by rohan 7/4/2015
        public string HostUrl { get; set; }
        public string OriginalImage { get; set; }
        public int ModelId { get; set; }
        public bool New { get; set; }
        public bool OfferExists { get; set; }
        public int MakeId { get; set; }
        public DealsStock DiscountSummary { get; set; }
        public PriceOverview CarPriceOverview { get; set; }
        public int BodyStyleId { get; set; }   
        public DateTime LaunchDate { get; set; }
	}
}
