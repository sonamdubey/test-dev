using System;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
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
