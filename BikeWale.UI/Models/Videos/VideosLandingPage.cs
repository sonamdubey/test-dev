using ApiGatewayLibrary;
using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using EditCMSWindowsService.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by Sajal Gupta on 31-03-2017
    /// Description : This model will bind data for videos landing page (Desktop + Mobile)
    /// </summary>
    public class VideosLandingPage
    {
        private readonly IVideosCacheRepository _videosCache = null;
        private readonly IVideos _videos = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
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

        private ushort _pageNo = 1;

        public VideosLandingPage(IVideos videos, IVideosCacheRepository videosCache, IBikeMakes<BikeMakeEntity, int> bikeMakes, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelCache)
        {
            _videos = videos;
            _videosCache = videosCache;
            _bikeMakes = bikeMakes;
            _objModelCache = objModelCache;
        }

        public VideosLandingPageVM GetData()
        {
            VideosLandingPageVM objVM = null;
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {

                objVM = new VideosLandingPageVM();

                BindLandingVideos(objVM);
                VideosBySubcategory objSubCat = new VideosBySubcategory(_videos);

                if (!Bikewale.Utility.BWConfiguration.Instance.UseAPIGateWay)
                {
                    objVM.ExpertReviewsWidgetData = objSubCat.GetData("", "55", _pageNo, ExpertReviewsTopCount);
                    objVM.FirstRideWidgetData = objSubCat.GetData("", "57", _pageNo, FirstRideWidgetTopCount);
                    objVM.LaunchAlertWidgetData = objSubCat.GetData("", "59", _pageNo, LaunchAlertWidgetTopCount);
                    objVM.FirstLookWidgetData = objSubCat.GetData("", "61", _pageNo, FirstLookWidgetTopCount);
                    objVM.PowerDriftBlockbusterWidgetData = objSubCat.GetData("", "62", _pageNo, PowerDriftBlockbusterWidgetTopCount);
                    objVM.MotorSportsWidgetData = objSubCat.GetData("", "51", _pageNo, MotorSportsWidgetTopCount);
                    objVM.PowerDriftSpecialsWidgetData = objSubCat.GetData("", "63", _pageNo, PowerDriftSpecialsWidgetTopCount);
                    objVM.PowerDriftTopMusicWidgetData = objSubCat.GetData("", "60", _pageNo, PowerDriftTopMusicWidgetTopCount);
                    objVM.MiscellaneousWidgetData = objSubCat.GetData("", "58", _pageNo, MiscellaneousWidgetTopCount);
                    objVM.Brands = new BrandWidgetModel(BrandWidgetTopCount, _bikeMakes, _objModelCache).GetData(Entities.BikeData.EnumBikeType.Videos);
                }
                else
                {
                    GetDataFromApiGateWay(objVM);
                }

                BindPageMetas(objVM);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "VideosLandingPage.GetData");
            }
            finally
            {
                watch.Stop();
                long elapsedMs = watch.ElapsedMilliseconds;
                log4net.ThreadContext.Properties["TimeTaken_Page"] = elapsedMs;
                ErrorClass objPageLog = new ErrorClass(new Exception("Videos Page Performance"), "VideosLandingPage.GetData-Page");
            }
            return objVM;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 4th May 2017
        /// Description : Function to call api gateway to fecth videos landing page widgets data
        /// </summary>
        private void GetDataFromApiGateWay(VideosLandingPageVM objVM)
        {
            CallAggregator ca = new CallAggregator();
            ca.AddCall("EditCMS", "GetVideosBySubCategories", new GrpcVideosBySubCategoriesURI()
            {
                ApplicationId = 2,
                SubCategoryIds = "55",
                StartIndex = _pageNo,
                EndIndex = ExpertReviewsTopCount,
                SortCategory = GrpcVideoSortOrderCategory.MostPopular
            });
            ca.AddCall("EditCMS", "GetVideosBySubCategories", new GrpcVideosBySubCategoriesURI()
            {
                ApplicationId = 2,
                SubCategoryIds = "57",
                StartIndex = _pageNo,
                EndIndex = FirstRideWidgetTopCount,
                SortCategory = GrpcVideoSortOrderCategory.MostPopular
            });
            ca.AddCall("EditCMS", "GetVideosBySubCategories", new GrpcVideosBySubCategoriesURI()
            {
                ApplicationId = 2,
                SubCategoryIds = "59",
                StartIndex = _pageNo,
                EndIndex = LaunchAlertWidgetTopCount,
                SortCategory = GrpcVideoSortOrderCategory.MostPopular
            });
            ca.AddCall("EditCMS", "GetVideosBySubCategories", new GrpcVideosBySubCategoriesURI()
            {
                ApplicationId = 2,
                SubCategoryIds = "61",
                StartIndex = _pageNo,
                EndIndex = FirstLookWidgetTopCount,
                SortCategory = GrpcVideoSortOrderCategory.MostPopular
            });
            ca.AddCall("EditCMS", "GetVideosBySubCategories", new GrpcVideosBySubCategoriesURI()
            {
                ApplicationId = 2,
                SubCategoryIds = "62",
                StartIndex = _pageNo,
                EndIndex = PowerDriftBlockbusterWidgetTopCount,
                SortCategory = GrpcVideoSortOrderCategory.MostPopular
            });
            ca.AddCall("EditCMS", "GetVideosBySubCategories", new GrpcVideosBySubCategoriesURI()
            {
                ApplicationId = 2,
                SubCategoryIds = "51",
                StartIndex = _pageNo,
                EndIndex = MotorSportsWidgetTopCount,
                SortCategory = GrpcVideoSortOrderCategory.MostPopular
            });
            ca.AddCall("EditCMS", "GetVideosBySubCategories", new GrpcVideosBySubCategoriesURI()
            {
                ApplicationId = 2,
                SubCategoryIds = "63",
                StartIndex = _pageNo,
                EndIndex = PowerDriftSpecialsWidgetTopCount,
                SortCategory = GrpcVideoSortOrderCategory.MostPopular
            });
            ca.AddCall("EditCMS", "GetVideosBySubCategories", new GrpcVideosBySubCategoriesURI()
            {
                ApplicationId = 2,
                SubCategoryIds = "60",
                StartIndex = _pageNo,
                EndIndex = PowerDriftTopMusicWidgetTopCount,
                SortCategory = GrpcVideoSortOrderCategory.MostPopular
            });
            ca.AddCall("EditCMS", "GetVideosBySubCategories", new GrpcVideosBySubCategoriesURI()
            {
                ApplicationId = 2,
                SubCategoryIds = "58",
                StartIndex = _pageNo,
                EndIndex = MiscellaneousWidgetTopCount,
                SortCategory = GrpcVideoSortOrderCategory.MostPopular
            });

            var apiData = ca.GetResultsFromGateway();

            if (apiData != null && apiData.OutputMessages != null)
            {
                var objApiData = apiData.OutputMessages;

                if (objApiData[0] != null)
                {
                    objVM.ExpertReviewsWidgetData = new VideosBySubcategoryVM();
                    objVM.ExpertReviewsWidgetData.VideoList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(Utilities.ConvertBytesToMsg<GrpcVideoListEntity>(objApiData[0].Payload));
                }
                if (objApiData[1] != null)
                {
                    objVM.FirstRideWidgetData = new VideosBySubcategoryVM();
                    objVM.FirstRideWidgetData.VideoList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(Utilities.ConvertBytesToMsg<GrpcVideoListEntity>(objApiData[1].Payload));
                }
                if (objApiData[2] != null)
                {
                    objVM.LaunchAlertWidgetData = new VideosBySubcategoryVM();
                    objVM.LaunchAlertWidgetData.VideoList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(Utilities.ConvertBytesToMsg<GrpcVideoListEntity>(objApiData[2].Payload));
                }
                if (objApiData[3] != null)
                {
                    objVM.FirstLookWidgetData = new VideosBySubcategoryVM();
                    objVM.FirstLookWidgetData.VideoList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(Utilities.ConvertBytesToMsg<GrpcVideoListEntity>(objApiData[3].Payload));
                }
                if (objApiData[4] != null)
                {
                    objVM.PowerDriftBlockbusterWidgetData = new VideosBySubcategoryVM();
                    objVM.PowerDriftBlockbusterWidgetData.VideoList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(Utilities.ConvertBytesToMsg<GrpcVideoListEntity>(objApiData[4].Payload));
                }
                if (objApiData[5] != null)
                {
                    objVM.MotorSportsWidgetData = new VideosBySubcategoryVM();
                    objVM.MotorSportsWidgetData.VideoList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(Utilities.ConvertBytesToMsg<GrpcVideoListEntity>(objApiData[5].Payload));
                }
                if (objApiData[6] != null)
                {
                    objVM.PowerDriftSpecialsWidgetData = new VideosBySubcategoryVM();
                    objVM.PowerDriftSpecialsWidgetData.VideoList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(Utilities.ConvertBytesToMsg<GrpcVideoListEntity>(objApiData[6].Payload));
                }
                if (objApiData[7] != null)
                {
                    objVM.PowerDriftTopMusicWidgetData = new VideosBySubcategoryVM();
                    objVM.PowerDriftTopMusicWidgetData.VideoList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(Utilities.ConvertBytesToMsg<GrpcVideoListEntity>(objApiData[7].Payload));
                }

                if (objApiData[8] != null)
                {
                    objVM.MiscellaneousWidgetData = new VideosBySubcategoryVM();
                    objVM.MiscellaneousWidgetData.VideoList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(Utilities.ConvertBytesToMsg<GrpcVideoListEntity>(objApiData[8].Payload));
                }
            }

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
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "VideosLandingPage.GetData()");
            }
        }

        private void BindLandingVideos(VideosLandingPageVM objVM)
        {
            try
            {
                IEnumerable<BikeVideoEntity> objLandingVideosList = _videosCache.GetVideosByCategory(EnumVideosCategory.FeaturedAndLatest, LandingVideosTopCount);

                if (objLandingVideosList != null)
                {
                    if (objLandingVideosList.Count() > 0)
                        objVM.LandingFirstVideoData = objLandingVideosList.FirstOrDefault();
                    if (objLandingVideosList.Count() > 1)
                        objVM.LandingOtherVideosData = (objLandingVideosList.Skip(1)).Take(LandingVideosTopCount - 1);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "VideosLandingPage.BindLandingVideos");
            }
        }
    }
}