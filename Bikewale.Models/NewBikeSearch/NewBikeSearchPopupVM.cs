﻿
namespace Bikewale.Models.NewBikeSearch
{
    public class NewBikeSearchPopupVM
    {
        public bool HasFilteredBikes { get; set; }
        public bool HasOtherRecommendedBikes { get; set; }
        public uint MakeId { get; set; }
        public uint CityId { get; set; }

        public string MakeName { get; set; }
    }
}
