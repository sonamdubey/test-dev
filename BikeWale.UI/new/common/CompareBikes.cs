using Bikewale.Common;
using Grpc.CMS;
using log4net;
using MySql.CoreDAL;
using Newtonsoft.Json;
/***********************************************************/
// Desc: Common class for research compare section
// Written By: Satish Sharma On 2009-09-29 5:35 PM	
/***********************************************************/
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI.HtmlControls;
//using BikeWale.Controls;

namespace Bikewale.New
{
    public class CompareBikes
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;
        protected ArrayList arObj = null;

        static bool _useGrpc = Convert.ToBoolean(ConfigurationManager.AppSettings["UseGrpc"]);
        static bool _logGrpcErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["LogGrpcErrors"]);
        static readonly ILog _logger = LogManager.GetLogger(typeof(CompareBikes));


        //DataSet dsRating;

        public CompareBikes() { }

        public CompareBikes(string versions)
        {
            GetAllModelRatings(versions);
        }

        /***********************************************************/
        // Input: Version id of Bikes selected by the user to compare
        // Output: Version id of featured Bike
        // Written By: Satish Sharma On 2009-09-29 5:40 PM
        // Modified By : Sadhana Upadhyay on 9 Sept 2014
        // Summary : to get sponsored bike by web api
        /***********************************************************/
        public static string GetFeaturedBike(string versions)
        {
            try
            {
                if (_useGrpc)
                {
                    var grpcInt = GrpcMethods.GrpcGetFeaturedCar(versions, 1, 2);
                    return grpcInt.IntOutput.ToString();
                }
                else
                {
                    return GetFeaturedBikeOldWay(versions);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                return GetFeaturedBikeOldWay(versions);
            }
        }

        private static string GetFeaturedBikeOldWay(string versions)
        {
            string featuredBikeId = "";

            try
            {
                if (_logGrpcErrors)
                {
                    _logger.Error(string.Format("Grpc did not work for GetFeaturedBikeOldWay {0}", versions));
                }

                // Get Sponsored bike by Web Api
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["cwApiHostUrl"]);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("/webapi/SponsoredCarVersion/GetSponsoredCarVersion/?vids=" + versions + "&categoryId=1&platformId=2").Result;

                    HttpContext.Current.Trace.Warn("response", response.ToString());

                    if (response.IsSuccessStatusCode)
                    {
                        featuredBikeId = JsonConvert.DeserializeObject<Int64>(response.Content.ReadAsStringAsync().Result).ToString();
                    }
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return featuredBikeId;
        }

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


        void GetAllModelRatings(string versions)
        {


            //string sql = string.Empty;
            //SqlCommand cmd =  new SqlCommand();
            //Database db = new Database();
            //dsRating = new DataSet();

            //sql = " SELECT (SELECT MaskingName FROM BikeMakes With(NoLock) WHERE ID = MO.BikeMakeId) AS MakeMaskingName, MO.ID as ModelId, MO.Name AS ModelName,MO.MaskingName AS ModelMaskingName, CV.ID As BikeVersionId, IsNull(MO.ReviewRate, 0) AS ModelRate, IsNull(MO.ReviewCount, 0) AS ModelTotal, "
            //    + " IsNull(CV.ReviewRate, 0) AS VersionRate, IsNull(CV.ReviewCount, 0) AS VersionTotal "
            //    + " FROM BikeModels AS MO, BikeVersions AS CV With(NoLock) WHERE CV.ID in ( " + db.GetInClauseValue(versions, "BikeVersionId", cmd) + " ) AND MO.ID = CV.BikeModelId ";

            //cmd.CommandText = sql;

            try
            {
                //    dsRating = db.SelectAdaptQry(cmd);
                throw new Exception("GetAllModelRatings(string versions) : Method not used/commented");
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        public string GetModelRatings(string versionId)
        {
            //string reviewString = string.Empty;
            try
            {
                throw new Exception("GetAllModelRatings(string versions) : Method not used/commented");

                //if (dsRating.Tables[0].Rows.Count > 0)
                //{
                //    DataRow[] drRating = dsRating.Tables[0].Select("BikeVersionId=" + versionId);

                //    if (drRating.Length > 0)
                //    {
                //        for (int i = 0; i < drRating.Length; ++i)
                //        {
                //            if (Convert.ToDouble(drRating[i]["ModelRate"]) > 0)
                //            {
                //                string reviews = Convert.ToDouble(drRating[i]["ModelTotal"]) > 1 ? " reviews" : " review";
                //                reviewString += "<div>" + CommonOpn.GetRateImage(Convert.ToDouble(drRating[i]["ModelRate"].ToString())) + "</div>"
                //                             + " <div style='margin-top:5px;'><a href='/research/" + drRating[i]["MakeMaskingName"].ToString() + "-bikes/" + drRating[i]["ModelMaskingName"].ToString() + "/userreviews/'>" + drRating[i]["ModelTotal"].ToString() + reviews + " </a></div>";
                //            }
                //            else
                //                reviewString = "<div style='margin-top:10px;'><a href='/research/userreviews-bikem-" + drRating[i]["ModelId"].ToString() + ".html'>Write a review</a></div>";
                //        }
                //    }
                //}
                //return reviewString;
            }
            catch (Exception err)
            {
                //Trace.Warn("GetModelRatings Error = " + err.Message);
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
                return string.Empty;
            }
        }	//End of GetModelRatings

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
        /// </summary>
        /// <param name="versionList"></param>
        /// <returns></returns>
        public DataSet GetComparisonBikeListByVersion(string versionList)
        {
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getcomparisondetails_26022016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@bikeversions", SqlDbType.VarChar, 50).Value = versionList;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversions", DbType.String, 50, versionList));

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
        }
    }
}