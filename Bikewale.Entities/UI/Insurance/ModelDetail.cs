using Newtonsoft.Json;

namespace Bikewale.UI.Entities.Insurance
{
    /// <summary>
    /// Created BY : Lucky Rathore on 18 November 2015.
    /// Description : For Bike's Models List. 
    /// </summary>
    public class ModelDetail
    {
        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }
    }
}
