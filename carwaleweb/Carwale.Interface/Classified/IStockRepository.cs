using Carwale.Entity.Classified;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified
{
    public interface IStockRepository
    {        

        List<StockSummary> GetLuxuryCarRecommendations(int carId, int dealerId, int pageId);


        ImageGalleryEntity GetImagesByProfileId(string inquiryId, bool isDealer);
        List<StockSummary> GetSimilarUsedModels(int modelId);
        IEnumerable<string> GetProfileIdsOfDealer(int dealerId);
        IEnumerable<string> GetLiveStocksByCertProgId(int certificationId);
        bool IsStockLive(string profileId);
    }//class
}//namespace
