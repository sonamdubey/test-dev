using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Offers
{
   public class OfferBookedCarInfo: OfferDealerDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string CarName { get; set; }
        public string OfferTitle { get; set; }
        public string OfferDesc { get; set; }
    }
}
