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
     * Discription: create model */
    [XmlRoot(ElementName = "question")]
    public class FilterItem
    {
        
        [JsonProperty("label")]
        [XmlAttribute(AttributeName = "name")]
        public string Label { get; set; }
        [JsonProperty("value")]
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
        [JsonProperty("icon")]
        [XmlAttribute(AttributeName = "icon")]
        public string Icon { get; set; }
    }
}