using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Nest;
using Carwale.Entity;

namespace Carwale.DTOs.Elastic
{
    public class ResultsFilterElastic
    {
        public List<StockBaseEntity> ResultsData { get; set; }

        public ElasticFiltersCount FiltersData { get; set; }

        public PagerOutputEntity PagerData { get; set; }
    }
}
