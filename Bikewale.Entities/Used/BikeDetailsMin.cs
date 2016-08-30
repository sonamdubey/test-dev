using System;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 27th August 2016
    /// Description : Bike Minimum details entity for used bikes
    /// </summary>
    [Serializable]
    public class BikeDetailsMin
    {
        public string ProfileId { set; get; }
        public BikePhoto Photo { get; set; }
        public uint AskingPrice { get; set; }
        public string ModelYear { get; set; }
        public uint KmsDriven { get; set; }
        public string OwnerType { get; set; }
        public string RegisteredAt { get; set; }
    }

    /// <summary>
    /// Created By : Sangram Nandkhile on 29 August 2016
    /// Description : Similar Bikes Minimum details entity for used bikes
    /// </summary>
    [Serializable]
    public class OtherUsedBikeDetails : BikeDetailsMin
    {
        public string MakeMaskingName { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }
        public string CityMaskingName { get; set; }

        public string BikeName { get; set; }
    }

}








