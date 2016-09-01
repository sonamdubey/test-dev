
using Bikewale.CoreDAL;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Web;
namespace Bikewale.BAL.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Used Bike Buyer Business Layer
    /// </summary>
    public class UsedBikeBuyer : IUsedBikeBuyer
    {
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly IUsedBikeBuyerRepository _objBuyerRepository = null;
        private readonly IUsedBikeSellerRepository _objSellerRepository = null;

        public UsedBikeBuyer(
            ICustomer<CustomerEntity, UInt32> objCustomer,
            IUsedBikeBuyerRepository objBuyerRepository,
            IUsedBikeSellerRepository objSellerRepository
            )
        {
            _objCustomer = objCustomer;
            _objBuyerRepository = objBuyerRepository;
            _objSellerRepository = objSellerRepository;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   Processes Photo request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool UploadPhotosRequest(PhotoRequest request)
        {
            bool isSuccess = false;
            string inquiryId = "", consumerType = "";
            byte consumer = default(byte);
            CustomerEntity objCust = null;
            bool isDealer = false;
            string buyerName, buyerEmail, buyerMobile, buyerId = "";
            if (request != null && UsedBikeProfileId.IsValidProfileId(request.ProfileId))
            {
                UsedBikeProfileId.GetInquiryId(request.ProfileId, out inquiryId, out consumerType);
                isDealer = consumerType.Equals("D", StringComparison.CurrentCultureIgnoreCase);
                consumer = Convert.ToByte(isDealer ? 1 : 0);
                BWCookies.GetBuyerDetailsFromCookie(out buyerName, out buyerMobile, out buyerEmail, out buyerId);
                string listingUrl = HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/used/sell/uploadbasic.aspx?id=" + request.ProfileId;
                if (String.IsNullOrEmpty(buyerId))
                {
                    objCust = new CustomerEntity() { CustomerName = request.Customer.CustomerName, CustomerEmail = request.Customer.CustomerEmail, CustomerMobile = request.Customer.CustomerMobile, ClientIP = CommonOpn.GetClientIP() };
                    UInt32 CustomerId = _objCustomer.Add(objCust);
                    if (CustomerId > 0)
                    {
                        buyerId = CustomerId.ToString();
                        string buyerData = objCust.CustomerName + ":" + objCust.CustomerMobile + ":" + objCust.CustomerEmail + ":" + BikewaleSecurity.EncryptUserId(CustomerId);
                        BWCookies.SetBuyerDetailsCookie(buyerData);
                    }
                }

                if (buyerId != "-1")
                {
                    isSuccess = _objBuyerRepository.UploadPhotosRequest(inquiryId, buyerId, consumer, request.Message);
                    if (isSuccess)
                    {
                        UsedBikeSellerBase seller = _objSellerRepository.GetSellerDetails(inquiryId, consumerType.Equals("D", StringComparison.CurrentCultureIgnoreCase) ? true : false);
                        if (seller != null && seller.Details != null && !String.IsNullOrEmpty(seller.Details.CustomerEmail) && !String.IsNullOrEmpty(seller.Details.CustomerName))
                        {
                            if (isDealer)
                            {
                                SendEmailSMSToDealerCustomer.UsedBikePhotoRequestEmailToDealer(seller.Details.CustomerEmail, seller.Details.CustomerName, buyerName, buyerMobile, request.BikeName, request.ProfileId);
                            }
                            else
                            {
                                SendEmailSMSToDealerCustomer.UsedBikePhotoRequestEmailToIndividual(seller.Details.CustomerEmail, seller.Details.CustomerName, buyerName, buyerMobile, request.BikeName, listingUrl);
                            }
                        }

                    }
                }
            }
            return isSuccess;
        }
    }
}
