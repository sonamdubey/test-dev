using AEPLCore.Cache.Interfaces;
using Carwale.BL.CarData;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.SponsoredCar;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CarWale.UnitTests.BL.CarData
{
    [TestClass]
    public class CarModelBlTest
    {
        Mock<IUnityContainer> container;
        Mock<ICarModelCacheRepository> modelsCacheRepo;
        Mock<ICarVersions> carVersions;
        Mock<IPhotos> carPhotosBL;
		Mock<ICacheManager> cache;
        Mock<ICarModelRepository> carModelsRepo;
        Mock<ISponsoredCarBL> sponsoredBL;
        Mock<ICarDataLogic> carDataLogic;
        Mock<ICarPriceQuoteAdapter> carPrices;
        Mock<ICarRecommendationLogic> carRecommendationLogic;
        [TestMethod]
        public void FetchModelIdFromMaskingName()
        {
            container = new Mock<IUnityContainer>();
            modelsCacheRepo = new Mock<ICarModelCacheRepository>();
            carVersions = new Mock<ICarVersions>();
            carPhotosBL = new Mock<IPhotos>();
			cache = new Mock<ICacheManager>();
            carModelsRepo = new Mock<ICarModelRepository>();
            carDataLogic = new Mock<ICarDataLogic>();
            sponsoredBL = new Mock<ISponsoredCarBL>();
            carPrices = new Mock<ICarPriceQuoteAdapter>();
            carRecommendationLogic = new Mock<ICarRecommendationLogic>();
            CarModelsBL Bl = new CarModelsBL(container.Object, modelsCacheRepo.Object, carVersions.Object,
                                            carPhotosBL.Object,sponsoredBL.Object, carDataLogic.Object, carPrices.Object,carRecommendationLogic.Object);
            CarModelMaskingResponse normalCaseRes = new CarModelMaskingResponse
            {
                MaskingName = "duster",
                MakeId = 45,
                MakeName = "Renault",
                ModelId = 560,
                Redirect = false
            };
            CarModelMaskingResponse oldToNewRediret = new CarModelMaskingResponse
            {
                MaskingName = "duster",
                MakeId = 45,
                MakeName = "Renault",
                ModelId = 560,
                Redirect = true
            };

            var result = Bl.FetchModelIdFromMaskingName("duster", "560");
            Assert.AreEqual(560, result.ModelId);

            result = Bl.FetchModelIdFromMaskingName("duster", "560", "renault");
            Assert.AreEqual(560, result.ModelId);

            CarModelMaskingResponse notFound = new CarModelMaskingResponse { };
            modelsCacheRepo.SetupSequence(modelCache => modelCache.GetModelByMaskingName(It.IsAny<string>()))
                                                                    .Returns(notFound)
                                                                    .Returns(notFound)
                                                                    .Returns(notFound)
                                                                    .Returns(normalCaseRes)
                                                                    .Returns(oldToNewRediret);

            result = Bl.FetchModelIdFromMaskingName("fuster", "", "renault", false);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.IsRedirect);
            Assert.AreEqual(result.RedirectUrl, "/renault-cars/", true);

            result = Bl.FetchModelIdFromMaskingName("fuster", "", "renault", true);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.IsRedirect);
            Assert.AreEqual(result.RedirectUrl, "/m/renault-cars/", true);

            result = Bl.FetchModelIdFromMaskingName("fuster", "", "", true);
            Assert.AreEqual(result.ModelId, -1);
            Assert.IsFalse(result.IsValid);
            Assert.IsFalse(result.IsRedirect);
            Assert.AreEqual(result.RedirectUrl, string.Empty, true);

            result = Bl.FetchModelIdFromMaskingName("duster", "", "renault", true);
            Assert.AreEqual(result.ModelId, 560);
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.IsRedirect);
            Assert.AreEqual(result.RedirectUrl, string.Empty, true);

            result = Bl.FetchModelIdFromMaskingName("dusterOld", "", "renault", true);
            Assert.IsTrue(result.IsRedirect);
            Assert.AreEqual(result.RedirectUrl, "/renault-cars/duster/", true);
        }
    }
}
