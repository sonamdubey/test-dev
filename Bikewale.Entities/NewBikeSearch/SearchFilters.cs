
namespace Bikewale.Entities.NewBikeSearch
{
    public class SearchFilters
    {
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public double MaxMileage { get; set; }
        public double MinMileage { get; set; }
        public ushort BodyStyle { get; set; }
        public double MinDisplacement { get; set; }
        public double MaxDisplacement { get; set; }
        public uint MakeId { get; set; }
        public bool ABS { get; set; }
        public bool DiscBrake { get; set; }
        public bool DrumBrake { get; set; }
        public bool AlloyWheel { get; set; }
        public bool SpokeWheel { get; set; }
        public bool Electric { get; set; }
        public bool Manual { get; set; }
    }
}
