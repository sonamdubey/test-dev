using Carwale.DAL.CoreDAL;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CompareCars;
using Carwale.Notifications;
using Carwale.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Carwale.DAL.CompareCars
{
    public class CompareCarsRepository : RepositoryBase,ICompareCarsRepository
    {
        private static string ApiHostUrl = string.Empty;
        private static string _imgHostUrl = Carwale.Utility.CWConfiguration._imgHostUrl;
        static CompareCarsRepository()
        {
            ApiHostUrl = "http://" + ConfigurationManager.AppSettings["HostUrl"] ?? "";
        }

        public Tuple<Hashtable, Hashtable> GetSubCategories()
        {
            Hashtable htSpecs = new Hashtable();
            Hashtable htFeatures = new Hashtable();
            List<SubCategoryData> specsList = new List<SubCategoryData>();
            List<SubCategoryData> featuresList = new List<SubCategoryData>();
            try
            {
                using (var con = CarDataMySqlReadConnection)
                {
                    var multi=con.QueryMultiple("GetCategoriesCompareCars_v16_11_7", commandType: CommandType.StoredProcedure);
                    specsList = multi.Read<SubCategoryData>().ToList();
                    featuresList = multi.Read<SubCategoryData>().ToList();
                }
                specsList.ForEach(x =>
                {
                    htSpecs[x.NodeCode.ToString()] = new SubCategoryData()
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name.ToString(),
                        NodeCode = x.NodeCode.ToString(),
                        SortOrder = x.SortOrder.ToString(),
                        Items = new List<Item>()
                    };
                });
                featuresList.ForEach(x =>
                {
                    htFeatures[x.NodeCode.ToString()] = new SubCategoryData()
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name.ToString(),
                        NodeCode = x.NodeCode.ToString(),
                        SortOrder = x.SortOrder.ToString(),
                        Items = new List<Item>()
                    };
                });
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CompareCarsRepository.GetSubCategories()");
                objErr.LogException();
                throw;
            }
            return new Tuple<Hashtable, Hashtable>(htSpecs, htFeatures);
        }

        public Hashtable GetItems()
        {
            Hashtable htItems = new Hashtable();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetItemsCompareCars_v16_11_7"))
                {
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            htItems[dr["ItemMasterId"].ToString()] = new ItemData()
                            {
                                ItemMasterId = dr["ItemMasterId"].ToString(),
                                Name = dr["Name"].ToString(),
                                NodeCode = dr["NodeCode"].ToString(),
                                SortOrder = dr["SortOrder"].ToString(),
                                OverviewSortOrder = dr["OverviewSortOrder"].ToString(),
                                IsOverviewable = Convert.ToBoolean(dr["IsOverviewable"].ToString()),
                                UnitType = dr["UnitType"].ToString(),
                                Values = new List<string>()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CompareCarsRepository.GetItems()");
                objErr.LogException();
                throw;
            }

            return htItems;
        }

        public Tuple<Hashtable, List<Color>, CarWithImageEntity> GetVersionData(int versionId)
        {
            Hashtable htValues = new Hashtable();
            List<Color> lstColors = new List<Color>();
            List<ValueData> values = new List<ValueData>();
            CarWithImageEntity cwie = new CarWithImageEntity();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_versionId", versionId);
                using (var con = CarDataMySqlReadConnection)
                {
                    var multi = con.QueryMultiple("GetVersionData_v17_10_1", param, commandType: CommandType.StoredProcedure);
                    if (multi != null)
                    {
                        cwie = multi.Read<CarWithImageEntity>().FirstOrDefault();
                        values = multi.Read<ValueData>().ToList();
                        lstColors = multi.Read<Color>().ToList();
                    }
                }
                cwie.Image = _imgHostUrl + ImageSizes._210X118 + cwie.OriginalImgPath ?? string.Empty;
                cwie.ImgPath = "";
                cwie.ImageSmall = _imgHostUrl + ImageSizes._110X61 + cwie.OriginalImgPath ?? string.Empty;

                values.ForEach(x => htValues[x.ItemMasterId.ToString()] = new ValueData()
                {
                    ItemMasterId = x.ItemMasterId.ToString(),
                    Value = x.Value.ToString()
                });
                }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarMakesRepository.GetAll()");
                objErr.LogException();
                throw;
            }

            return new Tuple<Hashtable, List<Color>, CarWithImageEntity>(htValues, lstColors, cwie);
        }
        
        public List<HotCarComparison> GetHotComaprisons(short _topCount)
        {
            List<HotCarComparison> _topComparisons = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("GetCompareCarsWidget_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DisplayCount", DbType.Int16, Convert.ToInt16(_topCount)));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                    {
                        _topComparisons = new List<HotCarComparison>();
                        if(dr!=null)
                        {

                            while (dr.Read())
                            {
                                HotCarComparison _car = new HotCarComparison();
                                int vesrionId;
                                float reviewRate;
                                ushort reviewCount;

                                _car.Image = new CarImageBase()
                                {
                                    HostUrl = _imgHostUrl,
                                    ImagePath = dr["OriginalImgPath"].ToString()
                                };

                                _car.HotCars = new List<ComparisonCarModel>();

                                UInt16.TryParse(dr["ReviewCount1"] != DBNull.Value ? dr["ReviewCount1"].ToString() : "", out reviewCount);
                                float.TryParse(dr["ReviewRate1"] != DBNull.Value ? dr["ReviewRate1"].ToString() : "", out reviewRate);
                                Int32.TryParse(dr["VersionId1"] != DBNull.Value ? dr["VersionId1"].ToString() : "", out vesrionId);
                                _car.HotCars.Add(new ComparisonCarModel()
                                {
                                    MakeName = dr["MakeName1"].ToString(),
                                    ModelName = dr["ModelName1"].ToString(),
                                    ModelId = Convert.ToInt32(dr["ModelId1"].ToString()),
                                    MaskingName = dr["MaskingName1"].ToString(),
                                    VersionId = vesrionId,
                                    VersionName = dr["VersionName1"].ToString(),
                                    Review = new CarReviewBase()
                                    {
                                        ReviewCount = reviewCount,
                                        OverallRating = reviewRate,
                                    }
                                });

                                UInt16.TryParse(dr["ReviewCount2"] != DBNull.Value ? dr["ReviewCount2"].ToString() : "", out reviewCount);
                                float.TryParse(dr["ReviewRate2"] != DBNull.Value ? dr["ReviewRate2"].ToString() : "", out reviewRate);
                                Int32.TryParse(dr["VersionId2"] != DBNull.Value ? dr["VersionId2"].ToString() : "", out vesrionId);
                                _car.HotCars.Add(new ComparisonCarModel()
                                {
                                    MakeName = dr["MakeName2"].ToString(),
                                    ModelName = dr["ModelName2"].ToString(),
                                    ModelId = Convert.ToInt32(dr["ModelId2"].ToString()),
                                    MaskingName = dr["MaskingName2"].ToString(),                        
                                    VersionId = vesrionId,
                                    VersionName = dr["VersionName2"].ToString(),
                                    Review = new CarReviewBase()
                                    {
                                        ReviewCount = reviewCount,
                                        OverallRating = reviewRate
                                    }
                                });
                                _car.IsSponsored = CustomParser.parseBoolObject(dr["IsSponsored"]);
                                _topComparisons.Add(_car);
                            }

                        }
                    }
                }

                foreach( HotCarComparison comparison in _topComparisons)
                {
                    comparison.CompareUrl = Format.GetCompareUrl(comparison.HotCars.Select(x => new Tuple<int, string>(x.ModelId, Format.FormatSpecial(x.MakeName)+"-"+x.MaskingName)).ToList());
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CompareCarsRepository.GetHotComaprisons()");
                objErr.LogException();
                throw;
            }
            return _topComparisons;
        
        }

        public List<CompareCarOverview> GetCompareCarsDetails(Pagination page)
        {
            string startIndex = Convert.ToString(((Convert.ToInt32(page.PageNo) - 1) * Convert.ToInt32(page.PageSize)) + 1);
            string lastIndex = Convert.ToString(Convert.ToInt32(startIndex) + Convert.ToInt32(page.PageSize) - 1);

            List<CompareCarOverview> _comparisonList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetCompareCarList_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartIndex",DbType.Int16,startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LastIndex",DbType.Int16, lastIndex));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                    {
                        _comparisonList = new List<CompareCarOverview>();

                        while (dr.Read())
                        {
                            _comparisonList.Add(new CompareCarOverview()
                            {
                                Car1 = new CompareCarVersionInfo()
                                {
                                    VersionId = (dr["VersionId1"]  != DBNull.Value && dr["VersionId1"].ToString() != string.Empty) ? Convert.ToInt16(dr["VersionId1"]) : 0,
                                    CarName = dr["Make1"].ToString() + " " + dr["Model1"].ToString(),
                                    ModelId = (dr["ModelId1"] != DBNull.Value && dr["ModelId1"].ToString() != string.Empty) ? Convert.ToInt16(dr["ModelId1"]) : 0
                                },
                                Car2 = new CompareCarVersionInfo()
                                {
                                    VersionId = (dr["VersionId2"] != DBNull.Value && dr["VersionId2"].ToString() != string.Empty) ? Convert.ToInt16(dr["VersionId2"]) : 0,
                                    CarName = dr["Make2"].ToString() + " " + dr["Model2"].ToString(),
                                    ModelId = (dr["ModelId2"] != DBNull.Value && dr["ModelId2"].ToString() != string.Empty) ? Convert.ToInt16(dr["ModelId2"]) : 0
                                    
                                },
                                HostUrl = _imgHostUrl,
                                OriginalImgPath = dr["OriginalImgPath"].ToString(),
                                DetailsUrl = ApiHostUrl + "/webapi/CompareCar/GetCompareCarDetails/?vids=" + dr["VersionId1"].ToString() + "," + dr["VersionId2"].ToString(),
                                IsSponsored = (dr["IsSponsored"] != DBNull.Value && dr["IsSponsored"].ToString() != string.Empty) ? Convert.ToBoolean(dr["IsSponsored"]) : false
                            });
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CompareCarsRepository.GetCompareCarList()");
                objErr.LogException();
                throw;
            }
            return _comparisonList;
        }

        public int GetCompareCarCount() 
        {
            int totalcount = 0;

            try
            {
                using (var con = CarDataMySqlReadConnection)
                {
                    totalcount = con.Query<int>("GetCompareCarsCount_v16_11_7", CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CompareCarsRepository.GetCompareCarCount()");
                objErr.LogException();
                throw;
            }
            return totalcount;        
        }

        public DataSet GetCarModels(int MakeId1, int MakeId2, int MakeId3, int MakeId4,int type)
        {
            DataSet ds = new DataSet();
            using (DbCommand cmd = DbFactory.GetDBCommand(("GetCarModelForComparison")))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeId1", DbType.Int32, Convert.ToInt32(MakeId1)));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeId2", DbType.Int32, Convert.ToInt32(MakeId2)));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeId3", DbType.Int32, Convert.ToInt32(MakeId3)));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeId4", DbType.Int32, Convert.ToInt32(MakeId4)));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_OnlyNew", DbType.Int32, type)); 

                ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
            }
            return ds;
        }


       public DataSet GetCarVersionsDataForCompare(int version1, int version2)
        {
            DataSet ds = new DataSet();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetCarComparisonData_Android"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId1", DbType.Int32, version1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId2", DbType.Int32, version2));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                }
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "GetCarVersionsDataForCompare");
                objErr.LogException();
                return new DataSet();
            }
            return ds;
        }

       public DataSet GetVersionsListForComapre(string ModelId)
       {       
           DataSet ds = new DataSet();
           try
           {
               using (DbCommand cmd = DbFactory.GetDBCommand("WA_FetchCarVersions_v16_11_7"))
               {
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int32, Convert.ToInt32(ModelId)));
                   ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
               }
           }
           catch (Exception err)
           {
               ExceptionHandler objErr = new ExceptionHandler(err, "GetVersionsListForComapre");
               objErr.LogException();
               return new DataSet();
           }
           return ds;
       }


    }
}
