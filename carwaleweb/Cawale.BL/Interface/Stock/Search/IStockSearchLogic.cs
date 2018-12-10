using Carwale.Entity.Classified;
using Carwale.Entity.Stock.Search;

namespace Carwale.BL.Interface.Stock.Search
{
    public interface IStockSearchLogic<out TOut> where TOut : SearchResultBase
    { 
        /// <summary>
        /// Get the Stocks for the filter applied
        /// </summary>
        /// <param name="filterInputs"></param>
        /// <returns>Search result</returns>
        TOut Get(FilterInputs filterInputs);
    }
}
