using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.NewBikeSearch
{
    public class FilterInput
    {
        public string[] Make { get; set; }
        public string[] Model { get; set; }
        public string[] Displacement { get; set; }
        public string MinBudget { get; set; }
        public string MaxBudget { get; set; }
        public string[] Mileage { get; set; }
        public string[] RideStyle { get; set; }
        public bool ABSAvailable { get; set; }
        public bool ABSNotAvailable { get; set; }
        public bool DiscBrake { get; set; }
        public bool DrumBrake { get; set; }
        public bool AlloyWheel { get; set; }
        public bool SpokeWheel { get; set; }
        public bool Electric { get; set; }
        public bool Manual { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string sc { get; set; }
        public string so { get; set; }
        public string PageSize { get; set; }
        public string PageNo { get; set; }
    }
}
