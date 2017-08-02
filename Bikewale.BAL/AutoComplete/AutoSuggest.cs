﻿using Bikewale.DAL.CoreDAL;
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
        public IEnumerable<SuggestOption> GetAutoSuggestResult(string inputText, int noOfRecords, AutoSuggestEnum source)
        {
            IEnumerable<SuggestOption> suggestionList = null;
            string completion_field = "mm_suggest";
            string indexName = string.Empty;
            try
            {
                indexName = GetIndexName(source);
                ElasticClient client = ElasticSearchInstance.GetInstance();

                ISuggestResponse _result = client.Suggest<SuggestionOutput>(s => s.Index(indexName).GlobalText(inputText).Completion(completion_field, c => c.OnField(completion_field).Size(noOfRecords)));

                if (_result != null && _result.Suggestions != null && _result.Suggestions.ContainsKey(completion_field))
                {
                    if (_result.Suggestions[completion_field][0].Options.Count() > 0)
                        suggestionList = _result.Suggestions[completion_field][0].Options;//.ToList<Nest.SuggestOption>();
                    else
                    {
                        if (!source.Equals(AutoSuggestEnum.AreaPinCodes))
                        {
                            _result = client.Suggest<SuggestionOutput>(s => s.Index(indexName).GlobalText(inputText)
                       .Completion(completion_field, c => c.OnField(completion_field).Fuzzy(ff => ff.MinLength(3).PrefixLength(0).Fuzziness(1)).Size(noOfRecords)
                       ));
                        }
                        else
                        {
                            _result = client.Suggest<SuggestionOutput>(s => s.Index(indexName).GlobalText(inputText)
                     .Completion(completion_field, c => c.OnField(completion_field).Size(noOfRecords)
                     ));
                        }

                        suggestionList = _result.Suggestions[completion_field][0].Options;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.BAL.ElasticSearchManger.GetAutoSuggestResult");
            }
            return suggestionList;
        }

        private string GetIndexName(AutoSuggestEnum source)
        {
            string indexName = string.Empty;
            switch (source)
            {
                case AutoSuggestEnum.AllMakeModel:
                    indexName = Bikewale.Utility.BWConfiguration.Instance.MMindexName;
                    break;
                case AutoSuggestEnum.PriceQuoteMakeModel:
                    indexName = Bikewale.Utility.BWConfiguration.Instance.PQindexName;
                    break;
                case AutoSuggestEnum.AllCity:
                    indexName = Bikewale.Utility.BWConfiguration.Instance.CityIndexName;
                    break;
                case AutoSuggestEnum.AreaPinCodes:
                    indexName = Bikewale.Utility.BWConfiguration.Instance.PinCodesIndexName;
                    break;
                case AutoSuggestEnum.UserReviews:
                    indexName = Bikewale.Utility.BWConfiguration.Instance.UserReviewIndexName;
                    break;
                default:
                    indexName = Bikewale.Utility.BWConfiguration.Instance.MMindexName;
                    break;
            }
            return indexName;
        }
        #endregion
    }   //End of class
}   //End of namespace
