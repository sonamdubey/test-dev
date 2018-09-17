using Bikewale.Entities.Finance.BajajAuto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Finance.BajajAuto
{
    public interface IBajajAutoCache
    {
        BajajBikeMappingEntity GetBajajFinanceBikeMappingInfo(uint versionId, uint pincodeId);
    }
}
