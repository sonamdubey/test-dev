using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikewaleOpr.BAL
{
    public class BikeModels : IBikeModels
    {
        private readonly IBikeModelsRepository _IBikeModel;

        public BikeModels(IBikeModelsRepository bikeModel)
        {
            _IBikeModel = bikeModel;
        }

        public IEnumerable<UsedModelsByMake> GetPendingUsedBikesWithoutModelImage()
        {
            IList<UsedModelsByMake> objBikesByMake = null;
            try
            {
                IEnumerable<UsedBikeImagesModel> usedBikeList = _IBikeModel.GetPendingUsedBikesWithoutModelImage();

                if (usedBikeList != null && usedBikeList.Count() > 0)
                {
                    objBikesByMake = new List<UsedModelsByMake>();
                    usedBikeList.GroupBy(makesGroup => makesGroup.MakeId).ToList().ForEach(makesGroup =>
                    {
                        UsedModelsByMake objBike = new UsedModelsByMake();
                        var firstModel = makesGroup.FirstOrDefault();
                        objBike.MakeId = firstModel.MakeId;
                        objBike.MakeName = firstModel.MakeName;
                        objBike.ModelList = new List<string>();
                        foreach (var bikeEntity in makesGroup)
                        {
                            objBike.ModelList.Add(bikeEntity.ModelName);
                        }
                        objBikesByMake.Add(objBike);
                    });
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.BAL.BikeModels.GetPendingUsedBikesWithoutModelImage");
            }
            return objBikesByMake;
        }
    }
}
