
using System;
using System.Collections.Generic;
namespace Bikewale.Entities.NewBikeSearch
{
    public class SearchFilters
    {
        public IEnumerable<Tuple<int, int>> PriceRange { get; set; }
        public IEnumerable<Tuple<double, double>> Mileage { get; set; }
        public IEnumerable<Tuple<double, double>> Displacement { get; set; }
        public IEnumerable<Tuple<double, double>> Power { get; set; }
        public ushort BodyStyle { get; set; }
        public uint MakeId { get; set; }
        public bool ABS { get; set; }
        public bool DiscBrake { get; set; }
        public bool DrumBrake { get; set; }
        public bool AlloyWheel { get; set; }
        public bool SpokeWheel { get; set; }
        public bool Electric { get; set; }
        public bool Manual { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool ExcludeMake { get; set; }
        public uint CityId { get; set; }
        public byte ModelStatus { get; set; }

    }
}
