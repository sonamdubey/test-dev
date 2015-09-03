using Bikewale.DTO.Widgets;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    public class BindMostPopularBikesControl
    {
        public static int? totalCount { get; set; }
        public static int? makeId { get; set; }
        public static int FetchedRecordsCount { get; set; }

        public static void BindMostPopularBikes(Repeater rptr)
        {
            MostPopularBikesList objBikeList = null;

            try
            {
                string _bwHostUrl = ConfigurationManager.AppSettings["bwApiHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = String.Format("api/ModelList/?totalCount={0}&makeId={1}", totalCount, makeId);

                objBikeList = BWHttpClient.GetApiResponseSync<MostPopularBikesList>(_bwHostUrl, _requestType, _apiUrl, objBikeList);

                if (objBikeList != null && objBikeList.PopularBikes.ToList().Count > 0)
                {
                    FetchedRecordsCount = objBikeList.PopularBikes.ToList().Count;
                    rptr.DataSource = objBikeList.PopularBikes.ToList();
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