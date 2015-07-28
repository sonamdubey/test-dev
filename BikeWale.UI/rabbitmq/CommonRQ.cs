using Bikewale.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;

/// <summary>
/// Summary description for CommonRQ
/// </summary>
/// 
namespace Bikewale.RabbitMQ
{
    public class BikeCommonRQ
    {
        #region enum decs
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        #endregion

        /// <summary>
        /// Created by       :   Chetan dev
        /// Modified Date    :   04 jan,2013
        /// Desc             :   Inserting bikephotos for Stock in IMG_AllBikephotos table
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="imageName"></param>
        /// <param name="category"></param>
        /// <param name="directoryPath"></param>
        public static string UploadImageToCommonDatabase(string photoId, string imageName, ImageCategories imgC, string directoryPath)
        {
            Database db = new Database();

            string url = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "IMG_AllBikePhotosInsert";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@ItemId", SqlDbType.BigInt).Value = photoId;
                        cmd.Parameters.Add("@OrigFileName", SqlDbType.VarChar).Value = imageName;
                        cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = imgC;
                        HttpContext.Current.Trace.Warn("category id",imgC.ToString());
                        //die path for original image
                        cmd.Parameters.Add("@DirPath", SqlDbType.VarChar).Value = directoryPath;
                        //host url for original image
                        cmd.Parameters.Add("@HostUrl", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings["RabbitImgHostURL"].ToString();
                        //output parameter complete url for original image
                        cmd.Parameters.Add("@Url", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output;
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        cmd.ExecuteNonQuery();
                        url = cmd.Parameters["@Url"].Value.ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("category id", imgC.ToString());
                HttpContext.Current.Trace.Warn("UploadImageProcessStart" + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                url = "sql exception" + ex.Message;
            } // catch Exception
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("category id", imgC.ToString());
                HttpContext.Current.Trace.Warn("UploadImageProcessStart" + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                url = "exception" + ex.Message;
            } // catch Exception
            return url;
        }   //End of UploadImageToCommonDatabase

        /// <summary>
        /// Summary : To get Id, HostUrl, DirectoryPth, ImageUrlThumbSmall of image
        /// </summary>
        /// <param name="imageList">List of Pending Image ID </param>
        /// <param name="imgC">category ID</param>
        /// <returns></returns>
        public DataSet FetchProcessedImagesList(string imageList, ImageCategories imgC)
        {
            //verify that all the values in the image list is numeric
            SqlCommand cmd;
            Database db = new Database();
            DataSet ds = null;
            try
            {
                cmd = new SqlCommand("IMG_FetchProcessedImageList");
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ImageList", SqlDbType.VarChar,1000).Value = imageList;
                cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = imgC;

                ds = db.SelectAdaptQry(cmd);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                //close the connection  
                db.CloseConnection();
            }

            return ds;
        }   //End of FetchProcessedImagesList

    }   //End of Class   
}   //End of namespace