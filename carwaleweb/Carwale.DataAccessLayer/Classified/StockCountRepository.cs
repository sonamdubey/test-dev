using Carwale.Entity.Classified;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications;
using Carwale.Interfaces.Classified;
using System.Collections;
using Nest;
using Carwale.DTOs.Classified;
using System.Linq;
using Carwale.Notifications.Logs;

namespace Carwale.DAL.Classified
{
    public class StockCountRepository : IStockCountRepository
    {

        /// <summary>
        /// Returns the count of Used Cars of a Car Make 
        /// Written By : Shalini on 05/11/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public UsedCarCount GetUsedCarsCount(int rootId,int cityId)
        {
            var usedCarsCount = new UsedCarCount();
            try
            {
                List<string> city = null;

                if (cityId == 1) 
                {
                    city = new List<string>() { "1", "6", "8", "13", "40" };
                }
                else if (cityId == 10)
                {
                    city = new List<string>() { "10", "224", "225", "246", "273" };
                }
                else 
                {
                    city = new List<string>() { cityId.ToString() };
                }

                string[] fields = { "price" };
                ElasticClient client = ElasticClientInstance.GetInstance();
                
                var results = client.Search<StockBaseEntity>(s => s.Index(System.Configuration.ConfigurationManager.AppSettings["ElasticIndexName"] ?? "newlocal")
                                                                .Type("stock")
                                                                
                                                                .Sort(sort=>sort.Field(selec=>selec.Field("price").Ascending()))
                                                                .Query(q => q
                                                                    .Bool(b => b
                                                                        .Filter(f => f
                                                                             .Bool(bb => bb.Must(ms =>
                                                                              {
                                                                               QueryContainer qq= ms.Term("rootId",rootId);
                                                                                  if (cityId > 0) {
                                                                                      qq &= ms.Terms(terms => terms.Field("cityIds").Terms<string>(city));
                                                                                    }
                                                                                 return qq;
                                                                             }
                                                                    )))
                                                            )));
                usedCarsCount.LiveListingCount = results.Total;
                usedCarsCount.MinLiveListingPrice = results.Total > 0 ? Convert.ToDouble(results.Documents.First().Price) : 0;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "StockRepository.GetUsedCarsCount()");
                objErr.LogException();
                throw;
            }
            return usedCarsCount;
        }
    }
}
