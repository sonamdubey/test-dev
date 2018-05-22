using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bikewale.Sitemap.MainSitemap
{
    /// <summary>
    /// Created by  :   Sumit Kate on 31 Oct 2017
    /// Description :   Site Map Repository
    /// </summary>
    internal class SiteMapRepository
    {
        private readonly string _sitemapSP = "";
        public SiteMapRepository()
        {

        }

        public SiteMapRepository(string sitemapSP)
        {
            _sitemapSP = sitemapSP;
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 31 Oct 2017
        /// Description :   Returns the data required to form Urls
        /// </summary>
        /// <returns></returns>
        public IDictionary<UrlType, IDictionary<int, ICollection<KeyValuePair<int, string>>>> GetData()
        {
            IDictionary<UrlType, IDictionary<int, ICollection<KeyValuePair<int, string>>>> SitemapList = new Dictionary<UrlType, IDictionary<int, ICollection<KeyValuePair<int, string>>>>();
            try
            {
                if (!String.IsNullOrEmpty(_sitemapSP))
                {
                    UrlType[] urls = { UrlType.Make, UrlType.Model, UrlType.ModelImage, UrlType.ScooterMake,
                                 UrlType.ModelComparison,UrlType.MakeExpertReviews,
                                 UrlType.ModelExpertReviews,UrlType.MakeNews, UrlType.ModelNews,UrlType.MakeUpcoming
                                , UrlType.MakeNewLaunches,UrlType.MakeUserReviews,UrlType.ModelUserReviews,UrlType.MakeVideos,UrlType.ModelVideos
                                    ,UrlType.SeriesPage, UrlType.SeriesNews,UrlType.SeriesExpertReview,UrlType.SeriesVideos };
                    GetData(urls, SitemapList);
                }
                else
                {
                    Logs.WriteErrorLog("Sitemap sp is not provided");
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("SiteMapRepository.GetData(): ", ex);

            }
            return SitemapList;
        }

        private void GetData(UrlType[] urls, IDictionary<UrlType, IDictionary<int, ICollection<KeyValuePair<int, string>>>> sitemapList)
        {
            int index = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(_sitemapSP))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        while (dr != null)
                        {
                            IDictionary<int, ICollection<KeyValuePair<int, string>>> cv = ParseReader(dr);
                            sitemapList.Add(urls[index++], cv);
                            if (dr.NextResult())
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("SiteMapRepository.GetData(): ", ex);
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Oct 2017
        /// Description :   Call Specific query accordingly
        /// </summary>
        /// <param name="u"></param>
        /// <param name="strQuery"></param>
        /// <param name="SitemapList"></param>
        private void GetData(UrlType u,
            string strQuery, IDictionary<UrlType, IDictionary<int, ICollection<KeyValuePair<int, string>>>> SitemapList)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(strQuery))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            IDictionary<int, ICollection<KeyValuePair<int, string>>> cv = ParseReader(dr);
                            SitemapList.Add(u, cv);
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("SiteMapRepository.GetData(): ", ex);
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Oct 2017
        /// Description :   Parses Data Reader and populates Dictionary
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static IDictionary<int, ICollection<KeyValuePair<int, string>>> ParseReader(IDataReader dr)
        {
            IDictionary<int, ICollection<KeyValuePair<int, string>>> retVal = null;
            try
            {
                retVal = new Dictionary<int, ICollection<KeyValuePair<int, string>>>();
                int colCount = dr.FieldCount;
                int index = 0;
                while (dr.Read())
                {
                    ICollection<KeyValuePair<int, string>> cv = new List<KeyValuePair<int, string>>();
                    for (int i = 0; i < colCount - 1; i++)
                    {
                        cv.Add(new KeyValuePair<int, string>(i, dr[i + 1].ToString()));
                    }
                    retVal.Add(index++, cv);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("SiteMapRepository.ParseReader(): ", ex);
            }

            return retVal;
        }
    }
}
