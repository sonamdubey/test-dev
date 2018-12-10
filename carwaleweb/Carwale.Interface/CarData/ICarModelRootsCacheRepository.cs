using Carwale.Entity.CarData;
using Carwale.Entity.CentralizedCacheRefresh;
using System.Collections.Generic;

namespace Carwale.Interfaces.CarData
{
    public interface ICarModelRootsCacheRepository
    {
        List<RootBase> GetRootsByMake(int makeId);
        RootBase GetRootByModel(int modelId);
        List<ModelsByRootAndYear> GetModelsByRootAndYear(int rootId, int year);
        List<CarLaunchDiscontinueYear> GetYearsByRootId(int rootId);
        IEnumerable<RootBase> GetRoots(string rootIds);
        bool RefreshCarModelRootCache(List<ModelRootAttribute> modelAttributes);
    }
}
