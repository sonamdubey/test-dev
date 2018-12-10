using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carwale.Entity.CarData;

namespace Carwale.UI.ViewModels.NewCars
{
    public class UpcomingCarWidget
    {
        public int MakeId { set; get; }
        public List<UpcomingCarModel> UpcomingCarsList { set; get; }
    }
}