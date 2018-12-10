using AutoBiz.DTOs.Deals.Stock;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Deals;
using System.Collections.Generic;

namespace Carwale.DTOs.Deals
{
    public class ProductDetailsDTO_Desktop
    {
        public List<City> CitiesList { get; set; }
        public DealsInquiryDetailDTO DealsInquiryDetails { get; set; }
        public List<DealsRecommendationDTO> DealsRecommedations { get; set; }
        public List<DealsStockDTO> VersionsDeals { get; set; }
        public int DealsCount { get; set; }
        public List<DealsStock> PQUserHistoryDeals { get; set; }
        public List<DealsStock> AdvUserHistoryDeals { get; set; }
        public List<DealsSummaryDesktop_DTO> BestSavingDeals { get; set; }
        public DealsStockDTO OfferOfTheWeek { get; set; }
        public List<DealsTestimonialDTO> Testimonials { get; set; }
    }
}
