using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.NewBikeSearch;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.BAL.BikeSearch
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 16th Nov 2017
    /// Summary : BAL for Bike Search Result
    /// </summary>
    public class BikeSearchResult : IBikeSearchResult
    {
        private readonly ISearchResult _searchResult;
        private readonly IBikeSearchCacheRepository _searchCacheRepo;
        private readonly BudgetFilterRanges _budgetFilterRanges;
        private const string zero = "0", _60LPlus = "6000000+";
        private readonly IBikeSearch _bikeSearch = null;

        private static readonly IList<RangeEntity> mileageRange = new List<RangeEntity>() { new RangeEntity { Min = 70, Max = 0 }, new RangeEntity { Min = 50, Max = 70 }, new RangeEntity { Min = 30, Max = 50 }, new RangeEntity { Min = 0, Max = 30 } };

        private static readonly IList<RangeEntity> displacementRange = new List<RangeEntity>() { new RangeEntity { Min = 0, Max = 110 }, new RangeEntity { Min = 110, Max = 150 }, new RangeEntity { Min = 150, Max = 200 }, new RangeEntity { Min = 200, Max = 250 }, new RangeEntity { Min = 250, Max = 500 }, new RangeEntity { Min = 500, Max = 0 }, new RangeEntity { Min = 110, Max = 125 }, new RangeEntity { Min = 125, Max = 150 } };

        public BikeSearchResult(ISearchResult searchResult, IBikeSearchCacheRepository searchCacheRepo, IBikeSearch bikeSearch)
        {
            _searchResult = searchResult;
            _searchCacheRepo = searchCacheRepo;
            _bikeSearch = bikeSearch;
            if (_searchCacheRepo != null)
            {
                _budgetFilterRanges = _searchCacheRepo.GetBudgetRanges();
            }
        }

        /// <summary>
        /// Created by : Snehal Dange on 13th April 2018
        /// Desc: Method created to map filters to elastic input
        /// </summary>
        /// <param name="filterInputs"></param>
        /// <returns></returns>
        private SearchFilters MapFiltersInputToES(FilterInput filterInputs)
        {
            SearchFilters filtersOutput = null;
            try
            {
                if (filterInputs != null)
                {
                    filtersOutput = new SearchFilters();
                    if (filterInputs.Make != null)
                    {
                        filtersOutput.Make = Array.ConvertAll(filterInputs.Make, uint.Parse);
                    }
                    if (filterInputs.Model != null)
                    {
                        filtersOutput.Model = Array.ConvertAll(filterInputs.Model, uint.Parse);
                    }

                    if (filterInputs.MinBudget != null || filterInputs.MaxBudget != null)
                    {
                        PriceRangeEntity priceRange = new PriceRangeEntity();
                        priceRange.Min = Convert.ToInt32(filterInputs.MinBudget);
                        priceRange.Max = Convert.ToInt32(filterInputs.MaxBudget);
                        IList<PriceRangeEntity> priceList = new List<PriceRangeEntity>() { priceRange };
                        filtersOutput.Price = priceList;

                    }

                    if (filterInputs.Displacement != null)
                    {
                        filtersOutput.Displacement = GetRangeValues(filterInputs.Displacement, displacementRange);
                    }

                    if (filterInputs.Mileage != null)
                    {
                        filtersOutput.Mileage = GetRangeValues(filterInputs.Mileage, mileageRange);
                    }

                    if (filterInputs.RideStyle != null)
                    {
                        filtersOutput.BodyStyle = filterInputs.RideStyle;
                    }
                    if (filterInputs.Brakes != null)
                    {
                        filtersOutput.Brakes = filterInputs.Brakes;
                    }
                    if (filterInputs.Wheels != null)
                    {
                        filtersOutput.Wheels = Array.ConvertAll(filterInputs.Wheels, sbyte.Parse);
                    }
                    if (filterInputs.StartType != null)
                    {
                        filtersOutput.StartType = Array.ConvertAll(filterInputs.StartType, sbyte.Parse);
                    }

                    if (filterInputs.ABSAvailable || filterInputs.ABSNotAvailable)
                    {
                        filtersOutput.ABS = filterInputs.ABSAvailable;
                    }
                    if (filterInputs.PageNo != null && filterInputs.PageSize != null)
                    {
                        filtersOutput.PageNumber = SqlReaderConvertor.ToUInt16(filterInputs.PageNo);
                        filtersOutput.PageSize = SqlReaderConvertor.ToUInt16(filterInputs.PageSize);
                    }
                    if (filterInputs.sc != null && filterInputs.so != null)
                    {
                        filtersOutput.SortCriteria = filterInputs.sc;
                        filtersOutput.SortOrder = filterInputs.so;
                    }
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BAL.BikeSearchResult.MapFiltersInputToES");
            }
            return filtersOutput;
        }

        /// <summary>
        /// Created by : Snehal Dange on 13th April 2018
        /// Description: Get range values from selected ids in UI.
        /// </summary>
        /// <param name="filterInput"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        private IEnumerable<RangeEntity> GetRangeValues(string[] filterInput, IList<RangeEntity> range)
        {
            IList<RangeEntity> rangeList = null;
            try
            {
                if (filterInput != null && filterInput.Any())
                {
                    rangeList = new List<RangeEntity>();
                    RangeEntity rangeObj = null;
                    var parsedArray = Array.ConvertAll(filterInput, int.Parse);
                    var rangeLen = range.Count();
                    if (parsedArray != null)
                    {
                        foreach (var filterIndex in parsedArray)
                        {
                            if (filterIndex <= rangeLen)
                            {
                                rangeObj = range[(filterIndex - 1)];
                                if (rangeObj != null)
                                {
                                    rangeList.Add(rangeObj);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BAL.BikeSearchResult.GetRangeValues");
            }
            return rangeList;
        }




        /// <summary>
        /// Created by : Snehal Dange on 11th April 2018
        /// Description: Map ES output to bike search api output
        /// </summary>
        /// <param name="objEsOutput"></param>
        /// <returns></returns>
        private SearchOutput MapEsOutputToBikeSearchOutput(BikeSearchOutputEntity objEsOutput)
        {
            SearchOutput objSearchOutput = null;
            try
            {
                if (objEsOutput != null)
                {
                    objSearchOutput = new SearchOutput();
                    List<SearchOutputBase> searchOutputList = new List<SearchOutputBase>();
                    IEnumerable<BikeModelDocumentEntity> bikesList = objEsOutput.Bikes;


                    foreach (var bike in bikesList)
                    {
                        SearchOutputBase bikesOutput = new SearchOutputBase();

                        ModelDetail bikemodel = new ModelDetail();
                        MakeBase bikeMakebase = null;

                        if (bike.BikeMake != null)
                        {
                            var bikeMakeObj = bike.BikeMake;
                            bikeMakebase = new MakeBase();
                            bikeMakebase.MakeId = SqlReaderConvertor.ToInt32(bikeMakeObj.MakeId);
                            bikeMakebase.MakeName = bikeMakeObj.MakeName;
                            bikeMakebase.MaskingName = bikeMakeObj.MakeMaskingName;
                            bikemodel.MakeBase = bikeMakebase;
                        }

                        if (bike.BikeModel != null)
                        {
                            var bikeModelObj = bike.BikeModel;
                            bikemodel.ModelId = SqlReaderConvertor.ToInt32(bikeModelObj.ModelId);
                            bikemodel.ModelName = bikeModelObj.ModelName;
                            bikemodel.MaskingName = bikeModelObj.ModelMaskingName;
                        }

                        if (bike.BikeImage != null)
                        {
                            var bikeImageObj = bike.BikeImage;
                            bikemodel.HostUrl = bikeImageObj.HostURL;
                            bikemodel.OriginalImagePath = bikeImageObj.ImageURL;
                        }

                        if (bike.TopVersion != null)
                        {
                            var topversionDetails = bike.TopVersion;
                            if (topversionDetails != null)
                            {
                                var exshowroomPrice = topversionDetails.PriceList.FirstOrDefault(m => m.PriceType == "Exshowroom");
                                if (exshowroomPrice != null)
                                {
                                    bikemodel.MinPrice = exshowroomPrice.PriceValue;
                                }
                                bikesOutput.Displacement = SqlReaderConvertor.ToFloat(topversionDetails.Displacement);

                                bikesOutput.FuelEfficiency = SqlReaderConvertor.ToUInt16(topversionDetails.Mileage);
                                bikesOutput.KerbWeight = SqlReaderConvertor.ToUInt16(topversionDetails.KerbWeight);
                                if (topversionDetails.Power != null)
                                {
                                    bikesOutput.Power = topversionDetails.Power.ToString();
                                }
                                if (topversionDetails.Exshowroom != null)
                                {
                                    bikesOutput.FinalPrice = Format.FormatPrice(topversionDetails.Exshowroom.ToString());
                                }
                            }

                        }

                        bikemodel.RatingCount = SqlReaderConvertor.ToInt32(bike.RatingsCount);
                        bikemodel.ReviewCount = SqlReaderConvertor.ToInt32(bike.UserReviewsCount);
                        bikemodel.ReviewRate = SqlReaderConvertor.ParseToDouble(bike.ReviewRatings.ToString("0.0"));
                        bikemodel.ReviewRateStar = (byte)Math.Round(bike.ReviewRatings);


                        bikesOutput.BikeModel = bikemodel;
                        bikesOutput.BikeName = string.Format("{0} {1}", bikeMakebase.MakeName, bikemodel.ModelName);
                        bikesOutput.AvailableSpecs = FormatMinSpecs.GetMinSpecs(bikesOutput.Displacement.ToString(), bikesOutput.FuelEfficiency.ToString(), bikesOutput.Power.ToString(), bikesOutput.KerbWeight.ToString());



                        searchOutputList.Add(bikesOutput);
                    }
                    objSearchOutput.SearchResult = searchOutputList;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BAL.BikeSearchResult.MapEsOutputToBikeSearchOutput");
            }
            return objSearchOutput;
        }

        private string GetApiUrl(InputBaseEntity filterInputs)
        {
            string apiUrlstr = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(filterInputs.Bike))
                    apiUrlstr += "&Bike=" + filterInputs.Bike.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.BrakeType))
                    apiUrlstr += "&BrakeType=" + filterInputs.BrakeType.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.Budget))
                    apiUrlstr += "&Budget=" + filterInputs.Budget.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.Displacement))
                    apiUrlstr += "&Displacement=" + filterInputs.Displacement.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.Mileage))
                    apiUrlstr += "&Mileage=" + filterInputs.Mileage.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.PageSize))
                    apiUrlstr += "&PageSize=" + filterInputs.PageSize;
                if (!String.IsNullOrEmpty(filterInputs.RideStyle))
                    apiUrlstr += "&RideStyle=" + filterInputs.RideStyle.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.sc))
                    apiUrlstr += "&sc=" + filterInputs.sc;
                if (!String.IsNullOrEmpty(filterInputs.so))
                    apiUrlstr += "&so=" + filterInputs.so;
                if (!String.IsNullOrEmpty(filterInputs.StartType))
                    apiUrlstr += "&StartType=" + filterInputs.StartType.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.AlloyWheel))
                    apiUrlstr += "&AlloyWheel=" + filterInputs.AlloyWheel.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.ABS))
                    apiUrlstr += "&ABS=" + filterInputs.ABS.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.AntiBreakingSystem))
                    apiUrlstr += "&AntiBreakingSystem=" + filterInputs.AntiBreakingSystem.Replace(" ", "+");
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.BikeSearchResult.GetApiUrl");

            }
            return apiUrlstr;
        }

        private Bikewale.DTO.NewBikeSearch.Pager GetPrevNextUrl(FilterInput filterInputs, InputBaseEntity input, int totalRecordCount)
        {
            Bikewale.DTO.NewBikeSearch.Pager objPager = null;
            int totalPageCount = 0;
            try
            {
                objPager = new Bikewale.DTO.NewBikeSearch.Pager();
                string apiUrlStr = GetApiUrl(input);
                totalPageCount = Paging.GetTotalPages(totalRecordCount, Convert.ToInt32(filterInputs.PageSize));

                if (totalPageCount > 0)
                {
                    string controllerurl = "/api/NewBikeSearch/?";

                    if (filterInputs.PageNo == totalPageCount.ToString())
                        objPager.NextPageUrl = string.Empty;
                    else
                        objPager.NextPageUrl = controllerurl + "PageNo=" + (Convert.ToInt32(filterInputs.PageNo) + 1) + apiUrlStr;

                    if (filterInputs.PageNo == "1")
                        objPager.PrevPageUrl = string.Empty;
                    else
                        objPager.PrevPageUrl = controllerurl + "PageNo=" + (Convert.ToInt32(filterInputs.PageNo) - 1) + apiUrlStr;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.BikeSearchResult.GetPrevNextUrl");

            }
            return objPager;
        }


        /// <summary>
        /// Created by : Vivek Singh Tomar on 16th Nov 2017
        /// Summary : Fetch bikes for given input query
        /// Modified by : Snehal Dange on 11th April 2018
        /// Description: Modified Data fetch from DB TO ES
        /// </summary>
        /// <param name="filterInputs"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public SearchOutput GetSearchResult(FilterInput filterInputs, InputBaseEntity input)
        {
            SearchOutput objSearchOutput = null;
            SearchFilters filtersInputES = null;
            BikeSearchOutputEntity objEsOutput = null;
            try
            {
                if (filterInputs != null)
                {
                    filtersInputES = MapFiltersInputToES(filterInputs);
                    if (filtersInputES != null)
                    {
                        objEsOutput = _bikeSearch.GetBikeSearch(filtersInputES);
                        if (objEsOutput != null)
                        {
                            objSearchOutput = MapEsOutputToBikeSearchOutput(objEsOutput);

                            if (objSearchOutput != null)
                            {
                                objSearchOutput.PageUrl = GetPrevNextUrl(filterInputs, input, objEsOutput.TotalCount);
                                objSearchOutput.TotalCount = objEsOutput.TotalCount;
                                objSearchOutput.CurrentPageNo = Convert.ToInt32(filterInputs.PageNo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BAL.BikeSearchResult.GetSearchResult");
            }
            return objSearchOutput;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Search by between budget links
        /// It returns list of Tuple representing Search bike by budget link
        /// Link
        ///     item1 as Link Title
        ///     items2 as Link Url
        ///     item3 as Bikes count
        /// </summary>
        /// <param name="minBudget">min budget</param>
        /// <param name="maxBudget">max budget</param>
        /// <returns></returns>
        public IEnumerable<Tuple<string, string, string, uint>> SearchBudgetLinksBetween(string minBudget, string maxBudget)
        {
            IEnumerable<Tuple<string, string, string, uint>> budgetUrls = null;
            try
            {
                if (_budgetFilterRanges != null && _budgetFilterRanges.Budget != null && _budgetFilterRanges.Budget.Any())
                {
                    if (!String.IsNullOrEmpty(maxBudget) && !String.IsNullOrEmpty(minBudget) && minBudget != zero && maxBudget != zero)
                    {
                        ICollection<Tuple<string, string, string, uint>> urls = null;
                        int minIndex = -1, maxIndex = -1;
                        var budgets = _budgetFilterRanges.Budget;
                        if (!String.IsNullOrEmpty(minBudget) && budgets.ContainsKey(minBudget))
                        {
                            minIndex = CollectionHelper.IndexOf(budgets, minBudget);
                        }
                        if (!String.IsNullOrEmpty(maxBudget) && budgets.ContainsKey(maxBudget))
                        {
                            maxIndex = CollectionHelper.IndexOf(budgets, maxBudget);
                        }

                        if (minIndex > -1 && maxIndex > -1)
                        {
                            urls = new List<Tuple<string, string, string, uint>>();

                            SortedDictionary<uint, Tuple<string, string, string, uint>> sortedBudgets = new SortedDictionary<uint, Tuple<string, string, string, uint>>();

                            sortedBudgets.Add(1, FormatSearchUnderBudgetUrls(minBudget, CollectionHelper.ValueAtIndex(budgets, minIndex)));
                            sortedBudgets.Add(2, FormatSearchUnderBudgetUrls(maxBudget, CollectionHelper.ValueAtIndex(budgets, maxIndex)));

                            uint outValue = 0;
                            string outKey = "";
                            if (CollectionHelper.TryValueAtIndex(budgets, minIndex - 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                            {
                                sortedBudgets.Add(0, FormatSearchUnderBudgetUrls(outKey, CollectionHelper.ValueAtIndex(budgets, minIndex - 1)));
                                sortedBudgets.Add(4, FormatSearchBetweenBudgetUrls(outKey, minBudget, (CollectionHelper.ValueAtIndex(budgets, minIndex) - CollectionHelper.ValueAtIndex(budgets, minIndex - 1))));
                                var prevIndex = minIndex - 1;
                                var prevKey = outKey;
                                var prevValue = outValue;
                                if (CollectionHelper.TryValueAtIndex(budgets, prevIndex - 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                                {
                                    sortedBudgets.Add(3, FormatSearchBetweenBudgetUrls(outKey, prevKey, (prevValue - outValue)));
                                }

                            }

                            if (CollectionHelper.TryValueAtIndex(budgets, maxIndex + 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                            {
                                sortedBudgets.Add(5, FormatSearchBetweenBudgetUrls(maxBudget, outKey, (outValue - CollectionHelper.ValueAtIndex(budgets, maxIndex))));
                            }
                            budgetUrls = sortedBudgets.Values.ToList();
                        }
                    }
                    else if (maxBudget != zero && !String.IsNullOrEmpty(maxBudget))
                    {
                        budgetUrls = SearchBudgetLinksUnder(maxBudget);
                    }
                    else if (minBudget != zero && !String.IsNullOrEmpty(minBudget))
                    {
                        budgetUrls = SearchBudgetLinksAbove(minBudget);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BAL.BikeSearchResult.FormatBudgetLinks");
            }
            return budgetUrls;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Search by above budget links
        /// It returns list of Tuple representing Search bike by budget link
        /// Link
        ///     item1 as Link Title
        ///     items2 as Link Url
        ///     item3 as Bikes count
        /// </summary>
        /// <param name="minBudget"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<string, string, string, uint>> SearchBudgetLinksAbove(string minBudget)
        {
            uint outValue = 0;
            string outKey = "";

            int minIndex = -1;
            var budgets = _budgetFilterRanges.Budget;
            IEnumerable<Tuple<string, string, string, uint>> urls = null;
            try
            {
                if (_budgetFilterRanges != null && _budgetFilterRanges.Budget != null && _budgetFilterRanges.Budget.Any())
                {
                    if (!String.IsNullOrEmpty(minBudget) && budgets.ContainsKey(minBudget))
                    {
                        minIndex = CollectionHelper.IndexOf(budgets, minBudget);
                    }

                    if (minIndex > -1)
                    {
                        SortedDictionary<uint, Tuple<string, string, string, uint>> sortedBudgets = new SortedDictionary<uint, Tuple<string, string, string, uint>>();
                        if (CollectionHelper.TryValueAtIndex(budgets, minIndex + 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                        {
                            uint totalBikes = _budgetFilterRanges.BikesCount;
                            sortedBudgets.Add(1, FormatSearchAboveBudgetUrls(outKey, totalBikes - outValue));
                            sortedBudgets.Add(4, FormatSearchBetweenBudgetUrls(minBudget, outKey, outValue - CollectionHelper.ValueAtIndex(budgets, minIndex)));
                            var nextIndex = minIndex + 1;
                            var nextKey = outKey;
                            var nextValue = outValue;
                            if (CollectionHelper.TryValueAtIndex(budgets, nextIndex + 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                            {
                                sortedBudgets.Add(2, FormatSearchAboveBudgetUrls(outKey, totalBikes - outValue));
                                sortedBudgets.Add(5, FormatSearchBetweenBudgetUrls(nextKey, outKey, outValue - nextValue));
                            }
                        }
                        if (CollectionHelper.TryValueAtIndex(budgets, minIndex - 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                        {
                            sortedBudgets.Add(0, FormatSearchAboveBudgetUrls(outKey, outValue));
                            sortedBudgets.Add(3, FormatSearchBetweenBudgetUrls(outKey, minBudget, CollectionHelper.ValueAtIndex(budgets, minIndex) - outValue));
                        }
                        urls = sortedBudgets.Values.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BAL.BikeSearchResult.SearchBudgetLinksAbove");
            }
            return urls;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Search by under budget links
        /// It returns list of Tuple representing Search bike by budget link
        /// Link
        ///     item1 as Link Text
        ///     item2 as Link Title
        ///     items3 as Link Url
        ///     item4 as Bikes count
        /// </summary>
        /// <param name="maxBudget"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<string, string, string, uint>> SearchBudgetLinksUnder(string maxBudget)
        {
            uint outValue = 0;
            string outKey = "";
            int maxIndex = -1;
            var budgets = _budgetFilterRanges.Budget;
            IEnumerable<Tuple<string, string, string, uint>> urls = null;
            try
            {
                if (_budgetFilterRanges != null && _budgetFilterRanges.Budget != null && _budgetFilterRanges.Budget.Any())
                {
                    if (!String.IsNullOrEmpty(maxBudget) && budgets.ContainsKey(maxBudget))
                    {
                        maxIndex = CollectionHelper.IndexOf(budgets, maxBudget);
                    }
                    if (maxIndex > -1)
                    {
                        SortedDictionary<uint, Tuple<string, string, string, uint>> sortedBudgets = new SortedDictionary<uint, Tuple<string, string, string, uint>>();
                        if (CollectionHelper.TryValueAtIndex(budgets, maxIndex - 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                        {
                            sortedBudgets.Add(1, FormatSearchUnderBudgetUrls(outKey, outValue));
                            sortedBudgets.Add(4, FormatSearchBetweenBudgetUrls(outKey, maxBudget, CollectionHelper.ValueAtIndex(budgets, maxIndex) - outValue));
                            var prevIndex = maxIndex - 1;
                            var prevKey = outKey;
                            var prevValue = outValue;
                            if (CollectionHelper.TryValueAtIndex(budgets, prevIndex - 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                            {
                                sortedBudgets.Add(3, FormatSearchBetweenBudgetUrls(outKey, prevKey, prevValue - outValue));
                            }
                        }
                        if (CollectionHelper.TryValueAtIndex(budgets, maxIndex - 2, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                        {
                            sortedBudgets.Add(0, FormatSearchUnderBudgetUrls(outKey, outValue));
                        }
                        if (CollectionHelper.TryValueAtIndex(budgets, maxIndex + 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                        {
                            sortedBudgets.Add(2, FormatSearchUnderBudgetUrls(outKey, outValue));
                            sortedBudgets.Add(5, FormatSearchBetweenBudgetUrls(maxBudget, outKey, outValue - CollectionHelper.ValueAtIndex(budgets, maxIndex)));
                        }
                        urls = sortedBudgets.Values.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BAL.BikeSearchResult.SearchBudgetLinksUnder");
            }
            return urls;
        }

        #region Private Helper Method

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Returns new bike budget url for min and max
        /// </summary>
        /// <param name="minBudget"></param>
        /// <param name="maxBudget"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private Tuple<string, string, string, uint> FormatSearchBetweenBudgetUrls(string minBudget, string maxBudget, uint count)
        {
            return new Tuple<string, string, string, uint>(String.Format("Bikes between <span>&#x20B9;</span> {0} and <span>&#x20B9;</span> {1}", Utility.Format.FormatPrice(minBudget), Utility.Format.FormatPrice(maxBudget)), String.Format("Bikes between &#x20B9; {0} and &#x20B9; {1}", Utility.Format.FormatPrice(minBudget), Utility.Format.FormatPrice(maxBudget)), string.Format("/new/bike-search/bikes-between-{0}-and-{1}/", minBudget, maxBudget), count);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Returns new bike budget url for min
        /// </summary>
        /// <param name="minBudget"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private Tuple<string, string, string, uint> FormatSearchUnderBudgetUrls(string minBudget, uint count)
        {
            return new Tuple<string, string, string, uint>(String.Format("Bikes under <span>&#x20B9;</span> {0}", Utility.Format.FormatPrice(minBudget)), String.Format("Bikes under &#x20B9; {0}", Utility.Format.FormatPrice(minBudget)), string.Format("/new/bike-search/bikes-under-{0}/", minBudget), count);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Returns new bike budget url for max
        /// </summary>
        /// <param name="minBudget"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private Tuple<string, string, string, uint> FormatSearchAboveBudgetUrls(string minBudget, uint count)
        {
            return new Tuple<string, string, string, uint>(String.Format("Bikes above <span>&#x20B9;</span> {0}", Utility.Format.FormatPrice(minBudget)), String.Format("Bikes above &#x20B9; {0}", Utility.Format.FormatPrice(minBudget)), string.Format("/new/bike-search/bikes-above-{0}/", minBudget), count);
        }
        #endregion
    }
}
