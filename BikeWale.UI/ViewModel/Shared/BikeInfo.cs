
using Bikewale.Entities.BikeData;
namespace Bikewale.Models.Shared
{
    public class BikeInfo
    {
        public string Url { get; set; }
        public string Bike { get; set; }
        public int PQSource { get; set; }
        public uint ModelId { get; set; }
        public Bikewale.Entities.GenericBikes.GenericBikeInfo Info { get; set; }
        public BikeSeriesEntity Series { get; set; }
    }
}
