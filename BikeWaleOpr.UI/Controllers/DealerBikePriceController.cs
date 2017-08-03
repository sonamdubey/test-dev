using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    public class DealerBikePriceController : Controller
    {
        private readonly ILocation location = null;
        private readonly IDealerPriceQuote dealerPriceQuote = null;
        public DealerBikePriceController(ILocation locationObject, IDealerPriceQuote dealerPriceQuoteObject)
        {
            location = locationObject;
            dealerPriceQuote = dealerPriceQuoteObject;
        }
        // GET: DealerBikePrice
        public ActionResult Index()
        {
            return View();
        }
    }
}