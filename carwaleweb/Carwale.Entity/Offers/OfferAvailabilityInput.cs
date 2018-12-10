using System.Collections.Generic;

namespace Carwale.Entity.Offers
{
    public class OfferAvailabilityInput
    {
        public int CityId { get; set; }
        public int StateId { get; set; }
        public List<int> ModelIds { get; set; }
    }
}
