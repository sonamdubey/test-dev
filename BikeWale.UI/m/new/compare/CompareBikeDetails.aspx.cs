using Bikewale.BindViewModels.Webforms.Compare;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.Schema;
using Bikewale.Mobile.Controls;
using Bikewale.Models;
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
        protected BreadcrumbList breadcrumb = null;
        protected BikeCompareEntity vmCompare = null;
        protected bool isSponsoredBike, isUsedBikePresent;
        protected Int64 sponsoredVersionId = 0;
        protected string templateSummaryTitle = string.Empty, comparisionText = string.Empty, targetedModels = string.Empty, featuredBike = string.Empty,
            compareSummaryText = string.Empty, baseUrl = string.Empty, bikeQueryString = string.Empty, cityName = string.Empty;
        protected IEnumerable<BikeMakeEntityBase> objMakes = null;
        protected string sponsoredBikeImpressionTracker = String.Empty;
        protected string knowMoreLinkText = string.Empty;
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
            CompareBikesDetails objCompare = new CompareBikesDetails();

            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            try
            {
                objCompare.maxComparisions = 2;
                objCompare.originalUrl = originalUrl;
                if (objCompare.ProcessQueryString() && !objCompare.isPermanentRedirect && !objCompare.isPageNotFound && !objCompare.isCompareLandingRedirection)
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
                    featuredBike = objCompare.FeaturedBikeLink;
                    BindPageWidgets(objCompare.versionsList, objCompare.CityId);
                    compareSummaryText = objCompare.summaryText;
                    templateSummaryTitle = objCompare.TemplateSummaryTitle;
                    breadcrumb = objCompare.Breadcrumb;
                    if (objCompare.SponsoredBike != null)
                    {
                        sponsoredBikeImpressionTracker = objCompare.SponsoredBike.BikeImpressionTracker;
                        knowMoreLinkText = objCompare.SponsoredBike.LinkText;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Mobile.New.CompareBikeDetails.BindCompareBikes");
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
                    UrlRewrite.Return404();
                }
                else if (objCompare.isCompareLandingRedirection)
                {
                    Response.Redirect("/m/comparebikes/", false);
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
        private void BindPageWidgets(string versions, int cityId)
        {
            if (ctrlSimilarBikes != null)
            {
                ctrlSimilarBikes.TopCount = 8;
                ctrlSimilarBikes.CityId = cityId;
                ctrlSimilarBikes.versionsList = versions;
            }
        }
    }   //End of Class
}   //End of namespace