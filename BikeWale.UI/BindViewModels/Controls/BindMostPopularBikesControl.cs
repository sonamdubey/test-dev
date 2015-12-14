using Bikewale.DTO.Widgets;
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
    public class BindMostPopularBikesControl
    {
        public int? totalCount { get; set; }
        public int? makeId { get; set; }
        public int FetchedRecordsCount { get; set; }

        public void BindMostPopularBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;

            MostPopularBikesList objBikeList = null;

            try
            {
                string _apiUrl = String.Format("/api/ModelList/?totalCount={0}&makeId={1}", totalCount, makeId);

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //objBikeList = objClient.GetApiResponseSync<MostPopularBikesList>(Utility.BWConfiguration.Instance.BwHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objBikeList);
                    objBikeList = objClient.GetApiResponseSync<MostPopularBikesList>(Utility.APIHost.BW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objBikeList);
                }

                if (objBikeList != null)
                {
                    var bikeList = objBikeList.PopularBikes.ToList();
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