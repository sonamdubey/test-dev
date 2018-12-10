using Carwale.BL.Interface.Stock.Search;
using Carwale.DTOs.Classified.Stock;
using Carwale.Entity.Classified;
using Carwale.Entity.Stock.Search;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.CarDetail;
using Carwale.Interfaces.Elastic;
using Carwale.Service.Controllers;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Results;

namespace CarWale.UnitTests
{
    [TestClass]
    public class ClassifiedControllerTest
    {
        FilterInputs filterInputs;
        IUnityContainer _unity;
        private readonly IElasticSearchManager _search;
        private readonly IStockRepository _stockRepository;
        private readonly ICarDetailsCache _carDetailsCache;
        private readonly IStockSearchLogic<SearchResultDesktop> _stockSearchLogic;
        [TestMethod]
        public void StockFilter_CheckOkFlow_IHttpActionResult()
        {
            filterInputs = GetFilterInputs();
            _unity = new UnityContainer();


            var mockElastic = new Mock<IElasticSearchManager>();

            //mockElastic
            //    .Setup(x => x.SearchIndex<ResultsFiltersPagerDesktop>(It.IsAny<string>()))
            //    .Returns(new ResultsFiltersPagerDesktop());

            _unity.RegisterInstance<FilterInputs>(filterInputs);
            _unity.RegisterInstance<IElasticSearchManager>(mockElastic.Object);

            var controller = new ClassifiedController(_stockRepository, _search, _carDetailsCache, _stockSearchLogic);
            var controllerContext = new HttpControllerContext();
            var request = new HttpRequestMessage();
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            request.Headers.Add("sourceid", "1");
            controllerContext.Request = request;
            controller.ControllerContext = controllerContext;


            var result = controller.GetResultsWithFiltersAndPager(filterInputs);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ResultsFiltersPagerDesktop>));
        }

        [TestMethod]
        public void StockFilter_SourceIdIncorrect_IHttpActionResult()
        {
            filterInputs = GetFilterInputs();
            _unity = new UnityContainer();


            var mockElastic = new Mock<IElasticSearchManager>();

            //mockElastic
            //    .Setup(x => x.SearchIndex<ResultsFiltersPagerDesktop>(It.IsAny<string>()))
            //    .Returns(new ResultsFiltersPagerDesktop());

            _unity.RegisterInstance<FilterInputs>(filterInputs);
            _unity.RegisterInstance<IElasticSearchManager>(mockElastic.Object);

            var controller = new ClassifiedController(_stockRepository, _search, _carDetailsCache, _stockSearchLogic);
            var controllerContext = new HttpControllerContext();
            var request = new HttpRequestMessage();
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            //request.Headers.Add("sourceid", "1");
            controllerContext.Request = request;
            controller.ControllerContext = controllerContext;


            var result = controller.GetResultsWithFiltersAndPager(filterInputs) as BadRequestErrorMessageResult;
            Assert.AreEqual("Source Id is incorrect", result.Message);
            
        }

        //[TestMethod]
        //public void StockFilter_CheckException_IHttpActionResult()
        //{
        //    filterInputs = GetFilterInputs();
        //    _unity = new UnityContainer();


        //    var mockElastic = new Mock<IElasticSearchManager>();

        //    mockElastic
        //        .Setup(x => x.SearchIndex<ResultsFiltersPagerDesktop>(It.IsAny<string>()))
        //        .Throws(new Exception("new Exception"));

        //    _unity.RegisterInstance<FilterInputs>(filterInputs);
        //    _unity.RegisterInstance<IElasticSearchManager>(mockElastic.Object);

        //    var controller = new ClassifiedController(_unity);
        //    var controllerContext = new HttpControllerContext();
        //    var request = new HttpRequestMessage();
        //    request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        //    request.Headers.Add("sourceid", "1");
        //    controllerContext.Request = request;
        //    controller.ControllerContext = controllerContext;


        //    var result = controller.GetResultsWithFiltersAndPager(filterInputs);
        //    Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));

        //}



        private FilterInputs GetFilterInputs()
        {
            return new FilterInputs()
            {
                city = "1"
            };
        }
    }
}