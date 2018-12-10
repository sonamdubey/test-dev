using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Carwale.Entity.Classified
{
    [Serializable, JsonObject]
    public class StockMake
    {
        [JsonProperty]
        public int MakeId { get; set; }

        [JsonProperty]
        public string MakeName { get; set; }

        [JsonProperty]
        public int MakeCount { get; set; }

        [JsonProperty]
        public List<StockRoot> RootList { get; set; }
    }

    [Serializable, JsonObject]
    public class StockRoot
    {
        [JsonProperty]
        public int RootId { get; set; }

        [JsonProperty]
        public string RootName { get; set; }

        [JsonProperty]
        public int RootCount { get; set; }

        [JsonProperty]
        public string ModelList { get; set; }

        [JsonProperty]
        public bool isSuperLuxury { get; set; }
    }

    //public class StockModel
    //{
    //    public int ModelId { get; set; }
    //    public string ModelName { get; set; }
    //    public int ModelCount { get; set; }
    //}
}
