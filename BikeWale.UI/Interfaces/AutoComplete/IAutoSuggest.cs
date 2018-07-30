using Bikewale.Entities.AutoComplete;
using System.Collections.Generic;


namespace Bikewale.Interfaces.AutoComplete
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 26 Aug 2015 For Auto Complete
    /// </summary>
    public interface IAutoSuggest
    {
        IEnumerable<Nest.SuggestOption<T>> GetAutoSuggestResult<T>(string inputText, int noOfRecords, AutoSuggestEnum source) where T : class;
    }
}
