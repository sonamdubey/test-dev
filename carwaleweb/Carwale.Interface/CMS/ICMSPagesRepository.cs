using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces
{
    public interface ICMSPagesRepository<T>
    {
        TOut GetPages<TOut>(int contentId) where TOut : Hashtable;
        TPageEntity GetPageContent<TPageEntity>(int contentId, int pageId);
    }
}
