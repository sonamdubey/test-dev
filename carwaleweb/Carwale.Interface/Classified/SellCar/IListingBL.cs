using Carwale.Entity.Classified.SellCarUsed;

namespace Carwale.Interfaces.Classified.SellCar
{
    public interface IListingsBL
    {
        void PushToQueue(Listing listing);
        bool UpdateExpiredListings();
    }
}
