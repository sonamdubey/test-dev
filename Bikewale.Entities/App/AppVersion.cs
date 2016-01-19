using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.App
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Description :   APP Version Entity
    /// Created On  :   07 Dec 2015
    /// </summary>
    public class AppVersion
    {
        public uint Id { get; set; }
        public bool IsSupported { get; set; }
        public bool IsLatest { get; set; }
    }
}
