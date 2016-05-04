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

            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();

            string conStr = db.GetConString();
            con = new SqlConnection(conStr);

            try
            {
                cmd = new SqlCommand("Classified_BikePhotos_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt);
                prm.Value = inquiryId;

                prm = cmd.Parameters.Add("@Description", SqlDbType.VarChar, 200);
                prm.Value = description;

                prm = cmd.Parameters.Add("@OriginalImagePath", SqlDbType.VarChar, 300);
                prm.Value = originalImageName;

                prm = cmd.Parameters.Add("@IsDealer", SqlDbType.Bit);
                prm.Value = isDealer;

                prm = cmd.Parameters.Add("@IsMain", SqlDbType.Bit);
                prm.Value = isMain;

                prm = cmd.Parameters.Add("@HostURL", SqlDbType.VarChar, 100);
                prm.Value = ConfigurationManager.AppSettings["imgHostURL"];

                // Out put parameter
                prm = cmd.Parameters.Add("@PhotoId", SqlDbType.BigInt);
                prm.Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                photoId = cmd.Parameters["@PhotoId"].Value.ToString();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                //close the connection	
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return photoId;
        }   // End of savebikephotos method

        public bool RemoveBikePhotos(string inquiryId, string photoId)
        {
            bool isRemoved = false;

            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();

            string conStr = db.GetConString();
            con = new SqlConnection(conStr);

            try
            {
                cmd = new SqlCommand("Classified_BikePhotos_Remove", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt);
                prm.Value = inquiryId;

                prm = cmd.Parameters.Add("@PhotoId", SqlDbType.BigInt);
                prm.Value = photoId;

                con.Open();
                cmd.ExecuteNonQuery();

                isRemoved = true;
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                //close the connection	
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return isRemoved;
        }   // End of RemoveBikePhotos

        public bool MakeMainImage(string inquiryId, string photoId, bool isDealer)
        {
            bool isMain = false;

            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();

            string conStr = db.GetConString();
            con = new SqlConnection(conStr);

            try
            {
                cmd = new SqlCommand("Classified_BikePhotos_MainImage", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt);
                prm.Value = inquiryId;

                prm = cmd.Parameters.Add("@PhotoId", SqlDbType.BigInt);
                prm.Value = photoId;

                prm = cmd.Parameters.Add("@IsDealer", SqlDbType.Bit);
                prm.Value = isDealer;

                con.Open();
                cmd.ExecuteNonQuery();

                isMain = true;
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                //close the connection	
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return isMain;
        }   // End of MakeMainImage method


        public bool AddImageDescription(string photoId, string imgDesc)
        {
            bool isAdded = false;

            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();

            string conStr = db.GetConString();
            con = new SqlConnection(conStr);

            try
            {
                cmd = new SqlCommand("Classified_BikePhotos_Desc", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@PhotoId", SqlDbType.BigInt);
                prm.Value = photoId;

                prm = cmd.Parameters.Add("@ImgDesc", SqlDbType.VarChar, 200);
                prm.Value = imgDesc;

                con.Open();
                cmd.ExecuteNonQuery();

                isAdded = true;
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                //close the connection	
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return isAdded;
        }

        /// <summary>
        ///     Update customer as a verified customer and list the bike on the bike search page
        /// </summary>
        public void UpdateIsVerifiedCustomer(string sellInquiryId)
        {
            HttpContext.Current.Trace.Warn("UpdateIsVerifiedCustomer sellinquiryid: ", sellInquiryId);

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "classified_upadateverifiedlisting";

                    //cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt).Value = sellInquiryId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], sellInquiryId));

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