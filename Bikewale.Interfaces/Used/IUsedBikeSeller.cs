
namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by :Sangram Nandkhile on 05 Nov 2016
    /// Desc: Interface for Used Bike Seller Repository
    /// </summary>
    public interface IUsedBikeSeller
    {
        bool RepostSellBikeAd(int inquiryId, ulong customerId);
    }
}
