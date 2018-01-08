using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Model Color DTO
    /// Author  : Sushil Kumar  
    /// Date    : 21st Jan 2016 
    /// Modified by  :   Sumit Kate on 07 Apr 2016
    /// Added Serializable attribute
    /// modified by : Sajal Gupta on 01-03-2017
    /// Description : Added ColorImageId
    /// </summary>
    [Serializable]
    public class NewBikeModelColor
    {
        public uint Id { get; set; }
        public uint ModelId { get; set; }
        public string ColorName { get; set; }
        public string HexCode { get { if (HexCodes != null && HexCodes.Any()) { return HexCodes.FirstOrDefault(); } return ""; } }
        public IEnumerable<string> HexCodes { get; set; }
        public UInt32 ColorImageId { get; set; }
    }

}
