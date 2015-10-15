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
        static readonly string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
        static readonly string _requestType = "application/json";

        public static void BindNewlyLaunchedBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;

            LaunchedBikeList objBikeList = null;

            try
            {

                string _apiUrl = String.Format("/api/NewLaunchedBike/?pageSize={0}&curPageNo={1}",pageSize,curPageNo);

                objBikeList = BWHttpClient.GetApiResponseSync<LaunchedBikeList>(_bwHostUrl, _requestType, _apiUrl, objBikeList);

                if (objBikeList != null)
                {
                  var bikeList = objBikeList.LaunchedBike.ToList();
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