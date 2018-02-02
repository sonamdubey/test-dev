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
        /// <summary>
        /// Modified by : Snehal Dange on 30th Jan 2018
        /// Description : Changed datatype of id from 'uint' to 'string'
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdatePageMetaStatus(string id, ushort status, uint updatedBy)
        {
            return _pageMetas.UpdatePageMetaStatus(id, status, updatedBy);
        }
    }
}
