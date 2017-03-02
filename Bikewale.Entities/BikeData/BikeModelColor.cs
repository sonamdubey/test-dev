using System;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// modified by : Sajal Gupta on 01-03-2017
    /// Description : Added ColorImageId
    /// </summary>
    [Serializable]
    public class BikeModelColor
    {
        public uint Id { get; set; }
        public uint ModelId { get; set; }
        public string ColorName { get; set; }
        public string HexCode { get; set; }
        public UInt16 NoOfDays { get; set; }
        public UInt32 ColorImageId { get; set; }
    }
}
