using Carwale.Entity.CarData;
using System.Collections.Generic;

namespace Carwale.Interfaces.CarData
{
    public interface ICarMileage
    {
        List<MileageDataEntity> GetMileageData(List<CarVersions> versionList);
    }
}
