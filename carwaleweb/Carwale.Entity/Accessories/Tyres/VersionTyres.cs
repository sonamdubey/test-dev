using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Accessories.Tyres
{
    [Serializable]
    public class VersionTyres : TyreList
    {
        public string VersionTyreSize { get; set; }
    }
}
