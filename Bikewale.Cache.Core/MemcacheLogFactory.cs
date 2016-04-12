using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Bikewale.Cache.Core
{
    class MemcacheLogFactory: Enyim.Caching.ILogFactory
    {
        public MemcacheLogFactory()
        {
        }

        Enyim.Caching.ILog Enyim.Caching.ILogFactory.GetLogger(string name)
        {
            log4net.Config.XmlConfigurator.Configure();

            return new MemcacheLog(LogManager.GetLogger(name));
        }

        Enyim.Caching.ILog Enyim.Caching.ILogFactory.GetLogger(Type type)
        {
            log4net.Config.XmlConfigurator.Configure();
            return new MemcacheLog(LogManager.GetLogger(type));
        }
    }
}

