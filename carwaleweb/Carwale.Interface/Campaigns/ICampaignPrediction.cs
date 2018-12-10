using Carwale.Entity.Campaigns;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.Interfaces.Campaigns
{
    public interface ICampaignPrediction
    {
        PredictionCampaignRequest GetPredictionModelRequest(HttpRequest httpRequest, int modelId, int cityId, int zoneId, int platformId);
        PredictionModelResponse GetPredictionModelResponse(HttpRequest httpRequest, int modelId, int cityId, int zoneId, int platformId);
    }
}
