using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.NewBikeSearch
{
    public class SearchResult : ISearchResult
    {
        int totalRecordCount = 0;
        ISearchQuery objSearchQuery = null;
        public SearchResult()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ISearchQuery, SearchQuery>()
                    .RegisterType<IProcessFilter, ProcessFilter>();

                objSearchQuery = container.Resolve<ISearchQuery>();

            }
        }


        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jan 2018
        /// Description :   returns Budgets ranges with bike count
        /// Modified by :   Sanjay George on 18 July 2018
        /// Description :   change CommandType to StoredProcedure
        /// </summary>
        /// <returns></returns>
        public BudgetFilterRanges GetBudgetRanges()
        {
            BudgetFilterRanges ranges = null;
            try
            {
                ranges = new BudgetFilterRanges();
                ranges.Budget = new System.Collections.Generic.Dictionary<string, uint>();
                ranges.Budget.Add("30000", 0);
                ranges.Budget.Add("40000", 0);
                ranges.Budget.Add("50000", 0);
                ranges.Budget.Add("60000", 0);
                ranges.Budget.Add("70000", 0);
                ranges.Budget.Add("80000", 0);
                ranges.Budget.Add("90000", 0);
                ranges.Budget.Add("100000", 0);
                ranges.Budget.Add("150000", 0);
                ranges.Budget.Add("200000", 0);
                ranges.Budget.Add("250000", 0);
                ranges.Budget.Add("300000", 0);
                ranges.Budget.Add("350000", 0);
                ranges.Budget.Add("500000", 0);
                ranges.Budget.Add("750000", 0);
                ranges.Budget.Add("1250000", 0);
                ranges.Budget.Add("1500000", 0);
                ranges.Budget.Add("3000000", 0);
                ranges.Budget.Add("6000000", 0);
                ranges.Budget.Add("6000000+", 0);

                using (DbCommand cmd = DbFactory.GetDBCommand("getnewbikesearchbudget"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                if (Convert.ToString(dr["Range"]) != "0")
                                    ranges.Budget[dr["Range"].ToString()] = SqlReaderConvertor.ToUInt32(dr["Bikes"]);
                            }
                            dr.Close();
                        }
                    }
                }

                int validRanges = ranges.Budget.Count - 1;
                uint currentCount = 0;
                int index = 0;
                var budgets = new System.Collections.Generic.Dictionary<string, uint>();
                foreach (var item in ranges.Budget)
                {
                    if (index < validRanges)
                    {
                        currentCount += item.Value;
                        budgets.Add(item.Key, currentCount);
                    }
                    else
                    {
                        budgets.Add(item.Key, item.Value);
                    }
                    index++;
                }
                ranges.Budget = budgets;
                ranges.BikesCount = budgets["6000000"] + budgets["6000000+"];
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.NewBikeSearch.SearchResult.GetBudgetRanges");
            }
            return ranges;
        }
    }
}
