using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.EditorialWidgets;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Models.EditorialPages;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 1 Apr 2017
    /// Summary    : Model to fetch data for bike care details page
    /// Modified by : Rajan Chauhan on 17 Apr 2018
    /// Description : Removed IBikeModelsCacheRepository dependencyssss
    /// </summary>
    public class BikeCareDetailPage : EditorialBasePage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository = null;
        private readonly IBikeSeriesCacheRepository _seriesCache = null;
        private readonly IBikeSeries _series;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelMaskingCacheRepo = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
        private string _basicId;
        #endregion

        #region Page level variables
        private uint basicId;
        public StatusCodes status;
        private GlobalCityAreaEntity currentCityArea;
        private uint CityId, MakeId, ModelId;
        private bool isMakeLive;
        private bool IsModelTagged { get { return ModelId > 0; } }
        private bool isSeriesAvailable;
        private bool isScooterOnlyMake;
        private Entities.GenericBikes.EnumBikeBodyStyles bodyStyle = EnumBikeBodyStyles.AllBikes;
        private BikeSeriesEntityBase bikeSeriesEntityBase;

        public bool IsMobile { get; set; }
        #endregion

        #region Constructor
        public BikeCareDetailPage(ICMSCacheContent cmsCache, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IBikeModelsCacheRepository<int> models, string basicId, IBikeMakesCacheRepository bikeMakesCacheRepository, IBikeSeriesCacheRepository seriesCache, IBikeSeries series, IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCacheRepo, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache)
            : base(bikeMakesCacheRepository, models, bikeModels, upcoming, series)
        {
            _cmsCache = cmsCache;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
            _basicId = basicId;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            _series = series;
            _seriesCache = seriesCache;
            _modelMaskingCacheRepo = modelMaskingCacheRepo;
            _objBikeVersionsCache = objBikeVersionsCache;
            ProcessQueryString();
        }
        #endregion

        #region Functions

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Process query string
        /// </summary>
        private void ProcessQueryString()
        {
            if (!string.IsNullOrEmpty(_basicId) && uint.TryParse(_basicId, out basicId) && basicId > 0)
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;
                status = StatusCodes.ContentFound;
            }
            else
            {
                status = StatusCodes.ContentNotFound;
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Get bike care detail page data
        /// Modified by : Snehal Dange on 29th Nov 2017
        /// Descritpion : Added ga for page
        /// </summary>
        public BikeCareDetailPageVM GetData(int widgetTopCount)
        {
            BikeCareDetailPageVM objData = new BikeCareDetailPageVM();
            try
            {
                objData.ArticleDetails = _cmsCache.GetArticlesDetails(basicId);
                if (objData.ArticleDetails != null)
                {
                    status = StatusCodes.ContentFound;
                    GetTaggedBikeListByMake(objData);
                    GetTaggedBikeListByModel(objData);
                    SetPageMetas(objData);
                    GetWidgetData(objData);
                    PopulatePhotoGallery(objData);
                    objData.Page = Entities.Pages.GAPages.Editorial_Details_Page;
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeCareDetailPage.GetData");
            }
            return objData;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Set expert reviews details page metas
        /// </summary>
        private void SetPageMetas(BikeCareDetailPageVM objData)
        {
            try
            {
                objData.PageMetaTags.CanonicalUrl = string.Format("{0}/bike-care/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/bike-care/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                objData.PageMetaTags.Title = string.Format("{0} | Maintenance Tips from Bike Experts - BikeWale", objData.ArticleDetails.Title);
                objData.PageMetaTags.Keywords = "Bike maintenance, bike common issues, bike common problems, Maintaining bikes, bike care";
                objData.PageMetaTags.Description = string.Format("Read about {0}. Read through more bike care tips to learn more about your bike maintenance.", objData.ArticleDetails.Title);
                objData.Page_H1 = objData.ArticleDetails.Title;
                SetBreadcrumList(objData);
                //SetPageJSONSchema(objData);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeCareDetailPage.SetPageMetas");
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 25th Aug 2017
        /// Description : To load json schema for the bikecare articles
        /// </summary>
        /// <param name="objData"></param>
        private void SetPageJSONSchema(BikeCareDetailPageVM objData)
        {
            var objSchema = new NewsArticle();
            objSchema.HeadLine = objData.ArticleDetails.Title;
            objSchema.DateModified = objData.ArticleDetails.DisplayDate.ToString();
            objSchema.DatePublished = objSchema.DateModified;
            objSchema.Description = FormatDescription.SanitizeHtml(objData.ArticleDetails.Description);
            if (objData.ArticleDetails.PageList != null && objData.ArticleDetails.PageList.Any())
            {
                objSchema.ArticleBody = Bikewale.Utility.FormatDescription.SanitizeHtml(Convert.ToString(objData.ArticleDetails.PageList.First().Content));
            }

            objSchema.ArticleImage = new ImageObject()
            {
                ImageUrl = objData.PageMetaTags.ShareImage,
                Height = "348",
                Width = "640"
            };
            objSchema.Author = new Author()
            {
                Name = objData.ArticleDetails.AuthorName
            };
            objSchema.MainEntityOfPage = new MainEntityOfPage() { PageUrlId = objData.PageMetaTags.CanonicalUrl };

            objData.PageMetaTags.SchemaJSON = Newtonsoft.Json.JsonConvert.SerializeObject(objSchema);
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Get tagged make in article
        /// </summary>
        private void GetTaggedBikeListByMake(BikeCareDetailPageVM objData)
        {
            try
            {
                IEnumerable<Entities.CMS.Articles.VehicleTag> vehicleTags = objData.ArticleDetails.VehiclTagsList;
                if (vehicleTags != null && vehicleTags.Any(m => m.MakeBase.MakeId > 0))
                {
                    Entities.CMS.Articles.VehicleTag taggedMakeObj = vehicleTags.First(m => m.MakeBase.MakeId > 0);
                    objData.Make = _bikeMakesCacheRepository.GetMakeDetails((uint)taggedMakeObj.MakeBase.MakeId);
                    if (objData.Make != null)
                    {
                        MakeId = (uint)objData.Make.MakeId;
                        isMakeLive = objData.Make.IsNew;
                        isScooterOnlyMake = objData.Make.IsScooterOnly;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeCareDetailPage.GetTaggedBikeListByMake");
            }
        }


        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Get tagged model in article
        /// </summary>
        private void GetTaggedBikeListByModel(BikeCareDetailPageVM objData)
        {
            try
            {

                IEnumerable<Entities.CMS.Articles.VehicleTag> vehicleTags = objData.ArticleDetails.VehiclTagsList;
                if (vehicleTags != null && vehicleTags.Any(m => m.ModelBase != null && m.ModelBase.ModelId > 0))
                {
                    Entities.CMS.Articles.VehicleTag taggedMakeObj = vehicleTags.First(m => m.ModelBase != null && m.ModelBase.ModelId > 0);
                    ModelId = (uint)taggedMakeObj.ModelBase.ModelId;
                    objData.Model = _modelMaskingCacheRepo.GetById((int)ModelId);
                    CheckModelSeriesData();

                    IEnumerable<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(ModelId, false);

                    if (objVersionsList != null && objVersionsList.Any())
                        bodyStyle = objVersionsList.First().BodyStyle;
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeCareDetailPage.GetTaggedBikeListByModel");
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Get data for the page widgets
        /// Modified By Sajal Gupta on 25-04-20187
        /// Descrition : Call most popular bike widget by body type
        /// Modified by : Sanskar Gupta on 22 Jan 2018
        /// Description : Added Newly Launched feature
        /// </summary>
        private void GetWidgetData(BikeCareDetailPageVM objData)
        {
            try
            {
                SetAdditionalVariables(objData);
                objData.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Detail);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeCareDetailPage.GetWidgetData");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Populate view model for photo gallery
        /// </summary>
        private void PopulatePhotoGallery(BikeCareDetailPageVM objData)
        {
            try
            {
                objData.PhotoGallery = new EditCMSPhotoGalleryVM();
                objData.PhotoGallery.Images = _cmsCache.GetArticlePhotos(Convert.ToInt32(basicId));
                if (objData.PhotoGallery.Images != null && objData.PhotoGallery.Images.Any())
                {
                    objData.PhotoGallery.ImageCount = objData.PhotoGallery.Images.Count();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeCareDetailPage.PopulatePhotoGallery");
            }
        }

        /// <summary>
        /// Created By :Snehal Dange on 8th Nov 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(BikeCareDetailPageVM objVM)
        {
            try
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

                bikeUrl += "bike-care/";
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, bikeUrl, "Bike Care"));
                if (objVM.Model != null)
                {
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objVM.Page_H1));
                }

                objVM.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.BikeCareIndexPage.SetBreadcrumList");
            }

        }

        /// <summary>
        /// Created by  :   Sumit Kate on 25 Apr 2018
        /// Description :   Set basic flags to get the editorial widgets
        /// Modified By : Deepak Israni on 8 May 2018
        /// Description : Added flag for show on road price button and added GA related information.
        /// </summary>
        /// <param name="objData"></param>
        private void SetAdditionalVariables(BikeCareDetailPageVM objData)
        {
            EditorialWidgetEntity editorialWidgetData = new EditorialWidgetEntity
            {
                IsMobile = IsMobile,
                IsMakeLive = isMakeLive,
                IsModelTagged = IsModelTagged,
                IsSeriesAvailable = isSeriesAvailable,
                IsScooterOnlyMake = isScooterOnlyMake,
                BodyStyle = bodyStyle,
                CityId = CityId,
                Make = objData.Make,
                Series = bikeSeriesEntityBase,
                ShowOnRoadPriceButton = true,
                GAInfo = new EditorialGAEntity
                {
                    CategoryId = EditorialGACategories.Editorial_Details_Page,
                    PQSourceId = PQSourceEnum.Mobile_BikeCare_Details_Page
                }
            };
            base.SetAdditionalData(editorialWidgetData);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 25 Apr 2018
        /// Description :   Checks model series
        /// </summary>
        private void CheckModelSeriesData()
        {
            try
            {
                bikeSeriesEntityBase = _bikeModels.GetSeriesByModelId(ModelId);
                isSeriesAvailable = null != bikeSeriesEntityBase && bikeSeriesEntityBase.IsSeriesPageUrl;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeCareDetailPage.CheckSeriesData");
            }
        }
        #endregion
    }
}