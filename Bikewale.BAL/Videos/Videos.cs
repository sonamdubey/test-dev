using Bikewale.DAL.Videos;
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

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IVideos, VideosRepository>();
                videosRepository = container.Resolve<IVideos>();
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : To egt BIke Videos by Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, uint totalCount)
        {
            IEnumerable<BikeVideoEntity> objVideosList = null;
            uint pageNo = 1;
            try
            {

                string _apiUrl = String.Format("/api/v1/videos/category/{0}/?appId=2&pageNo={1}&pageSize={2}", (int)categoryId, pageNo, totalCount);

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
        /// Description : overload function to get page wise data,To egt BIke Videos by Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, uint totalCount, uint pageNum)
        {
            IEnumerable<BikeVideoEntity> objVideosList = null;
            //uint pageNo = 1;
            try
            {

                string _apiUrl = String.Format("/api/v1/videos/category/{0}/?appId=2&pageNo={1}&pageSize={2}", (int)categoryId, pageNum, totalCount);

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
        public IEnumerable<BikeVideoEntity> GetVideosBySubCategory(string categoryIdList, uint pageNo, uint pageSize)
        {
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {
                string _apiUrl = String.Format("/api/v1/videos/subcategory/{0}/?appId=2&pageNo={1}&pageSize={2}", categoryIdList, pageNo, pageSize);

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
        /// Description : To get Bike Similar Videos  based on videoBasic Id
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoId, uint totalCount)
        {
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {
                //http://localhost/api/v1/videos/18838/similar/?appId=1&topCount=1
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

    }
}
