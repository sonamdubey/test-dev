﻿using Bikewale.BindViewModels.Webforms.Compare;
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
    public class CompareBikeDetails : System.Web.UI.Page
    {


        protected PageMetaTags pageMetas = null;
        protected GlobalCityAreaEntity cityArea;
        protected BikeCompareEntity vmCompare = null;
        protected bool isSponsoredBike, isUsedBikePresent;
        protected Int64 sponsoredVersionId = 0;
        protected string comparisionText = string.Empty, targetedModels = string.Empty;
        protected ICollection<BikeMakeEntityBase> objMakes = null;
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

        private void BindCompareBikes()
        {
            CompareBikes objCompare = new CompareBikes();
            try
            {
                if (!objCompare.isPermanentRedirect && !objCompare.isPageNotFound)
                {
                    cityArea = GlobalCityArea.GetGlobalCityArea();

                    objCompare.GetComparedBikeDetails();
                    vmCompare = objCompare.comparedBikes;
                    pageMetas = objCompare.PageMetas;
                    isSponsoredBike = objCompare.SponsoredVersionId > 0;
                    sponsoredVersionId = objCompare.SponsoredVersionId;
                    comparisionText = objCompare.ComparisionText;
                    objMakes = objCompare.makes;
                    isUsedBikePresent = objCompare.isUsedBikePresent;
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