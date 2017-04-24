using System;

namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    public class OtherVersionInfoEntity
    {
        public uint VersionId { get; set; }
        public string VersionName { get; set; }
        public ulong OnRoadPrice { get; set; }
        public UInt32 Price { get; set; }
        public UInt32 RTO { get; set; }
        public UInt32 Insurance { get; set; }
    }
}
