using Bikewale.Entities.Customer;
using Bikewale.Entities.MobileVerification;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using RabbitMqPublishing;
using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Reflection;
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

            }
            else
            {
                int inquiryId = _sellBikeRepository.Add(ad);
                ad.InquiryId = (uint)inquiryId;
                string bikeName = String.Format("{0} {1} {2}", ad.Make.MakeName, ad.Model.ModelName, ad.Version.VersionName);
                string profileId = null;
                
                 if (ad.Seller.SellerType == SellerType.Individual)
                {
                    profileId = String.Format("S{0}", ad.InquiryId);
                }

                else if (ad.Seller.SellerType == SellerType.Dealer)
                {
                    profileId = String.Format("D{0}", ad.InquiryId);
                } 
                //send sms and email to seller on successful listing
                
                SendEmailSMSToDealerCustomer.UsedBikeAdEmailToIndividual(ad.Seller, profileId, bikeName, ad.Expectedprice.ToString());
                SMSTypes smsType = new SMSTypes();
                smsType.UsedSellSuccessfulListingSMS(
                    EnumSMSServiceType.SuccessfulUsedSelllistingToSeller,
                    ad.Seller.CustomerMobile,
                    profileId,
                    HttpContext.Current.Request.ServerVariables["URL"].ToString());
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
                    //Register the new customer
                    objCust = new CustomerEntity() { CustomerName = user.CustomerName, CustomerEmail = user.CustomerEmail, CustomerMobile = user.CustomerMobile };
                    user.CustomerId = _objCustomer.Add(objCust);
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

        public bool VerifyMobile(SellerEntity seller)
        {
            return _mobileVerRespo.VerifyMobileVerificationCode(seller.CustomerMobile, seller.Otp, seller.Otp);
        }

        public SellBikeAd  GetById(int inquiryId, ulong customerId)
        {
            return _sellBikeRepository.GetById(inquiryId, customerId);
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("RemoveBikePhotos: ProfileId {0}, CustomerId {1}, photoId {2}", profileId, customerId,photoId));
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
        public SellBikeImageUploadResultEntity UploadBikeImage(bool isMain, ulong customerId, string profileId, string description, HttpFileCollection imageFiles)
        {
            SellBikeImageUploadResultEntity result = new SellBikeImageUploadResultEntity();
            try
            {
                string inquiryId = String.Empty;
                string inquiryType = String.Empty;

                Utility.UsedBikeProfileId.SplitProfileId(profileId, out inquiryId, out inquiryType);

                if (null != _sellBikeRepository.GetById(Convert.ToInt32(inquiryId), customerId))
                {
                    int index = 0;
                    string imageFileName, fileExtension, folderpath, fileName, fileSaveLocation, photoId, imgUrl;
                    string originalImagePath = String.Format("/bw/used/{0}/", profileId);
                    HttpPostedFile imageFile = null;
                    result.ImageResult = new List<SellBikeImageUploadResultBase>();

                    result.Status = ImageUploadResultStatus.Success;

                    if (imageFiles.Count <= MAX_UPLOAD_FILES_LIMIT)
                    {
                        folderpath = CreateImageFileDirectory(profileId);
                        foreach (string file in imageFiles)
                        {
                            imageFile = imageFiles[file];

                            SellBikeImageUploadResultBase imgResult = new SellBikeImageUploadResultBase();

                            if (imageFile.ContentLength <= MAX_FILE_SIZE_IN_BYTES)
                            {
                                if (imageFile != null && !String.IsNullOrEmpty(imageFile.FileName))
                                {
                                    imageFileName = imageFile.FileName;

                                    if (Utility.Image.IsValidFileExtension(imageFileName, out fileExtension))
                                    {
                                        fileName = String.Format("{0}_{1}{2}", Utility.UsedBikeProfileId.GetProfileNo(profileId), DateTime.Now.ToString("yyyyMMddhhmmssfff"), fileExtension);

                                        fileSaveLocation = String.Format("{0}{1}", folderpath, fileName);

                                        imageFile.SaveAs(fileSaveLocation);

                                        //call DAL to save
                                        photoId = _sellBikeRepository.SaveBikePhotos(
                                             (index == 0 && isMain),
                                             inquiryType.Equals("D", StringComparison.CurrentCultureIgnoreCase),
                                             Convert.ToInt32(inquiryId),
                                             originalImagePath + fileName,
                                             description
                                             );

                                        if (!String.IsNullOrEmpty(photoId))
                                        {
                                            imgResult.PhotoId = photoId;

                                            imgUrl = _sellBikeRepository.UploadImageToCommonDatabase(photoId, fileName, ImageCategories.BIKEWALESELLER, originalImagePath);
                                            imgResult.ImgUrl = imgUrl;
                                            if (Uri.IsWellFormedUriString(imgUrl, UriKind.RelativeOrAbsolute))
                                            {
                                                //RabbitMq Publish code here
                                                PushToRabbitMQ(photoId, imgUrl);

                                                imgResult.Status = ImageUploadStatus.Success;
                                            }
                                            else
                                            {
                                                imgResult.Status = ImageUploadStatus.UrlNotWellFormed;
                                            }
                                        }
                                        else
                                        {
                                            imgResult.Status = ImageUploadStatus.ErrorPhotoIdGeneration;
                                        }
                                    }
                                    else
                                    {
                                        imgResult.Status = ImageUploadStatus.InvalidImageFileExtension;
                                    }
                                }
                                else
                                {
                                    imgResult.Status = ImageUploadStatus.NoFile;
                                }
                            }
                            else
                            {
                                imgResult.Status = ImageUploadStatus.MaxImageSizeExceeded;
                            }
                            result.ImageResult.Add(imgResult);
                            index++;
                        }
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("UploadBikeImage({0},{1},{2},{3},{4})", isMain, customerId, profileId, description, (imageFiles != null ? Newtonsoft.Json.JsonConvert.SerializeObject(imageFiles.AllKeys) : "")));
                objErr.SendMail();

            }

            return result;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Oct 2016
        /// Description :   Push Image upload request to RabbitMQ queue
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="imgUrl"></param>
        private void PushToRabbitMQ(string photoId, string imgUrl)
        {
            try
            {
                RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add(GetDescription(ImageKeys.ID).ToLower(), photoId);
                nvc.Add(GetDescription(ImageKeys.CATEGORY).ToLower(), "BikeWaleSeller");
                nvc.Add(GetDescription(ImageKeys.LOCATION).ToLower(), imgUrl);
                nvc.Add(GetDescription(ImageKeys.CUSTOMSIZEWIDTH).ToLower(), "-1");
                nvc.Add(GetDescription(ImageKeys.CUSTOMSIZEHEIGHT).ToLower(), "-1");
                nvc.Add(GetDescription(ImageKeys.ISWATERMARK).ToLower(), Convert.ToString(false));
                nvc.Add(GetDescription(ImageKeys.ISCROP).ToLower(), Convert.ToString(false));
                nvc.Add(GetDescription(ImageKeys.ISMAIN).ToLower(), Convert.ToString(false));
                nvc.Add(GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(true));
                nvc.Add(GetDescription(ImageKeys.ISMASTER).ToLower(), "1");
                rabbitmqPublish.PublishToQueue(Utility.BWConfiguration.Instance.ImageQueueName, nvc);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("PushToRabbitMQ({0},{1},{2},{3},{4})", photoId, imgUrl));
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Oct 2016
        /// Description :   Create Image File Directory on Host server
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        private string CreateImageFileDirectory(string profileId)
        {
            string folderpath = Utility.Image.GetPathToSaveImages(String.Format(@"\\bw\\used\\{0}\\", profileId));

            if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("localhost") < 0)
            {
                if (folderpath.IndexOf("bikewale") >= 0)
                    folderpath = folderpath.Replace(@"\\bikewale\\", @"\\carwale\\");
            }

            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            return folderpath;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Oct 2016
        /// Description :   Gets the Enum name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
