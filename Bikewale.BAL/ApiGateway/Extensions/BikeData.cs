using System;
using System.Linq;
using System.Collections.Generic;
//using AEPLCore.Logging;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using Bikewale.Utility.ApiGatewayHelper;
using VehicleData.Service.ProtoClass;
using Google.Protobuf.Collections;

namespace Bikewale.BAL.ApiGateway.Extensions
{
	public static class BikeData
	{
		#region GetVehicleDataForVersionId Code
		public static IApiGatewayCaller GetVehicleDataForVersionId(IApiGatewayCaller caller, IEnumerable<int> versions)
		{
			try
			{
				if (versions != null)
				{
					VehicleDataRequest vehicleDataRequest = new VehicleDataRequest();
					vehicleDataRequest.VersionIds.AddRange(versions);
					vehicleDataRequest.ApplicationId = 2;
					vehicleDataRequest.ItemGroupTypes = string.Format("{0},{1}", (int)ItemGroupTypes.Individual, (int)ItemGroupTypes.IndividuallyShown);

					caller.Add(BWConfiguration.Instance.SpecsFeaturesServiceModuleName, "GetVehicleDataForVersionId", vehicleDataRequest);
				}
			}
			catch (Exception ex)
			{
				//Logger.LogException(e);
				ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Extensions.BikeData.GetVehicleDataForVersionId");
				return null;
			}

			return caller;
		}

		public static SpecsFeaturesEntity ConvertToBwSpecsFeaturesEntity(VehicleDataValue vehicleDataValue)
		{
			SpecsFeaturesEntity specsFeaturesEntity = null;
			try
			{
				if (vehicleDataValue != null)
				{
					specsFeaturesEntity = new SpecsFeaturesEntity();
					var specs = new List<SpecsFeaturesCategory>();
					foreach (var category in vehicleDataValue.Specifications)
					{
						specs.Add(new SpecsFeaturesCategory
						{
							DisplayText = category.Name,
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
				ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.ConvertToBWSpecsFeaturesEntity");
			}
			return specsFeaturesEntity;
		}

		private static IEnumerable<SpecsFeaturesItem> ConvertToBwSpecsFeaturesEntity(RepeatedField<Item> items)
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
							UnitTypeText = item.UnitTypeName
						});
					}
					return itemList;
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.ConvertToBWSpecFeaturesItemList");
			}
			return null;
		}
		#endregion
				
		#region GetVersionSpecsByItemId Code

		public static IApiGatewayCaller GetVersionSpecsByItemId(this IApiGatewayCaller caller, IEnumerable<int> versions, IEnumerable<EnumSpecsFeaturesItem> items)
		{
			try
			{
				if (versions != null && items != null)
				{
					VersionsDataByItemIdsRequest objInput = new VersionsDataByItemIdsRequest()
					{
						ApplicationId = 2,
						VersionIds = { versions },
						ItemIds = { items.Select(item => (int)item) }
					};

					caller.Add(BWConfiguration.Instance.SpecsFeaturesServiceModuleName, "VersionsDataByItemIds", objInput);
				}
			}
			catch (Exception ex)
			{
				//Logger.LogException(e);
				ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Extensions.BikeData.GetVersionSpecsByItemId");
				return null;
			}

			return caller;
		}

		public static IEnumerable<VersionMinSpecsEntity> ConvertToVersionMinSpecsList(VersionItemsDataResponse response)
		{
			try
			{				
				if (response != null)
				{
					ICollection<VersionMinSpecsEntity> objSpecs = new List<VersionMinSpecsEntity>();

					foreach (var versionData in response.VersionItemsDataList)
					{
						objSpecs.Add(new VersionMinSpecsEntity
						{
							VersionId = versionData.Id,
							MinSpecsList = ConvertToMinSpecsList(versionData.ItemList)
						});
					}

					return objSpecs;
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.ConvertToVersionMinSpecsList");
			}
			return null;
		}

		private static IEnumerable<SpecsItem> ConvertToMinSpecsList(RepeatedField<ItemData> itemDataList)
		{
			ICollection<SpecsItem> specItemList = null;
			try
			{
				if (itemDataList != null)
				{
					specItemList = new List<SpecsItem>();
					foreach (var itemData in itemDataList)
					{
						specItemList.Add(new SpecsItem
						{
							Id = itemData.ItemId,
							Icon = itemData.Icon,
							Name = itemData.ItemName,
							Value = itemData.Value,
							UnitType = itemData.UnitType
						});
					}
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.ConvertToMinSpecsList");
			}
			return specItemList;
		}

		#endregion
	}
}
