using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
        /// Model Color DTO
        /// Author  : Sushil Kumar  
        /// Date    : 21st Jan 2016 
        /// </summary>
        public class NewBikeModelColor
        {
            public uint Id { get; set; }
            public uint ModelId { get; set; }
            public string ColorName { get; set; }
            public IEnumerable<string> HexCodes { get; set; }
        }
}
