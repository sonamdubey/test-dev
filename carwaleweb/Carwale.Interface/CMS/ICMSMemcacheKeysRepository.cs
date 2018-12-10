using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CMS
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface to manage memcache memcache keys for editCMS
    /// </summary>
    public interface ICMSMemcacheKeysRepository
    {
        void AddKeyToMemcache(string key, ushort applicationId, string categoryIdList);
    }
}
