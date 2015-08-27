using Bikewale.Entities.AutoComplete;
using Bikewale.Interfaces.AutoComplete;
using Bikewale.Notifications;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.BAL.AutoComplete
{
    public class AutoSuggest : IAutoSuggest
    {
        #region GetAutoSuggestResult Method
        /// <summary>
        /// Created By : Sadhana Upadhyay on 26 Aug 2015
        /// Summary : To get Auto suggested result from elastic index
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="noOfRecords"></param>
        /// <returns></returns>
        public List<SuggestOption> GetAutoSuggestResult(string inputText, int noOfRecords)
        {
            List<SuggestOption> suggestionList = null;
            string completion_field = "mm_suggest";
            try
            {
                ElasticSearchManager objEs = new ElasticSearchManager(ConfigurationManager.AppSettings["AutoSuggestIndex"]);
                ElasticClient client = objEs.Client;

                ISuggestResponse _result = client.Suggest<SuggestionOutput>(s => s.GlobalText(inputText)
                    .Completion(completion_field, c => c.OnField(completion_field).Size(noOfRecords)
                    ));

                suggestionList = _result.Suggestions[completion_field][0].Options.ToList<Nest.SuggestOption>();
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.BAL.ElasticSearchManger.GetAutoSuggestResult");
                objErr.SendMail();
            }
            return suggestionList;
        }
        #endregion
    }   //End of class
}   //End of namespace
