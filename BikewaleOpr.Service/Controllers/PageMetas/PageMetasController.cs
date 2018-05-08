using Bikewale.Notifications;
using BikewaleOpr.BAL;
using BikewaleOpr.DTO.PageMeta;
using BikewaleOpr.Interface;
using System;
using System.Linq;
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
        /// Modified by : Snehal Dange on 30th Jan 2018
        /// Description: Changed datatype of 'pageMetaId' from uint to string to facilitate multiple delete functionalty
        /// Modified By : Deepak Israni on 20 April 2018
        /// Description : Versioned MakeDetails cache.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="activecontract"></param>
        /// <returns></returns>
        [HttpPost, Route("api/pagemetas/update/")]
        public IHttpActionResult UpdatePageMetaStatus([FromBody] PageMetaStatusDTO dtoObj)
        {
            try
            {
                int[] makeIdList = null;
                int[] modelIdList = null;

                bool result = _pageMetas.UpdatePageMetaStatus(dtoObj.PageMetaId, dtoObj.Status, dtoObj.UpdatedBy);

                if (!String.IsNullOrEmpty(dtoObj.MakeIdList))
                {
                    makeIdList = Array.ConvertAll(dtoObj.MakeIdList.TrimEnd(',').Split(','), int.Parse);
                }
                if (!String.IsNullOrEmpty(dtoObj.ModelIdList))
                {
                    modelIdList = Array.ConvertAll(dtoObj.ModelIdList.TrimEnd(',').Split(','), int.Parse);
                }
                if (result)
                {
                    if (makeIdList != null && makeIdList.Any())
                    {
                        foreach (var make in makeIdList.Distinct())
                        {
                            MemCachedUtil.Remove(string.Format("BW_MakeDetails_{0}_V1", make));
                        }
                    }
                    if (modelIdList != null && modelIdList.Any())
                    {
                        foreach (var model in modelIdList.Distinct())
                        {
                            MemCachedUtil.Remove(string.Format("BW_ModelDetail_V1_{0}", model));
                        }
                    }
                    return Ok(true);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("PageMetasController.UpdatePageMetaStatus({0},{1})", dtoObj.PageMetaId, dtoObj.Status));
                return InternalServerError();
            }
        }
    }
}
