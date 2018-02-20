using Bikewale.ElasticSearch.Entities;
using Bikewale.ElasticSearch.Indexes;
using Consumer;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElasticClientManager;

namespace Bikewale.ElasticSearch.PriceIndex
{
    /// <summary>
    /// Created By : Deepak Israni on 19 Feb 2018
    /// Description: Program for creation of bikewalepricingindex
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            IIndexFactory<ModelPriceDocument> i = new IndexFactory<ModelPriceDocument>();

            Logs.WriteInfoLog("Start");

            var data = ModelPriceRepository.GetData();
            Logs.WriteInfoLog("Data fetched, pricing list count: " + data.Count());

            i.DeleteIndex(ConfigurationManager.AppSettings["ElasticIndexName"]);

            Logs.WriteInfoLog("Initialize creation of bikewalepricingindex.");
            i.CreateIndex<ModelPriceDocument>(ConfigurationManager.AppSettings["ElasticIndexName"], m => m.Map<ModelPriceDocument>(type => type.AutoMap()));
            Logs.WriteInfoLog("Index created.");

            Logs.WriteInfoLog("Initialize storing documents in bikewalepricingindex.");
            ElasticClientOperations.AddDocument<ModelPriceDocument>(data, ConfigurationManager.AppSettings["ElasticIndexName"], m => m.Id);
            
            Logs.WriteInfoLog("End");
        }
    }
}
