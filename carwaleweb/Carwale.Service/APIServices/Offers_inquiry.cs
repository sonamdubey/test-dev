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
using Carwale.Entity.Offers;

namespace Carwale.Service.APIServices
{
    public class Offers_inquiry<T, TResponse> : IAPIService<T, TResponse>
        where T : OfferInputs
        where TResponse : APIResponseEntity, new()
    {
        IDealerSponsoredAdRespository _dealerSponsored;

        public Offers_inquiry(IDealerSponsoredAdRespository dealerSponsored)
        {
            _dealerSponsored = dealerSponsored;
        }

        public TResponse Request(T t)
        {
            var tcApi = new TCApi_InquirySoapClient();
            string inquiryJSON = GetInquiryJSON(t);

            var apiResponse = tcApi.AddNewCarCWOfferInquiry(t.DealerId.ToString(), inquiryJSON);

            var responseEntity = new TResponse()
            {
                ResponseId = string.IsNullOrWhiteSpace(apiResponse) ? 0 : (Convert.ToInt64(apiResponse) > 0 ? Convert.ToUInt64(apiResponse) : 0)
            };

            return responseEntity;
        }

        public void UpdateResponse(T t, TResponse t2)
        {
           // _dealerSponsored.UpdateInquiryPushResponse(t.PQDealerAdLeadId, t2.ResponseId,-1);
        }

        private string GetInquiryJSON(T t)
        {
            // serializing data
            var carInfo = new NewCarInquiryInfo()
            {
                CustomerName = t.Name,
                CustomerEmail = t.Email,
                CustomerMobile = t.Mobile,
                BuyingTime = "7",
                VersionId = t.VersionId.ToString(),
                CityId = t.CityId.ToString(),
                InquirySourceId = t.InquirySourceId,// inquiry from Carwale website
                InquiryTypeId = t.InquirySourceId,
                Eagerness = "-1",
                ModelId = "",
                FuelType = "",
                Transmission = "",
                RequestType = "1",
                CWCustomerId = string.Empty,
                CWInquiryId = t.PQId.ToString(),
                PreferedDate = "",
                Comments = "",
                CompanyName = "",
                RegistrationNo = "",
                IsCorporate = "false",
                TypeOfService = "",
                CwOfferId = t.OfferId,
                CouponCode = t.CouopnCode
            };

            return JsonConvert.SerializeObject(carInfo);
        }
    }
}
