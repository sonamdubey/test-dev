using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bikewale.Sitemap.DealerLocator
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 01-Nov-2017
    /// Summary: Data repo for dealer locator Sitemap
    /// 
    /// </summary>
    public class DealerLocatorSitemapRepository
    {
        /// <summary>
        /// Gets the dealer locator make cities.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DealerLocatorEntity> GetDealerLocatorMakeCities()
        {

            IList<DealerLocatorEntity> SitemapList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerlocatorsitemap"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            SitemapList = new List<DealerLocatorEntity>();

                            while (dr.Read())
                            {
                                SitemapList.Add(new DealerLocatorEntity
                                {
                                    MakeMaskingName = dr["MaskingName"].ToString(),
                                    CityMaskingName = dr["citymaskingname"].ToString()
                                });

                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Bikewale.Sitemap.DealerLocator: GetDealerLocatorMakeCities: " + ex);
            }
            return SitemapList;
        }
    }
}
