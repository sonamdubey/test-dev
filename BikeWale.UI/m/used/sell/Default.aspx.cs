using Bikewale.BAL.Customer;
using Bikewale.BAL.MobileVerification;
using Bikewale.BAL.Pager;
using Bikewale.BAL.UsedBikes;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.CacheHelper.BikeData;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Customer;
using Bikewale.DAL.Location;
using Bikewale.DAL.MobileVerification;
using Bikewale.DAL.Used;
using Bikewale.DTO.BikeBooking.Make;
using Bikewale.DTO.BikeBooking.Model;
using Bikewale.DTO.BikeBooking.Version;
using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.Used;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.Mobile.Used.Sell
{
    /// <summary>
    /// Created By : Sajal Gupta on 01/12/2016
    /// Description : Class for sell bike mobile page.
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected IEnumerable<Bikewale.Entities.BikeData.BikeMakeEntityBase> objMakeList = null;
        private IBikeMakesCacheRepository _makesRepository;
        protected IEnumerable<CityEntityBase> objCityList = null;
        protected string userEmail = null, userName = null, userId = null;
        protected bool isEdit = false;
        protected int inquiryId = 0;
        protected bool isAuthorized = true;
        protected SellBikeAd inquiryDetailsObject = null;
        protected SellBikeAdDTO inquiryDTO;
        protected string cookieCityName;
        protected uint cookieCityId;
        GlobalCityAreaEntity currentCityArea;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Created By : Sajal Gupta on 29/11/2016
        /// Description : Function to be called on page load.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            BindUserId();
            BindMakes();
            BindCities();
            CheckIsEdit();
        }

        /// <summary>
        /// Created By : Sajal Gupta on 20/10/2016
        /// Description : Function to bind userId.
        /// </summary>
        protected void BindUserId()
        {
            try
            {
                if (Bikewale.Common.CurrentUser.UserId > 0)
                {
                    userId = Bikewale.Common.CurrentUser.Id;
                    userEmail = Bikewale.Common.CurrentUser.Email;
                    userName = Bikewale.Common.CurrentUser.Name;
                    currentCityArea = GlobalCityArea.GetGlobalCityArea();
                    cookieCityName = currentCityArea.City;
                    cookieCityId = currentCityArea.CityId;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.used.sell.default.BindUserId()");

            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 20/10/2016
        /// Description : Function to bind makes.
        /// </summary>
        protected void BindMakes()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    _makesRepository = container.Resolve<IBikeMakesCacheRepository>();
                    objMakeList = _makesRepository.GetMakesByType(EnumBikeType.Used);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.m.used.sell.default.BindMakes()");

            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 20/10/2016
        /// Description : Function to bind Cities (both registered at and current city).
        /// </summary>
        protected void BindCities()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICity, CityRepository>();
                    container.RegisterType<ICacheManager, MemcacheManager>(); ;
                    container.RegisterType<ICityCacheRepository, CityCacheRepository>();
                    ICityCacheRepository cityCacheRepository = container.Resolve<ICityCacheRepository>();
                    objCityList = cityCacheRepository.GetAllCities(EnumBikeType.All);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.used.sell.default.BindCities()");
            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 22/10/2016
        /// Description : Function to check if it is edit request.
        /// Modified By : Sushil Kumar on 27th Jan 2017
        /// Description : User redirection based on customerId - remove thread abort exception
        /// </summary>
        protected void CheckIsEdit()
        {
            uint customerId = 0;
            try
            {
                customerId = Bikewale.Common.CurrentUser.UserId;

                string inquiry = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(inquiry) && Int32.TryParse(inquiry, out inquiryId) && inquiryId > 0)
                {
                    if (customerId > 0)
                        GetInquiryDetails();
                    isEdit = true;

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.used.sell.default.CheckIsEdit()");

            }
            finally
            {
                if (isEdit && customerId < 1) //user not logged-in
                {
                    Response.Redirect(String.Format("/m/users/login.aspx?ReturnUrl={0}/used/sell/?id={1}", Utility.BWConfiguration.Instance.BwHostUrl, inquiryId));
                }

            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 22/10/2016
        /// Description : Function to fetch inquiryDetails.
        /// </summary>
        protected void GetInquiryDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICustomerRepository<CustomerEntity, UInt32>, CustomerRepository<CustomerEntity, UInt32>>();
                    container.RegisterType<ICustomer<CustomerEntity, UInt32>, Customer<CustomerEntity, UInt32>>();
                    container.RegisterType<IMobileVerificationRepository, MobileVerificationRepository>();
                    container.RegisterType<IMobileVerification, MobileVerification>();
                    container.RegisterType<IUsedBikeBuyerRepository, UsedBikeBuyerRepository>();
                    container.RegisterType<ISellBikesRepository<SellBikeAd, int>, SellBikesRepository<SellBikeAd, int>>();
                    container.RegisterType<IUsedBikeSellerRepository, UsedBikeSellerRepository>();
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();
					container.RegisterType<IBikeModelsCacheHelper, BikeModelsCacheHelper>();
					container.RegisterType<IPager, Pager>().RegisterType<ICacheManager, MemcacheManager>(); ;
                    container.RegisterType<ISellBikes, SellBikes>();
                    var obj = container.Resolve<ISellBikes>();

                    if (obj != null)
                    {
                        SellBikeAd inquiryDetailsObject = obj.GetById(inquiryId, Bikewale.Common.CurrentUser.UserId);
                        if (inquiryDetailsObject != null)
                        {
                            inquiryDTO = ConvertToDto(inquiryDetailsObject);
                            if (inquiryDTO != null)
                                inquiryDTO.ManufacturingYear = (DateTime)inquiryDTO.ManufacturingYear;
                        }
                        isAuthorized = inquiryDetailsObject == null ? false : true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.used.m.sell.default.GetInquiryDetails()");

            }

        }

        /// <summary>
        /// Created By : Sajal Gupta on 24/10/2016
        /// Description : Function to convert entity to DTO.
        /// </summary>
        private SellBikeAdDTO ConvertToDto(SellBikeAd inquiryDetailsObject)
        {
            AutoMapper.Mapper.CreateMap<Bikewale.Entities.BikeData.BikeMakeEntityBase, BBMakeBase>();
            AutoMapper.Mapper.CreateMap<Bikewale.Entities.BikeData.BikeModelEntityBase, BBModelBase>();
            AutoMapper.Mapper.CreateMap<BikeVersionEntityBase, BBVersionBase>();
            AutoMapper.Mapper.CreateMap<Bikewale.Entities.Used.SellAdStatus, Bikewale.DTO.UsedBikes.SellAdStatus>();
            AutoMapper.Mapper.CreateMap<SellBikeAdOtherInformation, SellBikeAdOtherInformationDTO>();
            AutoMapper.Mapper.CreateMap<SellerEntity, SellerDTO>();
            AutoMapper.Mapper.CreateMap<BikePhoto, Bikewale.DTO.Used.Search.BikePhoto>();
            AutoMapper.Mapper.CreateMap<SellBikeAd, SellBikeAdDTO>();
            return AutoMapper.Mapper.Map<SellBikeAdDTO>(inquiryDetailsObject);
        }
    }
}