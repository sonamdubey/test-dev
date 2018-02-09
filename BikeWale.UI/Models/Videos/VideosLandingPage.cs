using ApiGatewayLibrary;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Schema;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.Images;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by Sajal Gupta on 31-03-2017
    /// Description : This model will bind data for videos landing page (Desktop + Mobile)
    /// Modified by :  Pratibha Verma on 8 Feb 2018
	/// Description :  Added one more parameter in constructor
    /// </summary>
    public class VideosLandingPage
    {
        private readonly IVideosCacheRepository _videosCache = null;
        private readonly IVideos _videos = null;
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelCache = null;

        public ushort LandingVideosTopCount { get; set; }
        public ushort ExpertReviewsTopCount { get; set; }
        public ushort FirstRideWidgetTopCount { get; set; }
        public ushort LaunchAlertWidgetTopCount { get; set; }
        public ushort FirstLookWidgetTopCount { get; set; }
        public ushort PowerDriftBlockbusterWidgetTopCount { get; set; }
        public ushort MotorSportsWidgetTopCount { get; set; }
        public ushort PowerDriftSpecialsWidgetTopCount { get; set; }
        public ushort PowerDriftTopMusicWidgetTopCount { get; set; }
        public ushort MiscellaneousWidgetTopCount { get; set; }
        public ushort BrandWidgetTopCount { get; set; }
        public bool IsMobile { get; set; }

        private ushort _pageNo = 1;
        private IBikeModels<BikeModelEntity, int> _objModelEntity = null;

        public VideosLandingPage(IVideos videos, IVideosCacheRepository videosCache, IBikeMakesCacheRepository bikeMakes, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelCache, IBikeModels<BikeModelEntity, int> objModelEntity)
        {
            _videos = videos;
            _videosCache = videosCache;
            _bikeMakes = bikeMakes;
            _objModelCache = objModelCache;
            _objModelEntity = objModelEntity;
        }

        /// Modified by : Snehal Dange on 29th Nov 2017
        /// Descritpion : Added ga for page
        /// Modified by : Pratibha Verma on 8 Feb 2018
        /// Description : Linkage of ImageWidget from Videos Landing Page
        public VideosLandingPageVM GetData()
        {
            VideosLandingPageVM objVM = null;
            bool isAPIData = Bikewale.Utility.BWConfiguration.Instance.UseAPIGateway;
            try
            {

                objVM = new VideosLandingPageVM();

                BindLandingVideos(objVM);
                VideosBySubcategory objSubCat = new VideosBySubcategory(_videos);

                if (isAPIData)
                {
                    isAPIData = GetDataFromApiGateWay(objVM, objSubCat);
                }


                if (!isAPIData)
                {
                    objVM.MotorSportsWidgetData = objSubCat.GetData("", "51", _pageNo, MotorSportsWidgetTopCount);
                    objVM.ExpertReviewsWidgetData = objSubCat.GetData("", "55", _pageNo, ExpertReviewsTopCount);
                    objVM.FirstRideWidgetData = objSubCat.GetData("", "57", _pageNo, FirstRideWidgetTopCount);
                    objVM.MiscellaneousWidgetData = objSubCat.GetData("", "58", _pageNo, MiscellaneousWidgetTopCount);
                    objVM.LaunchAlertWidgetData = objSubCat.GetData("", "59", _pageNo, LaunchAlertWidgetTopCount);
                    objVM.PowerDriftTopMusicWidgetData = objSubCat.GetData("", "60", _pageNo, PowerDriftTopMusicWidgetTopCount);
                    objVM.FirstLookWidgetData = objSubCat.GetData("", "61", _pageNo, FirstLookWidgetTopCount);
                    objVM.PowerDriftBlockbusterWidgetData = objSubCat.GetData("", "62", _pageNo, PowerDriftBlockbusterWidgetTopCount);
                    objVM.PowerDriftSpecialsWidgetData = objSubCat.GetData("", "63", _pageNo, PowerDriftSpecialsWidgetTopCount);
                }
                objVM.Brands = new BrandWidgetModel(BrandWidgetTopCount, _bikeMakes, _objModelCache).GetData(Entities.BikeData.EnumBikeType.Videos);
                BindPageMetas(objVM);
                objVM.Page = Entities.Pages.GAPages.Videos_Landing_Page;
                ImageCarausel imageCarausel = new ImageCarausel(0, 9, 7, EnumBikeBodyStyles.AllBikes, _objModelEntity);
                objVM.PopularSportsBikesWidget = imageCarausel.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "VideosLandingPage.GetData");
            }
            return objVM;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 4th May 2017
        /// Description : Function to call api gateway to fecth videos landing page widgets data
        /// </summary>
        private bool GetDataFromApiGateWay(VideosLandingPageVM objVM, VideosBySubcategory objSubCat)
        {
            bool isSuccess = false;
            try
            {
                string[] categoryIds = new string[] { "51", "55", "57", "58", "59", "60", "61", "62", "63" };
                ushort[] categoryTotalRecords = new ushort[] { MotorSportsWidgetTopCount, ExpertReviewsTopCount, FirstRideWidgetTopCount, MiscellaneousWidgetTopCount, LaunchAlertWidgetTopCount, PowerDriftTopMusicWidgetTopCount, FirstLookWidgetTopCount, PowerDriftBlockbusterWidgetTopCount, PowerDriftSpecialsWidgetTopCount };

                VideosBySubcategoryVM[] widgetsData = new VideosBySubcategoryVM[categoryIds.Length];
                CallAggregator ca = objSubCat.AddGrpcCallsToAPIGateway(categoryIds, categoryTotalRecords);


                var apiData = ca.GetResultsFromGateway();

                if (apiData != null && apiData.OutputMessages != null)
                {

                    var objApiData = apiData.OutputMessages;

                    if (objApiData != null && objApiData.Count > 0)
                    {

                        for (ushort i = 0; i < objApiData.Count; i++)
                        {
                            objSubCat.SetWidgetDataProperties("", categoryIds[i], objApiData[i].Payload, out widgetsData[i]);
                        }

                        objVM.MotorSportsWidgetData = widgetsData[0];
                        objVM.ExpertReviewsWidgetData = widgetsData[1];
                        objVM.FirstRideWidgetData = widgetsData[2];
                        objVM.MiscellaneousWidgetData = widgetsData[3];
                        objVM.LaunchAlertWidgetData = widgetsData[4];
                        objVM.PowerDriftTopMusicWidgetData = widgetsData[5];
                        objVM.FirstLookWidgetData = widgetsData[6];
                        objVM.PowerDriftBlockbusterWidgetData = widgetsData[7];
                        objVM.PowerDriftSpecialsWidgetData = widgetsData[8];

                    }

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "VideosLandingPage.GetDataFromApiGateWay()");

            }

            return isSuccess;

        }



        private void BindPageMetas(VideosLandingPageVM objPageVM)
        {
            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null)
                {
                    objPageVM.PageMetaTags.Title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison - BikeWale";
                    objPageVM.PageMetaTags.Keywords = "bike videos, video reviews, expert video reviews, road test videos, bike comparison videos";
                    objPageVM.PageMetaTags.Description = "Check latest bike and scooter videos, watch BikeWale expert's take on latest bikes and scooters - features, performance, price, fuel economy, handling and more.";

                }
                SetBreadcrumList(objPageVM);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "VideosLandingPage.GetData()");
            }
        }

        private void BindLandingVideos(VideosLandingPageVM objVM)
        {
            try
            {
                IEnumerable<BikeVideoEntity> objLandingVideosList = _videosCache.GetVideosByCategory(EnumVideosCategory.FeaturedAndLatest, LandingVideosTopCount);

                if (objLandingVideosList != null)
                {
                    if (objLandingVideosList.Any())
                        objVM.LandingFirstVideoData = objLandingVideosList.FirstOrDefault();
                    if (objLandingVideosList.Count() > 1)
                        objVM.LandingOtherVideosData = (objLandingVideosList.Skip(1)).Take(LandingVideosTopCount - 1);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "VideosLandingPage.BindLandingVideos");
            }
        }
        /// <summary>
        /// Created By :Snehal Dange on 8th Nov 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(VideosLandingPageVM objVM)
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string bikeUrl;
            bikeUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                bikeUrl += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Videos"));

            objVM.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }

    }
}