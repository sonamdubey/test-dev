using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.DTO.BikeData;
using System.Configuration;
using Bikewale.Common;

namespace Bikewale.BindViewModels.Controls
{
    public class BindAlternativeBikesControl
    {
        public static int VersionId { get; set; }
        public static int TopCpunt { get; set; }
        public static int? Deviation { get; set; }
        public static int FetchedRecordsCount { get; set; }

        public static void BindAlternativeBikes(Repeater rptAlternativeBikes)
        {
            SimilarBikeList similarBikeList = null;
            FetchedRecordsCount = 0;

            try
            {
                string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = String.Format("/api/SimilarBike/?versionId={0}&topCount={1}&deviation={2}", VersionId, TopCpunt,Deviation);

                similarBikeList = BWHttpClient.GetApiResponseSync<SimilarBikeList>(_bwHostUrl, _requestType, _apiUrl, similarBikeList);

                if (similarBikeList != null && similarBikeList.SimilarBike.ToList().Count > 0)
                {
                    FetchedRecordsCount = similarBikeList.SimilarBike.ToList().Count;
                    rptAlternativeBikes.DataSource = similarBikeList.SimilarBike.ToList();
                    rptAlternativeBikes.DataBind();
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