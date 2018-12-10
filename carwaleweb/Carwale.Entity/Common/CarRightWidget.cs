using Carwale.Entity.CarData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Common
{
    [Serializable]
    public class CarRightWidget
    {
        public List<TopSellingCarModel> PopularModels { get; set; }
        public List<UpcomingCarModel> UpcomingCars { get; set; }
    }
}
