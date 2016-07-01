using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate 21 Jan 2016
    /// Description :   Compare Specification Entity
    /// </summary>
    [Serializable,DataContract]
    public class CompareMainCategory
    {
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public List<CompareSubMainCategory> Spec { get; set; }
    }
}
