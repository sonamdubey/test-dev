
namespace Bikewale.Entities.UsedBikes
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Date : 21st june 2016
    /// Desc : carrier of most recent bike details
    /// </summary>
    public class MostRecentBikes
    {
        public uint MakeYear { get; set; }

        public string MakeName { get; set; }

        public string ModelName { get; set; }

        public string MakeMaskingName { get; set; }

        public string ModelMaskingName { get; set; }

        public string VersionName { get; set; }

        public uint BikePrice { get; set; }

        public string CityName { get; set; }

        public string ProfileId { get; set; }

        public uint AvailableBikes { get; set; }

        public string CityMaskingName { get; set; }
    }
}
