using AEPLCore.Logging;
using Bhrigu;
using Carwale.Entity.CarData;
using System;
using System.Configuration;

namespace Carwale.DAL.ApiGateway.Extensions.SimilarCar
{
    public static class SimilarCarApiGatewayCallerExtensions
    {
        private static Logger Logger = LoggerFactory.GetLogger();
        private static string _module = ConfigurationManager.AppSettings["SimilarCarModuleName"];

        public static IApiGatewayCaller GenerateSimilarCarCallerRequest(this IApiGatewayCaller caller, SimilarCarRequest similarRequest)
        {
            try
            {
                if (caller != null && similarRequest != null)
                {
                    RecommendationRequest request = new RecommendationRequest
                    {
                        Application = Application.Carwale,
                        Cookieid = similarRequest.UserIdentifier ?? string.Empty,
                        Enableboost = !string.IsNullOrWhiteSpace(similarRequest.UserIdentifier) && similarRequest.IsBoost,
                        Recommendationcount = similarRequest.Count,
                        Item = similarRequest.CarId,
                        Itemtype = ItemType.Models
                    };
                    caller.Add(_module, "GetRecommendation", request);
                }
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is InvalidCastException)
            {
                Logger.LogException(ex);
                return null;
            }
            return caller;
        }
    }
}
