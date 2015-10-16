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

        public int sortBy { get; set; }
        public int pageSize { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int? curPageNo { get; set; }
        public int FetchedRecordsCount { get; set; }

        readonly string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
        readonly string _requestType = "application/json";

        public void BindUpcomingBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;

            UpcomingBikeList objBikeList = null;

            try
            {
                string _apiUrl = String.Format("/api/UpcomingBike/?sortBy={0}&pageSize={1}", sortBy, pageSize);


                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    _apiUrl += String.Format("&makeId={0}&curPageNo={1}", MakeId, curPageNo);

                    if (ModelId.HasValue && ModelId.Value > 0)
                    {
                        _apiUrl += String.Format("&modelId={0}", ModelId);
                    }
                }

                objBikeList = BWHttpClient.GetApiResponseSync<UpcomingBikeList>(_bwHostUrl, _requestType, _apiUrl, objBikeList);

                if (objBikeList != null && objBikeList.UpcomingBike != null)
                {
                    FetchedRecordsCount = objBikeList.UpcomingBike.Count();

                    if (FetchedRecordsCount > 0)
                    {

                        rptr.DataSource = objBikeList.UpcomingBike;
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