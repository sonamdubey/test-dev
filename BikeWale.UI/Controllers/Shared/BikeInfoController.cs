using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.GenericBikes;

namespace Bikewale.Controllers.Shared
{
    public class BikeInfoController : Controller
    {
        private readonly ICacheManager cache = null;
        private readonly IGenericBikeRepository genericBike = null;
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

            return PartialView("~/views/m/shared/bikeinfo.cshtml", bikeInfo);
        }
    }
}