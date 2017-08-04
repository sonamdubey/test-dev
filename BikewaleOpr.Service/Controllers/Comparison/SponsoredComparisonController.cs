using Bikewale.Comparison.DTO;
using Bikewale.Comparison.Entities;
using Bikewale.Comparison.Interface;
using Bikewale.Notifications;
using BikewaleOpr.Service.AutoMappers;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers.Comparison
{

    /// <summary>
    /// Controller for sponsored comparisons
    /// </summary>
    /// <author>
    /// Sangram Nandkhile on 28-Jul-2017
    /// </author>
    public class SponsoredComparisonController : ApiController
    {
        private readonly ISponsoredComparisonRepository _objSponsoredRepo = null;

        public SponsoredComparisonController(ISponsoredComparisonRepository objSponsoredRepo)
        {
            _objSponsoredRepo = objSponsoredRepo;
        }

        /// <summary>
        /// Gets the Comparisons.
        /// </summary>
        /// <param name="statuses">The statuses.</param>
        /// <returns></returns>
        [HttpGet, Route("api/compare/sponsored/{statuses}/")]
        public IHttpActionResult GetSponsoredComparisons(string statuses)
        {
            IEnumerable<SponsoredComparison> objSponsoredComparison = null;
            IEnumerable<SponsoredComparisonDTO> objSponsoredComparisonDTO = null;
            try
            {
                objSponsoredComparison = _objSponsoredRepo.GetSponsoredComparisons(statuses);
                if (objSponsoredComparison != null)
                {
                    // Auto map the properties
                    objSponsoredComparisonDTO = SponsoredComparisonMapper.Convert(objSponsoredComparison);
                    return Ok(objSponsoredComparisonDTO);
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("comparison.SponsoredComparisonController.GetSponsoredComparisons", statuses));
                return InternalServerError();
            }
        }


        [HttpGet, Route("api/compare/sponsored/{id}/target/{targetModelId}/sponsor/{sponsoredModelId}/")]
        public IHttpActionResult GetSponsoredVersionMapping(uint id, uint targetModelId, uint sponsoredModelId)
        {
            TargetSponsoredMapping objVersionMapping = null;
            TargetSponsoredMappingDTO objVersionMappingDTO = null;
            try
            {
                objVersionMapping = _objSponsoredRepo.GetSponsoredComparisonVersionMapping(id, targetModelId, sponsoredModelId);
                if(objVersionMapping!= null)
                {
                    objVersionMappingDTO = SponsoredComparisonMapper.Convert(objVersionMapping);
                    return Ok(objVersionMappingDTO);
                }
                else
                {
                    return InternalServerError();
                }
                
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("comparison.SponsoredComparisonController.GetSponsoredVersionMapping: comparison Id: {0}", id));
                return InternalServerError();
            }
        }


        /// <summary>
        /// Saves the sponsored Comparison.
        /// </summary>
        /// <param name="Comparison">The Comparison.</param>
        /// <returns></returns>
        [HttpPost, Route("api/compare/sponsored/save/")]
        public IHttpActionResult SaveSponsoredComparison([FromBody]SponsoredComparison comparison)
        {
            uint comparisonId = 0;
            try
            {
                comparisonId = _objSponsoredRepo.SaveSponsoredComparison(comparison);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "comparison.SponsoredComparisonController.SaveSponsoredComparison");
                return InternalServerError();
            }
            return Ok(comparisonId);
        }


        /// <summary>
        /// Updates the sponsored Comparison status.
        /// </summary>
        /// <param name="comparisonId">The comparison identifier.</param>
        /// <param name="statusId">The status identifier.</param>
        /// <returns></returns>
        [HttpPost, Route("api/compare/sponsored/{comparisonId}/updatestatus/{statusId}/")]
        public IHttpActionResult UpdateSponsoredComparisonStatus(uint comparisonId, ushort statusId)
        {
            bool isSaved = false;
            try
            {
                isSaved = _objSponsoredRepo.ChangeSponsoredComparisonStatus(comparisonId, statusId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "comparison.SponsoredComparisonController.UpdateSponsoredComparisonStatus");
                return InternalServerError();
            }
            return Ok(isSaved);
        }

        /// <summary>
        /// Saves the sponsored comparisons bike rules.
        /// </summary>
        /// <param name="ruleObj">The rule object.</param>
        /// <returns></returns>
        [HttpPost, Route("api/compare/sponsored/rules/save/")]
        public IHttpActionResult SaveSponsoredComparisonsBikeRules([FromBody]VersionTargetMapping ruleObj)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objSponsoredRepo.SaveSponsoredComparisonBikeRules(ruleObj);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "comparison.SponsoredComparisonController.SaveSponsoredComparisonsBikeRules");
                return InternalServerError();
            }
            return Ok(isSuccess);
        }


        /// <summary>
        /// Deletes the sponsored comparison rules.
        /// </summary>
        /// <param name="comparisonId">The comparison identifier.</param>
        /// <returns></returns>
        [HttpPost, Route("api/compare/sponsored/{comparisonId}/delete/")]
        public IHttpActionResult DeleteSponsoredComparisonRules(uint comparisonId)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objSponsoredRepo.DeleteSponsoredComparisonBikeAllRules(comparisonId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "comparison.SponsoredComparisonController.DeleteSponsoredComparisonRules");
                return InternalServerError();
            }
            return Ok(isSuccess);
        }

        /// <summary>
        /// Deletes the sponsored comparison model rules.
        /// </summary>
        /// <param name="comparisonId">The comparison identifier.</param>
        /// <param name="sponsoredModelId">The sponsored model identifier.</param>
        /// <returns></returns>
        [HttpPost, Route("api/compare/sponsored/{comparisonId}/model/{sponsoredModelId}/rules/delete/")]
        public IHttpActionResult DeleteSponsoredComparisonModelRules(uint comparisonId, uint sponsoredModelId)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objSponsoredRepo.DeleteSponsoredComparisonBikeSponsoredModelRules(comparisonId, sponsoredModelId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "comparison.SponsoredComparisonController.DeleteSponsoredComparisonModelRules");
                return InternalServerError();
            }
            return Ok(isSuccess);
        }

        /// <summary>
        /// Deletes the sponsored comparison model rules.
        /// </summary>
        /// <param name="comparisonId">The comparison identifier.</param>
        /// <param name="sponsoredModelId">The sponsored model identifier.</param>
        /// <returns></returns>
        [HttpPost, Route("api/compare/sponsored/{comparisonId}/targetversion/{targetversionId}/rules/delete/")]
        public IHttpActionResult DeleteTargetComparisonModelRules(uint comparisonId, uint targetversionId)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objSponsoredRepo.DeleteSponsoredComparisonBikeTargetVersionRules(comparisonId, targetversionId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Camparison.SponsoredComparisonController.DeleteTargetComparisonModelRules");
                return InternalServerError();
            }
            return Ok(isSuccess);
        }
    }
}
