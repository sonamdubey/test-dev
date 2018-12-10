using Carwale.Entity.ImageUpload;
using Carwale.Entity.Stock;
using Carwale.Interfaces;
using Carwale.Interfaces.ImageUpload;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using Carwale.Utility;
using System;
using System.Configuration;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Net.Http;
using System.Net;

namespace Carwale.Service.Controllers.Classified
{
    public class ImageUploadController : ApiController
    {
        private readonly IStockImageRepository _stockImageRepo;
        private readonly IImageBL _imageBL;
        private const string _hmacSHAKey = "awsshakeyforimageupload";

        public ImageUploadController(IStockImageRepository stockImageRepo, IImageBL imageBL)
        {
            _stockImageRepo = stockImageRepo;
            _imageBL = imageBL;
        }

        [HttpPost, Route("api/stocks/images/validate/")]
        public IHttpActionResult Get()
        {
            return Ok(true);
        }

        [Route("api/stocks/images/token/"), HttpPost, ResponseType(typeof(ImageTokenDTO))]
        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,https://opr.carwale.com,https://oprst.carwale.com",headers: "*", methods: "POST")]
        public IHttpActionResult CreateToken([FromBody]TokenInput input,string encryptedId = "")
        {
            try
            {
                int inquiryId = 0;
                inquiryId = GetInquiryId(encryptedId);

                if (inquiryId <= 0 || input == null || string.IsNullOrWhiteSpace(input.Extension) || input.ImageType <= 0)
                {
                    return BadRequest();
                }
                
                ImageTokenDTO token = _imageBL.GenerateImageUploadToken(inquiryId, input.Extension, input.ImageType);
                if (token != null && token.Status)
                {
                    return Ok(token);
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [HttpPost, Route("api/stockimages/")]
        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,https://opr.carwale.com,https://oprst.carwale.com",headers: "*", methods: "POST")]
        public IHttpActionResult Post([FromBody]ImageTokenDTO dto,string encryptedId = "",bool isApproved = false)
        {
            try
            {
                int inquiryId = 0;
                inquiryId = GetInquiryId(encryptedId);

                if (inquiryId <= 0 || dto == null || string.IsNullOrWhiteSpace(dto.OriginalImgPath) || string.IsNullOrWhiteSpace(dto.Key))
                {
                    return BadRequest();
                }
                //get hash id from the original path
                string hash = BitConverter.ToString(_imageBL.HmacSHA256(dto.OriginalImgPath, Encoding.UTF8.GetBytes((_hmacSHAKey).ToCharArray())));
                if (hash != dto.Key)
                {
                    return BadRequest();
                }

                CarPhotos photo = new CarPhotos()
                {
                    StockId = inquiryId,
                    SellerType = 2, //Individual
                    HostUrl = ConfigurationManager.AppSettings["CDNHostURL"],
                    OriginalImgPath = $"/{dto.OriginalImgPath}",
                    ImageType = dto.ImageType,
                    IsMain = dto.ImageType == 1, //Front view
                    IsReplicated = true,
                    IsApproved = isApproved
                };

                int photoId = _stockImageRepo.Create(photo);
                if (photoId > 0)
                {
                    _imageBL.PushToIPCQueue(photoId, dto.OriginalImgPath);
                    return Ok(photoId);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [HttpDelete, Route("api/stocks/images/{photoId}/"), HandleException]
        public IHttpActionResult RemoveCarPhotos(int photoId,string encryptedId = "")
        {
            int inquiryId = 0;
            inquiryId = GetInquiryId(encryptedId);
           
            if (inquiryId <= 0 || photoId <= 0)
            {
                return BadRequest();
            }
            CarPhotos carPhotos = new CarPhotos()
            {
                TC_CarPhotoId = photoId,
                StockId = inquiryId,
                SellerType = 2
            };
            if (!_stockImageRepo.Delete(carPhotos))
            {
                return Json("Photo could not be found");
            }
            return Ok();
        }
        
        private int GetInquiryId(string encryptedId)
        {
            int inquiryId = 0;
            if(string.IsNullOrWhiteSpace(encryptedId))
            {
                inquiryId = GetInquiryIdFromCookie();
            }
            else
            {
                Int32.TryParse(CarwaleSecurity.Decrypt(encryptedId, true),out inquiryId);
            }
            return inquiryId;
        }
        private int GetInquiryIdFromCookie()
        {
            return Convert.ToInt32(CookieManager.GetEncryptedCookie("SellInquiry"));
        }
    }
}