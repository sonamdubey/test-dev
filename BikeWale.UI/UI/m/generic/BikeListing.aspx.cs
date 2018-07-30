using Bikewale.BindViewModels.Webforms.GenericBikes;
using Bikewale.Common;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.SEO;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Mobile.Generic
{
    /// <summary>
    /// Created By : Sushil Kumar on 22nd Dec 2016
    /// Description : Mobile Page for Bike listing for generic bikes by bike type scooters,mileage bikes,sports bikes ,etc. 
    /// </summary>
    public class BikeListing : System.Web.UI.Page
    {
        protected BestBikes ctrlBestBikes;
        protected IEnumerable<BestBikeEntityBase> objBestBikes = null;
        protected PageMetaTags pageMetas = new PageMetaTags();
        protected string pageMaskingName = string.Empty, pageName = string.Empty;
        protected string pageContent = "";
        protected string bannerImageUrl = string.Empty, bannerImagePos = string.Empty;
        protected PQSourceEnum pqSource;
        protected GlobalCityAreaEntity currentCityArea;
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
                        pageMaskingName = objBestBikesvm.PageMaskingName;
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
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Mobile.Generic.GetBestBikesList.GetBestBikesList");
                
            }
        }

        private string SetBannerImage(EnumBikeBodyStyles enumBikeBodyStyles)
        {
            string bannerImage = string.Empty;
            switch (enumBikeBodyStyles)
            {
                case EnumBikeBodyStyles.AllBikes:
                    pqSource = PQSourceEnum.Desktop_Generic_Bikes;
                    bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/m/best-bikes-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Cruiser:
                    pqSource = PQSourceEnum.Desktop_Generic_CruiserBikes;
                    bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/m/cruiser-style-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Mileage:
                    pqSource = PQSourceEnum.Desktop_Generic_MileageBikes;
                    bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/m/mileage-bikes-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Scooter:
                    pqSource = PQSourceEnum.Desktop_Generic_Scooters;
                    bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/m/scooter-style-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Sports:
                    pqSource = PQSourceEnum.Desktop_Generic_SportsBikes;
                    bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/m/sports-style-banner.jpg";
                    break;
                default:
                    pqSource = PQSourceEnum.Desktop_Generic_Bikes;
                    bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/m/best-bikes-banner.jpg";
                    break;

            }

            return bannerImage;
        }

    }
}