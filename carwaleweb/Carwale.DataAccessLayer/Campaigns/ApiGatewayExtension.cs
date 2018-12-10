using System;
using AEPLCore.Logging;
using Google.Protobuf;
using Carwale.DAL.ApiGateway;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Enum;
using ProtoBufClass.Campaigns;
using ProtoBufClass.Common;
using System.Configuration;
using AutoMapper;

namespace Carwale.DAL.Campaigns
{
    public static class ApiGatewayExtension
    {
        private static Logger Logger = LoggerFactory.GetLogger();
        private static readonly string _campaignModule = ConfigurationManager.AppSettings["DealerCampaignModule"] ?? string.Empty;
        public static void AddGetCampaignsOnModelCityPlatform(this IApiGatewayCaller caller, int modelId, Location locationObj, int platformId, int applicationId = (int)Application.CarWale, int count = 0)
        {
            try
            {
                if (caller != null && locationObj != null && platformId > 0)
                {
                    IMessage message = new PQRule
                    {
                        Model = new Item { Id = modelId },
                        City = new Item { Id = locationObj.CityId },
                        Zone = new Item { Id = locationObj.ZoneId },
                        Platform = new Item { Id = platformId },
                        ApplicationId = applicationId,
                        Count = count
                    };
                    caller.Add(_campaignModule, "GetCampaignsOnModelCityPlatform", message);
                }

            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        public static void AddGetCampaign(this IApiGatewayCaller caller, int modelId, Location locationObj, int platformId, bool recommendation, int applicationId = (int)Application.CarWale)
        {
            try
            {
                if (caller != null && locationObj != null && platformId > 0)
                {
                    IMessage message = new CampaignRequest
                    {
                        Model = modelId,
                        UserLocation = Mapper.Map<Location, GeoLocation>(locationObj),
                        Platform = platformId,
                        ApplicationId = applicationId,
                        ShowRecommendation = recommendation
                    };
                    caller.Add(_campaignModule, "GetCampaign", message);
                }

            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        public static void AddGetCampaignDetailsByCriteria(this IApiGatewayCaller caller, int dealerId)
        {
            try
            {
                if (caller != null && dealerId > 0)
                {
                    IMessage message = new CampaignCriteria { DealerId = dealerId };
                    caller.Add(_campaignModule, "GetCampaignDetailsByCriteria", message);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void GetAllTemplatesByPage(this IApiGatewayCaller caller, TemplateInput templateInput)
        {
            try
            {
                IMessage message = templateInput;
                caller.Add(_campaignModule, "GetAllTemplatesByPage", message);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}