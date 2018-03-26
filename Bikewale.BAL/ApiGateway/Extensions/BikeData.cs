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
		public static IApiGatewayCaller GetVehicleDataForVersionId(this IApiGatewayCaller caller, IEnumerable<int> versions)
		{
			try
			{
				if (caller != null)
				{
					VehicleDataRequest vehicleDataRequest = new VehicleDataRequest();
					vehicleDataRequest.VersionIds.AddRange(versions);
					vehicleDataRequest.ApplicationId = 2;
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
	}
}
