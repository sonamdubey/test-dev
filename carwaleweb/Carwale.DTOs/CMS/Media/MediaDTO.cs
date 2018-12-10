using Carwale.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Media
{
    public class MediaDTO
    {

        [JsonProperty("photos", NullValueHandling = NullValueHandling.Ignore)]
        public PhotosListingDTO Photos { get; set; }

        [JsonProperty("videos", NullValueHandling = NullValueHandling.Ignore)]
        public VideoListingDTO Videos { get; set; }
    }

    public class PhotosListingDTO
    {
        [JsonProperty("photosList", NullValueHandling = NullValueHandling.Ignore)]
        public List<ModelPhotos> PhotosList { get; set; }

        [JsonProperty("photosRecordCount", NullValueHandling = NullValueHandling.Ignore)]
        public uint ImageRecordCount { get; set; }
        
        [JsonProperty("nextPageUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string NextPageUrl { get; set; }
    }
    

    public class VideoListingDTO
    {
        [JsonProperty("videosList", NullValueHandling = NullValueHandling.Ignore)]
        public List<ModelVideoDTO> VideosList { get; set; }

        [JsonProperty("videoRecordCount", NullValueHandling = NullValueHandling.Ignore)]
        public uint VideoRecordCount { get; set; }
        
        [JsonProperty("nextPageUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string NextPageUrl { get; set; }
    }

    public class ModelVideoDTO : MakeModelEntityV2
    {
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("videoCount")]
        public int VideoCount { get; set; }

        [JsonProperty("subCategoryName")]
        public string SubCategoryName { get; set; }

        [JsonProperty("basicId")]
        public int BasicId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("videoUrl")]
        public string VideoUrl { get; set; }

        [JsonProperty("views")]
        public string Views { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("displayDate")]
        public string DisplayDate { get; set; }
    }
    
}
