using Nest;
using System;
using System.Collections.Generic;
using Consumer;
using Bikewale.Sitemap.Entities;

namespace Bikewale.Sitemap.MainSitemap
{
    public class SiteMapElasticSearch
    {
        private readonly ElasticClient _client;
        private static readonly string _displacement = "topVersion.displacement";
        private static readonly string _kerbWeight = "topVersion.kerbWeight";
        public SiteMapElasticSearch()
        {
            _client = ElasticSearchInstance.GetInstance();
        }

        public IEnumerable<SiteMapResultEntity> GetSiteMapResult()
        {
            List<SiteMapResultEntity> siteMapResultList = null;
            try
            {
                siteMapResultList = new List<SiteMapResultEntity>();
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
                  foreach (var x in elasticReasult.Documents)
                    {
                        siteMapResultList.Add(new SiteMapResultEntity() {
                            MakeMaskingName = x.BikeMake.MakeMaskingName,
                            ModelMaskingName = x.BikeModel.ModelMaskingName
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Bikewale.Sitemap.MainSitemap.GetSiteMapResult", ex);
            }
            return siteMapResultList;
        }
    }
}
