
namespace PriceQuoteLeadSMS
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 30 Nov 2015
    /// Summary : To store price quote lead notification details
    /// Modified by :   Sumit Kate on 14 Jan 2016
    /// Summary     :   Added Customer Id
    /// Modified by :   Sumit Kate on 29 Mar 2016
    /// Summary     :   Added CampaignId
    /// </summary>
    public class LeadNotificationEntity
    {
        public uint PQId { get; set; }
        public uint DealerId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerName { get; set; }
        public uint BikeVersionId { get; set; }
        public uint CityId { get; set; }
        public string SMSToCustomerMessage { get; set; }
        public string SMSToCustomerNumbers { get; set; }
        public ushort SMSToCustomerServiceType { get; set; }
        public string SMSToCustomerPageUrl { get; set; }
        public string EmailToCustomerMessageBody { get; set; }
        public string EmailToCustomerSubject { get; set; }
        public string EmailToCustomerReplyTo { get; set; }
        public string SMSToDealerMessage { get; set; }
        public string SMSToDealerNumbers { get; set; }
        public ushort SMSToDealerServiceType { get; set; }
        public string SMSToDealerPageUrl { get; set; }
        public string EmailToDealerMessageBody { get; set; }
        public string EmailToDealerSubject { get; set; }
        public string EmailToDealerReplyTo { get; set; }
        public ulong CustomerId { get; set; }
        public string CampaignId { get; set; }
    }
}
