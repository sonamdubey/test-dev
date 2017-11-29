using System;
using System.Collections.Generic;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created by  :   Sumit Kate on 21 Jan 2016
    /// Description :   Compare Spec Category Entity
    /// </summary>
    [Serializable]
    public class CompareSubMainCategory
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public List<CompareSubCategory> SpecCategory { get; set; }
    }
}
