using Bikewale.Interfaces.Used;

namespace Bikewale.BAL.UsedBikes
{
    /// <summary>
    /// Created by :Sangram Nandkhile on 05 Nov 2016
    /// Desc: Used Bike Seller Repository
    /// </summary>
    public class UsedBikeSeller : IUsedBikeSeller
    {
        private readonly IUsedBikeSellerRepository _sellerRepository = null;
        public UsedBikeSeller(IUsedBikeSellerRepository sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }
        public bool RepostSellBikeAd(int inquiryId, ulong customerId)
        {
            return _sellerRepository.RepostSellBikeAd(inquiryId, customerId);
        }
    }
}
