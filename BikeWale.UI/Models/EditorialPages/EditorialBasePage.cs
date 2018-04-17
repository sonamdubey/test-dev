﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Models.BikeModels;
using Bikewale.Models.Shared;
using Bikewale.Notifications;
using Bikewale.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using Bikewale;
using Bikewale.Interfaces.BikeData;
using Bikewale.Models.BikeMakes;
using Bikewale.Interfaces.BikeData.UpComing;

namespace Bikewale.Models.EditorialPages
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 17 April 2018
    /// Description : Class which holds all the functionality related to widgets on Editorial Pages (namely `News`, `Expert Reviews`, `Features` and `Bike Care`)
    /// </summary>
    public class EditorialBasePage
    {

        private bool IsMobile;
        private bool IsModelTagged;
        private bool IsMakeTagged;
        private bool IsMakeLive;
        private string MakeName, MakeMaskingName;
        private uint SeriesId;
        private string SeriesName, SeriesMaskingName;
        private EnumBikeBodyStyles BodyStyle;
        private bool IsSeriesAvailable;
        private bool IsScooterOnlyMake;
        private uint MakeId, ModelId, CityId;

        private IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets = null;
        private IBikeMakesCacheRepository _bikeMakesCacheRepository = null;
        private IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private IBikeModelsCacheRepository<int> _models = null;
        private IUpcoming _upcoming = null;
        private readonly IBikeSeries _series = null;

        public EditorialBasePage(IBikeMakesCacheRepository bikeMakesCacheRepository, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeSeries series)
        {
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            _models = models;
            _bikeModels = bikeModels;
            _series = series;
            _upcoming = upcoming;
        }

        
        public void SetAdditionalGenericVariables(bool isMobile, uint makeId, string makeName, string makeMaskingName, bool isMakeLive, uint modelId, uint seriesId, string seriesName, string seriesMaskingName, uint cityId, bool isSeriesAvailable, bool isScooterOnlyMake, EnumBikeBodyStyles bodyStyle)
        {
            IsMobile = isMobile;

            MakeId = makeId;
            MakeName = makeName;
            MakeMaskingName = makeMaskingName;
            IsMakeLive = isMakeLive;
            IsMakeTagged = MakeId > 0;

            ModelId = modelId;
            IsModelTagged = ModelId > 0;

            SeriesId = seriesId;
            SeriesName = seriesName;
            SeriesMaskingName = seriesMaskingName;

            CityId = cityId;
            IsSeriesAvailable = isSeriesAvailable;
            IsScooterOnlyMake = isScooterOnlyMake;
            BodyStyle = bodyStyle;
        }
        /// <summary>
        /// Created by  : Sanskar Gupta on 17 April 2018
        /// Description : Function returning Editorial Widget data `PageWidgets` 
        /// </summary>
        /// <param name="pageType">Type of the Page for which Editorial which data has to be returned. (e.g. `Listing`/`Detail` etc.)</param>
        /// <returns></returns>
        public IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> GetEditorialWidgetData(EnumEditorialPageType pageType)
        {
            try
            {

                switch (pageType)
                {
                    case EnumEditorialPageType.Listing:
                        break;
                    case EnumEditorialPageType.Detail:
                        SetEditorialDetailPageWidgets();
                        break;
                    default:
                        //_logger.Info(string.Format("Bikewale.Models.EditorialPages.EditorialWidgets.GetEditorialWidgetData()__{0} is not a valid PageType", pageType));
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.EditorialPages.EditorialWidgets.GetEditorialWidgetData()"));
            }
            return PageWidgets;
        }


        /// <summary>
        /// Created By  : Deepak Israni, Sanskar Gupta on 09 April 2018
        /// Description : Function to Set Editorial Widget Data. Added the conditionals to Bind the widgets using this function.
        /// </summary>
        /// <param name="objData">VM of the page.</param>
        private void SetEditorialDetailPageWidgets()
        {
            if (IsModelTagged)
            {
                if (BodyStyle == EnumBikeBodyStyles.Cruiser || BodyStyle == EnumBikeBodyStyles.Sports)
                {
                    // Model is Tagged, Body Style is either Cruiser or Sports
                    PageWidgets = GetWidgetDataForModelCruiserSports();
                }
                else if (BodyStyle == EnumBikeBodyStyles.Scooter)
                {
                    // Model is Tagged, Body Style is Scooter
                    PageWidgets = GetWidgetDataForModelScooter();
                }
                else
                {
                    // Model is Tagged, Body Style is something other than Cruiser/Sports/Scooter
                    PageWidgets = GetWidgetDataForModelExceptCruiserSportsScooter();
                }
            }
            else
            {
                // Model is Not Tagged
                PageWidgets = GetWidgetDataForMake();
            }
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to Set Editorial widget data for Model-News and Body Styles except Cruiser/Sports/Scooter.
        /// </summary>
        private IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> GetWidgetDataForModelExceptCruiserSportsScooter()
        {
            EditorialWidgetVM FirstWidget = new EditorialWidgetVM();
            FirstWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            EditorialWidgetVM SecondWidget = new EditorialWidgetVM();
            SecondWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            try
            {
                FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = IsSeriesAvailable ? BindWidget(EditorialWidgetCategory.Popular_Series) : BindWidget(EditorialWidgetCategory.Popular_Make);
                SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_All);
                if (!IsMobile)
                {
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.Upcoming_All);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWale.UI.Models.News.NewsDetailPage.SetWidgetDataForModelExceptCruiserSportsScooter"));
            }
            IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets = new Dictionary<EditorialPageWidgetPosition, EditorialWidgetVM>();
            PageWidgets.Add(EditorialPageWidgetPosition.First, FirstWidget);
            PageWidgets.Add(EditorialPageWidgetPosition.Second, SecondWidget);
            return PageWidgets;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to Set Editorial widget data for Model-News and Body Style Scooter.
        /// </summary>
        /// <param name="objData">VM of the page.</param>
        /// <param name="widgets">Array of widgets to be populated (4 items for now)</param>
        private IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> GetWidgetDataForModelScooter()
        {
            EditorialWidgetVM FirstWidget = new EditorialWidgetVM();
            FirstWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            EditorialWidgetVM SecondWidget = new EditorialWidgetVM();
            SecondWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            try
            {
                FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = IsSeriesAvailable ? BindWidget(EditorialWidgetCategory.Series_Scooters) : BindWidget(EditorialWidgetCategory.Popular_Make_Scooters);
                if (!IsMobile)
                {
                    FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.OtherBrands_All);
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_Scooters);
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.Upcoming_Scooters);
                }
                else
                {
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.OtherBrands_All);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWale.UI.Models.News.NewsDetailPage.SetWidgetDataForModelScooter"));
            }
            IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets = new Dictionary<EditorialPageWidgetPosition, EditorialWidgetVM>();
            PageWidgets.Add(EditorialPageWidgetPosition.First, FirstWidget);
            PageWidgets.Add(EditorialPageWidgetPosition.Second, SecondWidget);
            return PageWidgets;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to Set Editorial widget data for Model-News and Body Styles Cruiser/Sports.
        /// </summary>
        private IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> GetWidgetDataForModelCruiserSports()
        {
            EditorialWidgetVM FirstWidget = new EditorialWidgetVM();
            FirstWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            EditorialWidgetVM SecondWidget = new EditorialWidgetVM();
            SecondWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            try
            {
                FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = IsSeriesAvailable ? BindWidget(EditorialWidgetCategory.Popular_Series) : BindWidget(EditorialWidgetCategory.Popular_Make);
                if (!IsMobile)
                {
                    FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.Popular_BodyStyle);
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_All);
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.Upcoming_All);
                }
                else
                {
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_BodyStyle);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWale.UI.Models.News.NewsDetailPage.SetWidgetDataForModelCruiserSports"));
            }
            IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets = new Dictionary<EditorialPageWidgetPosition, EditorialWidgetVM>();
            PageWidgets.Add(EditorialPageWidgetPosition.First, FirstWidget);
            PageWidgets.Add(EditorialPageWidgetPosition.Second, SecondWidget);
            return PageWidgets;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to Set Editorial widget data for Make-News page.
        /// </summary>
        private IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> GetWidgetDataForMake()
        {
            EditorialWidgetVM FirstWidget = new EditorialWidgetVM();
            FirstWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            EditorialWidgetVM SecondWidget = new EditorialWidgetVM();
            SecondWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            try
            {
                if (IsMakeTagged && IsMakeLive)
                {
                    FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_Make);
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_All);
                    if (!IsMobile)
                    {
                        SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.Upcoming_All);
                    }
                }
                else
                {
                    FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_All);
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Upcoming_All);
                    if (!IsMobile)
                    {
                        FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.Popular_Scooters);
                        SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.Upcoming_Scooters);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWale.UI.Models.News.NewsDetailPage.SetWidgetDataForMakeNews"));
            }
            IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets = new Dictionary<EditorialPageWidgetPosition, EditorialWidgetVM>();
            PageWidgets.Add(EditorialPageWidgetPosition.First, FirstWidget);
            PageWidgets.Add(EditorialPageWidgetPosition.Second, SecondWidget);
            return PageWidgets;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta, Deepak Israni on 09 April 2018
        /// Description : Function to Bind Editorial Widget. Pass the `category` of the bike data to be bound and the VM object.
        /// </summary>
        /// <param name="category">The category Enum of the bike list to be bound.</param>
        /// <returns></returns>
        private EditorialWidgetInfo BindWidget(EditorialWidgetCategory category)
        {
            EditorialWidgetInfo widget = null;
            try
            {
                int editorialWidgetTopCount = IsMobile ? 9 : 6;
                widget = InitializeWidgetCategory(category);
                string viewAllTitle = string.Empty;

                switch (category)
                {
                    case EditorialWidgetCategory.Popular_All:
                        {
                            var mostPopular = _models.GetMostPopularBikesbyMakeCity((uint)editorialWidgetTopCount, 0, CityId);
                            mostPopular = SetAdPromotedBikes(mostPopular, editorialWidgetTopCount);
                            if (mostPopular == null || !mostPopular.Any())
                            {
                                return null;
                            }
                            ((EditorialPopularBikesWidget)widget).MostPopularBikeList = mostPopular;
                            SetWidgetStructureData(widget, "Popular Bikes", "PopularBikes", true, UrlFormatter.FormatGenericPageUrl(EnumBikeBodyStyles.AllBikes), "View all bikes", "View all bikes");
                        }
                        break;

                    case EditorialWidgetCategory.Popular_Make:
                        {
                            var mostPopular = _models.GetMostPopularBikesbyMakeCity((uint)editorialWidgetTopCount, MakeId, CityId);
                            if (mostPopular == null || !mostPopular.Any())
                            {
                                return null;
                            }
                            ((EditorialPopularBikesWidget)widget).MostPopularBikeList = mostPopular;
                            SetWidgetStructureData(widget, string.Format("{0} Bikes", MakeName), "PopularMakeBikes", true, UrlFormatter.BikeMakeUrl(MakeMaskingName), "View all bikes", "View all bikes");
                        }
                        break;

                    case EditorialWidgetCategory.Popular_BodyStyle:
                        {
                            var mostPopular = _models.GetPopularBikesByBodyStyle((ushort)BodyStyle, (uint)editorialWidgetTopCount, CityId);
                            if (mostPopular == null || !mostPopular.Any())
                            {
                                return null;
                            }
                            ((EditorialPopularBikesWidget)widget).MostPopularBikeList = mostPopular;
                            string title = null, tabId = null;
                            if (BodyStyle == EnumBikeBodyStyles.Sports)
                            {
                                title = "Sports Bikes";
                                viewAllTitle = "View all Sports bikes";
                                tabId = "PopularSportsBikes";
                            }
                            else if (BodyStyle == EnumBikeBodyStyles.Cruiser)
                            {
                                title = "Cruisers";
                                viewAllTitle = "View all Cruisers";
                                tabId = "PopularCruisers";
                            }
                            else
                            {
                                //_logger.Info(string.Format("BikeWale.UI.Models.News.NewsDetailPage.BindWidget__{0}. Body style : {1}, is invalid.", category, _bodyStyle));
                                return null;
                            }
                            SetWidgetStructureData(widget, title, tabId, true, UrlFormatter.FormatGenericPageUrl(BodyStyle), "View all bikes", viewAllTitle);
                        }
                        break;

                    case EditorialWidgetCategory.Popular_Scooters:
                        {
                            var mostPopular = _models.GetMostPopularScooters((uint)editorialWidgetTopCount, CityId);
                            if (mostPopular == null || !mostPopular.Any())
                            {
                                return null;
                            }
                            ((EditorialPopularBikesWidget)widget).MostPopularBikeList = mostPopular;
                            SetWidgetStructureData(widget, "Popular Scooters", "PopularScooters", true, UrlFormatter.FormatGenericPageUrl(BodyStyle), "View all scooters", "View all scooters");
                        }
                        break;

                    case EditorialWidgetCategory.Popular_Make_Scooters:
                        {
                            var mostPopular = _models.GetMostPopularScooters((uint)editorialWidgetTopCount, MakeId, CityId);
                            if (mostPopular == null || !mostPopular.Any())
                            {
                                return null;
                            }
                            ((EditorialPopularBikesWidget)widget).MostPopularBikeList = mostPopular;
                            SetWidgetStructureData(widget, string.Format("{0} Scooters", MakeName), "PopularMakeScooters", true, UrlFormatter.ScooterMakeUrl(MakeMaskingName, IsScooterOnlyMake), "View all scooters", "View all scooters");
                        }
                        break;

                    case EditorialWidgetCategory.Popular_Series:
                        {
                            var mostPopular = GetTopElements(FetchPopularSeriesBikes(SeriesId), editorialWidgetTopCount);
                            if (mostPopular == null || !mostPopular.Any())
                            {
                                return null;
                            }
                            ((EditorialPopularBikesWidget)widget).MostPopularBikeList = mostPopular;
                            viewAllTitle = string.Format("View all {0} bikes", SeriesName);
                            SetWidgetStructureData(widget, string.Format("{0} Bikes", SeriesName), "PopularSeriesBikes", true, UrlFormatter.BikeSeriesUrl(MakeMaskingName, SeriesMaskingName), viewAllTitle, viewAllTitle);
                        }
                        break;

                    case EditorialWidgetCategory.Series_Scooters:
                        {
                            var mostPopular = GetTopElements(FetchPopularSeriesBikes(SeriesId), editorialWidgetTopCount);
                            if (mostPopular == null || !mostPopular.Any())
                            {
                                return null;
                            }
                            ((EditorialPopularBikesWidget)widget).MostPopularBikeList = mostPopular;
                            viewAllTitle = string.Format("View all {0} Scooters", SeriesName);
                            SetWidgetStructureData(widget, string.Format("{0} Scooters", SeriesName), "PopularSeriesScooters", true, UrlFormatter.BikeSeriesUrl(MakeMaskingName, SeriesMaskingName), viewAllTitle, viewAllTitle);
                        }
                        break;

                    case EditorialWidgetCategory.Upcoming_All:
                        {
                            var upcomingBikes = _models.GetUpcomingBikesList(EnumUpcomingBikesFilter.Default, editorialWidgetTopCount);
                            if (upcomingBikes == null || !upcomingBikes.Any())
                            {
                                return null;
                            }
                            ((EditorialUpcomingBikesWidget)widget).UpcomingBikeList = upcomingBikes;
                            viewAllTitle = "View all upcoming bikes";
                            SetWidgetStructureData(widget, "Upcoming Bikes", "UpcomingBikes", true, UrlFormatter.UpcomingBikesUrl(), viewAllTitle, viewAllTitle);
                        }
                        break;

                    case EditorialWidgetCategory.Upcoming_Scooters:
                        {
                            var upcomingBikes = GetUpcomingScooters(editorialWidgetTopCount);
                            if (upcomingBikes == null || !upcomingBikes.Any())
                            {
                                return null;
                            }
                            ((EditorialUpcomingBikesWidget)widget).UpcomingBikeList = upcomingBikes;
                            SetWidgetStructureData(widget, "Upcoming Scooters", "UpcomingScooters", false);
                        }
                        break;

                    case EditorialWidgetCategory.OtherBrands_All:
                        {
                            var otherBrands = GetOtherScooterBrands((int)MakeId, editorialWidgetTopCount);
                            if (otherBrands == null || !otherBrands.Any())
                            {
                                return null;
                            }
                            ((EditorialOtherBrandsWidget)widget).OtherBrandsList = otherBrands;
                            viewAllTitle = "View other brands";
                            SetWidgetStructureData(widget, "Other Brands", "OtherBrands", true, UrlFormatter.AllScootersUrl(), viewAllTitle, viewAllTitle);
                        }
                        break;

                    default:
                        //_logger.Info(string.Format("BikeWale.UI.Models.News.NewsDetailPage.BindWidget__{0} is not a valid category", category));
                        break;
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWale.UI.Models.News.NewsDetailPage.BindWidget__Category: {0}", category));
            }
            return widget;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to Initialize the Editorial Widget categories. 
        /// </summary>
        private EditorialWidgetInfo InitializeWidgetCategory(EditorialWidgetCategory category)
        {
            EditorialWidgetInfo widget = null;
            switch (category)
            {
                case EditorialWidgetCategory.Popular_All:
                case EditorialWidgetCategory.Popular_Make:
                case EditorialWidgetCategory.Popular_Make_Scooters:
                case EditorialWidgetCategory.Popular_BodyStyle:
                case EditorialWidgetCategory.Popular_Scooters:
                case EditorialWidgetCategory.Popular_Series:
                case EditorialWidgetCategory.Series_Scooters:
                    widget = new EditorialPopularBikesWidget();
                    break;
                case EditorialWidgetCategory.Upcoming_All:
                case EditorialWidgetCategory.Upcoming_Make:
                case EditorialWidgetCategory.Upcoming_Scooters:
                    widget = new EditorialUpcomingBikesWidget();
                    break;
                case EditorialWidgetCategory.OtherBrands_All:
                    widget = new EditorialOtherBrandsWidget();
                    break;
                default:
                    return widget;
            }
            return widget;
        }

        /// <summary>
        /// Created By  : Deepak Israni on 12 April 2018
        /// Description : Function to Get Scooters of other brands.
        /// </summary>
        /// <param name="skipMakeId">MakeId of the scooter to be skipped</param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        private IEnumerable<BikeMakeEntityBase> GetOtherScooterBrands(int skipMakeId, int topCount)
        {
            IEnumerable<BikeMakeEntityBase> bikeList = null;
            try
            {
                bikeList = _bikeMakesCacheRepository.GetScooterMakes();

                if (bikeList != null && skipMakeId > 0 && bikeList.Any())
                {
                    bikeList = bikeList.Where(x => x.MakeId != skipMakeId);
                }

                if (bikeList != null && topCount > 0 && bikeList.Count() > topCount)
                {
                    bikeList = bikeList.Take(topCount);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "NewsDetailPage.GetOtherScooterBrands()");
            }
            return bikeList;
        }

        /// <summary>
        /// Created By : Deepak Israni on 9 April 2018
        /// Description: Function to get list of Upcoming scooters.
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        private IEnumerable<UpcomingBikeEntity> GetUpcomingScooters(int topCount)
        {
            IEnumerable<UpcomingBikeEntity> UpcomingScooters = null;

            UpcomingBikesListInputEntity Filters = new UpcomingBikesListInputEntity()
            {
                PageNo = 1,
                PageSize = topCount,
                BodyStyleId = (uint)EnumBikeBodyStyles.Scooter
            };

            UpcomingScooters = _upcoming.GetModels(Filters, EnumUpcomingBikesFilter.Default);

            return UpcomingScooters;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 11 April 2018
        /// Description : Function to set the status and position of a `Newly Launched` bike 
        /// </summary>
        private IEnumerable<MostPopularBikesBase> SetAdPromotedBikes(IEnumerable<MostPopularBikesBase> PopularBikesList, int topCount)
        {
            if (PopularBikesList == null)
            {
                return null;
            }
            BikeFilters obj = new BikeFilters();
            obj.CityId = CityId;
            IEnumerable<MostPopularBikesBase> promotedBikes = _bikeModels.GetAdPromotedBike(obj, true);
            return _bikeModels.GetAdPromoteBikeFilters(promotedBikes, PopularBikesList).Take(topCount);
        }


        /// <summary>
        /// Created By  : Sanskar Gupta on 11 April 2018
        /// Description : Function to set Miscellaneous data for a widget.
        /// </summary>
        /// <param name="widget">The object containing data for that widget</param>
        /// <param name="title">String for Title of the Widget</param>
        /// <param name="tabId">String for TabId of the Widget</param>
        /// <param name="showViewAll">Boolean denoting whether `View All` link to be shown at the bottom of the widget.</param>
        /// <param name="viewAllUrl">Url where the user is redirected when the `View All` button is clicked</param>
        /// <param name="viewAllText">String for Text of `View All` button</param>
        /// <param name="viewAllTitle">Link title as well as the button text of the view all link/button</param>
        private void SetWidgetStructureData(EditorialWidgetInfo widget, string title, string tabId, bool showViewAll, string viewAllUrl = null, string viewAllText = null, string viewAllTitle = null)
        {
            try
            {
                widget.Title = title;
                widget.TabId = tabId;
                widget.ShowViewAll = showViewAll;

                if (widget.ShowViewAll)
                {
                    widget.ViewAllUrl = viewAllUrl;
                    widget.ViewAllText = viewAllText;
                    widget.ViewAllTitle = viewAllTitle;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWale.UI.Models.News.NewsDetailPage.SetWidgetMiscData__WidgetTitle: {0}", title));
            }
        }

        /// <summary>
        /// Fetches the popular series bikes.
        /// Modified by : Sanskar Gupta on 16 April 2018
        /// Description : Added null check for `popularSeriesBikes`
        /// </summary>
        /// <param name="seriesId">The series identifier.</param>
        /// <returns></returns>
        private IEnumerable<MostPopularBikesBase> FetchPopularSeriesBikes(uint seriesId)
        {
            IEnumerable<MostPopularBikesBase> popularSeriesBikes = null;
            try
            {
                popularSeriesBikes = _models.GetMostPopularBikesByMakeWithCityPrice((int)MakeId, CityId);
                if (popularSeriesBikes == null)
                {
                    return null;
                }
                string modelIds = string.Empty;
                if (seriesId > 0)
                {
                    modelIds = _series.GetModelIdsBySeries(seriesId);
                }
                string[] modelArray = modelIds.Split(',');
                if (modelArray.Length > 0)
                {
                    popularSeriesBikes = (from bike in popularSeriesBikes
                                          where modelArray.Contains(bike.objModel.ModelId.ToString())
                                          select bike
                                         );
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.NewsIndexPage.FetchPopularBikes");
            }
            return popularSeriesBikes;
        }


        /// <summary>
        /// Created by  : Sanskar Gupta on 16 April 2018
        /// Description : Function to get first `noOfElements` elements from an IEnumerable<T> with null check handled in the same.
        /// </summary>
        /// <param name="elements">IEnumerable of the items</param>
        /// <param name="topCount">noOfElements to be taken out of the list.</param>
        /// <returns></returns>
        private IEnumerable<T> GetTopElements<T>(IEnumerable<T> elements, int noOfElements)
        {
            if (elements == null)
            {
                return null;
            }
            return elements.Take(noOfElements);
        }


    }
}
