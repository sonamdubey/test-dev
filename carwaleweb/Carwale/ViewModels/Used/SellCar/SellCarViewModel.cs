using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using System.Collections.Generic;

namespace Carwale.UI.ViewModels.Used.SellCar
{
    public class SellCarViewModel
    {
        public IEnumerable<Cities> PopularCities { get; set; }
        public PageMetaTags MetaData { get; set; }
        public Platform Source { get; set; }
    }   
}