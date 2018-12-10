using Carwale.Entity.CarData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CarData
{
    public interface ICarRecommendationLogic
    {
        List<int> GetSimilarCars(SimilarCarRequest request);
        List<SimilarCarModels> GetSimilarCarsByModel(SimilarCarRequest request);
    }
}
