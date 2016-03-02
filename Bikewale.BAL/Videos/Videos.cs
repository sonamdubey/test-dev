using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.BAL.Videos
{
    /// <summary>
    /// Created By : Sushl Kumar  on 18th Febrauary 2016
    /// Summary : Bussiness logic to get videos 
    /// </summary>
    public class Videos : IVideos
    {
        private readonly IVideos videosRepository = null;
        static string _cwHostUrl;
        static string _requestType;
        static string _applicationid;

        public Videos()
        {
            _cwHostUrl = BWConfiguration.Instance.CwApiHostUrl;
            _applicationid = BWConfiguration.Instance.ApplicationId;
            _requestType = BWConfiguration.Instance.APIRequestTypeJSON;
            
        }

        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : To egt BIke Videos by Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, ushort totalCount)
        {
            IEnumerable<BikeVideoEntity> objVideosList = null;
            //uint pageNo = 1;
            try
            {

                string _apiUrl = String.Format("/api/v1/videos/category/{0}/?appId=2&pageNo=1&pageSize={1}", (int)categoryId, totalCount);

                using (BWHttpClient objclient = new BWHttpClient())
                {
                    objVideosList = objclient.GetApiResponseSync<IEnumerable<BikeVideoEntity>>(APIHost.CW, _requestType, _apiUrl, objVideosList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objVideosList;
        }


        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : To get Bike Videos by Category/Categories
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public BikeVideosListEntity GetVideosBySubCategory(string categoryIdList, ushort pageNo, ushort pageSize, VideosSortOrder? sortOrder = null)
        {
            BikeVideosListEntity objVideosList = null;
            try
            {
                string _apiUrl = string.Empty;
                if(sortOrder.HasValue)
                {
                    _apiUrl = String.Format("/api/v1/videos/subcategory/{0}/?appId=2&pageNo={1}&pageSize={2}&sortCategory={3}", categoryIdList, pageNo, pageSize,sortOrder);
                }
                else
                {
                    _apiUrl = String.Format("/api/v1/videos/subcategory/{0}/?appId=2&pageNo={1}&pageSize={2}", categoryIdList, pageNo, pageSize);
                }
                

                using (BWHttpClient objclient = new BWHttpClient())
                {
                    objVideosList = objclient.GetApiResponseSync<BikeVideosListEntity>(APIHost.CW, _requestType, _apiUrl, objVideosList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objVideosList;
        }

        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : To get Bike Similar Videos  based on videoBasic Id
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoId, ushort totalCount)
        {
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {
                string _apiUrl = String.Format("/api/v1/videos/{0}/similar/?appId=2&topCount={1}", videoId, totalCount);
                using (BWHttpClient objclient = new BWHttpClient())
                {
                    objVideosList = objclient.GetApiResponseSync<IEnumerable<BikeVideoEntity>>(APIHost.CW, _requestType, _apiUrl, objVideosList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objVideosList;
        }


        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : To get video details based on videoBasic Id
        /// </summary>
        /// <param name="videoBasicId"></param>
        /// <returns></returns>
        public BikeVideoEntity GetVideoDetails(uint videoBasicId)
        {
            BikeVideoEntity objVideo = null;
            try
            {
                string _apiUrl = String.Format("/api/v1/videos/{0}/?appId=2", videoBasicId);

                using (BWHttpClient objclient = new BWHttpClient())
                {
                    objVideo = objclient.GetApiResponseSync<BikeVideoEntity>(APIHost.CW, _requestType, _apiUrl, objVideo);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objVideo;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 1st March 2016
        /// Description : To get Bike Videos by Bike Make Id 
        /// </summary>
        /// <param name="makeID"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetVideosByMakeModel(ushort pageNo, ushort pageSize, uint makeId , uint? modelId = null)
        {
            //BikeVideosListEntity objVideosList = null;
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {
                string _apiUrl = string.Empty;
                if (modelId.HasValue)
                {
                    _apiUrl = String.Format("/api/v1/videos/model/{0}/?appId=2&pageNo={1}&pageSize={2}", modelId, pageNo, pageSize);
                }
                else
                {
                    _apiUrl = String.Format("/api/v1/videos/make/{0}/?appId=2&pageNo={1}&pageSize={2}", makeId, pageNo, pageSize);
                }
                
                using (BWHttpClient objclient = new BWHttpClient())
                {
                    objVideosList = objclient.GetApiResponseSync<IEnumerable<BikeVideoEntity>>(APIHost.CW, _requestType, _apiUrl, objVideosList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "BikeVideosRepository.GetVideosByCategory");
                objErr.SendMail();
            }

            return objVideosList;
        }

    }
}
