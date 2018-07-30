using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// City DTO
    /// </summary>
    public class City
    {
        public uint CityId { get; set; }

        public string CityName { get; set; }

        public string CityMaskingName { get; set; }
    }
}
