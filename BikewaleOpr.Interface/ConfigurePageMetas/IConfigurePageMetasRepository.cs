using BikewaleOpr.Entity.ConfigurePageMetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.ConfigurePageMetas
{
    public interface IConfigurePageMetasRepository
    {
        IEnumerable<PageEntity> GetPagesList();
        uint SavePageMetas(PageMetasEntity objMetas);
        PageMetasEntity GetPageMetasById(uint pageMetaId);

        IEnumerable<PageMetaEntity> GetPageMetas(uint pageMetaStatus);
    }
}
