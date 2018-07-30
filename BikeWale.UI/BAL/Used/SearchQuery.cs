using Bikewale.Entities.Used.Search;
using Bikewale.Interfaces.Used.Search;
using Bikewale.Notifications;
using System;

namespace Bikewale.BAL.Used.Search
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 10 sept 2016
    /// Summary : Class to generate the used bikes search query
    /// </summary>
    public class SearchQuery : ISearchQuery
    {
        ISearchFilters _searchFilters = null;

        ProcessedInputFilters filterInputs;
        string whereClause = string.Empty;

        /// <summary>
        /// Passing the dependency through constructor so that filters can be applied to the query
        /// </summary>
        /// <param name="filterInputs"></param>
        public SearchQuery(ISearchFilters searchFilters)
        {
            _searchFilters = searchFilters;
        }

        /// <summary>
        /// Function to get the used bikes search query with all applied filters
        /// </summary>
        /// <param name="filterInputs">all raw filters from the user</param>
        /// <returns></returns>
        public string GetSearchResultQuery(InputFilters inputFilters)
        {
            string searchQuery = string.Empty;

            InitSearchQuery(inputFilters);

            try
            {
                searchQuery = string.Format(@" select sql_calc_found_rows {0}
                                                from {1} 
                                                {2}
                                                order by {3} limit {4},{5};
                                            
                                                select found_rows() as RecordCount;"
                                            , GetSelectClause(), GetFromClause(), GetWhereClause(), GetOrderByClause(), filterInputs.StartIndex - 1, filterInputs.PageSize);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.SearchQuery.GetSearchResultQuery");
                
            }

            return searchQuery;
        }

        /// <summary>
        /// Function to initialize the where clause and get the processed filters data to create a query
        /// </summary>
        /// <param name="inputFilters">raw input filters</param>
        private void InitSearchQuery(InputFilters inputFilters)
        {
            try
            {
                this.filterInputs = _searchFilters.ProcessFilters(inputFilters);

                // Do not change the sequence
                ApplyCityFilter();

                ApplyBikeFilter();

                ApplyBudgetFilter();

                ApplyKilometersFilter();

                ApplyAgeFilter();

                ApplyOwnersFilter();

                ApplySellerTypeFilter();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.SearchQuery.InitSearchQuery");
                
            }
        }

        /// <summary>
        /// Function to get the cities clause
        /// </summary>
        private void ApplyCityFilter()
        {
            try
            {
                if (filterInputs.CityId > 0)
                    whereClause += " ll.cityid = " + filterInputs.CityId;
                else
                    whereClause += " ll.cityid is not null ";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.SearchQuery.ApplyCityFilter");
                
            }
        }

        /// <summary>
        /// Function to get the make  and model filter clause
        /// </summary>
        private void ApplyBikeFilter()
        {
            string makeList = string.Empty, modelList = string.Empty, makeFilter = string.Empty, modelFilter = string.Empty;

            try
            {
                if (filterInputs.Make != null && filterInputs.Make.Length > 0)
                {
                    foreach (string str in filterInputs.Make)
                    {
                        makeList += str + ",";
                    }

                    makeList = makeList.Substring(0, makeList.Length - 1);

                    makeFilter = string.Format(" ll.makeid in ({0}) ", makeList);
                }

                if (filterInputs.Model != null && filterInputs.Model.Length > 0)
                {
                    foreach (string str in filterInputs.Model)
                    {
                        modelList += str + ",";
                    }

                    modelList = modelList.Substring(0, modelList.Length - 1);

                    modelFilter = string.Format(" ll.modelid in ({0}) ", modelList);
                }

                if (!String.IsNullOrEmpty(makeFilter) && !String.IsNullOrEmpty(modelFilter))
                    whereClause += string.Format(" and ( {0} or {1} ) ", makeFilter, modelFilter);
                else if (!String.IsNullOrEmpty(makeFilter))
                    whereClause += string.Format(" and {0} ", makeFilter);
                else if (!String.IsNullOrEmpty(modelFilter))
                    whereClause += string.Format(" and {0} ", modelFilter);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.SearchQuery.ApplyBikeFilter");
                
            }
        }

        /// <summary>
        ///  Function to get the model filter clause
        /// </summary>
        private void ApplyModelFilter()
        {
            string modelList = string.Empty;

            try
            {
                if (filterInputs.Model != null && filterInputs.Model.Length > 0)
                {
                    foreach (string str in filterInputs.Model)
                    {
                        modelList += str + ",";
                    }

                    modelList = modelList.Substring(0, modelList.Length - 1);

                    if (filterInputs.Make != null && filterInputs.Make.Length > 0)
                        whereClause += " or ";
                    else
                        whereClause += " and ";

                    whereClause += string.Format(" ll.modelid in ({0}) ", modelList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.SearchQuery.ApplyModelFilter");
                
            }
        }

        /// <summary>
        /// Function to get the budget filter clause
        /// </summary>
        private void ApplyBudgetFilter()
        {
            string budget = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(filterInputs.MaxBudget) && !String.IsNullOrEmpty(filterInputs.MinBudget))
                {
                    if (filterInputs.MaxBudget == "0")
                    {
                        if (filterInputs.MinBudget != "0")
                        {
                            whereClause += string.Format(" and ll.price > {0} ", filterInputs.MinBudget);
                        }
                        else
                        {
                            whereClause += string.Format(" and ll.price between {0} and {1} ", filterInputs.MinBudget, filterInputs.MaxBudget);
                        }
                    }
                    else
                        whereClause += string.Format(" and ll.price between {0} and {1} ", filterInputs.MinBudget, filterInputs.MaxBudget);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.SearchQuery.ApplyBudgetFilter");
                
            }
        }

        /// <summary>
        /// Function to get the kilometers filter in the query
        /// </summary>
        private void ApplyKilometersFilter()
        {
            try
            {
                if (!String.IsNullOrEmpty(filterInputs.Kms))
                    whereClause += string.Format(" and ll.kilometers < {0} ", filterInputs.Kms);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.SearchQuery.ApplyKilometersFilter");
                
            }
        }

        /// <summary>
        /// Function to apply the bike age filter
        /// </summary>
        private void ApplyAgeFilter()
        {
            try
            {
                if (!String.IsNullOrEmpty(filterInputs.Age))
                    whereClause += string.Format(" and  year (ll.makeyear) >= (year (now()) - {0}) ", filterInputs.Age);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.SearchQuery.ApplyAgeFilter");
                
            }
        }

        /// <summary>
        /// Function to apply the number of owners filter to the query
        /// </summary>
        private void ApplyOwnersFilter()
        {
            string owners = string.Empty;

            try
            {
                if (filterInputs.Owners != null && filterInputs.Owners.Length > 0)
                {
                    foreach (string str in filterInputs.Owners)
                    {
                        owners += str + ",";
                    }

                    owners = owners.Substring(0, owners.Length - 1);

                    whereClause += string.Format(" and ll.owner in ({0}) ", owners);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.SearchQuery.ApplyOwnersFilter");
                
            }
        }

        /// <summary>
        /// Function to get the seller types filter in the query
        /// </summary>
        private void ApplySellerTypeFilter()
        {
            string sellers = string.Empty;

            try
            {
                if (filterInputs.SellerTypes != null && filterInputs.SellerTypes.Length > 0)
                {
                    foreach (string str in filterInputs.SellerTypes)
                    {
                        sellers += str + ",";
                    }

                    sellers = sellers.Substring(0, sellers.Length - 1);

                    whereClause += string.Format(" and ll.sellertype in ({0}) ", sellers);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.SearchQuery.ApplySellerTypeFilter");
                
            }
        }

        /// <summary>
        /// Function to get the select clause for the search query
        /// Modified by :   Sumit Kate on 25 Oct 2016
        /// Description :   Added LastUpdated in select clause
        /// </summary>
        /// <returns></returns>
        private string GetSelectClause()
        {
            string selectClause = string.Empty;

            try
            {
                selectClause = @" inquiryid, profileid    
                                ,ll.makename,ll.modelname, ll.versionname, ll.versionid as bikeversionid, 
                                vs.makemaskingname as makemaskingname,vs.modelmaskingname as modelmaskingname,
                                ll.cityname city, lc.CityMaskingName as citymaskingname, ll.cityid,
	                            ll.seller, ll.sellertype, ll.owner,ifnull(ll.price, 0) as price, ll.kilometers,
	                            year(ll.makeyear) bikeyear, monthname(ll.makeyear) bikemonth,
	                            ifnull(ll.photocount, 0) as photocount , 
	                            ll.hosturl, ll.originalimagepath,
                                ll.color, ll.LastUpdated ";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.SearchQuery.GetSelectClause");
                
            }

            return selectClause;
        }

        /// <summary>
        /// Function to get the from clause
        /// </summary>
        /// <returns></returns>
        private string GetFromClause()
        {
            string fromClause = string.Empty;

            try
            {
                fromClause = @" livelistings as ll  
                        inner join bikeversions vs on vs.id = ll.versionid
                        inner join cities lc on lc.id = ll.cityid ";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.SearchQuery.GetFromClause");
                
            }

            return fromClause;
        }

        /// <summary>
        /// Function to get the where clause
        /// </summary>
        /// <returns></returns>
        private string GetWhereClause()
        {
            if (!string.IsNullOrEmpty(whereClause))
                whereClause = string.Format(" where {0} ", whereClause);

            return whereClause;
        }

        /// <summary>
        /// Function to get the order by clause
        /// Modified by :   Sumit Kate on 25 Oct 2016
        /// Description :   For Default sort, show listing with images of that day first followed by listing without image
        /// </summary>
        /// <returns></returns>
        private string GetOrderByClause()
        {
            string orderBy = string.Empty;

            try
            {
                switch (filterInputs.SortOrder)
                {
                    case 1:
                        orderBy = " ll.LastUpdated desc "; //most recent
                        break;
                    case 2:
                        orderBy = " ll.Price asc "; //low to high
                        break;

                    case 3:
                        orderBy = " ll.Price desc "; //high to low
                        break;

                    case 4:
                        orderBy = " ll.kilometers asc ";
                        break;

                    case 5:
                        orderBy = " ll.kilometers desc ";
                        break;
                    default:
                        orderBy = " date(ll.LastUpdated) desc, case when photocount > 0 then photocount end desc ";
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetOrderByClause");
                
            }
            return orderBy;
        }

    }   // class
}   // namespace