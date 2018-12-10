using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.NewCars
{
    public class RecentLaunchesDTO_Desktop
    {
        public List<Carwale.Entity.CarData.LaunchedCarModel> RecentLaunches { get; set; }
        public int Pageno { get; set; }
        public int RecordCount { get; set; }

        public int PageSize { get; set; }
    }
}
