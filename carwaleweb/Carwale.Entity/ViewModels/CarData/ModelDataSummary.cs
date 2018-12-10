using Carwale.Entity.CarData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ViewModels.CarData
{
    public class ModelDataSummary : ModelBase
    {
        public List<ModelFeatures> ModelFeature { get; set; }
        public List<CarModelSpecs> SpecsSummary { get; set; }
    }
}
