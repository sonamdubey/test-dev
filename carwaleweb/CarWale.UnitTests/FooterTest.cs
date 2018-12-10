using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Carwale.UI.Controllers.NewCars;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace CarWale.UnitTests
{
    [TestClass]
    public class FooterTest
    {
        IUnityContainer _container;
        [TestMethod]
        public void FooterTester()
        {
            _container = new UnityContainer();
            CarWidgetsController controller = new CarWidgetsController(_container);
            PartialViewResult result = controller.Footer() as PartialViewResult;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewName);
            Assert.IsNotNull(result.GetType());
            Assert.IsNull(result.Model);
        }
    }
}
