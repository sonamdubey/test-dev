using System;
using System.Collections.Generic;
using Google.Protobuf;
using AutoMapper;
using System.Configuration;
using Carwale.Entity.CMS.URIs;
using EditCMSWindowsService.Messages;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;

namespace Carwale.Adapters.Cms
{
    public class MostRecentArticlesAdapter : ApiGatewayAdapterBase<ArticleRecentURI, List<ArticleSummary>, GrpcArticleSummaryList>
    {
        public MostRecentArticlesAdapter()
            : base(Utility.CWConfiguration.EditCMSModuleName, "GetMostRecentArticles")
        {

        }

        protected override IMessage GetRequest(ArticleRecentURI recentArticleUri)
        {
            IMessage message = new GrpcArticleRecentURI()
            {
                ApplicationId = recentArticleUri.ApplicationId,
                ModelId = recentArticleUri.ModelId,
                TotalRecords = recentArticleUri.TotalRecords,
                ContentTypes = recentArticleUri.ContentTypes,
            };

            return message;
        }

        protected override List<ArticleSummary> BuildResponse(IMessage responseMessage)
        {
            var modelNews = responseMessage as GrpcArticleSummaryList;
            return Mapper.Map<Google.Protobuf.Collections.RepeatedField<GrpcArticleSummary>, List<ArticleSummary>>(modelNews.LstGrpcArticleSummary);
        }
    }
}
