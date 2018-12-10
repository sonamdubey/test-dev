using Carwale.Entity.Finance;
using Carwale.Entity.Common;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Finance
{
    public interface IFinanceOperations
    {
        LoanParams IsEligibleForLoan(LoanEligibilityRequestEntity input);
        List<IdName> GetFinanceCompanyListRepo(int clientId);
        List<int> SaveLead(FinanceLead objFinanceLead);
        bool UpdateLeadResponse(FinanceLead objFinanceLead);
        bool CheckEligibility(LoanEligibilityRequestEntity eligibilityInput);
    }
}
