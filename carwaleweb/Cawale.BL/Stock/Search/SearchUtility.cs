using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Enum;
using Carwale.Entity.Stock.Search;
using System;
using System.Linq;

namespace Carwale.BL.Stock.Search
{
    public static class SearchUtility
    {
        public static string GetNextPageQueryParameter(FilterInputs filterInputs, string excludeStocks, SearchResultBase results)
        {
            string nextPageQueryParam = string.Empty;
            nextPageQueryParam += $"pn={(string.IsNullOrEmpty(filterInputs.pn) ? 2 : Convert.ToInt32(filterInputs.pn) + 1)}";

            if (!string.IsNullOrEmpty(filterInputs.bodytype))
            {
                nextPageQueryParam += $"&bodytype={filterInputs.bodytype.TrimEnd(' ').Replace(' ', '+')}";
            }

            if (!string.IsNullOrEmpty(filterInputs.budget))
            {
                nextPageQueryParam += $"&budget={filterInputs.budget}";
            }

            if (!string.IsNullOrEmpty(filterInputs.car))
            {
                nextPageQueryParam += $"&car={filterInputs.car.TrimEnd(' ').Replace(' ', '+')}";
            }

            if (!string.IsNullOrEmpty(filterInputs.city))
            {
                nextPageQueryParam += $"&city={filterInputs.city}";
            }

            if (!string.IsNullOrEmpty(filterInputs.color))
            {
                nextPageQueryParam += $"&color={filterInputs.color.TrimEnd(' ').Replace(' ', '+')}";
            }

            if (!string.IsNullOrEmpty(filterInputs.filterby))
            {
                nextPageQueryParam += $"&filterby={filterInputs.filterby.TrimEnd(' ').Replace(' ', '+')}";
            }

            //Added By : Sadhana Upadhyay on 5 May 2015 to add filterbyadditional filter
            if (!string.IsNullOrEmpty(filterInputs.filterbyadditional))
            {
                nextPageQueryParam += $"&filterbyadditional={filterInputs.filterbyadditional.TrimEnd(' ').Replace(' ', '+')}";
            }

            if (!string.IsNullOrEmpty(filterInputs.fuel))
            {
                nextPageQueryParam += $"&fuel={filterInputs.fuel.TrimEnd(' ').Replace(' ', '+')}";
            }

            if (!string.IsNullOrEmpty(filterInputs.kms))
            {
                nextPageQueryParam += $"&kms={filterInputs.kms}";
            }

            if (!string.IsNullOrEmpty(filterInputs.owners))
            {
                nextPageQueryParam += $"&owners={filterInputs.owners.TrimEnd(' ').Replace(' ', '+')}";
            }

            if (!string.IsNullOrEmpty(filterInputs.ps))
            {
                nextPageQueryParam += $"&ps={filterInputs.ps}";
            }

            if (!string.IsNullOrEmpty(filterInputs.sc))
            {
                nextPageQueryParam += $"&sc={filterInputs.sc}";
            }

            if (!string.IsNullOrEmpty(filterInputs.so))
            {
                nextPageQueryParam += $"&so={filterInputs.so}";
            }

            if (!string.IsNullOrEmpty(filterInputs.seller))
            {
                nextPageQueryParam += $"&seller={filterInputs.seller.TrimEnd(' ').Replace(' ', '+')}";
            }

            if (!string.IsNullOrEmpty(filterInputs.trans))
            {
                nextPageQueryParam += $"&trans={filterInputs.trans.TrimEnd(' ').Replace(' ', '+')}";
            }

            if (!string.IsNullOrEmpty(filterInputs.year))
            {
                nextPageQueryParam += $"&year={filterInputs.year}";
            }

            if(results.LastNonFeaturedSlotRank > 0)
            {
                nextPageQueryParam += $"&lcr={results.LastNonFeaturedSlotRank}";
            }

            nextPageQueryParam = filterInputs.Latitude <= 0 ? nextPageQueryParam : $"{ nextPageQueryParam }&latitude={ filterInputs.Latitude }";
            nextPageQueryParam = filterInputs.Longitude <= 0 ? nextPageQueryParam : $"{ nextPageQueryParam }&longitude={ filterInputs.Longitude }";
            nextPageQueryParam = $"{nextPageQueryParam}&shouldfetchnearbycars={filterInputs.ShouldFetchNearbyCars}";
            NearbyCarsBucket bucket = results?.ResultData?.LastOrDefault()?.NearbyCarsBucket == null ? NearbyCarsBucket.Default : results.ResultData.LastOrDefault().NearbyCarsBucket;
            nextPageQueryParam = bucket == NearbyCarsBucket.Default ? nextPageQueryParam : $"{nextPageQueryParam}&lastnearbycarsbucket={results.ResultData.LastOrDefault().NearbyCarsBucket}";
            if (filterInputs.Area > 0)
            {
                nextPageQueryParam = $"{nextPageQueryParam}&area={filterInputs.Area}";
            }

            if (results != null)
            {
                if (!string.IsNullOrEmpty(results.NearbyCityIds))
                {
                    nextPageQueryParam += $"&nearbyCityId={results.NearbyCityId}&nearbyCityIds={results.NearbyCityIds}&nearbyCityIdsStockCount={results.NearbyCityIdsStockCount}";
                }
                nextPageQueryParam += $"&stockfetched={(filterInputs.stockFetched + (results.ResultData != null ? results.ResultData.Count : 0))}";

            }
            nextPageQueryParam = !string.IsNullOrEmpty(excludeStocks) ? $"{nextPageQueryParam}&excludestocks={excludeStocks}" : nextPageQueryParam;
            return nextPageQueryParam;
        }

        public static bool ShouldFetchFeatureStocks(int pageNumber, string sortOrder)
        {
            return pageNumber == 1 && string.IsNullOrWhiteSpace(sortOrder);
        }
    }
}
