using Bikewale.ManufacturerCampaign.Entities;
using System.Web.Mvc;

namespace Bikewaleopr.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Modified By : Prabhu Puredla
    /// Description : Changed EmailRequired property to enum type
    /// </summary>
    /// <param name="objCampaign"></param>
    public class ManufacturerCampaignPopup
    {
        public uint CampaignId { get; set; }
        public string PopupHeading { get; set; }
        public string PopupDescription { get; set; }
        [AllowHtml]
        public string PopupSuccessMessage { get; set; }

        public EnumEmailOptions EmailOption { get; set; }
        public bool PinCodeRequired { get; set; }
        public bool DealerRequired { get; set; }
        public uint DealerId { get; set; }
        
    }
}
