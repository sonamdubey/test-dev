using Carwale.Entity.Classified.CarValuation;

namespace Carwale.Interfaces.Classified.CarValuation
{
    public interface IValuationRepository
    {        
        int SaveValuationRequest(ValuationRequest valuationRequest, CarValuationResults valuationResults);
        
        ValuationBaseValue GetValuationBaseValue(int versionId, int cityId, int carYear);
        
        string GetNearestValuationCity(int cityId);

        ValuationRequest GetValuationRequest(int valuationId);
    }
}
