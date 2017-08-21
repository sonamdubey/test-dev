using BikewaleOpr.Entity.ConfigurePageMetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.ConfigurePageMetas
{
    /// <summary>
    /// Interface for Page meta repository
    /// </summary>
    public interface IPageMetasRepository
    {
        IEnumerable<PageEntity> GetPagesList();
        bool SavePageMetas(PageMetasEntity objMetas);
        PageMetasEntity GetPageMetasById(uint pageMetaId);
        bool UpdatePageMetaStatus(uint id, ushort status);
        IEnumerable<PageMetaEntity> GetPageMetas(uint pageMetaStatus);
    }
}
