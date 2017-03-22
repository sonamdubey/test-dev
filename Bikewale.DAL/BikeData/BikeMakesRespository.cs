using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bikewale.DAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Modified By : Lucky Rathore
    /// Summary : changes in function GetMakesByType
    /// Modified By :   Sumit Kate on 16 Nov 2015
    /// Summary :   Added new function UpcomingBikeMakes
    /// </summary>
    /// <typeparam name="T">Generic type (need to specify type while implementing this class)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this class)</typeparam>
    public class BikeMakesRepository<T, U> : IBikeMakes<T, U> where T : BikeMakeEntity, new()
    {
        /// <summary>
        /// Summary : Function to get all makes base entities
        /// Modified By : Lucky Rathore
        /// Summary : Added HostUrl and LogoUrl for BikeMakeEntityBase in GetMakesByType function.
        /// Modified by :   Sumit Kate on 03 Mar 2016
        /// Description :   Updated SP GetBikeMakes_New_03032016. Populate PopularityIndex.
        /// Modified by :   Sumit Kate on 29 Mar 2016
        /// Description :   GetBikeMakes_New_29032016 support Dealer request type which returns the makes list of BW and AB dealers
        /// Modified by :-Subodh 0n 08 nov 2016
        /// Description : getbikemakes_new_08112016 added ServiceCenter and changed par_request from string to tinyint(2)
        /// </summary>
        /// <param name="makeType">Type of bike data</param>
        /// <returns>Returns list of type BikeMakeEntityBase</returns>
        public List<BikeMakeEntityBase> GetMakesByType(EnumBikeType makeType)
        {
            List<BikeMakeEntityBase> objMakesList = null;


            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemakes_new_08112016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.Int32, makeType));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objMakesList = new List<BikeMakeEntityBase>();

                            while (dr.Read())
                            {
                                objMakesList.Add(new BikeMakeEntityBase()
                                {
                                    MakeId = Convert.ToInt32(dr["ID"]),
                                    MakeName = dr["NAME"].ToString(),
                                    MaskingName = dr["MaskingName"].ToString(),
                                    HostUrl = Convert.IsDBNull(dr["HostUrl"]) ? "" : dr["HostUrl"].ToString(),
                                    LogoUrl = Convert.IsDBNull(dr["LogoUrl"]) ? "" : dr["LogoUrl"].ToString(),
                                    PopularityIndex = Convert.IsDBNull(dr["PopularityIndex"]) ? default(UInt16) : Convert.ToUInt16(dr["PopularityIndex"])
                                });
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objMakesList;
        }

        public U Add(T t)
        {
            throw new NotImplementedException();
        }

        public bool Update(T t)
        {
            throw new NotImplementedException();
        }

        public bool Delete(U id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Summary : Function to get the make details by make id.
        /// </summary>
        /// <param name="id">MakeId. Only numbers are allowed. Negative values are not allowed.</param>
        /// <returns>Returns particular make's details in an object.</returns>
        public T GetById(U id)
        {
            T t = default(T);
            try
            {
                t = new T();

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmakedetails";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makename", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makemaskingname", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_new", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_used", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_futuristic", DbType.Boolean, ParameterDirection.Output));

                    // LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    HttpContext.Current.Trace.Warn("qry success");

                    if (!string.IsNullOrEmpty(cmd.Parameters["par_makename"].Value.ToString()))
                    {
                        t.MakeName = cmd.Parameters["par_makename"].Value.ToString();
                        t.MaskingName = cmd.Parameters["par_makemaskingname"].Value.ToString();
                        t.New = Convert.ToBoolean(cmd.Parameters["par_new"].Value);
                        t.Used = Convert.ToBoolean(cmd.Parameters["par_used"].Value);
                        t.Futuristic = Convert.ToBoolean(cmd.Parameters["par_futuristic"].Value);
                    }

                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetById sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetById ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return t;
        }

        public BikeDescriptionEntity GetMakeDescription(U makeId)
        {
            BikeDescriptionEntity objMake = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmakesynopsis"))
                {
                    //cmd.CommandText = "getmakesynopsis";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objMake = new BikeDescriptionEntity()
                            {
                                Name = Convert.ToString(dr["MakeName"]),
                                SmallDescription = Convert.ToString(dr["Description"]),
                                FullDescription = Convert.ToString(dr["Description"])
                            };

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetMakeDescription ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objMake;
        }

        /// <summary>
        /// Getting makes only by providing only request type
        /// </summary>
        /// <param name="RequestType">Pass value as New or Used or Upcoming or PriceQuote or ALL</param>
        /// <returns></returns>
        public DataTable GetMakes(string RequestType)
        {
            DataTable dt = null;

            using (DbCommand cmd = DbFactory.GetDBCommand("getbikemakes"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, RequestType));
                try
                {
                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
                    }
                }
                catch (SqlException ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
            return dt;
        }
        /// <summary>
        /// Created by: Sangram Nandkhile on 24 Nov 2016
        /// Summarry; Fetch make list by UINT makeid (method overload)
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public BikeMakeEntityBase GetMakeDetails(uint makeId)
        {
            BikeMakeEntityBase makeDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmakedetails";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makename", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makemaskingname", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_new", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_used", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_futuristic", DbType.Boolean, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
                    if (!string.IsNullOrEmpty(cmd.Parameters["par_makename"].Value.ToString()))
                    {
                        makeDetails = new BikeMakeEntityBase();
                        makeDetails.MakeName = cmd.Parameters["par_makename"].Value.ToString();
                        makeDetails.MaskingName = cmd.Parameters["par_makemaskingname"].Value.ToString();
                        makeDetails.MakeId = Convert.ToInt32(makeId);
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, string.Format("BikeMakeEntityBase.GetMakeDetails(): makeId : {0}", makeId));
                objErr.SendMail();
            }

            return makeDetails;
        }
        /// <summary>
        ///     Get Makeid and make name from the make id
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public BikeMakeEntityBase GetMakeDetails(string makeId)
        {
            // Validate the makeId
            if (!CommonOpn.IsNumeric(makeId))
                return null;

            BikeMakeEntityBase makeDetails = null;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmakedetails";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makename", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makemaskingname", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_new", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_used", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_futuristic", DbType.Boolean, ParameterDirection.Output));

                    // LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    if (!string.IsNullOrEmpty(cmd.Parameters["par_makename"].Value.ToString()))
                    {
                        makeDetails = new BikeMakeEntityBase();
                        makeDetails.MakeName = cmd.Parameters["par_makename"].Value.ToString();
                        makeDetails.MaskingName = cmd.Parameters["par_makemaskingname"].Value.ToString();
                        makeDetails.MakeId = Convert.ToInt32(makeId);
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return makeDetails;
        }   // End of getMakeDetails
        /// <summary>
        /// Returns the Upcoming Bike's Make list
        /// Author  :   Sumit Kate
        /// Created :   16 Nov 2015
        /// </summary>
        /// <returns>Upcoming Bike's Make list</returns>
        public IEnumerable<BikeMakeEntityBase> UpcomingBikeMakes()
        {
            IList<BikeMakeEntityBase> makes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getupcomingbikemakes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            makes = new List<BikeMakeEntityBase>();
                            while (reader.Read())
                            {
                                makes.Add(
                                    new BikeMakeEntityBase()
                                    {
                                        MakeId = Convert.ToInt32(reader["ID"]),
                                        MakeName = Convert.ToString(reader["Name"]),
                                        MaskingName = Convert.ToString(reader["MaskingName"]),
                                        LogoUrl = Convert.ToString(reader["LogoUrl"]),
                                        HostUrl = Convert.ToString(reader["HostURL"])
                                    }
                                  );
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return makes;
        }
        /// <summary>
        /// Written By : Sangram Nandkhile on 17 Jun 2016
        /// Description: Fetches discontinued bikes for a branch
        /// </summary>
        /// <param name="makeId">Make Id eg. 7 for Honda bikes</param>
        /// <returns></returns>
        public IEnumerable<BikeVersionEntity> GetDiscontinuedBikeModelsByMake(uint makeId)
        {
            IList<BikeVersionEntity> bikeLinkList = null;
            try
            {
                using (DbCommand DbCommand = DbFactory.GetDBCommand("getdiscontinuedbikemodelsbymake"))
                {
                    DbCommand.CommandType = CommandType.StoredProcedure;
                    DbCommand.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.UInt32, makeId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(DbCommand, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            bikeLinkList = new List<BikeVersionEntity>();
                            while (dr.Read())
                            {
                                bikeLinkList.Add(
                                        new BikeVersionEntity()
                                        {
                                            ModelMasking = Convert.ToString(dr["modelmaskingname"]),
                                            ModelName = Convert.ToString(dr["Name"])
                                        }
                                    );
                            }
                        }

                        dr.Close();
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return bikeLinkList;

        }


        /// <summary>
        /// Created by  :   Sumit Kate on 13 Sep 2016
        /// Description :   Returns all makes and their models
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeMakeModelBase> GetAllMakeModels()
        {
            IList<BikeMakeModelBase> makeModels = null;
            try
            {
                using (DbCommand DbCommand = DbFactory.GetDBCommand("getallmakemodel"))
                {
                    DbCommand.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(DbCommand, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            makeModels = new List<BikeMakeModelBase>();
                            while (dr.Read())
                            {
                                makeModels.Add(new BikeMakeModelBase()
                                {
                                    Make = new BikeMakeEntityBase()
                                    {
                                        MakeId = !Convert.IsDBNull(dr["id"]) ? Convert.ToInt32(dr["id"]) : default(int),
                                        MakeName = !Convert.IsDBNull(dr["name"]) ? Convert.ToString(dr["name"]) : default(string),
                                        MaskingName = !Convert.IsDBNull(dr["maskingname"]) ? Convert.ToString(dr["maskingname"]) : default(string),
                                        HostUrl = !Convert.IsDBNull(dr["hosturl"]) ? Convert.ToString(dr["hosturl"]) : default(string),
                                        LogoUrl = !Convert.IsDBNull(dr["logourl"]) ? Convert.ToString(dr["logourl"]) : default(string),
                                    }
                                }
                                );
                            }

                            if (dr.NextResult())
                            {
                                IList<BikeModelMake> Models = new List<BikeModelMake>();
                                while (dr.Read())
                                {
                                    Models.Add(
                                        new BikeModelMake()
                                        {
                                            MakeId = !Convert.IsDBNull(dr["bikemakeid"]) ? Convert.ToInt32(dr["bikemakeid"]) : default(int),
                                            ModelId = !Convert.IsDBNull(dr["id"]) ? Convert.ToInt32(dr["id"]) : default(int),
                                            ModelName = !Convert.IsDBNull(dr["name"]) ? Convert.ToString(dr["name"]) : default(string),
                                            MaskingName = !Convert.IsDBNull(dr["maskingname"]) ? Convert.ToString(dr["maskingname"]) : default(string),
                                        }
                                        );
                                }

                                if (Models != null)
                                {
                                    foreach (var bikeMake in makeModels)
                                    {
                                        var models = (from bike in Models
                                                      where bike.MakeId == bikeMake.Make.MakeId
                                                      select new BikeModelEntityBase()
                                                      {
                                                          ModelId = bike.ModelId,
                                                          ModelName = bike.ModelName,
                                                          MaskingName = bike.MaskingName
                                                      });
                                        if (models != null)
                                        {
                                            bikeMake.Models = models.ToList();
                                        }
                                    }
                                }
                            }


                        }

                        dr.Close();
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "GetAllMakeModels");
                objErr.SendMail();
            }

            return makeModels;
        }

        /// <summary>
        /// Created By : sumit kate on 19 Sep-2016
        /// Summary : To create Hash table for old masking names
        /// </summary>
        /// <returns></returns>
        public Hashtable GetOldMaskingNames()
        {
            Hashtable ht = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getoldmakemaskingnameslist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            ht = new Hashtable();

                            if (dr != null)
                            {
                                while (dr.Read())
                                {
                                    if (!ht.ContainsKey(dr["OldMaskingName"]))
                                        ht.Add(dr["OldMaskingName"], dr["NewMaskingName"]);
                                }
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.DAL.BikeData.BikeMakesRepository.GetOldMaskingNames");
                objErr.SendMail();
            }

            return ht;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 10 Mar 2017
        /// Summary: DAL to fetch scooter's brands
        /// </summary>
        public IEnumerable<BikeMakeEntityBase> GetScooterMakes()
        {
            IList<BikeMakeEntityBase> bikeLinkList = null;
            try
            {
                using (DbCommand DbCommand = DbFactory.GetDBCommand("getscootermakes"))
                {
                    DbCommand.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(DbCommand, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            bikeLinkList = new List<BikeMakeEntityBase>();
                            while (dr.Read())
                            {
                                bikeLinkList.Add(
                                        new BikeMakeEntityBase()
                                        {
                                            MaskingName = Convert.ToString(dr["makemaskingname"]),
                                            MakeName = Convert.ToString(dr["makename"]),
                                            MakeId = SqlReaderConvertor.ToUInt16(dr["makeid"]),
                                            IsScooterOnly = SqlReaderConvertor.ToBoolean(dr["IsScooterOnly"]),
                                            TotalCount = SqlReaderConvertor.ToUInt32(dr["ScooterCount"])
                                        }
                                    );
                            }
                        }

                        dr.Close();
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "BikeMakesRepository<T, U>.GetScooterMakes()");
            }

            return bikeLinkList;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 15 Mar 2017
        /// Summary    : Get scooter synopsis
        /// </summary>
        public BikeDescriptionEntity GetScooterMakeDescription(uint makeId)
        {
            BikeDescriptionEntity objDesc = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmakesynopsis"))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;

                    
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objDesc = new BikeDescriptionEntity()
                            {
                                Name = Convert.ToString(dr["MakeName"]),
                                SmallDescription = Convert.ToString(dr["scooterdescription"]),
                                FullDescription = Convert.ToString(dr["scooterdescription"])
                            };

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex,string.Format("Bikewale.DAL.BikeData.BikeMakeRepository.GetScooterMakeDescription: MakeId:{0}",makeId));
            }

            return objDesc;
        }
    }
}
