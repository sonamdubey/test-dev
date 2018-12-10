using Newtonsoft.Json;
using System;

namespace Carwale.Entity.PriceQuote
{
    [JsonObject]
    [Serializable]
    public class LeadSource
    {
        public string LeadClickSourceDesc { get; set; }
        public int LeadClickSourceId { get; set; }
        public int InquirySourceId { get; set; }
        public short AdType { get; set; }
        public short PlatformId { get; set; }
        public short ApplicationId { get; set; }
        public string OriginalLeadId { get; set; }
        public short PageId { get; set; }
        public short PropertyId { get; set; }
        public short SourceType { get; set; }
        public bool IsCitySet { get; set; }
    }
}
