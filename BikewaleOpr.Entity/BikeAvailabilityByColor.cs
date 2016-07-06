using System.Collections.Generic;

namespace BikewaleOpr.Entity
{
    public class BikeAvailabilityByColor
    {
        public uint ColorId { get; set; }
        public string ColorName { get; set; }
        public uint DealerId { get; set; }
        public short NoOfDays { get; set; }
        public bool isActive { get; set; }
        public uint VersionId { get; set; }
        public IEnumerable<string> HexCode { get; set; }
    }
}
