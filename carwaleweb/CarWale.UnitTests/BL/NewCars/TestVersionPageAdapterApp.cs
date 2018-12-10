using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carwale.Interfaces.CarData;
using Moq;
using Carwale.BL.NewCars;
using System.Collections.Generic;
using Carwale.Entity.CarData;
using Carwale.DTOs.CarData;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.Geolocation;
using Carwale.DTOs.CMS.ThreeSixtyView;
using AutoMapper;
using Carwale.Interfaces.Offers;
using Carwale.Interfaces.NewCars;

namespace CarWale.UnitTests.BL.NewCars
{
	[TestClass]
	public class TestVersionPageAdapterApp
	{
		Mock<ICarVersions> carVersion;
        Mock<ICarModelCacheRepository> modelCacheRepository;
        Mock<IOffersAdapter> offerBL;
		[TestMethod]
		public void TestGetVersionPageDTOForApp()
		{
			Mapper.CreateMap<CarVersionDetails, VersionListDTO>()
				.ForMember(dest => dest.Id, o => o.MapFrom(src => src.VersionId))
				.ForMember(dest => dest.Name, o => o.MapFrom(src => src.VersionName));
			Mapper.CreateMap<CarModelDetails, ThreeSixtyAvailabilityDTO>();
			carVersion = new Mock<ICarVersions>();
            modelCacheRepository = new Mock<ICarModelCacheRepository>();
            offerBL = new Mock<IOffersAdapter>();
			VersionPageAdapterApp versionAdapter = new VersionPageAdapterApp(carVersion.Object,modelCacheRepository.Object, offerBL.Object);
			Dictionary<int, List<CarVersionDetails>> resForModelId = new Dictionary<int, List<CarVersionDetails>>
																		{
																			[560] = new List<CarVersionDetails> {
																			new CarVersionDetails{ MakeId = 45,
																			ModelId = 560,
																			ModelName = "Duster",
																			VersionId = 5267
																			}
																		}};
			Dictionary<int, List<CarVersionDetails>> resForVersionId = new Dictionary<int, List<CarVersionDetails>>
																		{
																			[5267] = new List<CarVersionDetails> {
																			new CarVersionDetails{ MakeId = 45,
																			ModelId = 560,
																			ModelName = "Duster",
																			VersionId = 5267
																			}
																		}};
			Dictionary<int, List<CarVersionDetails>> emptyResult = new Dictionary<int, List<CarVersionDetails>>
																		{
																			[80000] = new List<CarVersionDetails> {
																			new CarVersionDetails{ MakeId = 0,
																			ModelId = 0,
																			ModelName = null,
																			VersionId = 0
																			}
																		}};
			carVersion.SetupSequence(versionBl =>
										versionBl.GetVersionDetailsList(It.IsAny<List<int>>(), It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
										.Returns(resForModelId)
										.Returns(resForVersionId)
										.Returns(resForModelId)
										.Returns(emptyResult)
										.Throws(new Exception());
			CarDetailsListDTOV2 result = versionAdapter.Get<CarDetailsListDTOV2, CarDataAdapterInputs>(
														new CarDataAdapterInputs {
															CustLocation = new Location
															{
																CityId = -1
															},
															ModelIds = new List<int> { 560 },
															VersionIds = null
														});
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Models.Count > 0);
			Assert.IsTrue(result.Models[0].ModelId == 560);
			Assert.IsNotNull(result.Models[0].VersionDetails);
			Assert.IsTrue(result.Models[0].VersionDetails[0].Id == 5267);

			result = versionAdapter.Get<CarDetailsListDTOV2, CarDataAdapterInputs>(
														new CarDataAdapterInputs
														{
															CustLocation = new Location
															{
																CityId = 1
															},
															ModelIds = null,
															VersionIds = new List<int> { 5267 }
														});
			Assert.IsNotNull(result);
			Assert.IsTrue(result.OrpText == string.Empty);
			Assert.IsTrue(result.Models.Count > 0);
			Assert.IsTrue(result.Models[0].ModelId == 560);
			Assert.IsNotNull(result.Models[0].VersionDetails);
			Assert.IsTrue(result.Models[0].VersionDetails[0].Id == 5267);

			result = versionAdapter.Get<CarDetailsListDTOV2, CarDataAdapterInputs>(
														new CarDataAdapterInputs
														{
															CustLocation = new Location
															{
																CityId = 1
															},
															ModelIds = new List<int> { 560 },
															VersionIds = new List<int> { 5267 }
														});
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Models.Count > 0);
			Assert.IsTrue(result.Models[0].ModelId == 560);
			Assert.IsNotNull(result.Models[0].VersionDetails);
			Assert.IsTrue(result.Models[0].VersionDetails[0].Id == 5267);

			result = versionAdapter.Get<CarDetailsListDTOV2, CarDataAdapterInputs>(
														new CarDataAdapterInputs
														{
															CustLocation = new Location
															{
																CityId = 1
															},
															ModelIds = null,
															VersionIds = new List<int> { 80000 }
														});
			Assert.IsTrue(result.Models.Count == 0);

			result = versionAdapter.Get<CarDetailsListDTOV2, CarDataAdapterInputs>(
														new CarDataAdapterInputs
														{
															CustLocation = new Location
															{
																CityId = 1
															},
															ModelIds = new List<int> { 80000 },
															VersionIds = null
														});
			Assert.IsTrue(result.Models.Count == 0);
		}
	}
}
