using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified
{
    public class LiveListingTopEntity
    {
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
        public string VersionName { get; set; }
        public string ProfileId { get; set; }
        public int VersionId { get; set; }
        public string CityName { get; set; }
        public long Price { get; set; }
        public long Kilometers { get; set; }
        public int MakeYear { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostURL { get; set; }
        public string CarFuelType { get; set; }
    }
}
