using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Getting All details of make model and versions of the bikes
/// </summary>

namespace Bikewale.Common
{
    /// <summary>
    ///     Class written for make, model and version operations
    /// </summary>
    public class MakeModelVersion
    {
        // Properties written 
        public string MakeId { get; set; }
        public string Make { get; set; }
        public string ModelId { get; set; }
        public string Model { get; set; }
        public string VersionId { get; set; }
        public string Version { get; set; }
        public bool IsFuturistic { get; set; }
        public bool IsNew { get; set; }
        public bool IsUsed { get; set; }
        public string SmallPic { get; set; }
        public string LargePic { get; set; }
        public string BikeName { get; set; }
        public string HostUrl { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string ModelMappingName { get; set; }
        public string MakeMappingName { get; set; }
        public string OriginalImagePath { get; set; }
        /// <summary>
        /// Getting makes only by providing only request type
        /// </summary>
        /// <param name="RequestType">Pass value as New or Used or Upcoming or PriceQuote or ALL</param>
        /// <returns></returns>
        public DataTable GetMakes(string RequestType)
        {
            DataTable dt = null;
            IEnumerable<Entities.BikeData.BikeMakeEntityBase> makes = null;
            EnumBikeType _requestType = EnumBikeType.All;

            try
            {
                if (Enum.TryParse(RequestType, true, out _requestType))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                                         .RegisterType<ICacheManager, MemcacheManager>()
                                         .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                                        ;
                        var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                        makes = objCache.GetMakesByType(_requestType);


                        var _makeList = (from mk in makes select new { Text = mk.MakeName, Value = mk.MakeId });

                        dt = new DataTable();

                        dt.Columns.Add("Text");
                        dt.Columns.Add("Value");
                        foreach (var make in _makeList)
                        {
                            dt.Rows.Add(make);
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
            return dt;
        }

        /// <summary>
        ///  Getting makes only by providing only request type
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="drpDownList"></param>
        public void GetMakes(EnumBikeType requestType, ref DropDownList drpDownList)
        {

            IEnumerable<Entities.BikeData.BikeMakeEntityBase> makes = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                                     .RegisterType<ICacheManager, MemcacheManager>()
                                     .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                                    ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                    makes = objCache.GetMakesByType(requestType);


                    if (makes != null && makes.Count() > 0)
                    {
                        if (drpDownList != null)
                        {
                            if (requestType == EnumBikeType.Used || requestType == EnumBikeType.UserReviews)
                            {
                                drpDownList.DataSource = makes.Select(a => new { Value = string.Format("{0}_{1}", a.MakeId, a.MaskingName), Text = a.MakeName, Id = a.MakeId });
                            }
                            else
                            {
                                drpDownList.DataSource = makes.Select(a => new { Value = a.MakeId, Text = a.MakeName });
                            }


                            drpDownList.DataValueField = "Value";
                            drpDownList.DataTextField = "Text";

                            drpDownList.DataBind();
                            drpDownList.Items.Insert(0, new ListItem("--Select Make--", "0"));
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

        }

        /// <summary>
        /// Getting Models only by providing MakeId and request type
        /// </summary>
        /// <param name="MakeId"></param>
        /// <param name="RequestType">Pass value as New or Used or Upcoming or PriceQuote</param>
        /// <returns></returns>

        public DataTable GetModels(string MakeId, string RequestType)
        {
            DataTable dt = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemodels"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, RequestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, MakeId));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
                    }
                }

            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + " : Make Id : " + MakeId + ", Request Type : " + RequestType + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return dt;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Getting Versions only by providing ModelId and request type
        /// </summary>
        /// <param name="ModelId"></param>
        /// <param name="RequestType">Pass value as New or Used or Upcoming or PriceQuote</param>
        /// <returns></returns>
        public DataTable GetVersions(string ModelId, string RequestType)
        {
            DataTable dt = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikeversions"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, RequestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, ModelId));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
                    }
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

            return dt;
        }
        /// <summary>
        /// Written By : Ashwini V. Todkar
        /// Getting Models with mapping name by providing MakeId and request type
        /// </summary>
        /// <param name="MakeId"></param>
        /// <param name="RequestType"></param>
        /// <returns></returns>
        public DataTable GetModelsWithMappingName(string MakeId, string RequestType)
        {
            DataTable dt = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemodelswithmappingname"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, RequestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, MakeId));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
                    }
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
            return dt;
        }
        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// PopulateWhere will set makeid, make, model on the basis of model id
        /// </summary>
        /// <param name="modelId"></param>
        public void GetModelDetails(string modelId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodeldetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, Configuration.GetDefaultCityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_make", DbType.String, 30, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_model", DbType.String, 30, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isfuturistic", DbType.Boolean, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_smallpic", DbType.String, 50, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_largepic", DbType.String, 50, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbType.String, 50, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_minprice", DbType.String, 50, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maxprice", DbType.String, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbType.String, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makemaskingname", DbType.String, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isnew", DbType.Boolean, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isused", DbType.Boolean, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_originalimagepath", DbType.String, 150, ParameterDirection.InputOutput));

                    // Bikewale.Notifications.// LogLiveSps.LogSpInGrayLog(cmd);

                    if (MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly) > 0)
                    {
                        HttpContext.Current.Trace.Warn("qry success");

                        if (!string.IsNullOrEmpty(cmd.Parameters["par_makeid"].Value.ToString()))
                        {
                            ModelId = Convert.ToString(cmd.Parameters["par_modelid"].Value);
                            Model = cmd.Parameters["par_model"].Value.ToString();
                            MakeId = Convert.ToString(cmd.Parameters["par_makeid"].Value);
                            Make = cmd.Parameters["par_make"].Value.ToString();
                            IsFuturistic = Convert.ToBoolean(cmd.Parameters["par_isfuturistic"].Value);
                            IsNew = Convert.ToBoolean(cmd.Parameters["par_isnew"].Value);
                            IsUsed = Convert.ToBoolean(cmd.Parameters["par_isused"].Value);
                            SmallPic = cmd.Parameters["par_smallpic"].Value.ToString();
                            LargePic = cmd.Parameters["par_largepic"].Value.ToString();
                            HostUrl = cmd.Parameters["par_hosturl"].Value.ToString();
                            MinPrice = Convert.ToString(cmd.Parameters["par_minprice"].Value);
                            MaxPrice = Convert.ToString(cmd.Parameters["par_maxprice"].Value);
                            ModelMappingName = cmd.Parameters["par_maskingname"].Value.ToString();
                            MakeMappingName = cmd.Parameters["par_makemaskingname"].Value.ToString();
                            OriginalImagePath = Convert.ToString(cmd.Parameters["par_originalimagepath"].Value);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetModelDetails sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetModelDetails ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // End of GetMakeFromModelId method

        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// PopulateWhere will set makeid, make, model id, model, version id, version on the basis of version id
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public void GetVersionDetails(string versionId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getversiondetails";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, Configuration.GetDefaultCityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_make", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_model", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_version", DbType.String, 60, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbType.String, 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_largepic", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_smallpic", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_minprice", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maxprice", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bike", DbType.String, 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makemaskingname", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_originalimagepath", DbType.String, 150, ParameterDirection.Output));
                    //Bikewale.Notifications.// LogLiveSps.LogSpInGrayLog(cmd);

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    if (!String.IsNullOrEmpty(cmd.Parameters["par_makeid"].Value.ToString()))
                    {
                        VersionId = cmd.Parameters["par_VersionId"].Value.ToString();
                        Version = cmd.Parameters["par_version"].Value.ToString();
                        ModelId = cmd.Parameters["par_modelid"].Value.ToString();
                        Model = cmd.Parameters["par_model"].Value.ToString();
                        MakeId = cmd.Parameters["par_makeid"].Value.ToString();
                        Make = cmd.Parameters["par_make"].Value.ToString();
                        BikeName = cmd.Parameters["par_bike"].Value.ToString();
                        HostUrl = cmd.Parameters["par_hosturl"].Value.ToString();
                        LargePic = cmd.Parameters["par_largepic"].Value.ToString();
                        SmallPic = cmd.Parameters["par_smallpic"].Value.ToString();
                        MinPrice = cmd.Parameters["par_minprice"].Value.ToString();
                        MaxPrice = cmd.Parameters["par_maxprice"].Value.ToString();
                        ModelMappingName = cmd.Parameters["par_maskingname"].Value.ToString();
                        MakeMappingName = cmd.Parameters["par_makemaskingname"].Value.ToString();
                        OriginalImagePath = cmd.Parameters["par_originalimagepath"].Value.ToString();
                    }
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
        }   // End of GetVersionDetails method

        /// <summary>
        ///     Get Makeid and make name from the make id
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public string GetMakeDetails(string makeId)
        {

            // Validate the makeId
            if (!CommonOpn.IsNumeric(makeId))
                return "";

            string sql = " select name as makename, id as makeid , maskingname from bikemakes   where id = @makeid ";


            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            Make = dr["MakeName"].ToString();
                            MakeId = dr["MakeId"].ToString();

                            BikeName = dr["MakeName"].ToString();
                            MakeMappingName = dr["MaskingName"].ToString();

                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return Make;
        }   // End of getMakeDetails

        /// <summary>
        ///     Written By : Ashish G. Kamble on 13/9/2012
        ///     Function will return the fullimage path if hosturl or imagepath is not null.
        ///     Else will return the nobike image
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static string GetModelImage(string hostUrl, string imagePath)
        {
            string fullImagePath = string.Empty;

            if (String.IsNullOrEmpty(hostUrl) || String.IsNullOrEmpty(imagePath))
            {
                fullImagePath = "http://imgd3.aeplcdn.com/0x0/bikewaleimg/images/noimage.png";
            }
            else
            {
                fullImagePath = ImagingFunctions.GetPathToShowImages(imagePath, hostUrl);
            }
            return fullImagePath;
        }   // End of GetModelImage function

        public static string GetModelImage(string hostUrl, string imagePath, string size)
        {
            string fullImagePath = string.Empty;

            fullImagePath = Bikewale.Utility.Image.GetPathToShowImages(imagePath, hostUrl, size);

            return fullImagePath;
        }

        /// <summary>
        ///     Written By : Ashish G. Kamble on 27 sept 2012
        ///     If price is not available show N/A else show original price
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static string GetFormattedPrice(string price)
        {
            if (String.IsNullOrEmpty(price) || price == "0")
                return "N/A";
            else
                return CommonOpn.FormatNumeric(price);
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 28 April 2014
        /// Summary    : PopulateWhere to get buyng preferences of user
        /// </summary>
        /// <returns>datatable containing id and buying preferences like 1 week or just researching</returns>
        public DataTable GetBuyingPreference()
        {
            ErrorClass objErr = new ErrorClass(new Exception("Method not used/commented"), "MakeModelVersion.GetBuyingPreference");
            objErr.SendMail();
            return null;

            //DataSet ds = null;
            //Database db = null;

            //try
            //{
            //    using (SqlCommand cmd = new SqlCommand("GetPricequoteBuyingPreferences"))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        db = new Database();
            //        ds = db.SelectAdaptQry(cmd);

            //        if (ds != null && ds.Tables[0].Rows.Count > 0)
            //        {
            //            return ds.Tables[0];
            //        }
            //    }
            //}
            //catch (SqlException ex)
            //{
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception ex)
            //{
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //return null;
        }

    }   // End of class
}   // End of namespace