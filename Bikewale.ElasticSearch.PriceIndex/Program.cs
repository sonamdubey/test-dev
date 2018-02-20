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
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            IIndexFactory<ModelPriceDocument> i = new IndexFactory<ModelPriceDocument>();

            Logs.WriteInfoLog("Start");
            var data = ModelPriceRepository.GetData();

            i.CreateIndex<ModelPriceDocument>(ConfigurationManager.AppSettings["ElasticIndexName"], m => m.Map<ModelPriceDocument>(type => type.AutoMap()));

            ElasticClientOperations.AddDocument<ModelPriceDocument>(data, ConfigurationManager.AppSettings["ElasticIndexName"], m => m.Id);

            Logs.WriteInfoLog("End");
        }
    }
}
