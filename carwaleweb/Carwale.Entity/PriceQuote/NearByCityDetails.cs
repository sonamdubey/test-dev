using System.Collections.Generic;

namespace Carwale.Entity.PriceQuote
{
    public class NearByCityDetails
    {
        public List<NearByCity> Cities { get; }
        public string WidgetHeading { get; set; }

        public NearByCityDetails()
        {
            Cities = new List<NearByCity>();
        }
    }
}
