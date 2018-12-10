using Carwale.Entity.Classified;
using Carwale.Entity.Dealers.Used;
using Carwale.Interfaces.Dealers.Used;
using System;
using System.Collections.Generic;
using Carwale.Entity.Classified;

namespace Carwale.BL.Dealers.Used
{
    public class UsedDealerStocksBL : IUsedDealerStocksBL
    {
        private const int _defaultStocksCount = 25;
        private const int _defaultSizeForVirtualPage = 100;
        private readonly IUsedDealerStocksRepository _usedDealerStocksRepository;

        public UsedDealerStocksBL(IUsedDealerStocksRepository usedDealerStocksRepository)
        {
            _usedDealerStocksRepository = usedDealerStocksRepository;
        }

        public DealerStocksEntity GetDealerStocksEntity(int dealerId, int from, int size)
        {
            size = size < 1 ? _defaultStocksCount : size;
            var queryResponse = _usedDealerStocksRepository.GetDealerFranchiseStocks(dealerId, from, size);
            var usedDealerStocksEntity = new DealerStocksEntity
            {
                Stocks = queryResponse.Documents,
                NextPageUrl = GetNextPageUrl(dealerId, from, size, queryResponse.Total)
            };  
            return usedDealerStocksEntity;
        }

        public IEnumerable<StockBaseEntity> GetDealerStocksForVirtualPage(int dealerId)
        {
            var queryResponse = _usedDealerStocksRepository.GetDealerFranchiseStocks(dealerId, 0, _defaultSizeForVirtualPage);
            return queryResponse.Documents;
        }

        private static string GetNextPageUrl(int dealerId, int from, int size, long totalStockCount)
        {
            return totalStockCount > from + size
                    ? $"api/dealers/{ dealerId }/stocks/?from={ from + size }&size={ size }"
                    : null;
        }

        public Dictionary<string, string> ValidateDealerStocksApiInputs(int dealerId, int from, int size)
        {
            var errorDictionary = new Dictionary<string, string>();
            if (dealerId <= 0)
            {
                errorDictionary.Add("dealerId", "DealerId must be greater than 0.");
            }
            if (from < 0)
            {
                errorDictionary.Add("from", "From must be greater than 0.");
            }
            if (size < 0)
            {
                errorDictionary.Add("size", "Size must be greater than 0.");
            }
            return errorDictionary;
        }

        public static string GetDealerOtherCarsUrl(string dealerName, int dealerId)
        {
            return $"/used/dealers/{dealerName?.Trim().ToLower().Replace(" ","-")}/?dealerId={dealerId}";
        }
        public static string GetDealerOtherCarsApiUrl(int dealerId)
        {
            return $"/api/dealers/{dealerId}/stocks/?from=0&size=25";
        }
    }
}
