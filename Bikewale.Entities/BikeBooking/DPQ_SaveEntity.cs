using System;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Created By : Sumit Kate on 29 Dec 2015
    /// Modified by : Snehal Dange on 14th May 2018
    /// Description:  Added SpamScore, RejectionReason and IsAccepted.
    /// </summary>
    public class DPQ_SaveEntity
    {
        public uint DealerId { get; set; }
        public uint PQId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public uint? ColorId { get; set; }
        //Added By  : Sadhana Upadhyay on 29 Dec 2015
        public UInt16? LeadSourceId { get; set; }
        public string UTMA { get; set; }
        public string UTMZ { get; set; }
        public string DeviceId { get; set; }
        public float SpamScore { get; set; }
        public string RejectionReason { get; set; }
        public bool IsAccepted { get; set; }
        public ushort OverallSpamScore { get; set; }
        public UInt16? PlatformId { get; set; }
        public string ClientIP { get; set; }
    }
}