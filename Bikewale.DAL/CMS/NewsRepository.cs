using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Bikewale.CoreDAL;
using Bikewale.Notifications;

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
            Database db = null;
            recordCount = 0;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetDefaultNewsPageDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@startindex", SqlDbType.Int).Value = startIndex;
                    cmd.Parameters.Add("@endindex", SqlDbType.Int).Value = endIndex;
                    cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = EnumCMSContentType.News;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            try
            {
                db = new Database();

                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetNewsPageDetails";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@BasicId", SqlDbType.Int).Value = contentId;
                        cmd.Parameters.Add("@IsPublished", SqlDbType.Bit).Value = true;

                        cmd.Parameters.Add("@AuthorName", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@DisplayDate", SqlDbType.DateTime).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Title", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Url", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Views", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Content", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@HostUrl", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ImagePathLarge", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MainImgCaption", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MainImgSet", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@CommentCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@ImagePathThumbnail", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@NextId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@NextUrl", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@NextTitle", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PrevId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PrevUrl", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PrevTitle", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;

                        LogLiveSps.LogSpInGrayLog(cmd);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        v = new V();

                        if (!string.IsNullOrEmpty(cmd.Parameters["@AuthorName"].Value.ToString()))
                        {
                            v.AuthorName = cmd.Parameters["@AuthorName"].Value.ToString();
                            v.DisplayDate = cmd.Parameters["@DisplayDate"].Value.ToString();
                            v.Title = cmd.Parameters["@Title"].Value.ToString();
                            v.Url = cmd.Parameters["@Url"].Value.ToString();
                            v.Views = Convert.ToUInt32(cmd.Parameters["@Views"].Value);
                            v.Data = cmd.Parameters["@Content"].Value.ToString();
                            v.HostUrl = cmd.Parameters["@HostUrl"].Value.ToString();
                            v.IsMainImageSet = Convert.ToBoolean(cmd.Parameters["@MainImgSet"].Value);
                            v.LargePicUrl = cmd.Parameters["@ImagePathLarge"].Value.ToString();
                            v.SmallPicUrl = cmd.Parameters["@ImagePathThumbnail"].Value.ToString();
                            v.MainImgCaption = cmd.Parameters["@MainImgCaption"].Value.ToString();
                            v.FacebookCommentCount = Convert.ToUInt32(cmd.Parameters["@CommentCount"].Value);

                            // Next, Previous new articles
                            v.NextId = cmd.Parameters["@NextId"].Value.ToString();
                            v.PrevId = cmd.Parameters["@PrevId"].Value.ToString();
                            v.NextUrl = cmd.Parameters["@NextUrl"].Value.ToString();
                            v.PrevUrl = cmd.Parameters["@PrevUrl"].Value.ToString();
                            v.NextTitle = cmd.Parameters["@NextTitle"].Value.ToString();
                            v.PrevTitle = cmd.Parameters["@PrevTitle"].Value.ToString();
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return v;
        }   // end of GetContentDetails
        
    }   // class
}   // namespace
