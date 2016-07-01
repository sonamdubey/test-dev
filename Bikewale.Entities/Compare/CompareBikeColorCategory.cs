using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Compare Bike Color Category. It contains version-wise color lists.
    /// </summary>
    [Serializable,DataContract]
    public class CompareBikeColorCategory
    {
        [DataMember]
        public List<CompareBikeColor> bikes { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}
