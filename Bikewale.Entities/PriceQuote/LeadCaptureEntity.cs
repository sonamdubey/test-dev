using Bikewale.Entities.Location;

namespace Bikewale.Entities.PriceQuote
{
    public class LeadCaptureEntity : GlobalCityAreaEntity
    {
        public uint ModelId { get; set; }
        public string BikeName { get; set; }
        public string Location { get; set; }
    }
}
