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

        static readonly string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
        static readonly string _requestType = "application/json";

        public static void BindMostPopularBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;

            MostPopularBikesList objBikeList = null;

            try
            {
                                
                string _apiUrl = String.Format("/api/ModelList/?totalCount={0}&makeId={1}", totalCount, makeId);

                objBikeList = BWHttpClient.GetApiResponseSync<MostPopularBikesList>(_bwHostUrl, _requestType, _apiUrl, objBikeList);

                if (objBikeList != null)
                {
                  var bikeList = objBikeList.PopularBikes.ToList();
                  if (bikeList.Count > 0)
                  {
                    FetchedRecordsCount = bikeList.Count;
                    rptr.DataSource = bikeList;
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