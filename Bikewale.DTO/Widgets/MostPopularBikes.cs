using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Widgets
{
    public class MostPopularBikes
    {
        public MakeBase objMake { get; set; }
        public ModelBase objModel { get; set; }
        public VersionBase objVersion { get; set; }
        public string BikeName { get; set; }
        public string HostURL { get; set; }
        public string OriginalImagePath { get; set; }
        public int ReviewCount { get; set; }
        public double ModelRating { get; set; }
        public Int64 VersionPrice { get; set; }
        public MinSpecs Specs { get; set; }
    }
}
