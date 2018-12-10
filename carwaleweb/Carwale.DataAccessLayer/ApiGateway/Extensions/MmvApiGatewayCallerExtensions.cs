using AEPLCore.Logging;
using Carwale.Entity.Enum;
using MMV.Service.ProtoClass;
using Offers.Protos.ProtoFiles;
using System;
using System.Configuration;

namespace Carwale.DAL.ApiGateway.Extensions.Mmv
{
    public static class MmvApiGatewayCallerExtensions
    {
        private static Logger Logger = LoggerFactory.GetLogger();
        private static string _module = ConfigurationManager.AppSettings["MmvModuleName"];

        public static bool AggregateGetVersionsByModelId(this IApiGatewayCaller caller, int modelId)
        {
            bool isCallAdded = false;
            try
            {
                GrpcInt message = new GrpcInt
                {
                    Value = modelId
                };
                if (caller != null)
                {
                    caller.Add(_module, "GetVersionsByModelId", message);
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