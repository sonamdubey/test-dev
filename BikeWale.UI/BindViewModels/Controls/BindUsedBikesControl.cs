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
        public int TotalRecords { get; set; }
        /// <summary>
        /// Total Fetched records
        /// </summary>
        public int FetchedRecordsCount { get; set; }               
        public int? CityId { get; set; }

        IEnumerable<PopularUsedBikesBase> popularUsedBikes = null;

        public void BindRepeater(Repeater repeater)
        {
            FetchedRecordsCount = 0;

            try
            {
                FetchPopularUsedBike(TotalRecords, CityId);
                
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

        private void FetchPopularUsedBike(int topCount, int? cityId)
        {            
            string hostURL = String.Empty;
            string requestType = "application/json";
            string apiUrl = String.Empty;
            try
            {
                hostURL = ConfigurationManager.AppSettings["bwHostUrl"];
                apiUrl = String.Format("/api/PopularUsedBikes/?topCount={0}&cityId={1}", topCount, cityId.HasValue ? cityId.Value.ToString() : "");

                popularUsedBikes = BWHttpClient.GetApiResponseSync<IEnumerable<PopularUsedBikesBase>>(hostURL, requestType, apiUrl, popularUsedBikes);
                
                if (popularUsedBikes != null)
                {
                    FetchedRecordsCount = popularUsedBikes.Count();
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