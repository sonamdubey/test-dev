using Carwale.Entity.Elastic;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified.ElasticSearch
{
    public interface IStockQueryProcessor
    {
        MultiSearchDescriptor GetMultiSearchDescriptorForSearchPage(ElasticOuptputs filterInputs, MultiSearchDescriptor msd);
    }
}
