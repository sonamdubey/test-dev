using Carwale.Entity.Common;
using System;
using System.Collections.Generic;

namespace Carwale.Entity.ES
{
    [Serializable]
    public class Pages : IdName
    {                      
        public List<PropertiesEntity> Properties { get; set; }        
    }
}