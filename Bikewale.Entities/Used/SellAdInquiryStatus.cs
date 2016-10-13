
namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13 Oct 2016
    /// Summary: Enum for Sell Bike data inquiry status
    /// </summary>
    public enum SellAdInquiryStatus
    {
        Verified = 1, // live and approved
        Fake = 2,
        Sold = 3,
        MobileUnverified = 4,
        VeriFiedButNotApproved = 5 // Mobile is verified but listing is yet to be approved
    }
}
