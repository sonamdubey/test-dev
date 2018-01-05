using Bikewale.Entities.BikeData;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
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
        /// Modified by : Sajal Gupta on 02-01-2017
        /// Desc : Read LastUpdatedModelSold, NewsCount from Db
        /// Modified by : Ashutosh Sharma on 12-Aug-2017
        /// Description : Read RatingCount of bikemodel from Db
        /// </summary>
        /// <param name="filterInputs"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public SearchOutputEntity GetSearchResult(FilterInput filterInputs, InputBaseEntity input)
        {
            SearchOutputEntity objSearch = new SearchOutputEntity();

            List<SearchOutputEntityBase> objSearchList = null;

            string sqlStr = string.Empty;
            try
            {
                objSearchQuery.InitSearchCriteria(filterInputs);

                sqlStr = objSearchQuery.GetSearchResultQuery();
                sqlStr += objSearchQuery.GetRecordCountQry();
                using (DbCommand cmd = DbFactory.GetDBCommand(sqlStr))
                {
                    cmd.CommandType = CommandType.Text;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objSearchList = new List<SearchOutputEntityBase>();

                            while (dr.Read())
                            {
                                SearchOutputEntityBase objList = new SearchOutputEntityBase();

                                objList.BikeName = dr["BikeName"].ToString();
                                objList.Displacement = Convert.ToSingle(dr["Displacement"]);
                                objList.FuelEfficiency = Convert.ToUInt16(dr["FuelEfficiencyOverall"]);
                                objList.FuelType = dr["FuelType"].ToString();
                                objList.KerbWeight = Convert.ToUInt16(dr["Weight"]);
                                objList.Power = dr["Power"].ToString();
                                objList.BikeModel = new BikeModelEntity();
                                objList.BikeModel.HostUrl = dr["HostURL"].ToString();
                                objList.BikeModel.OriginalImagePath = dr["ImagePath"].ToString();
                                objList.BikeModel.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objList.BikeModel.ModelName = dr["ModelName"].ToString();
                                objList.BikeModel.MaskingName = dr["ModelMappingName"].ToString();
                                objList.MaximumTorque = Convert.ToSingle(dr["MaximumTorque"]);
                                objList.BikeModel.MakeBase = new BikeMakeEntityBase()
                                {
                                    MakeId = Convert.ToInt32(dr["MakeId"]),
                                    MakeName = dr["MakeName"].ToString(),
                                    MaskingName = dr["MakeMaskingName"].ToString()
                                };
                                objList.BikeModel.MinPrice = Convert.ToInt64(dr["MinPrice"]);
                                objList.BikeModel.MaxPrice = Convert.ToInt64(dr["MaxPrice"]);
                                objList.BikeModel.RatingCount = Convert.ToInt32(dr["MoRatingsCount"]);
                                objList.BikeModel.ReviewCount = Convert.ToInt32(dr["MoReviewCount"]);
                                objList.BikeModel.ReviewRate = Convert.ToDouble(dr["MoReviewRate"]);
                                objList.BikeModel.ReviewRateStar = Convert.ToByte(Math.Round(SqlReaderConvertor.ParseToDouble(dr["MoReviewRate"]), MidpointRounding.AwayFromZero));
                                objList.FinalPrice = Format.FormatPrice(dr["MinPrice"].ToString());
                                objList.AvailableSpecs = FormatMinSpecs.GetMinSpecs(dr["Displacement"].ToString(), dr["FuelEfficiencyOverall"].ToString(), dr["Power"].ToString(), dr["weight"].ToString());
                                objList.SmallDescription = Convert.ToString(dr["smalldescription"]);
                                objList.FullDescription = Convert.ToString(dr["fulldescription"]);
                                objList.UnitsSold = SqlReaderConvertor.ToUInt32(dr["UnitsSold"]);
                                objList.LaunchedDate = SqlReaderConvertor.ToDateTime(dr["launchdate"]);
                                objList.PhotoCount = SqlReaderConvertor.ToUInt32(dr["PhotoCount"]);
                                objList.VideoCount = SqlReaderConvertor.ToUInt32(dr["VideoCount"]);
                                objList.VersionCount = SqlReaderConvertor.ToUInt32(dr["VersionCount"]);
                                objList.ColorCount = SqlReaderConvertor.ToUInt32(dr["ColorCount"]);
                                objList.LastUpdatedModelSold = SqlReaderConvertor.ToDateTime(dr["UnitSoldDate"]);
                                objList.NewsCount = SqlReaderConvertor.ToUInt32(dr["NewsCount"]);
                                objSearchList.Add(objList);
                            }
                            dr.NextResult();
                            if (dr.Read())
                                totalRecordCount = Convert.ToInt32(dr["RecordCount"]);

                            dr.Close();
                        }
                    }

                    objSearch.PageUrl = GetPrevNextUrl(filterInputs, input);
                    objSearch.SearchResult = objSearchList;
                    objSearch.TotalCount = totalRecordCount;
                    objSearch.CurrentPageNo = Convert.ToInt32(filterInputs.PageNo);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.NewBikeSearch.SearchResult.GetSearchResult");

            }
            return objSearch;
        }

        private PagingUrl GetPrevNextUrl(FilterInput filterInputs, InputBaseEntity input)
        {
            PagingUrl objPager = null;
            int totalPageCount = 0;
            try
            {
                objPager = new PagingUrl();
                string apiUrlStr = GetApiUrl(input);
                totalPageCount = Paging.GetTotalPages(totalRecordCount, Convert.ToInt32(filterInputs.PageSize));

                if (totalPageCount > 0)
                {
                    string controllerurl = "/api/NewBikeSearch/?";

                    if (filterInputs.PageNo == totalPageCount.ToString())
                        objPager.NextPageUrl = string.Empty;
                    else
                        objPager.NextPageUrl = controllerurl + "PageNo=" + (Convert.ToInt32(filterInputs.PageNo) + 1) + apiUrlStr;

                    if (filterInputs.PageNo == "1")
                        objPager.PrevPageUrl = string.Empty;
                    else
                        objPager.PrevPageUrl = controllerurl + "PageNo=" + (Convert.ToInt32(filterInputs.PageNo) - 1) + apiUrlStr;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.NewBikeSearch.SearchResult.GetPrevNextUrl");

            }
            return objPager;
        }

        private string GetApiUrl(InputBaseEntity filterInputs)
        {
            string apiUrlstr = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(filterInputs.Bike))
                    apiUrlstr += "&Bike=" + filterInputs.Bike.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.BrakeType))
                    apiUrlstr += "&BrakeType=" + filterInputs.BrakeType.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.Budget))
                    apiUrlstr += "&Budget=" + filterInputs.Budget.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.Displacement))
                    apiUrlstr += "&Displacement=" + filterInputs.Displacement.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.Mileage))
                    apiUrlstr += "&Mileage=" + filterInputs.Mileage.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.PageSize))
                    apiUrlstr += "&PageSize=" + filterInputs.PageSize;
                if (!String.IsNullOrEmpty(filterInputs.RideStyle))
                    apiUrlstr += "&RideStyle=" + filterInputs.RideStyle.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.sc))
                    apiUrlstr += "&sc=" + filterInputs.sc;
                if (!String.IsNullOrEmpty(filterInputs.so))
                    apiUrlstr += "&so=" + filterInputs.so;
                if (!String.IsNullOrEmpty(filterInputs.StartType))
                    apiUrlstr += "&StartType=" + filterInputs.StartType.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.AlloyWheel))
                    apiUrlstr += "&AlloyWheel=" + filterInputs.AlloyWheel.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.ABS))
                    apiUrlstr += "&ABS=" + filterInputs.ABS.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.AntiBreakingSystem))
                    apiUrlstr += "&AntiBreakingSystem=" + filterInputs.AntiBreakingSystem.Replace(" ", "+");
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.NewBikeSearch.SearchResult.GetApiUrl");

            }
            return apiUrlstr;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jan 2018
        /// Description :   returns Budgets ranges with bike count
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
                    cmd.CommandType = CommandType.Text;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                ranges.Budget[dr["Range"].ToString()] = SqlReaderConvertor.ToUInt32(dr["Bikes"]);
                            }
                            dr.Close();
                        }
                    }
                }

                int validRanges = ranges.Budget.Count - 2;
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
