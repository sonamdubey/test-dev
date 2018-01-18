using Bikewale.DTO.NewBikeSearch;
using Bikewale.Entities.NewBikeSearch;
using System;
using System.Collections.Generic;

namespace Bikewale.Interfaces.NewBikeSearch
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 16th Nov 2017
    /// Summary : Interface for Bike Search Result
    /// Modified by :   Sumit Kate on 05 Jan 2018
    /// Description :   Added SearchBudgetLinksBetween, SearchBudgetLinksAbove and SearchBudgetLinksUnder
    /// </summary>
    public interface IBikeSearchResult
    {
        SearchOutput GetSearchResult(FilterInput filterInputs, InputBaseEntity input);
        IEnumerable<Tuple<string, string, string, uint>> SearchBudgetLinksBetween(string minBudget, string maxBudget);
        IEnumerable<Tuple<string, string, string, uint>> SearchBudgetLinksAbove(string minBudget);
        IEnumerable<Tuple<string, string, string, uint>> SearchBudgetLinksUnder(string maxBudget);
    }
}
