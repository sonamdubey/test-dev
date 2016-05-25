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
using System.Data.Common;
using BikeWaleOPR.DAL.CoreDAL;
using BikeWaleOPR.Utilities;

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
            DataSet ds = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikeseries"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], makeId));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
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
        public bool SaveSeries(string name, string maskingName, string makeId)
        {

            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "savebikeseries";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, name));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, maskingName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbParamTypeMapper.GetInstance[SqlDbType.Int], BikeWaleAuthentication.GetOprUserId()));

                    isSuccess = MySqlDatabase.InsertQuery(cmd);
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
            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatebikeseries";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, name));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, maskingName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbParamTypeMapper.GetInstance[SqlDbType.Int], BikeWaleAuthentication.GetOprUserId()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbParamTypeMapper.GetInstance[SqlDbType.Int], seriesId));

                    isSuccess = MySqlDatabase.UpdateQuery(cmd);
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

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "deleteseries";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesid", DbParamTypeMapper.GetInstance[SqlDbType.Int], seriesId));

                    MySqlDatabase.UpdateQuery(cmd);
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
        private void SaveImagePathToDB(string seriesId, string imageName)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "savebikeseriesphotos";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbParamTypeMapper.GetInstance[SqlDbType.Int], seriesId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, ConfigurationManager.AppSettings["imgHostURL"]));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_originalimageurl", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 150, "/bw/series/" + imageName + "-" + seriesId + ".jpg?" + CommonOpn.GetTimeStamp()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isreplicated", DbParamTypeMapper.GetInstance[SqlDbType.Bit], 0));

                    MySqlDatabase.UpdateQuery(cmd);
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
                SaveImagePathToDB(seriesId, imageName);
                // Publish image to rabitmq for replication
                UploadSeriesPhoto(seriesId, imageName);
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
            DataSet ds = null;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getbikeseriesinfo";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesid", DbParamTypeMapper.GetInstance[SqlDbType.Int], seriesId));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
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
            DataTable dt = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelseries"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], MakeId));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
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
        }   //End of GetSeriesDdl

        public void UpdateModelSeries(string seriesId, string modelIdList)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatemodelseries";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesid", DbParamTypeMapper.GetInstance[SqlDbType.Int], seriesId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelidlist", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 500, modelIdList));

                    MySqlDatabase.UpdateQuery(cmd);
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
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "manageseriessynopsis";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesid", DbParamTypeMapper.GetInstance[SqlDbType.Int], seriesId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_discription", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], synopsis.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], CurrentUser.Id));

                    MySqlDatabase.UpdateQuery(cmd);
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

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandText = "getseriessynopsis";
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesid", DbParamTypeMapper.GetInstance[SqlDbType.Int], seriesId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_series", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_synopsis", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 8000, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_makename", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 8000, ParameterDirection.Output));

                        MySqlDatabase.ExecuteNonQuery(cmd);

                        series = cmd.Parameters["par_series"].Value.ToString();
                        synopsis = cmd.Parameters["par_synopsis"].Value.ToString();
                        makeName = cmd.Parameters["par_makename"].Value.ToString();

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

        }   // End of GetSeriesSynopsis
    }//End of class ManageBikeSeries
}//End of Namespace