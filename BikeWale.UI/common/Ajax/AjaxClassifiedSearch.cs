using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using AjaxPro;
using Bikewale.Common;
using Bikewale.Used;

namespace Bikewale.Ajax
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 14 Dec 2012
    ///     Summary : class for the ajax related funtions for classfied search.
    /// </summary>
	public class AjaxClassifiedSearch
	{
        /// <summary>
        ///     Written By : Ashish G. Kamble
        ///     Summary : Function will update the view count for the selected inquiry.
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="isDealer"></param>
        /// <returns></returns>
		[AjaxPro.AjaxMethod()]
		public bool UpdateViewCount( string inquiryId, string isDealer )
		{
			bool retVal = false;
			
			bool isDealerVal = false;
			if( CommonOpn.IsNumeric( inquiryId ) )
			{
				SearchCommon sc = new SearchCommon();
				if( isDealer == "1" )
					isDealerVal = true;				 				
				retVal = sc.UpdateViewCount( inquiryId, isDealerVal );				
			}
			return retVal;
		}

        /// <summary>
        ///     Written By : Ashish G. Kamble on 14 Dec 2012
        ///     Summary : Function will save the search criteria selected by customer into cookie.
        ///     Modified By : Ashwini Todkar on 16 April 2014
        ///     Summary     : set cookie created status to true or false
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool SaveUsedSearchCriteria(string queryString)
        {
            bool retVal = false;

            try
            {
                ClassifiedCookies.UsedSearchQueryString = queryString;
                retVal = true;
            }
            catch(Exception ex)
            {
                retVal = false;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : UsedSearchQueryString");
                objErr.SendMail();
            }

            return retVal;
        }   // End of SaveUsedSearchCriteria method.
        
        #region AjaxMethod  
        /// <summary>
        /// Written By : Ashwini Todkar
        /// Summary : Function returns bike details by profile ID 
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns>bike make ,bike model ,city </returns>
        [AjaxPro.AjaxMethod()]
        public string GetInquiryDetailsByProfileId(string profileId)
        {
            string inquiryDetails = string.Empty;
            DataTable dt = null;
            SellInquiryDetails objDetails = new SellInquiryDetails();
            dt = objDetails.GetInquiryDetailsByProfileId(profileId);

            if (dt != null && dt.Rows.Count > 0)
            {
                inquiryDetails = JSON.GetJSONString(dt);
            }
            return inquiryDetails;
        }   // End of SearchByProfileId method.
        #endregion

    }   // End of class
}   // End of namespace