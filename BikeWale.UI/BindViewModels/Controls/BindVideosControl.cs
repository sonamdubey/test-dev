using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.Cache.Core;
using Bikewale.DTO.Videos;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using log4net;
using EditCMSWindowsService.Messages;
using Grpc.CMS;
using Bikewale.BAL.GrpcFiles;

namespace Bikewale.BindViewModels.Controls
{
    public class BindVideosControl
    {
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }

        static string _cwHostUrl;
        static string _requestType;
        static string _applicationid  ;
        uint pageNo = 1;

        static bool _logGrpcErrors = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.LogGrpcErrors);
        static readonly ILog _logger = LogManager.GetLogger(typeof(BindVideosControl));
        static bool _useGrpc = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.UseGrpc);

        string cacheKey = "BW_Videos_JustLatest";

        static BindVideosControl()
        {
            _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
            _applicationid = ConfigurationManager.AppSettings["applicationId"];
            _requestType = "application/json";
        }

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
                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                        cacheKey = "BW_Videos_Model_" + ModelId.Value + "_P_" + pageNo + "_Cnt_" + TotalRecords;
                    else
                        cacheKey = "BW_Videos_Make_" + MakeId.Value + "_P_" + pageNo + "_Cnt_" + TotalRecords;
                }

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    ICacheManager _cache = container.Resolve<ICacheManager>();

                    objVideosList = _cache.GetFromCache<List<BikeVideoEntity>>(cacheKey, new TimeSpan(0, 15, 0), () => GetVideosFromCWAPI());
                }

                if (objVideosList != null && objVideosList.Count() > 0)
                {
                    FetchedRecordsCount = objVideosList.Count();

                    if (FetchedRecordsCount > 0)
                    {
                        rptr.DataSource = objVideosList;
                        rptr.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                if (_useGrpc)
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
                        _objVideoList = GrpcMethods.GetVideosBySubCategory((int)EnumVideosCategory.JustLatest,(uint)startIndex, (uint)endIndex);
                    }

                    if (_objVideoList != null && _objVideoList.LstGrpcVideos.Count > 0)
                    {
                        videoDTOList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objVideoList.LstGrpcVideos);
                    }
                    else
                    {
                        videoDTOList = GetVideosByMakeModelOldWay();
                    }
                }
                else
                {
                    videoDTOList = GetVideosByMakeModelOldWay();
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                videoDTOList = GetVideosByMakeModelOldWay();
            }

            return videoDTOList;
        }

        public List<BikeVideoEntity> GetVideosByMakeModelOldWay()
        {
            List<BikeVideoEntity> objVideosList = null;

            try
            {
                string _apiUrl = String.Format("/api/v1/videos/category/{0}/?appId=2&pageNo={1}&pageSize={2}", (int)EnumVideosCategory.JustLatest, pageNo, TotalRecords);

                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                        _apiUrl = String.Format("/api/v1/videos/model/{0}/?appId=2&pageNo={1}&pageSize={2}", ModelId.Value, pageNo, TotalRecords);
                    else
                        _apiUrl = String.Format("/api/v1/videos/make/{0}/?appId=2&pageNo={1}&pageSize={2}", MakeId.Value, pageNo, TotalRecords);
                }

                using (BWHttpClient objclient = new BWHttpClient())
                {
                    objVideosList = objclient.GetApiResponseSync<List<BikeVideoEntity>>(APIHost.CW, _requestType, _apiUrl, objVideosList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objVideosList;  
        }
    }
}