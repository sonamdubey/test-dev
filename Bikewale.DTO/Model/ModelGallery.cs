using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 4th Oct 2017
    /// Summary : DTO to hold default gallery component to show and list of components
    /// </summary>
    public class ModelGallery
    {
        [JsonProperty("selectedCategoryId")]
        public ushort SelectedCategoryId { get; set; }
        [JsonProperty("galleryComponents")]
        public IEnumerable<ModelGalleryComponent> GalleryComponents { get; set; }
    }
}
