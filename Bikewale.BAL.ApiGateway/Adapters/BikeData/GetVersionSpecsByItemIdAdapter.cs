using System;
using System.Collections.Generic;
using System.Linq;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using Google.Protobuf;
using Google.Protobuf.Collections;
using VehicleData.Service.ProtoClass;

namespace Bikewale.BAL.ApiGateway.Adapters.BikeData
{
	/// <summary>
	/// Created By : Ashish G. Kamble on 6 Apr 2018
	/// Summary : Adapter class responsible for executing GRPC method through APIGateway and return reponse to the client.
	/// </summary>
	public class GetVersionSpecsByItemIdAdapter : AbstractApiGatewayAdapter<VersionsDataByItemIds_Input, IEnumerable<VersionMinSpecsEntity>, VersionItemsDataResponse>
	{
		public GetVersionSpecsByItemIdAdapter() : base(BWConfiguration.Instance.SpecsFeaturesServiceModuleName, "VersionsDataByItemIds")
		{
		}

		/// <summary>
		/// Function have implementation to convert bikewale entity to GRPC Message which will be passed to the APIGateway
		/// </summary>
		/// <param name="input">Bikewale Entity.</param>
		/// <returns>Returns GRPC message</returns>
		protected override IMessage BuildRequest(VersionsDataByItemIds_Input input)
		{
			VersionsDataByItemIdsRequest requestInput = null;

			try
			{
				if (input != null && input.Versions != null && input.Versions.Any() && input.Items != null)
				{
					requestInput = new VersionsDataByItemIdsRequest
					{
						ItemIds = { input.Items.Select(specId => (int)specId) },
						VersionIds = { input.Versions },
						ApplicationId = 2
					};
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Adapters.BikeData.VersionsDataByItemIdsAdapter.VersionsDataByItemIds");
			}

			return requestInput;
		}

		/// <summary>
		/// Function to convert GRPC message to the respective bikewale entity. This function should be overridden in derived class
		/// </summary>
		/// <param name="responseMessage">GRPC message</param>
		/// <returns>Returns bikewale entity as a response for the current adapter method</returns>		
		protected override IEnumerable<VersionMinSpecsEntity> BuildResponse(IMessage responseMessage)
		{
			ICollection<VersionMinSpecsEntity> versionMinSpecsList = null;

			try
			{
				VersionItemsDataResponse response = responseMessage as VersionItemsDataResponse;

				if (response != null)
				{					
					versionMinSpecsList = new List<VersionMinSpecsEntity>();

					foreach (var versionItemsData in response.VersionItemsDataList)
					{
						versionMinSpecsList.Add(new VersionMinSpecsEntity
						{
							VersionId = versionItemsData.Id,
							MinSpecsList = ConvertToMinSpecsList(versionItemsData.ItemList)
						});
					}
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Adapters.BikeData.VersionsDataByItemIdsAdapter.BuildResponse");
			}

			return versionMinSpecsList;
		}

		/// <summary>
		/// Function to convert GRPC message for items into bikewale entities
		/// </summary>
		/// <param name="items">GRPC Message</param>
		/// <returns>Bikewale Entity</returns>
		private IEnumerable<SpecsItem> ConvertToMinSpecsList(RepeatedField<ItemData> items)
		{
			ICollection<SpecsItem> specItemList = null;

			try
			{
				if (items != null)
				{
					specItemList = new List<SpecsItem>();

					foreach (var itemData in items)
					{
                        specItemList.Add(new SpecsItem
                        {
                            Id = itemData.ItemId,
                            Icon = itemData.Icon,
                            Name = itemData.ItemName,
                            Value = itemData.Value,
                            UnitType = itemData.UnitType,
                            DataType = (EnumSpecDataType)itemData.DataTypeId,
                            CustomTypeId = itemData.CustomTypeId
						});
					}
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Adapters.BikeData.VersionsDataByItemIdsAdapter.ConvertToMinSpecsList");
			}

			return specItemList;
		}

	}	// class
}	// namespace
