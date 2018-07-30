using Bikewale.Entities.BikeBooking;
using Carwale.Entity.PaymentGateway;

namespace Bikewale.Interfaces.BikeBooking
{
    public interface IBikeBooking
    {
        BookingResults DoBooking(TransactionDetails entity, string sourceType);
    }
}
