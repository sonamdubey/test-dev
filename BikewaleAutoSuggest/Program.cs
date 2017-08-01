using Consumer;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ElasticClientManager;
using System.Reflection;


namespace BikewaleAutoSuggest
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            IEnumerable<TempList> objList = GetBikeListDb.GetBikeList();

            Logs.WriteInfoLog("All Make Model List : " + objList.Count());

            IEnumerable<TempList> objPriceQuoteList = (from temp in objList
                                    where temp.ModelId > 0 && temp.New && !temp.Futuristic
                                    select temp);
            Logs.WriteInfoLog("Price quote make model List count : " + objPriceQuoteList.Count());
            IEnumerable<TempList> ObjUserReviewList = objList.Where(x => x.UserReviewCount > 0);

            Logs.WriteInfoLog("UserReview make model List count : " + ObjUserReviewList.Count());

            IEnumerable<BikeList> suggestionList = GetBikeListDb.GetSuggestList(objList);

            IEnumerable<BikeList> PriceSuggestionList = GetBikeListDb.GetSuggestList(objPriceQuoteList);

            IEnumerable<BikeList> UserReviewList = GetBikeListDb.GetSuggestList(ObjUserReviewList);

            CreateIndex(suggestionList, ConfigurationManager.AppSettings["MMindexName"]);
            Logs.WriteInfoLog("All Make Model Index Created successfully");

            CreateIndex(PriceSuggestionList, ConfigurationManager.AppSettings["PQindexName"]);
            Logs.WriteInfoLog("Price Quote Make Model Index Created successfully");

            CreateIndex(UserReviewList,Bikewale.Utility.BWConfiguration.Instance.UserReviewIndexName);
            Logs.WriteInfoLog("User Review Make Model Index Created successfully");

        }

        private static void CreateIndex(IEnumerable<BikeList> suggestionList, string indexName)
        {
            try
            {

                ElasticClient client = ElasticClientOperations.GetElasticClient();
                if (!client.IndexExists(indexName).Exists)
                {
                    ElasticClientOperations.CreateIndex<BikeList>(req => req
                        .Index(indexName)
                        .AddMapping<BikeList>(type => type
                            .Type(ConfigurationManager.AppSettings["typeName"])
                            .MapFromAttributes()
                            .Properties(prop => prop
                                .Completion(c => c
                                    .Name(pN => pN.mm_suggest)
                                    .Payloads()
                                     .Context(cont => cont
                                    .Category("types", cate => cate
                                        .Default("makemodelall")))
                                    .IndexAnalyzer("standard")
                                    .SearchAnalyzer("standard")
                                    .PreserveSeparators(false)))));
                }
                client.DeleteByQuery<BikeList>(dd => dd.Index(indexName)
                    .Type(ConfigurationManager.AppSettings["typeName"])
                    .Query(qq => qq.MatchAll())
                    );

                ElasticClientOperations.AddDocument<BikeList>(suggestionList.ToList(), indexName, ConfigurationManager.AppSettings["typeName"], obj => obj.Id);
            }
            catch(Exception ex)
            {
                Logs.WriteErrorLog(MethodBase.GetCurrentMethod().Name, ex);
                Console.WriteLine(ex.Message);
            }
        }



    }
}
