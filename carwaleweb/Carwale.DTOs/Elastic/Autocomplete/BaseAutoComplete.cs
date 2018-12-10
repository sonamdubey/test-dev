using Newtonsoft.Json;
using System;

namespace Carwale.DTOs.Elastic.Autocomplete
{
    [Serializable]
    public class BaseAutoComplete
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
