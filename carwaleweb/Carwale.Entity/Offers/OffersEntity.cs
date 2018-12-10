using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Offers
{
    //added by ashish Verma
    [Serializable]
    public class OffersEntity : OfferMakeModelCity
    {
        public int OfferId { get; set; }
        public int DealerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AvailabilityCount { get; set; }
        public string Image { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImg { get; set; }
        public string TermsAndCondition { get; set; }
        public string ExpiryDate { get; set; }
        public int OfferType { get; set; }
        public string DealerName { get; set; }
        public int SourceCategory { get; set; } //added by ashish Verma
        public string ShortDescription { get; set; } //added by Piyush Sahu   
        public DateTime StartDate { get; set; }
    }
}
