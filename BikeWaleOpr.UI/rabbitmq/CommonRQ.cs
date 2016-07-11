using BikeWaleOpr.Common;
using BikeWaleOPR.Utilities;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;

/// <summary>
/// Summary description for CommonRQ
/// </summary>
/// 
namespace BikeWaleOpr.RabbitMQ
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
        public string UploadImageToCommonDatabase(string photoId, string imageName, ImageCategories imgC, string directoryPath)
        {
            string url = string.Empty;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "img_allbikephotosinsert";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_itemid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], photoId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_origfilename", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], imageName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_categoryid", DbParamTypeMapper.GetInstance[SqlDbType.Int], imgC));
                    //die path for original image
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dirpath", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], directoryPath));
                    //host url for original image
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], ConfigurationManager.AppSettings["RabbitImgHostURL"].ToString()));
                    //output parameter complete url for original image
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_url", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 255, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    url = cmd.Parameters["par_url"].Value.ToString();
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("UploadImageProcessStart" + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                url = "sql exception" + ex.Message;
            } // catch Exception
            catch (Exception ex)
            {
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
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "img_fetchprocessedimagelist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_imagelist", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 1000, imageList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_categoryid", DbParamTypeMapper.GetInstance[SqlDbType.Int], imgC));

                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            return ds;
        }   //End of FetchProcessedImagesList

        public DataSet CheckImageStatus(string imageId, ImageCategories imgC)
        {
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "img_checkphotosstatus";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_photoid", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], imageId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_categoryid", DbParamTypeMapper.GetInstance[SqlDbType.Int], (int)imgC));

                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            return ds;
        }
    }   //End of Class   
}   //End of namespace