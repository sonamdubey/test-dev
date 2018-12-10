using Carwale.Entity.CarData;
using System.Collections.Generic;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Dealers;
using Carwale.Entity;
using Carwale.Entity.Offers;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.Deals;

namespace Carwale.DTOs.PriceQuote
{
    /// <summary>
    /// Dto For MObileWeb PriceQuote
    /// Written By : Ashish Verma on 2/6/2014
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public class PQMobile
    {
        public ulong PQId { get; set; }
        public string EnId { get; set; }
        public List<PQItem> PriceQuoteList { get; set; }
        public SponsoredDealer SponsoredDealer { get; set; }
        public PQCarDetails CarDetails { get; set; }
        public CustLocation CityDetail { get; set; }
        public PQOfferEntity Offers { get; set; }
        public bool IsSponsoredCar { get; set; }
        public DiscountSummary DiscountSummaryDetails { get; set; }
        public ulong LinkedSponsoredCar { get; set; }
        public bool ShowInsuranceLink { get; set; }
        public bool ShowSellCarLink { get; set; }
        public bool ShowHdfcFinanceLink { get; set; }
    }
}
