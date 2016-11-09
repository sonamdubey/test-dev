
namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13 Oct 2016
    /// Summary: Interface for Sell bikes - BAL
    /// </summary>
    public interface ISellBikes
    {
        uint SaveSellBikeAd();
        bool SaveSellBikeAd(uint inquryId);
        bool UpdateOtherInformation();
        bool VerifyMobile();
    }
}
