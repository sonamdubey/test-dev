using Bikewale.BindViewModels.Webforms.GenericBikes;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Generic
{
    public class BikeListing : System.Web.UI.Page
    {
        protected BestBikes ctrlBestBikes;
        protected IEnumerable<BestBikeEntityBase> objBestBikes = null;
        protected PageMetaTags pageMetas = new PageMetaTags();
        protected string pageMaskingName = string.Empty, pageName = string.Empty;
        protected string pageContent = "";
        protected string bannerImageUrl = string.Empty;


        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Fetch most pouplar bikes list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();

            GetBestBikesList();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Get news list
        /// </summary>
        private void GetBestBikesList()
        {

            try
            {
                var objBestBikesvm = new BestBikesListing();
                if (!objBestBikesvm.IsPageNotFound)
                {
                    objBestBikesvm.TotalCount = 10;
                    objBestBikesvm.FetchBestBikesList(objBestBikesvm.TotalCount);
                    if (objBestBikesvm.FetchedRecordCount > 0)
                    {
                        objBestBikes = objBestBikesvm.objBestBikesList;
                        pageMetas = objBestBikesvm.PageMetas;
                        pageContent = objBestBikesvm.PageContent;
                        pageMaskingName = objBestBikesvm.PageMaskingName;
                        pageName = objBestBikesvm.PageName;
                        bannerImageUrl = SetBannerImage(objBestBikesvm.BodyStyleType);

                    }

                }
                else
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }


            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.Generic.GetBestBikesList.GetNewsList");
                objErr.SendMail();
            }
        }

        private string SetBannerImage(EnumBikeBodyStyles enumBikeBodyStyles)
        {
            string bannerImage = string.Empty;
            switch (enumBikeBodyStyles)
            {
                case EnumBikeBodyStyles.AllBikes:
                    bannerImage = "https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/top-bikes-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Cruiser:
                    bannerImage = "https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/cruiser-style-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Mileage:
                    bannerImage = "https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/mileage-bikes-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Scooter:
                    bannerImage = "https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/scooter-style-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Sports:
                    bannerImage = "https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/sports-style-banner.jpg";
                    break;
                default:
                    bannerImage = "https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/top-bikes-banner.jpg";
                    break;

            }

            return bannerImage;
        }

    }
}