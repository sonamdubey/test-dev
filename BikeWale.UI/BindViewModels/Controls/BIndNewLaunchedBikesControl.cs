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

        public void BindNewlyLaunchedBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;
            LaunchedBikeList objBikeList = null;

            try
            {

                string _apiUrl = String.Format("/api/NewLaunchedBike/?pageSize={0}&curPageNo={1}", pageSize, curPageNo);

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objBikeList = objClient.GetApiResponseSync<LaunchedBikeList>(Utility.APIHost.BW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objBikeList);
                }

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