using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.URIs
{
    /// <summary>
    /// for common parameters in articlebycategory and articlebysubcategory
    /// written by Natesh kumar on 19/9/14
    /// </summary>
    public class CommonURI
    {
        [JsonProperty(PropertyName = "applicationid")]
        public ushort ApplicationId { get; set; }

        [JsonProperty(PropertyName = "startindex")]
        public uint StartIndex { get; set; }

        [JsonProperty(PropertyName = "endindex")]
        public uint EndIndex { get; set; }

        [JsonProperty(PropertyName = "makeid")]
        public int MakeId { get; set; }

        [JsonProperty(PropertyName = "modelid")]
        public int ModelId { get; set; }

        [JsonProperty(PropertyName = "pageSize")]
        public uint PageSize { get; set; }

        [JsonProperty(PropertyName = "pageNo")]
        public uint PageNo { get; set; }

    }

    /// <summary>
    /// uri for articles by category inherited property from CommonURI
    /// written by Natesh kumar on 19/9/14
    /// </summary>
    public class ArticleByCatAndIdURI : CommonURI
    {
        [JsonProperty(PropertyName = "categoryidlist")]
        public string CategoryIdList { get; set; }
        [JsonProperty(PropertyName = "basicid")]
        public ulong BasicId { get; set; }
    }

    /// <summary>
    /// uri for articles by category inherited property from CommonURI
    /// written by Natesh kumar on 19/9/14
    /// </summary>
    public class ArticleByCatURI : CommonURI
    {
        [JsonProperty(PropertyName = "categoryidlist")]
        public string CategoryIdList { get; set; }
        [JsonProperty(PropertyName = "getAllMedia")]
        public bool GetAllMedia { get; set; }
    }

    /// <summary>
    /// uri for article by subcategory inherited property from CommonURI
    /// written by Natesh kumar on 19/9/14
    /// </summary>
    public class ArticleBySubCatURI : CommonURI
    {
        [JsonProperty(PropertyName = "categoryidlist")]
        public string CategoryIdList { get; set; }

        [JsonProperty(PropertyName = "subcategoryid")]
        public ushort SubCategoryId { get; set; }

        [JsonProperty(PropertyName = "subCategories")]
        public string SubCategories { get; set; }
    }

    /// <summary>
    /// uri for page details and content details
    /// written by Natesh kumar on 19/9/14
    /// </summary>
    public class ArticleContentURI
    {
        [JsonProperty(PropertyName = "basicid")]
        public ulong BasicId { get; set; }

        [JsonProperty(PropertyName = "applicationid")]
        public ushort ApplicationId { get; set; }
    }

    /// <summary>
    /// uri for featured articles
    /// written by Natesh kumar on 19/9/14
    /// </summary>
    public class ArticleFeatureURI
    {
        [JsonProperty(PropertyName = "applicationid")]
        public ushort ApplicationId { get; set; }

        [JsonProperty(PropertyName = "contenttypes")]
        public string ContentTypes { get; set; }

        [JsonProperty(PropertyName = "totalrecords")]
        public ushort TotalRecords { get; set; }
    }

    /// <summary>
    /// uri for recent articles with inherited property from featured
    /// written by Natesh kumar on 19/9/14
    /// </summary>
    public class ArticleRecentURI : ArticleFeatureURI
    {
        [JsonProperty(PropertyName = "makeid")]
        public int MakeId { get; set; }

        [JsonProperty(PropertyName = "modelid")]
        public int ModelId { get; set; }
    }
    /// <summary>
    /// uri for news feed input
    /// created by jitendra singh on 27 jan 2017
    /// </summary>
    public class ContentFeedURI
    {
        public string Slug { get; set; }
        public int SubCategoryId { get; set; }
        public int ApplicationId { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }

    /// <summary>
    /// uri for related articles with inherited property from featured
    /// written by Natesh kumar on 19/9/14
    /// </summary>
    public class ArticleRelatedURI : ArticleFeatureURI
    {
        [JsonProperty(PropertyName = "basicId")]
        public uint BasicId { get; set; }
    }
}
