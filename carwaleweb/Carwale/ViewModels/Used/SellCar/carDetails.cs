using Carwale.Entity.CarData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carwale.UI.ViewModels.Used.SellCar
{
    public class CarDetails
    {
        public IEnumerable<int> MakeYears { get; set; }
        public IEnumerable<CarMakeEntityBase> Makes { get; set; }
        public IEnumerable<CarMakeEntityBase> PopularMakes { get; set; }
        public string BuyingIndexApiUrl { get; set; }
        public IEnumerable<int> InsuranceYears { get; set; }
    }
}