using Bikewale.Common;
using Bikewale.Interfaces.Compare;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
/***********************************************************/
// Desc: Common class for research compare section
// Written By: Satish Sharma On 2009-09-29 5:35 PM	
/***********************************************************/
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.HtmlControls;
//using BikeWale.Controls;

namespace Bikewale.New
{
    /// <summary>
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : REmoved unused methods  and modified getfeatured bike
    /// </summary>
    public class CompareBikes
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;
        protected ArrayList arObj = null;


        // This function will get current versionId from dataset and will match it with the version in the array. 
        // if it matches, it will return index.this is just to match the order of Bikes being compared!
        public int GetVersionIndex(string versionNo, int arrSize, string[] versionId)
        {
            int index = -1;

            for (int i = 0; i < arrSize; i++)
            {
                //Trace.Warn( "Version Passed - Version Array - Index : " + versionNo + "-" + versionId[i] + "-" + i  );

                if (versionNo == versionId[i])
                {
                    //Trace.Warn( "Version Passed - Version Array - Index : " + versionNo + "-" + versionId[i] + "-" + i  );
                    index = i;
                    break;
                }
            }

            return index;
        }

        public void HideComparison(int column, int arrSize, HtmlTable table)
        {

            while (arrSize - column > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    table.Rows[i].Cells.RemoveAt(table.Rows[i].Cells.Count - 1);
                }

                column++; // One column has been deleted, increase the index by 1.
            }
        }


        /// <summary>
        /// Modified By : Sushil Kumar on 2nd Dec 2016
        /// Description : To get sponsored bike from BAL layer
        /// </summary>
        /// <param name="versions"></param>
        /// <returns></returns>
        public static Int64 GetFeaturedBike(string versions)
        {
            Int64 featuredBikeId = -1;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeCompare, Bikewale.BAL.Compare.BikeComparison>();

                    var _objCompare = container.Resolve<IBikeCompare>();

                    featuredBikeId = _objCompare.GetFeaturedBike(versions);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("{0}_GetFeaturedBike_{1}", HttpContext.Current.Request.ServerVariables["URL"], versions));
                objErr.SendMail();
            }

            return featuredBikeId;

        }



        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 Sept 2014
        /// Summary : To get featured Compare bike list
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public DataSet GetComparisonBikeList(UInt16 topCount)
        {
            DataSet ds = null;
            try
            {
                using (System.Data.Common.DbCommand cmd = DbFactory.GetDBCommand("getbikecomparisonmin"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topCount));

                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);
                }
            }
            catch (SqlException exSql)
            {
                ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }   // End of GetComparisonBikeList

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 Sept 2014
        /// Summary : To get compare bike details by version id list
        /// Modified By : Lucky Rathore
        /// Modified On : 15 Feb 2016
        /// Summary : SP name Changed.
        /// Modified By : Lucky Rathore
        /// Modified On : 26 Feb 2016
        /// Summary : SP name Changed.
        /// Modified On : 23 Jan 2017 by Sangram Nandkhile
        /// Summary : New parameter added - cityId
        /// </summary>
        /// <param name="versionList"></param>
        /// <returns></returns>
        public DataSet GetComparisonBikeListByVersion(string versionList, uint cityId)
        {
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getcomparisondetails_20012017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@bikeversions", SqlDbType.VarChar, 50).Value = versionList;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversions", DbType.String, 50, versionList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.UInt16, cityId));

                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }
    }
}