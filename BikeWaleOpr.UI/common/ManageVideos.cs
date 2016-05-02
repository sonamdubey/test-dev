using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using Ajax;
using AjaxPro;
using System.Net;
using System.IO;
using System.Data.Common;
using Bikewale.CoreDAL;

namespace BikeWaleOpr.Common
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Jan 2014
    /// Summary    : This class manages video operations  
    /// </summary>

    public class ManageVideos
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 17 Jan 2014
        /// Summary    : function validates youtube url and returns url id 
        /// </summary>
        /// <param name="srcUrl"></param>
        /// <returns></returns>
        public static string GetYouTubeVideoSrc(string srcUrl)
        {
            string videoSrc = string.Empty;

            if (srcUrl.Length > 0)
            {
                string src = string.Empty;
                string pattern = @"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)";
                string replacement = "$1";

                Regex rgx = new Regex(pattern);
                src = rgx.Replace(srcUrl.Trim(), replacement);

                if (src.Length == 11 && src.IndexOf(".com") == -1)
                {
                    videoSrc = src;
                }
            }

            return videoSrc;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Jan 2014
        /// Summary    : Retrieves all videos of input basic id
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public DataSet GetYouTubeVideos(string basicId)
        {
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getvideosbybasicid"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "getvideosbybasicid";
                    //cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = basicId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_basicid", DbParamTypeMapper.GetInstance[SqlDbType.Int], basicId)); 
                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("sql ex : ", ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, "GetYouTubeVideos");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "GetYouTubeVideos");
                objErr.SendMail();
            }
            return ds;
        }//End of GetYouTubeVideos method

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Jan 2014
        /// Summary    : Function to delete video of input basic id
        /// </summary>
        /// <param name="basicId"></param>
        public void DeleteYouTubeVideo(string basicId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("deletevideobybasicid"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "deletevideobybasicid";
                    //cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = basicId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_basicid", DbParamTypeMapper.GetInstance[SqlDbType.Int], basicId));
                    MySqlDatabase.UpdateQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("sql ex : ", ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, "DeleteYouTubeVideo");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "DeleteYouTubeVideo");
                objErr.SendMail();
            }
        }//End of DeleteYouTubeVideo method
        
        /// <summary>
        /// Written By : Ashwini Todkar on 17 Jan 2014
        /// Summary    : Function retrieves video details like url,views count ,like count and duration 
        /// </summary>
        /// <param name="basicId"></param>
        /// <param name="views"></param>
        /// <param name="likes"></param>
        /// <param name="videoUrl"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public string UpdateYoutubeVideoDetails(string basicId, int views, int likes, string videoUrl, string videoId, double duration)
        {            
            int returnValue = 0;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("modifyvideos"))
                    {
                        //cmd.CommandText = "modifyvideos";
                        cmd.CommandType = CommandType.StoredProcedure;

                        //cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = basicId;
                        //cmd.Parameters.Add("@Views", SqlDbType.Int).Value = views;
                        //cmd.Parameters.Add("@Likes", SqlDbType.Int).Value = likes;
                        //cmd.Parameters.Add("@VideoUrl", SqlDbType.VarChar, 150).Value = videoUrl;
                        //cmd.Parameters.Add("@VideoId", SqlDbType.VarChar, 20).Value = videoId;
                        //cmd.Parameters.Add("@Duration", SqlDbType.BigInt).Value = duration;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_basicid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], basicId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_views", DbParamTypeMapper.GetInstance[SqlDbType.Int], views));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_likes", DbParamTypeMapper.GetInstance[SqlDbType.Int], likes));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_videourl", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 150, videoUrl));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_videoid", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 20, videoId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_duration", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], duration));


                        returnValue = MySqlDatabase.ExecuteNonQuery(cmd);
                    }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("sql ex : ", ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, "UpdateYoutubeVideoDetails");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "UpdateYoutubeVideoDetails");
                objErr.SendMail();
            }
            return returnValue.ToString();
        }   // UpdateYoutubeVideoDetails method

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Jan 2014
        /// Summary    : Function retrieves video details like url,views count ,like count and duration 
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="views"></param>
        /// <param name="likes"></param>
        /// <param name="duration"></param>
        public void GetVideoDetailsFromYouTube(string videoId, out int views, out int likes, out double duration)
        {            
            views = 0;
            likes = 0;
            duration = 0;
            try
            {
                string responseStr = string.Empty;
                string url = "http://gdata.youtube.com/feeds/api/videos/" + videoId + "?v=2&alt=json";
                
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);

                httpRequest.Method = "Get";
                httpRequest.ContentType = "text/json";

                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
              
                //StreamReader stream = new StreamReader(response.GetResponseStream());
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseStr = streamReader.ReadToEnd();
                }

                HttpContext.Current.Trace.Warn(responseStr);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("LoadSearchResults : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }


    }//class
}   // namespace