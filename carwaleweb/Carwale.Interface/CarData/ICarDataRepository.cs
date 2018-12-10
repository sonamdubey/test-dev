
using Carwale.Entity;
namespace Carwale.Interfaces.CarData
{
    public interface ICarDataRepository
    {
        CarEntity GetVersionModel(int versionId);
    }
}
