using System;
using Carwale.Entity.Enum;
using Carwale.Notifications.Logs;
using EditCMSWindowsService.Messages;
using Carwale.Utility;
using Google.Protobuf;
using Carwale.Entity.CMS;

namespace Carwale.DAL.ApiGateway.Extensions
{
    public static class CmsApiGatewayCallerExtension
    {
        public static IApiGatewayCaller GetContentDetailsByCategory(this IApiGatewayCaller caller, int modelId, string categoryIds)
        {
            try
            {
                if (caller != null)
                {
                    GrpcArticleByCatURI articleDataRequest = new GrpcArticleByCatURI();
                    articleDataRequest.ModelId = modelId;           
                    articleDataRequest.CategoryIdList = categoryIds;
                    articleDataRequest.ApplicationId = (uint)Application.CarWale;
                    caller.Add(CWConfiguration.EditCMSModuleName, "GetContentDetailsByCategory", articleDataRequest);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
            return caller;
        }
        public static IApiGatewayCaller GetCarSynopsis(this IApiGatewayCaller caller, int modelId)
        {
            try
            {
                if (caller != null)
                {
                    GrpcCarSynopsisURI carSynopsisRequest = new GrpcCarSynopsisURI
                    {
                        ApplicationId = (int)Application.CarWale,
                        ModelId = modelId
                    };
                    caller.Add(CWConfiguration.EditCMSModuleName, "GetCarSynopsisV1", carSynopsisRequest);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
            return caller;
        }

        public static IApiGatewayCaller GetModelsImages(this IApiGatewayCaller caller,string modelIds,int requiredImageCount,Application application)
        {
            try
            {
                if (caller != null)
                {
                    IMessage modelListPhotoUri= new GrpcModelListPhotoURI
                    {
                        ModelIds = modelIds,
                        RequiredImageCount = requiredImageCount,
                        CategoryIds = string.Format("{0},{1}", (int)CMSContentType.Images, (int)CMSContentType.RoadTest),
                        ApplicationId = (int)application
                    };

                    caller.Add(CWConfiguration.CMSModule, "GetModelsImages", modelListPhotoUri);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
            return caller;
        }

        public static IApiGatewayCaller GetMakeVideos(this IApiGatewayCaller caller,int makeId,Application application,uint startIndex,uint endIndex)
        {
            try
            {
                if (caller != null)
                {
                    IMessage videoRequestMessage = new GrpcVideosByIdURI()
                    {
                        Id = makeId,
                        ApplicationId = (uint)application,
                        StartIndex = startIndex,
                        EndIndex = endIndex
                    };

                    caller.Add(CWConfiguration.CMSModule, "GetNewModelsVideosByMakeId", videoRequestMessage);
                }
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return null;
            }

            return caller;
        }
    }
}
