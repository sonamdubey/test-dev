using System;
using System.Collections.Generic;
using System.Linq;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using Google.Protobuf;
using Google.Protobuf.Collections;
using VehicleData.Service.ProtoClass;

namespace Bikewale.BAL.ApiGateway.Adapters.BikeData
{
	/// <summary>
	/// Created By : Ashish G. Kamble on 4 Apr 2018
	/// Summary : Adapter class responsible for executing GRPC method through APIGateway and return reponse to the client.
	/// </summary>
	public class GetVersionSpecsByIdAdapter : AbstractApiGatewayAdapter<IEnumerable<int>, SpecsFeaturesEntity, VehicleDataList>
	{
		/// <summary>
		/// Constructor will set all dependencies required to get the data from APIGateway
		/// </summary>
		public GetVersionSpecsByIdAdapter()
		{
			ModuleName = BWConfiguration.Instance.SpecsFeaturesServiceModuleName;
			MethodName = "GetVehicleDataForVersionId";
		}

		/// <summary>
		/// Function have implementation to convert bikewale entity to GRPC Message which will be passed to the APIGateway
		/// </summary>
		/// <param name="input">Bikewale Entity.</param>
		/// <returns>Returns GRPC message</returns>
		protected override IMessage BuildRequest(IEnumerable<int> input)
		{
			VehicleDataRequest vehicleDataRequest = null;

			try
			{
				if (input != null)
				{
					vehicleDataRequest = new VehicleDataRequest();
					vehicleDataRequest.VersionIds.AddRange(input);
					vehicleDataRequest.ApplicationId = 2;
					vehicleDataRequest.ItemGroupTypes = string.Format("{0},{1}", (int)ItemGroupTypes.Individual, (int)ItemGroupTypes.IndividuallyShown);
				}
			}			
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Adapters.BikeData.GetVehicleDataForVersionIdAdapter.BuildRequest");
			}

			return vehicleDataRequest;
		}

		/// <summary>
		/// Function to convert GRPC message to the respective bikewale entity. This function should be overridden in derived class
		/// Modified By : Rajan Chauhan on 12 Apr 2018
		/// Description : Convertor change for change in grpc response object
		/// </summary>
		/// <param name="responseMessage">GRPC message</param>
		/// <returns>Returns bikewale entity as a response for the current adapter method</returns>
		protected override SpecsFeaturesEntity BuildResponse(IMessage responseMessage)
		{
			SpecsFeaturesEntity specsFeaturesEntity = null;

			try
			{
				VehicleDataList vehicleDataList = responseMessage as VehicleDataList;

				ICollection<VehicleDataValue> vehicleDataValueList = vehicleDataList != null ? vehicleDataList.VehicleData : null;

				if (vehicleDataValueList != null && vehicleDataValueList.Any())
				{
					specsFeaturesEntity = new SpecsFeaturesEntity();
					ICollection<IEnumerator<Category>> vehicleSpecsEnumeratorList = new List<IEnumerator<Category>>();
					ICollection<IEnumerator<Category>> vehicleFeaturesEnumeratorList = new List<IEnumerator<Category>>();
					foreach (var vehicleDataValue in vehicleDataValueList)
					{
						vehicleSpecsEnumeratorList.Add(vehicleDataValue.Specifications.GetEnumerator());
						vehicleFeaturesEnumeratorList.Add(vehicleDataValue.Features.GetEnumerator());
					}

					ICollection<SpecsFeaturesCategory> specCategoryList = new List<SpecsFeaturesCategory>();
					while (vehicleSpecsEnumeratorList.All(specCat => specCat.MoveNext()))
					{
						specCategoryList.Add(new SpecsFeaturesCategory
						{
							DisplayText = vehicleSpecsEnumeratorList.FirstOrDefault().Current.Name,
							Icon = vehicleSpecsEnumeratorList.FirstOrDefault().Current.Icon,
							SpecsItemList = ConvertToBwSpecsFeaturesEntity(vehicleSpecsEnumeratorList.Select(specCat => specCat.Current.Items))
						});
					}
					specsFeaturesEntity.Specs = specCategoryList;
					while (vehicleFeaturesEnumeratorList.All(featureCat => featureCat.MoveNext()))
					{
						specsFeaturesEntity.Features = ConvertToBwSpecsFeaturesEntity(vehicleFeaturesEnumeratorList.Select(featureCat => featureCat.Current.Items));

					}
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Adapters.BikeData.GetVehicleDataForVersionIdAdapter.BuildResponse");
			}
			return specsFeaturesEntity;
		}

		/// <summary>
		/// Function to convert GRPC message for items into bikewale entities
		/// Modified By : Rajan Chauhan on 12 Apr 2018
		/// Description : Convertor change for change in grpc response object
		/// </summary>
		/// <param name="items">GRPC Message</param>
		/// <returns>Bikewale Entity</returns>
		private IEnumerable<SpecsFeaturesItem> ConvertToBwSpecsFeaturesEntity(IEnumerable<RepeatedField<Item>> itemsList)
		{
			try
			{
				if (itemsList != null)
				{
					ICollection<SpecsFeaturesItem> itemList = new List<SpecsFeaturesItem>();
					ICollection<IEnumerator<Item>> itemsEnumeratorList = new List<IEnumerator<Item>>();
					foreach (var items in itemsList)
					{
						itemsEnumeratorList.Add(items.GetEnumerator());
					}
					while (itemsEnumeratorList.All(items => items.MoveNext()))
					{
						itemList.Add(new SpecsFeaturesItem
						{
							DisplayText = itemsEnumeratorList.FirstOrDefault().Current.Name,
							Icon = itemsEnumeratorList.FirstOrDefault().Current.Icon,
							Id = itemsEnumeratorList.FirstOrDefault().Current.Id,
							ItemValues = itemsEnumeratorList.Select(item => item.Current.ItemValue).ToList(),
							UnitTypeText = itemsEnumeratorList.FirstOrDefault().Current.UnitTypeName,
							DataType = (EnumSpecDataType)itemsEnumeratorList.FirstOrDefault().Current.DataTypeId
						});
					}
					return itemList;
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Adapters.GetVehicleDataForVersionIdAdapter.ConvertToBwSpecsFeaturesEntity");
			}
			return null;
		}

	}	// class
}	// namespace
