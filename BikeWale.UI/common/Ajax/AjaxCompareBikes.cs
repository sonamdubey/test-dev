using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Common;
using AjaxPro;

namespace Bikewale.Ajax
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 26/7/2012
    /// Summary description for AjaxCompareBikes
    /// </summary>
    public class AjaxCompareBikes
    {
        /// <summary>
        ///     function will return model name and id on the basis of makeId
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="compareBikes">Prameter value new or all should be passed.</param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetModels(string makeId, string compareBikes)
        {
            DataSet ds = null;
            Database db = null;
            string jsonModels = String.Empty;

            if (String.IsNullOrEmpty(makeId))
                return jsonModels;

            string sql = " SELECT DISTINCT CAST(Mo.ID AS VARCHAR(10)) + '_' + Mo.MaskingName AS Value, Mo.Name As Text "
                        + " FROM BikeModels Mo, BikeVersions Ve, NewBikeSpecifications N With(NoLock) "
                        + " WHERE Mo.IsDeleted = 0 "
                        + " AND Ve.IsDeleted = 0 "
                        + " AND Mo.ID = Ve.BikeModelId "
                        + " AND Ve.ID = N.BikeVersionid "
                        + " AND Mo.BikeMakeId = " + makeId;

            if (!(String.IsNullOrEmpty(compareBikes)))
            { 
                if(compareBikes == "new")
                {
                    sql += " AND Ve.New=1 AND MO.Futuristic = 0 ";
                }
            }
            try
            {
                db = new Database();
                ds = db.SelectAdaptQry(sql);

                if (ds != null)
                {
                    jsonModels = JSON.GetJSONString(ds.Tables[0]);
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "AjaxCompareBikes.GetModels");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "AjaxCompareBikes.GetModels");
                objErr.SendMail();
            }
            finally 
            {
                db.CloseConnection();
            }
            return jsonModels;
        }

        /// <summary>
        ///     Function will return version make and id on the basis of make id and model id
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="compareBikes"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetVersions(string modelId, string compareBikes)
        {
            DataSet ds = null;
            Database db = null;
            string jsonVersions = string.Empty;

            if (String.IsNullOrEmpty(modelId))
                return jsonVersions;

            string sql = " SELECT DISTINCT Ve.ID AS Value, Ve.Name As Text "
                         + " FROM BikeModels Mo, BikeVersions Ve, NewBikeSpecifications N, BikeMakes Ma With(NoLock) "
                         + " WHERE Mo.IsDeleted = 0 "
                         + " AND Ve.IsDeleted = 0 "
                         + " AND Mo.ID = Ve.BikeModelId "
                         + " AND Ve.ID = N.BikeVersionid "
                         + " AND Ve.BikeModelId = " + modelId;

            if (!(String.IsNullOrEmpty(compareBikes)))
            {
                if (compareBikes == "new")
                {
                    sql += " AND Ve.New=1 ";
                }
            }
            try
            {
                db = new Database();
                ds = db.SelectAdaptQry(sql);

                if (ds != null)
                {
                    jsonVersions = JSON.GetJSONString(ds.Tables[0]);
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "AjaxCompareBikes.GetModels");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "AjaxCompareBikes.GetModels");
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return jsonVersions;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 18 Sept 2014
        /// Summary : to bind bike models 
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="compareBikes"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetNewBikeModels(string makeId, string compareBikes,string type)
        {
            DataTable dt = null;
            Database db = null;
            string retVal = String.Empty;

            if (String.IsNullOrEmpty(makeId))
                return retVal;

            string sql = " SELECT DISTINCT CAST(Mo.ID AS VARCHAR(10)) + '_' + Mo.MaskingName AS Value, Mo.Name As Text "
                        + " FROM BikeModels Mo, BikeVersions Ve, NewBikeSpecifications N With(NoLock) "
                        + " WHERE Mo.IsDeleted = 0 "
                        + " AND Ve.IsDeleted = 0 "
                        + " AND Mo.ID = Ve.BikeModelId "
                        + " AND Ve.ID = N.BikeVersionid "
                        + " AND Mo.BikeMakeId = " + makeId;

            if (!(String.IsNullOrEmpty(compareBikes)))
            {
                if (compareBikes == "new")
                {
                    sql += " AND Ve.New=1 AND MO.Futuristic = 0 ";
                }
            }
            try
            {
                db = new Database();
                dt = db.SelectAdaptQry(sql).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    retVal += "<li><a onclick='ShowVersion(this);' id = '" + dt.Rows[i]["Value"].ToString() + "' type = '" + type + "' >" + dt.Rows[i]["Text"].ToString() + "</a></li>";
                }
            }
            catch (Exception err)
            {
                retVal = "";
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return retVal;
        }
    }
}