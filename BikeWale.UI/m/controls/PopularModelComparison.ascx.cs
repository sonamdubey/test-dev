using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.m.controls
{
    /// <summary>
    /// Created by : Sajal Gupta on 12/09/2016
    /// Desc       : View Model to bind and pass repeater data to control
    /// </summary>
    public class PopularModelComparison : System.Web.UI.UserControl
    {

        public Repeater rptPopularBikesComparison;
        public uint fetchedCount { get; set; }
        public IEnumerable<SimilarCompareBikeEntity> objSimilarBikes = null;
        public uint versionId { get; set; }
        public string versionName { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindSimilarCompareBikes();
        }

        /// <summary>
        /// Created by : Sajal Gupta on 12/09/2016
        /// Desc       : To bind similar bikes
        /// </summary>
        private void BindSimilarCompareBikes()
        {
            BindSimilarCompareBikesControl objAlt = new BindSimilarCompareBikesControl();

            try
            {

                objSimilarBikes = objAlt.BindAlternativeBikes(Convert.ToString(versionId), 6);

                if (objSimilarBikes != null)
                    fetchedCount = (uint)objSimilarBikes.Count();

                if (fetchedCount > 0)
                {
                    var source = from bike in objSimilarBikes
                                 select new
                                 {
                                     OriginalImagePath1 = bike.OriginalImagePath1,
                                     OriginalImagePath2 = bike.OriginalImagePath2,
                                     Model1 = bike.Model1,
                                     Model2 = bike.Model2,
                                     Price1 = Format.FormatPrice(bike.Price1.ToString()),
                                     Price2 = Format.FormatPrice(bike.Price2.ToString()),
                                     MakeMasking1 = bike.MakeMasking1,
                                     MakeMasking2 = bike.MakeMasking2,
                                     VersionId1 = bike.VersionId1,
                                     VersionId2 = bike.VersionId2,
                                     ModelMasking1 = bike.ModelMaskingName1,
                                     ModelMasking2 = bike.ModelMaskingName2,
                                     HostUrl1 = bike.HostUrl1,
                                     HostUrl2 = bike.HostUrl2,

                                 };

                    rptPopularBikesComparison.DataSource = source.Distinct();
                    rptPopularBikesComparison.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        public override void Dispose()
        {
            rptPopularBikesComparison.DataSource = null;
            rptPopularBikesComparison.Dispose();
            base.Dispose();
        }
    }
}