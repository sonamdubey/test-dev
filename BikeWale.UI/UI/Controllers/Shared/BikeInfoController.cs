using Bikewale.Interfaces.BikeData;
using System.Web.Mvc;

namespace Bikewale.Controllers.Shared
{
    public class BikeInfoController : Controller
    {
        private readonly IBikeInfo info = null;

        //protected GenericBikeInfo bikeInfo { get; set; }
        //protected string bikeUrl = string.Empty, bikeName = string.Empty;
        //protected PQSourceEnum pqSource;

        public BikeInfoController(IBikeInfo _info)
        {
            info = _info;
        }

        public ActionResult Index()
        {
            return PartialView();
        }

        // GET: BikeInfo
        [Route("m/shared/bikeinfo/model/{modelId}/")]
        public ActionResult Index(uint modelId)
        {
            Bikewale.Models.Shared.BikeInfo bikeInfo = null;

            if (modelId > 0)
            {
                bikeInfo = info.GetBikeInfo(modelId);
            }

            return PartialView("~/views/bikemodels/_BikeInfoCard_AMP_Mobile.cshtml", bikeInfo);
        }
    }
}