using VehicleData.Service.ProtoClass;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using Carwale.Notifications.Logs;

namespace Carwale.DAL.ApiGateway.Extensions.CarData
{
    public static class CarDataApiGatewayCallerExtensions
    {
        private static string _module = ConfigurationManager.AppSettings["CarDataModuleName"];
        public static IApiGatewayCaller GenerateCarDataCallerRequest(this IApiGatewayCaller caller, List<int> versionIds)
        {
            try
            {
                if (caller != null)
                {
                    VehicleDataRequest vehicleDataRequest = new VehicleDataRequest();
                    vehicleDataRequest.VersionIds.AddRange(versionIds);
                    vehicleDataRequest.ApplicationId = (int)Application.CarWale;
                    vehicleDataRequest.ItemGroupTypes = "1,3";
                    caller.Add(_module, "GetVehicleDataForVersionId", vehicleDataRequest);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
            return caller;
        }
        public static IApiGatewayCaller GenerateCarDataCallerRequestOldApp(this IApiGatewayCaller caller, List<int> versionIds)
        {
            try
            {
                if (caller != null)
                {
                    VehicleDataRequest vehicleDataRequest = new VehicleDataRequest();
                    vehicleDataRequest.VersionIds.AddRange(versionIds);
                    vehicleDataRequest.ApplicationId = (int)Application.CarWale;
                    vehicleDataRequest.ItemGroupTypes = "1,3";
                    caller.Add(_module, "VehicleDataForOldApp", vehicleDataRequest);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
            return caller;
        }

        public static IApiGatewayCaller GetVersionSpecsSummary(this IApiGatewayCaller caller, IEnumerable<int> versionIds, IEnumerable<int> itemIds)
        {
            try
            {
                if (caller != null)
                {
                    SpecsSummaryRequest requestIds = new SpecsSummaryRequest
                    {
                        ApplicationId = Convert.ToInt32(Application.CarWale),
                    };
                    requestIds.VersionIds.AddRange(versionIds);
                    requestIds.ItemIds.AddRange(itemIds);
                    caller.Add(_module, "GetVersionSpecsSummary", requestIds);
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
            return caller;
        }
        public static IApiGatewayCaller GenerateModelSpecsRequest(this IApiGatewayCaller caller, IEnumerable<int> versionIds, int modelId)
        {
            try
            {
                if (caller != null)
                {
                    ModelSpecsSummaryRequest modelSpecsRequest = new ModelSpecsSummaryRequest();
                    modelSpecsRequest.ApplicationId = (int)Application.CarWale;
                    modelSpecsRequest.VersionIds.AddRange(versionIds);
                    modelSpecsRequest.ModelId = modelId;
                    caller.Add(_module, "GetModelSpecsSummary", modelSpecsRequest);
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return null;
            }
            return caller;
        }

        public static IApiGatewayCaller GenerateModelDataSummaryRequest(this IApiGatewayCaller caller, List<int> versionIds, int modelId)
        {
            try
            {
                if (caller != null)
                {
                    ModelSpecsSummaryRequest modelSpecsRequest = new ModelSpecsSummaryRequest();
                    modelSpecsRequest.ApplicationId = (int)Application.CarWale;
                    modelSpecsRequest.VersionIds.AddRange(versionIds);
                    modelSpecsRequest.ModelId = modelId;
                    caller.Add(_module, "GetModelFeatures", modelSpecsRequest);
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return null;
            }
            return caller;
        }

        public static IApiGatewayCaller GenerateVersionsDataByItemIdsRequest(this IApiGatewayCaller caller, List<int> versionIds, List<int> itemIds)
        {
            try
            {
                if (caller != null)
                {
                    VersionsDataByItemIdsRequest request = new VersionsDataByItemIdsRequest();
                    request.ApplicationId = (int)Application.CarWale;
                    request.VersionIds.AddRange(versionIds);
                    request.ItemIds.AddRange(itemIds);
                    caller.Add(_module, "VersionsDataByItemIds", request);
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return null;
            }
            return caller;
        }
    }
}
