
using Bikewale.DAL.Compare;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Compare;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 12 May 2016
    /// Desc       : View Model to bind and pass repeater data to control
    /// </summary>
    public class BindSimilarCompareBikesControl
    {
        public int VersionId { get; set; }
        public int TopCount { get; set; }
        public int? Deviation { get; set; }
        public int FetchedRecordsCount { get; set; }

        public void BindAlternativeBikes(Repeater rptSimlarCompareBikes, string versionList, uint count)
        {
            FetchedRecordsCount = 0;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IEnumerable<SimilarCompareBikeEntity> objSimilarBikes = new List<SimilarCompareBikeEntity>();
                    IBikeCompare objCompare = null;
                    using (IUnityContainer objPQCont = new UnityContainer())
                    {
                        objPQCont.RegisterType<IBikeCompare, BikeCompareRepository>();
                        objCompare = objPQCont.Resolve<IBikeCompare>();
                        objSimilarBikes = objCompare.GetSimilarCompareBikes(versionList, count);
                    }

                    if (objSimilarBikes.Count() > 0)
                    {
                        rptSimlarCompareBikes.DataSource = objSimilarBikes;
                        rptSimlarCompareBikes.DataBind();
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