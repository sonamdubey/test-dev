using Bikewale.Utility;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Bikewale.Models
{
    public class PopUpCityArea
    {
        public void GetData(string encodedQueryString)
        {
            PoupCityAreaVM objPopupCityAreaVM = new PoupCityAreaVM();
            try
            {
                string decodedQueryString = Utils.Utils.DecryptTripleDES(encodedQueryString);

                NameValueCollection queryCollection = HttpUtility.ParseQueryString(decodedQueryString);


                objPopupCityAreaVM.ModelId = SqlReaderConvertor.ToUInt16(queryCollection["modelid"]);
                objPopupCityAreaVM.IsPersistent = SqlReaderConvertor.ToBoolean(queryCollection["modelid"]);
                objPopupCityAreaVM.MakeName = queryCollection["modelid"];
                objPopupCityAreaVM.ModelName =queryCollection["modelid"];
                objPopupCityAreaVM.PQSourceId = SqlReaderConvertor.ToUInt16(queryCollection["modelid"]);
                objPopupCityAreaVM.PreSelectedCity = SqlReaderConvertor.ToUInt16(queryCollection["modelid"]);
                objPopupCityAreaVM.PageCategoryId = SqlReaderConvertor.ToUInt16(queryCollection["modelid"]);
                objPopupCityAreaVM.IsReload = SqlReaderConvertor.ToBoolean(queryCollection["modelid"]);

            }
            catch (Exception ex)
            {
                Notifications.ErrorClass objErr = new Notifications.ErrorClass(ex, "PopUpCityArea.GetData()");
            }
        }

    }
}