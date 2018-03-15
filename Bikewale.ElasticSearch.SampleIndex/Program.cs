
using Bikewale.ElasticSearch.Entities;
using Bikewale.ElasticSearch.Indexes;
using Consumer;
using ElasticClientManager;
using System.Collections.Generic;
namespace Bikewale.ElasticSearch.SampleIndex
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            IIndexFactory<EmployeeDocument> i = new IndexFactory<EmployeeDocument>();

            Logs.WriteInfoLog("Start");
            var data = new List<EmployeeDocument>();
            data.Add(new EmployeeDocument { Id = "1", Age = 25, Name = "Alice" });
            data.Add(new EmployeeDocument { Id = "2", Age = 23, Name = "Bob" });
            data.Add(new EmployeeDocument { Id = "3", Age = 26, Name = "Charlie" });

            i.CreateIndex<EmployeeDocument>("employeeindex", m => m.Map<EmployeeDocument>(type => type.AutoMap()));

            ElasticClientOperations.AddDocument<EmployeeDocument>(data, "employeeindex", m => m.Id);

            i.DeleteIndex("employeeindex");
            Logs.WriteInfoLog("End");
        }
    }

    class EmployeeDocument : Document
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
