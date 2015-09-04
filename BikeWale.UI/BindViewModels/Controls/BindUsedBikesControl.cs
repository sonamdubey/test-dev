using Bikewale.DTO.UsedBikes;
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
    public class BindUsedBikesControl
    {
        /// <summary>
        /// Total records requested
        /// </summary>
        public static int TotalRecords { get; set; }
        /// <summary>
        /// Total Fetched records
        /// </summary>
        public static int FetchedRecordsCount { get; set; }               
        public static int? CityId { get; set; }

        public static void BindRepeater(Repeater repeater)
        {
            IEnumerable<PopularUsedBikesBase> popularUsedBikes = null;
            try
            {
                popularUsedBikes = FetchPopularUsedBike(TotalRecords, CityId);
                if (repeater != null && FetchedRecordsCount > 0)
                {
                    repeater.DataSource = popularUsedBikes;
                    repeater.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private static IEnumerable<PopularUsedBikesBase> FetchPopularUsedBike(int topCount, int? cityId)
        {
            IEnumerable<PopularUsedBikesBase> popularUsedBikes = null;
            string hostURL = String.Empty;
            string requestType = "application/json";
            string apiUrl = String.Empty;
            try
            {
                hostURL = ConfigurationManager.AppSettings["bwHostUrl"];
                apiUrl = String.Format("api/PopularUsedBikes/?topCount={0}&cityId={1}", topCount, cityId.HasValue ? cityId.Value.ToString() : "");

                popularUsedBikes = BWHttpClient.GetApiResponseSync<IEnumerable<PopularUsedBikesBase>>(hostURL, requestType, apiUrl, popularUsedBikes);
                if (popularUsedBikes != null && popularUsedBikes.Count() > 0)
                {
                    FetchedRecordsCount = popularUsedBikes.Count();
                }
                else
                {
                    FetchedRecordsCount = 0;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return popularUsedBikes;
        }
    }
}