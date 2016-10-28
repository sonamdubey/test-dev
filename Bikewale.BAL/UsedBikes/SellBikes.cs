﻿using Bikewale.Entities.Customer;
using Bikewale.Entities.MobileVerification;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using RabbitMqPublishing;
using System;
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
        public SellBikes(ISellBikesRepository<SellBikeAd, int> sellBikeRepository,
            ICustomer<CustomerEntity, UInt32> objCustomer,
            ICustomerRepository<CustomerEntity, UInt32> objCustomerRepo,
            IMobileVerification mobileVerification,
            IMobileVerificationRepository mobileVerRespo,
            IUsedBikeBuyerRepository objBuyerRepository)
        {
            _sellBikeRepository = sellBikeRepository;
            _objCustomer = objCustomer;
            _objCustomerRepo = objCustomerRepo;
            _objBuyerRepository = objBuyerRepository;
            _mobileVerRespo = mobileVerRespo;
            _mobileVerification = mobileVerification;
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

        public SellBikeAd GetById(int inquiryId, ulong customerId)
        {
            return _sellBikeRepository.GetById(inquiryId, customerId);
        }

        public bool IsFakeCustomer(ulong custId)
        {
            return _sellBikeRepository.IsFakeCustomer(custId);
        }

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
