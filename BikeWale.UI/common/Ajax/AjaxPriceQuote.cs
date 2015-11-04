using System;
using System.Web;
using System.Data;
using AjaxPro;
using Bikewale.Common;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.Location;
using Bikewale.DAL.Location;
using Bikewale.Entities.Location;
using System.Collections.Generic;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.DAL.BikeBooking;

namespace Bikewale.Ajax
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 8/3/2012
    /// This class is used for price quote related ajax functions
    /// </summary>
    public class AjaxPriceQuote
    {
        /// <summary>
        ///     Written By : Ashish G. Kamble on 8/3/2012
        ///     This method will return city id and city names list in the json string format
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetPriceQuoteCities(string modelId)
        {
            string jsonCities = string.Empty;
            DataTable dt = null;

            StateCity objCities = new StateCity();

            dt = objCities.GetPriceQuoteCities(modelId);

            if (dt != null && dt.Rows.Count > 0)
            {
                jsonCities = JSON.GetJSONString(dt);
            }

            return jsonCities;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 21 July 2015
        /// Summary : To get CityId and CityName List
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetPriceQuoteCitiesNew(uint modelId)
        {
            string jsonCities = string.Empty;
            List<CityEntityBase> objCities = null;
            try
            {
                using(IUnityContainer container =new UnityContainer())
                {
                    container.RegisterType<ICity, CityRepository>();
                    ICity cityRepository = container.Resolve<ICity>();

                    objCities = cityRepository.GetPriceQuoteCities(modelId);

                    if (objCities != null)
                    {
                        jsonCities = JavaScriptSerializer.Serialize(objCities);
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "AjaxPriceQuote.GetPriceQuoteCitiesNew");
                objErr.SendMail();
            }
            return jsonCities;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 21 July 2015
        /// Summary : To get AreaId and AreaName listGetPriceQuoteCitiesNew
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetPriceQuoteArea(uint modelId,uint cityId)
        {
            string jsonCities = string.Empty;
            List<AreaEntityBase> objArea = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote dealerRepository = container.Resolve<IDealerPriceQuote>();

                    objArea = dealerRepository.GetAreaList(modelId, cityId);

                    if (objArea != null)
                    {
                        jsonCities = JavaScriptSerializer.Serialize(objArea);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "AjaxPriceQuote.GetPriceQuoteArea");
                objErr.SendMail();
            }
            return jsonCities;
        }
    }
}