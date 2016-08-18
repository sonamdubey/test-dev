using Bikewale.Common;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

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
            string jsonModels = String.Empty;
            uint _makeid; string sql = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(makeId) && uint.TryParse(makeId, out _makeid))
                {
                    sql = @" select distinct concat(ve.bikemodelid,'_',ve.modelmaskingname) as Value, ve.modelname as Text  
                         from bikeversions ve
                         join newbikespecifications n on ve.id = n.bikeversionid
                         where ve.isdeleted = 0                           
                         and ve.bikemakeid = " + _makeid;


                    if (!(String.IsNullOrEmpty(compareBikes)))
                    {
                        if (compareBikes == "New")
                        {
                            sql += " and ve.new=1 and ve.isfuturisticmodel = 0 and ve.isdeleted=0";
       
                        }
                    }

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(sql, ConnectionType.ReadOnly))
                    {

                        if (ds != null)
                        {
                            jsonModels = JSON.GetJSONString(ds.Tables[0]);
                        }
                    }

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

            string jsonVersions = string.Empty;
            uint _modelid; string sql = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(modelId) && uint.TryParse(modelId, out _modelid))
                {
                    sql = @" select distinct ve.id as Value, ve.name as Text
                         from bikeversions ve, newbikespecifications n
                         where ve.isdeleted = 0                         
                         and ve.id = n.bikeversionid
                         and ve.bikemodelid = " + _modelid;

                    if (!(String.IsNullOrEmpty(compareBikes)))
                    {
                        if (compareBikes == "new")
                        {
                            sql += " and ve.new=1 ";
                        }
                    }

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(sql, ConnectionType.ReadOnly))
                    {

                        if (ds != null)
                        {
                            jsonVersions = JSON.GetJSONString(ds.Tables[0]);
                        }
                    }
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
        public string GetNewBikeModels(string makeId, string compareBikes, string type)
        {

            string retVal = string.Empty;
            uint _makeid; string sql = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(makeId) && uint.TryParse(makeId, out _makeid))
                {
                    sql = @" select distinct concat(ve.bikemodelid,'_',ve.modelmaskingname) as Value, ve.modelname as Text  
                         from bikeversions ve, newbikespecifications n 
                         where ve.isdeleted = 0                           
                         and ve.id = n.bikeversionid  
                         and ve.bikemakeid = " + _makeid;


                    if (!(String.IsNullOrEmpty(compareBikes)))
                    {
                        if (compareBikes == "new")
                        {
                            sql += " and ve.new=1 and ve.isfuturisticmodel = 0 ";
                        }
                    }

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(sql, ConnectionType.ReadOnly))
                    {

                        if (ds != null)
                        {
                            DataTable dt = ds.Tables[0];
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                retVal += "<li><a onclick='ShowVersion(this);' id = '" + dt.Rows[i]["Value"].ToString() + "' type = '" + type + "' >" + dt.Rows[i]["Text"].ToString() + "</a></li>";
                            }
                        }
                    }
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