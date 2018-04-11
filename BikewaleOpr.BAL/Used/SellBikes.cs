using Bikewale.Entities.Used;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Interface.Used;
using System.Collections.Generic;
using System.Web;

namespace BikewaleOpr.BAL.Used
{
    /// <summary>
    /// Created by:Sangram Nandkhile on 24 Oct 2016
    /// Desc: Repository for Sell your bike 
    /// </summary>
    public class SellBikes : ISellBikes
    {
        private ISellerRepository _sellerRepo;

        public SellBikes(ISellerRepository sellerRepository)
        {
            _sellerRepo = sellerRepository;
        }

        public IEnumerable<SellBikeAd> GetClassifiedPendingInquiries()
        {
            return _sellerRepo.GetClassifiedPendingInquiries();
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 24 Oct 2016
        /// Summary    : To update edited entries
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="isApproved"></param>
        /// <param name="approvedBy"></param>
        /// <returns></returns>
        public bool SaveEditedInquiry(uint inquiryId, short isApproved, int approvedBy, string profileId, string bikeName, uint modelId)
        {
            bool isSuccess = _sellerRepo.SaveEditedInquiry(inquiryId, isApproved, approvedBy);
            if (isSuccess)
            {
                MemCachedUtil.Remove(string.Format("BW_ProfileDetails_V1_{0}", inquiryId));
                UsedBikeProfileDetails seller = _sellerRepo.GetUsedBikeSellerDetails((int)inquiryId, false);
                if (seller != null)
                {
                    SMSTypes newSms = new SMSTypes();
                    string modelImage = Bikewale.Utility.Image.GetPathToShowImages(seller.OriginalImagePath, seller.HostUrl, Bikewale.Utility.ImageSize._110x61);
                    if (isApproved == 0)
                    {
                        SendEmailSMSToDealerCustomer.UsedBikeEditedRejectionEmailToSeller(seller.SellerDetails, profileId, bikeName, modelImage, Format.FormatNumeric(seller.RideDistance));
                        newSms.RejectionEditedUsedSellListingSMS(
                            EnumSMSServiceType.RejectionEditedUsedBikeListingToSeller,
                            seller.SellerDetails.CustomerMobile,
                            profileId,
                            seller.SellerDetails.CustomerName,
                            HttpContext.Current.Request.ServerVariables["URL"]
                            );
                    }
                    else
                    {
                        string qEncoded = Utils.Utils.EncryptTripleDES(string.Format("sourceid={0}", (int)Bikewale.Entities.UserReviews.UserReviewPageSourceEnum.UsedBikes_Email));

                        string writeReview = string.Format("{0}/rate-your-bike/{1}/?q={2}", Bikewale.Utility.BWOprConfiguration.Instance.BwHostUrl, modelId, qEncoded);
                        SendEmailSMSToDealerCustomer.UsedBikeEditedApprovalEmailToSeller(seller.SellerDetails, profileId, bikeName, modelImage, Format.FormatNumeric(seller.RideDistance), writeReview);
                        newSms.ApprovalEditedUsedSellListingSMS(
                            EnumSMSServiceType.ApprovalEditedUsedBikeListingToSeller,
                            seller.SellerDetails.CustomerMobile,
                            profileId,
                            seller.SellerDetails.CustomerName,
                            HttpContext.Current.Request.ServerVariables["URL"]
                            );
                    }
                }
            }
            return isSuccess;
        }
    }
}
