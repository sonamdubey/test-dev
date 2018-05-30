using Bikewale.Utility;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Bikewale.Models
{
    public class PopUpCityArea
    {
        public PoupCityAreaVM GetData(string encodedQueryString)
        {
            PoupCityAreaVM objPopupCityAreaVM = new PoupCityAreaVM();
            try
            {
                string decodedQueryString = Utils.Utils.DecryptTripleDES(encodedQueryString);

                NameValueCollection queryCollection = HttpUtility.ParseQueryString(decodedQueryString);
                objPopupCityAreaVM.PQSourceId = SqlReaderConvertor.ToUInt16(queryCollection["pqsourceid"]);
                objPopupCityAreaVM.PageCategoryId = SqlReaderConvertor.ToUInt16(queryCollection["pagecatid"]);
                objPopupCityAreaVM.PreSelectedCity = SqlReaderConvertor.ToUInt16(queryCollection["preselcity"]);
                objPopupCityAreaVM.MakeId = SqlReaderConvertor.ToUInt16(queryCollection["makeid"]);
                objPopupCityAreaVM.MakeName = Convert.ToString(queryCollection["makename"]);
                objPopupCityAreaVM.ModelId = SqlReaderConvertor.ToUInt16(queryCollection["modelid"]);
                objPopupCityAreaVM.ModelName = Convert.ToString(queryCollection["modelname"]);
                objPopupCityAreaVM.Url = Convert.ToString(queryCollection["Url"]);

                BindPageMetaTags(objPopupCityAreaVM.PageMetaTags);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "PopUpCityArea.GetData()");
            }
            return objPopupCityAreaVM;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 14-Sep-2017
        /// Description :  Bind page meta tags.
        /// </summary>
        /// <param name="pageMetaTags"></param>
        private void BindPageMetaTags(PageMetaTags pageMetaTags)
        {
            try
            {
                pageMetaTags.Title = "Please provide more details | BikeWale";
                pageMetaTags.Description = "Please provide more details to proceed further.";
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "PopUpCityArea.BindPageMetaTags()");
            }
        }
    }
}