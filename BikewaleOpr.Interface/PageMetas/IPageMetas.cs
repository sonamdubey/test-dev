using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface
{
    /// <summary>
    /// Interface for Page metas BAL
    /// </summary>
    public interface IPageMetas
    {
        bool UpdatePageMetaStatus(uint id, ushort status );
    }
}
