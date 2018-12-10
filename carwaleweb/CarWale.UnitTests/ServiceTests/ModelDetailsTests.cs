using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.AdapterModels;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.NewCars;
using Carwale.Service.Controllers.CarData;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Results;

namespace CarWale.UnitTests.ServiceTests
{
    [TestClass]
    public class ModelDetailsTests
    {
        IUnityContainer _container = new UnityContainer();
        private readonly Mock<ICarModels> _carModelBL = new Mock<ICarModels>();
        private readonly Mock<ICarModelRepository> _carModelRepos = new Mock<ICarModelRepository>();
        private readonly Mock<ICarModelCacheRepository> _carModelCacheRepos = new Mock<ICarModelCacheRepository>();
        private readonly Mock<ICarMileage> _carMileage = new Mock<ICarMileage>();
        private readonly Mock<IPhotos> _photosCacheRepo = new Mock<IPhotos>();
        private readonly Mock<IVideosBL> _videosRepository = new Mock<IVideosBL>();
        private readonly Mock<ICarVersions> _carVersionBl = new Mock<ICarVersions>();

        [TestMethod]
        public void ModelDetailsApiForApp()
        {
            Mock<IServiceAdapterV2> mocAdapter = new Mock<IServiceAdapterV2>();
            ModelPageDTOApp_V2 modelDto = CreateMockObject();
            mocAdapter.Setup(x => x.Get<ModelPageDTOApp_V2, CarDataAdapterInputs>(It.IsAny<CarDataAdapterInputs>())).Returns(modelDto);
            _container.RegisterInstance("ModelPageApp_V2", mocAdapter.Object);
            CarModelDataController carModelController = new CarModelDataController(_container, _carModelBL.Object, _carModelRepos.Object, _carModelCacheRepos.Object, _carMileage.Object, _photosCacheRepo.Object, _videosRepository.Object, _carVersionBl.Object);
            var result = carModelController.Get_V2(1118, 1) as OkNegotiatedContentResult<ModelPageDTOApp_V2>;
            var model = result.Content;
            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.ModelDetails);
            Assert.IsNotNull(model.ModelColors);
            Assert.IsInstanceOfType(model, typeof(ModelPageDTOApp_V2));
            Assert.AreEqual("Hyundai", model.ModelDetails.MakeName);
            Assert.AreEqual("Elite i20", model.ModelDetails.ModelName);
            CarModelDataController carModelControllerApi = new CarModelDataController(_container, _carModelBL.Object, _carModelRepos.Object, _carModelCacheRepos.Object, _carMileage.Object, _photosCacheRepo.Object, _videosRepository.Object, _carVersionBl.Object);
            carModelControllerApi.Request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/v2/model/1118/?cityid=1");
            var apiResult = carModelControllerApi.Get_V2(1118, 1) as OkNegotiatedContentResult<ModelPageDTOApp_V2>;
            Assert.IsNotNull(apiResult);
            Assert.AreEqual(true, MmvApiCall());
        }

        public static bool MmvApiCall()
        {
            bool status = false;
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage res = httpClient.GetAsync("http://localhost/api/v2/model/1118/?cityid=1").Result;
                status = res.IsSuccessStatusCode;
            }
            return status;
        }
        private ModelPageDTOApp_V2 CreateMockObject()
        {
            List<ModelColorsDTO> listColors = new List<ModelColorsDTO>();
            List<SimilarCarModelsDtoV3> similarCars = new List<SimilarCarModelsDtoV3>();
            List<CarVersionDtoV3> carVersions = new List<CarVersionDtoV3>();
            List<MileageDataDTO_V1> mileageData = new List<MileageDataDTO_V1>();
            ModelColorsDTO color = new ModelColorsDTO()
            {
                Color = "Red Passion",
                HexCode = "910810"
            };
            SimilarCarModelsDtoV3 alternateCar = new SimilarCarModelsDtoV3()
            {
                ReviewRateNew = "3.5",
                ReviewCount = 129,
                PopularVersionId = 4318,
                HostUrl = "https://imgd.aeplcdn.com/",
                ModelImageOriginal = "/cw/ec/19751/Maruti-Suzuki-Baleno-Right-Front-Three-Quarter-95859.jpg?wm=0",
                MakeName = "Maruti Suzuki",
                ModelName = "Baleno",
                ModelId = 930,
                CarModelUrl = null,
                PriceOverview = new PriceOverviewDtoV3
                {
                    Price = "₹ 6.10L",
                    PriceLabel = "On Road Price (GST)",
                    PriceSuffix = "onwards",
                    PricePrefix = "",
                    LabelColor = "#82888b ",
                    ReasonText = null,
                }

            };
            CarVersionDtoV3 version = new CarVersionDtoV3()
            {
                Id = 5195,
                Version = "Era 1.2",
                SpecsSummary = "1197cc Petrol, Manual, 18.6 kmpl",
                New = true,
                CarFuelType = "Petrol",
                TransmissionType = "Manual",
                PriceOverview = new PriceOverviewDtoV3
                {
                    Price = "₹ 6.10L",
                    PriceLabel = "On Road Price (GST)",
                    PriceSuffix = "",
                    PricePrefix = "",
                    LabelColor = "#82888b ",
                    ReasonText = null,
                }
            };
            MileageDataDTO_V1 mileage = new MileageDataDTO_V1()
            {
                FuelType = "Petrol",
                FuelUnit = "Kmpl",
                Displacement = "1197",
                Transmission = "Manual",
                FinalAverage = "18.6"
            };
            listColors.Add(color);
            similarCars.Add(alternateCar);
            carVersions.Add(version);
            mileageData.Add(mileage);
            ModelPageDTOApp_V2 modelDto = new ModelPageDTOApp_V2()
            {
                ModelDetails = new CarModelDetailsDtoV2()
                {
                    MakeName = "Hyundai",
                    Futuristic = false,
                    New = true,
                    IsDiscontinuedCar = false,
                    ModelName = "Elite i20",
                    ReviewCount = 33,
                    HostUrl = "https://imgd.aeplcdn.com/",
                    OriginalImage = "/cw/ec/28531/Hyundai-Elite-i20-Right-Front-Three-Quarter-94070.jpg?wm=0",
                    OfferExists = false,
                    ModelRating = "3.5",
                    MaskingName = "elite-i20",
                    PriceOverview = new PriceOverviewDtoV3
                    {
                        Price = "₹ 6.10L",
                        PriceLabel = "On Road Price (GST)",
                        PricePrefix = "onwards",
                        PriceSuffix = "",
                        LabelColor = "#82888b ",
                        ReasonText = null,
                    },
                    ShareUrl = "/hyundai-cars/elite-i20/",
                    ThreeSixtyAvailability = new ThreeSixtyAvailabilityDTO
                    {
                        Is360InteriorAvailable = false,
                        Is360OpenAvailable = false,
                        Is360ExteriorAvailable = false
                    }
                },
                ModelColors = listColors,
                ModelVideos = new List<VideoDTO>(),
                SimilarCars = similarCars,
                NewCarVersions = carVersions,
                CallSlugNumber = "",
                MileageData = mileageData,
                EmiInfo = new EmiInformationDtoV2()
                {
                    Text = "EMI starts at",
                    Amount = "11,150",
                    TooltipMessage = "Assuming 10.5% rate of interest and a tenure of 60 months",
                    RupeeSymbol = "₹ "
                },
                OrpText = "",
                City = new CityDTO
                {
                    Id = 1,
                    Name = "Mumbai"
                }
            };
            return modelDto;
        }
    }
}
