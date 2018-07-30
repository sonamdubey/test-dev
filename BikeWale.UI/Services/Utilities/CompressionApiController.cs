using System.Web.Http;
using System.Web.Http.Results;

namespace Bikewale.Service.Utilities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 12 May 2016
    /// Description :   Extends ApiController to return the overridden Ok response.    
    /// </summary>
    public class CompressionApiController : ApiController
    {

        /// <summary>
        /// Created by  :   Sumit Kate on 12 May 2016
        /// Description :   Overrride the Ok response.
        /// It will enable gzip compression for the response from web api.        
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns>custom Ok result is returned</returns>
        protected override OkNegotiatedContentResult<T> Ok<T>(T content)
        {
            return new CustomOkResult<T>(content, this);
        }

        ///// <summary>
        ///// Returns the http Request as No content
        ///// </summary>
        ///// <returns>
        ///// Created by : Sangram Nandkhile on 08-May-2017 
        ///// </returns>
        //public StatusCodeResult NoContent()
        //{
        //    return new StatusCodeResult(HttpStatusCode.NoContent, this);
        //}

    }
}