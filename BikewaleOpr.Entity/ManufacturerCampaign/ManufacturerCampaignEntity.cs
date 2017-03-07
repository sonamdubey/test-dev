
namespace BikewaleOpr.Entities.ManufacturerCampaign
{
    /// <summary>
    /// Created by : Sajal Gupta on 30/08/2016
    /// Description : This entity holds data that is returned by fetchcampaigndetails method;
    ///  Modified By :- Subodh Jain 28/02/2017
    /// Description :- Added LeadCapturePopupHeading, LeadCapturePopupDescription, LeadCapturePopupMessage
    /// </summary>
    public class ManufacturerCampaignEntity
    {
        public string CampaignDescription;
        public string CampaignMaskingNumber;
        public int IsActive;
        public string TemplateHtml;
        public int PageId;
        public int IsDefault;
        public int TemplateId;
        public string LeadCapturePopupHeading;
        public string LeadCapturePopupDescription;
        public string LeadCapturePopupMessage;
    }
}



