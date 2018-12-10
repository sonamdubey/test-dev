using EditCMSWindowsService.Messages;
using System.Collections.Generic;
using Carwale.Utility;
using Google.Protobuf;
using AutoMapper;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;

namespace Carwale.Adapters.Cms
{
    public class VideosByModelIdAdapter : ApiGatewayAdapterBase<VideosByIdURI, List<Video>, GrpcVideosList>
    {
        public VideosByModelIdAdapter()
            : base(CWConfiguration.EditCMSModuleName, "GetVideosByModelId")
        {

        }

        protected override IMessage GetRequest(VideosByIdURI modelVideoUri)
        {
            IMessage message = new GrpcVideosByIdURI()
            {
                ApplicationId = modelVideoUri.ApplicationId,
                StartIndex = modelVideoUri.StartIndex,
                EndIndex = modelVideoUri.EndIndex,
                Id = modelVideoUri.ModelId,
            };

            return message;
        }

        protected override List<Video> BuildResponse(IMessage responseMessage)
        {
            var modelVideos = responseMessage as GrpcVideosList;
            return Mapper.Map<Google.Protobuf.Collections.RepeatedField<GrpcVideo>, List<Video>>(modelVideos?.LstGrpcVideos);
        }
    }
}
