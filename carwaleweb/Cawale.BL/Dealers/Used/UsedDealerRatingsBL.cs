using Carwale.Entity.Dealers;
using Carwale.Interfaces.Dealers.Used;
using Carwale.Interfaces.Stock;

namespace Carwale.BL.Dealers.Used
{
    public class UsedDealerRatingsBL : IUsedDealerRatingsBL
    {
        private readonly IUsedDealerRatingsRepository _ratingsRepository;
        private readonly IStockBL _stockBL;
        public UsedDealerRatingsBL(IUsedDealerRatingsRepository ratingsRepository, IStockBL stockBL)
        {
            _ratingsRepository = ratingsRepository;
            _stockBL = stockBL;
        }
        public UsedCarDealersRating GetRating(int dealerId)
        {
            return _ratingsRepository.GetRating(dealerId);
        }
        public void SaveRating(int dealerId, string ratingText)
        {
            _ratingsRepository.SaveRating(dealerId, ratingText);
            _stockBL.RefreshESStockOfDealer(dealerId);
        }
    }
}
