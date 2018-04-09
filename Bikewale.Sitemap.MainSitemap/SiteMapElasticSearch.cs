using Nest;
using System;
using System.Collections.Generic;
using Consumer;
using Bikewale.Sitemap.Entities;

namespace Bikewale.Sitemap.MainSitemap
{
    /// <summary>
    /// Created by  :   Pratibha Verma on 9 April 2018
    /// Description :   get data specs URL from ES
    /// </summary>
    public class SiteMapElasticSearch
    {
        private readonly ElasticClient _client;
        private static readonly string _displacement = "topVersion.displacement";
        private static readonly string _kerbWeight = "topVersion.kerbWeight";
        public SiteMapElasticSearch()
        {
            _client = ElasticSearchInstance.GetInstance();
        }

        public IDictionary<int, ICollection<KeyValuePair<int, string>>> GetSiteMapResult()
        {
            IDictionary<int, ICollection<KeyValuePair<int, string>>> siteMapESResult = null;
            try
            {
                var elasticReasult = _client.Search<SiteMapEntity>(
                       s => s.Index("bikeindex").Type("bikemodeldocument")
                       .From(0)
                       .Size(500)
                       .Query(q => q
                            .Bool(bq => bq
                                .Must(mq =>
                                    mq.Range(rq => rq
                                        .Field(_displacement).GreaterThan(0)
                                    )
                                    ||
                                    mq.Range(rq => rq
                                        .Field(_kerbWeight).GreaterThan(0)
                                    )
                                )
                            )
                         ));

                if (elasticReasult != null && elasticReasult.Hits != null && elasticReasult.Hits.Count > 0)
                {
                    siteMapESResult = new Dictionary<int, ICollection<KeyValuePair<int, string>>>();
                    int index = 0;
                    foreach (var x in elasticReasult.Documents)
                    {
                        ICollection<KeyValuePair<int, string>> cv = new List<KeyValuePair<int, string>>();
                        int i = 0;
                        cv.Add(new KeyValuePair<int, string>(i++,x.BikeMake.MakeMaskingName.ToString()));
                        cv.Add(new KeyValuePair<int, string>(i++, x.BikeModel.ModelMaskingName.ToString()));
                        siteMapESResult.Add(index++,cv);
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Bikewale.Sitemap.MainSitemap.GetSiteMapResult", ex);
            }
            return siteMapESResult;
        }
    }
}
