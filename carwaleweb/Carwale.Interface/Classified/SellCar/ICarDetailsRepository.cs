using Carwale.Entity.Classified.SellCarUsed;

namespace Carwale.Interfaces.Classified.SellCar
{
    public interface ICarDetailsRepository
    {
        bool UpdateCarDetails(SellCarInfo sellCarInfo, int inquiryId);
        bool UpdateCarDetailsV1(SellCarInfo sellCarInfo, int inquiryId);
    }
}
