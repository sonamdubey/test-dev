using Bikewale.Common;
using Bikewale.DTO.BikeData;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    public class BindUpcomingBikesControl
    {
        
        public static int sortBy {get;set;}
        public static int pageSize { get; set; }
        public static int? MakeId { get; set; }
        public static int? ModelId { get; set; }
        public static int? curPageNo { get; set; }
        public static int  FetchedRecordsCount {get;set;}

        public static void BindUpcomingBikes(Repeater rptr)
        {
            UpcomingBikeList objBikeList = null;
            
            try
            {
                string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = String.Format("api/UpcomingBike/?sortBy={0}&pageSize={1}", sortBy, pageSize);


                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    _apiUrl += String.Format("&makeId={0}&curPageNo={1}", MakeId, curPageNo);

                    if (ModelId.HasValue && ModelId.Value > 0)
                    {
                        _apiUrl += String.Format("&modelId={0}", ModelId);
                    }
                }

                objBikeList = BWHttpClient.GetApiResponseSync<UpcomingBikeList>(_bwHostUrl, _requestType, _apiUrl, objBikeList);

                if (objBikeList != null && objBikeList.UpcomingBike.ToList().Count > 0)
                {
                    FetchedRecordsCount = objBikeList.UpcomingBike.ToList().Count;
                    rptr.DataSource = objBikeList.UpcomingBike.ToList();
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