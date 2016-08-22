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