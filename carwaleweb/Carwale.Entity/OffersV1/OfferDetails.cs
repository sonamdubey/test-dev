using Carwale.Entity.CarData;
using System;
using Carwale.Entity.Common;

namespace Carwale.Entity.OffersV1
{
    public class OfferDetails
    {
        public string Heading { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CallOutLine { get; set; }
        public string Disclaimer { get; set; }
        public bool IsExtended { get; set; }
    }
}
