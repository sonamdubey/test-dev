using Carwale.Entity;
using Carwale.Entity.Classified;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock
{
    public class ResultsFiltersPagerDesktop
    {
        public IList<StockBaseEntity> ResultsData { get; set; }

        public PagerOutputEntity PagerData { get; set; }

        public CountData FiltersData { get; set; }

        //Added By : Sadhana Upadhyay on 28 May 2015 to get NearbyCities in same api
        public List<int> NearbyCities { get; set; }

        public List<Carwale.Entity.Classified.City> NearByCitiesWithCount { get; set; }

        public int LastNonFeaturedSlotRank { get; set; }

        public int LastDealerFeaturedSlotRank { get; set; }

        public int LastIndividualFeaturedSlotRank { get; set; }

        public string ExcludeStocks { get; set; }
    }
}
