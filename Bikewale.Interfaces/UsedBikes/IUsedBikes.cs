using Bikewale.Entities.UsedBikes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.UsedBikes
{
    public interface IUsedBikes 
    {
        public IEnumerable<PopularUsedBikesEntity> GetPopularUsedBikes(uint totalCount, int? city = null);
    }
}
