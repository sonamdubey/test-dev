using Carwale.Entity.ES;
using System.Collections.Generic;

namespace Carwale.Interfaces.ES
{
    public interface IPagesCache
    {
        List<Pages> GetPagesAndPropertiesCache(int applicationId, int platformId);
    }
}