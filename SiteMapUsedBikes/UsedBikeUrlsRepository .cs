using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.CoreDAL;
using System.Configuration;
using Consumer;

namespace SiteMapUsedBikes
{
    /// <summary>
    /// Created By: Aditi Srivastava on 22 Sep 2016
    /// Description: Get used bike urls for sitemap
    /// </summary>
    public class UsedBikeUrlsRepository 
    {

        public void GetUsedBikeUrls(List<String> bike1,List<String> bike2)
        {
            StringBuilder Url=null; 
            bike1.Add("bikes-in-india/");
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikeurls"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                 
                     using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                Url = new StringBuilder();
                                Url.Append(String.Format("bikes-in-{0}/",dr["usedcity"].ToString()));
                                bike1.Add(Url.ToString());
                            }

                            dr.NextResult();
                            while (dr.Read())
                            {
                                Url = new StringBuilder();
                                Url.Append(String.Format("{0}-bikes-in-india/", dr["usedmakeindia"].ToString()));
                                bike1.Add(Url.ToString());
                            }

                            dr.NextResult();
                            while (dr.Read())
                            {
                                Url = new StringBuilder();
                                Url.Append(String.Format("{0}-bikes-in-{1}/",dr["makemaskingname"].ToString(), dr["citymaskingname"].ToString()));
                                bike1.Add(Url.ToString());
                            }

                            dr.NextResult();
                            while (dr.Read())
                            {
                                Url = new StringBuilder();
                                Url.Append(String.Format("{0}-{1}-bikes-in-india/",dr["makemaskingname"].ToString(),dr["modelmaskingname"].ToString()));
                                bike2.Add(Url.ToString());
                            }

                            dr.NextResult();
                            while (dr.Read())
                            {
                                Url = new StringBuilder();
                                Url.Append(String.Format("{0}-{1}-bikes-in-{2}/", dr["makemaskingname"].ToString(),dr["modelmaskingname"].ToString(),dr["citymaskingname"].ToString()));
                                bike2.Add(Url.ToString());
                            }


                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("GetUsedBikeUrls: " + ex);
            }
        }
    }
}
