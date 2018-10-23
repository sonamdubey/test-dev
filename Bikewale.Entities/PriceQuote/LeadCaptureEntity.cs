using Bikewale.Entities.Dealer;
using Bikewale.Entities.Location;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Author  :   Unknown
    /// Modifier:   Kartik on 18 oct 2018
    /// Desc    :   Added OfferList,OfferCount,ShowOffersOnLeadPage to get offers on lead capture popup
    /// </summary>
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
        public IEnumerable<OfferEntityBase> OfferList { get; set; }
        public uint OfferCount { get { return OfferList != null ? (uint)OfferList.Count() : 0; } }
        public bool ShowOffersOnLeadPage { get; set; }
    }
}
