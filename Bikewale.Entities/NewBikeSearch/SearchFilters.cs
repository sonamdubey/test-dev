
using System.Collections.Generic;
namespace Bikewale.Entities.NewBikeSearch
{

    /// <summary>
    /// Modified by: Dhruv Joshi
    /// Dated: 8th March 2018
    /// Description: BodyStyle changed to IEnumerable of strings
    /// Modified by : Snehal Dange on 11th April 2018
    /// Description: Added Brakes,Wheels,StartType
    /// </summary>
    public class SearchFilters
    {
        public IEnumerable<string> BodyStyle { get; set; }
        public uint MakeId { get; set; }
        public bool? ABS { get; set; }
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

        public IEnumerable<string> Brakes { get; set; }
        public IEnumerable<uint> Wheels { get; set; }
        public IEnumerable<uint> StartType { get; set; }

        public IEnumerable<uint> Make { get; set; }
        public IEnumerable<uint> Model { get; set; }


        public string SortOrder { get; set; }

        public string SortCriteria { get; set; }
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
