using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified
{
    [Serializable]
    public class StockSummary
    {
        public string ProfileId { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public string MaskingName { get; set; }
        public string CityName { get; set; }
        public int Price { get; set; }
        public int Km { get; set; }
        public int MakeYear { get; set; }
        public string FuelType { get; set; }
        public string AreaName { get; set; }
        public string Color { get; set; }
        public string OriginalImgPath { get; set;}
        public string HostUrl { get; set; }
        public string MaskingNumber { get; set; }
    }
}
