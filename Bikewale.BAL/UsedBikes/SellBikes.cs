using Bikewale.Entities.Customer;
using Bikewale.Entities.MobileVerification;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using System;

namespace Bikewale.BAL.UsedBikes
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 14 Oct 2016
    /// Summary:    (BAL) Business access layer for sell bikes
    /// </summary>
    public class SellBikes : ISellBikes
    {
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

    }
}
