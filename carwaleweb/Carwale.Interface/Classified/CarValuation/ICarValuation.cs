using Carwale.DTOs.Classified;
using Carwale.Entity.Classified.CarValuation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified.CarValuation
{
    public interface ICarValuation
    {
        CarValuationResults GetValuation(ValuationRequest valuationRequest);
        CarValuationResults CalculateValuation(ValuationRequest valuationRequest);
        ResultsRecommendation GetValuationSuggestions(ValuationRequest valuationRequest, CarValuationResults valuationResults);
    }
}
