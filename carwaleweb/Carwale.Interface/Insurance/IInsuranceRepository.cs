using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Insurance;

namespace Carwale.Interfaces.Insurance
{
    public interface IInsuranceRepository
    {
        int SaveLead<T>(T t) where T : InsuranceLead;
        bool UpdateLeadResponse(int recordId, string apiResponse, double premiumAmount = 0, string quotaion = null);
        InsuranceDiscount GetDiscount(int modelId, int cityId);
    }
}
