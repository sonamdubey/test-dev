using System;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by  :   Sangram Nandkhile on 14 oct 2016
    /// Description :   Sell bike Status Entity.
    /// It describes the seller ad result status
    /// </summary>
    public class SellBikeAdStatusEntity
    {
        public UInt16 Status
        {
            get
            {
                return Convert.ToUInt16(this.Code);
            }
        }
        public SellAdStatus Code { get; set; }
        public string Message
        {
            get
            {
                switch (Code)
                {
                    case SellAdStatus.Approved:
                        return "Approved";
                    case SellAdStatus.Fake:
                        return "User has been marked fake";
                    case SellAdStatus.Sold:
                        return "Sold";
                    case SellAdStatus.MobileUnverified:
                        return "OTP generated";
                    case SellAdStatus.MobileVerified:
                        return "Mobile has been verified";
                    default:
                        return string.Empty;
                }
            }
        }
    }

    public class SellBikeInquiryResultEntity
    {
        public SellBikeAdStatusEntity Status { get; set; }
        public uint InquiryId { get; set; }
    }
}
