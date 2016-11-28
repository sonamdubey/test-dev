
namespace Bikewale.NoPhotoListingsCommunication
{
    /// <summary>
    /// Created By:-Subodh Jain 24 Nov 2016
    /// summary:- For photo upload sms and mail
    /// </summary>
    public class NoPhotoSMSEntity
    {
        public string CustomerName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string InquiryId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerNumber { get; set; }
    }
}
