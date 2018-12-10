using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Utility.Classified;
using Nest;
using System.Collections.Generic;

namespace Carwale.DAL.Classified.ElasticSearch
{
    public class SortDescriptorRepository : ISortDescriptorRepository
    {
        public SortDescriptor<StockBaseEntity> GetSortDescriptorForNonFeaturedStocks(ElasticOuptputs filterInputs, string sortField, SortDescriptor<Entity.Classified.StockBaseEntity> sortDescriptor)
        {
            if (filterInputs.ShouldFetchNearbyCars)
            {
                sortDescriptor.Script(sc => GetSortScriptForNearbyCars(filterInputs));
            }
            if (!string.IsNullOrEmpty(filterInputs.so))
            {
                sortDescriptor
                    .Field(f => f.Field("sellerType"))
                    .Field(new Field(sortField), filterInputs.so == "1" ? Nest.SortOrder.Descending : Nest.SortOrder.Ascending);
            }
            else
            {
                sortDescriptor.Script(sc => GetSortScoreScript(filterInputs));
            }
            return sortDescriptor;
        }

        private static ScriptSort GetSortScoreScript(ElasticOuptputs filterInputs)
        {
            return new ScriptSort
            {
                Type = "number",
                Order = Nest.SortOrder.Descending,
                Script = new InlineScript("doc['sortScore'].value + pow(sin(num*doc['sortScore'])/10.0,2)")
                {
                    Lang = "groovy",
                    Params = new Dictionary<string, object>
                       {
                          {"num", (double.Parse(filterInputs.sessionId) % 10000)},
                       }
                }
            };
        }

        private static ScriptSort GetSortScriptForNearbyCars(ElasticOuptputs filterInputs)
        {
            return new ScriptSort
            {
                Type = "number",
                Script = new InlineScript("double diff = doc['location'].planeDistance(params.targetLat, params.targetLong) ; return (diff/1000 <= params.bucketRange ? 1 : 2)")
                {
                    Lang = "painless",
                    Params = new Dictionary<string, object>
                      {
                         { "targetLat",  filterInputs.Latitude },
                         { "targetLong", filterInputs.Longitude },
                         { "bucketRange", Constants.CarsNearMeBucketRange }
                      }
                }
            };
        }
    }
}
