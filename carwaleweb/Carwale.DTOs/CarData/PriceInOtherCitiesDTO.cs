using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Articles;
using System.Collections.Generic;

namespace Carwale.DTOs.CarData
{
    public class PriceInOtherCitiesDTO
    {
        public CarModelDetails ModelDetails { get; set; }
        public List<ModelPriceInOtherCities> PriceInOtherCities { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
