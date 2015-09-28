using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using Bikewale.Common;
using System.Data.SqlClient;

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
        public string SeriesId { get; set; }
        public string OriginalImagePath { get; set; }
        /// <summary>
        /// Getting makes only by providing only request type
        /// </summary>
        /// <param name="RequestType">Pass value as New or Used or Upcoming or PriceQuote or ALL</param>
        /// <returns></returns>
        public DataTable GetMakes(string RequestType)
        {
            DataTable dt = null;
            Database db = null;

            using (SqlCommand cmd = new SqlCommand("GetBikeMakes"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = RequestType;
                try
                {
                    db = new Database();
                    dt = db.SelectAdaptQry(cmd).Tables[0];
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
        /// Getting Models only by providing MakeId and request type
        /// </summary>
        /// <param name="MakeId"></param>
        /// <param name="RequestType">Pass value as New or Used or Upcoming or PriceQuote</param>
        /// <returns></returns>

        public DataTable GetModels(string MakeId, string RequestType)
        {
            DataTable dt = null;
            Database db = null;

            using (SqlCommand cmd = new SqlCommand("GetBikeModels"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = RequestType;
                cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = MakeId;

                try
                {
                    db = new Database();
                    DataSet ds = null;

                    ds = db.SelectAdaptQry(cmd);

                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
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
            Database db = null;
            DataTable dt = null;

            using (SqlCommand cmd = new SqlCommand("GetBikeVersions"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = RequestType;
                cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = ModelId;

                try
                {
                    db = new Database();
                    dt = db.SelectAdaptQry(cmd).Tables[0];
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
        /// Written By : Ashwini V. Todkar
        /// Getting Models with mapping name by providing MakeId and request type
        /// </summary>
        /// <param name="MakeId"></param>
        /// <param name="RequestType"></param>
        /// <returns></returns>
        public DataTable GetModelsWithMappingName(string MakeId, string RequestType)
        {
            DataTable dt = null;
            Database db = null;

            using (SqlCommand cmd = new SqlCommand("GetBikeModelsWithMappingName"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = RequestType;
                cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = MakeId;

                try
                {
                    db = new Database();
                    dt = db.SelectAdaptQry(cmd).Tables[0];
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
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// PopulateWhere will set makeid, make, model on the basis of model id
        /// </summary>
        /// <param name="modelId"></param>
        public void GetModelDetails(string modelId)
        {
            Database db = null;
            SqlConnection conn = null;

            try
            {
                db = new Database();
                conn = new SqlConnection(db.GetConString());

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetModelDetails";
                    cmd.Connection = conn;

                    HttpContext.Current.Trace.Warn("modelId : " + modelId);

                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = Configuration.GetDefaultCityId;    // Prices for default city in webconfig
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Make", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Model", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@IsFuturistic", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@IsNew", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@IsUsed", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@SmallPic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@LargePic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@HostURL", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MinPrice", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MaxPrice", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MakeMaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@OriginalImagePath", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    HttpContext.Current.Trace.Warn("qry success");

                    if (!string.IsNullOrEmpty(cmd.Parameters["@MakeId"].Value.ToString()))
                    {
                        ModelId = cmd.Parameters["@ModelId"].Value.ToString();
                        Model = cmd.Parameters["@Model"].Value.ToString();
                        MakeId = cmd.Parameters["@MakeId"].Value.ToString();
                        Make = cmd.Parameters["@Make"].Value.ToString();
                        IsFuturistic = Convert.ToBoolean(cmd.Parameters["@IsFuturistic"].Value);
                        IsNew = Convert.ToBoolean(cmd.Parameters["@IsNew"].Value);
                        IsUsed = Convert.ToBoolean(cmd.Parameters["@IsUsed"].Value);
                        SmallPic = cmd.Parameters["@SmallPic"].Value.ToString();
                        LargePic = cmd.Parameters["@LargePic"].Value.ToString();
                        HostUrl = cmd.Parameters["@HostURL"].Value.ToString();
                        MinPrice = cmd.Parameters["@MinPrice"].Value.ToString();
                        MaxPrice = cmd.Parameters["@MaxPrice"].Value.ToString();
                        ModelMappingName = cmd.Parameters["@MaskingName"].Value.ToString();
                        MakeMappingName = cmd.Parameters["@MakeMaskingName"].Value.ToString();
                        SeriesId = cmd.Parameters["@SeriesId"].Value.ToString();
                        OriginalImagePath = cmd.Parameters["@OriginalImagePath"].Value.ToString();
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
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
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
            Database db = null;
            SqlConnection conn = null;

            try
            {
                db = new Database();
                conn = new SqlConnection(db.GetConString());

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetVersionDetails";

                    HttpContext.Current.Trace.Warn("VersionId : " + versionId);
                    cmd.Parameters.Add("@VersionId", SqlDbType.Int).Value = versionId;
                    HttpContext.Current.Trace.Warn("VersionId1 : " + versionId);
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = Configuration.GetDefaultCityId;    // Prices for default city in webconfig
                    HttpContext.Current.Trace.Warn("VersionId2 : " + versionId);
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    HttpContext.Current.Trace.Warn("VersionId3 : " + versionId);
                    cmd.Parameters.Add("@Make", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Model", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Version", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@HostUrl", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@LargePic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@SmallPic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MinPrice", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MaxPrice", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Bike", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MakeMaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@OriginalImagePath", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    if (!String.IsNullOrEmpty(cmd.Parameters["@MakeId"].Value.ToString()))
                    {
                        VersionId = cmd.Parameters["@VersionId"].Value.ToString();
                        Version = cmd.Parameters["@Version"].Value.ToString();
                        ModelId = cmd.Parameters["@ModelId"].Value.ToString();
                        Model = cmd.Parameters["@Model"].Value.ToString();
                        MakeId = cmd.Parameters["@MakeId"].Value.ToString();
                        Make = cmd.Parameters["@Make"].Value.ToString();
                        BikeName = cmd.Parameters["@Bike"].Value.ToString();
                        HostUrl = cmd.Parameters["@HostUrl"].Value.ToString();
                        LargePic = cmd.Parameters["@LargePic"].Value.ToString();
                        SmallPic = cmd.Parameters["@SmallPic"].Value.ToString();
                        MinPrice = cmd.Parameters["@MinPrice"].Value.ToString();
                        MaxPrice = cmd.Parameters["@MaxPrice"].Value.ToString();
                        ModelMappingName = cmd.Parameters["@MaskingName"].Value.ToString();
                        MakeMappingName = cmd.Parameters["@MakeMaskingName"].Value.ToString();
                        OriginalImagePath = cmd.Parameters["@OriginalImagePath"].Value.ToString();
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
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
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

            string sql = "";

            sql = " SELECT Name AS MakeName, ID AS MakeId , MaskingName FROM BikeMakes With(NoLock) "
                + " WHERE ID = @makeId ";

            SqlDataReader dr = null;
            Database db = new Database();
            SqlParameter[] param = { new SqlParameter("@makeId", makeId) };

            try
            {
                dr = db.SelectQry(sql, param);

                if (dr.Read())
                {
                    Make = dr["MakeName"].ToString();
                    MakeId = dr["MakeId"].ToString();

                    BikeName = dr["MakeName"].ToString();
                    MakeMappingName = dr["MaskingName"].ToString();
                }

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
                db.CloseConnection();
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
                fullImagePath = "http://img.aeplcdn.com/bikewaleimg/common/nobike.jpg";
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

            if (String.IsNullOrEmpty(hostUrl) || String.IsNullOrEmpty(imagePath))
            {
                fullImagePath = "http://img.aeplcdn.com/bikewaleimg/common/nobike.jpg";
            }
            else
            {
                fullImagePath = Bikewale.Utility.Image.GetPathToShowImages(imagePath, hostUrl, size);
            }
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
            DataSet ds = null;
            Database db = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetPricequoteBuyingPreferences"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    db = new Database();
                    ds = db.SelectAdaptQry(cmd);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return null;
        }

    }   // End of class
}   // End of namespace