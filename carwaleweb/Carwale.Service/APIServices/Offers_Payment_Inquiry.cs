using Carwale.Entity;
using Carwale.Entity.Dealers;
using Carwale.Entity.Offers;
using Carwale.Interfaces;
using Carwale.BL.TCApi_Inquiry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Service.APIServices
{
    public class Offers_Payment_Inquiry<T, TResponse> : IAPIService<T, TResponse>
        where T : OfferPaymentEntity
        where TResponse : APIResponseEntity, new()
    {

        public TResponse Request(T t)
        {
            var tcApi = new TCApi_InquirySoapClient();
            string inquiryJSON = GetInquiryJSON(t);

            var apiResponse = tcApi.BookNewCarOfferInquiry(t.DealerId.ToString(), inquiryJSON);

            var responseEntity = new TResponse()
            {
                ResponseId = string.IsNullOrWhiteSpace(apiResponse) ? 0 : (Convert.ToInt64(apiResponse) > 0 ? Convert.ToUInt64(apiResponse) : 0)
            };

            return responseEntity;
        }


        private string GetInquiryJSON(T t)
        {
            // serializing data
            var inquiryObj = new TC_PaymentInquiryEntity();

            inquiryObj.CouponCode = t.CouponCode;
            inquiryObj.CwOfferId = t.OfferId;
            inquiryObj.Payment = t.PaymentAmount;
            inquiryObj.BookingDate = t.BookingDate.ToString("MM/dd/yyyy");
            inquiryObj.InquiryId = t.AutobizInquiryId;

            return JsonConvert.SerializeObject(inquiryObj);
        }



        void IAPIService<T, TResponse>.UpdateResponse(T t, TResponse t2)
        {
            throw new NotImplementedException();
        }
    }
}
