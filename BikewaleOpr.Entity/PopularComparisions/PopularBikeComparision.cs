using System;

namespace BikewaleOpr.Entities.PopularComparisions
{
    /// <summary>
    /// Modified by Sajal Gupta on 02-06-2017
    /// Description : Added IsSponsored, SponsoredStartDate, SponsoredEndDate
    /// </summary>
    public class PopularBikeComparision
    {
        public uint ComparisionId { get; set; }
        public uint ModelId1 { get; set; }
        public uint ModelId2 { get; set; }
        public uint MakeId1 { get; set; }
        public uint MakeId2 { get; set; }
        public uint VersionId1 { get; set; }
        public uint VersionId2 { get; set; }
        public string Bike1 { get; set; }
        public string Bike2 { get; set; }
        public string OriginalImagePath1 { get; set; }
        public string OriginalImagePath2 { get; set; }
        public string HostUrl1 { get; set; }
        public string HostUrl2 { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public ushort PriorityOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsSponsored { get; set; }
        public DateTime SponsoredStartDate { get; set; }
        public DateTime SponsoredEndDate { get; set; }
    }
}
