using Consumer;
using ElasticClientManager;
using System;
using System.Collections.Generic;

namespace Bikewale.PinCodesAutosuggest
{
    public class Program
    {

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            List<PayLoad> objList = GetPinCodeListDb.GetPinCodeList();

            if (objList != null)
            {
                Logs.WriteInfoLog("All PinCodes List : " + objList.Count);

                List<PinCodeList> suggestionList = GetPinCodeListDb.GetSuggestList(objList);

                CreateIndex(suggestionList, System.Configuration.ConfigurationManager.AppSettings["PinCodesIndexName"]);
                Logs.WriteInfoLog("All Make Model Index Created successfully");

            }
            else
            {
                Logs.WriteInfoLog("No pincodes returned. Failed to create pincodes Index");
            }



        }

        private static void CreateIndex(List<PinCodeList> suggestionList, string indexName)
        {
            try
            {
                bool isUpdateOperation = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["isUpdate"]);
                string NewIndexName = System.Configuration.ConfigurationManager.AppSettings["NewIndexName"];
                string TypeName = System.Configuration.ConfigurationManager.AppSettings["typeName"];

                if (isUpdateOperation)
                {
                    ElasticClientOperations.AddDocument<PinCodeList>(suggestionList, indexName, TypeName, s => s.Id);
                }
                else
                {
                    var OldIndexName = System.Configuration.ConfigurationManager.AppSettings["OldIndexName"];
                    var aliasIndexName = System.Configuration.ConfigurationManager.AppSettings["aliasName"];
                    bool isDeleteOldIndex = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["isDeleteOldIndex"]);
                    ElasticClientOperations.CreateIndex<PinCodeList>(req => req
                        .Index(indexName)
                        .AddMapping<PinCodeList>(type => type
                            .Type(TypeName)
                            .MapFromAttributes()
                            .Properties(prop => prop
                                .Completion(c => c
                                    .Name(pN => pN.mm_suggest)
                                    .Payloads()
                                    .IndexAnalyzer("standard")
                                    .SearchAnalyzer("standard")
                                    .PreserveSeparators(false)))));

                    ElasticClientOperations.AddDocument<PinCodeList>(suggestionList, indexName, TypeName, s => s.Id);
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
