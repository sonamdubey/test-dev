using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using Bikewale.CoreDAL;
using System.Data.SqlClient;
using System.Data;
using Bikewale.Entities.BikeData;
using Bikewale.Utility;
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

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if(dr!=null)
                        {
                            objSearchList = new List<SearchOutputEntityBase>();

                            while(dr.Read())
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
                                objList.BikeModel.ReviewCount = Convert.ToInt32(dr["MoReviewCount"]);
                                objList.BikeModel.ReviewRate = Convert.ToDouble(dr["MoReviewRate"]);
                                objList.FinalPrice = Format.FormatPrice(dr["MinPrice"].ToString());
                                objList.AvailableSpecs = FormatMinSpecs.GetMinSpecs(dr["Displacement"].ToString(), dr["FuelEfficiencyOverall"].ToString(), dr["Power"].ToString());

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
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.DAL.NewBikeSearch.SearchResult.GetSearchResult");
                objErr.SendMail();
            }
            return objSearch;
        }

        private PagingUrl GetPrevNextUrl(FilterInput filterInputs, InputBaseEntity input)
        {
            PagingUrl objPager = null;
            int totalPageCount = 0;
            try
            {
                objPager=new PagingUrl();
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
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.DAL.NewBikeSearch.SearchResult.GetPrevNextUrl");
                objErr.SendMail();
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
                if (!String.IsNullOrEmpty(filterInputs.AntiBreakingSystem))
                    apiUrlstr += "&AntiBreakingSystem=" + filterInputs.AntiBreakingSystem.Replace(" ", "+");
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.DAL.NewBikeSearch.SearchResult.GetApiUrl");
                objErr.SendMail();
            }
            return apiUrlstr;
        }
    }
}
