using System;

namespace Bikewale.Entities.BikeBooking.v2
{
    /// <summary>
    /// Created By : Pratibha Verma on 26 June 2018
    /// Description: changes PQId data type and added cityId, VersionId, LeadId to remove PQId dependency
    /// </summary>
    public class DPQ_SaveEntity
    {
        public uint DealerId { get; set; }
        public string PQId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public uint? ColorId { get; set; }
        public UInt16? LeadSourceId { get; set; }
        public string UTMA { get; set; }
        public string UTMZ { get; set; }
        public string DeviceId { get; set; }
        public float SpamScore { get; set; }
        public string RejectionReason { get; set; }
        public bool IsAccepted { get; set; }
        public ushort OverallSpamScore { get; set; }
        public uint CityId { get; set; }
        public uint VersionId { get; set; }
        public uint LeadId { get; set; }
        public uint AreaId { get; set; }
        public UInt16? PlatformId { get; set; }
        public string ClientIP { get; set; }
    }
}