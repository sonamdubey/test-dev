
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Videos;
using System;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 mar 2017
    /// Description :   Similar Videos Model MVC model
    /// </summary>
    public class SimilarVideosModel
    {
        private readonly IVideos _video = null;
        private readonly uint _modelId, _videoId;
        public SimilarVideosModel(uint modelId, uint videoId, IVideos video)
        {
            _video = video;
            _modelId = modelId;
            _videoId = videoId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Mar 2017
        /// Description :   Returns Similar Video Models view model
        /// </summary>
        /// <returns></returns>
        public SimilarVideoModelsVM GetData()
        {
            SimilarVideoModelsVM similarVideosModel = null;
            if (_modelId > 0)
            {
                try
                {
                    similarVideosModel = new SimilarVideoModelsVM();
                    similarVideosModel.Videos = _video.GetSimilarModelsVideos(_videoId, _modelId, 9);
                    similarVideosModel.ModelId = _modelId;
                    BikeModelEntity objModel = new ModelHelper().GetModelDataById(_modelId);

                    if (objModel != null)
                    {
                        similarVideosModel.ViewAllLinkText = "View all";
                        similarVideosModel.ViewAllLinkUrl = String.Format("/{0}-bikes/{1}/videos/", objModel.MakeBase.MaskingName, objModel.MaskingName);
                        similarVideosModel.ViewAllLinkTitle = String.Format("{0} {1} Videos", objModel.MakeBase.MakeName, objModel.ModelName);
                    }
                }
                catch (Exception ex)
                {
                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("SimilarVideosModel.GetData({0})", _modelId));
                }
            }
            return similarVideosModel;
        }
    }
}