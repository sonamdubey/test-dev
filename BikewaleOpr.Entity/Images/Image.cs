
namespace BikewaleOpr.Entities.Images
{
    /// <summary>
    /// Author      : Sumit Kate
    /// Created On  : 09 Nov 2016
    /// Description : This class represents image entity which holds all properties of an image 
    /// </summary>
    public class Image
    {
        public string Extension { get; set; }

        public ulong? Id { get; set; }

        public uint CategoryId { get; set; }

        public uint? ItemId { get; set; }

        public string HostUrl { get; set; }

        public string OriginalPath { get; set; }

        public bool? IsReplicated { get; set; }

        public uint? ReplicatedId { get; set; }

        public decimal AspectRatio { get; set; }

        public bool? IsWaterMark { get; set; }

        public bool? IsMain { get; set; }

        public bool? IsMaster { get; set; }

        public uint? ProcessedId { get; set; }
    }
}
