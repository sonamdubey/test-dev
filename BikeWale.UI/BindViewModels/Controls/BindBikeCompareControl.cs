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
        static readonly string m_bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
        static readonly string m_requestType = "application/json";
        static readonly string m_BikeCompareListString = "/api/BikeCompareList/?topCount=";

        /// <summary>
        /// Total records requested
        /// </summary>    
        public int TotalRecords { get; set; }

        /// <summary>
        /// Total Fetched records
        /// </summary>    
        public int FetchedRecordCount { get; set; }

        public IEnumerable<TopBikeCompareBase> CompareList { get; set; }


        public TopBikeCompareBase FetchTopRecord()
        {
            if (FetchedRecordCount > 0)
            {
                if (CompareList != null)
                    return CompareList.First();
            }

            return null;
        }

        /// <summary>
        /// Bind the repeater to the Repeater
        /// </summary>
        /// <param name="repeater">Repeater object</param>
        public void BindBikeCompare(Repeater repeater, int skipCount = 0)
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

        public void FetchBikeCompares()
        {
            IEnumerable<TopBikeCompareBase> topBikeCompares = null;

            string apiUrl = String.Empty;
            FetchedRecordCount = 0;
            try
            {
                apiUrl = m_BikeCompareListString + TotalRecords;// String.Format("/api/BikeCompareList/?topCount={0}", m_TotalRecords);

                topBikeCompares = BWHttpClient.GetApiResponseSync<IEnumerable<TopBikeCompareBase>>(m_bwHostUrl, m_requestType, apiUrl, topBikeCompares);
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