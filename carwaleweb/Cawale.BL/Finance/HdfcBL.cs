using Carwale.BL.HDFC_Bank_C2T_Adapt;
using Carwale.DTOs.Finance;
using Carwale.Entity.Finance;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Finance;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carwale.BL.Finance
{
    public class Hdfc : IFinance<FinanceLead, ClientResponseDto>
    {
        private readonly IPQCacheRepository _pqCachedRepo;

        private readonly IFinanceOperations _financeRepo;

        public Hdfc(IPQCacheRepository pqCachedRepo, IFinanceOperations financeRepo)
        {
            _pqCachedRepo = pqCachedRepo;
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
                int pushStatus = 0;
                bool isHdfcCity = Array.IndexOf(CWConfiguration.HdfcCityList, lead.CityId) >= 0;

                lead.Indigo_UniqueKey = ConfigurationManager.AppSettings["IndigoUniqueKey"] ?? "HDFCC2TQ9W1E8W2Q";
                lead.Lead_Date_Time = (DateTime.Now).ToString("yyyy/MM/dd hh:mm:ss tt");
                lead.IP_Address = "101.102.103.104";
                lead.TypeOfLoan = "AL";
                lead.Product_Applied_For = lead.TypeOfLoan;
                lead.Source_Code = "Carwale";
                lead.Indigo_RequestFromYesNo = "Yes";
                lead.Res_address = "Current Residence Starts From-" + lead.Res_address;
                lead.Res_address2 = "Business Start Date-" + lead.Res_address2;
                lead.Res_address3 = "Buying Period-" + lead.BuyingPeriod;
                lead.Resi_City_other = "Profession Type-" + lead.Resi_City_other;
                lead.Resi_City_other1 = "Profit After Tax-" + lead.Resi_City_other1; 
                
                if (financeLeadId <= 0)
                {
                    List<int> result = _financeRepo.SaveLead(lead);
                    lead.FinanceLeadId = result[0];
                    financeLeadId = isHdfcCity ? lead.FinanceLeadId : -1;
                    pushStatus = result[1];
                }
                
                response.CromaNewLeadId = lead.FinanceLeadId;
                lead.Promo_Code = lead.FinanceLeadId.ToString();
            }
            catch (ArgumentNullException argumentNullException)
            {
                ExceptionHandler objErr = new ExceptionHandler(argumentNullException, "Carwale.BL.Finance.Hdfc.SaveLead() inputs: lead is null");
                objErr.LogException();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Finance.Hdfc.SaveLead() inputs: " + JsonConvert.SerializeObject(lead));
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

        public List<Entity.Finance.FinanceCity> GetCities()
        {
            throw new NotImplementedException();
        }

        public LoanParams IsEligibleForLoan(LoanEligibilityRequestEntity input)
        {
            LoanParams loanParams = new LoanParams();
            List<PQItem> pqList = _pqCachedRepo.GetPQ(input.CityId, input.VersionId).PriceQuoteList;

            var pqItem = from pq in pqList
                                  where pq.Key == "Ex-Showroom Price"
                                  select pq;

            if (pqItem.Count() > 0)
            {
                input.ExShowroomPrice = pqItem.ToList()[0].Value;
                loanParams = _financeRepo.IsEligibleForLoan(input);
            }
            else
            {
                loanParams.ExShowroomPrice = 0;
                loanParams.IsPermitted = false;
                loanParams.LoanAmount = 0;
            }

            return loanParams;
        }
    }
}
