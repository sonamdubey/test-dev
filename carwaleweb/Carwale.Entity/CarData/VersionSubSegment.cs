using Newtonsoft.Json;

namespace Carwale.Entity.CarData
{
    public class VersionSubSegment
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string VersionName { get; set; }
        public string SubSegment { get; set; }
        public int SubSegmentId { get; set; }
        public string Segment { get; set; }
        public string CarTransmission { get; set; }
        public string FuelType { get; set; }
        public string MaskingName { get; set; }
        public int AvgPrice { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsNew { get; set; }
    }
}
