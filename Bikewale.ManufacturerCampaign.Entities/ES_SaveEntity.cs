﻿using System;

namespace Bikewale.ManufacturerCampaign.Entities
{
    public class ES_SaveEntity
    {
        public uint DealerId { get; set; }
        public uint PQId { get; set; }
        public UInt64 CustomerId { get; set; }
        public String CustomerName { get; set; }
        public String CustomerEmail { get; set; }
        public String CustomerMobile { get; set; }
        public uint LeadSourceId { get; set; }
        public String UTMA { get; set; }
        public String UTMZ { get; set; }
        public String DeviceId { get; set; }
        public uint CampaignId { get; set; }
        public uint LeadId { get; set; }
        public float SpamScore { get; set; }
        public String Reason { get; set; }
        public bool IsAccepted { get; set; }
        public ushort OverallSpamScore { get; set; }
        public uint CityId { get; set; }
        public uint VersionId { get; set; }
        public string PQGUId{ get; set; }
    }
}
