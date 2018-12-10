using Carwale.Entity;
using Carwale.Entity.Common;
using Carwale.Entity.Enum;
using Carwale.Entity.Finance;
using Carwale.Entity.Insurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Finance
{

    public interface IFinanceCacheRepository
    {
        List<IdName> GetFinanceCompanyList(int clientId);
    }
}
