using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.Articles;
using Carwale.DTOs.CMS.Photos;
using Carwale.Entity.CMS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS
{
    public class GenericContentDetailDTO
    {
       [JsonProperty("news",NullValueHandling = NullValueHandling.Ignore)]
       public ArticleDetails_V1 News;

       [JsonProperty("expertreview",NullValueHandling = NullValueHandling.Ignore)]
       public ArticlePageDetails_V1 ExpertReview;

       [JsonProperty("video",NullValueHandling = NullValueHandling.Ignore)]
       public VideoDTO_V1 VideoContent;

       [JsonProperty("userreview",NullValueHandling = NullValueHandling.Ignore)]
       public  UserReviewDetailDTO UserReview;

       [JsonProperty("gallery",NullValueHandling = NullValueHandling.Ignore)]
       public List<ModelImageDTO> PhotoGallery;
    }
}
