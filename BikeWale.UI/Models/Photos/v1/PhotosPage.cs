using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Models.Images;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.Photos.v1
{
    /// <summary>
    /// Created by  : Rajan Chauhan on 11 Jan 2017
    /// Description :  To bind photo page
    /// </summary>
    public class PhotosPage
    {
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;
        private readonly IVideos _videos = null;
        public bool IsMobile { get; set; }
        private uint _pageNo;
        public uint PageSize;
        /// <summary>
        /// Number of videos to be fetched for videos widget.
        /// </summary>
        public ushort VideosCount { get; set; }


        /// <summary>
        /// Created by  :  Rajan Chauhan on 11 jan 2017
        /// Description :  To resolve depedencies for photo page
        /// </summary>
        public PhotosPage(bool isMobile, IBikeModels<BikeModelEntity, int> objModelEntity, IBikeMakesCacheRepository objMakeCache, IVideos videos , uint? pageNo)
        {
            IsMobile = isMobile;
            _objModelEntity = objModelEntity;
            _objMakeCache = objMakeCache;
            _videos = videos;
            _pageNo = (pageNo == null) ? 1 : (uint)pageNo;
        }

        /// <summary>
        /// Created by  :  Rajan Chauhan on 11 jan 2017
        /// Descritpion :  Added PageData for page
        /// </summary>
        /// <returns></returns>
        public PhotosPageVM GetData()
        {
            PhotosPageVM objData = null;
            try
            {
                objData = new PhotosPageVM();
                objData.PopularSportsBikesWidget = GetPopularSportsBikeWidget();
                BindBikeModelsPhotos(objData);
                BindMakesWidget(objData);
                BindVideosWidget(objData);
                SetBreadcrumList(ref objData);
                SetPageMetas(objData);
                objData.ImagesSynopsis = "BikeWale brings you high quality images of 250+ bike models and 50+ scooters in India. Be it your dream bike, or the one you are planning to buy next month, we have got good quality bike images for all your needs. Bike images are of paramount importance while one is planning to buy a bike. View images of your favorite motorcycle in multiple colors and different angles.";
                SetPageJSONLDSchema(objData);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.v1.PhotosPage : GetData");
            }

            return objData;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 06 Feb 2017
        /// Description :  Method to bind recent videos.
        /// </summary>
        /// <param name="objData"></param>
        private void BindVideosWidget(PhotosPageVM objData)
        {
            try
            {
                objData.Videos = new RecentVideos(1, VideosCount, _videos).GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.v1.PhotosPage : BindVideosWidget");

            }
        }

        private void CreatePager(PhotosPageVM objData)
        {
            try
            {
                int totalPageCount = (int)(objData.TotalBikeModels / PageSize);
                totalPageCount = objData.TotalBikeModels % PageSize > 0 ? totalPageCount + 1 : totalPageCount;
                string baseUrl = "/images/";
                objData.Pager = new Entities.Pager.PagerEntity
                {
                    BaseUrl = baseUrl,
                    PageUrlType = "page-",
                    PageNo = (int)_pageNo,
                    PageSize = (int)PageSize,
                    TotalResults = objData.TotalBikeModels
                };
                string prevUrl = string.Empty, nextUrl = string.Empty;
                Paging.CreatePrevNextUrl(totalPageCount, baseUrl, objData.Pager.PageNo, objData.Pager.PageUrlType, ref nextUrl, ref prevUrl);
                nextUrl = _pageNo > totalPageCount ? "" : nextUrl;
                objData.PageMetaTags.NextPageUrl = nextUrl;
                objData.PageMetaTags.PreviousPageUrl = prevUrl;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.v1.PhotosPage : CreatePager");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 13 Jan 2018
        /// Description : Method to get photos of bike models, method retrieves model ids based on current page no. and bind photos of those models. 
        /// </summary>
        /// <param name="objData">View Model of photos page.</param>
        private void BindBikeModelsPhotos(PhotosPageVM objData)
        {
            try
            {
                int startIndex = (int)((_pageNo - 1) * PageSize + 1);
                int endIndex = (int)(_pageNo * PageSize);
                ImagePager pager = new ImagePager()
                {
                    PageNo = (int)_pageNo,
                    StartIndex = startIndex,
                    EndIndex = endIndex,
                    PageSize = (int)PageSize
                };
                IEnumerable<ModelIdWithBodyStyle> objModelIds = _objModelEntity.GetModelIdsForImages(0, EnumBikeBodyStyles.AllBikes, ref pager);
                string modelIds = string.Join(",", objModelIds.Select(m => m.ModelId));
                objData.TotalBikeModels = pager.TotalResults;
                int requiredImageCount = 4;
                string categoryIds = CommonApiOpn.GetContentTypesString(
                    new List<EnumCMSContentType>()
                    {
                        EnumCMSContentType.PhotoGalleries,
                        EnumCMSContentType.RoadTest
                    }
                );
                objData.BikeModelsPhotos = _objModelEntity.GetBikeModelsPhotos(modelIds, categoryIds, requiredImageCount);
                CreatePager(objData);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.v1.Photos.PhotosPage : BindBikeModelsPhotos_{0}", objData));
            }
        }

        /// <summary>
        /// Created by  :  Rajan Chauhan on 11 jan 2017
        /// Descritpion :  To Add otherPopularMakes Widget in VM
        /// </summary>
        /// <returns></returns>
        private void BindMakesWidget(PhotosPageVM objData)
        {
            IEnumerable<BikeMakeEntityBase> makes = _objMakeCache.GetMakesByType(EnumBikeType.Photos).Take(9);
            objData.OtherPopularMakes = new OtherMakesVM()
            {
                Makes = makes,
                PageLinkFormat = "/{0}-bikes/",
                PageTitleFormat = "{0} Bikes",
                CardText = "bike"
            };
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 23 Jan 2018
        /// Description : Method to set page metas.
        /// </summary>
        /// <param name="objData">View Model of Photos page.</param>
        private void SetPageMetas(PhotosPageVM objData)
        {
            try
            {
                int totalPageCount = (int)(objData.TotalBikeModels / PageSize);
                totalPageCount = objData.TotalBikeModels % PageSize > 0 ? totalPageCount + 1 : totalPageCount;
                string title = "Bike Images, Bike Photos | Latest Bike Wallpapers - BikeWale";
                string description = "BikeWale brings you high-quality images of latest bikes in India. Images of 250+ bike models are available in different colors and angles.View images and photo gallery of your favorite motorcycle on BikeWale.";

                if (_pageNo == 1)
                {
                    objData.PageMetaTags.Title = title;
                    objData.PageMetaTags.Description = description;
                }
                else
                {
                    objData.PageMetaTags.Title = string.Format("Page {0} of {1} - {2}", _pageNo, totalPageCount, title);
                    objData.PageMetaTags.Description = string.Format("Page {0} of {1} - {2}", _pageNo, totalPageCount, description);
                }
                objData.PageMetaTags.CanonicalUrl = string.Format("{0}/images/", BWConfiguration.Instance.BwHostUrl);
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/images/", BWConfiguration.Instance.BwHostUrl);
                objData.PageMetaTags.Keywords = "bike images, bike photos, bike wallpapers, bike image gallery, bike photo gallery, bike images india, bike photos india, bike gallery india";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.v1.PhotosPage.SetPageMetas()");
            }

        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 15th Jan 2018
        /// Description : Set breadcrum list for image landing page 
        /// </summary>
        /// <param name="objData"></param>
        private void SetBreadcrumList(ref PhotosPageVM objData)
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                url += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
            url = string.Format("{0}new-bikes-in-india/", url);
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "New Bikes"));
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Images"));
            objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 12th Jan 2018
        /// Description : Bind sports bike image widget
        /// Modified by : Pratibha Verma on 8 Feb 2018
        /// Description : Get data from imageCarausel class
        /// </summary>
        /// <returns></returns>
        private ImageWidgetVM GetPopularSportsBikeWidget()
        {
            ImageWidgetVM imageWidget = null;
            try
            {
                ImageCarausel imageCarausel = new ImageCarausel(0,9,7, EnumBikeBodyStyles.Sports, _objModelEntity);
                imageWidget = imageCarausel.GetData();               
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.v1.PhotosPage");
            }
            return imageWidget;
        }
        private void SetPageJSONLDSchema(PhotosPageVM objData)
        {
            WebPage webpage = SchemaHelper.GetWebpageSchema(objData.PageMetaTags, objData.BreadcrumbList);

            if (webpage != null)
            {
                objData.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }
    }
}