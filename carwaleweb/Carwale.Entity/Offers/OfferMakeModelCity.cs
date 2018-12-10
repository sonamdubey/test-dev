using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Offers
{
    [Serializable]
    public class OfferMakeModelCity
    {
        public int MakeId { get; set; }
        public string MakeName { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string ZoneId { get; set; }
        public string ZoneName { get; set; }
    }
}
