
using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace BikewaleOpr.DALs.Bikedata
{
    /// <summary>
    /// 
    /// </summary>
    public class BikeMakesRepository : IBikeMakes
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
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.Bikedata.GetMakes_" + RequestType);
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
                    connection.Open();

                    objMakes = connection.Query<BikeMakeEntity>("GetMakesList", CommandType.StoredProcedure).ToList();

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.Bikedata.GetMakesList");
            }

            return objMakes;
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
                    connection.Open();

                    var param = new DynamicParameters();

                    param.Add("par_make", make.MakeName);
                    param.Add("par_makemaskingname", make.MaskingName);
                    param.Add("par_userid", make.UpdatedBy);
                    param.Add("par_ismakeexist", dbType: DbType.Int16, direction: ParameterDirection.Output);
                    param.Add("par_makeid", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Query<dynamic>("insertbikemake", param: param, commandType: CommandType.StoredProcedure);

                    isMakeExist = param.Get<short>("par_ismakeexist");
                    makeId = param.Get<int>("par_makeid");

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.Bikedata.AddMake");
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
                    connection.Open();

                    var param = new DynamicParameters();

                    param.Add("par_make", make.MakeName);
                    param.Add("par_makeid", make.MakeId);
                    param.Add("par_makemaskingname", make.MaskingName);
                    param.Add("par_isfuturistic", make.Futuristic);
                    param.Add("par_isnew", make.New);
                    param.Add("par_isused", make.Used);
                    param.Add("par_userid", make.UpdatedBy);

                    connection.Query("updatebikemake", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.Bikedata.UpdateMake");
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
                    connection.Open();

                    var param = new DynamicParameters();

                    param.Add("par_makeid", makeId);
                    param.Add("par_updatedby", updatedBy);

                    connection.Query("updatemodelversionisdeleted", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.Bikedata.DeleteMake");
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
                    connection.Open();

                    var param = new DynamicParameters();

                    param.Add("par_makeid", makeId);

                    dynamic temp = connection.Query<dynamic>("getmakesynopsis_10032017", param: param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    objSynopsis.BikeDescription = ReferenceEquals(null, temp) ? string.Empty : temp.description;
                    objSynopsis.ScooterDescription = ReferenceEquals(null, temp) ? string.Empty : temp.scooterdescription;

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.Bikedata.Getsynopsis");
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
                    connection.Open();

                    var param = new DynamicParameters();

                    param.Add("par_makeid", makeId);
                    param.Add("par_userid", updatedBy);
                    if (objSynopsis != null)
                    {
                        param.Add("par_discription", objSynopsis.BikeDescription);
                        param.Add("par_scootersynopsis", objSynopsis.ScooterDescription);
                    }

                    connection.Query("managemakesynopsis_10032017", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.Bikedata.UpdateSynopsis");
            }
        }
    }   // class
}   // namespace
