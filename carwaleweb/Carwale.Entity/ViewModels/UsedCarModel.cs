using Carwale.Entity.Classified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ViewModels
{
    public class UsedCarModel
    {
        public List<LiveListingTopEntity> ListTopLiveListingCars { get; set; }
        public DealerOfTheMonthModel DealerOfTheMonth { get; set; }

        public UsedCarModel()
        {
            this.ListTopLiveListingCars = new List<LiveListingTopEntity>();
            this.DealerOfTheMonth = new DealerOfTheMonthModel();
        }
    }
}
