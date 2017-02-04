using System;

namespace Bikewale.Entities.Used
{

    /// <summary>
    /// Created By : Sajal Gupta on 30-12-2016
    /// Description : Entity for used bikes in city count by make
    /// </summary>
    [Serializable]
    public class UsedBikesCountInCity
    {
        public uint BikeCount { get; set; }
        public uint CityId { get; set; }
        public string CityName { get; set; }
        public string CityMaskingName { get; set; }
        public uint StartingPrice { get; set; }
    }
}
