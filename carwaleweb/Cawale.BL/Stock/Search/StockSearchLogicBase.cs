using Carwale.BL.Interface.Stock.Search;
using Carwale.Entity.Classified;
using Carwale.Entity.Stock.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Carwale.BL.Stock.Search
{
    public abstract class StockSearchLogicBase<T> : IStockSearchLogic<T> where T : SearchResultBase
    {
        private const int _defaultPageSize = 24;
        public abstract T Get(FilterInputs filterInputs);
        protected virtual string SessionId
        {
            get { return GetSessionId(); }
        }

        //taking this as string because client entity is sending string
        //we don't want to do conversion now
        protected virtual int GetTotalPageSize(string pageSize)
        {
            return !String.IsNullOrEmpty(pageSize) ? Convert.ToInt32(pageSize) : _defaultPageSize;
        }

        protected virtual string GetExcludedStocksFromResultListings(FilterInputs filterInputs, IEnumerable<StockBaseEntity> listings)
        {
            if (string.IsNullOrEmpty(filterInputs.pn) || filterInputs.pn == "1")
            {
                StringBuilder stocksToBeExcluded = new StringBuilder();
                foreach (var stock in listings)
                {
                    if (!stock.IsPremium)
                    {
                        break;
                    }
                    else
                    {
                        stocksToBeExcluded.AppendFormat("+{0}", stock.ProfileId);
                    }
                }
                return stocksToBeExcluded.Length > 0 ? stocksToBeExcluded.ToString(1, stocksToBeExcluded.Length - 1) : string.Empty;
            }
            return filterInputs.ExcludeStocks?.Replace(' ', '+');
        }

        private static string GetSessionId()
        {
            if (HttpContext.Current.Request.Cookies.AllKeys.Contains("_cwv") && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["_cwv"].Value))
            {
                HttpCookie sessionCookie = HttpContext.Current.Request.Cookies["_cwv"];
                string[] sessionIdValue = sessionCookie.Value.Split('.');
                if (sessionIdValue.Length > 1)
                {
                    return sessionIdValue[2];
                }
            }
            return DateTime.Now.ToString("yyyyddMMhh");
        }
    }
}
