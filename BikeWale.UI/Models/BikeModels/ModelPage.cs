using Bikewale.BAL.GrpcFiles.Specs_Features;
using Bikewale.BindViewModels.Controls;
using Bikewale.BindViewModels.Webforms;
using Bikewale.common;
using Bikewale.DTO.PriceQuote;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.Pages;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.AdSlot;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Interfaces.Videos;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Models.BestBikes;
using Bikewale.Models.BikeSeries;
using Bikewale.Models.PriceInCity;
using Bikewale.Models.Used;
using Bikewale.Models.UserReviews;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Bikewale.Models.BikeModels
{
    /// <summary>
    /// Modified By : Sangram Nandkhile on 07 Dec 2016.
    /// Description : Removed unncessary functions
    /// Modified by : Ashutosh Sharma on 30 Aug 2017
    /// Description : Removed GST related code (revert GST related changes)
    /// Modified by :Snehal Dange on 3rd Nov 2017
    /// Description: Added Mileage widget
    /// </summary>
    public class ModelPage
    {
        #region Global Variables

        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IPriceQuote _objPQ = null;
        private readonly IBikeModels<Entities.BikeData.BikeModelEntity, int> _objModel = null;
        private readonly IDealerPriceQuoteDetail _objDealerDetails = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objVersionCache = null;
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        private readonly ICMSCacheContent _objArticles = null;
        private readonly IVideos _objVideos = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedBikescache = null;
        private readonly IServiceCenter _objServiceCenter;
        private readonly IPriceQuoteCache _objPQCache;
        private readonly IUsedBikesCache _usedBikesCache;
        private readonly IUpcoming _upcoming = null;
        private readonly IUserReviewsCache _userReviewsCache = null;
        private readonly IUserReviewsSearch _userReviewsSearch = null;
        private readonly IBikeSeries _bikeSeries = null;
        private readonly IAdSlot _adSlot = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository;

        private uint _modelId, _cityId, _areaId;

        private readonly IManufacturerCampaign _objManufacturerCampaign = null;

        private ModelPageVM _objData = null;

        private PQOnRoadPrice _pqOnRoad;
        private readonly StringBuilder _colorStr = new StringBuilder();

        public string RedirectUrl { get; set; }
        public StatusCodes Status { get; set; }
        public uint OtherDealersTopCount { get; set; }
        public PQSources Source { get; set; }
        public PQSourceEnum PQSource { get; set; }
        public LeadSourceEnum LeadSource { get; set; }
        public bool IsMobile { get; set; }
        public bool IsDealerPriceAvailble { get; set; }
        public ManufacturerCampaignServingPages ManufacturerCampaignPageId { get; set; }
        public string CurrentPageUrl { get; set; }

        /// <summary>
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added IAdSlot.
        /// </summary>
        public ModelPage(string makeMasking, string modelMasking, IUserReviewsSearch userReviewsSearch, IUserReviewsCache userReviewsCache, IBikeModels<Entities.BikeData.BikeModelEntity, int> objModel, IDealerPriceQuote objDealerPQ, IAreaCacheRepository objAreaCache, ICityCacheRepository objCityCache, IPriceQuote objPQ, IDealerCacheRepository objDealerCache, IDealerPriceQuoteDetail objDealerDetails, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache, ICMSCacheContent objArticles, IVideos objVideos, IUsedBikeDetailsCacheRepository objUsedBikescache, IServiceCenter objServiceCenter, IPriceQuoteCache objPQCache, IUsedBikesCache usedBikesCache, IBikeModelsCacheRepository<int> objBestBikes, IUpcoming upcoming, IManufacturerCampaign objManufacturerCampaign, IBikeSeries bikeSeries, IAdSlot adSlot, IBikeInfo bikeInfo, IBikeMakesCacheRepository bikeMakesCacheRepository)
        {
            _objModel = objModel;
            _objDealerPQ = objDealerPQ;
            _objAreaCache = objAreaCache;
            _objCityCache = objCityCache;
            _objPQ = objPQ;
            _objDealerCache = objDealerCache;
            _objDealerDetails = objDealerDetails;
            _objVersionCache = objVersionCache;
            _objBestBikes = objBestBikes;
            _upcoming = upcoming;
            _objArticles = objArticles;
            _objVideos = objVideos;
            _objUsedBikescache = objUsedBikescache;
            _objServiceCenter = objServiceCenter;
            _objPQCache = objPQCache;
            _usedBikesCache = usedBikesCache;
            _objManufacturerCampaign = objManufacturerCampaign;
            _userReviewsSearch = userReviewsSearch;
            _userReviewsCache = userReviewsCache;
            _bikeSeries = bikeSeries;
            _adSlot = adSlot;
            _bikeInfo = bikeInfo;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            ParseQueryString(makeMasking, modelMasking);
        }

        #endregion Global Variables

        #region Methods
        /// <summary>
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added call to BindAdSlotTags.
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public ModelPageVM GetData(uint? versionId)
        {
            try
            {
                _objData = new ModelPageVM();

                if (_modelId > 0)
                {
                    _objData.ModelId = _modelId;

                    #region Do Not change the sequence

                    CheckCityCookie();
                    _objData.CityId = _cityId;
                    _objData.AreaId = _areaId;
                    _objData.VersionId = versionId.HasValue ? versionId.Value : 0;

                    _objData.ModelPageEntity = FetchModelPageDetails(_modelId);
                    _objData.BikeSpecsFeatures = new BikeSpecsFeaturesVM()
                    {
                        BikeName = _objData.BikeName,
                        ModelName = _objData.ModelPageEntity.ModelDetails.ModelName,
                        VersionSpecsFeatures = SpecsFeaturesServiceGateway.Call()
                    };
                    if (_objData.IsModelDetails && _objData.ModelPageEntity.ModelDetails.New)
                    {
                        FetchOnRoadPrice(_objData.ModelPageEntity);
                    }

                    LoadVariants(_objData.ModelPageEntity);
                    if (_objData.IsModelDetails && _objData.ModelPageEntity.ModelDetails.New)
                    {
                        GetManufacturerCampaign();
                    }
                    BindControls();

                    BindColorString();
                    if (_modelId > 0)
                    {
                        BindMileageWidget(_objData);
                    }


                    CreateMetas();

                    ImageAccordingToVersion();

                    BindVersionPriceListSummary();

                    if (_objData.SimilarBikes != null)
                    {
                        _objData.SimilarBikes.BodyStyle = _objData.BodyStyle;
                    }
                    _objData.Page = GAPages.Model_Page;

                    SetBreadcrumList();
                    SetPageJSONLDSchema();
                    ShowInnovationBanner(_modelId);
                    BindAdSlotTags();
                    if (_objData != null && _objData.ModelPageEntity != null && _objData.ModelPageEntity.ModelVersionSpecs != null)
                    {
                        _objData.IsElectricBike = _objData.ModelPageEntity.ModelVersionSpecs.FuelType.Equals("Electric");
                    }
                    #endregion Do Not change the sequence
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetData({0})", _modelId));
            }

            return _objData;
        }

        private void ImageAccordingToVersion()
        {
            try
            {
                if (_objData.ModelPageEntity != null && _objData.ModelPageEntity.ModelVersions != null && _objData.ModelPageEntity.ModelVersions.Any() && _objData.ModelPageEntity.AllPhotos != null && _objData.ModelPageEntity.AllPhotos.Any())
                {
                    string OriginalImagePath = _objData.ModelPageEntity.ModelVersions.FirstOrDefault(m => m.VersionId == _objData.VersionId).OriginalImagePath;

                    if (!String.IsNullOrEmpty(OriginalImagePath))
                    {

                        _objData.ModelPageEntity.AllPhotos.ElementAt(0).HostUrl = _objData.ModelPageEntity.ModelVersions.FirstOrDefault(m => m.VersionId == _objData.VersionId).HostUrl;
                        _objData.ModelPageEntity.AllPhotos.ElementAt(0).OriginalImgPath = OriginalImagePath;
                    }

                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "ImageAccordingToVersion()");
            }
        }


        /// <summary>
        /// Created by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Bind ad slot to adtags.
        /// </summary>
        private void BindAdSlotTags()
        {
            try
            {
                if (_objData.AdTags != null)
                {
                    _objData.AdTags.Ad_292x399 = _adSlot.CheckAdSlotStatus("Ad_292x399"); //For similar bikes widget desktop
                    _objData.AdTags.Ad_200x253 = _adSlot.CheckAdSlotStatus("Ad_200x253");  //For similar bikes widget mobile
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "ModelPage.BindAdSlotTags");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Sep 2017
        /// Description :   To Show Innovation Banner
        /// </summary>
        /// <param name="_modelId"></param>
        private void ShowInnovationBanner(uint _modelId)
        {
            try
            {
                if (!String.IsNullOrEmpty
                    (BWConfiguration.Instance.InnovationBannerModels))
                {
                    _objData.AdTags.ShowInnovationBannerDesktop = _objData.AdTags.ShowInnovationBannerMobile = BWConfiguration.Instance.InnovationBannerModels.Split(',').Contains(_modelId.ToString());
                    _objData.AdTags.InnovationBannerGALabel = _objData.BikeName.Replace(" ", "_");
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, String.Format("ShowInnovationBanner({0})", _modelId));
            }
        }

        /// <summary>
        /// Created By  : Sangram Nandkhile on 31st Aug 2017
        /// Description : To load json schema for the list items
        /// Modified By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema along with product
        /// Modified By  : Sushil Kumar on 17th Sep 2017
        /// Description : Added logic to show product schema in webpage schema for some of the models
        /// Modified By: Snehal Dange on 22nd Sep 2017
        /// Descrption  :Added logic to show similar bikes schema 
        /// Modified By  : Sushil Kumar on 30th Jan 2018
        /// Description : Show user review count for product if available else ratings count
        /// </summary>
        private void SetPageJSONLDSchema()
        {
            try
            {

                //set webpage schema for the model page
                WebPage webpage = SchemaHelper.GetWebpageSchema(_objData.PageMetaTags, _objData.BreadcrumbList);

                if (webpage != null)
                {
                    Product product = new Product();
                    product.Description = _objData.ModelPageEntity.ModelDesc.SmallDescription;
                    product.Name = _objData.BikeName;
                    product.Image = Bikewale.Utility.Image.GetPathToShowImages(_objData.ModelPageEntity.ModelDetails.OriginalImagePath, _objData.ModelPageEntity.ModelDetails.HostUrl, Bikewale.Utility.ImageSize._210x118);
                    product.Model = _objData.ModelPageEntity.ModelDetails.ModelName;

                    product.Brand = new Brand
                    {
                        Name = _objData.ModelPageEntity.ModelDetails.MakeBase.MakeName,
                        Url = string.Format("{0}/{1}-bikes/", BWConfiguration.Instance.BwHostUrl, _objData.ModelPageEntity.ModelDetails.MakeBase.MaskingName)
                    };

                    if (!_objData.IsUpcomingBike && _objData.ModelPageEntity.ModelDetails.RatingCount > 0)
                    {
                        product.AggregateRating = new AggregateRating
                        {
                            RatingValue = Convert.ToDouble(_objData.ModelPageEntity.ModelDetails.ReviewUIRating),
                            WorstRating = 1,
                            BestRating = 5,
                            ItemReviewed = product.Name
                        };
                        if (_objData.ModelPageEntity.ModelDetails.ReviewCount > 0)
                        {
                            product.AggregateRating.ReviewCount = (uint)_objData.ModelPageEntity.ModelDetails.ReviewCount;
                        }
                        else
                        {
                            product.AggregateRating.RatingCount = (uint)_objData.ModelPageEntity.ModelDetails.RatingCount;
                        }
                    }
                    if (_objData.IsUpcomingBike)
                    {
                        product.AggregateOffer = new AggregateOffer
                        {
                            LowPrice = (uint)_objData.ModelPageEntity.UpcomingBike.EstimatedPriceMin,
                            HighPrice = (uint)_objData.ModelPageEntity.UpcomingBike.EstimatedPriceMax
                        };
                    }
                    else
                    {
                        product.AggregateOffer = new AggregateOffer
                        {
                            LowPrice = (uint)_objData.ModelPageEntity.ModelDetails.MinPrice,
                            HighPrice = (uint)_objData.ModelPageEntity.ModelDetails.MaxPrice
                        };

                        if (_objData.IsDiscontinuedBike)
                        {
                            product.AggregateOffer.Availability = OfferAvailability._Discontinued;
                        }

                    }
                    if (_objData.ModelPageEntity.ModelColors != null && _objData.ModelPageEntity.ModelColors.Any())
                    {
                        product.Color = _objData.ModelPageEntity.ModelColors.Select(x => x.ColorName);
                    }

                    SetAdditionalProperties(product);
                    SetSimilarBikesProperties(product);
                    _objData.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
                    _objData.PageMetaTags.PageSchemaJSON = SchemaHelper.JsonSerialize(product);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.ModelPage.SetPageJSONLDSchema => BikeName: {0}", _objData.BikeName));
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// Modified by : Snehal Dange on 27th Dec 2017
        /// Description: Added 'new bikes' in breadcrumb
        /// </summary>
        private void SetBreadcrumList()
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string bikeUrl, scooterUrl;
            bikeUrl = scooterUrl = string.Format("{0}/", BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                bikeUrl += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}new-bikes-in-india/", bikeUrl), "New Bikes"));

            if (_objData.ModelPageEntity.ModelDetails.MakeBase != null)
            {
                if (_objData.IsModelDetails)
                {
                    bikeUrl = string.Format("{0}{1}-bikes/", bikeUrl, _objData.ModelPageEntity.ModelDetails.MakeBase.MaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} Bikes", _objData.ModelPageEntity.ModelDetails.MakeBase.MakeName)));
                }
                if (_objData.BodyStyle.Equals(EnumBikeBodyStyles.Scooter) && !(_objData.ModelPageEntity.ModelDetails.MakeBase.IsScooterOnly)
                    && _objData.IsModelDetails && _objData.ModelPageEntity.ModelDetails.MakeBase != null)
                {
                    if (IsMobile)
                    {
                        scooterUrl += "m/";
                    }
                    scooterUrl = string.Format("{0}{1}-scooters/", scooterUrl, _objData.ModelPageEntity.ModelDetails.MakeBase.MaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, scooterUrl, string.Format("{0} Scooters ", _objData.ModelPageEntity.ModelDetails.MakeBase.MakeName)));
                }
                if (_objData.ModelsBySeries != null && _objData.ModelsBySeries.SeriesBase != null && _objData.ModelsBySeries.SeriesBase.IsSeriesPageUrl && !string.IsNullOrEmpty(_objData.ModelsBySeries.SeriesBase.MaskingName))
                {
                    bikeUrl = string.Format("{0}{1}/", bikeUrl, _objData.ModelsBySeries.SeriesBase.MaskingName);
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, _objData.ModelsBySeries.SeriesBase.SeriesName));
                }
            }
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, _objData.Page_H1));

            _objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }

        /// <summary>
        /// Sets the additional properties for JSONLD
        /// </summary>
        /// <param name="product">The product.</param>
        private void SetAdditionalProperties(Product product)
        {
            try
            {
                List<AdditionalProperty> listSpecs = new List<AdditionalProperty>();
                AdditionalProperty property = null;

                if (_objData != null && _objData.ModelPageEntity != null && _objData.ModelPageEntity.ModelVersionSpecs != null)
                {
                    if (_objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall > 0)
                    {
                        property = new AdditionalProperty
                        {
                            Name = "Mileage",
                            Value = Convert.ToString(_objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall),
                            UnitText = "KMPL"

                        };
                        listSpecs.Add(property);
                    }

                    if (_objData.ModelPageEntity.ModelVersionSpecs.Displacement > 0)
                    {
                        property = new AdditionalProperty
                        {
                            Name = "Displacement",
                            Value = Convert.ToString(_objData.ModelPageEntity.ModelVersionSpecs.Displacement),
                            UnitText = "CC"

                        };
                        listSpecs.Add(property);
                    }

                    if (_objData.ModelPageEntity.ModelVersionSpecs.MaxPower > 0)
                    {
                        property = new AdditionalProperty
                        {
                            Name = "Max Power",
                            MaxValue = Convert.ToString(_objData.ModelPageEntity.ModelVersionSpecs.MaxPower),
                            UnitText = "BHP"

                        };
                        listSpecs.Add(property);

                    }
                    if (_objData.ModelPageEntity.ModelVersionSpecs.KerbWeight > 0)
                    {
                        property = new AdditionalProperty
                        {
                            Name = "Weight",
                            Value = Convert.ToString(_objData.ModelPageEntity.ModelVersionSpecs.KerbWeight),
                            UnitText = "KG"

                        };
                    }
                    listSpecs.Add(property);

                    if (_objData.ModelPageEntity.ModelVersionSpecs.TopSpeed > 0)
                    {
                        property = new AdditionalProperty
                        {
                            Name = "Top speed",
                            MaxValue = Convert.ToString(_objData.ModelPageEntity.ModelVersionSpecs.TopSpeed),
                            UnitText = "KMPH"

                        };
                        listSpecs.Add(property);
                    }
                    product.AdditionalProperty = listSpecs;
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.BikeModels.ModelPage --> SetAdditionalProperties(), Model: {0}", _modelId));
            }

        }

        private void BindVersionPriceListSummary()
        {
            try
            {
                if (_objData.IsModelDetails)
                {
                    string cityList = string.Empty;
                    string priceDescription = string.Empty;
                    string versionDescirption = _objData.ModelPageEntity.ModelVersions.Count > 1 ? string.Format(" It is available in {0} versions", _objData.ModelPageEntity.ModelVersions.Count) : string.Format(" It is available in {0} version", _objData.ModelPageEntity.ModelVersions.Count);
                    ushort index = 0;
                    if (_objData.ModelPageEntity.ModelVersions != null)
                    {
                        if (_objData.ModelPageEntity.ModelVersions.Count > 1)
                        {
                            versionDescirption += " - ";
                            foreach (var version in _objData.ModelPageEntity.ModelVersions)
                            {
                                index++;
                                if (_objData.ModelPageEntity.ModelVersions.Count <= index)
                                    break;
                                else if (index > 1)
                                    versionDescirption = string.Format("{0},", versionDescirption);
                                versionDescirption = string.Format("{0} {1}", versionDescirption, version.VersionName);
                            }
                            versionDescirption = string.Format("{0} and {1}", versionDescirption, _objData.ModelPageEntity.ModelVersions.Last().VersionName);
                        }
                        else if (_objData.ModelPageEntity.ModelVersions.Count == 1)
                            versionDescirption += string.Format(" - {0}", _objData.ModelPageEntity.ModelVersions.First().VersionName);
                    }

                    if (_objData.BikePrice > 0 && _objData.IsLocationSelected && _objData.City != null && !_objData.ShowOnRoadButton)
                        priceDescription = string.Format("Price - &#x20B9; {0} onwards (On-road, {1}).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(_objData.BikePrice)), _objData.City.CityName);
                    else if (_objData.SelectedVersion != null && _objData.SelectedVersion.AverageExShowroom > 0 && _objData.LocationCookie != null && _objData.LocationCookie.CityId > 0)
                        priceDescription = string.Format("Price - &#x20B9; {0} onwards (Avg. Ex-showroom price).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(_objData.SelectedVersion.AverageExShowroom)));
                    else
                        priceDescription = _objData.ModelPageEntity.ModelDetails.MinPrice > 0 ? string.Format("Price - &#x20B9; {0} onwards (Ex-showroom, {1}).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(_objData.ModelPageEntity.ModelDetails.MinPrice)), Bikewale.Utility.BWConfiguration.Instance.DefaultName) : string.Empty;

                    index = 0;
                    if (_objData.PriceInTopCities != null && _objData.PriceInTopCities.PriceQuoteList != null && _objData.PriceInTopCities.PriceQuoteList.Count() > 1)
                    {
                        cityList = _objData.PriceInTopCities.PriceQuoteList != null && _objData.PriceInTopCities.PriceQuoteList.Any() ? string.Format(". See price of {0} in top {1}:", _objData.ModelPageEntity.ModelDetails.ModelName, _objData.PriceInTopCities.PriceQuoteList.Count() > 1 ? "cities" : "city") : string.Empty;
                        foreach (var city in _objData.PriceInTopCities.PriceQuoteList)
                        {
                            index++;
                            cityList = string.Format("{0} {1}", cityList, city.CityName);
                            if (index >= 3 || _objData.PriceInTopCities.PriceQuoteList.Count() <= index)
                                break;
                            else if (index >= 1)
                                cityList = string.Format("{0},", cityList);
                        }
                        cityList = string.Format("{0} and {1}", cityList, _objData.PriceInTopCities.PriceQuoteList.Last().CityName);
                    }

                    if (_objData.PriceInTopCities != null && _objData.PriceInTopCities.PriceQuoteList != null && _objData.PriceInTopCities.PriceQuoteList.Count() == 1)
                    {
                        cityList = string.Format("{0}", _objData.PriceInTopCities.PriceQuoteList.First().CityName);
                    }

                    _objData.VersionPriceListSummary = string.Format("{0} {1}{2}{3}.", _objData.BikeName, priceDescription, versionDescirption, cityList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeModels.ModelPage --> BindVersionPriceListSummary()");
            }
        }

        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- To bind Description on model page
        /// Modified by :   Sumit Kate on 20 Jan 2017
        /// Description :   Model Page SEO changes
        /// </summary>
        private void BindDescription()
        {
            try
            {
                if (_objData.IsModelDetails)
                {
                    string bikeModelName = _objData.ModelPageEntity.ModelDetails.ModelName, specsDescirption = string.Empty;

                    string versionDescirption = _objData.ModelPageEntity.ModelVersions.Count > 1 ? string.Format(" It is available in {0} versions", _objData.ModelPageEntity.ModelVersions.Count) : string.Format(" It is available in {0} version", _objData.ModelPageEntity.ModelVersions.Count);

                    string priceDescription = string.Empty;
                    if (_objData.BikePrice > 0 && _objData.IsLocationSelected && _objData.City != null && !_objData.ShowOnRoadButton)
                        priceDescription = string.Format("Price - &#x20B9; {0} onwards (On-road, {1}).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(_objData.BikePrice)), _objData.City.CityName);
                    else if (_objData.SelectedVersion != null && _objData.SelectedVersion.AverageExShowroom > 0 && _objData.LocationCookie != null && _objData.LocationCookie.CityId > 0)
                        priceDescription = string.Format("Price - &#x20B9; {0} onwards (Avg. Ex-showroom price).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(_objData.SelectedVersion.AverageExShowroom)));
                    else
                        priceDescription = _objData.ModelPageEntity.ModelDetails.MinPrice > 0 ? string.Format("Price - &#x20B9; {0} onwards (Ex-showroom, {1}).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(_objData.ModelPageEntity.ModelDetails.MinPrice)), Bikewale.Utility.BWConfiguration.Instance.DefaultName) : string.Empty;
                    if (_objData.ModelPageEntity != null && _objData.ModelPageEntity.ModelVersionSpecs != null && (_objData.ModelPageEntity.ModelVersionSpecs.TopSpeed > 0 || _objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall > 0))
                    {
                        if ((_objData.ModelPageEntity.ModelVersionSpecs.TopSpeed > 0 && _objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall > 0))
                            specsDescirption = string.Format("{0} has a mileage of {1} kmpl and a top speed of {2} kmph.", bikeModelName, _objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall, _objData.ModelPageEntity.ModelVersionSpecs.TopSpeed);
                        else if (_objData.ModelPageEntity.ModelVersionSpecs.TopSpeed == 0)
                        {
                            specsDescirption = string.Format("{0} has a mileage of {1} kmpl.", bikeModelName, _objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall);
                        }
                        else
                        {
                            specsDescirption = string.Format("{0} has a top speed of {1} kmph.", bikeModelName, _objData.ModelPageEntity.ModelVersionSpecs.TopSpeed);
                        }
                    }
                    _objData.ModelSummary = string.Format("{0} {1}{2}. {3} {4}", _objData.BikeName, priceDescription, versionDescirption, specsDescirption, _colorStr);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeModels.ModelPage --> BindDescription()");
            }
        }

        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- values to controls field
        /// Modified by :  Subodh Jain on 21 Dec 2016
        /// Description :  Added dealer card and service center card
        /// Modified by :   Sumit Kate on 02 Jan 2017
        /// Description :   Set makename,modelname,make and model masking name to news widget
        /// Modified by: Vivek Singh Tomar on 23 Aug 2017
        /// Summary: Added page enum to similar bike widget
        /// Modified by : Vivek Singh Tomar on 28th Sep 2017
        /// Summary : Added BindModelsBySeries
        /// Modified by : Ashutosh Sharma on 29 Sep 2017 
        /// Description : Get emi details for avg ex-showroom price when bike price is zero.
        /// Modified by : Vivek Singh Tomar on 12th Oct 2017 
        /// Summary : Removed initialisation of service centers
        /// Modified by : Ashutosh Sharma on 03 Nov 2017
        /// Description : Changed order of bind upcoming bikes wiget to get bodystyle.
        /// Modified by : Ashutosh Sharma on 07 Feb 2018
        /// Description : Fetching upcoming bikes for new and discontined bikes also.
        /// Modified by : Rajan Chauhan on 08 Feb 2018
        /// Description : PriceInTopCities now have 9 cities instead of 8 for modelPage Ad_Model_BTF_300x250 logic
        /// </summary>
        private void BindControls()
        {
            try
            {
                if (_objData != null && _objData.IsModelDetails)
                {
                    var objMake = _objData.ModelPageEntity.ModelDetails.MakeBase;

                    _objData.News = new RecentNews(3, (uint)objMake.MakeId, _objData.ModelId, objMake.MakeName, objMake.MaskingName, _objData.ModelPageEntity.ModelDetails.ModelName, _objData.ModelPageEntity.ModelDetails.MaskingName, "News", _objArticles).GetData();

                    BindExpertReviews();
                    BindComparisionReviews();

                    _objData.Videos = new RecentVideos(1, 3, (uint)objMake.MakeId, objMake.MakeName, objMake.MaskingName, _objData.ModelId, _objData.ModelPageEntity.ModelDetails.ModelName, _objData.ModelPageEntity.ModelDetails.MaskingName, _objVideos).GetData();
                    _objData.ReturnUrl = Utils.Utils.EncryptTripleDES(string.Format("returnUrl=/{0}-bikes/{1}/&sourceid={2}", objMake.MaskingName, _objData.ModelPageEntity.ModelDetails.MaskingName, (int)(IsMobile ? UserReviewPageSourceEnum.Mobile_ModelPage : UserReviewPageSourceEnum.Desktop_ModelPage)));

                    if (!_objData.IsUpcomingBike)
                    {
                        DealerCardWidget objDealer = new DealerCardWidget(_objDealerCache, _cityId, (uint)objMake.MakeId);
                        objDealer.TopCount = OtherDealersTopCount;
                        _objData.OtherDealers = objDealer.GetData();

                        if (_cityId > 0)
                        {
                            var dealerData = new DealerCardWidget(_objDealerCache, _cityId, (uint)objMake.MakeId);
                            dealerData.TopCount = 3;
                            _objData.OtherDealers = dealerData.GetData();
                        }
                        else
                        {
                            _objData.DealersServiceCenter = new DealersServiceCentersIndiaWidgetModel((uint)objMake.MakeId, objMake.MakeName, objMake.MaskingName, _objDealerCache).GetData();
                        }

                        _objData.UsedModels = BindUsedBikeByModel((uint)objMake.MakeId, _cityId);

                        if (_cityId > 0)
                            _objData.PriceInTopCities = new ModelPriceInNearestCities(_objPQCache, _modelId, _cityId, 9).GetData();
                        else
                            _objData.PriceInTopCities = new PriceInTopCities(_objPQCache, _modelId, 9).GetData();

                        if ((_objData.PriceInTopCities != null && _objData.PriceInTopCities.PriceQuoteList != null && _objData.PriceInTopCities.PriceQuoteList.Any()) || (_objData.ModelPageEntity.ModelVersions != null && _objData.ModelPageEntity.ModelVersions.Count > 0))
                        {
                            _objData.IsShowPriceTab = true;
                        }

                        GetBikeRankingCategory();

                        BindUserReviewsWidget(_objData);

                        if (_objData.BikeRanking != null)
                        {
                            BindBestBikeWidget(_objData.BikeRanking.BodyStyle, _cityId);
                        }

                        if (_objData.IsNewBike)
                        {
                            _objData.LeadCapture = new LeadCaptureEntity()
                            {
                                ModelId = _modelId,
                                CityId = _cityId,
                                AreaId = _areaId,
                                Area = _objData.LocationCookie.Area,
                                City = _objData.LocationCookie.City,
                                Location = _objData.Location,
                                BikeName = _objData.BikeName,
                                IsManufacturerCampaign = _objData.IsManufacturerLeadAdShown || _objData.IsManufacturerEMIAdShown || _objData.IsManufacturerTopLeadAdShown
                            };


                            // When dealer Price isn't avalablle, call function to get on-road pricing
                            uint onRoadPrice = _objData.BikePrice;
                            if (!IsDealerPriceAvailble || onRoadPrice == 0)
                            {
                                bool hasAreaAvailable = false;
                                uint _emiCityId = (this._cityId == 0 || onRoadPrice == 0) ? 1 : this._cityId;
                                _objData.BikeVersionPrices = _objPQ.GetVersionPricesByModelId(this._modelId, _emiCityId, out hasAreaAvailable);
                                if (_objData.BikeVersionPrices != null)
                                {
                                    var selectedversion = _objData.BikeVersionPrices.SingleOrDefault(x => x.VersionId == _objData.VersionId);
                                    if (selectedversion != null)
                                    {
                                        onRoadPrice = (uint)selectedversion.OnRoadPrice;
                                    }
                                }
                            }

                            if (onRoadPrice > 0)
                            {
                                if (_objData.DetailedDealer != null && _objData.DetailedDealer.PrimaryDealer != null)
                                {
                                    SetDealerEMIDetails(onRoadPrice);
                                }
                                else
                                {
                                    _objData.EMIDetails = SetDefaultEMIDetails(onRoadPrice);
                                }

                                BindEMICalculator(onRoadPrice);
                            }

                        }
                    }

                    // Set body style
                    if (_objData.VersionId > 0 && _objData.ModelPageEntity.ModelVersions != null && _objData.ModelPageEntity.ModelVersions.Count > 0)
                    {
                        var selected = _objData.ModelPageEntity.ModelVersions.FirstOrDefault(x => x.VersionId == _objData.VersionId);
                        if (selected != null)
                        {
                            _objData.BodyStyle = selected.BodyStyle;
                            _objData.BodyStyleText = _objData.BodyStyle == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes";
                            _objData.BodyStyleTextSingular = _objData.BodyStyle == EnumBikeBodyStyles.Scooter ? "scooter" : "bike";
                        }
                    }
                    _objData.objUpcomingBikes = BindUpCompingBikesWidget();
                    BindSimilarBikes(_objData);
                    BindModelsBySeriesId(_objData);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ModelPage.BindControls");
            }
        }

        private void BindExpertReviews()
        {
            try
            {
                var objMake = _objData.ModelPageEntity.ModelDetails.MakeBase;

                if (objMake != null)
                {
                    RecentExpertReviews objExpertReviews = new RecentExpertReviews(5, (uint)objMake.MakeId, _objData.ModelId, objMake.MakeName, objMake.MaskingName, _objData.ModelPageEntity.ModelDetails.ModelName, _objData.ModelPageEntity.ModelDetails.MaskingName, _objArticles, string.Format("{0} Reviews", _objData.BikeName));

                    List<EnumCMSContentType> categoryList = new List<EnumCMSContentType>
					{
						EnumCMSContentType.RoadTest
					};
                    List<EnumCMSContentSubCategoryType> subCategoryList = new List<EnumCMSContentSubCategoryType>
					{
						EnumCMSContentSubCategoryType.Road_Test,
						EnumCMSContentSubCategoryType.First_Drive,
						EnumCMSContentSubCategoryType.Long_Term_Report
					};
                    _objData.ExpertReviews = objExpertReviews.GetData(categoryList, subCategoryList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ModelPage.BindExpertReviews");
            }
        }

        private void BindComparisionReviews()
        {
            try
            {
                var objMake = _objData.ModelPageEntity.ModelDetails.MakeBase;

                if (objMake != null)
                {
                    RecentExpertReviews objExpertReviews = new RecentExpertReviews(3, (uint)objMake.MakeId, _objData.ModelId, objMake.MakeName, objMake.MaskingName, _objData.ModelPageEntity.ModelDetails.ModelName, _objData.ModelPageEntity.ModelDetails.MaskingName, _objArticles, string.Format("{0} Reviews", _objData.BikeName));
                    _objData.ComparisionTestExpertReviews = objExpertReviews.GetComparisonTests();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ModelPage.BindComparisionReviews");
            }
        }

        private void BindSimilarBikes(ModelPageVM objData)
        {
            try
            {
                if (_modelId > 0)
                {
                    var objSimilarBikes = new SimilarBikesWidget(_objVersionCache, _objData.VersionId, PQSourceEnum.Desktop_DPQ_Alternative);

                    objSimilarBikes.TopCount = 9;
                    objSimilarBikes.CityId = _cityId;
                    objSimilarBikes.IsNew = _objData.IsNewBike;
                    objSimilarBikes.IsUpcoming = _objData.IsUpcomingBike;
                    objSimilarBikes.IsDiscontinued = _objData.IsDiscontinuedBike;
                    _objData.SimilarBikes = objSimilarBikes.GetData();
                    if (_objData.IsSimilarBikesAvailable)
                    {
                        _objData.SimilarBikes.Make = objData.ModelPageEntity.ModelDetails.MakeBase;
                        _objData.SimilarBikes.Model = _objData.ModelPageEntity.ModelDetails;
                        _objData.SimilarBikes.VersionId = _objData.VersionId;
                        _objData.SimilarBikes.Page = GAPages.Model_Page;
                    }
                    else
                    {
                        if (_objData.IsNewBike || _objData.IsUpcomingBike)
                            BindPopularBodyStyle(_objData);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Models.ModelPage.BindSimilarBikes({0})", _modelId));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Aug 2017
        /// Description :   Bind Popular by BodyStyle
        /// </summary>
        /// <param name="objData"></param>
        private void BindPopularBodyStyle(ModelPageVM objData)
        {
            try
            {
                if (_modelId > 0)
                {
                    var modelPopularBikesByBodyStyle = new PopularBikesByBodyStyle(_objBestBikes);
                    modelPopularBikesByBodyStyle.CityId = _cityId;
                    modelPopularBikesByBodyStyle.ModelId = _modelId;
                    modelPopularBikesByBodyStyle.TopCount = 9;

                    objData.PopularBodyStyle = modelPopularBikesByBodyStyle.GetData();
                    objData.PopularBodyStyle.PQSourceId = PQSource;
                    objData.PopularBodyStyle.ShowCheckOnRoadCTA = true;
                    objData.BodyStyle = objData.PopularBodyStyle.BodyStyle;
                    objData.BodyStyleName = BindPopularBikesStyle(objData.BodyStyle);
                    objData.BodyStyleText = BindPopularBikesStyle(objData.BodyStyle);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Models.ModelPage.BindPopularBodyStyle({0})", _modelId));
            }
        }

        private string BindPopularBikesStyle(EnumBikeBodyStyles bodyStyle)
        {
            string strBodyStyle = String.Empty;
            switch (bodyStyle)
            {

                case EnumBikeBodyStyles.Mileage:
                    strBodyStyle = "Mileage Bikes";
                    break;
                case EnumBikeBodyStyles.Sports:
                    strBodyStyle = "Sports Bikes";
                    break;
                case EnumBikeBodyStyles.Cruiser:
                    strBodyStyle = "Cruisers";
                    break;
                case EnumBikeBodyStyles.Scooter:
                    strBodyStyle = "Scooters";
                    break;
                case EnumBikeBodyStyles.AllBikes:
                default:
                    strBodyStyle = "Bikes";
                    break;
            }
            return strBodyStyle;
        }

        /// <summary>
        /// created by :- Subodh Jain on 17 july 2017
        /// Summary added BindUserReviewSWidget
        /// </summary>
        /// <param name="makeMasking"></param>
        /// <param name="modelMasking"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public void BindUserReviewsWidget(ModelPageVM objPage)
        {
            try
            {
                ReviewDataCombinedFilter objFilter = new ReviewDataCombinedFilter()
                {
                    InputFilter = new Entities.UserReviews.Search.InputFilters()
                    {
                        Model = _modelId.ToString(),
                        SO = 1,
                        PN = 1,
                        PS = 3,
                        Reviews = true
                    },
                    ReviewFilter = new ReviewFilter()
                    {
                        RatingQuestion = !IsMobile,
                        ReviewQuestion = false,
                        SantizeHtml = true,
                        SanitizedReviewLength = (uint)(IsMobile ? 150 : 270),
                        BasicDetails = true
                    }
                };

                var objUserReviews = new UserReviewsSearchWidget(_modelId, objFilter, _userReviewsCache, _userReviewsSearch);
                objUserReviews.ActiveReviewCateory = FilterBy.MostRecent;
                objPage.UserReviews = objUserReviews.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Model.BindUserReviewSWidget({0})", _modelId));
            }
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Binding data for upcoming bike widget
        /// Modified by : Ashutosh Sharma on 03 Nov 2017
        /// Description : Removed filters and and added conditions for bodystyle and deviated price.
        /// Modified by : Ashutosh Sharma on 07 Feb 2018
        /// Description : Fetching upcoming bikes for new and discontined bikes also, at front of bike list will be having same bodystyle as requested bike model.
        /// </summary>
        /// <returns></returns>
        private UpcomingBikesWidgetVM BindUpCompingBikesWidget()
        {
            UpcomingBikesWidgetVM objUpcomingBikes = null;
            try
            {
                UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_upcoming);
                ulong avgExpectedPrice = _objData.ModelPageEntity != null && _objData.ModelPageEntity.UpcomingBike != null ? (_objData.ModelPageEntity.UpcomingBike.EstimatedPriceMin + _objData.ModelPageEntity.UpcomingBike.EstimatedPriceMax) / 2 : 0;
                byte percentDeviation = 15;
                ulong deviatedPriceMin = avgExpectedPrice - (avgExpectedPrice * percentDeviation) / 100;
                ulong deviatedPriceMax = avgExpectedPrice + (avgExpectedPrice * percentDeviation) / 100;


                objUpcoming.SortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
                if ((_objData.IsNewBike || _objData.IsDiscontinuedBike) && _objData.ModelPageEntity != null && _objData.ModelPageEntity.ModelDetails != null &&  _objData.ModelPageEntity.ModelDetails.MakeBase != null)
                {
                    objUpcoming.Filters = new UpcomingBikesListInputEntity()
                    {
                        MakeId = _objData.ModelPageEntity.ModelDetails.MakeBase.MakeId,
                        PageNo = 1,
                        PageSize = 9
                    };
                }
                objUpcomingBikes = objUpcoming.GetData();

                if (objUpcomingBikes != null && objUpcomingBikes.UpcomingBikes != null)
                {
                    if (_objData.IsUpcomingBike)
                    {
                        objUpcomingBikes.UpcomingBikes = objUpcomingBikes.UpcomingBikes
                                        .OrderByDescending(
                                            m => m.BodyStyleId == (uint)_objData.BodyStyle &&
                                            ((deviatedPriceMin <= m.EstimatedPriceMin && m.EstimatedPriceMin <= deviatedPriceMax)
                                            || (deviatedPriceMin <= m.EstimatedPriceMax && m.EstimatedPriceMax <= deviatedPriceMax))
                                        )
                                        .ThenByDescending(
                                            m => (deviatedPriceMin <= m.EstimatedPriceMin && m.EstimatedPriceMin <= deviatedPriceMax) ||
                                            (deviatedPriceMin <= m.EstimatedPriceMax && m.EstimatedPriceMax <= deviatedPriceMax)
                                        )
                                        .Where(x => x.ModelBase.ModelId != _modelId)
                                        .Take(9)
                                        .TakeWhile(m =>
                                            (deviatedPriceMin <= m.EstimatedPriceMin && m.EstimatedPriceMin <= deviatedPriceMax) || (deviatedPriceMin <= m.EstimatedPriceMax && m.EstimatedPriceMax <= deviatedPriceMax)).Take(10).TakeWhile(m => (deviatedPriceMin <= m.EstimatedPriceMin && m.EstimatedPriceMin <= deviatedPriceMax)
                                            || (deviatedPriceMin <= m.EstimatedPriceMax && m.EstimatedPriceMax <= deviatedPriceMax)
                                        ); 
                    }
                    else
                    {
                        
                        IEnumerable<UpcomingBikeEntity> upcomingBikesBodyStyle = objUpcomingBikes.UpcomingBikes.Where(m => m.BodyStyleId == (uint)_objData.BodyStyle);
                        if (upcomingBikesBodyStyle != null)
                        {
                            objUpcomingBikes.UpcomingBikes = upcomingBikesBodyStyle.Concat(objUpcomingBikes.UpcomingBikes.Where(m => m.BodyStyleId != (uint)_objData.BodyStyle)).Take(9);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ModelPage.BindUpCompingBikesWidget()");
            }
            return objUpcomingBikes;
        }

        /// <summary>
        /// Created BY : Sushil Kumar on 14th March 2015
        /// Summary : To set EMI details for the dealer if no EMI Details available for the dealer
        /// </summary>
        private EMI SetDefaultEMIDetails(uint bikePrice)
        {
            EMI _objEMI = null;
            if (bikePrice > 0)
            {
                try
                {
                    _objEMI = new EMI();
                    _objEMI.MaxDownPayment = Convert.ToSingle(40 * bikePrice / 100);
                    _objEMI.MinDownPayment = Convert.ToSingle(10 * bikePrice / 100);
                    _objEMI.MaxTenure = 48;
                    _objEMI.MinTenure = 12;
                    _objEMI.MaxRateOfInterest = 15;
                    _objEMI.MinRateOfInterest = 10;
                    _objEMI.ProcessingFee = 0; //2000

                    _objEMI.Tenure = Convert.ToUInt16((_objEMI.MaxTenure - _objEMI.MinTenure) / 2 + _objEMI.MinTenure);
                    _objEMI.RateOfInterest = (_objEMI.MaxRateOfInterest - _objEMI.MinRateOfInterest) / 2 + _objEMI.MinRateOfInterest;
                    _objEMI.MinLoanToValue = Convert.ToUInt32(Math.Round(bikePrice * 0.7, MidpointRounding.AwayFromZero));
                    _objEMI.MaxLoanToValue = bikePrice;
                    _objEMI.EMIAmount = Convert.ToUInt32(Math.Round((_objEMI.MinLoanToValue * _objEMI.RateOfInterest / 1200) / (1 - Math.Pow((1 + (_objEMI.RateOfInterest / 1200)), (-1.0 * _objEMI.Tenure)))));
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "setDefaultEMIDetails");
                }
            }
            return _objEMI;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 5th Dec 2017
        /// Summary : Set EMI details when dealer EMI details present
        /// </summary>
        /// <param name="price"></param>
        private void SetDealerEMIDetails(uint price)
        {
            try
            {
                //EMI details
                #region Set EMI Details
                var objEMI = _objData.DetailedDealer.PrimaryDealer.EMIDetails;
                if (objEMI != null)
                {
                    //Setting the dealer down payment amount as objEMI contains the percentage value

                    objEMI.MinDownPayment = Convert.ToSingle(objEMI.MinDownPayment * price / 100);
                    objEMI.MaxDownPayment = Convert.ToSingle(objEMI.MaxDownPayment * price / 100);

                    var _objEMI = SetDefaultEMIDetails(price);
                    if (objEMI.MinDownPayment < 1 || objEMI.MaxDownPayment < 1)
                    {
                        objEMI.MinDownPayment = _objEMI.MinDownPayment;
                        objEMI.MaxDownPayment = _objEMI.MaxDownPayment;
                    }

                    if (objEMI.MinTenure < 1 || objEMI.MaxTenure < 1)
                    {
                        objEMI.MinTenure = _objEMI.MinTenure;
                        objEMI.MaxTenure = _objEMI.MaxTenure;
                    }

                    if (objEMI.MinRateOfInterest < 1 || objEMI.MaxRateOfInterest < 1)
                    {
                        objEMI.MinRateOfInterest = _objEMI.MinRateOfInterest;
                        objEMI.MaxRateOfInterest = _objEMI.MaxRateOfInterest;
                    }

                    objEMI.Tenure = Convert.ToUInt16((objEMI.MaxTenure - objEMI.MinTenure) / 2 + objEMI.MinTenure);
                    objEMI.RateOfInterest = (objEMI.MaxRateOfInterest - objEMI.MinRateOfInterest) / 2 + objEMI.MinRateOfInterest;
                    objEMI.MinLoanToValue = Convert.ToUInt32(price * .7);
                    objEMI.MaxLoanToValue = price;
                    objEMI.EMIAmount = Convert.ToUInt32(Math.Round((_objEMI.MinLoanToValue * _objEMI.RateOfInterest / 1200) / (1 - Math.Pow((1 + (_objEMI.RateOfInterest / 1200)), (-1.0 * _objEMI.Tenure)))));
                }
                else
                {
                    objEMI = SetDefaultEMIDetails(price);
                }

                _objData.EMIDetails = objEMI;
                #endregion
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SetDealerEMIDetails");
            }
        }

        /// <summary>
        /// Created by      :   Sumit Kate on 30 Nov 2017
        /// Descriptiion    :   Bind EMI calculator widget on model page
        /// </summary>
        private void BindEMICalculator(uint Price)
        {
            try
            {
                _objData.EMICalculator = new EMICalculatorVM { EMI = _objData.EMIDetails, BikePrice = Price, EMIJsonBase64 = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(Newtonsoft.Json.JsonConvert.SerializeObject(_objData.EMIDetails)) };
                _objData.EMICalculator.ESEMICampaign = _objData.EMICampaign;
                _objData.EMICalculator.IsMobile = IsMobile;
                _objData.EMICalculator.PQId = _objData.PQId;
                _objData.EMICalculator.IsPremiumDealer = _objData.IsPremiumDealer;
                _objData.EMICalculator.DealerDetails = _objData.DealerDetails;
                _objData.EMICalculator.PremiumDealerLeadSourceId = IsMobile ? LeadSourceEnum.EMI_Calculator_ModelPage_Mobile : LeadSourceEnum.EMI_Calculator_ModelPage_Desktop;
                _objData.EMICalculator.BikeName = _objData.BikeName;
                _objData.EMICalculator.IsPrimaryDealer = _objData.IsPrimaryDealer;
                _objData.EMICalculator.IsManufacturerLeadAdShown = _objData.EMICalculator.ESEMICampaign != null;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindEMICalculator");
            }
        }

        private void BindBestBikeWidget(EnumBikeBodyStyles BodyStyleType, uint? cityId = null)
        {
            try
            {
                OtherBestBikesModel otherBestBikesModel = new OtherBestBikesModel((uint)_objData.ModelPageEntity.ModelDetails.MakeBase.MakeId, _objData.ModelPageEntity.ModelDetails.MakeBase.MakeName, (uint)_objData.ModelId, BodyStyleType, _objBestBikes, cityId);

                _objData.OtherBestBikes = otherBestBikesModel.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("FetchBestBikesList{0} ", BodyStyleType));
            }
        }

        private UsedBikeByModelCityVM BindUsedBikeByModel(uint makeId, uint cityId)
        {
            UsedBikeByModelCityVM UsedBikeModel = new UsedBikeByModelCityVM();
            try
            {
                UsedBikesByModelCityWidget objUsedBike = new UsedBikesByModelCityWidget(_usedBikesCache, 6, makeId, _modelId, cityId);
                UsedBikeModel = objUsedBike.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ModelPage.BindUsedBikeByModel()");
            }

            return UsedBikeModel;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 13 Jan 2017
        /// Description: To get model ranking details
        /// </summary>
        /// <param name="modelId"></param>
        private void GetBikeRankingCategory()
        {
            BindGenericBikeRankingControl bikeRankingSlug = new BindGenericBikeRankingControl();
            bikeRankingSlug.ModelId = _modelId;
            var objBikeRanking = bikeRankingSlug.GetBikeRankingByModel();
            if (objBikeRanking != null)
            {
                _objData.BikeRanking = new BikeRankingPropertiesEntity()
                {
                    ModelId = bikeRankingSlug.ModelId,
                    Rank = objBikeRanking.Rank,
                    BodyStyle = objBikeRanking.BodyStyle,
                    StyleName = bikeRankingSlug.StyleName,
                    BikeType = bikeRankingSlug.BikeType,
                    RankText = bikeRankingSlug.RankText
                };
            }
            else
                _objData.BikeRanking = new BikeRankingPropertiesEntity();
        }

        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- Metas description according to discountinue,upcoming,continue bikes
        /// Modified by :- Subodh Jain 19 june 2017
        /// Summary :- Added TargetModels and Target Make
        /// Modified by :- Ashutosh Sharma on 30 Aug 2017
        /// Description :- Removed GST from Title and Description 
        /// Modified by : Ashutosh Sharma on 13 Oct 2017
        /// Description : Meta Description replaced with ModelSummary for SynopsisSummaryMergeMakeIds in BWConfiguration.
        /// Modified by : Ashutosh Sharma on 30 Nov 2017
        /// Description : Meta Description replaced with ModelSummary for all makes for new bikes.
        /// </summary>
        private void CreateMetas()
        {
            try
            {
                if (_objData.IsModelDetails)
                {
                    BindDescription();

                    BikeVersionMinSpecs objAvgPriceVersion = null;
                    if (_objData.ModelPageEntity != null && _objData.ModelPageEntity.ModelVersions.Any())
                    {
                        objAvgPriceVersion = _objData.ModelPageEntity.ModelVersions.FirstOrDefault(x => x.AverageExShowroom > 0);
                    }
                    uint AvgPrice = objAvgPriceVersion != null ? objAvgPriceVersion.AverageExShowroom : 0;


                    if (_objData.ModelPageEntity.ModelDetails.Futuristic && _objData.ModelPageEntity.UpcomingBike != null)
                    {
                        _objData.PageMetaTags.Description = string.Format("{0} {1} Price in India is expected between Rs. {2} and Rs. {3}. Check out {0} {1}  specifications, reviews, mileage, versions, news & images at BikeWale.com. Launch date of {1} is around {4}", _objData.ModelPageEntity.ModelDetails.MakeBase.MakeName, _objData.ModelPageEntity.ModelDetails.ModelName, Bikewale.Utility.Format.FormatNumeric(Convert.ToString(_objData.ModelPageEntity.UpcomingBike.EstimatedPriceMin)), Bikewale.Utility.Format.FormatNumeric(Convert.ToString(_objData.ModelPageEntity.UpcomingBike.EstimatedPriceMax)), _objData.ModelPageEntity.UpcomingBike.ExpectedLaunchDate);
                    }
                    else if (!_objData.ModelPageEntity.ModelDetails.New)
                    {
                        _objData.PageMetaTags.Description = string.Format("{0} {1} Price in India - Rs. {2}. It has been discontinued in India. There are {3} used {1} bikes for sale. Check out {1} specifications, reviews, mileage, versions, news & images at BikeWale.com", _objData.ModelPageEntity.ModelDetails.MakeBase.MakeName, _objData.ModelPageEntity.ModelDetails.ModelName, Bikewale.Utility.Format.FormatNumeric((_objData.BikePrice > 0 ? _objData.BikePrice : AvgPrice).ToString()), _objData.ModelPageEntity.ModelDetails.UsedListingsCnt);
                    }
                    else
                    {
                        _objData.PageMetaTags.Description = _objData.ModelSummary;
                    }

                    _objData.PageMetaTags.Title = string.Format("{0} Price, Images, Colours, Mileage & Reviews | BikeWale", _objData.BikeName);

                    _objData.PageMetaTags.CanonicalUrl = string.Format("{0}/{1}-bikes/{2}/", BWConfiguration.Instance.BwHostUrl, _objData.ModelPageEntity.ModelDetails.MakeBase.MaskingName, _objData.ModelPageEntity.ModelDetails.MaskingName);

                    _objData.AdTags.TargetedModel = _objData.ModelPageEntity.ModelDetails.ModelName;
                    _objData.AdTags.TargetedMakes = _objData.ModelPageEntity.ModelDetails.MakeBase.MakeName;
                    _objData.PageMetaTags.AlternateUrl = BWConfiguration.Instance.BwHostUrl + "/m/" + _objData.ModelPageEntity.ModelDetails.MakeBase.MaskingName + "-bikes/" + _objData.ModelPageEntity.ModelDetails.MaskingName + "/";
                    _objData.AdTags.TargetedCity = _objData.LocationCookie.City;
                    _objData.PageMetaTags.Keywords = string.Format("{0},{0} Bike, bike, {0} Price, {0} Reviews, {0} Images, {0} Mileage", _objData.BikeName);
                    _objData.PageMetaTags.OGImage = Bikewale.Utility.Image.GetPathToShowImages(_objData.ModelPageEntity.ModelDetails.OriginalImagePath, _objData.ModelPageEntity.ModelDetails.HostUrl, Bikewale.Utility.ImageSize._476x268);
                    _objData.Page_H1 = _objData.BikeName;

                    CheckCustomPageMetas();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.BikeModels.ModelPage --> CreateMetas() ModelId: {0}, MaskingName: {1}", _modelId, ""));
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 13th Aug 2017
        /// Description : Function to check and set custom page metas and summary for the page
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="objMakeBase"></param>
        private void CheckCustomPageMetas()
        {
            try
            {
                if (_objData.IsModelDetails && _objData.ModelPageEntity.ModelDetails.Metas != null)
                {
                    var metas = _objData.ModelPageEntity.ModelDetails.Metas.FirstOrDefault(m => m.PageId == (int)(IsMobile ? BikewalePages.Mobile_ModelPage : BikewalePages.Desktop_ModelPage));

                    if (metas != null)
                    {
                        if (!string.IsNullOrEmpty(metas.Title))
                        {
                            _objData.PageMetaTags.Title = metas.Title;
                        }
                        if (!string.IsNullOrEmpty(metas.Description))
                        {
                            _objData.PageMetaTags.Description = metas.Description;
                        }
                        if (!string.IsNullOrEmpty(metas.Keywords))
                        {
                            _objData.PageMetaTags.Keywords = metas.Keywords;
                        }
                        if (!string.IsNullOrEmpty(metas.Heading))
                        {
                            _objData.Page_H1 = metas.Heading;
                        }
                        if (!string.IsNullOrEmpty(metas.Summary))
                        {
                            _objData.ModelSummary = metas.Summary;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.BikeModels.ModelPage.CheckCustomPageMetas() modelId:{0}", _modelId));
            }
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   20 Nov 2015
        /// Description     :   To Load version dropdown at Specs for each variant
        /// Modified by     :   Sumit Kate on 04 Apr 2017
        /// Description     :   Loads the default selected version based on pricing
        /// - Default version is selected based on the priority
        ///     Dealer Pricing (Highest priority) -> Bikewale Pricing -> Version pricing (Lowest)
        ///  Modified by : Ashutosh Sharma on 30 Aug 2017 
        ///  Description : Removed IsGstPrice flag
        ///  Modified by : Ashutosh Sharma on 26-Sep-2017
        ///  Description : Added condition to get version id for futuristic bike models.
        /// </summary>
        private void LoadVariants(BikeModelPageEntity modelPg)
        {
            try
            {
                if (modelPg != null && modelPg.ModelDetails != null && modelPg.ModelVersions != null && !modelPg.ModelDetails.Futuristic)
                {
                    if (_pqOnRoad != null)
                    {
                        ///Dealer Pricing
                        if (_pqOnRoad.IsDealerPriceAvailable && _pqOnRoad.DPQOutput != null && _pqOnRoad.DPQOutput.Varients.Any() && modelPg.ModelVersions.Count > 0)
                        {
                            foreach (var version in modelPg.ModelVersions)
                            {
                                var selectVersion = _pqOnRoad.DPQOutput.Varients.FirstOrDefault(m => m.objVersion.VersionId == version.VersionId);
                                if (selectVersion != null)
                                {
                                    version.Price = selectVersion.OnRoadPrice;
                                }
                            }

                            ///Choose the min price version of dealer
                            if (_objData.VersionId == 0)
                            {
                                _objData.VersionId = (uint)_pqOnRoad.DPQOutput.Varients.OrderBy(m => m.OnRoadPrice).FirstOrDefault().objVersion.VersionId;
                            }
                            IsDealerPriceAvailble = true;
                        }//Bikewale Pricing
                        else if (_pqOnRoad.BPQOutput != null && _pqOnRoad.BPQOutput.Varients != null)
                        {
                            bool isSelectedUpdated = false;
                            foreach (var version in modelPg.ModelVersions)
                            {
                                var selected = _pqOnRoad.BPQOutput.Varients.FirstOrDefault(p => p.VersionId == version.VersionId);
                                if (selected != null)
                                {
                                    version.Price = !_objData.ShowOnRoadButton ? selected.OnRoadPrice : selected.Price;
                                    if (modelPg.ModelVersions.Any() && version.Price == 0 && !isSelectedUpdated)
                                    {
                                        _objData.SelectedVersion = modelPg.ModelVersions.FirstOrDefault(m => m.AverageExShowroom > 0 && m.VersionId == version.VersionId);
                                        isSelectedUpdated = true;
                                    }
                                }
                            }
                            ///Choose the min price version of city level pricing
                            if (_objData.VersionId == 0 && _pqOnRoad.BPQOutput.Varients.Any())
                            {
                                _objData.VersionId = (uint)_pqOnRoad.BPQOutput.Varients.OrderBy(m => m.OnRoadPrice).FirstOrDefault().VersionId;
                            }

                        }//Version Pricing
                        else
                        {
                            ///Choose the min price version
                            if (_objData.VersionId == 0)
                            {
                                var nonZeroVersion = modelPg.ModelVersions.Where(m => m.Price > 0);
                                if (nonZeroVersion != null && nonZeroVersion.Any())
                                {
                                    _objData.SelectedVersion = nonZeroVersion.OrderBy(x => x.Price).FirstOrDefault();
                                    _objData.VersionId = (uint)_objData.SelectedVersion.VersionId;
                                    _objData.BikePrice = _objData.CityId == 0 ? (uint)_objData.SelectedVersion.Price : 0;
                                }
                                else
                                {
                                    var firstVersion = modelPg.ModelVersions.FirstOrDefault();
                                    if (firstVersion != null)
                                    {
                                        _objData.VersionId = (uint)firstVersion.VersionId;
                                        _objData.BikePrice = _objData.CityId == 0 ? (uint)firstVersion.Price : 0;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ///Choose the min price version
                        if (_objData.VersionId == 0)
                        {
                            if (_objData.IsDiscontinuedBike)
                            {
                                var nonZeroVersion = modelPg.ModelVersions.Where(m => m.Price > 0);
                                if (nonZeroVersion != null && nonZeroVersion.Any())
                                {
                                    _objData.SelectedVersion = nonZeroVersion.OrderBy(x => x.Price).FirstOrDefault();
                                    _objData.VersionId = (uint)_objData.SelectedVersion.VersionId;
                                    _objData.BikePrice = (uint)_objData.SelectedVersion.Price;
                                }
                                else
                                {
                                    _objData.VersionId = (uint)modelPg.ModelVersions.FirstOrDefault().VersionId;
                                    _objData.BikePrice = Convert.ToUInt32(_objData.SelectedVersion != null ? _objData.SelectedVersion.Price : modelPg.ModelVersions.FirstOrDefault().Price);
                                }
                            }
                            else
                            {
                                var nonZeroVersion = modelPg.ModelVersions.Where(m => m.Price > 0);
                                if (nonZeroVersion != null && nonZeroVersion.Any())
                                {
                                    _objData.SelectedVersion = nonZeroVersion.OrderBy(x => x.Price).FirstOrDefault();
                                    _objData.VersionId = (uint)_objData.SelectedVersion.VersionId;
                                    _objData.BikePrice = _objData.ShowOnRoadButton ? (uint)_objData.SelectedVersion.Price : (_objData.CityId == 0 ? (uint)_objData.SelectedVersion.Price : 0);
                                }
                                else
                                {
                                    _objData.VersionId = (uint)modelPg.ModelVersions.FirstOrDefault().VersionId;
                                    _objData.BikePrice = _objData.ShowOnRoadButton ? (uint)_objData.SelectedVersion.Price : (_objData.CityId == 0 ? (uint)_objData.SelectedVersion.Price : 0);

                                }
                            }
                        }

                        if (_objData.CityId != 0 && !_objData.IsDiscontinuedBike && !_objData.HasCityPricing)
                        {
                            foreach (var version in modelPg.ModelVersions)
                            {
                                version.Price = 0;
                            }
                        }
                    }

                    if (modelPg.ModelVersions.Count > 0)
                    {
                        var firstVer = modelPg.ModelVersions.FirstOrDefault();
                        if (firstVer != null)
                            _objData.VersionName = firstVer.VersionName;
                    }
                }
                else if (modelPg != null && modelPg.ModelVersions != null)
                {
                    BikeVersionMinSpecs objBikeVersionMinSpecs = modelPg.ModelVersions.FirstOrDefault();
                    if (objBikeVersionMinSpecs != null)
                    {
                        _objData.VersionId = Convert.ToUInt32(objBikeVersionMinSpecs.VersionId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.BikeModels.ModelPage --> LoadVariants() ModelId: {0}, MaskingName: {1}", _modelId, ""));
            }
        }

        /// <summary>
        /// Created By:- Sushil Kumar on 29-Mar-2017 
        /// Summary:- Process the input query
        /// </summary>
        /// <param name="modelMasking"></param>
        private void ParseQueryString(string makeMasking, string modelMasking)
        {
            ModelMaskingResponse objModelResponse = null;
            string newMakeMasking = string.Empty;
            bool isMakeRedirection = false;
            try
            {
                newMakeMasking = ProcessMakeMaskingName(makeMasking, out isMakeRedirection);
                if (!string.IsNullOrEmpty(newMakeMasking) && !string.IsNullOrEmpty(makeMasking) && !string.IsNullOrEmpty(modelMasking))
                {

                    objModelResponse = new Common.ModelHelper().GetModelDataByMasking(makeMasking, modelMasking);

                    if (objModelResponse != null)
                    {
                        if (objModelResponse.StatusCode == 200)
                        {
                            _modelId = objModelResponse.ModelId;
                            Status = StatusCodes.ContentFound;
                        }
                        else if (objModelResponse.StatusCode == 301 || isMakeRedirection)
                        {
                            RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(modelMasking, objModelResponse.MaskingName).Replace(makeMasking, newMakeMasking);
                            Status = StatusCodes.RedirectPermanent;
                        }
                        else
                        {
                            Status = StatusCodes.ContentNotFound;
                        }
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + "ParseQueryString");
                Status = StatusCodes.RedirectPermanent;
                RedirectUrl = "/new-bikes-in-india/";
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 11th Dec 2017
        /// Description : Process make masking name for redirection
        /// </summary>
        /// <param name="make"></param>
        /// <param name="isMakeRedirection"></param>
        /// <returns></returns>
        private string ProcessMakeMaskingName(string make, out bool isMakeRedirection)
        {
            MakeMaskingResponse makeResponse = null;
            Common.MakeHelper makeHelper = new Common.MakeHelper();
            isMakeRedirection = false;
            if (!string.IsNullOrEmpty(make))
            {
                makeResponse = makeHelper.GetMakeByMaskingName(make);
            }
            if (makeResponse != null)
            {
                if (makeResponse.StatusCode == 200)
                {
                    return makeResponse.MaskingName;
                }
                else if (makeResponse.StatusCode == 301)
                {
                    isMakeRedirection = true;
                    return makeResponse.MaskingName;
                }
                else
                {
                    return "";
                }
            }

            return "";
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// Modified by : Sajal Gupta on 28-02-2017
        /// Description : Get model page data from calling BAL layer instead of calling cache layer.
        /// Modified by : Ashutosh Sharma on 30 Aug 2017
        /// Description : Removed IsGstPrice flag.
        /// </summary>
        private BikeModelPageEntity FetchModelPageDetails(uint modelID)
        {
            BikeModelPageEntity modelPg = null;
            try
            {
                if (modelID > 0)
                {
                    modelPg = _objModel.GetModelPageDetails(Convert.ToInt16(modelID), (int)_objData.VersionId);

                    if (modelPg != null)
                    {
                        if (modelPg.ModelVersions != null && _objData.VersionId > 0)
                        {
                            _objData.SelectedVersion = modelPg.ModelVersions.FirstOrDefault(v => v.VersionId == _objData.VersionId);
                        }

                        if (_objData.VersionId > 0 && _objData.SelectedVersion != null)
                        {
                            _objData.BikePrice = _objData.CityId == 0 ? Convert.ToUInt32(_objData.SelectedVersion.Price) : (_objData.HasCityPricing ? Convert.ToUInt32(_objData.SelectedVersion.Price) : 0);
                        }
                        else if (modelPg.ModelDetails != null)
                        {
                            _objData.BikePrice = _objData.CityId == 0 ? Convert.ToUInt32(modelPg.ModelDetails.MinPrice) : (_objData.HasCityPricing ? Convert.ToUInt32(modelPg.ModelDetails.MinPrice) : 0);
                        }

                        // for new bike
                        if (!modelPg.ModelDetails.Futuristic && modelPg.ModelVersionSpecs != null && _objData.SelectedVersion != null)
                        {
                            // Check it versionId passed through url exists in current model's versions
                            _objData.VersionId = (uint)_objData.SelectedVersion.VersionId;
                        }

                        //for all bikes including upcoming bikes as details are mandatory
                        if (modelPg.ModelDetails != null && modelPg.ModelDetails.ModelName != null && modelPg.ModelDetails.MakeBase != null)
                        {
                            _objData.BikeName = string.Format("{0} {1}", modelPg.ModelDetails.MakeBase.MakeName, modelPg.ModelDetails.ModelName);
                        }

                        // Discontinued bikes
                        if (modelPg.ModelDetails != null && !modelPg.ModelDetails.New && modelPg.ModelVersions != null && modelPg.ModelVersions.Count > 1 && _objData.SelectedVersion != null)
                        {
                            _objData.BikePrice = (uint)_objData.SelectedVersion.Price;
                        }

                        if (modelPg.ModelDetails != null && modelPg.ModelDetails.PhotosCount > 0 && modelPg.ModelColors != null && modelPg.ModelColors.Any())
                        {
                            var colorImages = modelPg.ModelColors.Where(x => x.ColorImageId > 0);
                            if (colorImages != null && colorImages.Any())
                            {
                                string returnUrl;

                                if (IsMobile)
                                    returnUrl = string.Format("/m/{0}-bikes/{1}/", modelPg.ModelDetails.MakeBase.MaskingName, modelPg.ModelDetails.MaskingName);
                                else
                                    returnUrl = string.Format("/{0}-bikes/{1}/", modelPg.ModelDetails.MakeBase.MaskingName, modelPg.ModelDetails.MaskingName);

                                _objData.ColourImageUrl = string.Format("/{0}-bikes/{1}/images/?q={2}", modelPg.ModelDetails.MakeBase.MaskingName, modelPg.ModelDetails.MaskingName, EncodingDecodingHelper.EncodeTo64(string.Format("colorImageId={0}&retUrl={1}", colorImages.FirstOrDefault().ColorImageId, returnUrl)));

                                _objData.ModelColorPhotosCount = colorImages.Count();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.BikeModels.ModelPage -> FetchmodelPgDetails(): Modelid ==> {0}", _modelId));
            }
            return modelPg;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   20 Nov 2015
        /// Description     :   Fetch On road price depending on City, Area and DealerPQ and BWPQ
        /// Modified By     :   Sushil Kumar on 19th April 2016
        /// Description     :   Removed repeater binding for rptCategory and rptDiscount as view breakup popup removed
        /// Modified by     :   Sajal Gupta on 13-01-2017
        /// Description     :   Changed flag objData.IsLocationSelected if onroad price not available
        /// Modifide By :- Subodh jain on 02 March 2017
        /// Summary:- added manufacturer campaign leadpopup changes
        /// Modified by : Ashutosh Sharma on 30 Aug 2017
        /// Description : Removed IsGstPrice flag
        /// </summary>
        private void FetchOnRoadPrice(BikeModelPageEntity modelPage)
        {
            var errorParams = string.Empty;
            try
            {
                if (_cityId > 0 && _objData.City != null && ((_objData.City.HasAreas && _areaId > 0) || !_objData.City.HasAreas))
                {
                    _pqOnRoad = GetOnRoadPrice();
                    // Set Pricequote Cookie
                    if (_pqOnRoad != null)
                    {
                        if (_pqOnRoad.PriceQuote != null)
                        {
                            _objData.DealerId = _pqOnRoad.PriceQuote.DealerId;
                        }
                        _objData.MPQString = EncodingDecodingHelper.EncodeTo64(Bikewale.Common.PriceQuoteQueryString.FormQueryString(_cityId.ToString(), _pqOnRoad.PriceQuote.PQId.ToString(), _areaId.ToString(), _objData.VersionId.ToString(), _objData.DealerId.ToString()));

                        if (_pqOnRoad.IsDealerPriceAvailable && _pqOnRoad.DPQOutput != null && _pqOnRoad.DPQOutput.Varients != null && _pqOnRoad.DPQOutput.Varients.Any())
                        {
                            #region when dealer Price is Available

                            var selectedVariant = _pqOnRoad.DPQOutput.Varients.FirstOrDefault(p => p.objVersion.VersionId == _objData.VersionId);
                            if (selectedVariant != null)
                            {
                                _objData.BikePrice = selectedVariant.OnRoadPrice;
                                uint totalDiscountedPrice = 0;
                                if (selectedVariant.PriceList != null)
                                {
                                    totalDiscountedPrice = CommonModel.GetTotalDiscount(_pqOnRoad.discountedPriceList);
                                }

                                if (_pqOnRoad.discountedPriceList != null && _pqOnRoad.discountedPriceList.Count > 0)
                                {
                                    _objData.BikePrice = (_objData.BikePrice - totalDiscountedPrice);
                                }
                            }
                            else // Show dealer properties and Bikewale priceQuote when dealer has pricing for any of the bike
                            // Added on 13 Feb 2017 Pivotal Id:138698777
                            {
                                SetBikeWalePQ(_pqOnRoad);
                            }

                            #endregion when dealer Price is Available
                        }
                        else
                        {
                            SetBikeWalePQ(_pqOnRoad);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(errorParams))
                    errorParams = "=== modelpage ===" + Newtonsoft.Json.JsonConvert.SerializeObject(modelPage);
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.BikeModels.ModelPage -> FetchOnRoadPrice() " + " ===== parameters ========= " + errorParams);
            }
            finally
            {
                if (_pqOnRoad != null && _pqOnRoad.PriceQuote != null && _pqOnRoad.PriceQuote.IsDealerAvailable && _cityId > 0 && _objData.VersionId > 0)
                {
                    _objData.DetailedDealer = _objDealerDetails.GetDealerQuotationV2(_cityId, _objData.VersionId, _objData.DealerId, _areaId);
                }
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Jun 2017
        /// Description :   Fetches Manufacturer Campaigns
        /// Modified by  :  Sushil Kumar on 11th Aug 2017
        /// Description :   Store dealerid for manufacturer campaigns for impressions tracking
        /// Modified by :   Sumit Kate on 30 Nov 2017
        /// Description :   Enable EMI ES Campaign on Model Page
        /// </summary>
        private void GetManufacturerCampaign()
        {
            try
            {
                if (_objManufacturerCampaign != null && !(_objData.IsDealerDetailsExists))
                {
                    ManufacturerCampaignEntity campaigns = _objManufacturerCampaign.GetCampaigns(_modelId, _cityId, ManufacturerCampaignPageId);
                    if (campaigns.LeadCampaign != null)
                    {
                        _objData.LeadCampaign = new ManufactureCampaignLeadEntity()
                        {
                            Area = GlobalCityArea.GetGlobalCityArea().Area,
                            CampaignId = campaigns.LeadCampaign.CampaignId,
                            DealerId = campaigns.LeadCampaign.DealerId,
                            DealerRequired = campaigns.LeadCampaign.DealerRequired,
                            EmailRequired = campaigns.LeadCampaign.EmailRequired,
                            LeadsButtonTextDesktop = campaigns.LeadCampaign.LeadsButtonTextDesktop,
                            LeadsButtonTextMobile = campaigns.LeadCampaign.LeadsButtonTextMobile,
                            LeadSourceId = (int)LeadSource,
                            PqSourceId = (int)PQSource,
                            GACategory = "Model_Page",
                            GALabel = string.Format("{0}_{1}", _objData.BikeName, _objData.City != null ? _objData.City.CityName : string.Empty),
                            LeadsHtmlDesktop = campaigns.LeadCampaign.LeadsHtmlDesktop,
                            LeadsHtmlMobile = campaigns.LeadCampaign.LeadsHtmlMobile,
                            LeadsPropertyTextDesktop = campaigns.LeadCampaign.LeadsPropertyTextDesktop,
                            LeadsPropertyTextMobile = campaigns.LeadCampaign.LeadsPropertyTextMobile,
                            MakeName = _objData.ModelPageEntity.ModelDetails.MakeBase.MakeName,
                            Organization = campaigns.LeadCampaign.Organization,
                            MaskingNumber = campaigns.LeadCampaign.MaskingNumber,
                            PincodeRequired = campaigns.LeadCampaign.PincodeRequired,
                            PopupDescription = campaigns.LeadCampaign.PopupDescription,
                            PopupHeading = campaigns.LeadCampaign.PopupHeading,
                            PopupSuccessMessage = campaigns.LeadCampaign.PopupSuccessMessage,
                            ShowOnExshowroom = campaigns.LeadCampaign.ShowOnExshowroom,
                            PQId = _objData.PQId,
                            VersionId = _objData.VersionId,
                            CurrentPageUrl = CurrentPageUrl,
                            PlatformId = Convert.ToUInt16(IsMobile ? 2 : 1),
                            BikeName = _objData.BikeName,
                            LoanAmount = Convert.ToUInt32((_objData.BikePrice) * 0.8)
                        };

                        _objData.IsManufacturerTopLeadAdShown = !_objData.ShowOnRoadButton;
                        _objData.IsManufacturerLeadAdShown = (_objData.LeadCampaign.ShowOnExshowroom || (_objData.IsLocationSelected && !_objData.LeadCampaign.ShowOnExshowroom));


                        if (_objData.PQId == 0 && _cityId != 0)
                        {
                            PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                            objPQEntity.CityId = Convert.ToUInt16(_cityId);
                            objPQEntity.AreaId = Convert.ToUInt32(_areaId);
                            objPQEntity.ClientIP = "";
                            objPQEntity.SourceId = Convert.ToUInt16(Source);
                            objPQEntity.ModelId = _modelId;
                            objPQEntity.VersionId = _objData.VersionId;
                            objPQEntity.PQLeadId = Convert.ToUInt16(PQSource);
                            objPQEntity.UTMA = HttpContext.Current.Request.Cookies["__utma"] != null ? HttpContext.Current.Request.Cookies["__utma"].Value : "";
                            objPQEntity.UTMZ = HttpContext.Current.Request.Cookies["_bwutmz"] != null ? HttpContext.Current.Request.Cookies["_bwutmz"].Value : "";
                            objPQEntity.DeviceId = HttpContext.Current.Request.Cookies["BWC"] != null ? HttpContext.Current.Request.Cookies["BWC"].Value : "";
                            _objData.PQId = _objData.LeadCampaign.PQId = (uint)_objPQ.RegisterPriceQuote(objPQEntity);

                            if (_objData.LeadCampaign.DealerId == Bikewale.Utility.BWConfiguration.Instance.CapitalFirstDealerId)
                            {
                                var versions = _objPQCache.GetOtherVersionsPrices(_modelId, _cityId);
                                _objData.LeadCampaign.LoanAmount = (uint)Convert.ToUInt32((versions.FirstOrDefault(m => m.VersionId == _objData.VersionId).OnRoadPrice) * 0.8);
                            }
                        }
                        _objManufacturerCampaign.SaveManufacturerIdInPricequotes(_objData.PQId, campaigns.LeadCampaign.DealerId);
                    }

                    if (campaigns.EMICampaign != null)
                    {
                        _objData.EMICampaign = new ManufactureCampaignEMIEntity()
                        {
                            Area = GlobalCityArea.GetGlobalCityArea().Area,
                            CampaignId = campaigns.EMICampaign.CampaignId,
                            DealerId = campaigns.EMICampaign.DealerId,
                            Organization = campaigns.EMICampaign.Organization,
                            DealerRequired = campaigns.EMICampaign.DealerRequired,
                            EmailRequired = campaigns.EMICampaign.EmailRequired,
                            EMIButtonTextDesktop = campaigns.EMICampaign.EMIButtonTextDesktop,
                            EMIButtonTextMobile = campaigns.EMICampaign.EMIButtonTextMobile,
                            LeadSourceId = (int)LeadSource,
                            PqSourceId = (int)PQSource,
                            EMIPropertyTextDesktop = campaigns.EMICampaign.EMIPropertyTextDesktop,
                            EMIPropertyTextMobile = campaigns.EMICampaign.EMIPropertyTextMobile,
                            MakeName = _objData.ModelPageEntity.ModelDetails.MakeBase.MakeName,
                            MaskingNumber = campaigns.EMICampaign.MaskingNumber,
                            PincodeRequired = campaigns.EMICampaign.PincodeRequired,
                            PopupDescription = campaigns.EMICampaign.PopupDescription,
                            PopupHeading = campaigns.EMICampaign.PopupHeading,
                            PopupSuccessMessage = campaigns.EMICampaign.PopupSuccessMessage,
                            VersionId = _objData.VersionId,
                            CurrentPageUrl = CurrentPageUrl,
                            PlatformId = Convert.ToUInt16(IsMobile ? 2 : 1),
                            LoanAmount = Convert.ToUInt32((_objData.BikePrice) * 0.8)
                        };

                        _objData.IsManufacturerEMIAdShown = true;
                        if (campaigns.LeadCampaign == null)
                            _objManufacturerCampaign.SaveManufacturerIdInPricequotes(_objData.PQId, campaigns.EMICampaign.DealerId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ModelPage.GetManufacturerCampaign({0},{1},{2})", _modelId, _cityId, ManufacturerCampaignPageId));
            }
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 14 Feb 2017
        /// Summary: To set price variable with bikewale pricequote
        /// Modified by : Ashutosh Sharma on 30 Aug 2017 
        /// Description : Removed IsGstPrice flag
        /// </summary>
        /// <param name="pqOnRoad"></param>
        private void SetBikeWalePQ(PQOnRoadPrice pqOnRoad)
        {
            if (pqOnRoad != null && pqOnRoad.BPQOutput != null)
            {
                _objData.CampaignId = pqOnRoad.BPQOutput.CampaignId;
            }

            if (pqOnRoad != null && pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null && _objData.VersionId > 0)
            {
                var objSelectedVariant = pqOnRoad.BPQOutput.Varients.FirstOrDefault(p => p.VersionId == _objData.VersionId);
                if (objSelectedVariant != null)
                    _objData.BikePrice = _objData.IsLocationSelected && !_objData.ShowOnRoadButton ? Convert.ToUInt32(objSelectedVariant.OnRoadPrice) : Convert.ToUInt32(objSelectedVariant.Price);

                _objData.IsBPQAvailable = true;
            }
        }

        /// <summary>
        /// Author: Sangram Nandkhile
        /// Desc: Removed API Call for on road Price Quote
        /// Modified By : Lucky Rathore on 09 May 2016.
        /// Description : modelImage intialize.
        /// Modified By : Lucky Rathore on 27 June 2016
        /// Description : replace cookie __utmz with _bwutmz
        /// </summary>
        /// <returns></returns>
        private PQOnRoadPrice GetOnRoadPrice()
        {
            try
            {
                BikeQuotationEntity bpqOutput = null;
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                objPQEntity.CityId = Convert.ToUInt16(_cityId);
                objPQEntity.AreaId = Convert.ToUInt32(_areaId);
                objPQEntity.ClientIP = "";
                objPQEntity.SourceId = Convert.ToUInt16(Source);
                objPQEntity.ModelId = _modelId;
                objPQEntity.VersionId = _objData.VersionId;
                objPQEntity.PQLeadId = Convert.ToUInt16(PQSource);
                objPQEntity.UTMA = HttpContext.Current.Request.Cookies["__utma"] != null ? HttpContext.Current.Request.Cookies["__utma"].Value : "";
                objPQEntity.UTMZ = HttpContext.Current.Request.Cookies["_bwutmz"] != null ? HttpContext.Current.Request.Cookies["_bwutmz"].Value : "";
                objPQEntity.DeviceId = HttpContext.Current.Request.Cookies["BWC"] != null ? HttpContext.Current.Request.Cookies["BWC"].Value : "";
                PQOutputEntity objPQOutput = _objDealerPQ.ProcessPQV2(objPQEntity);

                if (objPQOutput != null)
                {
                    if (_objData.VersionId == 0)
                        _objData.VersionId = objPQOutput.VersionId;
                    _pqOnRoad = new PQOnRoadPrice();
                    _pqOnRoad.PriceQuote = objPQOutput;
                    if (objPQOutput != null && objPQOutput.PQId > 0)
                    {
                        _objData.PQId = (uint)objPQOutput.PQId;
                        bpqOutput = new BikeQuotationEntity();
                        bpqOutput.Varients = _objPQCache.GetOtherVersionsPrices(_modelId, _cityId);
                        if (bpqOutput != null)
                        {
                            _pqOnRoad.BPQOutput = bpqOutput;
                        }
                        if (objPQOutput.DealerId != 0)
                        {
                            _objData.ShowOnRoadButton = false;
                            _objData.IsAreaSelected = true;
                            PQ_QuotationEntity oblDealerPQ = null;
                            AutoBizCommon dealerPq = new AutoBizCommon();
                            try
                            {
                                oblDealerPQ = dealerPq.GetDealePQEntity(_cityId, objPQOutput.DealerId, _objData.VersionId);
                                if (oblDealerPQ != null)
                                {
                                    uint insuranceAmount = 0;
                                    foreach (var price in oblDealerPQ.PriceList)
                                    {
                                        _pqOnRoad.IsInsuranceFree = Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(objPQOutput.DealerId.ToString(), string.Empty, price.CategoryName, price.Price, ref insuranceAmount);
                                    }
                                    _pqOnRoad.IsInsuranceFree = true;
                                    _pqOnRoad.DPQOutput = oblDealerPQ;
                                    if (_pqOnRoad.DPQOutput.objOffers != null && _pqOnRoad.DPQOutput.objOffers.Count > 0)
                                        _pqOnRoad.DPQOutput.discountedPriceList = OfferHelper.ReturnDiscountPriceList(_pqOnRoad.DPQOutput.objOffers, _pqOnRoad.DPQOutput.PriceList);
                                    _pqOnRoad.InsuranceAmount = insuranceAmount;
                                    if (oblDealerPQ.discountedPriceList != null && oblDealerPQ.discountedPriceList.Count > 0)
                                    {
                                        _pqOnRoad.IsDiscount = true;
                                        _pqOnRoad.discountedPriceList = oblDealerPQ.discountedPriceList;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorClass.LogError(ex, string.Format("Bikewale.Models.BikeModels.ModelPage --> GetOnRoadPrice() ModelId: {0}, MaskingName: {1}", _modelId, ""));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.BikeModels.ModelPage --> GetOnRoadPrice() ModelId: {0}, MaskingName: {1}", _modelId, ""));
            }
            return _pqOnRoad;
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 05 Jan 2016
        /// Description :   Replaced the Convert.ToXXX with XXX.TryParse method
        /// Modified By : Sushil Kumar on 26th August 2016
        /// Description : Replaced location name from location cookie to selected location objects for city and area respectively.
        /// </summary>
        private void CheckCityCookie()
        {
            if (_modelId > 0)
            {
                _objData.LocationCookie = GlobalCityArea.GetGlobalCityArea();
                _cityId = _objData.LocationCookie.CityId;
                _areaId = _objData.LocationCookie.AreaId;
                if (_objData.LocationCookie.CityId > 0)
                {
                    var cities = _objCityCache.GetPriceQuoteCities(_modelId);

                    if (cities != null)
                    {
                        var selectedCity = cities.FirstOrDefault(m => m.CityId == _cityId);

                        if (selectedCity != null)
                        {
                            _objData.HasCityPricing = true;
                            _objData.City = selectedCity;
                            _objData.ShowOnRoadButton = selectedCity.HasAreas && _areaId <= 0;
                            _objData.IsAreaSelected = selectedCity.HasAreas && _areaId > 0;
                            if (!_objData.IsAreaSelected) _areaId = 0;
                        }
                        else
                        {
                            _objData.City = new Entities.Location.CityEntityBase
                            {
                                CityId = _cityId,
                                CityName = _objData.LocationCookie.City
                            };


                        }
                    }
                }
                else
                {
                    _objData.ShowOnRoadButton = true;
                }
            }
        }

        private void BindColorString()
        {
            try
            {
                if (_objData.ModelPageEntity != null && _objData.ModelPageEntity.ModelColors != null && _objData.ModelPageEntity.ModelColors.Any())
                {
                    int colorCount = _objData.ModelPageEntity.ModelColors.Count();
                    string lastColor = _objData.ModelPageEntity.ModelColors.Last().ColorName;
                    if (colorCount > 1)
                    {
                        _colorStr.AppendFormat("{0} is available in {1} different colours : ", _objData.BikeName, colorCount);
                        var colorArr = _objData.ModelPageEntity.ModelColors.Select(x => x.ColorName).Take(colorCount - 1);
                        // Comma separated colors (except last one)
                        _colorStr.Append(string.Join(", ", colorArr));
                        // Append last color with And
                        _colorStr.AppendFormat(" and {0}.", lastColor);
                    }
                    else if (colorCount == 1)
                    {
                        _colorStr.AppendFormat("{0} is available in {1} colour.", _objData.BikeName, lastColor);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeModels.ModelPage -->" + "BindColorString()");
            }
        }

        /// <summary>
        /// Created By :Snehal Dange on 22 Sep 2017
        /// Description : Page schema for similar bike on model page
        /// </summary>
        /// <param name="product"></param>
        private void SetSimilarBikesProperties(Product product)
        {
            try
            {

                if (_objData != null && _objData.SimilarBikes != null && _objData.SimilarBikes.Bikes != null)
                {
                    IList<Product> listSimilarBikes = new List<Product>();
                    foreach (var bike in _objData.SimilarBikes.Bikes)
                    {
                        listSimilarBikes.Add(new Product
                        {
                            Name = String.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName),
                            Url = String.Format("{0}{1}", BWConfiguration.Instance.BwHostUrl, UrlFormatter.BikePageUrl(bike.MakeBase.MaskingName, bike.ModelBase.MaskingName)),
                            Image = Image.GetPathToShowImages(bike.OriginalImagePath, bike.HostUrl, ImageSize._310x174)
                        });
                    }
                    product.IsSimilarTo = listSimilarBikes;

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.BikeModels.ModelPage.SetSimilarBikesProperties(), Model: {0}", _modelId));
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 28th Sep 2017
        /// Summary : Bind models by series id
        /// </summary>
        /// <param name="_objData"></param>
        public void BindModelsBySeriesId(ModelPageVM objData)
        {
            try
            {
                if (_modelId > 0)
                {
                    BikeModelsBySeriesPage objModelsBySeries = new BikeModelsBySeriesPage(_bikeSeries);
                    objData.ModelsBySeries = objModelsBySeries.GetData(_modelId, objData.ModelPageEntity.ModelDetails.ModelSeries.SeriesId);
                    if (objData.ModelsBySeries != null && objData.ModelsBySeries.SeriesModels != null)
                    {
                        objData.ModelsBySeries.Page = GAPages.Model_Page;
                        objData.ModelsBySeries.SeriesBase = objData.ModelPageEntity.ModelDetails.ModelSeries;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.BikeModels.BindModelsBySeriesId Model: {0}", _modelId));
            }
        }

        /// <summary>
        /// Created By:Snehal Dange on 3rd Nov 20127
        /// Description :  Get Mileagedetails for a particular model
        /// </summary>
        /// <param name="_objData"></param>
        private void BindMileageWidget(ModelPageVM _objData)
        {
            try
            {
                BikeMileageEntity obj = null;
                ModelMileageWidgetVM mileageWidgetObj = null;
                if (_modelId > 0 && _objData != null && _objModel != null)
                {

                    obj = _objModel.GetMileageDetails(_modelId);

                    if (obj != null)
                    {
                        mileageWidgetObj = new ModelMileageWidgetVM();
                        if (obj.Bikes != null && obj.BodyStyleMileage != null)
                        {
                            mileageWidgetObj.MileageInfo = obj.Bikes.FirstOrDefault(m => m.Model.ModelId == _modelId);
                            mileageWidgetObj.AvgBodyStyleMileageByUserReviews = obj.BodyStyleMileage.FirstOrDefault().AvgBodyStyleMileageByUserReviews;
                            mileageWidgetObj.SimilarBikeList = obj.Bikes.Where(u => u.Model.ModelId != _modelId);
                        }


                        if (mileageWidgetObj.MileageInfo != null)
                        {
                            if (mileageWidgetObj.MileageInfo.Rank <= 3)
                            {
                                mileageWidgetObj.WidgetHeading = string.Format("{0} with similar mileage", (mileageWidgetObj.MileageInfo.BodyStyleId.Equals((uint)EnumBikeBodyStyles.Scooter) ? "Scooters" : "Bikes"));
                            }
                            else
                            {
                                mileageWidgetObj.WidgetHeading = string.Format("{0} with better mileage", (mileageWidgetObj.MileageInfo.BodyStyleId.Equals((uint)EnumBikeBodyStyles.Scooter) ? "Scooters" : "Bikes"));
                            }
                            _objData.Mileage = mileageWidgetObj;


                            _objData.IsMileageByUsersAvailable = (mileageWidgetObj.MileageInfo.BodyStyleId.Equals((uint)EnumBikeBodyStyles.Scooter) || mileageWidgetObj.MileageInfo.BodyStyleId.Equals((uint)EnumBikeBodyStyles.SemiFaired) || mileageWidgetObj.MileageInfo.BodyStyleId.Equals((uint)EnumBikeBodyStyles.Street)) && (mileageWidgetObj.MileageInfo.MileageByUserReviews > 0);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.BikeModels.BindMileageWidget ModelId: {0}", _modelId));
            }

        }
        #endregion Methods
    }
}