using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Dealers;
using System.Data.SqlClient;
using System.Data;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications;
using Carwale.Entity.CarData;
using Carwale.Interfaces.Dealers;
using Carwale.Utility;
using Carwale.Entity;
using Carwale.Notifications.Logs;
using Dapper;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.Campaigns;

namespace Carwale.DAL.Dealers
{
    public class NewCarDealersRepository : RepositoryBase, INewCarDealersRepository
    {
        private static string _imgHostUrl = Carwale.Utility.CWConfiguration._imgHostUrl;

        /// <summary>
        /// Gets the list of makes based on cityid passed
        /// Written By : Supriya on 29/5/2014
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<CarMakeEntityBase> GetMakesByCity(int cityId)
        {
            var carMakesList = new List<CarMakeEntityBase>();
            try
            {
                using (var cmd = DbFactory.GetDBCommand("GetNewCarDealerMakeByCity_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int16, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            carMakesList.Add(new CarMakeEntityBase()
                            {
                                MakeId = Convert.ToInt16(dr["Value"]),
                                MakeName = dr["Text"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersRepository.GetMakesByCity()");
                objErr.LogException();
                throw;
            }
            return carMakesList;
        }

        /// <summary>
        /// Gets the list of cities based on makeid passed
        /// Written By : Supriya on 8/9/2014
        /// </summary>
        /// <returns></returns>
        public List<DealerStateEntity> GetCitiesByMake(int makeId)
        {
            List<DealerStateEntity> stateList = new List<DealerStateEntity>();
            try
            {
                using (var cmd = DbFactory.GetDBCommand("GetNewCarDealerCityByMake_v18_4_1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeId", DbType.Int16, makeId));

                    using (var dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            stateList.Add(new DealerStateEntity()
                            {
                                StateId = Convert.ToInt16(dr["StateId"]),
                                StateName = dr["StateName"].ToString(),
                                CityId = Convert.ToInt16(dr["CityId"]),
                                CityName = dr["CityName"].ToString(),
								CityMaskingName = dr["CityMaskingName"].ToString(),
                                TotalCount = Convert.ToInt16(dr["TotalCount"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersRepository.GetCitiesByMake()");
                objErr.LogException();
                throw;
            }
            return stateList;
        }

        /// <summary>
        /// Gets the list of popularcities based on makeid passed
        /// Written By : Supriya on 8/9/2014
        /// </summary>
        /// <returns></returns>
        public List<PopularCitiesEntity> GetPopularCitiesByMake(int makeId)
        {
            var popularCities = new List<PopularCitiesEntity>();
            try
            {
                using (var cmd = DbFactory.GetDBCommand("GetNewCarDealerPopularCityByMake_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeId", DbType.Int16, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            if (dr["CityId"] != DBNull.Value)
                            {
                                popularCities.Add(new PopularCitiesEntity()
                                {
                                    CityId = Convert.ToInt16(dr["CityId"]),
                                    CityName = dr["CityName"].ToString(),
                                    CityImgUrl = dr["CityImgUrl"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersRepository.GetPopularCitiesByMake()");
                objErr.LogException();
                throw;
            }
            return popularCities;
        }

        public List<NewCarDealerCountByMake> GetCarCountByMakesAndType(string type)
        {
            var NCDMakeCount = new List<NewCarDealerCountByMake>();
            try
            {
                using (var cmd = DbFactory.GetDBCommand("GetNewCarDealerCountByMake_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            if (dr["MakeId"] != DBNull.Value)
                            {
                                NCDMakeCount.Add(new NewCarDealerCountByMake()
                                {
                                    MakeId = Convert.ToInt32(dr["MakeId"]),
                                    MakeName = dr["MakeName"].ToString(),
                                    Count = Convert.ToInt32(dr["DealerCount"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersRepository.GetCarCountByMakesAndType()");
                objErr.LogException();
                throw;
            }
            return NCDMakeCount;
        }

        /// <summary>
        /// Created By Chetan Thambad on 03/02/16 
        /// To get Dealers Specific Models
        /// BranchId is nothing but DealerId
        /// </summary>
        /// <returns></returns>
        public List<MakeModelEntity> GetDealerModels(int dealerId)
        {
            var dealerSpecificModels = new List<MakeModelEntity>();
            try
            {
                using (var cmd = DbFactory.GetDBCommand("GetModelOnBranchId_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_BranchId", DbType.Int16, dealerId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            dealerSpecificModels.Add(new MakeModelEntity()
                                {
                                    ModelId = CustomParser.parseIntObject(dr["ModelId"]),
                                    ModelName = CustomParser.parseStringObject(dr["ModelName"].ToString()),
                                    MakeId = CustomParser.parseIntObject(dr["MakeId"])
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersRepository.GetDealerModels()");
                objErr.LogException();
                throw;
            }
            return dealerSpecificModels;
        }

        /// <summary>
        /// This function returns the list of NCS dealers for a particular model and city
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<NewCarDealersList> GetNCSDealers(int modelId, int cityId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId);
                param.Add("v_CityId", cityId);

                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<NewCarDealersList>("GetNCSDealers_v18_11_1", param, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersRepository.GetNewCarDealers()");
                objErr.LogException();
                throw;
            }
        }

        public DealerMicrositeImage GetDealerMicrositeImages(int dealerId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@DealerId", dealerId);
                using (var con = Connection)
                {
                    return con.Query<DealerMicrositeImage>("[dbo].[GetDealerMicrositeImages]", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersRepository.GetNewCarDealers()");
                objErr.LogException();
                throw;
            }
        }

        public List<ClientCampaignMapping> GetClientCampaignMapping()
        {
            try
            {
                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<ClientCampaignMapping>("GetClientCampaignMapping", commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }
        public IEnumerable<NewCarDealer> GetDealerListByCityMake(int makeId, int cityId)
        {
            var param = new DynamicParameters();
            param.Add("v_MakeId", makeId);
            param.Add("v_CityId", cityId);
            var query = @"select d.Id AS DealerId 
                            ,d.Organization AS Name
                            ,d.Address1 AS Address
		                    ,d.EmailId
		                    ,d.PinCode
		                    ,d.FaxNo
		                    ,d.WebsiteUrl AS Website
		                    ,d.MobileNo 
		                    ,d.Lattitude AS Latitude
		                    ,d.Longitude AS Longitude
		                    ,d.ShowroomStartTime AS StartTime
		                    ,d.ShowroomEndTime AS EndTime
		                    ,d.PhoneNo AS LandLineNo
		                    ,d.FirstName
		                    ,d.LastName
		                    ,d.LandlineCode
                            ,d.ProfilePhotoHostUrl
		                    ,d.ProfilePhotoUrl
                            ,ct.NAME AS CityName
		                    ,ct.ID AS CityId
		                    ,st.NAME AS State
		                    ,st.ID AS StateId
                            FROM cwmasterdb.dealers d 
                            INNER JOIN cwmasterdb.dealermakes dmk on dmk.dealerId = d.Id AND d.TC_DealerTypeId = 2 and d.ApplicationId = 1 -- Carwale dealers;
                            INNER JOIN cwmasterdb.cities ct on ct.Id = d.CityId
                            INNER JOIN cwmasterdb.states st on st.Id = ct.StateId
                            WHERE (d.CityId = @v_CityId) 
                            and dmk.MakeId = @v_MakeId
                            and d.Status =0
                            and d.IsLocatorActive = 1";
            using (var con = NewCarMySqlReadConnection)
            {
                return con.Query<NewCarDealer>(query, param);
            }
        }
    }
}
