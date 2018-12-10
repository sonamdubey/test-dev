using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Offers
{
    public class OfferPGAndroid
    {
        public int PQId { get; set; }
        public string CustName{get;set;}
        public string CustEmail { get; set; }
        public string MobileNo { get; set; }
        public long ResponseId { get; set; }
        public string CityName { get; set; }
        public string OfferDesc { get; set; }
        public string CarName { get; set; }
        public string CarImage { get; set; }
    }
}
