using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bikewale.Sitemap.PriceInCity
{
    class PriceInCityUrlsRepository
    {

        public IEnumerable<PriceInCityEnitity> GetPriceInCityUrls()
        {

            IList<PriceInCityEnitity> SitemapList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getpriceincitysitemap"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            SitemapList = new List<PriceInCityEnitity>();

                            while (dr.Read())
                            {
                                SitemapList.Add(new PriceInCityEnitity
                                {
                                    MakeMaskingName = dr["MakeMaskingName"].ToString(),
                                    ModelMaskingName = dr["MaskingName"].ToString(),
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
                Logs.WriteErrorLog("GetUsedBikeUrls: " + ex);
            }
            return SitemapList;
        }
    }
}
