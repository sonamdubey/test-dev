using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Interfaces.BikeData
{
    public interface IBikeSeries<T,U> : IRepository<T,U>
    {
        List<BikeModelEntity> GetModelsList(U seriesId);
        BikeDescriptionEntity GetSeriesDescription(U seriesId);
        List<BikeModelEntityBase> GetModelsListBySeriesId(U seriesId);
    }
}
