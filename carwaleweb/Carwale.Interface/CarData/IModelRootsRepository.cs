using Carwale.Entity.CarData;
using System.Collections.Generic;

namespace Carwale.Interfaces.CarData
{
    public interface IModelRootsRepository
    {
        List<RootBase> GetRootsByMake(int makeId, bool isCriticalRead = false);
        RootBase GetRootByModel(int modelId, bool isCriticalRead = false);
        List<ModelsByRootAndYear> GetModelsByRootAndYear(int rootId, int year, bool isCriticalRead = false);
        List<CarLaunchDiscontinueYear> GetYearsByRootId(int rootId, bool isCriticalRead = false);
        IEnumerable<RootBase> GetRoots(string rootIds, bool isCriticalRead = false);
    }
}
