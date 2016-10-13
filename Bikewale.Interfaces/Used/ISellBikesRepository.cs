
namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13 Oct 2016
    /// Summary: Interface for Sell bikes - DAL
    /// </summary>
    public interface ISellBikesRepository
    {
        uint SaveSellBikeAd();
        bool SaveSellBikeAd(uint inquryId);
        bool UpdateOtherInformation();
        bool VerifyMobile();
    }
}
