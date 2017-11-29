using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by : Sajal Gupta on 12/09/2016
    /// Desc       : View Model to bind and pass repeater data to control
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : To check for sponsord bike for version and added variables for the same.REmoved repeater logic and dind data using list object
    /// </summary>
    public class PopularModelComparison : System.Web.UI.UserControl
    {

        public uint fetchedCount { get; set; }
        protected ICollection<SimilarCompareBikeEntity> objSimilarBikes = null;
        public uint versionId { get; set; }
        public string versionName { get; set; }
        public int? cityid { get; set; }
        public ushort TopCount { get; set; }
        public Int64 SponsoredVersionId { get; set; }
        public String FeaturedBikeLink { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindPopularCompareBikes();
        }

        /// <summary>
        /// Created by : Sajal Gupta on 12/09/2016
        /// Desc       : To bind similar bikes
        /// Modified By : Sushil Kumar on 2nd Dec 2016
        /// Description : To check for sponsord bike for version
        /// Modified By : Sushil Kumar on 11th Jan 2016
        /// Description : Commented method to get sponsored versions to skip binding of sponsored model
        /// </summary>
        private void BindPopularCompareBikes()
        {
            try
            {
                if (versionId > 0)
                {
                    BindSimilarCompareBikesControl objAlt = new BindSimilarCompareBikesControl();
                    objAlt.cityid = cityid.HasValue && cityid > 0 ? cityid.Value : Convert.ToInt16(Bikewale.Utility.BWConfiguration.Instance.DefaultCity);
                    //SponsoredVersionId = objAlt.CheckSponsoredBikeForAnyVersion(versionId.ToString());
                    objSimilarBikes = objAlt.BindPopularCompareBikes(versionId.ToString(), TopCount);
                    fetchedCount = objAlt.FetchedRecordsCount;
                    FeaturedBikeLink = objAlt.FeaturedBikeLink;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + "BindPopularCompareBikes");
            }

        }

    }
}