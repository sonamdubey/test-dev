using Bikewale.Common;
using Bikewale.DTO.BikeData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    public class BindNewLaunchedBikesControl
    {
        public static int pageSize { get; set; }
        public static int? curPageNo { get; set; }
        public static int FetchedRecordsCount { get; set; }

        public static void BindNewlyLaunchedBikes(Repeater rptr)
        {
            LaunchedBikeList objBikeList = null;

            try
            {
                string _bwHostUrl = ConfigurationManager.AppSettings["bwApiHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = String.Format("api/NewLaunchedBike/?pageSize={0}&curPageNo={1}",pageSize,curPageNo);

                objBikeList = BWHttpClient.GetApiResponseSync<LaunchedBikeList>(_bwHostUrl, _requestType, _apiUrl, objBikeList);

                if (objBikeList != null && objBikeList.LaunchedBike.ToList().Count > 0)
                {
                    FetchedRecordsCount = objBikeList.LaunchedBike.ToList().Count;
                    rptr.DataSource = objBikeList.LaunchedBike.ToList();
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