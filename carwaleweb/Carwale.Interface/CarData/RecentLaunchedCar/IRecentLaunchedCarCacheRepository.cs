using Carwale.Entity.CarData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CarData.RecentLaunchedCar
{
    /// <summary>
    /// inteface of recentlaunchedcars for cache layer
    /// written by Natesh Kumar on 1/10/2014
    /// </summary>
    public interface IRecentLaunchedCarCacheRepository
    {
         List<RecentLaunchedCarEntity> GetRecentLaunchedCars();
    }
}
