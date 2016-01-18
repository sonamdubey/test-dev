using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{

    public class BikeVersionColorsAvailability
    {
        public uint ColorId { get; set; }
        public string ColorName { get; set; }
        public short NoOfDays { get; set; }
        public uint VersionId { get; set; }
        public string HexCode { get; set; }
    }
    
    public class BikeVersionColorsWithAvailability
    {
        public uint ColorId { get; set; }
        public string ColorName { get; set; }
        public short NoOfDays { get; set; }
        public uint VersionId { get; set; }
        public IEnumerable<string> HexCode { get; set; }
    }
}
