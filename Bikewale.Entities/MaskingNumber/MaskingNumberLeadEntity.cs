using Bikewale.Entities.Dealer;
using System.Collections.Generic;

namespace Bikewale.Entities.MaskingNumber
{
    /// <summary>
    /// Author  : Kartik Rathod on 20 nov 2019
    /// Desc    : Making number related entity
    /// </summary>
    public class MaskingNumberLeadEntity
    {
        public uint DealerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public uint CityId { get; set; }
        public uint CampaignId { get; set; }
        public string Comments { get; set; }
        public uint ModelId { get; set; }
        public uint VersionId { get; set; }
        public string PqGuId { get; set; }
        public uint LeadId { get; set; }
        public float SpamScore { get; set; }
        public bool IsAccepted { get; set; }
        public ushort OverallSpamScore { get; set; }
        public uint CustomerId { get; set; }
        public LeadTypeEnum LeadTypeId { get; set; }
        public ushort InquirySource { get; set; }
        public IDictionary<string, string> Others { get; set; }
    }
}
