using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Offers
{
    public class  OffersDTO
    {
        public int OfferId { get; set; }
        public int DealerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImg { get; set; }
        public string ExpiryDate { get; set; }
        public string ShortDescription { get; set; }   
        public int AvailabilityCount { get; set; }
    }
}
