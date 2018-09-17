using Bikewale.Entities.Dealer;
using Bikewale.Entities.Location;
using System.Collections.Generic;

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
        public bool IsMLAActive { set; get; }
        public ushort MlaLeadSourceId { get; set; }
        public IEnumerable<SecondaryDealerBase> MLADealers { set; get; }
        public ushort PageId { get; set; }
    }
}
