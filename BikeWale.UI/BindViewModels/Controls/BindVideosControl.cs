using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using EditCMSWindowsService.Messages;
using Grpc.CMS;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    public class BindVideosControl
    {
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }
        uint pageNo = 1;

        static bool _logGrpcErrors = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.LogGrpcErrors);
        static readonly ILog _logger = LogManager.GetLogger(typeof(BindVideosControl));
        static bool _useGrpc = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.UseGrpc);

        /// <summary>
        /// Function to bind the videos with repeater. Function will get the data from CW api and cache it in bikewale.
        /// </summary>
        /// <param name="rptr"></param>
        public void BindVideos(Repeater rptr)
        {
            FetchedRecordsCount = 0;
            List<BikeVideoEntity> objVideosList = null;

            try
            {
                objVideosList = GetVideosFromCWAPI();

                if (objVideosList != null && objVideosList.Any())
                {
                    FetchedRecordsCount = objVideosList.Count();

                    if (FetchedRecordsCount > 0)
                    {
                        rptr.DataSource = objVideosList.Take(TotalRecords);
                        rptr.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }

        /// <summary>
        /// Written By : Ashish G. Kamble
        /// Function to get the data from CW api.
        /// </summary>
        /// <returns></returns>
        private List<BikeVideoEntity> GetVideosFromCWAPI()
        {
            return GetVideosByMakeModelViaGrpc();
        }

        /// <summary>
        /// Author: Prasad Gawde
        /// </summary>
        /// <returns></returns>
        private List<BikeVideoEntity> GetVideosByMakeModelViaGrpc()
        {
            List<BikeVideoEntity> videoDTOList = null;
            try
            {

                GrpcVideosList _objVideoList;
                int startIndex, endIndex;
                Bikewale.Utility.Paging.GetStartEndIndex((int)TotalRecords, (int)pageNo, out startIndex, out endIndex);

                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                        _objVideoList = GrpcMethods.GetVideosByModelId(ModelId.Value, (uint)startIndex, (uint)endIndex);
                    else
                        _objVideoList = GrpcMethods.GetVideosByMakeId(MakeId.Value, (uint)startIndex, (uint)endIndex);
                }
                else
                {
                    _objVideoList = GrpcMethods.GetVideosBySubCategory((int)EnumVideosCategory.JustLatest, (uint)startIndex, (uint)endIndex);
                }

                if (_objVideoList != null && _objVideoList.LstGrpcVideos.Count > 0)
                {
                    videoDTOList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objVideoList.LstGrpcVideos);
                }

            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return videoDTOList;
        }


    }
}