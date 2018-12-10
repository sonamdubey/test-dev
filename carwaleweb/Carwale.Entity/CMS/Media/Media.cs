using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.Media
{
    [Serializable,JsonObject]
    public class Media 
    {
        
        [JsonProperty("photos", NullValueHandling = NullValueHandling.Ignore)]
        public PhotosListing Photos { get; set; }

        [JsonProperty("videos", NullValueHandling = NullValueHandling.Ignore)]
        public VideoListing Videos { get; set; }
    }

    [Serializable, JsonObject]
    public class PhotosListing
    {
        [JsonProperty("photosList", NullValueHandling = NullValueHandling.Ignore)]
        public List<ModelPhotos> PhotosList { get; set; }
        
        [JsonProperty("photosRecordCount", NullValueHandling = NullValueHandling.Ignore)]
        public uint ImageRecordCount { get; set; }
    }

    [Serializable, JsonObject]
    public class VideoListing
    {
        [JsonProperty("videosList", NullValueHandling = NullValueHandling.Ignore)]
        public List<ModelVideo> VideosList { get; set; }

        [JsonProperty("videoRecordCount", NullValueHandling = NullValueHandling.Ignore)]
        public uint VideoRecordCount { get; set; }
    }
}
