using Bikewale.Entities.CMS;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

namespace Bikewale.DAL.CMS
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 13 May 2014
    /// Summary : Class have functions to get the news.
    /// </summary>
    /// <typeparam name="T">CMSContentListEntity</typeparam>
    /// <typeparam name="V">CMSContentDetailsEntity</typeparam>
    public class NewsRepository<T,V> : CMSMainRepository<T,V> where T : CMSContentListEntity, new()
                                                              where V : CMSPageDetailsEntity, new()
    {
        /// <summary>
        /// Function to get list of the news articles
        /// </summary>
        /// <param name="startIndex">Mandatory</param>
        /// <param name="endIndex">Mandatory</param>
        /// <param name="recordCount">Total news count</param>
        /// <param name="filters">Filter to be applied</param>
        /// <returns></returns>
        public override IList<T> GetContentList(int startIndex, int endIndex, out int recordCount, ContentFilter filters)
        {
            IList<T> objList = default(List<T>);
            recordCount = 0;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getdefaultnewspagedetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_startindex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_endindex", DbType.Int32, endIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_categoryid", DbType.Int64, EnumCMSContentType.News));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new List<T>();

                            while (dr.Read())
                            {
                                objList.Add(new T()
                                {
                                    ContentId = Convert.ToInt32(dr["BasicId"]),
                                    AuthorName = Convert.ToString(dr["AuthorName"]),
                                    Description = Convert.ToString(dr["Description"]),
                                    DisplayDate = Convert.ToString(dr["DisplayDate"]),
                                    FacebookCommentCount = Convert.ToUInt32(dr["FacebookCommentCount"]),
                                    HostUrl = Convert.ToString(dr["HostUrl"]),
                                    IsSticky = Convert.ToBoolean(dr["IsSticky"]),
                                    LargePicUrl = Convert.ToString(dr["ImagePathLarge"]),
                                    RowNumber = Convert.ToInt32(dr["Row_No"]),
                                    SmallPicUrl = Convert.ToString(dr["ImagePathThumbnail"]),
                                    Title = Convert.ToString(dr["Title"]),
                                    Views = Convert.ToUInt32(dr["VIEWS"]),
                                    ContentUrl = Convert.ToString(dr["Url"])
                                });
                            }

                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    recordCount = Convert.ToInt32(dr["RecordCount"]);
                                }
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return objList;
        }   // End of GetContentList

        /// <summary>
        /// Function to get the content(article) details.
        /// </summary>
        /// <param name="contentId">Id of the content</param>
        /// <returns>Returns object containing the details.</returns>
        public override V GetContentDetails(int contentId, int pageId)
        {
            V v = default(V);

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "getnewspagedetails";

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_basicid", DbType.Int32, contentId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_ispublished", DbType.Boolean, true));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_authorname", DbType.String, 100, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_displaydate", DbType.DateTime, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_title", DbType.String, 250, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_url", DbType.String, 200, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_views", DbType.Int32, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_content", DbType.String, 8000, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbType.String, 250, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_imagepathlarge", DbType.String, 100, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_mainimgcaption", DbType.String, 250, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_mainimgset", DbType.Boolean, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_commentcount", DbType.Int32, ParameterDirection.Output));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_imagepaththumbnail", DbType.String, 100, ParameterDirection.Output));

                        
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_nextid", DbType.Int64, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_nexturl", DbType.String, 200, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_nexttitle", DbType.String, 250, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_previd", DbType.Int64, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_prevurl", DbType.String, 200, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_prevtitle", DbType.String, 250, ParameterDirection.Output));
// LogLiveSps.LogSpInGrayLog(cmd);
                        MySqlDatabase.ExecuteNonQuery(cmd,ConnectionType.ReadOnly);
                        v = new V();

                        if (!string.IsNullOrEmpty(cmd.Parameters["par_authorname"].Value.ToString()))
                        {
                            v.AuthorName = cmd.Parameters["par_authorname"].Value.ToString();
                            v.DisplayDate = cmd.Parameters["par_displaydate"].Value.ToString();
                            v.Title = cmd.Parameters["par_title"].Value.ToString();
                            v.Url = cmd.Parameters["par_url"].Value.ToString();
                            v.Views = Convert.ToUInt32(cmd.Parameters["par_views"].Value);
                            v.Data = cmd.Parameters["par_content"].Value.ToString();
                            v.HostUrl = cmd.Parameters["par_hosturl"].Value.ToString();
                            v.IsMainImageSet = Convert.ToBoolean(cmd.Parameters["par_mainimgset"].Value);
                            v.LargePicUrl = cmd.Parameters["par_imagepathlarge"].Value.ToString();
                            v.SmallPicUrl = cmd.Parameters["par_imagepaththumbnail"].Value.ToString();
                            v.MainImgCaption = cmd.Parameters["par_mainimgcaption"].Value.ToString();
                            v.FacebookCommentCount = Convert.ToUInt32(cmd.Parameters["par_commentcount"].Value);

                            // Next, Previous new articles
                            v.NextId = cmd.Parameters["par_nextid"].Value.ToString();
                            v.PrevId = cmd.Parameters["par_previd"].Value.ToString();
                            v.NextUrl = cmd.Parameters["par_nexturl"].Value.ToString();
                            v.PrevUrl = cmd.Parameters["par_prevurl"].Value.ToString();
                            v.NextTitle = cmd.Parameters["par_nexttitle"].Value.ToString();
                            v.PrevTitle = cmd.Parameters["par_prevtitle"].Value.ToString();
                        }
                    }
            }
            catch (SqlException err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return v;
        }   // end of GetContentDetails
        
    }   // class
}   // namespace
