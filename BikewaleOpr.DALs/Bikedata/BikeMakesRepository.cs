
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Linq;
using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using Dapper;
using MySql.CoreDAL;

namespace BikewaleOpr.DALs.Bikedata
{
    /// <summary>
    /// 
    /// </summary>
    public class BikeMakesRepository : IBikeMakesRepository
    {
        /// <summary>
        /// Created By : Sushil Kumar on  25th Oct 2016
        /// Description :  Getting makes only by providing only request type
        /// </summary>
        /// <param name="RequestType">Pass value as New or Used or Upcoming or PriceQuote or ALL</param>
        /// <returns></returns>
        public IEnumerable<BikeMakeEntityBase> GetMakes(string RequestType)
        {
            IList<BikeMakeEntityBase> _objBikeMakes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemakes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, RequestType));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            _objBikeMakes = new List<BikeMakeEntityBase>();
                            while (dr.Read())
                            {
                                BikeMakeEntityBase _objMake = new BikeMakeEntityBase();
                                _objMake.MakeName = Convert.ToString(dr["Text"]);
                                _objMake.MakeId = SqlReaderConvertor.ToInt32(dr["Value"]);
                                _objBikeMakes.Add(_objMake);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.GetMakes_" + RequestType);
            }
            return _objBikeMakes;
        }


        /// <summary>
        /// Function to get the bike makes list along with other details for all makes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeMakeEntity> GetMakesList()
        {
            IEnumerable<BikeMakeEntity> objMakes = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetReadonlyConnection())
                {
                    objMakes = connection.Query<BikeMakeEntity>("GetMakesList", CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.GetMakesList");
            }

            return objMakes;
        }


        /// <summary>
        /// Createed by Sajal Gupta on 20-11-2017
        /// Descriptioption : DAL func to get make footer category data
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<MakeFooterCategory> GetMakeFooterCategoryData(uint makeId)
        {
            IEnumerable<MakeFooterCategory> objMakeFooterData = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetReadonlyConnection())
                {

                    var param = new DynamicParameters();
                    param.Add("par_makeId", makeId);

                    objMakeFooterData = connection.Query<MakeFooterCategory>("getmakefootercategorydata", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.GetMakeFooterCategoryData");
            }

            return objMakeFooterData;

        }

        /// <summary>
        /// Createed by Sajal Gupta on 20-11-2017
        /// Descriptioption : DAL func to save make footer category data
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public void SaveMakeFooterData(uint makeId, uint categoryId, string categorydescription, string userId)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("par_makeid", makeId);
                    param.Add("par_userid", userId);
                    param.Add("par_categorydescription", categorydescription);
                    param.Add("par_categoryId", categoryId);

                    connection.Query("savemakefootercategories", param: param, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.SaveMakeFooterData");
            }
        }

        /// <summary>
        /// Createed by Sajal Gupta on 20-11-2017
        /// Descriptioption : DAL func to delete all make footer category data for amke
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public void DisableAllMakeFooterCategories(uint makeId, string userId)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {

                    var param = new DynamicParameters();

                    param.Add("par_makeid", makeId);
                    param.Add("par_userid", userId);

                    connection.Query("disableallmakefootercategoriesformake", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.DisableAllMakeFooterCategories");
            }
        }

        /// <summary>
        /// Function to add new make to the bikewale database
        /// </summary>
        /// <param name="make"></param>
        /// <param name="isMakeExist"></param>
        /// <param name="makeId"></param>
        public void AddMake(BikeMakeEntity make, ref short isMakeExist, ref int makeId)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("par_make", make.MakeName);
                    param.Add("par_makemaskingname", make.MaskingName);
                    param.Add("par_userid", make.UpdatedBy);
                    param.Add("par_ismakeexist", dbType: DbType.Int16, direction: ParameterDirection.Output);
                    param.Add("par_makeid", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Query<dynamic>("insertbikemake", param: param, commandType: CommandType.StoredProcedure);

                    isMakeExist = param.Get<short>("par_ismakeexist");
                    makeId = param.Get<int>("par_makeid");

                    if (makeId > 0)
                    {
                        // Create name value collection
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("v_MakeId", makeId.ToString());
                        nvc.Add("v_MakeName", make.MakeName);
                        nvc.Add("v_MaskingName", make.MaskingName);
                        nvc.Add("v_Futuristic", "0");
                        nvc.Add("v_Used", "1");
                        nvc.Add("v_New", "1");
                        SyncBWData.PushToQueue("BW_AddBikeMakes", DataBaseName.CW, nvc);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.AddMake");
            }
        }


        /// <summary>
        /// Function to update the make information
        /// </summary>
        /// <param name="make"></param>
        public void UpdateMake(BikeMakeEntity make)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {

                    var param = new DynamicParameters();

                    param.Add("par_make", make.MakeName);
                    param.Add("par_makeid", make.MakeId);
                    param.Add("par_makemaskingname", make.MaskingName);
                    param.Add("par_isfuturistic", make.Futuristic);
                    param.Add("par_isnew", make.New);
                    param.Add("par_isused", make.Used);
                    param.Add("par_userid", make.UpdatedBy);

                    connection.Query("updatebikemake", param: param, commandType: CommandType.StoredProcedure);

                    // Push the data to carwale DB
                    // Create name value collection
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("v_MakeId", make.MakeId.ToString());
                    nvc.Add("v_MakeName", make.MakeName);
                    nvc.Add("v_IsNew", Convert.ToString(make.New ? 1 : 0));
                    nvc.Add("v_IsUsed", Convert.ToString(make.Used ? 1 : 0));
                    nvc.Add("v_IsFuturistic", Convert.ToString(make.Used ? 1 : 0));
                    nvc.Add("v_MaskingName", make.MaskingName);
                    nvc.Add("v_IsDeleted", null);
                    SyncBWData.PushToQueue("BW_UpdateBikeMakes", DataBaseName.CW, nvc);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.UpdateMake");
            }
        }


        /// <summary>
        /// Function to delete the make
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="updatedBy"></param>
        public void DeleteMake(int makeId, int updatedBy)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {

                    var param = new DynamicParameters();

                    param.Add("par_makeid", makeId);
                    param.Add("par_updatedby", updatedBy);

                    connection.Query("updatemodelversionisdeleted", param: param, commandType: CommandType.StoredProcedure);

                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("v_MakeId", makeId.ToString());
                    nvc.Add("v_MaskingName", null);
                    nvc.Add("v_MakeName", null);
                    nvc.Add("v_IsNew", null);
                    nvc.Add("v_IsUsed", null);
                    nvc.Add("v_IsFuturistic", null);
                    nvc.Add("v_IsDeleted", "1");
                    SyncBWData.PushToQueue("BW_UpdateBikeMakes", DataBaseName.CW, nvc);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.DeleteMake");
            }
        }


        /// <summary>
        /// Function to get the make synopsis from database
        /// Modified by : Sajal gupta on 10-03-2017
        /// Description : Fetch scooter synopsis
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public SynopsisData Getsynopsis(int makeId)
        {
            SynopsisData objSynopsis = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    objSynopsis = new SynopsisData();

                    var param = new DynamicParameters();

                    param.Add("par_makeid", makeId);

                    dynamic temp = connection.Query<dynamic>("getmakesynopsis_10032017", param: param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    objSynopsis.BikeDescription = ReferenceEquals(null, temp) ? string.Empty : temp.description;
                    objSynopsis.ScooterDescription = ReferenceEquals(null, temp) ? string.Empty : temp.scooterdescription;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.Getsynopsis");
            }

            return objSynopsis;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 3 Feb 2017
        /// Summary : Function to update the synopsis for the given make
        /// Modified by : Sajal gupta on 10-03-2017
        /// Description : Save scooter synopsis
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="synopsis"></param>
        /// <param name="updatedBy"></param>
        public void UpdateSynopsis(int makeId, int updatedBy, SynopsisData objSynopsis)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("par_makeid", makeId);
                    param.Add("par_userid", updatedBy);
                    if (objSynopsis != null)
                    {
                        param.Add("par_discription", objSynopsis.BikeDescription);
                        param.Add("par_scootersynopsis", objSynopsis.ScooterDescription);
                    }

                    connection.Query("managemakesynopsis_10032017", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.UpdateSynopsis");
            }
        }

        /// <summary>
        /// Modified by : Snehal Dange on 11-08-2017
        /// Description : Get Bike makes
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public IEnumerable<BikeMakeEntityBase> GetMakes(ushort requestType)
        {
            IEnumerable<BikeMakeEntityBase> objMakes = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("par_requesttype", requestType);

                    objMakes = connection.Query<BikeMakeEntityBase>("getbikemakes_11082017", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.BikeData.GetMakes_{0}", requestType));
            }

            return objMakes;
        }




        /// <summary>
        /// Created by : Vivek Singh Tomar on 1st Aug 2017
        /// Description : To fetch the model details list for given make id
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<BikeModelEntityBase> GetModelsByMake(EnumBikeType requestType, uint makeId)
        {
            IEnumerable<BikeModelEntityBase> objBikeModelEntityBaseList = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_makeid", makeId);
                    param.Add("par_requesttype", requestType.ToString());

                    objBikeModelEntityBaseList = connection.Query<BikeModelEntityBase>
                        ("getbikemodels_new_07082017", param: param, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.BikeData.GetModelsByMake_{0}_{1}", requestType, makeId));
            }
            return objBikeModelEntityBaseList;
        }


        /// <summary>
        /// Gets the make details by identifier.
        /// Author: Sangram Nandkhile on 08 Dec 2017
        /// </summary>
        /// <param name="makeId">The make identifier.</param>
        /// <returns></returns>
        public BikeMakeEntity GetMakeDetailsById(uint makeId)
        {
            BikeMakeEntity makeDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmakedetails_14082017";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            makeDetails = new BikeMakeEntity();
                            while (reader.Read())
                            {
                                makeDetails.MakeId = SqlReaderConvertor.ToInt32(reader["id"]);
                                makeDetails.MakeName = Convert.ToString(reader["name"]);
                                makeDetails.MaskingName = Convert.ToString(reader["maskingname"]);
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.BikeData.GetMakeDetailsById_MakeId: =>{0}", makeId));
            }

            return makeDetails;
        }
    }   // class
}   // namespace
