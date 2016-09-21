
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
            try
            {
                //Check for valid photo request
                if (isValidPhotoRequest(request))
                {
                    buyer = request.Buyer;
                    //Extract inquiryid and type of inquiry(individual/dealer)
                    UsedBikeProfileId.SplitProfileId(request.ProfileId, out inquiryId, out consumerType);
                    //set bool for dealer listing or individual
                    isDealer = consumerType.Equals("D", StringComparison.CurrentCultureIgnoreCase);
                    consumer = Convert.ToByte(isDealer ? 1 : 2);
                    //Process Used bike customer cookie
                    buyer = ProcessUserCookie(buyer);
                    //If valid customer
                    if (buyer.CustomerId > 0)
                    {
                        //Save Photo request
                        isSuccess = _objBuyerRepository.UploadPhotosRequest(inquiryId, buyer.CustomerId, consumer, request.Message);
                        if (isSuccess)
                        {
                            //Notify the seller about photo upload request
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
        /// Description :   Checks whether buyer has already submitted
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="inquiryId"></param>
        /// <param name="isDealer"></param>
        /// <returns></returns>
        public BikeInterestDetails ShowInterestInBike(CustomerEntityBase buyer, string profileId, bool isDealer)
        {
            string inquiryId = "", consumerType = "";
            BikeInterestDetails buyerInterest = new BikeInterestDetails();
            try
            {
                if (buyer == null)
                {
                    buyer = new CustomerEntityBase();
                }
                //Extract inquiryid and type of inquiry(individual/dealer)
                UsedBikeProfileId.SplitProfileId(profileId, out inquiryId, out consumerType);
                //set bool for dealer listing or individual
                isDealer = consumerType.Equals("D", StringComparison.CurrentCultureIgnoreCase);
                //Process Used bike customer cookie
                buyer = ProcessUserCookie(buyer);
                //If valid customer
                if (buyer.CustomerId > 0)
                {
                    //Check if shown interest already
                    buyerInterest.ShownInterest = _objBuyerRepository.HasShownInterestInUsedBike(isDealer, inquiryId, buyer.CustomerId);
                }
                //If already interest shown get seller info
                if (buyerInterest.ShownInterest)
                {
                    UsedBikeSellerBase seller = _objSellerRepository.GetSellerDetails(inquiryId, isDealer);
                    buyerInterest.Seller = seller;
                }
                else
                {
                    buyerInterest.Buyer = buyer;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("ShowInterestInBike({0},{1})", Newtonsoft.Json.JsonConvert.SerializeObject(buyer), profileId));
                objErr.SendMail();
            }

            return buyerInterest;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 03 Sep 2016
        /// Description :   Process User Cookie for Form Pre-Fill and Customer registration if new customer
        /// </summary>
        /// <param name="buyer"></param>
        /// <returns></returns>
        private CustomerEntityBase ProcessUserCookie(CustomerEntityBase buyer)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("ProcessUserCookie({0})", Newtonsoft.Json.JsonConvert.SerializeObject(buyer)));
                objErr.SendMail();
            }
            return buyer;
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
                //Get Sellers info
                UsedBikeSellerBase seller = _objSellerRepository.GetSellerDetails(inquiryId, isDealer);
                //Form the upload url for seller email
                string listingUrl = string.Format("{0}/used/sell/uploadbasic.aspx?id={1}", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, request.ProfileId);
                if (seller != null)
                {
                    if (seller.Details != null)
                    {
                        if (!String.IsNullOrEmpty(seller.Details.CustomerEmail) && !String.IsNullOrEmpty(seller.Details.CustomerName))
                        {
                            if (isDealer)
                            {
                                //Send Email to Dealer
                                SendEmailSMSToDealerCustomer.UsedBikePhotoRequestEmailToDealer(seller.Details.CustomerEmail, seller.Details.CustomerName, buyer.CustomerName, buyer.CustomerMobile, request.BikeName, request.ProfileId);
                            }
                            else
                            {
                                //Send Email to Individual seller
                                SendEmailSMSToDealerCustomer.UsedBikePhotoRequestEmailToIndividual(seller.Details, buyer, request.BikeName, listingUrl);
                            }
                        }
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
                //Check if Customer exists
                objCust = _objCustomer.GetByEmail(buyer.CustomerEmail);
                if (objCust != null && objCust.CustomerId > 0)
                {
                    //If exists update the mobile number and name
                    _objCustomerRepo.UpdateCustomerMobileNumber(buyer.CustomerMobile, buyer.CustomerEmail, buyer.CustomerName);
                    //set customer id for further use
                    buyer.CustomerId = objCust.CustomerId;
                }
                else
                {
                    //Register the new customer
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
    }
}
