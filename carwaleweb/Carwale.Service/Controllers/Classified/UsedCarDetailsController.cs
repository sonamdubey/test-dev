using AutoMapper;
using Carwale.DAL.Security;
using Carwale.DTOs.Classified;
using Carwale.DTOs.Classified.CarDetails;
using Carwale.Entity.Classified;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified.CarDetail;
using Carwale.Interfaces.Elastic;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Carwale.Interfaces.Stock;

namespace Carwale.Service.Controllers.Classified
{
    public class UsedCarDetailsController : ApiController
    {
        private static readonly bool _skipValidation = Convert.ToBoolean(ConfigurationManager.AppSettings["SkipValidation"]);
        private readonly ISecurityRepository<bool> _securityRepo;
        private readonly ICarDetail _carDetailBL;
        private readonly IElasticSearchManager _searchManager;
        private readonly IStockRecommendationsBL _stockRecommendationsBL;

        public UsedCarDetailsController(ICarDetail carDetail, IElasticSearchManager searchManager, IStockRecommendationsBL stockRecommendationsBL, ISecurityRepository<bool> securityRepo)
        {
            _securityRepo = securityRepo;
            _carDetailBL = carDetail;
            _searchManager = searchManager;
            _stockRecommendationsBL = stockRecommendationsBL;
        }

        [Route("api/UsedCarDetails/")]
        public HttpResponseMessage Get(HttpRequestMessage request, string car, string dc = "")
        {
            if (_skipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && _securityRepo.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
            {
                int sourceId;
                int usedCarNotificationId;
                string imeiCode;
                ReadInfoFromHeader(request, out sourceId, out usedCarNotificationId, out imeiCode);

                UsedCarDetails carDetails = _carDetailBL.GetCompleteCarDetailsMobile(car, dc, imeiCode, usedCarNotificationId, sourceId);
                if (carDetails != null)
                {
                    carDetails.alternativeCars = new List<UsedCar>();
                    List<StockBaseEntity> recommendationStock = _searchManager.SearchIndexProfileRecommendation<List<StockBaseEntity>, string>(car.ToUpper(), 6);

                    if (recommendationStock != null)
                    {
                        carDetails.alternativeCars = Mapper.Map<List<StockBaseEntity>, List<UsedCar>>(recommendationStock);
                    }
                    return Request.CreateResponse<UsedCarDetails>(HttpStatusCode.OK, carDetails);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong on the server");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
        }

        private static void ReadInfoFromHeader(HttpRequestMessage request, out int sourceId, out int usedCarNotificationId, out string imeiCode)
        {
            int appVersionId;
            usedCarNotificationId = 0;
            sourceId = 0;
            imeiCode = string.Empty;

            if (request.Headers.Contains("SourceId")
                && int.TryParse(request.Headers.GetValues("sourceID").First(), out sourceId)
                && request.Headers.Contains("appVersion")
                && int.TryParse(request.Headers.GetValues("appVersion").First(), out appVersionId)
                && IsAppSendingCorrectIMEI(sourceId, appVersionId))
            {
                if (request.Headers.Contains("IMEI"))
                {
                    imeiCode = request.Headers.GetValues("IMEI").First();
                }
                if (request.Headers.Contains("usedCarNotificationId"))
                {
                    int.TryParse(request.Headers.GetValues("usedCarNotificationId").First(), out usedCarNotificationId);
                }
            }
        }

        /// <summary>
        /// IMEI being sent correctly after android app version 69 and ios version 18
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="andNotificationAppVersionId"></param>
        /// <param name="iosNotificationAppVersionId"></param>
        /// <param name="appVersionId"></param>
        /// <returns></returns>
        private static bool IsAppSendingCorrectIMEI(int sourceId, int appVersionId)
        {
            int andNotificationAppVersionId;
            int iosNotificationAppVersionId;
            if (int.TryParse(ConfigurationManager.AppSettings["AndAppNotificationVersionId"], out andNotificationAppVersionId)
                && int.TryParse(ConfigurationManager.AppSettings["IosAppNotificationVersionId"], out iosNotificationAppVersionId))
            {
                return (sourceId == (int)Platform.CarwaleiOS && appVersionId >= iosNotificationAppVersionId) || (sourceId == (int)Platform.CarwaleAndroid && appVersionId >= andNotificationAppVersionId);
            }
            return false;
        }
    }
}
