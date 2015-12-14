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
        public int VersionId { get; set; }
        public int TopCpunt { get; set; }
        public int? Deviation { get; set; }
        public int FetchedRecordsCount { get; set; }

        public void BindAlternativeBikes(Repeater rptAlternativeBikes)
        {
            SimilarBikeList similarBikeList = null;
            FetchedRecordsCount = 0;

            try
            {
                string _apiUrl = String.Format("/api/SimilarBike/?versionId={0}&topCount={1}&deviation={2}", VersionId, TopCpunt,Deviation);
                                
                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    similarBikeList = objClient.GetApiResponseSync<SimilarBikeList>(Utility.APIHost.BW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, similarBikeList);
                }

                if (similarBikeList != null && similarBikeList.SimilarBike != null)
                {                       
                    FetchedRecordsCount = similarBikeList.SimilarBike.Count();

                    if (FetchedRecordsCount > 0)
                    {
                    
                        rptAlternativeBikes.DataSource = similarBikeList.SimilarBike;
                        rptAlternativeBikes.DataBind();
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