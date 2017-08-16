using BikewaleOpr.Entity.ConfigurePageMetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Models.ConfigurePageMetas
{
    public class ConfigurePageMetaSearchVM
    {
        public IEnumerable<PageMetaEntity> PageMetaList { get; set; }

        public ushort PageMetaStatus { get; set; }

    }
}
