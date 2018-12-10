namespace Carwale.Interfaces.PriceQuote
{
    public interface IOperationsTrackingRepository
    {
        void TrackOperations(int versionId, int cityId, bool isMetallic, int sourceCityId, int updatedBy);
    }
}
