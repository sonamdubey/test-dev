using Bikewale.DTO.Make;
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
    public static class BindMakePage
    {
        public static int? totalCount { get; set; }
        public static int? makeId { get; set; }
        public static int FetchedRecordsCount { get; set; }
        public static MakeBase Make { get; set; }
        public static BikeDescription BikeDesc { get; set; }

        public static void BindMostPopularBikes(Repeater rptr)
        {
            MakePage objBikeList = null;
            Make = new MakeBase();
            BikeDesc = new BikeDescription();
            FetchedRecordsCount = 0;
            try
            {
                string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = String.Format("/api/MakePage/?makeId={0}", makeId);

                objBikeList = BWHttpClient.GetApiResponseSync<MakePage>(_bwHostUrl, _requestType, _apiUrl, objBikeList);

                if (objBikeList != null && objBikeList.PopularBikes != null && objBikeList.PopularBikes.Count() > 0)
                {
                    FetchedRecordsCount = objBikeList.PopularBikes.Count();
                    Make = objBikeList.PopularBikes.FirstOrDefault().objMake;
                    BikeDesc = objBikeList.Description;
                    rptr.DataSource = objBikeList.PopularBikes;
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