using Bikewale.BindViewModels.Webforms.Compare;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Mobile.New
{
    /// <summary>
    /// Created By :  Sushil kumar on 2nd Feb 2017 
    /// Description : Bind compare bikes page with new logic and design
    /// </summary>
    public class CompareBikeDetails : System.Web.UI.Page
    {
        protected PageMetaTags pageMetas = null;
        protected GlobalCityAreaEntity cityArea;
        protected BikeCompareEntity vmCompare = null;
        protected bool isSponsoredBike, isUsedBikePresent;
        protected Int64 sponsoredVersionId = 0;
        protected string comparisionText = string.Empty, targetedModels = string.Empty;
        protected IEnumerable<BikeMakeEntityBase> objMakes = null;
        public SimilarCompareBikes ctrlSimilarBikes;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            BindCompareBikes();
        }

        /// <summary>
        /// Created By :  Sushil kumar on 2nd Feb 2017 
        /// Description : Bind viewmodel data to page level variables for compare bikes section
        ///                 Handle Redirectiona and pagenot found issues related to the same
        /// </summary>
        private void BindCompareBikes()
        {
            CompareBikes objCompare = new CompareBikes();
            try
            {
                if (objCompare.ProcessQueryString() && !objCompare.isPermanentRedirect && !objCompare.isPageNotFound)
                {
                    objCompare.GetComparedBikeDetails();
                    vmCompare = objCompare.comparedBikes;
                    pageMetas = objCompare.PageMetas;
                    isSponsoredBike = objCompare.SponsoredVersionId > 0;
                    sponsoredVersionId = objCompare.SponsoredVersionId;
                    comparisionText = objCompare.ComparisionText;
                    objMakes = objCompare.makes;
                    isUsedBikePresent = objCompare.isUsedBikePresent;
                    targetedModels = objCompare.TargetedModels;
                    BindPageWidgets(objCompare.versionsList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Mobile.New.CompareBikeDetails.BindCompareBikes");
                objCompare.isPageNotFound = true;
            }
            finally
            {
                if (objCompare.isPermanentRedirect)
                {
                    Bikewale.Common.CommonOpn.RedirectPermanent(objCompare.redirectionUrl);
                }
                else if (objCompare.isPageNotFound)
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// Created By :  Sushil kumar on 2nd Feb 2017 
        /// Description : Bind page related widgets
        /// </summary>
        /// <param name="versions"></param>
        private void BindPageWidgets(string versions)
        {
            if (ctrlSimilarBikes != null)
            {
                ctrlSimilarBikes.TopCount = 4;
                ctrlSimilarBikes.versionsList = versions;
            }

        }

    }   //End of Class
}   //End of namespace