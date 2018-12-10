using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    /// <summary>
    /// for making list object
    /// written by natesh kumar
    /// </summary>
    [Serializable]
        public class RecentLaunchObject
        {
            public List<RecentLaunchedCarEntity> newLaunches = new List<RecentLaunchedCarEntity>();
        }
    
}
