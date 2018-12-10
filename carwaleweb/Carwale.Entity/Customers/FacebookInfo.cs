using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Customers
{
    public class FBGraph
    {
    public string CustomerId { get; set; }
    [JsonProperty("id")]
    public string Id {get;set;}
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("first_name")]
    public string First_name { get; set; }
    [JsonProperty("gender")]
    public string Gender { get; set; }
    [JsonProperty("last_name")]
    public string Last_name { get; set; }
    [JsonProperty("link")]
    public string Link { get; set; }
    [JsonProperty("locale")]
    public string Locale { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("updated_time")]
    public string Updated_time { get; set; }
    [JsonProperty("username")]
    public string Username { get; set; }
    [JsonProperty("verified")]
    public bool Verified { get; set; }
    }
}
