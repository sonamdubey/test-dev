using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Stock.SimiliarCars
{
    public class StockSummaryApp
    {
        public string ProfileId { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public string OriginalImgPath { get; set; }
        public string Price { get; set; }
        public string Year { get; set; }
        public string Kms { get; set; }
        public string Fuel { get; set; }
        public string GearBox { get; set; }
        public string AreaName { get; set; }
        public string City { get; set; }
        public string CertificationScore { get; set; }
        public string DealerRatingText { get; set; }
    }
}
