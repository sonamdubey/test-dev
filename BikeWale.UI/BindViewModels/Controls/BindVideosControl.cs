using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.DTO.Videos;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;

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

        static BindVideosControl()
        {
            _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
            _applicationid = ConfigurationManager.AppSettings["applicationId"];
            _requestType = "application/json";
        }

        public void BindVideos(Repeater rptr)
        {
            FetchedRecordsCount = 0;
            List<BikeVideoEntity> objVideosList = null;
            uint pageNo = 1;
            try
            {                

                string _apiUrl = String.Format("/api/v1/videos/category/{0}/?appId=2&pageNo={1}&pageSize={2}", (int)EnumVideosCategory.JustLatest,pageNo, TotalRecords);
                
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
    }
}