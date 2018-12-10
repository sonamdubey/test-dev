using System;
using Grpc.Core;
using System.Configuration;
using System.Threading.Tasks;
using Carwale.DAL.PredictionService;

namespace Predictive
{
    public static partial class PredictiveScore
    {
        public partial class PredictiveScoreClient
        {
            private static int _deadline = Convert.ToInt32(ConfigurationManager.AppSettings["PredictionScoreServiceWaitTime"] ?? "500");

            public PredictiveScoreClient(bool wrapper) : base(PredictiveServiceChannel.Channel)
            {
            }

            private static DateTime GetForwardTime(int incrementMillisecond)
            {
                return DateTime.Now.AddMilliseconds(incrementMillisecond).ToUniversalTime();
            }

            #region GRPC calls

            public ModelResponse GetCampaignScore(CampaignRequest request)
            {
                return GetCampaignScore(request, null, GetForwardTime(_deadline));
            }

            #endregion
        }
    }
}