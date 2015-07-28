/*
	This class will use to bind controls like filling makes, states
	Written by: Satish Sharma On Jan 21, 2008 12:28 PM
*/

using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Bikewale.Common
{	
	public class JSON
	{
		public static string GetJSONString(DataTable Dt)
		{		
			string[] StrDc = new string[Dt.Columns.Count];
		
			string HeadStr = string.Empty;
			for (int i = 0; i < Dt.Columns.Count; i++)		
			{
		
				StrDc[i] = Dt.Columns[i].Caption;
				HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "" + "\",";		
			}
		
			HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
			StringBuilder Sb = new StringBuilder();
		
			Sb.Append("{\""+ Dt.TableName +"\" : [");
			bool isRowsAvailable = false;
			
			for (int i = 0; i < Dt.Rows.Count; i++)		
			{		
				string TempStr = HeadStr;
		
				Sb.Append("{");
				for (int j = 0; j < Dt.Columns.Count; j++)		
				{		
					TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "", Dt.Rows[i][j].ToString());	
				}
				Sb.Append(TempStr + "},");
				
				isRowsAvailable = true;
			}
			
			if( isRowsAvailable )
				Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
		
			Sb.Append("]}");
			return Sb.ToString();		
		}
		
		public static string GetJSONFromDT(DataTable dt)
		{
		
			StringBuilder JsonString = new StringBuilder();
			JsonString.Append("{ ");
	
			JsonString.Append("\"TABLE\":[{ ");
				JsonString.Append("\"ROW\":[ ");
	
				for (int i = 0; i < dt.Rows.Count; i++)
	
				{
	
					JsonString.Append("{ ");
	
					JsonString.Append("\"COL\":[ ");
	
					for (int j = 0; j < dt.Columns.Count; j++)
	
					{
	
						if (j < dt.Columns.Count - 1)
	
						{
	
							JsonString.Append("{" + "\"DATA\":\"" + dt.Rows[i][j].ToString() + "\"},");
	
						}
	
						else if (j == dt.Columns.Count - 1)
	
						{
	
							JsonString.Append("{" + "\"DATA\":\"" + dt.Rows[i][j].ToString() + "\"}");
	
						}
	
					}
	
					if (i == dt.Rows.Count - 1)
	
					{
	
						JsonString.Append("]} ");
	
					}
	
					else
	
					{
						JsonString.Append("]}, ");
					}
	
				}
	
				JsonString.Append("]}]}");
	
				return JsonString.ToString();
		}

	}//class
}//namespace
