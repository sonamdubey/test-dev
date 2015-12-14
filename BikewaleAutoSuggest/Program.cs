using Consumer;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleAutoSuggest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TempList> objList = GetBikeListDb.GetBikeList();

            Logs.WriteInfoLog("All Make Model List : " + objList.Count);

            var objPriceQuoteList = (from temp in objList
                                    where temp.ModelId > 0 && temp.New == true && temp.Futuristic==false
                                    select temp).ToList<TempList>();
            Logs.WriteInfoLog("Price quote make model List count : " + objPriceQuoteList.Count);

            List<CityTempList> objCityList = GetCityList.CityList();
            Logs.WriteInfoLog("city List count : " + objCityList.Count);

            List<BikeList> suggestionList = GetBikeListDb.GetSuggestList(objList);
            List<BikeList> PriceSuggestionList = GetBikeListDb.GetSuggestList(objPriceQuoteList);
            List<CityList> CityList = GetCityList.GetSuggestList(objCityList);

            CreateIndex(suggestionList, ConfigurationManager.AppSettings["MMindexName"]);
            Logs.WriteInfoLog("All Make Model Index Created successfully");
            CreateIndex(PriceSuggestionList, ConfigurationManager.AppSettings["PQindexName"]);
            Logs.WriteInfoLog("Price Quote Make Model Index Created successfully");
            CreateIndex(CityList, ConfigurationManager.AppSettings["cityIndexName"]);
            Logs.WriteInfoLog("All City Index Created successfully");
        }

        private static void CreateIndex(List<BikeList> suggestionList, string indexName)
        {
            try
            {
                string[] HostUrls = ConfigurationManager.AppSettings["esHost"].Split(';');
                Random arbit = new Random();
                var node = new Uri("http://" + HostUrls[arbit.Next(HostUrls.Length)]);

                var settings = new ConnectionSettings(
                    node,
                    defaultIndex: indexName
                );

                var elasticClient = new ElasticClient(settings);

                elasticClient.DeleteByQuery<BikeList>(dd => dd
                    .Type(ConfigurationManager.AppSettings["typeName"])
                    .Query(qq => qq.MatchAll())
                    );


                foreach (BikeList item in suggestionList)
                {
                    Console.WriteLine(item.Id);
                    elasticClient.Index<BikeList>(item, qq => qq.Id(item.Id).Index(indexName).Type(ConfigurationManager.AppSettings["typeName"]));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void CreateIndex(List<CityList> suggestionList, string indexName)
        {
            try
            {
               // var node = new Uri("http://" + ConfigurationManager.AppSettings["esHost"] + ":9200");

                string[] HostUrls = ConfigurationManager.AppSettings["esHost"].Split(';');
                Random arbit = new Random();
                var node = new Uri("http://" + HostUrls[arbit.Next(HostUrls.Length)]);    //check point

                var settings = new ConnectionSettings(
                    node,
                    defaultIndex: indexName
                );

                var elasticClient = new ElasticClient(settings);

                elasticClient.DeleteByQuery<BikeList>(dd => dd
                    .Type(ConfigurationManager.AppSettings["typeName"])
                    .Query(qq => qq.MatchAll())
                    );


                foreach (CityList item in suggestionList)
                {
                    Console.WriteLine(item.Id);
                    elasticClient.Index<CityList>(item, qq => qq.Id(item.Id).Index(indexName).Type(ConfigurationManager.AppSettings["typeName"]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
