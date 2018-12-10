using Carwale.BL.GrpcFiles;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.Enum;
using Carwale.Utility;
using EditCMSWindowsService.Messages;
using Google.Protobuf;

namespace Carwale.Adapters.Cms
{
    public class CarSynopsisAdapter : ApiGatewayAdapterBase<int, ArticlePageDetails, GrpcArticlePageDetails>
    {

        public CarSynopsisAdapter()
            : base(CWConfiguration.EditCMSModuleName, "GetCarSynopsisV1")
        {
            
        }

        protected override IMessage GetRequest(int input)
        {
            return new GrpcCarSynopsisURI
            {
                ApplicationId = (int)Application.CarWale,
                ModelId = input
            };
        }

        protected override ArticlePageDetails BuildResponse(IMessage responseMessage)
        {
            var response = responseMessage as GrpcArticlePageDetails;

            return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(response);
        }
    }
}
