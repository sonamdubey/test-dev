using BikewaleOpr.Entity.ConfigurePageMetas;
using System.Collections.Generic;

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
        bool UpdatePageMetaStatus(string id, ushort status);
        IEnumerable<PageMetaEntity> GetPageMetas(uint pageMetaStatus);
    }
}
