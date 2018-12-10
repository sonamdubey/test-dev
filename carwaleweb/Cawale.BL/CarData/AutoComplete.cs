using Carwale.Cache.CarData;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Cawale.BL
{
    public class AutoComplete
    {
#if Ashwini
        public static DataTable GetResults(string source, string textValue, string Params)
        {
            DataView dv = new DataView();
            DataTable dt = new DataTable();
            int rows;
            string key;
            if (textValue.Length >= 1)
            {
                if (textValue.Length > 2)
                {
                    string firstTwoLetters = textValue.Substring(0, 2).ToLower();
                    key = "ac_" + source.ToLower() + "_" + firstTwoLetters + "_" + Params;
                }
                else
                {
                    string textvalue = textValue.ToLower();
                    key = "ac_" + source.ToLower() + "_" + textvalue + "_" + Params;
                }
                DataTable dt1 = AutoCompleteCache.GetResults(key);

                if (dt1.Rows.Count > 0)
                {
                    try
                    {
                        EnumerableRowCollection<DataRow> results;
                        results = from myRow in dt1.AsEnumerable()
                                  where myRow["n"].ToString().StartsWith(EscapeLikeValue(textValue.ToLower()))
                                  select myRow;
                        if (results.Count() > 0)
                            dt = results.Take(10).CopyToDataTable().DefaultView.ToTable(false, "l", "v");
                    }
                    catch (Exception ex)
                    {
                        var objErr = new ExceptionHandler(ex, string.Empty);
                        objErr.LogException();
                    }
                }
            }
            return dt;
        }

#endif
        internal static DataTable SelectTopDataRow(DataTable dt, int count)
        {
            DataTable dtn = dt.Clone();
            for (int i = 0; i < count; i++)
            {
                dtn.ImportRow(dt.Rows[i]);
            }
            return dtn;
        }
        
        internal static string EscapeLikeValue(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}