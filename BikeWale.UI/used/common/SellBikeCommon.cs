using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Bikewale.Common;
using System.Data.Common;
using Bikewale.Notifications.CoreDAL;

namespace Bikewale.Used
{
    /// <summary>
    /// Wriiten By : Ashish G. Kamble on 23/8/2012
    /// Class contains common functions for sell bike
    /// </summary>
    public class SellBikeCommon
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="imageUrlFull"></param>
        /// <param name="imageUrlThumb"></param>
        /// <param name="imageUrlThumbSmall"></param>
        /// <param name="description"></param>
        /// <param name="isDealer"></param>
        /// <param name="isMain"></param>
        /// <returns></returns>
        public string SaveBikePhotos(string inquiryId, string originalImageName, string description, bool isDealer, bool isMain)
        {
            string photoId = "";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_bikephotos_insert"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_description", DbType.String, 200, description));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_directorypath", DbType.String, 200, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isreplicated", DbType.Boolean,Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_originalimagepath", DbType.String, 300, originalImageName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isdealer", DbType.Boolean, isDealer));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ismain", DbType.Boolean, isMain));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbType.String, 100, ConfigurationManager.AppSettings["imgHostURL"]));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_photoid", DbType.Int64, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd);

                    photoId = cmd.Parameters["par_photoid"].Value.ToString(); 
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            return photoId;
        }   // End of savebikephotos method

        public bool RemoveBikePhotos(string inquiryId, string photoId)
        {
            bool isRemoved = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_bikephotos_remove"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_photoid", DbType.Int64, photoId));

                    MySqlDatabase.ExecuteNonQuery(cmd);

                    isRemoved = true; 
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception

            return isRemoved;
        }   // End of RemoveBikePhotos

        public bool MakeMainImage(string inquiryId, string photoId, bool isDealer)
        {
            bool isMain = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_bikephotos_mainimage"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_photoid", DbType.Int64, photoId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isdealer", DbType.Boolean, isDealer));

                    MySqlDatabase.ExecuteNonQuery(cmd);

                    isMain = true; 
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception

            return isMain;
        }   // End of MakeMainImage method


        public bool AddImageDescription(string photoId, string imgDesc)
        {
            bool isAdded = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_bikephotos_desc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_photoid", DbType.Int64, photoId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_imgdesc", DbType.String, 200, imgDesc));

                    MySqlDatabase.ExecuteNonQuery(cmd);

                    isAdded = true; 
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception

            return isAdded;
        }

        /// <summary>
        ///     Update customer as a verified customer and list the bike on the bike search page
        /// </summary>
        public void UpdateIsVerifiedCustomer(string sellInquiryId)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "classified_upadateverifiedlisting";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, sellInquiryId));


                    MySqlDatabase.UpdateQuery(cmd);
                    HttpContext.Current.Trace.Warn("update success verified...");
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("UpdateIsVerifiedCustomer sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("UpdateIsVerifiedCustomer ex: " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }   

    }   // End of class
}   // End of namespace