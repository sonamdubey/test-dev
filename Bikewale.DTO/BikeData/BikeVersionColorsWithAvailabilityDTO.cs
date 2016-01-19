using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeData
{
    public class BikeVersionColorsWithAvailabilityDTO
    {
        public uint ColorId { get; set; }
        public string ColorName { get; set; }
        public short NoOfDays { get; set; }
        public uint VersionId { get; set; }
        public IEnumerable<string> HexCode { get; set; }
    }
}
