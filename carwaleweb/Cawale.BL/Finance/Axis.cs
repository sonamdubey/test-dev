using Carwale.DTOs.Finance;
using Carwale.Entity.Finance;
using Carwale.Interfaces.Finance;
using Carwale.Notifications;
using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.Finance
{
    public class Axis : IFinance<FinanceLead, ClientResponseDto>
    {
        private readonly IFinanceOperations _financeRepo;

        public Axis(IFinanceOperations financeRepo)
        {
            _financeRepo = financeRepo;
        }
        public ClientResponseDto SaveLead(FinanceLead lead)
        {
            var response = new ClientResponseDto();       
            try
            {
                if (lead == null)
                {
                    throw new ArgumentNullException();
                }
                var financeLeadId = lead.FinanceLeadId;              
                if (financeLeadId <= 0)
                {
                    List<int> result = _financeRepo.SaveLead(lead);
                    lead.FinanceLeadId = result[0];
                }

                response.CromaNewLeadId = lead.FinanceLeadId;
            }
            catch (ArgumentNullException argumentNullException)
            {
                ExceptionHandler objErr = new ExceptionHandler(argumentNullException, "Carwale.BL.Finance.Axis.SaveLead() inputs: lead is null");
                objErr.LogException();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Finance.Axis.SaveLead() inputs: " + JsonConvert.SerializeObject(lead));
                objErr.LogException();
            }
            return response; 
        }

        public List<Entity.MakeEntity> GetMakes(Entity.Enum.Application application)
        {
            throw new NotImplementedException();
        }

        public List<Entity.ModelBase> GetModels(int makeId, Entity.Enum.Application application)
        {
            throw new NotImplementedException();
        }

        public List<Entity.VersionBase> GetVersions(int modelId, Entity.Enum.Application application)
        {
            throw new NotImplementedException();
        }

        public List<FinanceCity> GetCities()
        {
            throw new NotImplementedException();
        }

        public LoanParams IsEligibleForLoan(LoanEligibilityRequestEntity input)
        {
            throw new NotImplementedException();
        }
    }
}
