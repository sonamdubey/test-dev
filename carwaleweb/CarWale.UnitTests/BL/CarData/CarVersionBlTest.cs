using Carwale.BL.CarData;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Microsoft.Practices.Unity;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Customer;
using Carwale.Interfaces.Accessories.Tyres;
using Carwale.DAL.ApiGateway;
using AEPLCore.Cache.Interfaces;

namespace CarWale.UnitTests.BL.CarData
{
    [TestClass]
    public class CarVersionBlTest
    {
        Mock<ICarVersionCacheRepository> versionCacheRepo;
        Mock<IPriceQuoteBL> priceQuoteBL;
        Mock<IPQCacheRepository> pqCacheRepo;
        Mock<ICacheManager> cacheProvider;
        Mock<IDealsCache> dealsCache;
        Mock<ICarVersionRepository> carVersionRepo;
        Mock<IPrices> prices;
        Mock<ICarPriceQuoteAdapter> carPqAdapter;
        Mock<ICarModelCacheRepository> carModelCacheRepo;
        Mock<IGeoCitiesCacheRepository> geoCitiesCacheRepo;
        Mock<IPQGeoLocationBL> pqGeoLocationBL;
        Mock<ICustomerTracking> customerTracking;
        Mock<ICarModels> carModels;
        Mock<IApiGatewayCaller> apiGatewayCaller;
        Mock<ITyresBL> tyresBl;

