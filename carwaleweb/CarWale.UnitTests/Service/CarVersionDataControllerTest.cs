using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.Service.Controllers.CarData;
using Moq;
using System.Collections.Generic;
using Carwale.Entity.CarData;
using System.Web.Http.Results;
using Carwale.DTOs.CarData;
using System.Configuration;
using AutoMapper;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Microsoft.Practices.Unity;
using Carwale.Interfaces.NewCars;
using Carwale.Entity.AdapterModels;

namespace CarWale.UnitTests.Service
{
    [TestClass]
    public class CarVersionDataControllerTest
    {
        Mock<ICarVersionRepository> carVersionRepo;
        Mock<ICarVersionCacheRepository> carVersionCacheRepo;
        Mock<ICarVersions> carVersion;
        Mock<ICarModelCacheRepository> modelCacheRepository;
        [TestMethod]
        public void TestGetVersionDetailsListV2()
        {
            UnityContainer container = new UnityContainer();
            carVersionRepo = new Mock<ICarVersionRepository>();
            carVersionCacheRepo = new Mock<ICarVersionCacheRepository>();
            carVersion = new Mock<ICarVersions>();

            CarVersionsDataController controller = new CarVersionsDataController(carVersionRepo.Object, carVersionCacheRepo.Object, carVersion.Object, container, modelCacheRepository.Object);

            var adapter = new Mock<IServiceAdapterV2>();
            container.RegisterInstance("VersionPageAppAdapter", adapter.Object);

            CarDetailsListDTOV2 res = new CarDetailsListDTOV2
            {
                Models = new List<VersionDetailsDtoV2> {
                                                                    new VersionDetailsDtoV2{ MakeId = 45,
                                                                    ModelId = 560,
                                                                    VersionDetails = new List<VersionListDTO>{
                                                                        new VersionListDTO{
                                                                            Id = 5267
                                                                        }
                                                                    } }
                                                                },
                OrpText = ConfigurationManager.AppSettings["ShowPriceInCityText"]
            };
            CarDetailsListDTOV2 emptyResult = new CarDetailsListDTOV2
            {
                Models = new List<VersionDetailsDtoV2>()
            };
            adapter.SetupSequence(versionAdapter => versionAdapter.Get<CarDetailsListDTOV2, CarDataAdapterInputs>(It.IsAny<CarDataAdapterInputs>()))
                                                            .Returns(res)
                                                            .Returns(emptyResult)
                                                            .Returns(emptyResult)
                                                            .Throws(new Exception());
            var response1 = controller.GetVersionDetailsListV2(-1, "560", null) as OkNegotiatedContentResult<CarDetailsListDTOV2>;
            var result = response1.Content;
            Assert.IsTrue(result.Models.Count > 0);
            Assert.AreEqual(result.Models[0].MakeId, 45);

            var response = controller.GetVersionDetailsListV2(1, "80000", null);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));

            response = controller.GetVersionDetailsListV2(1, null, "80000");
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));

            response = controller.GetVersionDetailsListV2(1, null, null);
            Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult));

            response = controller.GetVersionDetailsListV2(1, "1", "2");
            Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult));

            response = controller.GetVersionDetailsListV2(1, "1,-2", null);
            Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult));

            response = controller.GetVersionDetailsListV2(1, null, "1,-2");
            Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult));
        }
    }
}
