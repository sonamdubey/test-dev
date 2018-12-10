using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace AppWebApi.Models
{
    /* Author: Rakesh Yadav
     * Date Created: 12 June 2013
     * Discription: Create Model */
    [XmlRoot(ElementName = "filter")]
    public class Filter
    {
        [XmlAttribute(AttributeName = "displayName")]
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [XmlAttribute(AttributeName = "displayOrder")]
        [JsonProperty("displayOrder")]
        public string DisplayOrder { get; set; }
        [XmlAttribute(AttributeName = "queryVariable")]
        [JsonProperty("queryVariable")]
        public string QueryVariable { get; set; }

        public List<FilterItem> filterItem = new List<FilterItem>();
    }
}