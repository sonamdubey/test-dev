using Bikewale.DAL.CoreDAL;
using Bikewale.Entities.AutoComplete;
using Bikewale.Interfaces.AutoComplete;
using Bikewale.Notifications;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BAL.AutoComplete
{
    public class AutoSuggest : IAutoSuggest
    {
        private static readonly string _allMakeModel = "AllMakeModel",
            _priceQuoteMakeModel = "PriceQuoteMakeModel",
            _allCity = "AllCity",
            _areaPinCodes = "AreaPinCodes",
            _userReviews = "UserReviews",
            _pinCodeCapitalFirst = "AreaPinCodes",
            _nonUpcomingBikes = "NonUpcomingBikes",
            _bikeImages = "BikeImages";

        #region GetAutoSuggestResult PopulateWhere
        /// <summary>
        /// Created By : Sadhana Upadhyay on 26 Aug 2015
        /// Summary : To get Auto suggested result from elastic index
        /// Modified by :   Sumit Kate on 05 Jan 2016
        /// Description :   Added below condition
        /// if (_result!=null && _result.Suggestions!=null && _result.Suggestions.ContainsKey(completion_field))
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="noOfRecords"></param>
        /// <returns></returns>
        public IEnumerable<SuggestOption<T>> GetAutoSuggestResult<T>(string inputText, int noOfRecords, AutoSuggestEnum source) where T : class
        {
            IEnumerable<SuggestOption<T>> suggestionList = null;
            string indexName = string.Empty;
            try
            {
                suggestionList = GetSuggestionList<T>(inputText, noOfRecords, source);
                if (suggestionList != null)
                {
                    suggestionList = suggestionList.Take(noOfRecords);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.ElasticSearchManger.GetAutoSuggestResult");
            }
            return suggestionList;

        }

        private IEnumerable<SuggestOption<T>> GetSuggestionList<T>(string inputText, int noOfRecords, AutoSuggestEnum source) where T : class
        {
            IEnumerable<SuggestOption<T>> suggestionList = null;
            string completion_field = "mm_suggest";
            string indexName = string.Empty;
            try
            {
                indexName = GetIndexName(source);
                ElasticClient client = ElasticSearchInstance.GetInstance();

                if (client != null)
                {
                    var context = GetContext(source);
                    Func<SearchDescriptor<T>, SearchDescriptor<T>> selectorWithContext = null;
                    Func<SuggestContextQueriesDescriptor<T>, IPromise<IDictionary<string, IList<ISuggestContextQuery>>>> contentDict = null;

                    contentDict = new Func<SuggestContextQueriesDescriptor<T>, IPromise<IDictionary<string, IList<ISuggestContextQuery>>>>
                    (
                        cc => cc.Context("types",
                        context.Select<string, Func<SuggestContextQueryDescriptor<T>, ISuggestContextQuery>>(
                            v => cd => cd.Context(v)
                            ).ToArray()));

                    selectorWithContext = new Func<SearchDescriptor<T>, SearchDescriptor<T>>
                        (sd => sd.Index(indexName)
                        .Suggest(
                            s => s.Completion(
                                completion_field,
                                c => c.Field(completion_field)
                                    .Prefix(inputText)
                                    .Contexts(contentDict)
                                    .Size(50)) // Send size as 50 since suggetion query doesn't return all count. Do not modify this till ES bug is fixed
                                    ));


                    ISearchResponse<T> _result = client.Search<T>(selectorWithContext);

                    if (_result.Suggest[completion_field][0].Options.Count <= 0 && source != AutoSuggestEnum.PinCodeCapitalFirst)
                    {
                        selectorWithContext = new Func<SearchDescriptor<T>, SearchDescriptor<T>>
                            (sd => sd.Index(indexName)
                            .Suggest(
                                s => s.Completion(
                                    completion_field,
                                    c => c.Field(completion_field)
                                .Fuzzy(
                                        ff => ff.MinLength(2)
                                        .PrefixLength(0)
                                        .Fuzziness(Fuzziness.EditDistance(1))
                                        )
                                        .Prefix(inputText)
                                        .Size(noOfRecords)
                                        .Contexts(contentDict))
                                        ));

                        _result = client.Search<T>(selectorWithContext);
                    }
                    if (_result != null && _result.Suggest != null)
                    {
                        suggestionList = _result.Suggest[completion_field][0].Options;
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.ElasticSearchManger.GetSuggestionList");
            }

            return suggestionList;
        }


        private string GetIndexName(AutoSuggestEnum source)
        {
            string indexName = string.Empty;
            switch (source)
            {
                case AutoSuggestEnum.AllMakeModel:
                case AutoSuggestEnum.PriceQuoteMakeModel:
                case AutoSuggestEnum.UserReviews:
                case AutoSuggestEnum.NonUpcomingBikes:
                case AutoSuggestEnum.BikeImages:
                    indexName = Utility.BWConfiguration.Instance.MMindexName;
                    break;
                case AutoSuggestEnum.AllCity:
                    indexName = Utility.BWConfiguration.Instance.CityIndexName;
                    break;
                case AutoSuggestEnum.AreaPinCodes:
                    indexName = Utility.BWConfiguration.Instance.PinCodesIndexName;
                    break;
                case AutoSuggestEnum.PinCodeCapitalFirst:
                    indexName = Utility.BWConfiguration.Instance.CapitalFirstPinCode;
                    break;
                default:
                    indexName = Utility.BWConfiguration.Instance.MMindexName;
                    break;
            }
            return indexName;
        }
        #endregion
        /// <summary>
        /// Modified By : Rajan Chauhan on 09 Jan 2018
        /// Description : Added BikeImages context
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private IEnumerable<string> GetContext(AutoSuggestEnum source)
        {
            IList<string> indexName = new List<string>();
            switch (source)
            {
                case AutoSuggestEnum.AllMakeModel:
                    indexName.Add(_allMakeModel);
                    break;
                case AutoSuggestEnum.PriceQuoteMakeModel:
                    indexName.Add(_priceQuoteMakeModel);
                    break;
                case AutoSuggestEnum.UserReviews:
                    indexName.Add(_userReviews);
                    break;
                case AutoSuggestEnum.NonUpcomingBikes:
                    indexName.Add(_nonUpcomingBikes);
                    break;
                case AutoSuggestEnum.AllCity:
                    indexName.Add(_allCity);
                    break;
                case AutoSuggestEnum.AreaPinCodes:
                    indexName.Add(_areaPinCodes);
                    break;
                case AutoSuggestEnum.PinCodeCapitalFirst:
                    indexName.Add(_pinCodeCapitalFirst);
                    break;
                case AutoSuggestEnum.BikeImages:
                    indexName.Add(_bikeImages);
                    break;
                default:
                    indexName.Add(_allMakeModel);
                    break;
            }
            return indexName;

        }

    }   //End of class
}   //End of namespace
