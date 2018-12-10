using Carwale.Entity;
using Carwale.Entity.Finance;
using Carwale.Entity.Enum;
using Carwale.Entity.Insurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Finance
{

    public interface IFinance<TEntity, TOut>
    {
        TOut SaveLead(TEntity lead);
        List<MakeEntity> GetMakes(Application application);
        List<ModelBase> GetModels(int makeId, Application application);
        List<VersionBase> GetVersions(int modelId, Application application);
        List<FinanceCity> GetCities();
        LoanParams IsEligibleForLoan(LoanEligibilityRequestEntity input);
    }
}
