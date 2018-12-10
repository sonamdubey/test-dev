using Carwale.Entity.Common;
using System;

namespace Carwale.Entity.ES
{
    [Serializable]
    public class PropertiesEntity : IdName
    {
        public int PageId { get; set; }
        public int PagePropertyMappingId { get; set; }
    }
}