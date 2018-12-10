using Carwale.BL.GrpcFiles;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Utility;
using EditCMSWindowsService.Messages;
using Google.Protobuf;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;

namespace Carwale.Adapters.Cms
{
    public class ContentDetailsByCategoryAdapter : ApiGatewayAdapterBase<ArticleByCatURI, ArticlePageDetails, GrpcArticlePageDetails>
    {
        public ContentDetailsByCategoryAdapter():base(CWConfiguration.EditCMSModuleName, "GetContentDetailsByCategory")
        {
            
        }

        protected override IMessage GetRequest(ArticleByCatURI input)
        {
            IMessage message = new GrpcArticleByCatURI()
            {
                ModelId = input.ModelId,
                CategoryIdList = input.CategoryIdList,
                ApplicationId = (uint) Application.CarWale
            };

            return message;
        }

        protected override ArticlePageDetails BuildResponse(IMessage responseMessage)
        {
            var result = responseMessage as GrpcArticlePageDetails;

            return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(result);
        }
    }
}
