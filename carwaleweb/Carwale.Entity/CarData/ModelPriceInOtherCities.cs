using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    /// <summary>
    /// Created By :Shalini Nair
    /// </summary>
    [Serializable]
    public class ModelPriceInOtherCities
    {
        public string CityName { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string StateName { get; set; }
        public string CityMaskingName { get; set; }
        public int CityId { get; set; }
    }
}
