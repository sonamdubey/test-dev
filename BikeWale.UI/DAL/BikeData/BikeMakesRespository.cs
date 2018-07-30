using Bikewale.Entities.BikeData;
using Bikewale.Entities.SEO;
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
        /// Modified by sajal Gupta on 14-11-2017
        /// Description : Added MakeCategoryId
        /// Modified by : Rajan Chauhan on 11 Jan 2018
        /// Description : Added PhotosCount
        /// </summary>
        /// <param name="makeType">Type of bike data</param>
        /// <returns>Returns list of type BikeMakeEntityBase</returns>
        public List<BikeMakeEntityBase> GetMakesByType(EnumBikeType requestType)
        {
            List<BikeMakeEntityBase> objMakesList = null;


            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemakes_new_11012018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.Int32, requestType));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objMakesList = new List<BikeMakeEntityBase>();

                            while (dr.Read())
                            {
                                objMakesList.Add(new BikeMakeEntityBase()
                                {
                                    MakeId = SqlReaderConvertor.ToInt32(dr["ID"]),
                                    MakeName = Convert.ToString(dr["NAME"]),
                                    MaskingName = Convert.ToString(dr["MaskingName"]),
                                    HostUrl = Convert.ToString(dr["HostUrl"]),
                                    LogoUrl = Convert.ToString(dr["LogoUrl"]),
                                    PopularityIndex = SqlReaderConvertor.ToUInt16(dr["PopularityIndex"]),
                                    TotalCount = SqlReaderConvertor.ToUInt32(dr["ModelCount"]),
                                    PhotosCount = SqlReaderConvertor.ToUInt32(dr["PhotosCount"]),
                                    MakeCategoryId = SqlReaderConvertor.ToUInt16(dr["MakeCategoryId"])
                                });
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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

        public ICollection<MostPopularBikesBase> GetMostPopularBikesByModelBodyStyle(int modelId, int topCount, uint cityId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MostPopularBikesBase> GetMostPopularBikes(int? topCount = null, int? makeId = null)
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetById ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

                }
            }
            return dt;
        }
        /// <summary>
        /// Created by: Sangram Nandkhile on 24 Nov 2016
        /// Summarry; Fetch make list by UINT makeid (method overload)
        /// Modified by Sajal Gupta on 26-04-2017
        /// Description : Added is Scooter only flag
        /// Modified By : Deepak Israni on 20 April 2018
        /// Description : Versioned the SP to get IsNew and IsFuturistic flags.
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
                    cmd.CommandText = "getmakedetails_20042018";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            makeDetails = new BikeMakeEntityBase();
                            while (reader.Read())
                            {
                                makeDetails.MakeId = SqlReaderConvertor.ToInt32(reader["id"]);
                                makeDetails.MakeName = Convert.ToString(reader["name"]);
                                makeDetails.MaskingName = Convert.ToString(reader["maskingname"]);
                                makeDetails.HostUrl = Convert.ToString(reader["hosturl"]);
                                makeDetails.LogoUrl = Convert.ToString(reader["logourl"]);
                                makeDetails.IsScooterOnly = SqlReaderConvertor.ToBoolean(reader["isscooteronly"]);
                                makeDetails.IsNew = SqlReaderConvertor.ToBoolean(reader["New"]);
                                makeDetails.IsFuturistic = SqlReaderConvertor.ToBoolean(reader["Futuristic"]);
                            }

                            if (reader.NextResult())
                            {
                                var metas = new List<CustomPageMetas>();
                                while (reader.Read())
                                {
                                    var meta = new CustomPageMetas();
                                    meta.PageId = SqlReaderConvertor.ToUInt32(reader["pageid"]);
                                    meta.Title = Convert.ToString(reader["title"]);
                                    meta.Description = Convert.ToString(reader["description"]);
                                    meta.Keywords = Convert.ToString(reader["keywords"]);
                                    meta.Heading = Convert.ToString(reader["heading"]);
                                    meta.Summary = Convert.ToString(reader["summary"]);
                                    meta.MakeId = (uint)makeDetails.MakeId;
                                    metas.Add(meta);
                                }
                                makeDetails.Metas = metas;
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("BikeMakeEntityBase.GetMakeDetails(): makeId : {0}", makeId));

            }

            return makeDetails;
        }

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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(err, "GetAllMakeModels");

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
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.BikeMakesRepository.GetOldMaskingNames");

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
                ErrorClass.LogError(err, "BikeMakesRepository<T, U>.GetScooterMakes()");
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

                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.BikeData.BikeMakeRepository.GetScooterMakeDescription: MakeId:{0}", makeId));
            }

            return objDesc;
        }

        /// <summary>
        /// Created By: Snehal Dange on 22nd Nov 2017
        /// Description: To get sub foooter content and model price list on make page
        /// </summary>
        /// <param name="makeId"></param>
        public MakeSubFooterEntity GetMakeFooterCategoriesandPrice(uint makeId)
        {
            MakeSubFooterEntity footerContent = null;
            try
            {
                if (makeId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("getmakefootercategoriesandprice"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null)
                            {
                                footerContent = new MakeSubFooterEntity();
                                IList<MakeFooterCategory> makeSummary = new List<MakeFooterCategory>();
                                IList<BikeVersionPriceEntity> priceList = null;
                                while (dr.Read())
                                {
                                    makeSummary.Add(new MakeFooterCategory
                                        {

                                            CategoryId = SqlReaderConvertor.ToUInt32(dr["CategoryId"]),
                                            CategoryName = Convert.ToString(dr["CategoryName"]),
                                            CategoryDescription = Convert.ToString(dr["CategoryDescription"])
                                        }
                                        );
                                }

                                if (dr.NextResult())
                                {
                                    priceList = new List<BikeVersionPriceEntity>();

                                    while (dr.Read())
                                    {
                                        priceList.Add(new BikeVersionPriceEntity()
                                        {
                                            Make = new BikeMakeBase
                                            {
                                                MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]),
                                                MakeName = Convert.ToString(dr["Make"]),
                                                MakeMaskingName = Convert.ToString(dr["MakeMaskingName"])

                                            },
                                            Model = new BikeModelEntityBase
                                            {
                                                ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]),
                                                ModelName = Convert.ToString(dr["Model"]),
                                                MaskingName = Convert.ToString(dr["ModelMaskingName"])
                                            },
                                            VersionPrice = SqlReaderConvertor.ToInt32(dr["VersionPrice"])

                                        }
                                            );

                                    }
                                }
                                dr.Close();
                                footerContent.FooterDescription = makeSummary;
                                footerContent.ModelPriceList = priceList;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.DAL.BikeData.BikeMakeRepository.GetMakeFooterCategoriesandPrice: MakeId:{0}", makeId));
            }
            return footerContent;
        }

        /// <summary>
        /// Created By: Snehal Dange on 13th Dec 2017
        /// Descritpion: Method to get list of  makes in which dealer showroom is present for city
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikeMakeEntityBase> GetDealerBrandsInCity(uint cityId)
        {
            IList<BikeMakeEntityBase> objMakesList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerbrandsincity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, Convert.ToInt32(cityId)));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objMakesList = new List<BikeMakeEntityBase>();
                            while (dr.Read())
                            {
                                objMakesList.Add(new BikeMakeEntityBase
                                {
                                    MaskingName = Convert.ToString(dr["makemaskingname"]),
                                    MakeName = Convert.ToString(dr["makename"]),
                                    MakeId = SqlReaderConvertor.ToUInt16(dr["makeid"]),
                                    TotalCount = SqlReaderConvertor.ToUInt32(dr["dealerscount"]),
                                    HostUrl = Convert.ToString(dr["hosturl"]),
                                    LogoUrl = Convert.ToString(dr["logourl"]),
                                    IsScooterOnly = SqlReaderConvertor.ToBoolean(dr["isscooteronly"]),
                                    PopularityIndex = SqlReaderConvertor.ToUInt16(dr["PopularityIndex"]),
                                    MakeCategoryId = SqlReaderConvertor.ToUInt16(dr["MakeCategoryId"])

                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(" Bikewale.DAL.Dealer.GetDealerBrandsInCity_City_{0}", cityId));
            }
            return objMakesList;
        }

        /// <summary>
        /// Created By: Snehal Dange on 14th Dec 2017
        /// Descritpion: Method to get list of  makes in which service center is present for city
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikeMakeEntityBase> GetServiceCenterBrandsInCity(uint cityId)
        {
            IList<BikeMakeEntityBase> objMakesList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getservicecenterbrandsincity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, Convert.ToInt32(cityId)));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objMakesList = new List<BikeMakeEntityBase>();
                            while (dr.Read())
                            {
                                objMakesList.Add(new BikeMakeEntityBase
                                {
                                    MaskingName = Convert.ToString(dr["makemaskingname"]),
                                    MakeName = Convert.ToString(dr["makename"]),
                                    MakeId = SqlReaderConvertor.ToUInt16(dr["makeid"]),
                                    TotalCount = SqlReaderConvertor.ToUInt32(dr["servicecenterscount"]),
                                    HostUrl = Convert.ToString(dr["hosturl"]),
                                    LogoUrl = Convert.ToString(dr["logourl"]),
                                    IsScooterOnly = SqlReaderConvertor.ToBoolean(dr["isscooteronly"]),
                                    PopularityIndex = SqlReaderConvertor.ToUInt16(dr["PopularityIndex"]),
                                    MakeCategoryId = SqlReaderConvertor.ToUInt16(dr["MakeCategoryId"])

                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(" Bikewale.DAL.Dealer.GetServiceCenterBrandsInCity_City_{0}", cityId));
            }
            return objMakesList;
        }

        /// <summary>
        /// Created by : Snehal Dange on 18th Jan 2018
        /// Description : Created as a common method to get 'research more about make' details when city is present or not.
        /// Modified by : Sanskar Gupta on 31st Jan 2018
        /// Description : Added logic to fetch 'ScootersCount' from the SP.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        private ResearchMoreAboutMake GetResearchMoreAboutMakeDetails(string spName, uint makeId, uint cityId = 0)
        {
            ResearchMoreAboutMake obj = null;
            IList<BikeSeriesEntity> objSeriesList = null;
            try
            {
                if (makeId > 0)
                {

                    using (DbCommand cmd = DbFactory.GetDBCommand(spName))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                        if (cityId > 0)
                        {
                            cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                        }

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null && dr.Read())
                            {
                                obj = new ResearchMoreAboutMake();
                                obj.Make = new BikeMakeEntityBase
                                {
                                    MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]),
                                    MakeName = Convert.ToString(dr["MakeName"]),
                                    MaskingName = Convert.ToString(dr["MakeMaskingName"])
                                };
                                obj.ScootersCount = SqlReaderConvertor.ToInt32(dr["totalscooterscount"]);
                                obj.IsScooterOnlyMake = SqlReaderConvertor.ToBoolean(dr["isscooteronly"]);
                                if (cityId > 0)
                                {
                                    obj.DealerShowroomCount = SqlReaderConvertor.ToUInt16(dr["dealerscount"]);
                                    obj.ServiceCentersCount = SqlReaderConvertor.ToUInt16(dr["servicecenterscount"]);
                                    obj.UsedBikesCount = SqlReaderConvertor.ToUInt16(dr["usedbikescount"]);
                                }

                                obj.TotalDealerShowroomCount = SqlReaderConvertor.ToUInt16(dr["totaldealerscount"]);
                                obj.TotalServiceCentersCount = SqlReaderConvertor.ToUInt16(dr["totalservicecenterscount"]);
                                obj.TotalUsedBikesCount = SqlReaderConvertor.ToUInt16(dr["totalusedbikescount"]);
                                if (dr.NextResult())
                                {
                                    objSeriesList = new List<BikeSeriesEntity>();

                                    while (dr.Read())
                                    {
                                        objSeriesList.Add(new BikeSeriesEntity
                                        {
                                            SeriesId = SqlReaderConvertor.ToUInt16(dr["seriesid"]),
                                            SeriesName = Convert.ToString(dr["seriesname"]),
                                            MaskingName = Convert.ToString(dr["seriesmaskingname"])
                                        }
                                            );
                                    }


                                }
                                obj.SeriesList = objSeriesList;
                                dr.Close();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.GetResearchMoreAboutMakeDetails:Makeid {0} ,CityId {1},", makeId, cityId));
            }
            return obj;

        }
        /// <summary>
        /// Created by : Snehal Dange on 16th Jan 2017
        /// Description: Method to get ResearchMoreAboutMake widget data .
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public ResearchMoreAboutMake ResearchMoreAboutMake(uint makeId)
        {
            ResearchMoreAboutMake obj = null;
            try
            {
                if (makeId > 0)
                {
                    obj = GetResearchMoreAboutMakeDetails("researchmoreaboutmake_01022018", makeId);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.ResearchMoreAboutMake:Makeid_{0}", makeId));
            }
            return obj;
        }

        /// <summary>
        /// Created by : Snehal Dange on 16th Jan 2017
        /// Description: Method to get ResearchMoreAboutMake widget data  when city is present
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ResearchMoreAboutMake ResearchMoreAboutMakeByCity(uint makeId, uint cityId)
        {
            ResearchMoreAboutMake obj = null;
            try
            {
                if (makeId > 0 && cityId > 0)
                {
                    obj = GetResearchMoreAboutMakeDetails("researchmoreaboutmakebycity_01022018", makeId, cityId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.ResearchMoreAboutMakeByCity: Makeid- {0},CityId- {1}", makeId, cityId));
            }
            return obj;
        }

        /// <summary>
        /// Created By : Deepak Israni on 9th Feb 2018
        /// Description : To get the total expert review count and number of models with expert reviews
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public ExpertReviewCountEntity GetExpertReviewCountByMake(uint makeId)
        {
            ExpertReviewCountEntity obj = null;
            String spName = "getexpertreviewcountbymake";

            try
            {
                if (makeId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand(spName))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_bikemakeid", DbType.Int32, makeId));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null && dr.Read())
                            {
                                obj = new ExpertReviewCountEntity()
                                {
                                    MakeId = makeId,
                                    ModelCount = SqlReaderConvertor.ToUInt32(dr["ModelCount"]),
                                    ExpertReviewCount = SqlReaderConvertor.ToUInt32(dr["ExpertReviewCount"])
                                };

                                dr.Close();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.GetExpertReviewCountByMake: Makeid- {0}", makeId));
            }

            return obj;
        }
    }
}
