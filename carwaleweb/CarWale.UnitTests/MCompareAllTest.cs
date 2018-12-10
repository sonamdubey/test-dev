using Carwale.DTOs.CarData;
using Carwale.Entity.CompareCars;
using Carwale.Interfaces.CompareCars;
using Carwale.UI.Controllers.m;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CarWale.UnitTests
{
    [TestClass]
    public class WhenRequestingMCompareAllPage
    {
        IUnityContainer _container;
        

        [TestMethod]
        public void ThenReturnCompareAllDTO()
        {
            _container = new UnityContainer();

            var mockCCCache = new Mock<ICompareCarsCacheRepository>();

            mockCCCache.SetupSequence(x => x.GetHotComaprisons(It.IsAny<short>()))
                .Returns(GetHotCompareModels(50))
                .Returns(GetHotCompareModels(29))
                .Returns(GetHotCompareModels(7))
                .Returns(GetHotCompareModels(23))
                .Returns(GetHotCompareModels(0))
                .Returns(GetHotCompareModels(23));

            _container.RegisterInstance<ICompareCarsCacheRepository>(mockCCCache.Object);
            var controller = new MCompareCarController(_container);
            var result = controller.AllCompareCars() as ViewResult;
            var model = result.Model as CompareAllDTO;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, model.CompareList.Count);
            Assert.AreEqual(1, model.CurrentPage);
            Assert.AreEqual(5, model.TotalPages);

            result = controller.AllCompareCars(2) as ViewResult;
            model = result.Model as CompareAllDTO;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, model.CompareList.Count);
            Assert.AreEqual(2, model.CurrentPage);
            Assert.AreEqual(3, model.TotalPages);

            result = controller.AllCompareCars(1) as ViewResult;
            model = result.Model as CompareAllDTO;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(7, model.CompareList.Count);
            Assert.AreEqual(1, model.CurrentPage);
            Assert.AreEqual(1, model.TotalPages);

            result = controller.AllCompareCars(3) as ViewResult;
            model = result.Model as CompareAllDTO;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, model.CompareList.Count);
            Assert.AreEqual(3, model.CurrentPage);
            Assert.AreEqual(3, model.TotalPages);

            result = controller.AllCompareCars(1) as ViewResult;
            // Assert
            Assert.IsNull(result);

            result = controller.AllCompareCars(4) as ViewResult;
            // Assert
            Assert.IsNull(result);

        }

        private List<HotCarComparison> GetHotCompareModels(int maxCompares)
        {
            var compareList = new List<HotCarComparison>();
            for (var i = 0; i < maxCompares; i++)
            {
                compareList.Add(new HotCarComparison());
            }
            return compareList;
        }
    }
}
