using Carwale.DTOs.Insurance;
using Carwale.Entity;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Insurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Insurance
{
    public interface IInsurance
    {
        QuotationDto SubmitLead(InsuranceLead inputs);
        InsuranceResponse SubmitLeadV2(InsuranceLead inputs);
        List<MakeEntity> GetMakes(Application application);
        List<ModelBase> GetModels(int makeId, Application application);
        List<VersionBase> GetVersions(int modelId, Application application);
        List<InsuranceCity> GetCities(Application application);
    }
}
