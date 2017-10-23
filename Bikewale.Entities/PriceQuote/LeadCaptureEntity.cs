using Bikewale.Entities.Location;

namespace Bikewale.Entities.PriceQuote
{
    public class LeadCaptureEntity : GlobalCityAreaEntity
    {
        public uint ModelId { get; set; }
        public string BikeName { get; set; }
        public string Location { get; set; }
        public bool IsManufacturerCampaign { get; set; }
        public bool IsAmp { get; set; }
        public bool IsApp { get; set; }
        public ushort PlatformId { get; set; }
        public string ManufacturerLeadAdAMPConvertedContent { get; set; }
    }
}
