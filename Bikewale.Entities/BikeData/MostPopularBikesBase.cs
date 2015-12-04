using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    public class MostPopularBikesBase
    {
        public BikeMakeEntityBase objMake { get; set; }
        public BikeModelEntityBase objModel { get; set; }
        public BikeVersionsListEntity objVersion { get; set; }
        public string BikeName { get; set; }
        public string HostURL { get; set; }
        public string OriginalImagePath { get; set; }
        public int ReviewCount {get; set; }
        public double ModelRating { get; set; }
        public Int64 VersionPrice { get; set; }
        public MinSpecsEntity Specs { get; set; }
        public ushort BikePopularityIndex { get; set; }                 
    }
}
