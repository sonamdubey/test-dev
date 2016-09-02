
using Bikewale.CoreDAL;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
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
        private readonly ICustomerRepository<CustomerEntity, UInt32> _objCustomerRepo = null;
        public UsedBikeBuyer(
            ICustomer<CustomerEntity, UInt32> objCustomer,
            IUsedBikeBuyerRepository objBuyerRepository,
            IUsedBikeSellerRepository objSellerRepository,
            ICustomerRepository<CustomerEntity, UInt32> objCustomerRepo
            )
        {
            _objCustomer = objCustomer;
            _objBuyerRepository = objBuyerRepository;
            _objSellerRepository = objSellerRepository;
            _objCustomerRepo = objCustomerRepo;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   Processes Photo request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool UploadPhotosRequest(PhotoRequest request)
        {
            bool isSuccess = false, isDealer = false;
            byte consumer = default(byte);
            string inquiryId = "", consumerType = "";
            CustomerEntityBase buyer = null;
            //Check for valid photo request
            try
            {
                if (isValidPhotoRequest(request))
                {
                    buyer = request.Buyer;

                    //Extract inquiryid and type of inquiry(individual/dealer)
                    UsedBikeProfileId.SplitProfileId(request.ProfileId, out inquiryId, out consumerType);
                    //set bool for dealer listing or individual
                    isDealer = consumerType.Equals("D", StringComparison.CurrentCultureIgnoreCase);
                    consumer = Convert.ToByte(isDealer ? 1 : 2);

                    //if tempcurrentuser cookie exists return the buyers basic details
                    BWCookies.GetBuyerDetailsFromCookie(ref buyer);

                    //Is new Customer 
                    if (buyer.CustomerId == 0 && !String.IsNullOrEmpty(buyer.CustomerEmail))
                    {
                        //perform customer registration with submitted details
                        RegisterBuyer(buyer);
                        //customer registration successful
                        if (buyer.CustomerId > 0)
                        {
                            //create tempcurrentuser cookie
                            string buyerData = String.Format("{0}:{1}:{2}:{3}", buyer.CustomerName, buyer.CustomerMobile, buyer.CustomerEmail, BikewaleSecurity.EncryptUserId(Convert.ToInt64(buyer.CustomerId)));
                            BWCookies.SetBuyerDetailsCookie(buyerData);
                        }
                    }

                    if (buyer.CustomerId > 0)
                    {
                        isSuccess = _objBuyerRepository.UploadPhotosRequest(inquiryId, buyer.CustomerId, consumer, request.Message);
                        if (isSuccess)
                        {
                            NotifySeller(request, isDealer, inquiryId, buyer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("UploadPhotosRequest({0})", Newtonsoft.Json.JsonConvert.SerializeObject(request)));
                objErr.SendMail();
            }
            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 02 Sep 2016
        /// Description :   Notify the seller about the Upload Photo Request from the buyer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isDealer"></param>
        /// <param name="inquiryId"></param>
        /// <param name="buyer"></param>
        private void NotifySeller(PhotoRequest request, bool isDealer, string inquiryId, CustomerEntityBase buyer)
        {
            try
            {
                UsedBikeSellerBase seller = _objSellerRepository.GetSellerDetails(inquiryId, isDealer);
                //Form the upload url for seller email
                string listingUrl = string.Format("{0}/used/sell/uploadbasic.aspx?id={1}", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, request.ProfileId);
                if (seller != null && seller.Details != null && !String.IsNullOrEmpty(seller.Details.CustomerEmail) && !String.IsNullOrEmpty(seller.Details.CustomerName))
                {
                    if (isDealer)
                    {
                        SendEmailSMSToDealerCustomer.UsedBikePhotoRequestEmailToDealer(seller.Details.CustomerEmail, seller.Details.CustomerName, buyer.CustomerName, buyer.CustomerMobile, request.BikeName, request.ProfileId);
                    }
                    else
                    {
                        SendEmailSMSToDealerCustomer.UsedBikePhotoRequestEmailToIndividual(seller.Details, buyer, request.BikeName, listingUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("NotifySeller({0})", Newtonsoft.Json.JsonConvert.SerializeObject(request)));
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 02 Sep 2016
        /// Description :   Saves the Customer by calling CustomerDAL or updates the details if record already exists
        /// </summary>
        /// <param name="buyer"></param>
        /// <returns></returns>
        private void RegisterBuyer(CustomerEntityBase buyer)
        {
            CustomerEntity objCust = null;
            try
            {
                objCust = _objCustomer.GetByEmail(buyer.CustomerEmail);
                if (objCust != null && objCust.CustomerId > 0)
                {
                    _objCustomerRepo.UpdateCustomerMobileNumber(buyer.CustomerMobile, buyer.CustomerEmail, buyer.CustomerName);
                    buyer.CustomerId = objCust.CustomerId;
                }
                else
                {
                    objCust = new CustomerEntity() { CustomerName = buyer.CustomerName, CustomerEmail = buyer.CustomerEmail, CustomerMobile = buyer.CustomerMobile, ClientIP = CommonOpn.GetClientIP() };
                    buyer.CustomerId = _objCustomer.Add(objCust);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("RegisterBuyer({0})", Newtonsoft.Json.JsonConvert.SerializeObject(buyer)));
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 02 Sep 2016
        /// Description :   Validates the upload photo request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool isValidPhotoRequest(PhotoRequest request)
        {
            if (request != null)
            {
                if (request.Buyer != null)
                    if (!String.IsNullOrEmpty(request.ProfileId))
                        return UsedBikeProfileId.IsValidProfileId(request.ProfileId);
            }
            return false;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 02 Sep 2016
        /// Description :   Checks whether buyer has already submitted
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="inquiryId"></param>
        /// <param name="isDealer"></param>
        /// <returns></returns>
        public BikeInterestDetails ShowInterestInBike(CustomerEntityBase buyer, string profileId, bool isDealer)
        {
            bool hasShownInterest = false;
            byte consumer = default(byte);
            string inquiryId = "", consumerType = "";
            BikeInterestDetails buyerInterest = new BikeInterestDetails();
            if (buyer == null)
            {
                buyer = new CustomerEntityBase();
            }
            //Extract inquiryid and type of inquiry(individual/dealer)
            UsedBikeProfileId.SplitProfileId(profileId, out inquiryId, out consumerType);
            //set bool for dealer listing or individual
            isDealer = consumerType.Equals("D", StringComparison.CurrentCultureIgnoreCase);
            consumer = Convert.ToByte(isDealer ? 1 : 2);

            //if tempcurrentuser cookie exists return the buyers basic details
            BWCookies.GetBuyerDetailsFromCookie(ref buyer);
            //Is new Customer 
            if (buyer.CustomerId == 0 && !String.IsNullOrEmpty(buyer.CustomerEmail))
            {
                //perform customer registration with submitted details
                RegisterBuyer(buyer);
                //customer registration successful
                if (buyer.CustomerId > 0)
                {
                    //create tempcurrentuser cookie
                    string buyerData = String.Format("{0}:{1}:{2}:{3}", buyer.CustomerName, buyer.CustomerMobile, buyer.CustomerEmail, BikewaleSecurity.EncryptUserId(Convert.ToInt64(buyer.CustomerId)));
                    BWCookies.SetBuyerDetailsCookie(buyerData);
                }
            }
            if (buyer.CustomerId > 0)
            {
                hasShownInterest = _objBuyerRepository.HasShownInterestInUsedBike(isDealer, inquiryId, buyer.CustomerId);
            }
            buyerInterest.ShownInterest = hasShownInterest;
            if (hasShownInterest)
            {
                UsedBikeSellerBase seller = _objSellerRepository.GetSellerDetails(inquiryId, isDealer);
                buyerInterest.Seller = seller;
            }
            else
            {
                buyerInterest.Buyer = buyer;
            }

            return buyerInterest;
        }
    }
}
