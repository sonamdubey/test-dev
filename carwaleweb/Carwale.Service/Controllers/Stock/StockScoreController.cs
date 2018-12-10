using Carwale.Entity.Stock;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Stock;
using Carwale.Service.Filters;
using Carwale.Utility.Classified;
using System.Web.Http;

namespace Carwale.Service.Controllers.Stock
{
    public class StockScoreController : ApiController
    {
        private readonly IStockScoreRepository _stockScoreRepository;
        private readonly IStockBL _stockBL;
        public StockScoreController(IStockScoreRepository stockScoreRepository, IStockRepository stockRepository, IStockBL stockBL)
        {
            _stockScoreRepository = stockScoreRepository;
            _stockBL = stockBL;
        }

        [HttpPut, Route("api/stocks/{profileId}/sortscore/"), ValidateModel("stockSortScore"), ApiAuthorization, HandleException, LogApi]
        public IHttpActionResult UpdateStockScore(string profileId, StockSortScore stockSortScore)
        {
            if (!StockValidations.IsProfileIdValid(profileId))
            {
                ModelState.AddModelError("profileId", "Invalid profileId");
                return BadRequest(ModelState);
            }

            bool isStockUpdated = _stockScoreRepository.UpdateStockScore(profileId, stockSortScore);
            if(!isStockUpdated)
            {
                ModelState.AddModelError("profileId", "Stock is not live");
                return BadRequest(ModelState);
            }
            _stockBL.RefreshESStock(profileId);
            return Ok();
        }
    }
}
