using System.Collections.Generic;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carwale.Interfaces.Deals;
using Carwale.BL.Deals;
using Carwale.Interfaces;
using Carwale.Entity;
using Carwale.DAL.Deals;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Cache.Deals;
using AEPLCore.Cache;
using Carwale.Entity.Deals;
using AutoMapper;
using Carwale.DTOs.Deals;
using Carwale.Entity.CarData;
using Carwale.DTOs.CarData;
using AutoPoco;
using AutoPoco.Engine;
using Microsoft.Practices.Unity;
using Carwale.Service;
using Carwale.Interfaces.Campaigns;
using AEPLCore.Cache.Interfaces;

namespace CarWale.UnitTests.AdvantageUnitTests
{
    [TestClass]
    public class TestDealsBL
    {
        static IRepository<DealsInquiryDetail> _dealsInquiryRepository = new DealsInquiryRepository();
        static IDealsRepository _dealsRepository = new DealsRepository();
        static ICacheManager _cacheProvider = new CacheManager();
        static IDealsCache _dealsCache = new DealsCache(_cacheProvider, _dealsRepository);
        static IGenerationSessionFactory factory;
        static IUnityContainer container;
        static ICampaign _campaignBL; 

        static TestDealsBL()
        {
            container = UnityBootstrapper.Initialise();
            
            _campaignBL = container.Resolve<ICampaign>();

            factory = AutoPocoContainer.Configure(x =>
            {
                x.Conventions(c =>
                {
                    c.UseDefaultConventions();
                });
                x.AddFromAssemblyContainingType<Carwale.Entity.Deals.ProductDetails>();
            });
            Mapper.CreateMap<MakeEntity, CarMakesDTO>();
            Mapper.CreateMap<ModelEntity, CarModelsDTO>();
            Mapper.CreateMap<CarVersionEntity, Versions>()
                .ForMember(dest => dest.ID,
            opts => opts.MapFrom(
               src => src.ID))
               .ForMember(dest => dest.Name,
            opts => opts.MapFrom(
               src => src.Name));
            Mapper.CreateMap<ColorEntity, CarColorDTO>();
            Mapper.CreateMap<CarImageBase, CarImageBaseDTO>()
                .ForMember(dest => dest.OriginalImgPath,
                opts => opts.MapFrom(
                    src => src.ImagePath));
            Mapper.CreateMap<DealsStock, DealsStockAndroid_DTO>();
            Mapper.CreateMap<Carwale.Entity.Deals.ProductDetails, ProductDetailsDTO_Android>();
        }

