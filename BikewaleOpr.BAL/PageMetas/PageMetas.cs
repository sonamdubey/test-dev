using BikewaleOpr.Interface;
using BikewaleOpr.Interface.ConfigurePageMetas;

namespace BikewaleOpr.BAL
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 17-Aug-2017
    /// Summary:BAL for Page metas
    /// 
    /// </summary>
    /// <seealso cref="BikewaleOpr.Interface.IPageMetas" />
    public class PageMetas : IPageMetas
    {
        private readonly IPageMetasRepository _pageMetas = null;
        public PageMetas(IPageMetasRepository PageMetas)
        {
            _pageMetas = PageMetas;
        }
        public bool UpdatePageMetaStatus(uint id, ushort status)
        {
           return  _pageMetas.UpdatePageMetaStatus(id, status);
        }
    }
}
