using Carwale.Entity.Rules;
using System.Collections.Generic;

namespace Carwale.Interfaces.Rules
{
    public interface IRulesCacheRepository<T>
    {
        List<T> GetPanIndiaModelRules(int makeId, int applicationId);
        List<T> GetPanStateModelRules(int stateId, int makeId, int applicationId);
        List<T> GetCityModelRules(int cityId, int makeId, int applicationId);
    }
}
