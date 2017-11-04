using System;

namespace Bikewale.Entities.BikeData
{
    public class BikeModelsListEntity : BikeModelEntity
    {
        public UInt16 ModelRank { get; set; }
        public UInt16 ModelCount { get; set; }
        public string SeriesHostUrl { get; set; }
        public string SeriesSmallPicUrl { get; set; }
    }
}
