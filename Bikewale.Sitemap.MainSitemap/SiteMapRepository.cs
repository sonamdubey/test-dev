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
        private readonly string strMakes = @"SELECT 1 AS UrlType, MaskingName AS C1 FROM bikemakes WHERE IsDeleted = 0;";

        private readonly string strModels = @"SELECT 2 AS UrlType,MakeMaskingName AS C1, MaskingName AS C2 FROM bikemodels WHERE IsDeleted = 0;";
        private readonly string strModelImages = @"SELECT 3 AS UrlType,MakeMaskingName C1,MaskingName AS C2 FROM bikemodels WHERE IsDeleted = 0 AND HostUrl IS NOT NULL;";

        private readonly string strScooterMakes = @"SELECT distinct 4 as UrlType, bm.MaskingName AS C1 FROM bikemakes bm INNER JOIN bikeversions bv ON bv.BikeMakeId = bm.Id AND bv.BodyStyleId = 5 AND bv.new = 1 AND bv.futuristic = 0 AND bv.IsDeleted = 0 INNER JOIN bikemodels bmo ON bv.bikemodelid = bmo.id AND bmo.New = 1 AND bmo.IsDeleted = 0 AND bm.isScooterOnly = 0;";

        private readonly string strModelSpecs = @"SELECT distinct 5 AS UrlType, MakeMaskingName C1, ModelMaskingName AS C2 FROM bikeversions bv INNER JOIN newbikespecifications s on bv.Id = s.BikeVersionId AND bv.IsDeleted = 0;";

        private readonly string strMakeExpertReviews = @"SELECT DISTINCT 7 AS UrlType, MakeMaskingName C1 FROM bikemodels bm WHERE bm.Isdeleted = 0 AND IFNULL(bm.ExpertReviewsCount, 0) > 0;";

        private readonly string strModelExpertReviews = @"SELECT DISTINCT 8 AS UrlType, MakeMaskingName C1, MaskingName AS C2 FROM bikemodels bm WHERE bm.Isdeleted = 0 AND IFNULL(bm.ExpertReviewsCount, 0) > 0;";

        private readonly string strMakeNews = @"SELECT DISTINCT 9 AS UrlType, MakeMaskingName C1 FROM bikemodels bm WHERE bm.Isdeleted = 0 AND IFNULL(bm.NewsCount, 0) > 0;";


        private readonly string strModelNews = @"SELECT DISTINCT 10 AS UrlType, MakeMaskingName C1, MaskingName AS C2 FROM bikemodels bm WHERE bm.Isdeleted = 0 AND IFNULL(bm.NewsCount, 0) > 0;";

        private readonly string strMakeUpcoming = @"SELECT DISTINCT 11 AS UrlType, bm.maskingname makename FROM expectedbikelaunches ecl INNER JOIN bikemodels mo ON ecl.bikemodelid = mo.id AND mo.isdeleted = 0 AND mo.futuristic = 1 AND ecl.isdeleted = 0 AND ecl.islaunched = 0 INNER JOIN bikeversions bv ON bv.bikemodelid = mo.id AND bv.isdeleted = 0 INNER JOIN bikemakes bm ON ecl.BikeMakeId = bm.id AND bm.isdeleted = 0 ORDER BY bm.maskingname;";

        private readonly string strMakeNewLaunches = @"SELECT DISTINCT 12 AS UrlType, mo.makemaskingname AS makemaskingname FROM expectedbikelaunches AS bl INNER JOIN bikemodels AS mo ON mo.id = bl.bikemodelid AND bl.islaunched = 1 AND bl.launchdate IS NOT NULL AND YEAR(bl.launchdate) > 2013 AND mo.isdeleted = 0 AND mo.new = 1 AND mo.isnewmake = 1 INNER JOIN bikeversions AS bv ON bv.bikemodelid = mo.id AND bv.new = 1 AND bv.isdeleted = 0 ORDER BY mo.makemaskingname;";

        private readonly string strMakeUserReviews = @"SELECT distinct 13 AS UrlType, bm.maskingname FROM bikemakes AS bm INNER JOIN bikemodels bmo ON bmo.bikemakeid = bm.id AND bmo.reviewcount > 0 AND bmo.isdeleted = 0 AND bm.isdeleted = 0 ORDER BY bm.maskingname;";

        private readonly string strModelUserReviews = @"SELECT distinct 14 AS UrlType, bm.maskingname as MakeMaskingName, bmo.MaskingName AS ModelMaskingName FROM bikemakes AS bm INNER JOIN bikemodels bmo ON bmo.bikemakeid = bm.id AND bmo.reviewcount > 0 AND bmo.isdeleted = 0 AND bm.isdeleted = 0 ORDER BY bm.maskingname;";

        private readonly string strMakeVideos = @"SELECT DISTINCT 15 AS UrlType, bm.MaskingName AS MakeMaskingName FROM bikemodels AS bmo INNER JOIN bikemakes AS bm ON bm.id = bmo.BikeMakeId AND bmo.VideosCount > 0 AND bmo.IsDeleted = 0 AND bm.IsDeleted = 0 ORDER BY bm.MaskingName;";

        private readonly string strModelVideos = @"SELECT DISTINCT 16 AS UrlType, bm.MaskingName AS MakeMaskingName, bmo.MaskingName AS ModelMaskingName FROM bikemodels AS bmo INNER JOIN bikemakes AS bm ON bm.id = bmo.BikeMakeId AND bmo.VideosCount > 0 AND bmo.IsDeleted = 0 AND bm.IsDeleted = 0 ORDER BY bm.MaskingName , bmo.MaskingName;";

        private readonly string strModelCompare = @"SELECT distinct 6 AS UrlType, m1.MakeMaskingName AS makemasking1, m1.MaskingName as modelmasking1, m2.MakeMaskingName AS makemasking2, m2.MaskingName as modelmasking2 FROM (SELECT DISTINCT a.modelid, a.similarModelId, a.compareOrder FROM similarbikemodels AS a LEFT JOIN similarbikemodels AS b ON a.modelid = b.similarModelId AND a.similarModelId = b.modelid WHERE a.modelid < b.modelid OR b.modelid IS NULL ORDER BY a.modelid , a.compareOrder) AS sb INNER JOIN bikemodels m1 ON sb.modelid = m1.id AND m1.IsDeleted = 0 INNER JOIN bikemodels m2 ON sb.similarModelId = m2.id AND m2.IsDeleted = 0;";

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
                GetData(UrlType.Make, strMakes, SitemapList);
                GetData(UrlType.Model, strModels, SitemapList);
                GetData(UrlType.ModelImage, strModelImages, SitemapList);
                GetData(UrlType.ScooterMake, strScooterMakes, SitemapList);
                GetData(UrlType.ModelSpec, strModelSpecs, SitemapList);
                GetData(UrlType.MakeExpertReviews, strMakeExpertReviews, SitemapList);
                GetData(UrlType.ModelExpertReviews, strModelExpertReviews, SitemapList);
                GetData(UrlType.MakeNews, strMakeNews, SitemapList);
                GetData(UrlType.ModelNews, strModelNews, SitemapList);
                GetData(UrlType.MakeUpcoming, strMakeUpcoming, SitemapList);
                GetData(UrlType.MakeNewLaunches, strMakeNewLaunches, SitemapList);
                GetData(UrlType.MakeUserReviews, strMakeUserReviews, SitemapList);
                GetData(UrlType.ModelUserReviews, strModelUserReviews, SitemapList);
                GetData(UrlType.MakeVideos, strMakeVideos, SitemapList);
                GetData(UrlType.ModelVideos, strModelVideos, SitemapList);
                GetData(UrlType.ModelComparison, strModelCompare, SitemapList);
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("SiteMapRepository.GetData(): ", ex);

            }
            return SitemapList;
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
                    cmd.CommandType = CommandType.Text;
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
