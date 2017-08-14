﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class Review
    {
        [JsonProperty("@type")]
        public string Type { get { return "Review"; } }
        public string Author { get { return "Rating"; } }
        public string DatePublished { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public ReviewRating ReviewRating {get;set;}
    }
}
