using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace BikeWale.Sitemap
{
    /// <summary>
    /// Created By: Aditi Srivastava on 22 Sep 2016
    /// Description: Get used bike urls for sitemap
    /// </summary>
    public class UsedBikeUrlsRepository
    {
        /// <summary>
        /// Summary: get masking names for makes, models and cities to construct all used bike urls
        /// </summary>
        /// <param name="bike1"></param>
        /// <param name="bike2"></param>
        public IEnumerable<UsedBikeEntity> GetUsedBikeUrls()
        {

            IList<UsedBikeEntity> SitemapList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikesitemap"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            SitemapList = new List<UsedBikeEntity>();

                            while (dr.Read())
                            {
                                SitemapList.Add(new UsedBikeEntity
                                {
                                    MakeName = dr["makemaskingname"].ToString(),
                                    ModelName = dr["modelmaskingname"].ToString(),
                                    CityName = dr["citymaskingname"].ToString(),
                                });

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
            return SitemapList;
        }
    }
}
