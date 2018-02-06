using Bikewale.Notifications;
using BikewaleOpr.BAL;
using BikewaleOpr.Interface;
using System;
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
        /// Modified by : Ashutosh Sharma on 04 Oct 2017
        /// Description : Changed cacke key from 'BW_ModelDetail_' to 'BW_ModelDetail_V1'.
        /// Modified by : Rajan Chauhan on 06 Feb 2018.
        /// Description : Changed version of key from 'BW_ModelDetail_V1_' to 'BW_ModelDetail_'.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="activecontract"></param>
        /// <returns></returns>
        [HttpPost, Route("api/pagemetas/update/{pageMetaId}/{status}/{modelId}/{makeId}/")]
        public IHttpActionResult UpdatePageMetaStatus(uint pageMetaId, ushort status, uint modelId, uint makeId)
        {
            try
            {
                bool result = _pageMetas.UpdatePageMetaStatus(pageMetaId, status);
                if (result)
                {
                    if (modelId > 0)
                        MemCachedUtil.Remove(string.Format("BW_ModelDetail_{0}", modelId));

                    MemCachedUtil.Remove("BW_MakeDetails_" + makeId);

                    return Ok(true);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("PageMetasController.UpdatePageMetaStatus({0},{1})", pageMetaId, status));
                return InternalServerError();
            }
        }
    }
}
