using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.CacheHelper.BikeData
{
	public class BikeModelsCacheHelper : IBikeModelsCacheHelper
	{
		private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;

		public BikeModelsCacheHelper(IBikeModelsRepository<BikeModelEntity, int> modelRepository)
		{
			_modelRepository = modelRepository;
		}

		/// <summary>
		/// Created by  : Pratibha Verma on 9 May 2018
		/// Description : to get make model list
		/// </summary>
		/// <param name="requestType"></param>
		/// <returns></returns>
		public IEnumerable<MakeModelListEntity> GetMakeModelList(EnumBikeType requestType)
		{
			IEnumerable<MakeModelListEntity> makeModel = null;
			try
			{
				IEnumerable<BikeMakeModelEntity> objModels = _modelRepository.GetAllModels(requestType);
				if (objModels != null && objModels.Any())
				{
					makeModel = (from obj in objModels
								 group obj.ModelBase by new { obj.MakeBase.MakeId, obj.MakeBase.MakeName, obj.MakeBase.MaskingName } into g
								 select new MakeModelListEntity
								 {
									 MakeBase = new BikeMakeEntityBase { MakeId = g.Key.MakeId, MakeName = g.Key.MakeName, MaskingName = g.Key.MaskingName },
									 ModelBase = g.ToList()
								 }
								 );
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "Bikewale.CacheHelper.BikeData.GetMakeModelList");
			}
			return makeModel;
		}
	}
}
