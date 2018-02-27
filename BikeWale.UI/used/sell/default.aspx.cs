using Bikewale.BAL.Customer;
using Bikewale.BAL.MobileVerification;
using Bikewale.BAL.Pager;
using Bikewale.BAL.UsedBikes;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
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
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Used.Sell
{
    public class Default : System.Web.UI.Page
    {
        protected IEnumerable<Bikewale.Entities.BikeData.BikeMakeEntityBase> objMakeList = null;
        private IBikeMakesCacheRepository _makesRepository;
        protected IEnumerable<CityEntityBase> objCityList = null;
        protected string userId = null;
        protected bool isEdit = false;
        protected int inquiryId = 0;
        protected bool isAuthorized = true;
        protected SellBikeAd inquiryDetailsObject = null;
        protected SellBikeAdDTO inquiryDTO;
        protected string userEmail = null;
        protected string userName = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(Request.RawUrl);
            dd.DetectDevice();

            BindMakes();
            BindCities();
            BindUserId();
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
                if (CurrentUser.Id != null)
                {
                    userId = CurrentUser.Id;
                    userEmail = CurrentUser.Email;
                    userName = CurrentUser.Name;
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
                ErrorClass.LogError(ex, "Exception : Bikewale.used.sell.default.BindMakes()");

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
        /// </summary>
        protected void CheckIsEdit()
        {
            try
            {
                string inquiry = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(inquiry) && Int32.TryParse(inquiry, out inquiryId) && inquiryId > 0)
                {
                    if (userId == "-1") //user not logged-in
                    {
                        Response.Redirect(String.Format("/users/login.aspx?ReturnUrl={0}", HttpContext.Current.Request.RawUrl), false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }

                    isEdit = true;
                    GetInquiryDetails();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.used.sell.default.CheckIsEdit()");

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
                ISellBikes obj;

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
                    container.RegisterType<IPager, Pager>().RegisterType<ICacheManager, MemcacheManager>(); ;
                    container.RegisterType<ISellBikes, SellBikes>();
                    obj = container.Resolve<ISellBikes>();
                    if (obj != null)
                    {
                        SellBikeAd inquiryDetailsObject = obj.GetById(inquiryId, Convert.ToUInt64(userId));
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
                ErrorClass.LogError(ex, "Exception : Bikewale.used.sell.default.GetInquiryDetails()");

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