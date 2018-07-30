using Bikewale.BindViewModels.Webforms.GenericBikes;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.SEO;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Generic
{
    /// <summary>
    /// Created By : Sushil Kumar on 22nd Dec 2016
    /// Description : DEsktop Page for Bike listing for generic bikes by bike type scooters,mileage bikes,sports bikes ,etc. 
    /// </summary>
    public class BikeListing : System.Web.UI.Page
    {
        protected BestBikes ctrlBestBikes;
        protected IEnumerable<BestBikeEntityBase> objBestBikes = null;
        protected PageMetaTags pageMetas = new PageMetaTags();
        protected string pageName = string.Empty;
        protected string pageContent = string.Empty;
        protected string bannerImageUrl = string.Empty, bannerImagePos = string.Empty;
        protected GlobalCityAreaEntity currentCityArea;
        protected PQSourceEnum pqSource;
        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }


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
            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            currentCityArea = GlobalCityArea.GetGlobalCityArea();

            GetBestBikesList();

        }

        /// <summary>
        /// Created By : Sushil Kumar on 22nd Dec 2016
        /// Description : Get best bikes list and bind other page related components 
        /// Modified by : Sajal Gupta on 03-02-2017
        /// Description : Call differnet funvtion if cityid is available.
        /// </summary>
        private void GetBestBikesList()
        {

            try
            {
                var objBestBikesvm = new BestBikesListing();
                if (!objBestBikesvm.IsPageNotFound)
                {
                    objBestBikesvm.TotalCount = 10;

                    if (currentCityArea.CityId > 0)
                        objBestBikesvm.FetchBestBikesList(objBestBikesvm.TotalCount, currentCityArea.CityId);
                    else
                        objBestBikesvm.FetchBestBikesList(objBestBikesvm.TotalCount);

                    if (objBestBikesvm.FetchedRecordCount > 0)
                    {
                        objBestBikes = objBestBikesvm.objBestBikesList;
                        pageMetas = objBestBikesvm.PageMetas;
                        pageContent = objBestBikesvm.PageContent;
                        pageName = objBestBikesvm.PageName;
                        bannerImageUrl = SetBannerImage(objBestBikesvm.BodyStyleType);
                        ctrlBestBikes.CurrentPage = objBestBikesvm.BodyStyleType;
                    }

                }
                else
                {
                    UrlRewrite.Return404();
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Generic.GetBestBikesList.GetBestBikesList");
                
            }
        }

        private string SetBannerImage(EnumBikeBodyStyles enumBikeBodyStyles)
        {
            string bannerImage = string.Empty;
            switch (enumBikeBodyStyles)
            {
                case EnumBikeBodyStyles.AllBikes:
                    pqSource = PQSourceEnum.Desktop_Generic_Bikes;
                    bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/d/best-bikes-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Cruiser:
                    pqSource = PQSourceEnum.Desktop_Generic_CruiserBikes;
                    bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/d/cruiser-style-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Mileage:
                    pqSource = PQSourceEnum.Desktop_Generic_MileageBikes;
                    bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/d/mileage-bikes-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Scooter:
                    pqSource = PQSourceEnum.Desktop_Generic_Scooters;
                    bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/d/scooter-style-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Sports:
                    pqSource = PQSourceEnum.Desktop_Generic_SportsBikes;
                    bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/d/sports-style-banner.jpg";
                    break;
                default:
                    pqSource = PQSourceEnum.Desktop_Generic_Bikes;
                    bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/d/best-bikes-banner.jpg";
                    break;

            }

            return bannerImage;
        }

    }
}