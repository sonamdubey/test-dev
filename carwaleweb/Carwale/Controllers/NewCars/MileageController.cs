using Carwale.BL.CMS;
using Carwale.BL.NewCars;
using Carwale.DTOs.CarData;
using Carwale.DTOs.NewCars;
using Carwale.Entity;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.NewCars
{
    public class MileageController : Controller
    {
        private readonly IUnityContainer _unityContainer;
        private readonly ICarModels _carModelsBl;

        public MileageController(IUnityContainer container)
        {
            _unityContainer = container;
            _carModelsBl = _unityContainer.Resolve<ICarModels>();
        }

        public ActionResult Index()
        {
            Response.AddHeader("Vary", "User-Agent");
            CarDataAdapterInputs modelInput = null;
            try
            {
                string modelMaskingName = Request["model"] != null ? Request.QueryString["model"] : string.Empty;
				string makeMaskingName = Request["make"] != null ? Request.QueryString["make"] : string.Empty;
				string queryModelid = Request["modelid"] != null ? Request.QueryString["modelid"] : string.Empty;

                ModelMaskingValidationEntity modelinfo = _carModelsBl.FetchModelIdFromMaskingName(modelMaskingName, queryModelid, makeMaskingName);

				if (modelinfo.Status == CarStatus.Discontinued)
				{
				    string url = modelinfo.IsRedirect ? modelinfo.RedirectUrl : ManageCarUrl.CreateModelUrl(makeMaskingName, modelMaskingName);
					return RedirectPermanent(url);
				}
                else if (modelinfo.IsRedirect)
                {
                    return RedirectPermanent(modelinfo.RedirectUrl+"mileage/");
                }
                else if (!modelinfo.IsValid)
                {
                    return HttpNotFound();
                }

                modelInput = GetModelInput(modelinfo.ModelId);
                MileagePageDTO dto = null;
                IServiceAdapterV2 mileageAdapter = _unityContainer.Resolve<IServiceAdapterV2>("Mileage");
                dto = mileageAdapter.Get<MileagePageDTO, CarDataAdapterInputs>(modelInput);                

                if (dto == null)
                {
                    return new HttpNotFoundResult();
                }
                SetPageProperties(modelInput, dto);
                return View(modelInput.IsMobile ? "~/Views/m/CarData/Mileage.cshtml" : "~/Views/NewCar/Mileage.cshtml", dto);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "MileageController.Index()\n Exception : " + ex.Message);
            }
            return new EmptyResult();
        }
        private void SetPageProperties(CarDataAdapterInputs modelInput, MileagePageDTO dto)
        {
            ViewBag.IsCityPage = false;
            ViewBag.IsVersionPage = false;
            ViewBag.IsMobile = modelInput.IsMobile;
            ViewBag.MakeName = dto.ModelDetails.MakeName;
            ViewBag.CityName = modelInput.CustLocation.CityName;
            ViewBag.CityZone = modelInput.CustLocation.ZoneId <= 0 ? modelInput.CustLocation.CityName : modelInput.CustLocation.ZoneName;
            ViewBag.CityId = modelInput.CustLocation.CityId;
        }
        private CarDataAdapterInputs GetModelInput(int modelId)
        {
            try
            {
                var modelInput = new CarDataAdapterInputs()
                {
                    CustLocation = new Location()
                    {
                        CityId = CookiesCustomers.MasterCityId,
                        CityName = CookiesCustomers.MasterCity,
                        ZoneId = CookiesCustomers.MasterZoneId,
                        AreaId = CookiesCustomers.MasterAreaId,
                        ZoneName = CookiesCustomers.MasterZone,
                    }
                };
                
                modelInput.ModelDetails = new CarEntity() { ModelId = modelId };
                modelInput.IsMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
				modelInput.CwcCookie = UserTracker.GetSessionCookie();
				return modelInput;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

    }
}
