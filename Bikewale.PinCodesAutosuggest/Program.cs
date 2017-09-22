using Consumer;
using ElasticClientManager;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Bikewale.PinCodesAutosuggest
{
    public class Program
    {
        /// <summary>
        /// Created By : Sushil Kumar on 9th March 2017
        /// Description : Main function to build elastic search index for pincode suggestion
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                IEnumerable<PayLoad> objList = GetPinCodeListDb.GetPinCodeList(1);

                if (objList != null)
                {
                    Logs.WriteInfoLog("All PinCodes List : " + objList.Count());

                    IEnumerable<PinCodeList> suggestionList = GetPinCodeListDb.GetSuggestList(objList);

                    CreateIndex(suggestionList);
                  
                    Logs.WriteInfoLog("All PinCode Index Created successfully");

                }
                else
                {
                    Logs.WriteInfoLog("No pincodes returned. Failed to create pincodes Index");
                }

                #region for capital first index (code to be refactor)
                IEnumerable<PayLoad> objListCP = GetPinCodeListDb.GetPinCodeList(2);

                if (objListCP != null)
                {
                    Logs.WriteInfoLog("capital first PinCodes List : " + objListCP.Count());

                    IEnumerable<PinCodeList> suggestionList = GetPinCodeListDb.GetSuggestList(objListCP);

                    CreateIndexCapitalFirst(suggestionList);

                    Logs.WriteInfoLog("capital first PinCode Index Created successfully");

                }
                else
                {
                    Logs.WriteInfoLog("No pincodes returned. Failed to create pincodes Index");
                } 
                #endregion

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Bikewale.PinCodesAutosuggest.Program.Main", ex);
                Console.WriteLine(ex.Message);
            }



        }

        /// <summary>
        /// Created By : Sushil Kumar on 9th March 2017
        /// Description : To create,modify and delete elastic search index for the specified indexname and suggestionlist
        /// </summary>
        /// <param name="suggestionList"></param>
        /// <param name="indexName"></param>
        private static void CreateIndex(IEnumerable<PinCodeList> suggestionList)
        {
            try
            {
                bool isUpdateOperation = Boolean.Parse(ConfigurationManager.AppSettings["isUpdate"]);
                string NewIndexName = ConfigurationManager.AppSettings["NewIndexName"];
                

                if (isUpdateOperation)
                {
                    ElasticClientOperations.AddDocument<PinCodeList>(suggestionList.ToList(), NewIndexName, s => s.Id);
                }
                else
                {
                    var OldIndexName = ConfigurationManager.AppSettings["OldIndexName"];
                    var aliasIndexName = ConfigurationManager.AppSettings["aliasName"];
                    bool isDeleteOldIndex = Boolean.Parse(ConfigurationManager.AppSettings["isDeleteOldIndex"]);


                    ElasticClient client = ElasticClientOperations.GetElasticClient();
                   
                        var response = client.CreateIndex(NewIndexName,
                          ind => ind
                       .Settings(s => s.NumberOfShards(2)
                           .NumberOfReplicas(2)
                       )
                      .Mappings(m => m
                          .Map<PinCodeList>(type => type.AutoMap()
                              .Properties(prop => prop
                              .Nested<PinCodeSuggestion>(n =>
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

                    


                    ElasticClientOperations.AddDocument<PinCodeList>(suggestionList.ToList(), NewIndexName, s => s.Id);
                    ElasticClientOperations.Alias(aliases => aliases.Remove(a => a.Alias(aliasIndexName).Index("*"))
                                               .Add(a => a.Alias(aliasIndexName).Index(NewIndexName)));
                    if (isDeleteOldIndex)
                    {
                        ElasticClientOperations.DeleteIndex(OldIndexName);
                    }
                }

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Bikewale.PinCodesAutosuggest.Program.CreateIndex", ex);
                Console.WriteLine(ex.Message);
            }
        }

        private static void CreateIndexCapitalFirst(IEnumerable<PinCodeList> suggestionList)
        {
            string indexName = ConfigurationManager.AppSettings["cfPincodeIndex"];
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
                              .Map<PinCodeList>(type => type.AutoMap()
                                  .Properties(prop => prop
                                  .Nested<PinCodeSuggestion>(n =>
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
                client.DeleteByQuery<PinCodeList>(dd => dd.Index(indexName)
                    .Type(ConfigurationManager.AppSettings["typeName"])
                    .Query(qq => qq.MatchAll())
                    );

                var response2 = ElasticClientOperations.AddDocument<PinCodeList>(suggestionList.ToList(), indexName, obj => obj.Id);


            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(MethodBase.GetCurrentMethod().Name, ex);
                Console.WriteLine(ex.Message);
            }


}
    }
}
