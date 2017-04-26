using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 28 Mar 2017
    /// Description :   BikeModelRank Model
    /// </summary>
    public class BikeModelRank
    {
        private readonly IBikeModelsCacheRepository<int> _modelCache;
        private readonly uint _modelId;

        public BikeModelRank(IBikeModelsCacheRepository<int> modelCache, uint modelId)
        {
            _modelCache = modelCache;
            _modelId = modelId;
        }
        public BikeModelRankVM GetData()
        {
            BikeModelRankVM objVM = null;
            BikeRankingEntity rankObj;
            objVM = new BikeModelRankVM();
            objVM.Rank = _modelCache.GetBikeRankingByCategory(_modelId);

            if (objVM.Rank == null)
                objVM.Rank=new BikeRankingEntity();
                rankObj = objVM.Rank;
            switch (rankObj.BodyStyle)
            {

                case EnumBikeBodyStyles.Mileage:
                    objVM.StyleName = "Mileage Bikes";
                    objVM.BikeType = "Mileage Bike";
                    break;
                case EnumBikeBodyStyles.Sports:
                    objVM.StyleName = "Sports Bikes";
                    objVM.BikeType = "Sports Bike";
                    break;
                case EnumBikeBodyStyles.Cruiser:
                    objVM.StyleName = "Cruisers";
                    objVM.BikeType = "Cruiser";
                    break;
                case EnumBikeBodyStyles.Scooter:
                    objVM.StyleName = "Scooters";
                    objVM.BikeType = "Scooter";
                    break;
                case EnumBikeBodyStyles.AllBikes:
                default:
                    objVM.StyleName = "Bikes";
                    objVM.BikeType = "Bike";
                    break;

            }
            int rank = objVM.Rank.Rank;
            objVM.RankText = Bikewale.Utility.Format.FormatRank(rank);

            return objVM;
        }
    }
}