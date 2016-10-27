using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Service.AutoMappers.UsedBikes;
using Bikewale.Service.Utilities;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;

namespace Bikewale.Service.Controllers.UsedBikes
{
    public class UsedBikeSellerController : CompressionApiController
    {
        private readonly ISellBikes _usedBikesRepo = null;

        public UsedBikeSellerController(ISellBikes UsedBikesRepo)
        {
            _usedBikesRepo = UsedBikesRepo;
        }

        [HttpPost, Route("api/used/sell/listing/"), ResponseType(typeof(SellBikeInquiryResultDTO))]
        public IHttpActionResult Post([FromBody]SellBikeAdDTO ad)
        {
            SellBikeAd adEntity = UsedBikeBuyerMapper.Convert(ad);

            if (ModelState.IsValid)
            {
                SellBikeInquiryResultEntity inquiry = _usedBikesRepo.SaveSellBikeAd(adEntity);
                SellBikeInquiryResultDTO inquiryDTO = UsedBikeBuyerMapper.Convert(inquiry);
                return Ok(inquiryDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet, Route("api/used/sell/listing/"), ResponseType(typeof(SellBikeAdDTO))]
        public IHttpActionResult Get(int inquiryId, ulong customerId)
        {
            SellBikeAd adEntity = null;
            SellBikeAdDTO adDTO = null;
            if (ModelState.IsValid)
            {
                adEntity = _usedBikesRepo.GetById(inquiryId, customerId);
                adDTO = UsedBikeBuyerMapper.Convert(adEntity);
                return Ok(adDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost, Route("api/used/sell/listing/{profileId}/")]
        public IHttpActionResult Post([FromBody]SellBikeAdDTO ad, string profileId)
        {
            SellBikeAd adEntity = UsedBikeBuyerMapper.Convert(ad);
            if (ModelState.IsValid)
            {
                _usedBikesRepo.SaveSellBikeAd(adEntity);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost, Route("api/used/sell/listing/otherinfo/")]
        public IHttpActionResult Post(SellBikeAdOtherInformationDTO otherInfo, int inquiryId, ulong customerId)
        {
            SellBikeAdOtherInformation sellOtherEntity = null;
            bool result = false;
            if (ModelState.IsValid)
            {
                sellOtherEntity = UsedBikeBuyerMapper.Convert(otherInfo);
                result = _usedBikesRepo.UpdateOtherInformation(sellOtherEntity, inquiryId, customerId);
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost, ResponseType(typeof(bool)), Route("api/used/sell/listing/{profileId}/verifymobile/")]
        public IHttpActionResult Post(SellerDTO seller)
        {
            SellerEntity sellerEntity = null;
            bool result;
            if (ModelState.IsValid)
            {
                sellerEntity = UsedBikeBuyerMapper.Convert(seller);
                result = _usedBikesRepo.VerifyMobile(sellerEntity);
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
