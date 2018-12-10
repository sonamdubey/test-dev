using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carwale.Service.Controllers;
using Carwale.Entity.Classified;
using System.Net.Http;
using System.Web.Http.Controllers;
using Moq;
using Carwale.Interfaces.Classified;
using Carwale.DTOs.Classified.Stock;
using Carwale.Interfaces.Elastic;
using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using System;
using Carwale.Interfaces.NewCars;
using Carwale.DTOs.NewCars;
using System.Collections.Generic;
using Carwale.DTOs.PriceQuote;
using System.Threading.Tasks;
using System.Net;

namespace CarWale.UnitTests
{
    [TestClass]
    public class AppNewCarsControllerTest
    {
        IUnityContainer _container;
        [TestMethod]
        public async Task AppNewCarsLandingApiTest()
        {
            _container = new UnityContainer();
            var mockCacheRepository = new Mock<IServiceAdapterV2>();
            _container.RegisterInstance("AppAdapterNewCarsV3", mockCacheRepository.Object);
            var topSellingModels = new List<TopSellingCarModelV2>();
            topSellingModels.Add(new TopSellingCarModelV2() {
                MakeId = 10,
                MakeName = "Maruti Suzuki",
                ModelId = 1082,
                ModelName = "Dzire",
                MaskingName = "dzire",
                OverallRating = 3.59f,
                ReviewCount = 41,
                HostUrl = "https://imgd.aeplcdn.com/",
                OriginalImgPath ="/cw/ec/26860/Maruti-Suzuki-New-Dzire-Right-Front-Three-Quarter-96747.jpg?wm=0",
                Price = new PriceOverviewDTO()
                {
                    Price = "₹ 5.50L",
                    PriceLabel =  "Avg. Ex-Showroom",
                    PriceSuffix= "onwards",
                    PricePrefix =  "",
                    LabelColor = "#82888b ",
                    ReasonText = null,
                    City = new Carwale.DTOs.Geolocation.CityDTO()
                    {
                        Id = 0,
                        Name = null
                    },
                   CityColor = ""
                }

            });
            var recentLaunches = new List<LaunchedCarModelV2>();
            recentLaunches.Add(new LaunchedCarModelV2() {
                MakeId = 10,
                MakeName = "Maruti Suzuki",
                ModelId = 1140,
                ModelName = "Celerio X",
                MaskingName = "celerio-x",
                LaunchedDate = null,
                OverallRating = 0,
                ReviewCount = 0,
                HostUrl = "https://imgd.aeplcdn.com/",
                OriginalImgPath = "/cw/ec/31393/Maruti-Suzuki-Celerio-X-Exterior-113855.jpg?wm=0",
                Price =  new PriceOverviewDTO()
                {
                    Price ="₹ 4.58L",
                    PriceLabel = "Avg. Ex-Showroom",
                    PriceSuffix = "onwards",
                    PricePrefix =  "",
                    LabelColor =  "#82888b ",
                    ReasonText =  null,
                    City = new Carwale.DTOs.Geolocation.CityDTO()
                    {
                        Id = 0,
                        Name = null
                    },
                    CityColor = ""
                }
            });
            mockCacheRepository.SetupSequence(x => x.Get<NewCarHomeV3, int>(It.IsAny<int>()))
                               .Returns(new NewCarHomeV3 {
                                   OrpText = "",
                                   TopSellingModels= topSellingModels,
                                   RecentLaunches = recentLaunches
                               })
                               .Returns(null);
            var controller = new AppNewCarsController(_container);
            var result1 = controller.GetResult(1) as OkNegotiatedContentResult<NewCarHomeV3>;
            Assert.IsNotNull(result1);
            var model1 = result1.Content;
            Assert.AreEqual(1, model1.RecentLaunches.Count);
            Assert.AreEqual(1, model1.TopSellingModels.Count);
            Assert.IsNotNull(model1.TopSellingModels[0].Price);
            Assert.AreEqual(HttpStatusCode.NotFound, await NewCarApiCall("http://localhost/api/v3/newcars/-2"));
            Assert.AreEqual(HttpStatusCode.OK, await NewCarApiCall("http://localhost/api/v3/newcars/1"));
            mockCacheRepository.SetupSequence(x => x.Get<NewCarHomeV3, int>(It.IsAny<int>()))
                              .Returns(null);
            result1 = controller.GetResult(100000) as OkNegotiatedContentResult<NewCarHomeV3>;
            Assert.IsNull(result1);
        }

        public static async Task<HttpStatusCode> NewCarApiCall(string apiCallPath)
        {
            HttpResponseMessage res = null;
            using (HttpClient httpClient = new HttpClient())
            {
                res = await httpClient.GetAsync(apiCallPath).ConfigureAwait(false);
            }
            return res.StatusCode;
        }
    }
}