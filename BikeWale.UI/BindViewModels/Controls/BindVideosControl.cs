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
    public static class BindVideosControl
    {
        public static int TotalRecords { get; set; }
        public static int? MakeId { get; set; }
        public static int? ModelId { get; set; }
        public static int FetchedRecordsCount { get; set; }

        static string _cwHostUrl;
        static string _requestType;
        static BindVideosControl()
        {
          _cwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
          _requestType = "application/json";
        }
        public static void BindVideos(Repeater rptr)
        {
            try
            {
                FetchedRecordsCount = 0;
                VideosList objVideos = null;
                
                string _apiUrl = "/api/videos/cat/" + EnumVideosCategory.JustLatest + "/pn/1/ps/" + TotalRecords;                 

                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                        _apiUrl = "/api/videos/pn/1/ps/" + TotalRecords + "/model/" + ModelId.Value;
                    else
                        _apiUrl = "/api/videos/pn/1/ps/" + TotalRecords + "/make/" + MakeId.Value;
                }

                objVideos = BWHttpClient.GetApiResponseSync<VideosList>(_cwHostUrl, _requestType, _apiUrl, objVideos);

                if (objVideos != null)
                {
                  var list = objVideos.Videos.ToList();
                  if (list.Count > 0)
                  {
                    FetchedRecordsCount =list.Count;
                    rptr.DataSource = list;
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