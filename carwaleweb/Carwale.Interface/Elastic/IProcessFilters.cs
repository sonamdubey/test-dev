using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Elastic
{
    public interface IProcessFilters
    {
		//Modified By : Sadhana Upadhyay on 10 Mar 2015
       ElasticOuptputs ProcessFilterParams(FilterInputs inputs);
    }
}
