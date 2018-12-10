using Carwale.Entity.Rules;
using System.Collections.Generic;

namespace Carwale.Interfaces.Rules
{
    public interface IRulesRepository<T>
    {
        List<T> GetModelRules(int stateId, int cityId, int makeId, int applicationId);
    }
}
