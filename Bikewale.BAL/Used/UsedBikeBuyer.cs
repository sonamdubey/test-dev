
using Bikewale.Entities.Customer;
using Bikewale.Entities.MobileVerification;
using Bikewale.Entities.UrlShortner;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.MobileVerification;
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
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IMobileVerification _mobileVerification = null;
        public UsedBikeBuyer(
            ICustomer<CustomerEntity, UInt32> objCustomer,
            IUsedBikeBuyerRepository objBuyerRepository,
            IUsedBikeSellerRepository objSellerRepository,
            ICustomerRepository<CustomerEntity, UInt32> objCustomerRepo,
            IMobileVerificationRepository mobileVerRespo,
            IMobileVerification mobileVerificetion
            )
        {
            _objCustomer = objCustomer;
            _objBuyerRepository = objBuyerRepository;
            _objSellerRepository = objSellerRepository;
            _objCustomerRepo = objCustomerRepo;
            _mobileVerRespo = mobileVerRespo;
            _mobileVerification = mobileVerificetion;
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
                ErrorClass.LogError(ex, String.Format("UploadPhotosRequest({0})", Newtonsoft.Json.JsonConvert.SerializeObject(request)));
                
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
                ErrorClass.LogError(ex, String.Format("ShowInterestInBike({0},{1})", Newtonsoft.Json.JsonConvert.SerializeObject(buyer), profileId));
                
            }

            return buyerInterest;
        }


        #region Private methods
        /// <summary>
        /// Created by  :   Sumit Kate on 03 Sep 2016
        /// Description :   Process User Cookie for Form Pre-Fill and Customer registration if new customer
        /// Modified by :   Sumit Kate on 02 Mar 2017
        /// Description :   replaced : with &
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
                        string buyerData = String.Format("{0}&{1}&{2}&{3}", buyer.CustomerName, buyer.CustomerEmail, buyer.CustomerMobile, BikewaleSecurity.EncryptUserId(Convert.ToInt64(buyer.CustomerId)));
                        BWCookies.SetBuyerDetailsCookie(buyerData);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("ProcessUserCookie({0})", Newtonsoft.Json.JsonConvert.SerializeObject(buyer)));
                
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
                string listingUrl = string.Format("{0}/used/sell/default.aspx?id={1}#uploadphoto", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, request.ProfileId.Substring(1, (request.ProfileId).Length - 1));
                if (seller != null)
                {
                    if (seller.Details != null)
                    {
                        if (!String.IsNullOrEmpty(seller.Details.CustomerEmail) && !String.IsNullOrEmpty(seller.Details.CustomerName))
                        {
                            if (isDealer)
                            {
                                //Send Email to Dealer
                                //SendEmailSMSToDealerCustomer.UsedBikePhotoRequestEmailToDealer(seller.Details.CustomerEmail, seller.Details.CustomerName, buyer.CustomerName, buyer.CustomerMobile, request.BikeName, request.ProfileId);
                                SendEmailSMSToDealerCustomer.UsedBikePhotoRequestEmailToIndividual(seller.Details, buyer, request.BikeName, listingUrl);
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
                ErrorClass.LogError(ex, String.Format("NotifySeller({0})", Newtonsoft.Json.JsonConvert.SerializeObject(request)));
                
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
                objCust = _objCustomer.GetByEmailMobile(buyer.CustomerEmail, buyer.CustomerMobile);
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
                    objCust = new CustomerEntity() { CustomerName = buyer.CustomerName, CustomerEmail = buyer.CustomerEmail, CustomerMobile = buyer.CustomerMobile, ClientIP = CurrentUser.GetClientIP() };
                    buyer.CustomerId = _objCustomer.Add(objCust);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("RegisterBuyer({0})", Newtonsoft.Json.JsonConvert.SerializeObject(buyer)));
                
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
        #endregion

        /// <summary>
        /// Created by  :   Sumit Kate on 23 Sep 2016
        /// Description :   Used Bike Purchase Inquiry Business Logic
        /// </summary>
        /// <param name="buyer"></param>
        /// <param name="profileId"></param>
        /// <param name="pageUrl"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public PurchaseInquiryResultEntity SubmitPurchaseInquiry(CustomerEntityBase buyer, string profileId, string pageUrl, ushort sourceId)
        {
            PurchaseInquiryResultEntity result = new PurchaseInquiryResultEntity();
            result.InquiryStatus = new PurchaseInquiryStatusEntity();
            ClassifiedInquiryDetailsMin inquiryDetails = null;
            bool isDealer = false, isNewInquiry = false;
            string inquiryId = "", consumerType = "";

            try
            {
                //Check for valid used bike inquiry id
                if (UsedBikeProfileId.IsValidProfileId(profileId))
                {
                    if (buyer != null)
                    {
                        //Extract inquiryid and type of inquiry(individual/dealer)
                        UsedBikeProfileId.SplitProfileId(profileId, out inquiryId, out consumerType);
                        //set bool for dealer listing or individual
                        isDealer = consumerType.Equals("D", StringComparison.CurrentCultureIgnoreCase);
                        //Get buyer details from cookie if bwcookie exists else registers the customer and creates the bwcookie for further use
                        buyer = ProcessUserCookie(buyer);
                        //Is customer valid
                        if (buyer.CustomerId > 0)
                        {
                            //check if custoemrs mobile is verified
                            if (_mobileVerRespo.IsMobileVerified(buyer.CustomerMobile, buyer.CustomerEmail))
                            {
                                //Check if buyer has crossed the daily lead limiy
                                if (_objBuyerRepository.IsBuyerEligible(buyer.CustomerMobile))
                                {
                                    //Save the customer inquiry
                                    if (_objSellerRepository.SaveCustomerInquiry(inquiryId, buyer.CustomerId, sourceId, out isNewInquiry) > 0)
                                    {
                                        //get inquiry details for notification
                                        inquiryDetails = _objSellerRepository.GetInquiryDetails(inquiryId);
                                        //get seller details
                                        UsedBikeSellerBase seller = _objSellerRepository.GetSellerDetails(inquiryId, false);
                                        result.Seller = seller.Details;
                                        result.SellerAddress = seller.Address;
                                        if (isNewInquiry)
                                        {
                                            result.InquiryStatus.Code = PurchaseInquiryStatusCode.Success;
                                            //Notify individual seller
                                            NotifyPurchaseInquiryIndividualSeller(inquiryDetails.BikeName, pageUrl, profileId, inquiryDetails.Price, seller.Details, buyer);
                                            //Notify buyer
                                            NotifyPurchaseInquiryBuyer(inquiryDetails.BikeName, pageUrl, profileId, buyer, seller, inquiryDetails);
                                        }
                                        else
                                        {
                                            result.InquiryStatus.Code = PurchaseInquiryStatusCode.DuplicateUsedBikeInquiry;
                                        }
                                    }
                                }
                                else
                                {
                                    result.InquiryStatus.Code = PurchaseInquiryStatusCode.MaxLimitReached;
                                }
                            }
                            else
                            {
                                MobileVerificationEntity mobileVer = null;
                                mobileVer = _mobileVerification.ProcessMobileVerification(buyer.CustomerEmail, buyer.CustomerMobile);

                                SMSTypes st = new SMSTypes();
                                st.SMSMobileVerification(buyer.CustomerMobile, buyer.CustomerName, mobileVer.CWICode, pageUrl);
                                result.InquiryStatus.Code = PurchaseInquiryStatusCode.MobileNotVerified;
                            }
                        }
                    }
                    else
                    {
                        result.InquiryStatus.Code = PurchaseInquiryStatusCode.InvalidCustomerInfo;
                    }
                }
                else
                {
                    result.InquiryStatus.Code = PurchaseInquiryStatusCode.InvalidRequest;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("SubmitPurchaseInquiry({0},{1})", Newtonsoft.Json.JsonConvert.SerializeObject(buyer), profileId));
                
            }
            return result;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 23 Sep 2016
        /// Description :   Sends sms and email to used bike listing individual seller
        /// </summary>
        /// <param name="bike"></param>
        /// <param name="pageUrl"></param>
        /// <param name="profileId"></param>
        /// <param name="formattedPrice"></param>
        /// <param name="seller"></param>
        /// <param name="buyer"></param>
        private void NotifyPurchaseInquiryIndividualSeller(string bike, string pageUrl, string profileId, uint formattedPrice, CustomerEntityBase seller, CustomerEntityBase buyer)
        {
            try
            {
                string msg = String.Format("New inquiry on BikeWale for your {0}. Buyer details: {1},{2}.", bike, buyer.CustomerName, buyer.CustomerMobile);
                SMSTypes st = new SMSTypes();
                st.UsedPurchaseInquirySMS(EnumSMSServiceType.UsedPurchaseInquiryIndividualSeller, seller.CustomerMobile, msg, pageUrl);

                SendEmailSMSToDealerCustomer.UsedBikePurchaseInquiryEmailToIndividual(seller, buyer, profileId, bike, Bikewale.Utility.Format.FormatNumeric(formattedPrice.ToString()));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("NotifyPurchaseInquiryIndividualSeller({0},{1},{2},{3},{4},{5})", bike, pageUrl, profileId, formattedPrice, Newtonsoft.Json.JsonConvert.SerializeObject(seller), Newtonsoft.Json.JsonConvert.SerializeObject(buyer)));
                
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 23 Sep 2016
        /// Description :   Sends sms and email to used bike inquiry buyer
        /// </summary>
        /// <param name="bike"></param>
        /// <param name="pageUrl"></param>
        /// <param name="profileId"></param>
        /// <param name="buyer"></param>
        /// <param name="seller"></param>
        /// <param name="inquiryDetails"></param>
        private void NotifyPurchaseInquiryBuyer(string bike, string pageUrl, string profileId, CustomerEntityBase buyer, UsedBikeSellerBase seller, ClassifiedInquiryDetailsMin inquiryDetails)
        {

            try
            {
                UrlShortnerResponse response = null;
                string listingUrl = String.Format("{0}/used/bikes-in-{1}/{2}-{3}-{4}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, inquiryDetails.CityMaskingName, inquiryDetails.MakeMaskingName, inquiryDetails.ModelMaskingName, profileId);
                if (!String.IsNullOrEmpty(listingUrl))
                {
                    response = new UrlShortner().GetShortUrl(listingUrl);
                }
                string shortUrl = response != null ? response.ShortUrl : listingUrl;
                string msg = string.Format("For {0} you selected at BikeWale, call its seller {1} at {2}. Visit {3} for more details.", bike, seller.Details.CustomerName, seller.Details.CustomerMobile, shortUrl);
                SMSTypes st = new SMSTypes();
                st.UsedPurchaseInquirySMS(EnumSMSServiceType.UsedPurchaseInquiryIndividualBuyer, buyer.CustomerMobile, msg, pageUrl);

                SendEmailSMSToDealerCustomer.UsedBikePurchaseInquiryEmailToBuyer(seller.Details, buyer, seller.Address, profileId, bike, inquiryDetails.KmsDriven.ToString(), "", Bikewale.Utility.Format.FormatNumeric(inquiryDetails.Price.ToString()), listingUrl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("NotifyPurchaseInquiryBuyer({0},{1},{2},{3},{4},{5})", bike, pageUrl, profileId,
                    Newtonsoft.Json.JsonConvert.SerializeObject(buyer),
                    Newtonsoft.Json.JsonConvert.SerializeObject(seller),
                    Newtonsoft.Json.JsonConvert.SerializeObject(inquiryDetails)
                    ));
                
            }
        }
    }
}
