using Bikewale.Entities.Used;
using Bikewale.Notifications;
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
        public bool SaveEditedInquiry(uint inquiryId, short isApproved, int approvedBy, string profileId, string bikeName)
        {
            bool isSuccess = _sellerRepo.SaveEditedInquiry(inquiryId, isApproved, approvedBy);
            if (isSuccess)
            {
                MemCachedUtil.Remove(string.Format("BW_ProfileDetails_{0}", inquiryId));
                UsedBikeProfileDetails seller = _sellerRepo.GetUsedBikeSellerDetails((int)inquiryId, false);
                if (seller != null)
                {
                    SMSTypes newSms = new SMSTypes();
                    string modelImage = Bikewale.Utility.Image.GetModelImage(seller.HostUrl, seller.OriginalImagePath, Bikewale.Utility.ImageSize._110x61);
                    if (isApproved == 0)
                    {
                        SendEmailSMSToDealerCustomer.UsedBikeEditedRejectionEmailToSeller(seller.SellerDetails, profileId, bikeName, modelImage, seller.RideDistance, "");
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
                        SendEmailSMSToDealerCustomer.UsedBikeEditedApprovalEmailToSeller(seller.SellerDetails, profileId, bikeName, modelImage, seller.RideDistance, "");
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
