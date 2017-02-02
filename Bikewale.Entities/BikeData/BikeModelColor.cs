using System;

namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class BikeModelColor
    {
        public uint Id { get; set; }
        public uint ModelId { get; set; }
        public string ColorName { get; set; }
        public string HexCode { get; set; }
        public UInt16 NoOfDays { get; set; }
    }
}
