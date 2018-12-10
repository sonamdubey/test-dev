using Carwale.Entity.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Media
{
    public class GalleryStateDTO
    {
        [JsonProperty("activeSection")]
        public GallerySections ActiveSection {get;set;}

        [JsonProperty("activeFilter")]
        public GalleryFilters ActiveFilter {get;set;}

        [JsonProperty("activeSlideIndex")]
        public int ActiveSlideIndex {get;set;}

        [JsonProperty("activeVideoIndex")]
        public int ActiveVideoIndex { get; set; }

        [JsonProperty("isSpecificUrl")]
        public bool IsSpecificUrl { get; set; } 
    }
}
