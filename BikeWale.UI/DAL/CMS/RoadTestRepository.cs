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
    /// Summary : Class have methods to get the road test information.
    /// </summary>
    /// <typeparam name="T">CMSContentListEntity</typeparam>
    /// <typeparam name="V">CMSContentDetailsEntity</typeparam>
    public class RoadTestRepository<T, V> : CMSMainRepository<T, V> where T : CMSContentListEntity, new()
                                                                    where V : CMSPageDetailsEntity, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public override IList<T> GetContentList(int startIndex, int endIndex, out int recordCount, ContentFilter filters)
        {
            IList<T> objList = default(List<T>);
            recordCount = 0;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getroadtestlist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_startindex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_endindex", DbType.Int32, endIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_categoryid", DbType.Byte, EnumCMSContentType.RoadTest)); 
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, (filters.MakeId > 0) ? filters.MakeId : Convert.DBNull));  
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, (filters.ModelId > 0) ? filters.ModelId : Convert.DBNull));


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
                                    //FacebookCommentCount = Convert.ToUInt32(dr["FacebookCommentCount"]),
                                    HostUrl = Convert.ToString(dr["HostURL"]),
                                    //IsSticky = Convert.ToBoolean(dr["IsSticky"]),
                                    LargePicUrl = Convert.ToString(dr["ImagePathLarge"]),
                                    RowNumber = Convert.ToInt32(dr["Row_No"]),
                                    SmallPicUrl = Convert.ToString(dr["ImagePathThumbnail"]),
                                    Title = Convert.ToString(dr["Title"]),
                                    Views = Convert.ToUInt32(dr["Views"]),
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

    }   // Class
}   // namespace
