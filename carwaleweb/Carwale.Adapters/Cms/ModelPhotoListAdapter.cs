using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using EditCMSWindowsService.Messages;
using System.Collections.Generic;
using Carwale.Utility;
using Google.Protobuf;
using Carwale.BL.GrpcFiles;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;

namespace Carwale.Adapters.Cms
{
    public class ModelPhotoListAdapter : ApiGatewayAdapterBase<ModelPhotoURI, List<ModelImage>, GrpcModelImageList>
    {
        public ModelPhotoListAdapter()
            : base(CWConfiguration.EditCMSModuleName, "GetModelPhotosList")
        {
                
        }

        protected override IMessage GetRequest(ModelPhotoURI modelPhotoUri)
        {
            IMessage message = new GrpcModelPhotoURI()
            {
                ApplicationId = modelPhotoUri.ApplicationId,
                ModelId = modelPhotoUri.ModelId,
                CategoryIdList = modelPhotoUri.CategoryIdList
            };

            return message;
        }

        protected override List<ModelImage> BuildResponse(IMessage responseMessage)
        {
            var modelImages = responseMessage as GrpcModelImageList;
            if (modelImages == null || modelImages.LstGrpcModelImage == null || modelImages.LstGrpcModelImage.Count <= 0)
                return new List<ModelImage>();
            return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(modelImages);
        }
    }
}
