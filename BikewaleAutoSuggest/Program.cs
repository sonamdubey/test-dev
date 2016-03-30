using Consumer;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ElasticClientManager;
using log4net;
using System.Reflection;

namespace BikewaleAutoSuggest
{
    class Program
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            List<TempList> objList = GetBikeListDb.GetBikeList();

            Logs.WriteInfoLog("All Make Model List : " + objList.Count);

            var objPriceQuoteList = (from temp in objList
                                    where temp.ModelId > 0 && temp.New == true && temp.Futuristic==false
                                    select temp).ToList<TempList>();
            Logs.WriteInfoLog("Price quote make model List count : " + objPriceQuoteList.Count);

            List<BikeList> suggestionList = GetBikeListDb.GetSuggestList(objList);
            List<BikeList> PriceSuggestionList = GetBikeListDb.GetSuggestList(objPriceQuoteList);
            
            CreateIndex(suggestionList, ConfigurationManager.AppSettings["MMindexName"]);
            Logs.WriteInfoLog("All Make Model Index Created successfully");
            CreateIndex(PriceSuggestionList, ConfigurationManager.AppSettings["PQindexName"]);
            Logs.WriteInfoLog("Price Quote Make Model Index Created successfully");
           
        }

        private static void CreateIndex(List<BikeList> suggestionList, string indexName)
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
                                    .IndexAnalyzer("standard")
                                    .SearchAnalyzer("standard")
                                    .PreserveSeparators(false)))));
                }
                client.DeleteByQuery<BikeList>(dd => dd.Index(indexName)
                    .Type(ConfigurationManager.AppSettings["typeName"])
                    .Query(qq => qq.MatchAll())
                    );

                ElasticClientOperations.AddDocument<BikeList>(suggestionList, indexName, ConfigurationManager.AppSettings["typeName"], obj => obj.Id);
            }
            catch(Exception ex)
            {
                log.Error(MethodBase.GetCurrentMethod().Name, ex);
                Console.WriteLine(ex.Message);
            }
        }

    }
}
