using Carwale.Entity.Geolocation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    public class PqVersionCitiesEntity
    {
        public string SmallPicUrl { get; set; }
        public string LargePicUrl { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public double ReviewRate { get; set; }
        public bool OfferExists { get; set; }
        public int ReviewCount { get; set; }
        public string ExShowroomCity { get; set; }
        public string CarName { get; set; }
        public string OriginalImgPath { get; set; }
        public string HostUrl { get; set; }
        public List<CarVersionEntity> Versions { get; set; }
        public List<City> Cities { get; set; }
        public List<CityZones> Zones { get; set; }
    }
}
