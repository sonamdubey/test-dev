using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bikewale.Sitemap.ServiceCenter
{
    class ServiceCenterUrlsRepository
    {

        public IEnumerable<ServiceCenterEnitity> GetServiceCenterUrls()
        {

            IList<ServiceCenterEnitity> SitemapList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getservicecentersitemap"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            SitemapList = new List<ServiceCenterEnitity>();

                            while (dr.Read())
                            {
                                SitemapList.Add(new ServiceCenterEnitity
                                {
                                    MakeMaskingName = dr["MaskingName"].ToString(),
                                    CityMaskingName = dr["citymaskingname"].ToString()
                                });

                            }
                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    SitemapList.Add(new ServiceCenterEnitity
                                    {
                                        MakeMaskingName = dr["MaskingName"].ToString(),
                                        CityMaskingName = dr["citymaskingname"].ToString()
                                    });
                                }
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
