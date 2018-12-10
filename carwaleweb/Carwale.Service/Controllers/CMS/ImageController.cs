using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Service.Filters.CMS;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Carwale.Service.Controllers.CMS
{
    public class ImageController : ApiController
    {
        private readonly IPhotos _photos;
        public ImageController(IPhotos photos)
        {
            _photos = photos;
        }

        /// <summary>
        /// api to get list od images based on modelid
        /// written by Natesh Kumar on 20/9/14
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        [HttpGet,CMSImageApiValidator]     // actionfilter for querystring validator
        public IHttpActionResult ModelPhotoList([FromUri]ModelPhotosBycountURI queryString)
        {
            var content = _photos.GetModelPhotosByCount(queryString);

            if (content.Count <= 0)
            {
                return NotFound();
            }

            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json") });
        }

        /// <summary>
        /// api to get required no. of photolist based on modelid
        /// written by Natesh Kumar on 20/9/14
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        [HttpGet,CMSImageApiValidator] // actionfilter for querystring validator
        public IHttpActionResult ModelPhotoByCount([FromUri]ModelPhotosBycountURI queryString)
        {
            var content = _photos.GetModelPhotosByCount(queryString);

            if (content.Count <= 0)
                return NotFound();

            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json") });
        }

        /// <summary>
        /// api to get list of photos for other models
        /// written by Natesh Kumar on 20/9/14
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        [HttpGet,CMSImageApiValidator]  // actionfilter for querystring validator
        public IHttpActionResult OtherModelPhotoList([FromUri]RelatedPhotoURI queryString)
        {
            var content = _photos.GetOtherModelPhotosList(queryString);

            if (content.RecordCount <= 0)
                return NotFound();

            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json") });
        }

        /// <summary>
        /// api to get list of photo for similar models
        /// written by Natesh Kumar on 20/9/14
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        [HttpGet,CMSImageApiValidator]  // actionfilter for querystring validator
        public IHttpActionResult SimilarModelPhotoList([FromUri]RelatedPhotoURI queryString)
        {
            var content = _photos.GetSimilarModelPhotosList(queryString);

            if (content.RecordCount <= 0)
            {
                return NotFound();
            }

            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json") });
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 30 Sept 2014
        /// Summary : Function to get the the ariticle photos list for the given basic id.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetArticlePhotos([FromUri]ArticlePhotoUri queryString)
        {
            if (queryString.basicId == 0)
                return BadRequest();

            var content = _photos.GetArticlePhotos(queryString);

            if (content.Count <= 0)
            {
                return NotFound();
            }

            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json") });
        }

    }
}