        IUnityContainer _container;
        [TestMethod]
        public void FetchVersionInfoFromMaskingName()
        {
            _container = new UnityContainer();
            versionCacheRepo = new Mock<ICarVersionCacheRepository>();
            priceQuoteBL = new Mock<IPriceQuoteBL>();
            pqCacheRepo = new Mock<IPQCacheRepository>();
            cacheProvider = new Mock<ICacheManager>();
            dealsCache = new Mock<IDealsCache>();
            carVersionRepo = new Mock<ICarVersionRepository>();
            prices = new Mock<IPrices>();
            carPqAdapter = new Mock<ICarPriceQuoteAdapter>();
            carModelCacheRepo = new Mock<ICarModelCacheRepository>();
            geoCitiesCacheRepo = new Mock<IGeoCitiesCacheRepository>();
            pqGeoLocationBL = new Mock<IPQGeoLocationBL>();
            customerTracking = new Mock<ICustomerTracking>();
            tyresBl = new Mock<ITyresBL>();
            carModels = new Mock<ICarModels>();
            _container.RegisterInstance<ICarModels>(carModels.Object);
            apiGatewayCaller = new Mock<IApiGatewayCaller>();
            CarVersionsBL Bl = new CarVersionsBL(versionCacheRepo.Object, priceQuoteBL.Object,
                                            cacheProvider.Object, dealsCache.Object, carVersionRepo.Object, prices.Object, carPqAdapter.Object, carModelCacheRepo.Object, geoCitiesCacheRepo.Object, pqGeoLocationBL.Object, _container, customerTracking.Object, tyresBl.Object);

            //model level mocks
            ModelMaskingValidationEntity normalCaseModelBLRes = new ModelMaskingValidationEntity
            {
                ModelId = 560,
                IsValid = true,
                IsRedirect = false,
                RedirectUrl = string.Empty
            };
            ModelMaskingValidationEntity oldToNewRediretModelBLRes = new ModelMaskingValidationEntity
            {
                ModelId = 560,
                IsValid = true,
                IsRedirect = true,
                RedirectUrl = "http://localhost:81/renault-cars/duster/"
            };
            ModelMaskingValidationEntity makeRedirectModelBLRes = new ModelMaskingValidationEntity
            {
                ModelId = -1,
                IsValid = true,
                IsRedirect = true,
                RedirectUrl = "http://localhost:81/renault-cars/"
            };

            //version level mocks
            VersionMaskingResponse normalRes = new VersionMaskingResponse
            {
                ModelId = 560,
                VersionId = 5267,
                VersionMaskingName = "rxepetrol",
                Redirect = false,
                Valid = true,
                ModelMaskingName = "duster",
                MakeName = "renault"
            };
            VersionMaskingResponse oldToNewRedirect = new VersionMaskingResponse
            {
                ModelId = 560,
                VersionId = 5267,
                VersionMaskingName = "rxepetrol",
                Redirect = true,
                Valid = true,
                ModelMaskingName = "duster",
                MakeName = "renault"
            };
            VersionMaskingResponse modelRedirectRes = new VersionMaskingResponse
            {
                ModelId = 0,
                VersionId = 0,
                VersionMaskingName = string.Empty,
                Redirect = false,
                Valid = false,
                ModelMaskingName = "duster",
                MakeName = "renault"
            };
            VersionMaskingResponse makeRedirectRes = new VersionMaskingResponse
            {
                ModelId = 560,
                VersionId = 5267,
                VersionMaskingName = "rxepetrol",
                Redirect = false,
                Valid = true,
                ModelMaskingName = "duster",
                MakeName = "renault"
            };

            carModels.SetupSequence(carbl => carbl.FetchModelIdFromMaskingName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                                                    .Returns(normalCaseModelBLRes)
                                                                    .Returns(normalCaseModelBLRes)
                                                                    .Returns(normalCaseModelBLRes)
                                                                    .Returns(normalCaseModelBLRes)
                                                                    .Returns(normalCaseModelBLRes)
                                                                    .Returns(normalCaseModelBLRes)
                                                                    .Returns(makeRedirectModelBLRes)
                                                                    .Returns(normalCaseModelBLRes)
                                                                    .Returns(normalCaseModelBLRes)
                                                                    .Returns(oldToNewRediretModelBLRes)
                                                                    .Returns(oldToNewRediretModelBLRes)
                                                                    .Returns(oldToNewRediretModelBLRes)
                                                                    .Returns(oldToNewRediretModelBLRes);


            versionCacheRepo.SetupSequence(versioncache => versioncache.GetVersionInfoFromMaskingName(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                                                                    .Returns(modelRedirectRes)
                                                                    .Returns(modelRedirectRes)
                                                                    .Returns(oldToNewRedirect)
                                                                    .Returns(oldToNewRedirect)
                                                                    .Returns(oldToNewRedirect)
                                                                    .Returns(oldToNewRedirect)
                                                                    .Returns(makeRedirectRes)
                                                                    .Returns(normalRes)
                                                                    .Returns(normalRes)
                                                                    .Returns(normalRes)
                                                                    .Returns(normalRes)
                                                                    .Returns(oldToNewRedirect)
                                                                    .Returns(oldToNewRedirect);

            //For Desktop site when version masking invalid and model masking valid so goto model page
            var result0 = Bl.FetchVersionInfoFromMaskingName("duster", "rxepetsdfsrol", string.Empty, string.Empty, false, "renault");
            Assert.AreEqual(true, result0.IsRedirect);
            Assert.AreEqual("/renault-cars/duster/", result0.RedirectUrl);
            Assert.AreEqual(true, result0.IsValid);

            //For msite when version masking invalid and model masking valid so goto model page
            var result1 = Bl.FetchVersionInfoFromMaskingName("duster", "rxepetsdfsrol", string.Empty, string.Empty, true, "renault");
            Assert.AreEqual(true, result1.IsRedirect);
            Assert.AreEqual("/renault-cars/duster/", result1.RedirectUrl);
            Assert.AreEqual(true, result1.IsValid);

            //For desktop site when version masking name present in history and model masking valid go to new version page
            var result2 = Bl.FetchVersionInfoFromMaskingName("duster", "rxepetdfsrol", string.Empty, string.Empty, false, "renault");
            Assert.AreEqual(5267, result2.VersionId);
            Assert.AreEqual(560, result2.ModelId);
            Assert.AreEqual(true, result2.IsRedirect);
            Assert.AreEqual("/renault-cars/duster/rxepetrol/", result2.RedirectUrl);
            Assert.AreEqual(true, result2.IsValid);

            //For m site when version masking name present in history and model masking valid go to new version page
            var result3 = Bl.FetchVersionInfoFromMaskingName("duster", "rxepetdfsrol", string.Empty, string.Empty, true, "renault");
            Assert.AreEqual(5267, result3.VersionId);
            Assert.AreEqual(560, result3.ModelId);
            Assert.AreEqual(true, result3.IsRedirect);
            Assert.AreEqual("/m/renault-cars/duster/rxepetrol/", result3.RedirectUrl);
            Assert.AreEqual(true, result3.IsValid);

            //For Desktop when versionid id  present in url and model masking valid go to new masking name version page 
            var result4 = Bl.FetchVersionInfoFromMaskingName("duster", "rx-petrol-5267", string.Empty, "5267", false, "renault");
            Assert.AreEqual(5267, result4.VersionId);
            Assert.AreEqual(560, result4.ModelId);
            Assert.AreEqual(true, result4.IsRedirect);
            Assert.AreEqual("/renault-cars/duster/rxepetrol/", result4.RedirectUrl);
            Assert.AreEqual(true, result4.IsValid);

            //For msite when versionid id  present in url and model masking valid go to new masking name version page 
            var result5 = Bl.FetchVersionInfoFromMaskingName("duster", "rx-petrol-5267", string.Empty, "5267", true, "renault");
            Assert.AreEqual(5267, result5.VersionId);
            Assert.AreEqual(560, result5.ModelId);
            Assert.AreEqual(true, result5.IsRedirect);
            Assert.AreEqual("/m/renault-cars/duster/rxepetrol/", result5.RedirectUrl);
            Assert.AreEqual(true, result5.IsValid);

            //when model masking name is invalid  and version masking valid go to make page 
            var result6 = Bl.FetchVersionInfoFromMaskingName("duster", "rxepetrol", string.Empty, "5267", false, "renault");
            Assert.AreEqual(-1, result6.VersionId);
            Assert.AreEqual(-1, result6.ModelId);
            Assert.AreEqual(true, result6.IsRedirect);
            Assert.AreEqual("http://localhost:81/renault-cars/", result6.RedirectUrl);
            Assert.AreEqual(true, result6.IsValid);

            //for desktop when version masking and model masking is valid 
            var result7 = Bl.FetchVersionInfoFromMaskingName("duster", "rxepetrol", string.Empty, string.Empty, false, "renault");
            Assert.AreEqual(5267, result7.VersionId);
            Assert.AreEqual(560, result7.ModelId);
            Assert.AreEqual(false, result7.IsRedirect);
            Assert.AreEqual(string.Empty, result7.RedirectUrl);
            Assert.AreEqual(true, result7.IsValid);

            //for msite when version masking and model masking is valid 
            var result8 = Bl.FetchVersionInfoFromMaskingName("duster", "rxepetrol", string.Empty, string.Empty, true, "renault");
            Assert.AreEqual(5267, result8.VersionId);
            Assert.AreEqual(560, result8.ModelId);
            Assert.AreEqual(false, result8.IsRedirect);
            Assert.AreEqual(string.Empty, result8.RedirectUrl);
            Assert.AreEqual(true, result8.IsValid);

            //for desktop when model masking name is in history and version masking is valid then redirect to new url
            var result9 = Bl.FetchVersionInfoFromMaskingName("dusteer", "rxepetrol", string.Empty, string.Empty, false, "renault");
            Assert.AreEqual(5267, result9.VersionId);
            Assert.AreEqual(560, result9.ModelId);
            Assert.AreEqual(true, result9.IsRedirect);
            Assert.AreEqual("/renault-cars/duster/rxepetrol/", result9.RedirectUrl);
            Assert.AreEqual(true, result9.IsValid);

            //for msite when model masking name is in history and version masking is valid then redirect to new url
            var result10 = Bl.FetchVersionInfoFromMaskingName("dusteer", "rxepetrol", string.Empty, string.Empty, true, "renault");
            Assert.AreEqual(5267, result10.VersionId);
            Assert.AreEqual(560, result10.ModelId);
            Assert.AreEqual(true, result10.IsRedirect);
            Assert.AreEqual("/m/renault-cars/duster/rxepetrol/", result10.RedirectUrl);
            Assert.AreEqual(true, result10.IsValid);

            //for desktop when model masking name is in history and version masking is also in history then redirect to new url  
            var result11 = Bl.FetchVersionInfoFromMaskingName("dusteer", "rxeppetrol", string.Empty, string.Empty, false, "renault");
            Assert.AreEqual(5267, result11.VersionId);
            Assert.AreEqual(560, result11.ModelId);
            Assert.AreEqual(true, result11.IsRedirect);
            Assert.AreEqual("/renault-cars/duster/rxepetrol/", result11.RedirectUrl);
            Assert.AreEqual(true, result11.IsValid);

            //for msite when model masking name is in history and version masking is also in history then redirect to new url  
            var result12 = Bl.FetchVersionInfoFromMaskingName("dusteer", "rxeppetrol", string.Empty, string.Empty, true, "renault");
            Assert.AreEqual(5267, result12.VersionId);
            Assert.AreEqual(560, result12.ModelId);
            Assert.AreEqual(true, result12.IsRedirect);
            Assert.AreEqual("/m/renault-cars/duster/rxepetrol/", result12.RedirectUrl);
            Assert.AreEqual(true, result12.IsValid);
        }
    }
}
