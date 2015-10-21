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

        static BindVideosControl()
        {
            _cwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
            _requestType = "application/json";
        }

        public void BindVideos(Repeater rptr)
        {
            try
            {
                FetchedRecordsCount = 0;
                VideosList objVideos = null;

                string _apiUrl = String.Format("/api/videos/cat/{0}/pn/1/ps/{1}", EnumVideosCategory.JustLatest, TotalRecords);

                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                        _apiUrl = String.Format("/api/videos/pn/1/ps/{0}/model/{1}", TotalRecords, ModelId.Value);
                    else
                        _apiUrl = String.Format("/api/videos/pn/1/ps/{0}/make/{1}", TotalRecords, MakeId.Value);
                }

                objVideos = BWHttpClient.GetApiResponseSync<VideosList>(_cwHostUrl, _requestType, _apiUrl, objVideos);

                if (objVideos != null)
                {                    
                    FetchedRecordsCount = objVideos.Videos.Count();

                    if (FetchedRecordsCount > 0)
                    {                        
                        rptr.DataSource = objVideos.Videos;
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