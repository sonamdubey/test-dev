using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 25 Mar 2017
    /// Description :   BikeInfo Widget Model
    /// </summary>
    public class BikeInfoWidget
    {
        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _cityCacheRepo;
        private readonly BikeInfoTabType _pageId;
        private readonly IBikeModels<BikeModelEntity, int> _models;
        private readonly IBikeSeries _bikeSeries = null;
        private readonly uint _modelId, _cityId, _tabCount;

        /// <summary>
        /// Created by  :   Sumit Kate on 25 Mar 2017
        /// Description :   Constructor to initialize member variables
        /// </summary>
        /// <param name="bikeInfo"></param>
        /// <param name="cityCacheRepo"></param>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="totalTabCount"></param>
        /// <param name="pageId"></param>
        public BikeInfoWidget(IBikeInfo bikeInfo, ICityCacheRepository cityCacheRepo, uint modelId, uint cityId, uint totalTabCount, BikeInfoTabType pageId, IBikeModels<BikeModelEntity, int> models, IBikeSeries bikeSeries)
        {
            _bikeInfo = bikeInfo;
            _cityCacheRepo = cityCacheRepo;
            _modelId = modelId;
            _cityId = cityId;
            _tabCount = totalTabCount;
            _pageId = pageId;
            _bikeSeries = bikeSeries;
            _models = models;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 25 Mar 2017
        /// Description :   Returns the View Model
        /// </summary>
        /// <returns></returns>
        public BikeInfoVM GetData()
        {
            BikeInfoVM objVM = null;
            try
            {
                objVM = new BikeInfoVM();
                objVM.BikeInfo = _bikeInfo.GetBikeInfo(_modelId, _cityId, false);
                GenericBikeInfo bikeInfo = objVM.BikeInfo;
                if (bikeInfo != null)
                {
                    if (_cityId > 0)
                    {
                        var objCityList = _cityCacheRepo.GetAllCities(EnumBikeType.All);
                        objVM.CityDetails = objCityList.FirstOrDefault(c => c.CityId == _cityId);

                    }

                    bikeInfo.Tabs = BindInfoWidgetDatas(bikeInfo, objVM.CityDetails, _tabCount, _pageId);
                    objVM.BikeName = string.Format("{0} {1}", bikeInfo.Make.MakeName, bikeInfo.Model.ModelName);
                    objVM.BikeUrl = string.Format("{0}", Bikewale.Utility.UrlFormatter.BikePageUrl(bikeInfo.Make.MaskingName, bikeInfo.Model.MaskingName));
                    objVM.IsDiscontinued = (!bikeInfo.IsNew && !bikeInfo.IsFuturistic);
                    objVM.IsUpcoming = bikeInfo.IsFuturistic;
                    objVM.Category = _pageId;

                    if (bikeInfo.Make != null && bikeInfo.Make.MakeId > 0 && _modelId > 0)
                    {
                        objVM.Series = BindSeriesData(Convert.ToUInt32(bikeInfo.Make.MakeId));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikeInfoWidget.GetData({0},{1},{2},{3})", _modelId, _cityId, _tabCount, _pageId));
            }
            return objVM;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 25 Mar 2017
        /// Description :   Returns Tabs for Bike Info widget
        /// </summary>
        /// <param name="_genericBikeInfo"></param>
        /// <param name="cityDetails"></param>
        /// <param name="totalTabCount"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        private ICollection<BikeInfoTab> BindInfoWidgetDatas(GenericBikeInfo _genericBikeInfo, CityEntityBase cityDetails, uint totalTabCount, BikeInfoTabType pageId)
        {
            IList<BikeInfoTab> tabs = null;
            try
            {
                tabs = new List<BikeInfoTab>();
                if (_genericBikeInfo != null)
                {
                    if (_genericBikeInfo.ExpertReviewsCount > 0)
                    {
                        tabs.Add(new BikeInfoTab()
                        {
                            URL = Bikewale.Utility.UrlFormatter.FormatExpertReviewUrl(_genericBikeInfo.Make.MaskingName, _genericBikeInfo.Model.MaskingName),
                            Title = "Expert Reviews",
                            TabText = "Expert Reviews",
                            IconText = "reviews",
                            Count = _genericBikeInfo.ExpertReviewsCount,
                            Tab = BikeInfoTabType.ExpertReview
                        });
                    }
                    if (_genericBikeInfo.NewsCount > 0)
                    {
                        tabs.Add(new BikeInfoTab()
                        {
                            URL = Bikewale.Utility.UrlFormatter.FormatNewsUrl(_genericBikeInfo.Make.MaskingName, _genericBikeInfo.Model.MaskingName),
                            Title = "News",
                            TabText = "News",
                            IconText = "reviews",
                            Count = _genericBikeInfo.NewsCount,
                            Tab = BikeInfoTabType.News
                        });
                    }
                    if (_genericBikeInfo.PhotosCount > 0)
                    {
                        tabs.Add(new BikeInfoTab()
                        {
                            URL = Bikewale.Utility.UrlFormatter.FormatPhotoPageUrl(_genericBikeInfo.Make.MaskingName, _genericBikeInfo.Model.MaskingName),
                            Title = "Images",
                            TabText = "Images",
                            IconText = "photos",
                            Count = _genericBikeInfo.PhotosCount,
                            Tab = BikeInfoTabType.Image
                        });
                    }
                    if (_genericBikeInfo.VideosCount > 0)
                    {
                        tabs.Add(new BikeInfoTab()
                        {
                            URL = Bikewale.Utility.UrlFormatter.FormatVideoPageUrl(_genericBikeInfo.Make.MaskingName, _genericBikeInfo.Model.MaskingName),
                            Title = "Videos",
                            TabText = "Videos",
                            IconText = "videos",
                            Count = _genericBikeInfo.VideosCount,
                            Tab = BikeInfoTabType.Videos
                        });
                    }
                    if (_genericBikeInfo.IsSpecsAvailable)
                    {
                        tabs.Add(new BikeInfoTab()
                        {
                            URL = Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(_genericBikeInfo.Make.MaskingName, _genericBikeInfo.Model.MaskingName),
                            Title = "Specification",
                            TabText = "Specs",
                            IconText = "specs",
                            IsVisible = _genericBikeInfo.IsSpecsAvailable,
                            Tab = BikeInfoTabType.Specs
                        });
                    }
                    if (_genericBikeInfo.DealersCount > 0)
                    {
                        tabs.Add(new BikeInfoTab()
                        {
                            URL = (cityDetails != null) ? Bikewale.Utility.UrlFormatter.DealerLocatorUrl(_genericBikeInfo.Make.MaskingName, cityDetails.CityMaskingName) : Bikewale.Utility.UrlFormatter.DealerLocatorUrl(_genericBikeInfo.Make.MaskingName),
                            Title = string.Format("Dealers in {0}", cityDetails != null ? cityDetails.CityName : "India"),
                            TabText = "Dealers",
                            IconText = "dealers",
                            Count = _genericBikeInfo.DealersCount,
                            Tab = BikeInfoTabType.Dealers
                        });
                    }
                }
                if (tabs.Any())
                {
                    tabs = tabs.Where(m => (m.Count > 0 || m.IsVisible) && pageId != m.Tab).OrderBy(m => m.Tab).Take((int)totalTabCount).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "VideoDetailsHelper.BindInfoWidgetDatas");
            }
            return tabs;
        }


        /// <summary>
        /// Created by : Snehal Dange on 25th Oct 2018
        /// Desc :  Bind the series data in generic info widget
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        private BikeSeriesEntity BindSeriesData(uint makeId)
        {
            BikeSeriesEntity seriesInfo = null;
            BikeSeriesEntityBase objSeries = null;
            uint seriesId = 0;
            try
            {
                objSeries = _models.GetSeriesByModelId(_modelId);
                if (objSeries != null && objSeries.SeriesId > 0)
                {
                    seriesId = objSeries.SeriesId;
                    IEnumerable<BikeSeriesEntity> makeSeriesList = _bikeSeries.GetMakeSeries(makeId, _cityId);
                    if (makeSeriesList != null && makeSeriesList.Any())
                    {
                        seriesInfo = makeSeriesList.FirstOrDefault(s => s.SeriesId == seriesId);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikeInfoWidget.BindSeriesData(ModelId : {0} , CityId : {1} , SeriesId : {2})", _modelId, _cityId, seriesId));
            }
            return seriesInfo;
        }
    }

}