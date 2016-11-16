using Bikewale.Entities.Customer;
using Bikewale.Entities.MobileVerification;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.BAL.UsedBikes
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 14 Oct 2016
    /// Summary:    (BAL) Business access layer for sell bikes
    /// </summary>
    public class SellBikes : ISellBikes
    {
        private const uint MAX_FILE_SIZE_IN_BYTES = 4194304;
        private const uint MAX_UPLOAD_FILES_LIMIT = 10;
        protected bool isEdit = false;
        private readonly ISellBikesRepository<SellBikeAd, int> _sellBikeRepository = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly ICustomerRepository<CustomerEntity, UInt32> _objCustomerRepo = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IUsedBikeBuyerRepository _objBuyerRepository = null;
        private readonly IMobileVerification _mobileVerification = null;
        private readonly IUsedBikeSellerRepository _sellerRepository = null;
        public SellBikes(ISellBikesRepository<SellBikeAd, int> sellBikeRepository,
            ICustomer<CustomerEntity, UInt32> objCustomer,
            ICustomerRepository<CustomerEntity, UInt32> objCustomerRepo,
            IMobileVerification mobileVerification,
            IMobileVerificationRepository mobileVerRespo,
            IUsedBikeBuyerRepository objBuyerRepository,
            IUsedBikeSellerRepository sellerRepository)
        {
            _sellBikeRepository = sellBikeRepository;
            _objCustomer = objCustomer;
            _objCustomerRepo = objCustomerRepo;
            _objBuyerRepository = objBuyerRepository;
            _mobileVerRespo = mobileVerRespo;
            _mobileVerification = mobileVerification;
            _sellerRepository = sellerRepository;
        }


        public SellBikeInquiryResultEntity SaveSellBikeAd(SellBikeAd ad)
        {
            SellBikeInquiryResultEntity result = new SellBikeInquiryResultEntity();
            result.Status = new SellBikeAdStatusEntity();
            // Check if user is registered
            if (RegisterUser(ad.Seller) > 0)
            {
                // Check if customer is fake
                if (!IsFakeCustomer(ad.Seller.CustomerId))
                {
                    AddOrUpdateAd(ad);
                    result.InquiryId = ad.InquiryId;
                    //Check if mobile verified
                    if (!_mobileVerRespo.IsMobileVerified(ad.Seller.CustomerMobile, ad.Seller.CustomerEmail))
                    {
                        // Send OTP
                        MobileVerificationEntity mobileVer = null;
                        mobileVer = _mobileVerification.ProcessMobileVerification(ad.Seller.CustomerEmail, ad.Seller.CustomerMobile);
                        SMSTypes st = new SMSTypes();
                        st.SMSMobileVerification(ad.Seller.CustomerMobile, ad.Seller.CustomerName, mobileVer.CWICode, ad.PageUrl);
                        result.Status.Code = SellAdStatus.MobileUnverified;
                    }
                    else
                    {
                        result.Status.Code = SellAdStatus.MobileVerified;
                        if(!isEdit)
                        SendNotification(ad);
                    }
                }
                else // Redirect user
                {
                    result.Status.Code = SellAdStatus.Fake;
                }
            }
            else
            {
                //Register user
                AddOrUpdateAd(ad);
            }

            result.CustomerId = ad.Seller.CustomerId;

            return result;
        }

        private void AddOrUpdateAd(SellBikeAd ad)
        {

            if (ad.InquiryId > 0)
            {
                _sellBikeRepository.Update(ad);
                isEdit = true;
            }
            else
            {
                int inquiryId = _sellBikeRepository.Add(ad);
                ad.InquiryId = (uint)inquiryId;
            }
        }

        private ulong RegisterUser(SellerEntity user)
        {
            CustomerEntity objCust = null;
            try
            {
                //Check if Customer exists
                objCust = _objCustomer.GetByEmail(user.CustomerEmail);
                if (objCust != null && objCust.CustomerId > 0)
                {
                    //If exists update the mobile number and name
                    _objCustomerRepo.UpdateCustomerMobileNumber(user.CustomerMobile, user.CustomerEmail, user.CustomerName);
                    //set customer id for further use
                    user.CustomerId = objCust.CustomerId;
                }
                else
                {
                    //Register the new customer and send login details
                    objCust = new CustomerEntity() { CustomerName = user.CustomerName, CustomerEmail = user.CustomerEmail, CustomerMobile = user.CustomerMobile };
                    user.CustomerId = _objCustomer.Add(objCust);
                    SendEmailSMSToDealerCustomer.CustomerRegistrationEmail(objCust.CustomerEmail, objCust.CustomerName, objCust.Password);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("RegisteredUser({0})", Newtonsoft.Json.JsonConvert.SerializeObject(user)));
                objErr.SendMail();
            }
            return user.CustomerId;
        }

        public bool UpdateOtherInformation(SellBikeAdOtherInformation adInformation, int inquiryAd, ulong customerId)
        {
            return _sellBikeRepository.UpdateOtherInformation(adInformation, inquiryAd, customerId);
        }
        /// <summary>
        /// Modified By : Aditi Srivastava on 10 Nov 2016
        /// Description : Send email notifcation for successful listing when mobile is verified
        /// </summary>
        /// <param name="seller"></param>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        public bool VerifyMobile(SellerEntity seller)
        {
            bool mobileVerified= _mobileVerRespo.VerifyMobileVerificationCode(seller.CustomerMobile, seller.Otp, seller.Otp);
            isEdit = seller.IsEdit;
            if (mobileVerified && !isEdit)
            {
                //send notification for successful listing
                SellBikeAd ad = _sellBikeRepository.GetById((int)seller.InquiryId, seller.CustomerId);
                SendNotification(ad);
            }
            return mobileVerified;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 10 Nov 2016
        /// Description: To send email and sms to seller on succesful listing of used bike
        /// </summary>
        public void SendNotification(SellBikeAd ad)
        {
            string bikeName = String.Format("{0} {1} {2}", ad.Make.MakeName, ad.Model.ModelName, ad.Version.VersionName);
            string profileId = (ad.Seller.SellerType == SellerType.Individual) ? String.Format("S{0}", ad.InquiryId) : String.Format("D{0}", ad.InquiryId);
            SendEmailSMSToDealerCustomer.UsedBikeAdEmailToIndividual(ad.Seller, profileId, bikeName, ad.Expectedprice.ToString());
            SMSTypes smsType = new SMSTypes();
            smsType.UsedSellSuccessfulListingSMS(
                EnumSMSServiceType.SuccessfulUsedSelllistingToSeller,
                ad.Seller.CustomerMobile,
                profileId,
                HttpContext.Current.Request.ServerVariables["URL"].ToString());
        }
        /// <summary>
        /// Modified by :   Sumit Kate on 02 Nov 2016
        /// Description :   Return bike photos along with profile details
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public SellBikeAd GetById(int inquiryId, ulong customerId)
        {
            try
            {
                SellBikeAd sellBike = _sellBikeRepository.GetById(inquiryId, customerId);
                if (sellBike != null)
                {
                    sellBike.Photos = _sellBikeRepository.GetBikePhotos(inquiryId, false);
                    if (sellBike.Photos != null)
                    {
                        foreach (BikePhoto photo in sellBike.Photos)
                        {
                            if (!photo.HostUrl.Contains("aeplcdn"))
                            {
                                photo.ImageUrl = String.Format("http://{0}{1}", photo.HostUrl, photo.OriginalImagePath);
                            }
                            else
                            {
                                photo.ImageUrl = Utility.Image.GetPathToShowImages(photo.OriginalImagePath, photo.HostUrl, ImageSize._144x81);
                            }
                        }
                    }
                }
                return sellBike;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("GetById({0},{1})", inquiryId, customerId));
                objErr.SendMail();
                return null;
            }
        }

        public bool IsFakeCustomer(ulong custId)
        {
            return _sellBikeRepository.IsFakeCustomer(custId);
        }
        /// <summary>
        /// Created By: Aditi Srivastava on 27 Oct 2016
        /// Description : Function to remove bike photos
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="inquiryId"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        public bool RemoveBikePhotos(ulong customerId, string profileId, string photoId)
        {
            bool isSuccess = false;
            try
            {
                int inquiryId = Convert.ToInt32(profileId.Substring(1));
                if (customerId > 0 && _sellBikeRepository.GetById(inquiryId, customerId) != null)
                {
                    isSuccess = _sellerRepository.RemoveBikePhotos(inquiryId, photoId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("RemoveBikePhotos: ProfileId {0}, CustomerId {1}, photoId {2}", profileId, customerId, photoId));
                objErr.SendMail();
            }

            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Oct 2016
        /// Description :   Sell Bike Image upload
        /// Algorithm   :   
        /// 1	read file name
        /// 2	if file name is blank
        /// 	2.1	Show alert message "Please select a file to upload" and exit
        /// 3	is valid file extension
        /// 	3.1	show error message "You are trying to upload invalid file. We accept only jpg, gif and png file formats." and exit
        /// 4	create path to upload image to server path
        /// 	4.1	if host is localhost then set directory path from HttpContext.Current.Request["APPL_PHYSICAL_PATH"] + \bw\used\S<inquiryId>\
        /// 	4.2 if host is not localhost then set directory path from ConfigurationManager.AppSettings["imgPathFolder"] + \bw\used\S<inquiryId>\
        /// 5	save file with name <inquiryid>_timestamp.ext to directory path and save path to db call classified_bikephotos_insert and on successful insert returns photoid
        /// 6	save newly uploaded image details for RabbitMQ processing. call img_allbikephotosinsert(photoid, imageFileName,BIKEWALESELLER,/bw/used/S<inquiryId>/) returns absolute image url
        /// 	6.1	if image url is absolute or relative, push the image details to rabbitMQ for further upload to amazon s3 server
        /// 7	show upload image using temp path.(image will be uploaded to s3 by image consumer and path will be host url will be changed by it).
        /// </summary>
        /// <param name="isMain"></param>
        /// <param name="customerId"></param>
        /// <param name="profileId"></param>
        /// <param name="description"></param>
        /// <param name="imageFiles"></param>
        /// <returns></returns>
        public SellBikeImageUploadResultEntity UploadBikeImage(bool isMain, ulong customerId, string profileId, string fileExtension, string description)
        {
            SellBikeImageUploadResultEntity result = new SellBikeImageUploadResultEntity();
            try
            {
                string inquiryId = String.Empty;
                string inquiryType = String.Empty;
                Utility.UsedBikeProfileId.SplitProfileId(profileId, out inquiryId, out inquiryType);
                IEnumerable<BikePhoto> photos = null;
                int inqId = Convert.ToInt32(inquiryId);
                if (null != _sellBikeRepository.GetById(inqId, customerId))
                {
                    int photoCount = 0;
                    string fileName, photoId;
                    string originalImagePath = String.Format("/bw/used/{0}/", profileId);
                    result.ImageResult = new List<SellBikeImageUploadResultBase>();

                    photos = _sellBikeRepository.GetBikePhotos(inqId, false);
                    photoCount = photos != null ? photos.Count() : 0;
                    result.Status = ImageUploadResultStatus.Success;

                    if (MAX_UPLOAD_FILES_LIMIT > photoCount)
                    {

                        SellBikeImageUploadResultBase imgResult = new SellBikeImageUploadResultBase();
                        fileName = String.Format("{0}_{1}_{2}{3}",
                            Utility.UsedBikeProfileId.GetProfileNo(profileId),
                            DateTime.Now.ToString("yyyyMMddhhmmssfff"),
                            RandomNoGenerator.GetUniqueKey(10),
                            fileExtension);
                        photoId = _sellBikeRepository.SaveBikePhotos(
                             isMain,
                             inquiryType.Equals("D", StringComparison.CurrentCultureIgnoreCase),
                             inqId,
                             originalImagePath + fileName,
                             description
                             );
                        imgResult.PhotoId = photoId;
                        if (!String.IsNullOrEmpty(photoId))
                        {
                            imgResult.Status = ImageUploadStatus.Success;
                        }
                        else
                        {
                            imgResult.Status = ImageUploadStatus.ErrorPhotoIdGeneration;
                        }
                        result.ImageResult.Add(imgResult);

                    }
                    else
                    {
                        result.Status = ImageUploadResultStatus.FileUploadLimitExceeded;
                    }
                }
                else
                {
                    result.Status = ImageUploadResultStatus.UnauthorizedAccess;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("UploadBikeImage({0},{1},{2},{3})", isMain, customerId, profileId, description));
                objErr.SendMail();
                result.Status = ImageUploadResultStatus.InternalError;

            }
            return result;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 03 Nov 2016
        /// Description :   Marks Main Image for gived profile id
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="customerId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public bool MakeMainImage(uint photoId, ulong customerId, string profileId)
        {
            bool isSuccess = false;
            try
            {
                string inquiryId = String.Empty;
                string inquiryType = String.Empty;
                int inqId = 0;
                if (Utility.UsedBikeProfileId.IsValidProfileId(profileId))
                {
                    Utility.UsedBikeProfileId.SplitProfileId(profileId, out inquiryId, out inquiryType);
                    if (!String.IsNullOrEmpty(inquiryId))
                    {
                        inqId = Convert.ToInt32(inquiryId);
                        if (customerId > 0 && _sellBikeRepository.GetById(inqId, customerId) != null)
                        {
                            isSuccess = _sellBikeRepository.MarkMainImage(inqId, photoId, inquiryType.Equals("D", StringComparison.CurrentCultureIgnoreCase));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("MakeMainImage({0},{1},{2})", photoId, customerId, profileId));
                objErr.SendMail();
            }

            return isSuccess;
        }
    }
}
