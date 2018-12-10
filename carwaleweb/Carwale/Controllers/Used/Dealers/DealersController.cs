using Carwale.Interfaces.Dealers.Used;
using System.Web.Mvc;
using Carwale.Utility;
using Carwale.UI.ViewModels.Used.Dealers;
using Carwale.Entity.Classified;
using System.Collections.Generic;
using AutoMapper;
using Carwale.Entity.Classified.Search;
using System.Linq;
using Carwale.Service.Filters;
using System.Net;

namespace Carwale.UI.Controllers.Used
{
    public class DealersController : Controller
    {
        private readonly IUsedDealerStocksBL _usedDealerStocksBL;
        public DealersController(IUsedDealerStocksBL usedDealerStocksBL)
        {
            _usedDealerStocksBL = usedDealerStocksBL;
        }
        [HttpGet, Route("used/dealers/{dealerName}"), HandleException]
        public ActionResult Index(string dealerName, int dealerId)
        {
            if(string.IsNullOrWhiteSpace(dealerName) || dealerId < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dealerStocks = _usedDealerStocksBL.GetDealerStocksForVirtualPage(dealerId);
            if(!dealerStocks.IsNotNullOrEmpty())
            {
                return HttpNotFound();
            }
            var viewModel = GetViewModel(dealerStocks);
            return View("~/Views/Used/VirtualPage.cshtml", viewModel);
        }

        private static VirtualPageViewModel GetViewModel(IEnumerable<StockBaseEntity> stocks)
        {
            string dealerName = stocks.FirstOrDefault()?.SellerName;
            var viewModel = new VirtualPageViewModel
            {
                Results = Mapper.Map<IEnumerable<StockBaseEntity>, IList<StockBaseData>>(stocks),
                DealerName = dealerName,
                DealerLogo = stocks.FirstOrDefault()?.CertProgLogoUrl,
                Title = $"{ dealerName }{(!string.IsNullOrWhiteSpace(stocks.FirstOrDefault()?.CityName) ? $", {stocks.First().CityName}" : string.Empty)}"
            };
            foreach(var stock in viewModel.Results)
            {
                stock.Price = Format.FormatFullPrice(stock.Price);
                stock.Km = $"{Format.Numeric(stock.Km)} km";
            }
            return viewModel;
        }
    }
}