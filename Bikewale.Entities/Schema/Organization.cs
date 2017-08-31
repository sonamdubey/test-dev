
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class Organization
    {
        [JsonProperty("@type")]
        public string Type { get { return "Organization"; } }

        [JsonProperty("name")]
        public string Name { get { return "BikeWale"; } }

        [JsonProperty("logo", NullValueHandling = NullValueHandling.Ignore)]
        public ImageObject Logo
        {
            get
            {
                return new ImageObject()
                {
                    ImageUrl = "https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-logo.png",
                    Height = "39",
                    Width = "114"
                };
            }
        }
            //[JsonProperty("logo", NullValueHandling = NullValueHandling.Ignore)]
            //public string LogoUrl { get; set; }
        }
    }
