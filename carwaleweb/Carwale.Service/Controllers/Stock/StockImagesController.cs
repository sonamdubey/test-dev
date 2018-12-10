using Carwale.BL.Stock;
using Carwale.DTOs.Stock.Images;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock;
using Carwale.Interfaces;
using Carwale.Interfaces.Stock;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using Carwale.Utility;
using Carwale.Utility.Classified;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using static Carwale.Entity.Stock.StockImageStatus;

namespace Carwale.Service.Controllers.Photo
{
    public class StockImagesController : ApiController
    {
        private readonly IStockImageRepository _stockImageRepo;
        private readonly IStockBL _stockBL;
        private readonly string _usedCarsStockImageQueueName = ConfigurationManager.AppSettings["UsedCarStockImageQueue"].ToString();
        private static readonly string _imageHostUrl = ConfigurationManager.AppSettings["CDNHostURL"].ToString();
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public StockImagesController(IStockImageRepository stockImageRepo, IStockBL stockBL)
        {
            _stockImageRepo = stockImageRepo;
            _stockBL = stockBL;
        }

        [Route("api/stocks/{stockId}/images")]
        [ApiAuthorization, LogApi]
        public IHttpActionResult Post(int stockId, [FromBody]StockImageList stockImages)
        {
            try
            {
                if (stockImages == null)
                {
                    ModelState.AddModelError("stockImages", "stockImages should not be empty.");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                PushToStockImagesQueue(stockId, stockImages);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError(ex);
            }
        }

        [Route("api/stocks/{stockId}/images")]
        [ApiAuthorization, LogApi]
        public IHttpActionResult Put(int stockId, [FromBody]StockImageList stockImages)
        {
            try
            {
                if (stockImages == null)
                {
                    ModelState.AddModelError("stockImages", "stockImages should not be empty.");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                PushToStockImagesQueue(stockId, stockImages);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError(ex);
            }
        }

        [Route("api/stocks/{stockId}/images")]
        [ApiAuthorization, LogApi]
        public IHttpActionResult Delete(int stockId, [FromBody]StockImageDelete stockImages)
        {
            try
            {
                if (stockImages == null)
                {
                    ModelState.AddModelError("stockImages", "stockImages should not be empty.");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var stockImgList = new StockImageList()
                {
                    StockId = stockId,
                    SellerType = stockImages.SellerType,
                    SourceId = stockImages.SourceId,
                    StockImages = stockImages.Ids.Select(x => new StockImage() { Id = x }).ToList()
                };

                PushToStockImagesQueue(stockId, stockImgList);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError(ex);
            }
        }

        [Route("api/stocks/{profileId}/images")]
        [AuthenticateBasic]
        public IHttpActionResult Get(string profileId)
        {
            try
            {
                int inquiryId = StockBL.GetInquiryId(profileId);
                if (inquiryId <= 0)
                {
                    return BadRequest("ProfileId is not Valid.");
                }

                int requestSource = Request.Headers.GetValueFromHttpHeader<int>("SourceId");
                CarDetailsEntity carDetails = _stockBL.GetStock(profileId);
                if (carDetails != null && carDetails.ImageList != null && carDetails.ImageList.ImageUrlAttributes != null && carDetails.ImageList.ImageUrlAttributes.Count > 0)
                {
                    switch ((Platform)requestSource)
                    {
                        case Platform.CarwaleAndroid:
                        case Platform.CarwaleiOS:
                            {
                                ImageGalleryApp images = new ImageGalleryApp()
                                {
                                    HostUrl = _imageHostUrl,
                                    OriginalImgPaths = carDetails.ImageList.ImageUrlAttributes.Select(url => url.OriginalImgPath).ToList()
                                };
                                if (carDetails.BasicCarInfo != null)
                                {
                                    images.IsChatAvailable = carDetails.BasicCarInfo.IsChatAvailable;
                                }
                                return Json(images, _serializerSettings);
                            }
                        default:
                            return BadRequest("Invalid SourceId.");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }
        /// <summary>
        /// Returns json object of images segregated into main image and other images
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns>json</returns>
        /// <optional parameters>image size(eg.1280x720) </optional>
        [Route("api/v1/stocks/{profileId}/images/"), HandleException]
        public IHttpActionResult GetImages(string profileId)
        {   
            if (!StockValidations.IsProfileIdValid(profileId))
            {
                return BadRequest("ProfileId is not Valid.");
            }
            var carDetails = _stockBL.GetStock(profileId);
            if (carDetails?.BasicCarInfo == null || carDetails.BasicCarInfo.PhotoCount == 0)
            {
                return NotFound();
            }
            string imageSize = HttpUtility.ParseQueryString(Request.RequestUri.Query)["imageSize"];
            if (string.IsNullOrWhiteSpace(imageSize))
            {
                imageSize = ImageSizes._300x225;
            }
            string mainImage = "";
            List<string> otherImages = new List<string>();
            foreach (var image in carDetails.ImageList.ImageUrlAttributes)
            {
                if (image.IsMain)
                {
                    mainImage = ImageSizes.CreateImageUrl(_imageHostUrl, imageSize, image.OriginalImgPath);
                }
                else
                {
                    otherImages.Add(ImageSizes.CreateImageUrl(_imageHostUrl, imageSize, image.OriginalImgPath));
                }
            }
            return Json(new { mainImage, otherImages }, _serializerSettings);
        }

        [Route("api/stocks/images/uploadstatus/{imageIdList}")]
        public IHttpActionResult GetProcessedImages(string imageIdList)
        {
            try
            {
                List<CarPhotos> carPhotos = _stockImageRepo.GetProcessedImage(imageIdList);
                return Ok(carPhotos);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError(ex);
            }
        }

        [Route("api/stocks/{stockId}/images/uploadstatus")]
        [HttpPut, ApiAuthorization, LogApi]
        public IHttpActionResult ChangeStatus(int stockId, [FromBody]StockImageStatusList statusList)
        {
            List<StatusResposne> resp = new List<StatusResposne>();
            try
            {
                if (statusList == null)
                {
                    ModelState.AddModelError("stockImageStatus", "object is null.");
                    return BadRequest(ModelState);
                }

                var validation = new StockImageStatusListValidator().Validate(statusList);
                if (!validation.IsValid && validation.Errors != null)
                {
                    FluentValidationError.LogValidationErrorsAsync(validation.Errors, "Image Upload Status Errors");
                    foreach (var error in validation.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return BadRequest(ModelState);
                }
                foreach (var status in statusList.Status)
                {
                    var updated = _stockImageRepo.UpdateStatus(status, stockId, statusList.SellerType);
                    resp.Add(new StatusResposne
                    {
                        Id = status.Id,
                        Action = status.Action,
                        IsSuccessful = updated,

                    });
                }
                _stockImageRepo.setMainImage(stockId, (int)statusList.SellerType);
                return Content((System.Net.HttpStatusCode)207, resp, new JsonMediaTypeFormatter { SerializerSettings = _serializerSettings });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError(ex);
            }
        }

        private void PushToStockImagesQueue(int stockId, StockImageList stockImages)
        {
            var stockImageWrapper = new StockImagesWrapper()
            {
                OperationType = Request.Method.ToString(),
                StockImages = stockImages
            };
            stockImageWrapper.StockImages.StockId = stockId;
            stockImageWrapper.StockImages.SellerType = stockImages.SellerType;
            stockImageWrapper.StockImages.SourceId = stockImages.SourceId;

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("StockImageApiWrapper", JsonConvert.SerializeObject(stockImageWrapper));
            RabbitMqPublish usedImageApiQueue = new RabbitMqPublish();
            usedImageApiQueue.PublishToQueue(_usedCarsStockImageQueueName, nvc);
        }
    }

    internal class StatusResposne
    {
        public int? Id { get; set; }
        public ActionType Action { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
