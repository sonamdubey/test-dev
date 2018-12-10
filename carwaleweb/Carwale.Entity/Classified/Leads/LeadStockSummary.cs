using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.Leads
{
    public class LeadStockSummary
    {
        public string ProfileId { get; set; }
        public int InquiryId { get; set; }
        public bool IsDealer { get; set; }
        public string CarName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Price { get; set; }
        public string Kilometers { get; set; }
        public string RatingText { get; set; }
        public int CityId { get; set; }
        public string Color { get; set; }
        public string FuelName { get; set; }
        public string MainImageUrl { get; set; }
        public int CtePackageId { get; set; }
    }
}
