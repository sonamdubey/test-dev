
namespace BikewaleOpr.Entity
{
    public class BikeColorAvailability
    {
        public uint ColorId { get; set; }
        public string ColorName { get; set; }
        public uint DealerId { get; set; }
        public short NoOfDays { get; set; }
        public bool isActive { get; set; }
        public uint VersionId { get; set; }
        public string HexCode { get; set; }
    }
}
