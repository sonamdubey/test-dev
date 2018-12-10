using Carwale.Entity.Classified;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Stock;
using Carwale.Utility;
using System;
using System.Collections.Generic;

namespace Carwale.BL.Stock
{
    public class StocksBySlot : IStocksBySlot
    {
        private static readonly Random _random = new Random();
        public List<StockBaseEntity> GetStocksAccordingToSlot(IDictionary<CwBasePackageId, List<StockBaseEntity>> mappingOfPackagesAndStocks, IEnumerable<Slot> slots)
        {
            List<StockBaseEntity> franchiseStocks = null;
            List<StockBaseEntity> diamondStocks = null;
            List<StockBaseEntity> platinumStocks = null;
            if (mappingOfPackagesAndStocks != null)
            {
                mappingOfPackagesAndStocks.TryGetValue(CwBasePackageId.Franchise, out franchiseStocks);
                mappingOfPackagesAndStocks.TryGetValue(CwBasePackageId.Diamond, out diamondStocks);
                mappingOfPackagesAndStocks.TryGetValue(CwBasePackageId.Platinum, out platinumStocks);
            }
            int remainingFranchiseStockCount = franchiseStocks == null ? 0 : franchiseStocks.Count;
            int remainingDiamondStockCount = diamondStocks == null ? 0 : diamondStocks.Count;
            int remainingPlatinumStockCount = platinumStocks == null ? 0 : platinumStocks.Count;

            if ((remainingFranchiseStockCount == 0 && remainingDiamondStockCount == 0 && remainingPlatinumStockCount == 0) || !slots.IsNotNullOrEmpty())
            {
                return null;
            }
            return AddStockToResult(slots, franchiseStocks, diamondStocks, platinumStocks, remainingFranchiseStockCount, remainingDiamondStockCount, remainingPlatinumStockCount);
        }

        private static List<StockBaseEntity> AddStockToResult(IEnumerable<Slot> slots, List<StockBaseEntity> franchiseStocks,
                                                              List<StockBaseEntity> diamondStocks, List<StockBaseEntity> platinumStocks,
                                                              int remainingFranchiseStockCount, int remainingDiamondStockCount, int remainingPlatinumStockCount)
        {
            var result = new List<StockBaseEntity>();
            foreach (Slot slot in slots)
            {
                //need to recalculate random number since stocks can exhaust during each iteration
                int maxValue = GetUpperLimitForRandomNumber(slot, remainingFranchiseStockCount, remainingDiamondStockCount, remainingPlatinumStockCount);
                int randomNum = _random.Next(maxValue);
                if (remainingFranchiseStockCount > 0 && randomNum < slot.FranchiseeProbability)
                {
                    remainingFranchiseStockCount = AddStockToResult(result, franchiseStocks, remainingFranchiseStockCount);
                }
                else if (remainingDiamondStockCount > 0 && randomNum < (slot.DiamondProbability + (remainingFranchiseStockCount == 0 ? 0 : slot.FranchiseeProbability)))
                {
                    remainingDiamondStockCount = AddStockToResult(result, diamondStocks, remainingDiamondStockCount);
                }
                else if (remainingPlatinumStockCount > 0 && randomNum < (slot.PlatinumProbability + (remainingFranchiseStockCount == 0 ? 0 : slot.FranchiseeProbability)
                                                                                         + (remainingDiamondStockCount == 0 ? 0 : slot.DiamondProbability)))
                {
                    remainingPlatinumStockCount = AddStockToResult(result, platinumStocks, remainingPlatinumStockCount);
                }
                else
                {
                    //do nothing. Added for sonar comments
                }
            }
            return result;
        }
        private static int AddStockToResult(List<StockBaseEntity> result, List<StockBaseEntity> stocks, int stockCount)
        {
            result.Add(GetRandomStockFromList(stocks, stockCount));
            return --stockCount;
        }
        private static StockBaseEntity GetRandomStockFromList(List<StockBaseEntity> stocks, int stockCount)
        {
            int selectedIndex = _random.Next(stockCount);
            StockBaseEntity stock = stocks[selectedIndex];
            SwapStockToLastIndex(stocks, selectedIndex, stockCount - 1); //so that next time this stock is excluded
            return stock;
        }

        private static void SwapStockToLastIndex(List<StockBaseEntity> stocks, int frontIndex, int backIndex)
        {
            var temp = stocks[backIndex];
            stocks[backIndex] = stocks[frontIndex];
            stocks[frontIndex] = temp;
        }

        /// <summary>
        /// This method calculates upper bound for probability based on stock availability.
        /// eg. franchise(0)[40%]---diamond(2)[20%]----platinum(2)[40%]
        /// since franchise is not available, new sample space is (100-40)=60 [100% considering only diamond and platinum]
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        private static int GetUpperLimitForRandomNumber(Slot slot, int remainingFranchiseStockCount, int remainingDiamondStockCount, int remainingPlatinumStockCount)
        {
            int upperBound = 100; //100% probability to start with
            if (remainingFranchiseStockCount == 0)
            {
                upperBound -= slot.FranchiseeProbability;
            }
            if (remainingDiamondStockCount == 0)
            {
                upperBound -= slot.DiamondProbability;
            }
            if (remainingPlatinumStockCount == 0)
            {
                upperBound -= slot.PlatinumProbability;
            }
            return upperBound;
        }
    }
}
