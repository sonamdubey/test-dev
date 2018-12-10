using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Collections;
using AjaxPro;
using Carwale.UI.Common;
using Carwale.Interfaces.CarData;
using Carwale.Service;
using Carwale.Entity.CarData;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CarwaleAjax
{
    public class AjaxCompareCars
    {


        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;
        
        /// <summary>
        /// Gets the versions of model selected by the user through drop-down and sends it back to UI via Ajax. Second parameter specifies that whether only available model required or all model including discontinued one required
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="compareOption"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string GetCarVersionHavingSpecs(int modelId, string onlyNew)
        {
            ICarVersionCacheRepository _carVersionCache = UnityBootstrapper.Resolve<ICarVersionCacheRepository>();
            List<CarVersionEntity> carVersions = _carVersionCache.GetCarVersionsByType("Compare",modelId);
            return Newtonsoft.Json.JsonConvert.SerializeObject(carVersions);
        }


    }
}