        [TestMethod]
        public void GetProductDetailsNullTest()
        {
            var dealsCacheMock = new Mock<IDealsCache>();
            IDeals dealsBL = new DealsBL(_dealsInquiryRepository, dealsCacheMock.Object, _dealsRepository, _campaignBL);
            ProductDetailsDTO_Android result;
            dealsCacheMock.Setup(x => x.GetProductDetails(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns((Carwale.Entity.Deals.ProductDetails)null);
            result = dealsBL.GetProductDetails(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetProductDetailsNotNullTest()
        {
            IGenerationSession session = factory.CreateSession();
            var dealsCacheMock = new Mock<IDealsCache>();
            IDeals dealsBL = new DealsBL(_dealsInquiryRepository, dealsCacheMock.Object, _dealsRepository, _campaignBL);
            ProductDetailsDTO_Android result;
            dealsCacheMock.Setup(x => x.GetProductDetails(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(session.Single<Carwale.Entity.Deals.ProductDetails>().Get());
            result = dealsBL.GetProductDetails(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetProductDetailsDataTest()
        {
            IGenerationSession session = factory.CreateSession();
            var dealsCacheMock = new Mock<IDealsCache>();
            var dealsMock = new Mock<DealsBL>(_dealsInquiryRepository, dealsCacheMock.Object, _dealsRepository, _campaignBL) { CallBase = true };
            ProductDetailsDTO_Android result;
            string carwale = "https://www.carwale.com/";
            var versionList = session.List<CarVersionEntity>(4).Get();
            var colorList = session.List<ColorEntity>(3).First(1).Impose(x => x.ColorId, 1).Impose(x => x.ColorName, "Black").Next(1).Impose(x => x.ColorId, 2).Impose(x => x.ColorName, "Red Saphire").Next(1).Impose(x => x.ColorId, 3).Impose(x => x.ColorName, "White").All().Get();
            var dealsStock = session.List<DealsStock>(5).First(1).Impose(x => x.Color, session.Single<ColorEntity>().Impose(x => x.ColorId, 1).Get()).Impose(x => x.ManufacturingYear, 2013).Impose(x => x.Savings, 100)
                .Next(1).Impose(x => x.Color, session.Single<ColorEntity>().Impose(x => x.ColorId, 1).Get()).Impose(x => x.ManufacturingYear, 2014).Impose(x => x.Savings, 200).Impose(x => x.PriceBreakUpId,1)
                .Next(1).Impose(x => x.Color, session.Single<ColorEntity>().Impose(x => x.ColorId, 1).Get()).Impose(x => x.ManufacturingYear, 2015).Impose(x => x.Savings, 150)
                .Next(1).Impose(x => x.Color, session.Single<ColorEntity>().Impose(x => x.ColorId, 3).Get()).Impose(x => x.ManufacturingYear, 2014).Impose(x => x.Savings, 200).Impose(x => x.PriceBreakUpId, 1)
                .Next(1).Impose(x => x.Color, session.Single<ColorEntity>().Impose(x => x.ColorId, 2).Get()).Impose(x => x.ManufacturingYear, 2014).Impose(x => x.Savings, 200)
                .All().Get();
            var carImage = new CarImageBase() { HostUrl = carwale, ImagePath = "xcent.jpg" };
            var make = session.Single<MakeEntity>().Impose(x => x.MakeName, "Maruti Suzuki").Get();
            var model = session.Single<ModelEntity>().Impose(x => x.MaskingName, "swift-dzire").Impose(x => x.ModelName, "Swift Dzire[2013-2014]").Get();
            var productDetails = session.Single<Carwale.Entity.Deals.ProductDetails>().Impose(x => x.DealsStock, dealsStock).Impose(x => x.Version, versionList).Impose(x => x.ModelColorsEntity, colorList).Impose(x => x.CarImage, carImage)
                .Impose(x => x.Make, make).Impose(x => x.Model, model).Get();
            dealsCacheMock.Setup(x => x.GetProductDetails(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(productDetails);
            dealsMock.Setup(x => x.GetReasonsText(It.IsAny<DealsStock>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new BookingReasons { Heading = "3 Reasons", Reasons = new List<ReasonsText> { new ReasonsText("Reason", "Description"), new ReasonsText("Reason1", "Description1") } });
          //  dealsMock.Setup(x => x.FillPriceBreakUp(It.IsAny<DealsPriceBreakupEntity>())).Returns(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(), new KeyValuePair<string, string>() });
            result = dealsMock.Object.GetProductDetails(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.CarColorsDetails);
            Assert.IsNotNull(result.CarImage);
            Assert.IsNotNull(result.Make);
            Assert.IsNotNull(result.Model);
            Assert.IsNotNull(result.Version);
            Assert.IsTrue(result.CarColorsDetails.Count == 3);
            Assert.IsTrue(result.CarColorsDetails[0].Deals.Count == 3);
            Assert.IsTrue(result.CarColorsDetails[1].Deals.Count == 1);
            Assert.IsTrue(result.CarColorsDetails[2].Deals.Count == 1);
            Assert.IsTrue(result.Version.Count == 4);
            Assert.IsNotNull(result.CarColorsDetails[0].CarColor);
            Assert.IsNotNull(result.CarColorsDetails[1].CarColor);
            Assert.IsNotNull(result.CarColorsDetails[2].CarColor);
            Assert.AreEqual(result.CarColorsDetails[0].CarImage.HostUrl, carwale);
            Assert.AreEqual(result.CarColorsDetails[1].CarImage.HostUrl, carwale);
            Assert.AreEqual(result.CarColorsDetails[2].CarImage.HostUrl, carwale);
            Assert.AreEqual(result.CarColorsDetails[0].CarImage.OriginalImgPath, "/cars/deals/marutisuzuki/swift-dzire/black.jpg");
            Assert.AreEqual(result.CarColorsDetails[1].CarImage.OriginalImgPath, "/cars/deals/marutisuzuki/swift-dzire/red-saphire.jpg");
            Assert.AreEqual(result.CarColorsDetails[2].CarImage.OriginalImgPath, "/cars/deals/marutisuzuki/swift-dzire/white.jpg");
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.TollFreeNumber));
            Assert.IsTrue(result.BookingAmount > 0);
            Assert.IsTrue(result.CarColorsDetails[0].CurrentYear == 2014);
            Assert.IsTrue(result.CarColorsDetails[1].CurrentYear == 2014);
            Assert.IsTrue(result.CarColorsDetails[2].CurrentYear == 2014);
            Assert.AreEqual(result.Model.ModelName, "Swift Dzire");
            Assert.AreEqual(result.Make.MakeName, "Maruti Suzuki");
            Assert.AreEqual(result.CarImage.HostUrl, carwale);
            Assert.AreEqual(result.CarImage.OriginalImgPath, "xcent.jpg");
            Assert.AreEqual(result.CarColorsDetails[0].Deals[0].ReasonsSlug.Heading, "3 Reasons");
            Assert.AreEqual(result.CarColorsDetails[0].Deals[1].ReasonsSlug.Heading, "3 Reasons");
            Assert.AreEqual(result.CarColorsDetails[0].Deals[2].ReasonsSlug.Heading, "3 Reasons");
            Assert.AreEqual(result.CarColorsDetails[1].Deals[0].ReasonsSlug.Heading, "3 Reasons");
            Assert.AreEqual(result.CarColorsDetails[2].Deals[0].ReasonsSlug.Heading, "3 Reasons");
            Assert.IsTrue(result.CarColorsDetails[0].Deals[0].ReasonsSlug.Reasons.Count == 2);
            Assert.IsTrue(result.CarColorsDetails[0].Deals[1].ReasonsSlug.Reasons.Count == 2);
            Assert.IsTrue(result.CarColorsDetails[0].Deals[2].ReasonsSlug.Reasons.Count == 2);
            Assert.IsTrue(result.CarColorsDetails[1].Deals[0].ReasonsSlug.Reasons.Count == 2);
            Assert.IsTrue(result.CarColorsDetails[2].Deals[0].ReasonsSlug.Reasons.Count == 2);
        }

        [TestMethod]
        public void GetProductDetailsDealsNotAvailableTest()
        {
            IGenerationSession session = factory.CreateSession();
            var dealsCacheMock = new Mock<IDealsCache>();
            var dealsBL = new DealsBL(_dealsInquiryRepository, dealsCacheMock.Object, _dealsRepository, _campaignBL);
            ProductDetailsDTO_Android result;
            string carwale = "https://www.carwale.com/";
            var carImage = new CarImageBase() { HostUrl = carwale, ImagePath = "xcent.jpg" };
            var make = session.Single<MakeEntity>().Impose(x => x.MakeName, "Maruti Suzuki").Get();
            var model = session.Single<ModelEntity>().Impose(x => x.MaskingName, "swift-dzire").Impose(x => x.ModelName, "Swift Dzire[2013-2014]").Get();
            var productDetails = session.Single<Carwale.Entity.Deals.ProductDetails>().Impose(x => x.CarImage, carImage).Impose(x => x.Make, make).Impose(x => x.Model, model).Get();
            dealsCacheMock.Setup(x => x.GetProductDetails(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(productDetails);
            result = dealsBL.GetProductDetails(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.CarColorsDetails);
            Assert.IsNotNull(result.CarImage);
            Assert.IsNotNull(result.Make);
            Assert.IsNotNull(result.Model);
            Assert.IsNotNull(result.Version);
            Assert.IsTrue(result.CarColorsDetails.Count == 0);
            Assert.IsTrue(result.Version.Count == 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.TollFreeNumber));
            Assert.IsTrue(result.BookingAmount > 0);
            Assert.AreEqual(result.Model.ModelName, "Swift Dzire");
            Assert.AreEqual(result.Make.MakeName, "Maruti Suzuki");
            Assert.AreEqual(result.CarImage.HostUrl, carwale);
            Assert.AreEqual(result.CarImage.OriginalImgPath, "xcent.jpg");
        }

        [TestMethod]
        public void IsShowDealsNullTest()
        {
            var dealsMock = new Mock<DealsBL>(_dealsInquiryRepository, _dealsCache, _dealsRepository, _campaignBL) { CallBase = true };
            dealsMock.Setup(x => x.GetAdvantageCities(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns((List<Carwale.DTOs.City>)null);
            Assert.IsFalse(dealsMock.Object.IsShowDeals(It.IsAny<int>()));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(It.IsAny<int>(), false));
            Assert.IsTrue(dealsMock.Object.IsShowDeals(0, true));
            Assert.IsTrue(dealsMock.Object.IsShowDeals(-1, true));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(-1));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(-1, false));
        }

        [TestMethod]
        public void IsShowDealsEmptyTest()
        {
            var dealsMock = new Mock<DealsBL>(_dealsInquiryRepository, _dealsCache, _dealsRepository, _campaignBL) { CallBase = true };
            dealsMock.Setup(x => x.GetAdvantageCities(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Carwale.DTOs.City>());
            Assert.IsFalse(dealsMock.Object.IsShowDeals(It.IsAny<int>()));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(It.IsAny<int>(), false));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(0, true));
            Assert.IsTrue(dealsMock.Object.IsShowDeals(-1, true));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(-1));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(-1, false));
        }

        [TestMethod]    
        public void IsShowDealsCityTest()
        {
            var dealsMock = new Mock<DealsBL>(_dealsInquiryRepository, _dealsCache, _dealsRepository,_campaignBL) { CallBase = true };
            dealsMock.Setup(x => x.GetAdvantageCities(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Carwale.DTOs.City>() { new Carwale.DTOs.City() { CityId = 1 } });
            Assert.IsFalse(dealsMock.Object.IsShowDeals(2));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(2, false));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(2, true));
            Assert.IsTrue(dealsMock.Object.IsShowDeals(1));
            Assert.IsTrue(dealsMock.Object.IsShowDeals(1, true));
            Assert.IsTrue(dealsMock.Object.IsShowDeals(1, false));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(-1));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(-1, false));
        }


        [TestMethod]
        public void IsShowDealsMultiCityTest()
        {
            var dealsMock = new Mock<DealsBL>(_dealsInquiryRepository, _dealsCache, _dealsRepository, _campaignBL) { CallBase = true };
            dealsMock.Setup(x => x.GetAdvantageCities(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Carwale.DTOs.City>() { new Carwale.DTOs.City() { CityId = 1 }, new Carwale.DTOs.City() { CityId = 2 } });
            Assert.IsTrue(dealsMock.Object.IsShowDeals(2));
            Assert.IsTrue(dealsMock.Object.IsShowDeals(2, false));
            Assert.IsTrue(dealsMock.Object.IsShowDeals(2, true));
            Assert.IsTrue(dealsMock.Object.IsShowDeals(1));
            Assert.IsTrue(dealsMock.Object.IsShowDeals(1, true));
            Assert.IsTrue(dealsMock.Object.IsShowDeals(1, false));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(-1));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(-1, false));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(3));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(3, false));
            Assert.IsFalse(dealsMock.Object.IsShowDeals(3, true));
        }
    }
}
