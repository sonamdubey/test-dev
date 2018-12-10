using Carwale.BL.Stock;
using Carwale.Entity.Stock;
using Carwale.Interfaces.Stock;
using Carwale.Service.Filters;
using System.Web.Http;

namespace Carwale.Service.Controllers.Stock
{
    public class StockPackagesController : ApiController
    {
        private readonly IStockPackagesRepository _stockPackagesRepository;

        public StockPackagesController(IStockPackagesRepository stockPackagesRepository)
        {
            _stockPackagesRepository = stockPackagesRepository;
        }

        [Route("api/stocks/{profileId}/packages/"), HandleException]
        public IHttpActionResult Get(string profileId)
        {
            int inquiryId = StockBL.GetInquiryId(profileId);
            if (inquiryId <= 0)
            {
                return BadRequest("ProfileId is not Valid.");
            }

            return Ok(_stockPackagesRepository.GetBoostPackage(StockBL.GetInquiryId(profileId)));
        }

        [Route("api/stocks/{profileId}/packages/"), LogApi, HandleException, ApiAuthorization, ValidateModel("stockPackages")]
        public IHttpActionResult Put(string profileId, StockBoostPackages stockPackages)
        {
            bool isDealer;
            int inquiryId = StockBL.GetInquiryId(profileId, out isDealer);
            if (inquiryId <= 0 || !isDealer)
            {
                return BadRequest("ProfileId is not Valid.");
            }

            BoostPackResponseStatus status = _stockPackagesRepository.UpdateBoostPackage(inquiryId, isDealer, stockPackages.BoostPackageId);

            switch (status)
            {
                case BoostPackResponseStatus.StockNotLive:
                    return BadRequest($"Stock {profileId} is invalid or not live");
                case BoostPackResponseStatus.InvalidPackageId:
                    return BadRequest($"Invalid PackageId {stockPackages.BoostPackageId}");
                case BoostPackResponseStatus.DuplicatePackage:
                    return BadRequest($"Same BoostPackageId { stockPackages.BoostPackageId } already exists for this stock.");
                case BoostPackResponseStatus.ServerError:
                    return InternalServerError();
                default:
                    return Ok("Boost package successfully added");
            }
        }
    }
}
