using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    /// <summary>
    /// Written By : Supriya K
    /// Created Date : 1/7/2014
    /// DESC : Entity for videoUrl of model
    /// </summary>
    public class ModelVideoList
    {
        [JsonProperty("videoUrl")]
        public string VideoUrl { get; set; }

        [JsonProperty("videoTitle")]
        public string VideoTitle { get; set; }
    }
}