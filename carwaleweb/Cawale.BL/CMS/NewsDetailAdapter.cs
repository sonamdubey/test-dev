using AEPLCore.Utils.Serializer;
using ApiGatewayLibrary;
using Carwale.BL.GrpcFiles;
using Carwale.DTOs.CMS;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Common;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.Utility;
using EditCMSWindowsService.Messages;
using Google.Protobuf;
using Grpc.CMS;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace Carwale.BL.CMS
{
    public class NewsDetailAdapter : IServiceAdapterV2
    {
        private readonly ICMSContent _cmsCacheRepository;
        private readonly IUnityContainer _container;
        private readonly ulong _basicId;
        private readonly string _contentTypes = string.Empty;
        private readonly int _numberOfRecords = 6;
        private readonly ICarModels _models;
        private readonly IVideosBL _videoBL;
        private static readonly bool _useAPIGateway = ConfigurationManager.AppSettings["useApiGateway"] == "true";

        public NewsDetailAdapter(ICarModels models, ICMSContent cmsCacheRepository, IVideosBL videoBL, IUnityContainer container, ulong basicId, string contentTypes)
        {
            _container = container;
            _models = models;
            _cmsCacheRepository = cmsCacheRepository;
            _basicId = basicId;
            _videoBL = videoBL;
            _contentTypes = contentTypes;
        }
        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(ContentDetailPagesDTO<U>(input), typeof(T));
        }

        private ContentDetailPagesDTO_V2 ContentDetailPagesDTO<U>(U input)
        {
            ContentDetailPagesDTO_V2 newsDetailsDTO = new ContentDetailPagesDTO_V2();
            ulong basicId = _basicId;
            string articleUrl = (string)Convert.ChangeType(input, typeof(U));

            try
            {
                ulong articleBasicId = (ulong)GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetBasicIdFromArticleUrl(articleUrl + (basicId > 0 ? "-" + basicId : string.Empty) + "/"));

                if (articleBasicId <= 0)
                {
                    articleUrl = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetArticleUrlFromBasicId(basicId));

                    newsDetailsDTO.ArticlePages = new ArticlePageDetails()
                    {
                        ArticleUrl = articleUrl
                    };
                    newsDetailsDTO.IsRedirect = true;

                    return newsDetailsDTO;
                }

                newsDetailsDTO = new ContentDetailPagesDTO_V2
                {
                    ArticlePages = _cmsCacheRepository.GetContentPages(new ArticleContentURI { BasicId = articleBasicId })
                };

                if (newsDetailsDTO.ArticlePages != null && newsDetailsDTO.ArticlePages.Title != null)
                {
                    newsDetailsDTO.ArticlePages.Title = newsDetailsDTO.ArticlePages.Title.Replace("&#x20B9;", "₹");
                }

                if (newsDetailsDTO.ArticlePages != null)
                {
                    if (_useAPIGateway)
                    {
                        GetRelatedDataAPIGateway(newsDetailsDTO, articleBasicId);
                    }

                    else                    
                    {
                        GetRelatedData(newsDetailsDTO, articleBasicId);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewsDetailAdapter.GetNewsDetailDTO()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return newsDetailsDTO;
        }


        //only to be called in case api gateway stops working. to be removed after successful testing on live.
        private void GetRelatedData(ContentDetailPagesDTO_V2 newsDetailsDTO, ulong articleBasicId)
        {
            string tags = string.Join(",", newsDetailsDTO.ArticlePages.TagsList.ToArray());
            var RelatedArticles = _cmsCacheRepository.GetRelatedArticles(new ArticleRelatedURI
            {
                ApplicationId = Convert.ToUInt16(CMSAppId.Carwale),
                BasicId = (uint)_basicId,
                ContentTypes = _contentTypes,
                TotalRecords = 10
            });

            if (RelatedArticles != null && RelatedArticles.Count > 0)
            {
                RelatedArticles.RemoveAll(x => x.BasicId == articleBasicId);
                newsDetailsDTO.RelatedArticleIds = RelatedArticles.Select(x => x.BasicId).Distinct().ToList();
            }

            newsDetailsDTO.NewsWidgetModel = new NewsRightWidget();
            newsDetailsDTO.CarWidgetModel = new CarRightWidget();
            newsDetailsDTO.VideoWidgetModel = new PopularVideoWidget();

            var queryStringNews = new ArticleRecentURI();
            queryStringNews.ApplicationId = Convert.ToUInt16(CMSAppId.Carwale);
            queryStringNews.ContentTypes = _contentTypes;
            queryStringNews.TotalRecords = Convert.ToUInt16(_numberOfRecords);

            var queryStringPopular = new ArticleByCatURI();
            queryStringPopular.ApplicationId = Convert.ToUInt16(CMSAppId.Carwale);
            queryStringPopular.CategoryIdList = _contentTypes;
            queryStringPopular.StartIndex = 1;
            queryStringPopular.EndIndex = Convert.ToUInt32(ConfigurationManager.AppSettings["PopularNewsList"]);

            newsDetailsDTO.NewsWidgetModel.RecentNews = _cmsCacheRepository.GetMostRecentArticles(queryStringNews)
                                                                          .Where(x => x.BasicId != articleBasicId)
                                                                          .ToList();
            if (newsDetailsDTO.NewsWidgetModel.RecentNews.Count > 5)
            {
                newsDetailsDTO.NewsWidgetModel.RecentNews.RemoveAt(5);
            }

            newsDetailsDTO.NewsWidgetModel.PopularNews = _cmsCacheRepository.GetContentListByCategory(queryStringPopular)
                                                                            .Articles.OrderByDescending(x => Convert.ToInt32(x.Views))
                                                                            .Take(_numberOfRecords)
                                                                            .Where(x => x.BasicId != articleBasicId)
                                                                            .ToList();
            if (newsDetailsDTO.NewsWidgetModel.PopularNews.Count > 5)
            {
                newsDetailsDTO.NewsWidgetModel.PopularNews.RemoveAt(5);
            }

            Pagination page = new Pagination { PageNo = 1, PageSize = 5 };

            newsDetailsDTO.CarWidgetModel.PopularModels = _models.GetTopSellingCarModels(page);

            newsDetailsDTO.CarWidgetModel.UpcomingCars = _models.GetUpcomingCarModels(page);

            newsDetailsDTO.VideoWidgetModel.Videos = _videoBL.GetVideoList((uint)newsDetailsDTO.ArticlePages.BasicId, (uint)CMSAppId.Carwale)
                                                                   .OrderBy(x => x.Popularity)
                                                                   .ToList();
        }

        private void GetRelatedDataAPIGateway(ContentDetailPagesDTO_V2 newsDetailsDTO, ulong articleBasicId)
        {
            ushort applicationId = Convert.ToUInt16(CMSAppId.Carwale);
            ushort totalRecords = Convert.ToUInt16(_numberOfRecords);
            string tags = string.Join(",", newsDetailsDTO.ArticlePages.TagsList.ToArray());
            var queryStringPopular = new ArticleByCatURI
            {
                ApplicationId = applicationId,
                CategoryIdList = _contentTypes,
                StartIndex = 1,
                EndIndex = Convert.ToUInt32(ConfigurationManager.AppSettings["PopularNewsList"])
            };
            KeyValuePair<string, IMessage>[] calls = {
                        new KeyValuePair<string, IMessage>("GetRelatedArticlesByBasicId", new GrpcRelatedArticlesURI
                            {
                                ApplicationId = applicationId,
                                ContentTypes = _contentTypes,
                                BasicId = (uint)_basicId,
                                TotalRecords = 10
                            }),
                        new KeyValuePair<string, IMessage>("GetMostRecentArticles",new GrpcArticleRecentURI
                                {
                                    ApplicationId = applicationId,
                                    ContentTypes = _contentTypes,
                                    TotalRecords = totalRecords
                                }),
                        new KeyValuePair<string, IMessage>("GetSponsoredArticle", new GetSponsoredArticleURI
                            {
                                CategoryList = _contentTypes,
                                Author = CWConfiguration.SponsoredAuthorId
                            }),
                        new KeyValuePair<string, IMessage>("GetContentListByCategory", new GrpcArticleByCatURI
                            {
                                ApplicationId = queryStringPopular.ApplicationId,
                                CategoryIdList = queryStringPopular.CategoryIdList,
                                EndIndex = queryStringPopular.EndIndex,
                                StartIndex = queryStringPopular.StartIndex
                            })};
            var result = GrpcMethods.GetDataFromGateway(calls);
            List<Carwale.Entity.CMS.Articles.ArticleSummary> relatedArticles = null;
            if (result != null)
            {
                relatedArticles = _cmsCacheRepository.GetRelatedArticles(null,
                GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(Serializer.ConvertBytesToMsg<GrpcArticleSummaryList>(result.OutputMessages[0].Payload)));
            }
            if (relatedArticles != null && relatedArticles.Count > 0)
            {
                relatedArticles.RemoveAll(x => x.BasicId == articleBasicId);
                newsDetailsDTO.RelatedArticleIds = relatedArticles.Select(x => x.BasicId).Distinct().ToList();
            }

            newsDetailsDTO.NewsWidgetModel = new NewsRightWidget();
            newsDetailsDTO.CarWidgetModel = new CarRightWidget();
            newsDetailsDTO.VideoWidgetModel = new PopularVideoWidget();
            newsDetailsDTO.NewsWidgetModel.RecentNews = _cmsCacheRepository.GetMostRecentArticles(null,
                GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(Serializer.ConvertBytesToMsg<GrpcArticleSummaryList>(result.OutputMessages[1].Payload)))
                .Where(x => x.BasicId != articleBasicId)
                .ToList();
            if (newsDetailsDTO.NewsWidgetModel.RecentNews.Count > 5)
            {
                newsDetailsDTO.NewsWidgetModel.RecentNews.RemoveAt(5);
            }
            var sponsoredArticles = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(Serializer.ConvertBytesToMsg<GrpcArticleSummary>(result.OutputMessages[2].Payload));
            var contentListByCategory = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(Serializer.ConvertBytesToMsg<GrpcCMSContent>(result.OutputMessages[3].Payload), _contentTypes);
            newsDetailsDTO.NewsWidgetModel.PopularNews = _cmsCacheRepository.GetContentListByCategory(queryStringPopular, sponsoredArticle: sponsoredArticles,
                results: contentListByCategory, makeApiCall: false)
                                                                            .Articles.OrderByDescending(x => Convert.ToInt32(x.Views))
                                                                            .Take(_numberOfRecords)
                                                                            .Where(x => x.BasicId != articleBasicId)
                                                                            .ToList();
            if (newsDetailsDTO.NewsWidgetModel.PopularNews.Count > 5)
            {
                newsDetailsDTO.NewsWidgetModel.PopularNews.RemoveAt(5);
            }

            Pagination page = new Pagination() { PageNo = 1, PageSize = 5 };

            newsDetailsDTO.CarWidgetModel.PopularModels = _models.GetTopSellingCarModels(page);

            newsDetailsDTO.CarWidgetModel.UpcomingCars = _models.GetUpcomingCarModels(page);

            newsDetailsDTO.VideoWidgetModel.Videos = _videoBL.GetVideoList((uint)newsDetailsDTO.ArticlePages.BasicId, (uint)CMSAppId.Carwale)
                                                                   .OrderBy(x => x.Popularity)
                                                                   .ToList();
        }
    }
}