using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Bikewale.Service.Utilities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 12 May 2016
    /// Description :   Inherite from built in OK result type to support web api response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomOkResult<T> : OkNegotiatedContentResult<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="controller"></param>
        public CustomOkResult(T content, ApiController controller)
            : base(content, controller) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentNegotiator"></param>
        /// <param name="request"></param>
        /// <param name="formatters"></param>
        public CustomOkResult(T content, IContentNegotiator contentNegotiator, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
            : base(content, contentNegotiator, request, formatters) { }

        /// <summary>
        /// Created by  :   Sumit Kate on 12 May 2016
        /// Description :   Override ExecuteAsync which is default method to return the API response into requested format
        /// Override the Response content with string content.
        /// The Host server takes care of compressing using gzip when requested using HTTP header Accept-Encoding: gzip, deflate, sdch
        /// StringContent is filled with Serialized content of type T
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.ExecuteAsync(cancellationToken);
            //Serializes the content into string and modify the response
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(this.Content));
            return response;
        }
    }
}