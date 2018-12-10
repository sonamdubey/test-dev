using Carwale.Entity.Dealers;

namespace Carwale.Interfaces.Dealers.Used
{
    public interface IUsedDealerRatingsRepository
    {
        UsedCarDealersRating GetRating(int dealerId);
        void SaveRating(int dealerId, string ratingText);
    }
}
