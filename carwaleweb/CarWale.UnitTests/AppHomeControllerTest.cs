using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Carwale.Interfaces.NewCars;
using Moq;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.Geolocation;
using Carwale.DTOs;
using Carwale.Service.Controllers;
using System.Web.Http.Results;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Carwale.DAL;

namespace CarWale.UnitTests
{
	/// <summary>
	/// Summary description for AppHomeControllerTest
	/// </summary>
	[TestClass]
	public class AppHomeControllerTest
	{
		IUnityContainer _container;		
		[TestMethod]
		public void TestStatusCodes()
		{
			_container = new UnityContainer();
			var mockAdapter = new Mock<IServiceAdapterV2>();
			mockAdapter.SetupSequence(x => x.Get<CarHomeV3, CarDataAdapterInputs>(It.IsAny<CarDataAdapterInputs>()))
								.Returns(new CarHomeV3 { OrpText = string.Empty })
								.Returns(null)
								.Throws<Exception>();
			_container.RegisterInstance("AppAdapterHomeV3", mockAdapter.Object);
			var testInput1 = 1;
			var testInput2 = 999;
			var testInput3 = 999999999;
			var controller = new AppHomeController(_container);

			//when cityid is present in the database
			var result1 = controller.GetV1(testInput1) as OkNegotiatedContentResult<CarHomeV3>;
			var model1 = result1.Content;
			Assert.IsNotNull(model1);
			Assert.AreEqual(string.Empty, model1.OrpText);

			//when cityid is not present in the database
			var result2 = controller.GetV1(testInput2);
			Assert.IsInstanceOfType(result2, typeof(NotFoundResult));

			//when city id is valid but exception occurs
			var result3 = controller.GetV1(testInput3);
			Assert.IsInstanceOfType(result3, typeof(InternalServerErrorResult));


		}
	}
}
