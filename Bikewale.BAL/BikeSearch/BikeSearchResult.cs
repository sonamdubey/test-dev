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
        public BikeSearchResult(ISearchResult searchResult)
        {
            _searchResult = searchResult;
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

                objSearchEntity.BudgetLinks = FormatBudgetLinks(_searchResult.GetBudgetRanges(), filterInputs.MinBudget, filterInputs.MaxBudget);

                searchResult = SearchOutputMapper.Convert(objSearchEntity);

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BAL.BikeSearchResult.GetSearchResult");
            }
            return searchResult;
        }

        private IEnumerable<Tuple<string, string, uint>> FormatBudgetLinks(BudgetFilterRanges budgetFilterRanges, string minBudget, string maxBudget)
        {
            ICollection<Tuple<string, string, uint>> urls = new List<Tuple<string, string, uint>>();
            int minIndex = -1, maxIndex = -1;
            uint outValue = 0;
            string zero = "0", _60LPlus = "6000000+", outKey = "";
            try
            {
                var budgets = budgetFilterRanges.Budget;
                if (budgetFilterRanges != null && budgetFilterRanges.Budget != null && budgetFilterRanges.Budget.Count > 0)
                {
                    if (!String.IsNullOrEmpty(minBudget) && budgetFilterRanges.Budget.ContainsKey(minBudget))
                    {
                        minIndex = IndexOf(budgetFilterRanges.Budget, minBudget);
                    }
                    if (!String.IsNullOrEmpty(maxBudget) && budgetFilterRanges.Budget.ContainsKey(maxBudget))
                    {
                        maxIndex = IndexOf(budgetFilterRanges.Budget, maxBudget);
                    }

                    if (minBudget != zero && maxBudget != zero && minIndex > -1 && maxIndex > -1)
                    {
                        urls.Add(FormatSearchUnderBudgetUrls(minBudget, ValueAtIndex(budgets, minIndex)));
                        urls.Add(FormatSearchUnderBudgetUrls(maxBudget, ValueAtIndex(budgets, maxIndex)));
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
                    else if (maxBudget != zero && maxIndex > -1)
                    {
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
                    else if (minBudget != zero && minIndex > -1)
                    {
                        if (TryValueAtIndex(budgets, minIndex + 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                        {
                            uint totalBikes = (uint)budgets.Where(m => m.Key != zero).Sum(m => m.Value);
                            urls.Add(FormatSearchAboveBudgetUrls(outKey, totalBikes - outValue));
                            urls.Add(FormatSearchBetweenBudgetUrls(minBudget, outKey, outValue - ValueAtIndex(budgets, minIndex)));
                            var nextIndex = minIndex + 1;
                            var nextKey = outKey;
                            var nextValue = outValue;
                            if (TryValueAtIndex(budgets, nextIndex + 1, out outKey, out outValue) && outValue > 0 && !String.IsNullOrEmpty(outKey) && !_60LPlus.Equals(outKey))
                            {
                                urls.Add(FormatSearchAboveBudgetUrls(outKey, totalBikes - outValue));
                                urls.Add(FormatSearchBetweenBudgetUrls(outKey, nextKey, outValue - nextValue));
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
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BAL.BikeSearchResult.FormatBudgetLinks");
            }
            return urls;
        }

        private Tuple<string, string, uint> FormatSearchBetweenBudgetUrls(string minBudget, string maxBudget, uint count)
        {
            return new Tuple<string, string, uint>(String.Format("Bikes between Rs. {0} and Rs. {1}", Utility.Format.FormatPrice(minBudget), Utility.Format.FormatPrice(maxBudget)), string.Format("/new/bike-search/bikes-between-{0}-and-{1}/", minBudget, maxBudget), count);
        }

        private Tuple<string, string, uint> FormatSearchUnderBudgetUrls(string minBudget, uint count)
        {
            return new Tuple<string, string, uint>(String.Format("Bikes under Rs. {0}", Utility.Format.FormatPrice(minBudget)), string.Format("/new/bike-search/bikes-under-{0}/", minBudget), count);
        }

        private Tuple<string, string, uint> FormatSearchAboveBudgetUrls(string minBudget, uint count)
        {
            return new Tuple<string, string, uint>(String.Format("Bikes above Rs. {0}", Utility.Format.FormatPrice(minBudget)), string.Format("/new/bike-search/bikes-above-{0}/", minBudget), count);
        }

        private int IndexOf(System.Collections.Generic.IDictionary<string, uint> dictionary, string key)
        {
            int index = 0;
            try
            {
                int size = dictionary.Count;
                foreach (var item in dictionary)
                {
                    if (item.Key.Equals(key))
                    {
                        break;
                    }
                    index++;
                }

            }
            catch (Exception ex)
            {

            }
            return index;
        }

        private uint ValueAtIndex(System.Collections.Generic.IDictionary<string, uint> dictionary, int index)
        {
            int currentIndex = 0;
            uint value = 0;
            int size = dictionary.Count;
            if (index < size)
            {
                foreach (var item in dictionary)
                {
                    if (index == currentIndex)
                    {
                        value += item.Value;
                        break;
                    }
                    currentIndex++;
                    value += item.Value;
                }
            }
            return value;
        }

        private bool TryValueAtIndex(System.Collections.Generic.IDictionary<string, uint> dictionary, int index, out string key, out uint value)
        {
            int currentIndex = 0;
            bool isFound = false;
            key = "";
            value = 0;
            try
            {
                int size = dictionary.Count;

                if (index < size)
                {
                    foreach (var item in dictionary)
                    {
                        if (index == currentIndex)
                        {
                            value += item.Value;
                            key = item.Key;
                            isFound = true;
                            break;
                        }
                        currentIndex++;
                        value += item.Value;
                    }
                }
            }
            catch (Exception)
            {
                if (!isFound)
                {
                    key = "";
                    value = 0;
                }
            }
            return isFound;
        }
    }
}
