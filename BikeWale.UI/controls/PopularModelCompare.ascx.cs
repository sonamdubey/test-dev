using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created by:-Subodh Jain on 12 sep 2016
    /// Description :- Compare popular similar bikes on model page
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : To check for sponsord bike for version and added variables for the same.REmoved repeater logic and dind data using list object
    /// </summary>
    public class PopularModelCompare : UserControl
    {
        public string versionId { get; set; }
        protected ICollection<SimilarCompareBikeEntity> objSimilarBikes = null;
        public string ModelName;
        public uint fetchedCount { get; set; }
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
        /// Created by : Subodh Jain 12 sep 2016
        /// Desc       : To bind popular compare  bikes for model page
        /// Modified By : Sushil Kumar on 2nd Dec 2016
        /// Description : To check for sponsord bike for version
        /// Modified By : Sushil Kumar on 11th Jan 2016
        /// Description : Commented method to get sponsored versions to skip binding of sponsored model
        /// </summary>
        private void BindPopularCompareBikes()
        {
            try
            {
                if (!string.IsNullOrEmpty(versionId))
                {
                    BindSimilarCompareBikesControl objAlt = new BindSimilarCompareBikesControl();
                    objAlt.cityid = cityid.HasValue && cityid > 0 ? cityid.Value : Convert.ToInt16(Bikewale.Utility.BWConfiguration.Instance.DefaultCity);
                    //SponsoredVersionId = objAlt.CheckSponsoredBikeForAnyVersion(versionId.ToString());
                    objSimilarBikes = objAlt.BindPopularCompareBikes(versionId, TopCount);
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
