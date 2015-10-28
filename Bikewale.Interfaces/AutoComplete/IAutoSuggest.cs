using Bikewale.Entities.AutoComplete;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.AutoComplete
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 26 Aug 2015 For Auto Complete
    /// </summary>
    public interface IAutoSuggest
    {
        IEnumerable<SuggestOption> GetAutoSuggestResult(string inputText, int noOfRecords,AutoSuggestEnum source);
    }
}
