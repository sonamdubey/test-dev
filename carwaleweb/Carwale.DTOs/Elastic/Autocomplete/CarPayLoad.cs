using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Elastic
{
    [Serializable]
    public class CarPayLoad
    {
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string ColorId { get; set; }
        public string ColorName { get; set; }
        public string MakeId { get; set; }
        public string MakeName { get; set; }
        public string ModelId { get; set; }
        public string ModelName { get; set; }
        public string RootId { get; set; }
        public string RootName { get; set; }
        public string MaskingName { get; set; }
        public string VersionName { get; set; }
        public string OutputName { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsDiscontinue { get; set; }
        public string VersionId { get; set; }
        public List<CarPayLoad> CarComparisionList { get; set; }
        public string DesktopLink { get; set; }
        public string MobileLink { get; set; }
        public FeaturedCar FeaturedCar { get; set; }
    }
}
