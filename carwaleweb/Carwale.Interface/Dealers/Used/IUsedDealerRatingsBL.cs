using Carwale.Entity.Dealers;

namespace Carwale.Interfaces.Dealers.Used
{
    public interface IUsedDealerRatingsBL
    {
        void SaveRating(int dealerId, string ratingText);
        UsedCarDealersRating GetRating(int dealerId);
    }
}
