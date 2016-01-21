using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Bike Color
    /// Author  :   Sumit Kate
    /// Date    :   25 Aug 2015
    /// </summary>
    public class BikeColor
    {
        public int ColorId { get; set; }
        public uint VersionId { get; set; }
        public string Color { get; set; }
        public IEnumerable<string> HexCodes { get; set; }
    }

    public class BikeModelColor
    {
        public int ModelColorId { get; set; }
        public string HexCode { get; set; }
    }
    
}
