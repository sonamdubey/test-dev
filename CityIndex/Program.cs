using Consumer;
using ElasticClientManager;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
namespace CityAutoSuggest
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            IEnumerable<CityTempList> objCityList = GetCityList.CityList();                                    //  Call City SP
            Logs.WriteInfoLog("city List count : " + objCityList.Count());

            IEnumerable<CityList> CityList = GetCityList.GetSuggestList(objCityList);                          //  Create Inputs, Outputs and Payload

            CreateIndex(CityList);                          //  Create Index
        }

        private static void CreateIndex(IEnumerable<CityList> suggestionList)
        {
            try
            {
                string indexName = ConfigurationManager.AppSettings["cityIndexName"];

                    ElasticClient client = ElasticClientOperations.GetElasticClient();
                    if (!client.IndexExists(indexName).Exists)
                    {
                        var response = client.CreateIndex(indexName,
                          ind => ind
                       .Settings(s => s.NumberOfShards(2)
                           .NumberOfReplicas(2)
                       )
                      .Mappings(m => m
                          .Map<CityList>(type => type.AutoMap()
                              .Properties(prop => prop
                              .Nested<CitySuggestion>(n =>
                                      n.Name(c => c.mm_suggest)
                                      .AutoMap()
                                      .Properties(prop2 => prop2
                                          .Nested<Payload>(n2 =>
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
                    client.DeleteByQuery<CityList>(dd => dd.Index(indexName)
                        .Type(ConfigurationManager.AppSettings["typeName"])
                        .Query(qq => qq.MatchAll())
                        );


                    ElasticClientOperations.AddDocument<CityList>(suggestionList.ToList(), indexName, obj => obj.Id);
                
         
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(MethodBase.GetCurrentMethod().Name, ex);
                Console.WriteLine(ex.Message);
            }
        }
    
    }
}
