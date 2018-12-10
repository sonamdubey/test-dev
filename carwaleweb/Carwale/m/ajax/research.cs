using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using AjaxPro;
using MobileWeb.Common;
using System.Configuration;
using MobileWeb.DataLayer;
using Carwale.Interfaces.CarData;
using Carwale.Service;
using Carwale.Entity.CarData;
using System.Collections.Generic;

namespace MobileWeb.Ajax
{
    public class Research
    {                
        /*
         * Author : Supriya 
         * Created Date : 7/5/2013
         * Desc : Method to push Test Drive request to CRM
         * Modified by Supriya on 19/2/2014 to fetch maskingname
         */
        [AjaxPro.AjaxMethod()]
        public string GetNewCarModels(string makeId,string type)
        {
            string retVal = "";
           
            ICarModelCacheRepository _carmodelCache = UnityBootstrapper.Resolve<ICarModelCacheRepository>();
            List<CarModelEntityBase> carModels = _carmodelCache.GetCarModelsByType("new",Convert.ToInt32(makeId));
            try
            {
                for (int i = 0; i < carModels.Count; i++)
                {
                    retVal += "<li><a onclick='ShowVersion(this);' id = '" + carModels[i].ModelId + "' type = '" + type + "' MaskingName ='" + carModels[i].MaskingName + "' >" + carModels[i].ModelName + "</a></li>";
                }
            }
            catch (Exception err)
            {
                retVal = "";
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return retVal;
        }		
    }
}