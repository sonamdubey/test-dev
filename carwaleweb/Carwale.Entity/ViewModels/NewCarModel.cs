using Carwale.Entity.CarData;
using Carwale.Entity.CompareCars;
using System.Collections.Generic;

namespace Carwale.Entity.ViewModels
{
    public class NewCarModel
    {
        public List<UpcomingCarModel> Upcoming;
        public List<LaunchedCarModel> NewLaunches;
        public List<HotCarComparison> HotComparisons;
        public TopSellingModel TopSellingModel;
    }
}