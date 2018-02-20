using Bikewale.ElasticSearch.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.ElasticSearch.Indexes;
using Consumer;
using ElasticClientManager;
using System.Configuration;
namespace BikeIndex
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 20th Feb 2018
    /// Description: Program.cs for creating index and adding new doc to index (bikeindex) 
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            IIndexFactory<BikeModelDocument> bikeIndex = new IndexFactory<BikeModelDocument>();
            string indexName = ConfigurationManager.AppSettings["BikeIndexName"];

            Logs.WriteInfoLog("Bike Index Creation Started");

            BikeModelRepository objDAL = new BikeModelRepository();
            IEnumerable<BikeModelDocument> data = objDAL.GetBikeModelList();
            
            bikeIndex.CreateIndex<BikeModelDocument>(indexName, m => m.Map<BikeModelDocument>(type => type.AutoMap()));

            Logs.WriteInfoLog("Index Created");
            Logs.WriteInfoLog("Data Size = " + data.Count());

            ElasticClientOperations.AddDocument<BikeModelDocument>(data.ToList(), indexName, m => m.Id);

            Logs.WriteInfoLog("Document Added to bikeindex");


        }
    }
}
