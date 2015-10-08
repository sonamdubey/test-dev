using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Interfaces.Dealer;
using Bikewale.Entities.Dealer;
using Bikewale.Notifications;
using Bikewale.Entities.Location;
using Bikewale.Entities.BikeData;
using Bikewale.CoreDAL;
using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace Bikewale.DAL.Dealer
{
    /// <summary>
    /// Created By : Ashwini Todkar on4 June 2014
    /// Summary    : Implements logic to access New Bike Dealers data
    /// </summary>
    public class DealersRepository : IDealer
    {
        /// <summary>
        /// Method to get list of make and total dealers for which dealers are available in india
        /// </summary>
        /// <returns></returns>
        public List<NewBikeDealersMakeEntity> GetDealersMakesList()
        {
            List<NewBikeDealersMakeEntity> objMakeList = null;
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealersMakeList";

                    objMakeList = new List<NewBikeDealersMakeEntity>();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        while (dr.Read())
                        {
                            objMakeList.Add(new NewBikeDealersMakeEntity
                            {
                                MakeId = Convert.ToInt32(dr["MakeId"]),
                                MakeName = Convert.ToString(dr["BikeMake"]),
                                MaskingName = Convert.ToString(dr["MaskingName"]),
                                DealersCount = Convert.ToInt32(dr["TotalCount"])
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersMakesList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersMakesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objMakeList;
        }

        /// <summary>
        /// Method to get list of dealers cities for given make in india 
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public NewBikeDealersListEntity GetDealersCitiesListByMakeId(uint makeId)
        {
            NewBikeDealersListEntity objDealerList = null;
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealersCitiesByMakeId";
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;

                    objDealerList = new NewBikeDealersListEntity();
                    objDealerList.TotalDealers = 0;

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        objDealerList.CityWiseDealers = new List<CityWiseDealersCountEntity>();
                        objDealerList.StatesList = new List<StateEntityBase>();

                        while (dr.Read())
                        {
                            objDealerList.CityWiseDealers.Add(new CityWiseDealersCountEntity
                            {
                                CityId = Convert.ToUInt32(dr["CityId"]),
                                CityName = Convert.ToString(dr["City"]),
                                CityMaskingName = Convert.ToString(dr["CityMaskingName"]),
                                StateId = Convert.ToInt32(dr["StateId"]),
                                DealersCount = Convert.ToInt32(dr["TotalDealers"])
                            });

                            //get state list
                            if (Convert.ToUInt32(dr["StateRank"]) == 1)
                            {
                                objDealerList.StatesList.Add(new StateEntityBase
                                {
                                    StateId = Convert.ToUInt32(dr["StateId"]),
                                    StateName = Convert.ToString(dr["StateName"])
                                });
                            }

                            objDealerList.TotalDealers = objDealerList.TotalDealers + Convert.ToInt32(dr["TotalDealers"]);
                        }
                    }
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("GetDealersCitiesListByMakeId sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersCitiesListByMakeId ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return objDealerList;
        }

        /// <summary>
        /// Method to get list of dealers with its details for given city and make
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<NewBikeDealerEntity> GetDealersList(uint makeId, uint cityId)
        {
            List<NewBikeDealerEntity> objDealerList = null;
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealersList";
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                    objDealerList = new List<NewBikeDealerEntity>();
                    StateEntityBase objState =  null;
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        objState = new StateEntityBase();

                        while (dr.Read())
                        {
                            objState.StateName = dr["STATE"].ToString();
                            objDealerList.Add(new NewBikeDealerEntity
                            {
                                Name = Convert.ToString(dr["DealerName"]),
                                Address = Convert.ToString(dr["Address"]),
                                PhoneNo = Convert.ToString(dr["ContactNo"]),
                                Email = Convert.ToString(dr["EMailId"]),
                                PinCode = Convert.ToString(dr["PinCode"]),
                                CityName = Convert.ToString(dr["City"]),
                                Website = Convert.ToString(dr["WebSite"]),
                                Fax = Convert.ToString(dr["FaxNo"]),
                                State = objState
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objDealerList;
        }

        /// <summary>
        /// Method to get list of dealers with its details for given city and make
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public NewBikeDealerEntityList GetNewBikeDealersList(int makeId, int cityId, EnumNewBikeDealerClient? clientId = null)
        {
            IList<NewBikeDealerEntityBase> objDealerList = null;
            NewBikeDealerEntityList objLstNewBikeDealerEntity = null;
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetNewBikeDealers";
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                    if(clientId.HasValue)
                        cmd.Parameters.AddWithValue("@ClientId", clientId.Value);
                    
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {                         
                        objLstNewBikeDealerEntity = new NewBikeDealerEntityList();
                        objDealerList = new List<NewBikeDealerEntityBase>();
                        bool isFirstRow = true;
                        while (dr.Read())
                        {
                            if(isFirstRow)
                            {
                                objLstNewBikeDealerEntity.BikeMake = Convert.ToString(dr["BikeMake"]);
                                objLstNewBikeDealerEntity.MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objLstNewBikeDealerEntity.City = Convert.ToString(dr["City"]);
                                objLstNewBikeDealerEntity.State = Convert.ToString(dr["State"]);
                                isFirstRow = false;
                            } 
                            NewBikeDealerEntityBase objDealer = new NewBikeDealerEntityBase();
                            objDealer.Id = Convert.ToInt32(dr["DealerId"]);
                            objDealer.Name = Convert.ToString(dr["DealerName"]);
                            objDealer.Address = Convert.ToString(dr["Address"]);
                            objDealer.ContactNo = Convert.ToString(dr["ContactNo"]);
                            objDealer.Email = Convert.ToString(dr["EMailId"]);
                            objDealer.PinCode = Convert.ToString(dr["PinCode"]);
                            objDealer.Website = Convert.ToString(dr["WebSite"]);
                            objDealer.Fax = Convert.ToString(dr["FaxNo"]);
                            objDealer.WorkingHours = Convert.ToString(dr["WorkingHours"]);
                            objDealerList.Add(objDealer);
                        }
                        objLstNewBikeDealerEntity.Dealers = objDealerList;
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeShowrooms sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeShowrooms ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objLstNewBikeDealerEntity;
        }

        /// <summary>
        /// Method to get list of makes for which dealers are available in given city.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<BikeMakeEntityBase> GetDealersMakeListByCityId(uint cityId)
        {
            List<BikeMakeEntityBase> objMakeList = null;
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealerMakesByCityId";
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                    objMakeList = new List<BikeMakeEntityBase>();                  

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        while (dr.Read())
                        {
                            objMakeList.Add(new BikeMakeEntityBase
                            {
                                MakeId = Convert.ToInt32(dr["ID"]),
                                MakeName = Convert.ToString(dr["Name"]),
                                MaskingName = Convert.ToString(dr["MaskingName"])                       
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersMakeListByCityId sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersMakeListByCityId ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objMakeList;
        }

        /// <summary>
        /// Method to get list of all cities in which dealers are available
        /// </summary>
        /// <returns></returns>
        public List<CityEntityBase> GetDealersCitiesList()
        {
            List<CityEntityBase> objCityList = null;
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[GetDealerCities_New]";

                    objCityList = new List<CityEntityBase>();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        while (dr.Read())
                        {
                            objCityList.Add(new CityEntityBase
                            {
                                CityId =Convert.ToUInt32(dr["ID"]),
                                CityName = Convert.ToString(dr["NAME"]),
                                CityMaskingName = Convert.ToString(dr["CityMaskingName"])
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersCitiesList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersCitiesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objCityList;
        }

    }//End class
}
