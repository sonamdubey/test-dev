using Bikewale.DTO.NewBikeSearch;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
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
        public BikeSearchResult(ISearchResult searchResult, IBikeSearchCacheRepository searchCacheRepo)
        {
            _searchResult = searchResult;
            _searchCacheRepo = searchCacheRepo;
            if (_searchCacheRepo != null)
            {
                _budgetFilterRanges = _searchCacheRepo.GetBudgetRanges();
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 16th Nov 2017
        /// Summary : Fetch bikes for given input query
        /// </summary>
        /// <param name="filterInputs"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public SearchOutput GetSearchResult(FilterInput filterInputs, InputBaseEntity input)
        {
            SearchOutput searchResult = null;
            try
            {
                SearchOutputEntity objSearchEntity = _searchResult.GetSearchResult(filterInputs, input);

                objSearchEntity.BudgetLinks = SearchBudgetLinksBetween(filterInputs.MinBudget, filterInputs.MaxBudget);

                searchResult = SearchOutputMapper.Convert(objSearchEntity);

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BAL.BikeSearchResult.GetSearchResult");
            }
            return searchResult;
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
                            minIndex = IndexOf(budgets, minBudget);
                        }
                        if (!String.IsNullOrEmpty(maxBudget) && budgets.ContainsKey(maxBudget))
                        {
                            maxIndex = IndexOf(budgets, maxBudget);
                        }

                        if (minIndex > -1 && maxIndex > -1)
                        {
                            urls = new List<Tuple<string, string, string, uint>>();
                            urls.Add(FormatSearchUnderBudgetUrls(minBudget, ValueAtIndex(budgets, minIndex)));
                            urls.Add(FormatSearchUnderBudgetUrls(maxBudget, ValueAtIndex(budgets, maxIndex)));
                            uint outValue = 0;
                            string outKey = "";
                            if (TryValueAtIndex(budgets, minIndex - 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                            {
                                urls.Add(FormatSearchUnderBudgetUrls(outKey, ValueAtIndex(budgets, minIndex - 1)));
                                urls.Add(FormatSearchBetweenBudgetUrls(outKey, minBudget, (ValueAtIndex(budgets, minIndex) - ValueAtIndex(budgets, minIndex - 1))));
                                var prevIndex = minIndex - 1;
                                var prevKey = outKey;
                                var prevValue = outValue;
                                if (TryValueAtIndex(budgets, prevIndex - 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                                {
                                    urls.Add(FormatSearchBetweenBudgetUrls(outKey, prevKey, (prevValue - outValue)));
                                }

                            }

                            if (TryValueAtIndex(budgets, maxIndex + 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                            {
                                urls.Add(FormatSearchBetweenBudgetUrls(maxBudget, outKey, (outValue - ValueAtIndex(budgets, maxIndex))));
                            }
                        }

                        budgetUrls = urls;
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
            ICollection<Tuple<string, string, string, uint>> urls = null;
            try
            {
                if (_budgetFilterRanges != null && _budgetFilterRanges.Budget != null && _budgetFilterRanges.Budget.Any())
                {
                    if (!String.IsNullOrEmpty(minBudget) && budgets.ContainsKey(minBudget))
                    {
                        minIndex = IndexOf(budgets, minBudget);
                    }

                    if (minIndex > -1)
                    {
                        urls = new List<Tuple<string, string, string, uint>>();
                        if (TryValueAtIndex(budgets, minIndex + 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                        {
                            uint totalBikes = _budgetFilterRanges.BikesCount;
                            urls.Add(FormatSearchAboveBudgetUrls(outKey, totalBikes - outValue));
                            urls.Add(FormatSearchBetweenBudgetUrls(minBudget, outKey, outValue - ValueAtIndex(budgets, minIndex)));
                            var nextIndex = minIndex + 1;
                            var nextKey = outKey;
                            var nextValue = outValue;
                            if (TryValueAtIndex(budgets, nextIndex + 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                            {
                                urls.Add(FormatSearchAboveBudgetUrls(outKey, totalBikes - outValue));
                                urls.Add(FormatSearchBetweenBudgetUrls(nextKey, outKey, outValue - nextValue));
                            }
                        }
                        if (TryValueAtIndex(budgets, minIndex - 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                        {
                            urls.Add(FormatSearchUnderBudgetUrls(outKey, outValue));
                            urls.Add(FormatSearchBetweenBudgetUrls(outKey, minBudget, ValueAtIndex(budgets, minIndex) - outValue));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

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
            ICollection<Tuple<string, string, string, uint>> urls = null;
            if (_budgetFilterRanges != null && _budgetFilterRanges.Budget != null && _budgetFilterRanges.Budget.Any())
            {
                if (!String.IsNullOrEmpty(maxBudget) && budgets.ContainsKey(maxBudget))
                {
                    maxIndex = IndexOf(budgets, maxBudget);
                }
                if (maxIndex > -1)
                {
                    urls = new List<Tuple<string, string, string, uint>>();
                    if (TryValueAtIndex(budgets, maxIndex - 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                    {
                        urls.Add(FormatSearchUnderBudgetUrls(outKey, outValue));
                        urls.Add(FormatSearchBetweenBudgetUrls(outKey, maxBudget, ValueAtIndex(budgets, maxIndex) - outValue));
                        var prevIndex = maxIndex - 1;
                        var prevKey = outKey;
                        var prevValue = outValue;
                        if (TryValueAtIndex(budgets, prevIndex - 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                        {
                            urls.Add(FormatSearchBetweenBudgetUrls(outKey, prevKey, prevValue - outValue));
                        }
                    }
                    if (TryValueAtIndex(budgets, maxIndex - 2, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                    {
                        urls.Add(FormatSearchUnderBudgetUrls(outKey, outValue));
                    }
                    if (TryValueAtIndex(budgets, maxIndex + 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                    {
                        urls.Add(FormatSearchUnderBudgetUrls(outKey, outValue));
                        urls.Add(FormatSearchBetweenBudgetUrls(maxBudget, outKey, outValue - ValueAtIndex(budgets, maxIndex)));
                    }
                }
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

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Returns index of key in given dictionary of type <string,uint>
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private int IndexOf<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, TKey key)
        {
            int index = 0;
            try
            {
                int size = dictionary.Count();
                foreach (var item in dictionary)
                {
                    if (item.Key.Equals(key))
                    {
                        break;
                    }
                    index++;
                }

            }
            catch (Exception)
            {

            }
            return index;
        }

        /// <summary>
        /// Craeted by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Returns Value present at index in given dictionary of type <string,uint>
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private TValue ValueAtIndex<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, int index)
        {
            int currentIndex = 0;
            TValue value = default(TValue);
            int size = dictionary.Count();
            if (index < size)
            {
                foreach (var item in dictionary)
                {
                    if (index == currentIndex)
                    {
                        value = item.Value;
                        break;
                    }
                    currentIndex++;
                }
            }
            return value;
        }

        /// <summary>
        /// Craeted by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Try to get key and value in given dictionary of type <string,uint>
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool TryValueAtIndex<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, int index, out TKey key, out TValue value)
        {
            int currentIndex = 0;
            bool isFound = false;
            key = default(TKey);
            value = default(TValue);
            try
            {
                int size = dictionary.Count();

                if (index < size)
                {
                    foreach (var item in dictionary)
                    {
                        if (index == currentIndex)
                        {
                            value = item.Value;
                            key = item.Key;
                            isFound = true;
                            break;
                        }
                        currentIndex++;
                    }
                }
            }
            catch (Exception)
            {
                if (!isFound)
                {
                    key = default(TKey);
                    value = default(TValue);
                }
            }
            return isFound;
        }
        #endregion
    }
}
