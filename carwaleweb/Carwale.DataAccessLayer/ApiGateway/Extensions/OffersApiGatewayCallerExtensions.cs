using AEPLCore.Logging;
using Carwale.Entity.Enum;
using Carwale.Entity.OffersV1;
using Google.Protobuf;
using Offers.Protos.ProtoFiles;
using System;
using System.Configuration;

namespace Carwale.DAL.ApiGateway.Extensions.Offers
{
    public static class OffersApiGatewayCallerExtensions
    {
        private static Logger Logger = LoggerFactory.GetLogger();
        private static string _module = ConfigurationManager.AppSettings["OffersModuleName"];

        public static bool AggregateGetOffersOnCriteria(this IApiGatewayCaller caller, OfferInput offerInput)
        {
            bool isCallAdded = false;
            try
            {
                IMessage message = new OfferCriteria
                {
                    ApplicationId = (int)Application.CarWale,
                    RequiredOfferCount = 1, //Only one offer is required currently
                    ModelRule = new ModelRule { MakeId = offerInput.MakeId, ModelId = offerInput.ModelId, VersionId = offerInput.VersionId },
                    CityRule = new CityRule { StateId = offerInput.StateId, CityId = offerInput.CityId }
                };
                if (caller != null)
                {
                    caller.Add(_module, "GetOffersOnCriteria", message);
                    isCallAdded = true;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
            return isCallAdded;
        }
    }
}