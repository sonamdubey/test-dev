using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using BikeWaleOpr.Common;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using RabbitMqPublishing;
using BikeWaleOpr.RabbitMQ;

namespace BikeWaleOpr.Common
{
    /// <summary>
    /// Created By : Ashwini Todkar on 24 Feb 2014
    /// </summary>
    public class ManageBikeSeries
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 24 Feb 2014
        /// Summary    : function to retrive all series of a make 
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public DataSet GetSeries(string makeId)
        {
            Database db = null;
            DataSet ds = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBikeSeries"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("Sql Exception in GetMakeSeries: ", sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception in GetMakeSeries: ", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            return ds;
        }//End of GetMakeSeries()

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Feb 2014
        /// Summary    : function to add new make series and also update series details 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maskingName"></param>
        /// <param name="makeId"></param>
        /// <param name="id" > if id = -1 then it adds record to BikeSeries table else update particular bike series</param>
        public bool SaveSeries(string name,string maskingName,string makeId)
        {
            Database db = null;
            bool isSuccess = false;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SaveBikeSeries";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 30).Value = name;
                    cmd.Parameters.Add("@MaskingName", SqlDbType.VarChar, 50).Value = maskingName;
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    cmd.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = BikeWaleAuthentication.GetOprUserId();

                    db = new Database();
                    isSuccess = db.InsertQry(cmd);               
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("Sql Exception in SaveSeries", sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception in SaveSeries", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }

            return isSuccess;
        }//End of SaveSeries()

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Feb 2014
        /// Summary    : function to update series details like masking name and series name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maskingName"></param>
        /// <param name="seriesId"></param>
        public bool UpdateSeries(string name, string maskingName, string seriesId)
        {
            Database db = null;
            bool isSuccess = false;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "UpdateBikeSeries";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 30).Value = name;
                    cmd.Parameters.Add("@MaskingName", SqlDbType.VarChar, 50).Value = maskingName;
                    cmd.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = BikeWaleAuthentication.GetOprUserId();
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = seriesId;
                    db = new Database();

                    isSuccess = db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("Sql Exception in UpdateSeries", sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception in UpdateSeries", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }

            return isSuccess;
        }//End of SaveSeries()

        /// <summary>
        /// Written By : Ashwini Todkar on 25 Feb 2014
        /// Summary    : Function to delete series from BikeSeries table
        /// </summary>
        /// <param name="seriesId"></param>
        public void DeleteSeries(string seriesId)
        {
            Database db = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "DeleteSeries";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = seriesId;

                    db = new Database();

                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("Sql Exception in DeleteSeries", sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception in DeleteSeries", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
        }//End of DeleteSeries

        /// <summary>
        /// Written By : Ashwini Todkar on 25 Feb 2014
        /// summary    : method to publish image in rabbitMQ queue
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="imageUrl"></param>
        private void UploadSeriesPhoto(string seriesId, string imageName)
        {
            //get RabittMQ server ip address/host
            string hostUrl = ConfigurationManager.AppSettings["RabbitImgHostURL"].ToString();

            string imageUrl = "http://" + hostUrl + "/bw/series/" + imageName + "-" + seriesId + ".jpg";

            //RabbitMq Publish code here
            RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ID).ToLower(), seriesId);
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CATEGORY).ToLower(), "BIKESERIES");
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl);
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEWIDTH).ToLower(), "-1");
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEHEIGHT).ToLower(), "-1");
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISWATERMARK).ToLower(), Convert.ToString(false));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISCROP).ToLower(), Convert.ToString(false));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISMAIN).ToLower(), Convert.ToString(false));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(true));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISMASTER).ToLower(), "1");
            rabbitmqPublish.PublishToQueue(ConfigurationManager.AppSettings["ImageQueueName"], nvc);
        }

        /// Written By : Ashwini Todkar on 25th Feb 2014
        /// Summary : To save Image Path in Database
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="imageName"></param>
     private void SaveImagePathToDB(string seriesId,string imageName)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    Database db = new Database();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SaveBikeSeriesPhotos";
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = seriesId;
                    cmd.Parameters.Add("@HostUrl", SqlDbType.VarChar, 100).Value = ConfigurationManager.AppSettings["imgHostURL"];
                    cmd.Parameters.Add("@OriginalImageUrl", SqlDbType.VarChar, 150).Value = "/bw/series/" + imageName + "-" + seriesId + ".jpg?" + CommonOpn.GetTimeStamp();
                    cmd.Parameters.Add("@IsReplicated", SqlDbType.Bit).Value = 0;

                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("SavePhoto sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SavePhoto ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 25th Feb 2014
        /// summary  : method set hosturl,small and large pic url in database abd also publish image to queue
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="imageName"></param>
        public void SaveSeriesPhoto(string seriesId, string imageName)
        {
            try
            {
                // Save image path to database
                SaveImagePathToDB(seriesId,imageName);
                // Publish image to rabitmq for replication
                UploadSeriesPhoto(seriesId,imageName);
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("SaveSeriesPhoto  ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Written By : Ashwini Todkar on 25th Feb 2014
        /// Summary    : function to get details of a series HostUrl,SmallPic,LargePic,MakeMaskingName,SeriesMaskingName etc
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public DataSet GetSeriesDetails(string seriesId)
        {
            Database db = null;
            DataSet ds = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetBikeSeriesInfo";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = seriesId;
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("SqlEx in GetSeriesDetails : " + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Ex in GetSeriesDetails: " + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {              
                db.CloseConnection();
            }
            return ds;
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 28th Feb 2014
        /// Summary : To get Series List
        /// </summary>
        /// <param name="MakeId"></param>
        /// <returns></returns>
        public DataTable GetSeriesDdl(string MakeId)
        {
            Database db = null;
            DataTable dt = null;

            using (SqlCommand cmd = new SqlCommand("GetModelSeries"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
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
        }   //End of GetSeriesDdl

        public void UpdateModelSeries(string seriesId, string modelIdList)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    Database db = new Database();
                    cmd.CommandText = "UpdateModelSeries";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = seriesId;
                    cmd.Parameters.Add("@ModelIdList", SqlDbType.VarChar, 500).Value = modelIdList;

                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of UpdateModelSeries

        /// <summary>
        /// Written By : Sadhana Upadhyay on 27th Feb 2014
        /// Summary    : Method to save or update the Series synopsis.
        /// </summary>
        /// <param name="makeId">Id of the make whose synopsis is to be updated.</param>
        public void ManageSeriesSynopsis(string seriesId, string synopsis)
        {
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "ManageSeriesSynopsis";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = seriesId;
                    cmd.Parameters.Add("@Discription", SqlDbType.VarChar).Value = synopsis.Trim();
                    cmd.Parameters.Add("@UserId", SqlDbType.BigInt).Value = CurrentUser.Id;

                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("ManageSeriesSynopsis Sql Error : ", sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ManageSeriesSynopsis Exception : ", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
        }   // End of ManageSeriesSynopsis


        /// <summary>
        /// Written By : Sadhana Upadhyay on 27th Feb 2014
        /// Summary    : Method to fill data in textbox when synopsis exists for BikeSeries
        /// </summary>
        /// <param name="makeId"></param>
        public void GetSeriesSynopsis(string seriesId, ref string series, ref string synopsis, ref string makeName)
        {
            Database db = null;

            try
            {
                db = new Database();

                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "GetSeriesSynopsis";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = seriesId;
                        cmd.Parameters.Add("@Series", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Synopsis", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MakeName", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output;

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        series = cmd.Parameters["@Series"].Value.ToString();
                        synopsis = cmd.Parameters["@Synopsis"].Value.ToString();
                        makeName = cmd.Parameters["@MakeName"].Value.ToString();

                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("GetSeriesSynopsis Sql Error : ", sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetSeriesSynopsis Exception : ", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
        }   // End of GetSeriesSynopsis
    }//End of class ManageBikeSeries
}//End of Namespace