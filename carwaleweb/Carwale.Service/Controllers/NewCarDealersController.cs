using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web;
using System.Net.Http;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using Carwale.Interfaces.Dealers;
using Carwale.DAL.Dealers;
using Carwale.Entity.Dealers;
using Carwale.Cache.Core;
using Carwale.Interfaces;
using Carwale.BL.Dealers;
using Newtonsoft.Json;
using System.Collections.Specialized;
using Carwale.Service.Filters;
using Carwale.Utility;
using Carwale.Service.APIServices;
using Carwale.Interfaces.Notifications;
using Carwale.BL.Notifications;
using Carwale.DAL.Notifications;
using Carwale.Entity;

namespace Carwale.Service.Controllers
{
    public class NewCarDealersController : ApiController
    {
        /// <summary>
        /// Populates the list of dealers based on cityid and makeid passed in querystring
        /// Written By : Supriya on 29/5/2014
        /// </summary>
        /// <returns></returns>

        [AthunticateBasic]
        public HttpResponseMessage GetNewCarDealers()
        {
            var response = new HttpResponseMessage();
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                if (!RegExValidations.IsPositiveNumber(nvc["cityId"].ToString()) || !RegExValidations.IsPositiveNumber(nvc["makeId"].ToString()))
                {
                    response.Content = new StringContent("Bad Request");
                    return response;
                }

                int cityId = Convert.ToInt32(nvc["cityId"]);
                int makeId = Convert.ToInt32(nvc["makeId"]);

                IUnityContainer container = new UnityContainer();
                container.RegisterType<INewCarDealersRepository, NewCarDealersRepository>()
                         .RegisterType<ICacheProvider, MemcacheManager>()
                         .RegisterType<INewCarDealers, NewCarDealersCache>();

                INewCarDealers newCarDealerList = container.Resolve<INewCarDealers>();

                response.Content = new StringContent(JsonConvert.SerializeObject(newCarDealerList.GetDealersList(cityId, makeId)));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersController.GetNewCarDealers()");
                objErr.LogException();
            }
            return response;
        }

        /// <summary>
        /// Populates the list of makes based on cityId paased in querystring
        /// Written By : Supriya on 29/5/2014
        /// </summary>
        /// <returns></returns>

        [AthunticateBasic]
        public HttpResponseMessage GetMakesByCity()
        {
            var response = new HttpResponseMessage();
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                if (!RegExValidations.IsPositiveNumber(nvc["cityId"].ToString()))
                {
                    response.Content = new StringContent("Bad Request");
                    return response;
                }

                int cityId = Convert.ToInt32(nvc["cityId"]);

                IUnityContainer container = new UnityContainer();
                container.RegisterType<INewCarDealersRepository, NewCarDealersRepository>()
                         .RegisterType<ICacheProvider, MemcacheManager>()
                         .RegisterType<INewCarDealers, NewCarDealersCache>();

                INewCarDealers makeList = container.Resolve<INewCarDealers>();

                response.Content = new StringContent(JsonConvert.SerializeObject(makeList.GetMakesByCity(cityId)));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersController.GetMakesByCity()");
                objErr.LogException();
            }
            return response;
        }

        public HttpResponseMessage PostTC_DealerInquiryDetails([FromBody] DealerInquiryDetails dealerInquiryDetails)
        {
            var response = new HttpResponseMessage();
            try
            {
                var dealerInquiry = new DealerInquiryDetails()
                {
                    CityId = dealerInquiryDetails.CityId,
                    VersionId = dealerInquiryDetails.VersionId,
                    DealerId = dealerInquiryDetails.DealerId,
                    Email = dealerInquiryDetails.Email,
                    Name = dealerInquiryDetails.Name,
                    Mobile = dealerInquiryDetails.Mobile,
                    BuyTimeText = dealerInquiryDetails.BuyTimeText,
                    BuyTimeValue = dealerInquiryDetails.BuyTimeValue,
                    RequestType = dealerInquiryDetails.RequestType,
                    DealerMobile =dealerInquiryDetails.DealerMobile,
                    DealerName = dealerInquiryDetails.DealerName,
                    InquirySourceId = dealerInquiryDetails.InquirySourceId,
                    ModelName = dealerInquiryDetails.ModelName,
                    PageUrl = dealerInquiryDetails.PageUrl,
                    DealerEmail = dealerInquiryDetails.DealerEmail
                };

                IUnityContainer container = new UnityContainer();
                container.RegisterType<IAPIService<DealerInquiryDetails, APIResponseEntity>, TCApi_Inquiry<DealerInquiryDetails, APIResponseEntity>>()
                            .RegisterType<IQueueService<NameValueCollection>, RebbitMQManager<NameValueCollection>>()
                            .RegisterType<ISMSNotifications, SMSNotification>()
                            .RegisterType<ISMSRepository, SMSRepository>()
                            .RegisterType<IEmailNotifications, Email>()
                            .RegisterType<IQueueObjectProcessor<DealerInquiryDetails>, PQInquiryToDealer<DealerInquiryDetails, APIResponseEntity>>();

                var tcApi = container.Resolve<IQueueObjectProcessor<DealerInquiryDetails>>();

                tcApi.ProcessObject(dealerInquiry);

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PriceQuoteController.PostPQ()");
                objErr.LogException();
            }
            return response;
        }
    }
}
