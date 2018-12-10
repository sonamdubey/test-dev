using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class ProductDetailsDTO_Msite
    {
        public List<City> CitiesList { get; set; }
        public DealsInquiryDetailDTO DealsInquiryDetails { get; set; }
        public List<DealsRecommendationDTO> DealsRecommedations { get; set; }
        public List<DealsStockDTO> AllVersionList { get; set; }
        public int DealsCount { get; set; }
        public DealsStockDTO OfferOfTheWeek { get; set; }
        public List<DealsTestimonialDTO> Testimonials { get; set; }
    }
}
