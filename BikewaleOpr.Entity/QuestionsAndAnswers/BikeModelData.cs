using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.QnA
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 26 June 2018
    /// Description : Entity to hold bike model data associated with a particular Question ID (Questions & Answers module)
    /// </summary>
    public class BikeModelData
    {
        public uint ModelId { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string BikeName { get; set; }

    }
}
