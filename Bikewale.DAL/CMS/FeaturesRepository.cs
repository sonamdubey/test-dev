using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Threading.Tasks;
using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Bikewale.Notifications;
using Bikewale.CoreDAL;

namespace Bikewale.DAL.CMS
{
    public class FeaturesRepository<T, V> : CMSMainRepository<T, V> where T : CMSContentListEntity, new()
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
            //Database db = null;
            recordCount = 0;

            //try
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.CommandText = "GetFeaturesList";
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.Add("@startindex", SqlDbType.Int).Value = startIndex;
            //        cmd.Parameters.Add("@endindex", SqlDbType.Int).Value = endIndex;
            //        cmd.Parameters.Add("@CategoryId", SqlDbType.TinyInt).Value = EnumCMSContentType.Features;

            //        db = new Database();

            //        using (SqlDataReader dr = db.SelectQry(cmd))
            //        {
            //            if (dr != null)
            //            {
            //                objList = new List<T>();

            //                while (dr.Read())
            //                {
            //                    objList.Add(new T()
            //                    {
            //                        ContentId = Convert.ToInt32(dr["BasicId"]),
            //                        AuthorName = Convert.ToString(dr["AuthorName"]),
            //                        Description = Convert.ToString(dr["Description"]),
            //                        DisplayDate = Convert.ToString(dr["DisplayDate"]),
            //                        HostUrl = Convert.ToString(dr["HostURL"]),
            //                        LargePicUrl = Convert.ToString(dr["ImagePathLarge"]),
            //                        RowNumber = Convert.ToInt32(dr["Row_No"]),
            //                        SmallPicUrl = Convert.ToString(dr["ImagePathThumbnail"]),
            //                        Title = Convert.ToString(dr["Title"]),
            //                        Views = Convert.ToUInt32(dr["Views"]),
            //                        ContentUrl = Convert.ToString(dr["Url"])
            //                    });
            //                }

            //                if (dr.NextResult())
            //                {
            //                    if (dr.Read())
            //                    {
            //                        recordCount = Convert.ToInt32(dr["RecordCount"]);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (SqlException err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    db.CloseConnection();
            //}

            return objList;
        }   // End of GetContentList

    }   // class
}   // namespace
