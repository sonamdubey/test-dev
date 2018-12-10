using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.BL.TCApi_Inquiry;
using Newtonsoft.Json;
using Carwale.Entity;
using System.Collections.Specialized;
using Carwale.Interfaces.Dealers;

namespace Carwale.Service.APIServices
{
    public class TCApi_Inquiry<T, TResponse> : IAPIService<T, TResponse>
        where T : DealerInquiryDetails
        where TResponse : APIResponseEntity, new()
    {        
        IDealerSponsoredAdRespository _dealerSponsored;

        public TCApi_Inquiry(IDealerSponsoredAdRespository dealerSponsored)
        {           
            _dealerSponsored = dealerSponsored;
        }

        public TResponse Request(T t)
        {
            var tcApi = new TCApi_InquirySoapClient();
            string inquiryJSON = GetInquiryJSON(t);

            var apiResponse = tcApi.AddNewCarInquiry(t.DealerId.ToString(), inquiryJSON);

            var responseEntity = new TResponse()
            {
                ResponseId = string.IsNullOrWhiteSpace(apiResponse) ? 0 : (Convert.ToInt64(apiResponse) > 0 ? Convert.ToUInt64(apiResponse) : 0)
            };

            return responseEntity;
        }

        public void UpdateResponse(T t, TResponse t2)
        {
        //    _dealerSponsored.UpdateInquiryPushResponse(t.PQDealerAdLeadId, t2.ResponseId,t.CampaignId);
        }

        public string GetInquiryJSON(T t)
        {
            // serializing data
            var carInfo = new NewCarInquiryInfo()
            {
                CustomerName = t.Name,
                CustomerEmail = t.Email,
                CustomerMobile = t.Mobile,
                BuyingTime = t.BuyTimeValue.ToString(),
                VersionId = t.VersionId.ToString(),
                CityId = t.CityId.ToString(),
                InquirySourceId = t.InquirySourceId,// inquiry from Carwale website
                InquiryTypeId = t.RequestType.ToString(),
                Eagerness = "-1",
                ModelId = t.ModelId.ToString(),
                FuelType = "",
                Transmission = "",
                RequestType = t.RequestType.ToString(),
                CWCustomerId = string.Empty,
                CWInquiryId = string.Empty,
                PreferedDate = "",
                Comments = t.Comments,
                CompanyName = "",
                RegistrationNo = "",
                IsCorporate = "false",
                TypeOfService = "",
                DealsStockId = t.DealsStockId.ToString(),
                IsPaymentSuccess = t.IsPaymentSuccess.ToString().ToLower()
            };

            return JsonConvert.SerializeObject(carInfo);
        }
    }
}
