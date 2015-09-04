using Bikewale.Entities.Compare;
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
    /// <summary>
    /// Bike Compare View Model
    /// Author  :   Sumit Kate
    /// Date    :   02 Sept 2015
    /// </summary>
    public class BindBikeCompareControl
    {
        /// <summary>
        /// Total records requested
        /// </summary>
        public static int TotalRecords { get; set; }
        /// <summary>
        /// Total Fetched records
        /// </summary>
        public static int FetchedRecordCount { get; set; }
                
        public static IEnumerable<TopBikeCompareBase> CompareList { get; set; }        

        public static TopBikeCompareBase FetchTopRecord()
        {
            if (FetchedRecordCount > 0)
            {
                return CompareList.First();
            }
            return null;
        }

        /// <summary>
        /// Bind the repeater to the Repeater
        /// </summary>
        /// <param name="repeater">Repeater object</param>
        public static void BindBikeCompare(Repeater repeater, int skipCount = 0)
        {
            try
            {
                if (FetchedRecordCount > 0 && repeater != null)
                {
                    repeater.DataSource = CompareList.Skip(skipCount);
                    repeater.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        public static void FetchBikeCompares()
        {
            IEnumerable<TopBikeCompareBase> topBikeCompares = null;
            string hostURL = String.Empty;
            string requestType = "application/json";
            string apiUrl = String.Empty;
            try
            {
                hostURL = ConfigurationManager.AppSettings["bwHostUrl"];
                apiUrl = String.Format("/api/BikeCompareList/?topCount={0}", TotalRecords);

                topBikeCompares = BWHttpClient.GetApiResponseSync<IEnumerable<TopBikeCompareBase>>(hostURL, requestType, apiUrl, topBikeCompares);
                if (topBikeCompares != null && topBikeCompares.Count() > 0)
                {
                    FetchedRecordCount = topBikeCompares.Count();
                    CompareList = topBikeCompares;
                }
                else
                {
                    FetchedRecordCount = 0;
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