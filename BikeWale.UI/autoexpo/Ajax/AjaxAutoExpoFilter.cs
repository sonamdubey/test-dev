using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ajax;
using AjaxPro;
using Carwale.CMS.DAL.AutoExpo;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;


/// <summary>
/// Gets the list of models for selected make.
/// </summary>
/// 
namespace AutoExpo
{
    public class AjaxAutoExpoFilter
    {
        [AjaxPro.AjaxMethod]
        public static string GetFilterData(int makeId)
        {
            PopulateFilters filterData = new PopulateFilters();
            DataSet ds = filterData.GetModelFilterData(makeId);
            DataTable dt = ds.Tables[0];
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
    }
}