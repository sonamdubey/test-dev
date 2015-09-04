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

        public static void BindVideos(Repeater rptr)
        {
            try
            {
                VideosList objVideos = null;

                string _cwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
                string _requestType = "application/json";

                string _apiUrl = "/api/videos/?categoryId=" + EnumVideosCategory.JustLatest + "&pageNo=1&pageSize=" + TotalRecords;


                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                        _apiUrl = "/api/videos/?pageNo=1&pageSize=" + TotalRecords + "&modelId=" + ModelId.Value;
                    else
                        _apiUrl = "/api/videos/?pageNo=1&pageSize=" + TotalRecords + "&makeId=" + MakeId.Value;
                }

                objVideos = BWHttpClient.GetApiResponseSync<VideosList>(_cwHostUrl, _requestType, _apiUrl, objVideos);

                if (objVideos != null && objVideos.Videos.ToList().Count > 0)
                {
                    FetchedRecordsCount = objVideos.Videos.ToList().Count;

                    rptr.DataSource = objVideos.Videos.ToList();
                    rptr.DataBind();
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