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
	public class GetVersionSpecsByIdAdapter : AbstractApiGatewayAdapater<IEnumerable<int>, SpecsFeaturesEntity, VehicleDataValue>
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
		/// </summary>
		/// <param name="responseMessage">GRPC message</param>
		/// <returns>Returns bikewale entity as a response for the current adapter method</returns>
		protected override SpecsFeaturesEntity BuildResponse(IMessage responseMessage)
		{
			SpecsFeaturesEntity specsFeaturesEntity = null;
			
			try
			{
				VehicleDataValue vehicleDataValue = responseMessage as VehicleDataValue;

				if (vehicleDataValue != null)
				{
					specsFeaturesEntity = new SpecsFeaturesEntity();
					var specs = new List<SpecsFeaturesCategory>();
					foreach (var category in vehicleDataValue.Specifications)
					{
						specs.Add(new SpecsFeaturesCategory
						{
							DisplayText = category.Name,
                            Icon = category.Icon,
							SpecsItemList = ConvertToBwSpecsFeaturesEntity(category.Items)
						});
					}
					specsFeaturesEntity.Specs = specs;
					var specsCategory = vehicleDataValue.Features.FirstOrDefault();
					if (specsCategory != null)
					{
						var features = ConvertToBwSpecsFeaturesEntity(specsCategory.Items);
						specsFeaturesEntity.Features = features;
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
		/// </summary>
		/// <param name="items">GRPC Message</param>
		/// <returns>Bikewale Entity</returns>
		private IEnumerable<SpecsFeaturesItem> ConvertToBwSpecsFeaturesEntity(RepeatedField<Item> items)
		{
			try
			{
				if (items != null)
				{
					var itemList = new List<SpecsFeaturesItem>();
					foreach (var item in items)
					{
						itemList.Add(new SpecsFeaturesItem
						{
							DisplayText = item.Name,
							Icon = item.Icon,
							Id = item.Id,
							ItemValues = item.ItemValues,
							UnitTypeText = item.UnitTypeName,
                            DataType = (EnumSpecDataType)item.DataTypeId
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
