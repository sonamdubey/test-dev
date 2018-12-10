using Carwale.BL.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.PriceQuote;
using Carwale.Service;
using Microsoft.Practices.Unity;
using Moq;

namespace CarWale.UnitTests.BL.CarData
{
	public class CarMakeBase
	{
		protected readonly Mock<ICarMakesCacheRepository> _makesCacheRepo;
		protected readonly Mock<ICarModels> _carModelsBL;
		protected readonly Mock<ICarPriceQuoteAdapter> _prices;
		protected readonly Mock<ICarModelRepository> _modelRepo;
		protected readonly Mock<ICarModelCacheRepository> _modelCacheRepo;
		protected readonly CarMakesBL carMakesBL;
        protected readonly IPhotos photoBL;
        protected readonly IUnityContainer container = UnityBootstrapper.Resolver.GetContainer();
		protected CarMakeBase()
		{
			_makesCacheRepo = new Mock<ICarMakesCacheRepository>();
			_carModelsBL = new Mock<ICarModels>();
			_prices = new Mock<ICarPriceQuoteAdapter>();
			_modelRepo = new Mock<ICarModelRepository>();
			_modelCacheRepo = new Mock<ICarModelCacheRepository>();
            carMakesBL = new CarMakesBL(_makesCacheRepo.Object, _carModelsBL.Object, _prices.Object, _modelRepo.Object, _modelCacheRepo.Object);
            photoBL = container.Resolve<IPhotos>();
        }
	}
}
