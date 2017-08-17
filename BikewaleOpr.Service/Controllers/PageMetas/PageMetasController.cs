using Bikewale.Notifications;
using BikewaleOpr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers.PageMetas
{
    /// <summary>
    /// Controller for Page metas
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    /// <author>
    /// Sangram Nandkhile on 17-Aug-2017
    /// 
    /// </author>
    public class PageMetasController : ApiController
    {
        private readonly IPageMetas _pageMetas = null;
        public PageMetasController(IPageMetas pageMetas)
        {
            _pageMetas = pageMetas;
        }

        /// <summary>
        /// Created by  :   sangram Nandkhile on 17-08-2017
        /// Description :   updates page mets status
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="activecontract"></param>
        /// <returns></returns>
        [HttpPost, Route("api/pagemetas/update/{pageMetaId}/{status}/")]
        public IHttpActionResult UpdatePageMetaStatus(uint pageMetaId, ushort status)
        {
            try
            {
                bool result = _pageMetas.UpdatePageMetaStatus(pageMetaId, status);
                if (result)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("PageMetasController.UpdatePageMetaStatus({0},{1})", pageMetaId, status));
                return InternalServerError();
            }
        }
    }
}
