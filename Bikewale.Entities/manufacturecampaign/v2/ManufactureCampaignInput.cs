
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.Entities.BikeBooking;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Entities.manufacturecampaign.v2
{
    /// <summary>
    /// Created by  :   Pratibha Verma on 19 June 2018
    /// Description :   changes PQId data type
    /// Modified by :   Sanjay George on 18 Oct 2018
    /// Description :   Added FloatingBtnLeadSourceId to pass correct lead source to partial view
    /// Modified By : Prabhu Puredla on 24 oct 2018
    /// Description : Added  IsTopSellingPitch property
    /// Modified by : Pratibha Verma on 10 October 2018
    /// Description : Added OffersList property
    /// Modified by : Pratibha Verma on 22 November 2018
    /// Description : Added TopCardABTest and PageId properties for AB test
    /// </summary>
    public class ManufactureCampaignLeadEntity : ManufacturerCampaignLeadConfiguration
    {
        public int LeadSourceId { get; set; }
        public int PqSourceId { get; set; }
        public string MakeName { get; set; }
        public string Area { get; set; }
        public string GAAction { get; set; }
        public string GACategory { get; set; }
        public string GALabel { get; set; }
        public string PQId { get; set; }
        public uint VersionId { get; set; }
        public string PageUrl { get; set; }
        public string CurrentPageUrl { get; set; }
        public ushort PlatformId { get; set; }
        public string BikeName { get; set; }
        public bool IsAmp { get; set; }
        public uint LoanAmount { get; set; }
        public LeadSourceEnum FloatingBtnLeadSourceId {get; set;}
        public bool IsTopSellingPitch { get; set; }
        public string TopSellingPitchText { set; get; }
        public IEnumerable<string> OffersList { get; set; }
        public uint OfferCount { get { return OffersList != null ? (uint)OffersList.Count() : 0; } }
        public ESTopCardABTest TopCardABTest { get; set; }
        public ManufacturerCampaignServingPages PageId { get; set; }
    }
}
