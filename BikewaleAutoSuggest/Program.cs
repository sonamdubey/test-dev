using Consumer;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ElasticClientManager;
using System.Reflection;
using Elasticsearch.Net;

namespace BikewaleAutoSuggest
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            IEnumerable<TempList> objList = GetBikeListDb.GetBikeList();

            Logs.WriteInfoLog("All Make Model List : " + objList.Count());

            //IEnumerable<TempList> objPriceQuoteList = (from temp in objList
            //                        where temp.ModelId > 0 && temp.New && !temp.Futuristic
            //                        select temp);
            //Logs.WriteInfoLog("Price quote make model List count : " + objPriceQuoteList.Count());
            //IEnumerable<TempList> ObjUserReviewList = objList.Where(x => x.UserRatingsCount > 0);

            //Logs.WriteInfoLog("UserReview make model List count : " + ObjUserReviewList.Count());

            IEnumerable<BikeList> suggestionList = GetBikeListDb.GetSuggestList(objList);

            //IEnumerable<BikeList> PriceSuggestionList = GetBikeListDb.GetSuggestList(objPriceQuoteList);

            //IEnumerable<BikeList> UserReviewList = GetBikeListDb.GetSuggestList(ObjUserReviewList);

            CreateIndex(suggestionList, ConfigurationManager.AppSettings["MMindexName"]);
            Logs.WriteInfoLog("All Make Model Index Created successfully");

            //CreateIndex(PriceSuggestionList, ConfigurationManager.AppSettings["PQindexName"]);
            //Logs.WriteInfoLog("Price Quote Make Model Index Created successfully");

            //CreateIndex(UserReviewList, Bikewale.Utility.BWConfiguration.Instance.UserReviewIndexName);
            //Logs.WriteInfoLog("User Review Make Model Index Created successfully");

        }


        private static void CreateIndex(IEnumerable<BikeList> suggestionList, string indexName)
        {
            try
            {

                ElasticClient client = ElasticClientOperations.GetElasticClient();
                if (!client.IndexExists(indexName).Exists)
                {

                    var response = client.CreateIndex(indexName,
                      ind => ind
                   .Settings(s => s.NumberOfShards(2)
                       .NumberOfReplicas(2)
                   )
                  .Mappings(m => m
                      .Map<BikeList>(type => type.AutoMap()
                          .Properties(prop => prop
                          .Nested<BikeSuggestion>(n =>
                                  n.Name(c => c.mm_suggest)
                                  .AutoMap()
                                  .Properties(prop2 => prop2
                                      .Nested<PayLoad>(n2 =>
                                          n2.Name(c2 =>
                                              c2.input).AutoMap())))
                              .Completion(c => c
                              .Name(pN => pN.mm_suggest)
                              .Contexts(cont => cont
                                    .Category(cate => cate
                                        .Name("types").Path(s => s.mm_suggest.contexts.types)
                                        ))

                              .Analyzer("standard")
                              .SearchAnalyzer("standard")
                              .PreserveSeparators(false))))));
             
                }
                client.DeleteByQuery<BikeList>(dd => dd.Index(indexName)
                    .Type(ConfigurationManager.AppSettings["typeName"])
                    .Query(qq => qq.MatchAll())
                    );

                var response2 = ElasticClientOperations.AddDocument<BikeList>(suggestionList.ToList(), indexName, obj => obj.Id);

            }
            catch(Exception ex)
            {
                Logs.WriteErrorLog(MethodBase.GetCurrentMethod().Name, ex);
                Console.WriteLine(ex.Message);
            }
        }



    }
}
