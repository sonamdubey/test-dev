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

                objPopupCityAreaVM.ModelId = SqlReaderConvertor.ToUInt16(queryCollection["modelid"]);
                objPopupCityAreaVM.PQSourceId = SqlReaderConvertor.ToUInt16(queryCollection["pqsourceid"]);
                objPopupCityAreaVM.PreSelectedCity = SqlReaderConvertor.ToUInt16(queryCollection["preselcity"]);
                objPopupCityAreaVM.Url = Convert.ToString(queryCollection["Url"]);
            }
            catch (Exception ex)
            {
                Notifications.ErrorClass objErr = new Notifications.ErrorClass(ex, "PopUpCityArea.GetData()");
            }
            return objPopupCityAreaVM;
        }

    }
}