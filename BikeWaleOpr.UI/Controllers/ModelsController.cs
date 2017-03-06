using BikewaleOpr.Interface.BikeData;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    public class ModelsController : Controller
    {
        private readonly IBikeMakes _makesRepo;

        public ModelsController(IBikeMakes makesRepo)
        {
            _makesRepo = makesRepo;
        }

        // GET: Models
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UsedModelImageUpload()
        {
            ViewBag.MakeList = _makesRepo.GetMakes("NEW");
            return View();
        }
    }
}