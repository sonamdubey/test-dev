using Newtonsoft.Json;
using System;

[Serializable]
public class CarTradeLead
{
    [JsonProperty("customer_name")]
    public string CustomerName { get; set; }
    [JsonProperty("datetime")]
    public DateTime RequestDateTime { get; set; }
    [JsonProperty("customer_phone")]
    public string CustomerMobile { get; set; }
    [JsonIgnore]
    public string SourceIdentifier { get { return "cartrade"; } }
}