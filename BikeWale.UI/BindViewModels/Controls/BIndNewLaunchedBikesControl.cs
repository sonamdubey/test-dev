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
        public int pageSize { get; set; }
        public int? curPageNo { get; set; }
        public int FetchedRecordsCount { get; set; }

        readonly string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
        readonly string _requestType = "application/json";

        public void BindNewlyLaunchedBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;
            LaunchedBikeList objBikeList = null;

            try
            {

                string _apiUrl = String.Format("/api/NewLaunchedBike/?pageSize={0}&curPageNo={1}", pageSize, curPageNo);

                objBikeList = BWHttpClient.GetApiResponseSync<LaunchedBikeList>(_bwHostUrl, _requestType, _apiUrl, objBikeList);

                if (objBikeList != null)
                {
                    FetchedRecordsCount = objBikeList.LaunchedBike.Count();

                    if (FetchedRecordsCount > 0)
                    {
                        rptr.DataSource = objBikeList.LaunchedBike;
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