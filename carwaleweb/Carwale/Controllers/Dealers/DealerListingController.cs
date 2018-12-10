using Carwale.BL.Dealers;
using AEPLCore.Cache;
using Carwale.Cache.Dealers;
using Carwale.DAL.Dealers;
using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using Carwale.Entity.ViewModels;
using System.Web;
using System.Web.Mvc;
using Carwale.Notifications;
using System.Collections.Specialized;
using MobileWeb.Common;
using Newtonsoft.Json;
using System.Web.SessionState;
using Carwale.UI.Common;
using Carwale.Utility;
using Carwale.UI.Filters;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;
using Carwale.DTOs.Geolocation;
using AutoMapper;

namespace Carwale.UI.Controllers.Dealers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class DealerListingController : Controller
    {
        private readonly IUnityContainer _container;
        private readonly INewCarDealers _newCarDealerList;
        private readonly ICarMakes _carMakes;
		private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepository;

		public DealerListingController(IUnityContainer container, INewCarDealers newCarDealerList, ICarMakes carMakes, IGeoCitiesCacheRepository geoCitiesCacheRepository)
        {
            _container = container;
            _newCarDealerList = newCarDealerList;
            _carMakes = carMakes;
			_geoCitiesCacheRepository = geoCitiesCacheRepository;
		}

        [DeviceDetectionFilter]
        //[Route("{makeName}-dealer-showrooms/{cityName}-{cityId}")]
        public ActionResult GetDealerList(string makeName, int cityId, string cityName)
        {
            int makeId = 0;
            DealerListModel DealerCollectionModel = new DealerListModel();

            try
            {
                NameValueCollection Qs = Request.QueryString;
                if (Qs["makeId"] != null && Carwale.UI.Common.CommonOpn.CheckId(Qs["makeId"]) == true)
                    Int32.TryParse(Qs["makeId"], out makeId);

                DealerCollectionModel.Dealers = _newCarDealerList.GetDealersList(-1, Convert.ToInt32(cityId), Convert.ToInt32(makeId));
				DealerCollectionModel.makeId = makeId;
				DealerCollectionModel.cityId = cityId;
				var cityDetails = Mapper.Map<CitiesDTO>(_geoCitiesCacheRepository.GetCityDetailsById(cityId));
				DealerCollectionModel.CityName = (cityDetails!=null)?cityDetails.CityName??string.Empty : string.Empty;
				DealerCollectionModel.CityMaskingName = (cityDetails != null) ? cityDetails.CityMaskingName??string.Empty : string.Empty;

				DealerCollectionModel.mapData = JsonConvert.SerializeObject(DealerCollectionModel.Dealers.NewCarDealers.Select(item => new { isPremium = item.IsPremium, latitude = item.Latitude, longitude = item.Longitude, name = item.Name, address = item.Address, mobileNo = item.MobileNo, dealerId = item.DealerId, cityName = item.CityName, state = item.State, pincode = item.PinCode }));
                DealerCollectionModel.BreadcrumbEntitylist = BindBreadCrumb(DealerCollectionModel.Dealers);
                if (DealerCollectionModel.Dealers.NewCarDealers.Count < 1)//get MakeName and CityName if no results in api method
                {
                    return Redirect("/new/locatenewcardealers.aspx");
                }
                ViewBag.subHeading = DealerCollectionModel.Dealers.MakeName + " has " + DealerCollectionModel.Dealers.NewCarDealers.Count + " authorized dealer outlet" + (DealerCollectionModel.Dealers.NewCarDealers.Count > 1 ? "s" : "") + " / showroom" + (DealerCollectionModel.Dealers.NewCarDealers.Count > 1 ? "s" : "") + " in " + DealerCollectionModel.Dealers.CityName;
                ViewBag.UserIP = UserTracker.GetUserIp();
            }

            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealerListingController.DealerList()\n Exception : " + ex.Message);
                objErr.LogException();
            }

            return View("~/views/dealers/dealerList.cshtml", DealerCollectionModel);
        }
        private List<Carwale.Entity.BreadcrumbEntity> BindBreadCrumb(NewCarDealerEntiy newCarDealerEntity)
        {
            string makeName = !string.IsNullOrWhiteSpace(newCarDealerEntity.MakeName) ? UrlRewrite.FormatSpecial(newCarDealerEntity.MakeName) : string.Empty;
            List <Carwale.Entity.BreadcrumbEntity> _BreadcrumbEntitylist = new List<Carwale.Entity.BreadcrumbEntity>();
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = "New Cars", Link = "/new/", Text = "New Cars" });
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = "New Car Dealers", Link = "/new/locatenewcardealers.aspx", Text = "New Car Dealers" });
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = string.Format("{0} Dealers", newCarDealerEntity.MakeName),Link = string.Format("/new/{0}-dealers/", makeName), Text=string.Format("{0} Dealers", newCarDealerEntity.MakeName) });
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Text = string.Format("{0} Dealers in {1}", newCarDealerEntity.MakeName, newCarDealerEntity.CityName) });
            return _BreadcrumbEntitylist;
        }
    }
}
