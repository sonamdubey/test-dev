using Carwale.Entity.AutoComplete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.AutoComplete
{
    public interface IAutoCompleteBL
    {
        List<LabelValue> GetResults(string source, string textValue, string Params);
        List<LabelValue> GetLimitedResults(string source, string textValue, string Params, int noOfValues);
    }
}
