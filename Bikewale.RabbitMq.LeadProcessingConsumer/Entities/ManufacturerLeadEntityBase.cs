namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Jul 2017
    /// Description :   This serves as base class for All lead entity
    /// </summary>
    internal class ManufacturerLeadEntityBase
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public ushort RetryAttempt { get; set; }
        public uint DealerId { get; set; }
        public uint LeadId { get; set; }
        public uint CampaignId { get; set; }
        public string InquiryJSON { get; set; }
        public uint VersionId { get; set; }
        public uint CityId { get; set; }
        public uint PinCodeId { get; set; }
        public uint ManufacturerDealerId { get; set; }
        public string BikeName { get; set; }
        public string DealerName { get; set; }
        public bool SendLeadSMSCustomer { get; set; }
    }
}
