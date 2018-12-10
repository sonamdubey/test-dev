using Carwale.Entity.Classified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ViewModels
{
    public class DealerOfTheMonthModel
    {
        public List<LiveListingTopEntity> ListDealerOfTheMonthCars { get; set; }
        public int CountDealerOfTheMonthCars { get; set; }

        public DealerOfTheMonthModel()
        {
            this.ListDealerOfTheMonthCars = new List<LiveListingTopEntity>();
        }
    }
}
