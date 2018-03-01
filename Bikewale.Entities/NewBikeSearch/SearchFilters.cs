
using System;
using System.Collections.Generic;
namespace Bikewale.Entities.NewBikeSearch
{
    public class SearchFilters
    {
        public ushort BodyStyle { get; set; }
        public uint MakeId { get; set; }
        public bool ABS { get; set; }
        public bool DiscBrake { get; set; }
        public bool DrumBrake { get; set; }
        public bool AlloyWheel { get; set; }
        public bool SpokeWheel { get; set; }
        public bool Electric { get; set; }
        public bool Manual { get; set; }
        public ushort PageNumber { get; set; }
        public ushort PageSize { get; set; }
        public bool ExcludeMake { get; set; }
        public uint CityId { get; set; }
        public byte ModelStatus { get; set; }

        public IEnumerable<PriceRangeEntity> Price { get; set; }

        public IEnumerable<RangeEntity> Mileage { get; set; }

        public IEnumerable<RangeEntity> Displacement { get; set; }

        public IEnumerable<RangeEntity> Power { get; set; }

    }

    public class RangeEntity
    {
        public double Min { get; set; }
        public double Max { get; set; }
    }
    public class PriceRangeEntity
    {
       public int Min { get; set; }
        public int Max { get; set; }
    }
}
