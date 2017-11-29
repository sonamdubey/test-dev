using System.Web.Mvc;

namespace Bikewaleopr.ManufacturerCampaign.Entities
{
    public class ManufacturerCampaignPopup
    {
        public uint CampaignId { get; set; }
        public string PopupHeading { get; set; }
        public string PopupDescription { get; set; }
        [AllowHtml]
        public string PopupSuccessMessage { get; set; }

        public bool EmailRequired { get; set; }
        public bool PinCodeRequired { get; set; }
        public bool DealerRequired { get; set; }
        public uint DealerId { get; set; }
        
    }
}
