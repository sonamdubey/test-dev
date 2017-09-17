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

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.BAL.ElasticSearchManger.GetAutoSuggestResult");
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
                                    .Size(noOfRecords))
                                    ));


                    ISearchResponse<T> _result = client.Search<T>(selectorWithContext);

                    if (_result.Suggest[completion_field][0].Options.Count <= 0)
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

                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.BAL.ElasticSearchManger.GetSuggestionList");
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
                    indexName = Utility.BWConfiguration.Instance.MMindexName;
                    break;
                case AutoSuggestEnum.AllCity:
                    indexName = Utility.BWConfiguration.Instance.CityIndexName;
                    break;
                case AutoSuggestEnum.AreaPinCodes:
                    indexName = Utility.BWConfiguration.Instance.PinCodesIndexName;
                    break;
                default:
                    indexName = Utility.BWConfiguration.Instance.MMindexName;
                    break;
            }
            return indexName;
        }
        #endregion

        private IEnumerable<string> GetContext(AutoSuggestEnum source)
        {
            IList<string> indexName = new List<string>();
            switch (source)
            {
                case AutoSuggestEnum.AllMakeModel:
                    indexName.Add("AllMakeModel");
                    break;
                case AutoSuggestEnum.PriceQuoteMakeModel:
                    indexName.Add("PriceQuoteMakeModel");
                    break;
                case AutoSuggestEnum.UserReviews:
                    indexName.Add("UserReviews");
                    break;
                case AutoSuggestEnum.AllCity:
                    indexName.Add("AllCity");
                    break;
                case AutoSuggestEnum.AreaPinCodes:
                    indexName.Add("AreaPinCodes");
                    break;
                default:
                    indexName.Add("AllMakeModel");
                    break;
            }
            return indexName;

        }

    }   //End of class
}   //End of namespace
