using Carwale.Entity.ES;
using System.Collections.Generic;

namespace Carwale.Interfaces.ES
{
    public interface IPagesRepository
    {
        List<Pages> GetPagesAndProperties(int applicationId, int platformId);
    }
}