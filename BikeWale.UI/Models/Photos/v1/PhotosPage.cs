using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
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
        public bool IsMobile { get; set; }
        private uint _pageNo;
        public uint PageSize;


        /// <summary>
        /// Created by  :  Rajan Chauhan on 11 jan 2017
        /// Description :  To resolve depedencies for photo page
        /// </summary>
        public PhotosPage(bool isMobile, IBikeModels<BikeModelEntity, int> objModelEntity, IBikeMakesCacheRepository objMakeCache, uint? pageNo)
        {
            IsMobile = isMobile;
            _objModelEntity = objModelEntity;
            _objMakeCache = objMakeCache;
            _pageNo = (pageNo == null) ? 1 : (uint)pageNo;
        }

        /// <summary>
        /// Created by  :  Rajan Chauhan on 11 jan 2017
        /// Descritpion :  Added PageData for page
        /// </summary>
        /// <returns></returns>
        public PhotosPageVM GetData()
        {
            PhotosPageVM _objData = null;
            try
            {
                _objData = new PhotosPageVM();
                _objData.PopularSportsModelsImages = BindPopularSportsBikeWidget();
                BindBikeModelsPhotos(_objData);
                BindMakesWidget(_objData);
                SetBreadcrumList(ref _objData);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.v1.PhotosPage : GetData");
            }

            return _objData;
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

        private void BindBikeModelsPhotos(PhotosPageVM objData)
        {
            try
            {
                ImagePager pager = new ImagePager()
                {
                    PageNo = (int)_pageNo,
                    StartIndex = 1,
                    EndIndex = 30,
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
                objData.BikeModelsPhotos = _objModelEntity.GetBikeModelsPhotos(new ImagePager(), modelIds, categoryIds, requiredImageCount);
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

        private void SetPageMetas()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.Photos.v1.PhotosPage.SetPageMetas()");
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
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ModelImages> BindPopularSportsBikeWidget()
        {
            IEnumerable<ModelImages> modelsImages = null;
            try
            {
                IEnumerable<ModelIdWithBodyStyle> modelIdsWithBodyStyle = _objModelEntity.GetModelIdsForImages(0, EnumBikeBodyStyles.Sports, 1, 9);
                var modelIds = string.Join(",", modelIdsWithBodyStyle.Select(m => m.ModelId.ToString()));
                if (!string.IsNullOrEmpty(modelIds))
                {
                    modelsImages = _objModelEntity.GetBikeModelsPhotoGallery(modelIds, 7);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.v1.PhotosPage");
            }
            return modelsImages;
        }
    }
}