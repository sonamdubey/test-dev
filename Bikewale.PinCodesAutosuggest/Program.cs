using Consumer;
using ElasticClientManager;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
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
                IEnumerable<PayLoad> objList = GetPinCodeListDb.GetPinCodeList();

                if (objList != null)
                {
                    Logs.WriteInfoLog("All PinCodes List : " + objList.Count());

                    IEnumerable<PinCodeList> suggestionList = GetPinCodeListDb.GetSuggestList(objList);

                    CreateIndex(suggestionList);
                    Logs.WriteInfoLog("All Make Model Index Created successfully");

                }
                else
                {
                    Logs.WriteInfoLog("No pincodes returned. Failed to create pincodes Index");
                }
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
                bool isUpdateOperation = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["isUpdate"]);
                string NewIndexName = System.Configuration.ConfigurationManager.AppSettings["NewIndexName"];
                

                if (isUpdateOperation)
                {
                    ElasticClientOperations.AddDocument<PinCodeList>(suggestionList.ToList(), NewIndexName, s => s.Id);
                }
                else
                {
                    var OldIndexName = System.Configuration.ConfigurationManager.AppSettings["OldIndexName"];
                    var aliasIndexName = System.Configuration.ConfigurationManager.AppSettings["aliasName"];
                    bool isDeleteOldIndex = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["isDeleteOldIndex"]);


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
    }
}
