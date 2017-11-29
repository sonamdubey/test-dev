using System;
using System.Collections.Generic;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created by  :   Sumit Kate on 21 Jan 2016
    /// Description :   Compare Spec Entity
    /// </summary>
    [Serializable]
    public class CompareSubCategory
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public List<CompareBikeData> CompareSpec { get; set; }
    }
}
