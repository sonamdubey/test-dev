using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.URIs
{
    /// <summary>
    /// uri for model photo list
    /// written by Natesh kumar on 19/9/14
    /// </summary>
    public class ModelPhotoURI
    {
        [JsonProperty(PropertyName = "applicationid")]
        public ushort ApplicationId { get; set; }

        [JsonProperty(PropertyName = "modelid")]
        public int ModelId { get; set; }

        [JsonProperty(PropertyName = "categoryidlist")]
        public string CategoryIdList { get; set; }

        [JsonProperty(PropertyName = "platformId")]
        public string PlatformId { get; set; }

        [JsonIgnore]
        public string SimilarModelsList { get; set; }
    }
    /// <summary>
    /// uri for model photo with no. of records required
    /// written by Natesh kumar on 19/9/14
    /// </summary>
    public class ModelPhotosBycountURI : ModelPhotoURI
    {
        [JsonProperty(PropertyName = "totalrecords")]
        public uint TotalRecords { get; set; }
    }

    /// <summary>
    /// uri for photos related to model
    /// written by Natesh kumar on 19/9/14
    /// </summary>
    public class RelatedPhotoURI : ModelPhotoURI
    {

        [JsonProperty(PropertyName = "startindex")]
        public uint StartIndex { get; set; }

        [JsonProperty(PropertyName = "endindex")]
        public uint EndIndex { get; set; }

    }

    /// <summary>
    /// Written By : Ashish G. Kamble on 30 Sept 2014
    /// Summary : Class to hold uri data for article photo gallery.
    /// </summary>
    public class ArticlePhotoUri
    {
        [JsonProperty(PropertyName = "basicid")]
        public ulong basicId { get; set; }
    }
}
