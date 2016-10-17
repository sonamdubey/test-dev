using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeData
{
    /// <summary>
    /// Created by: Aditi Srivastava on 17 Oct 2016
    /// Summary: To get colors by version id
    /// </summary>
    [Serializable]
    public class BikeColorsbyVersion
    {
        public uint ColorId { get; set; }
        public string ColorName { get; set; }
        public IEnumerable<string> HexCode { get; set; }
    }
}
