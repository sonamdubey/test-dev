using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.V8;
using React;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Bikewale.ReactConfig), "Configure")]

namespace Bikewale
{
    public static class ReactConfig
    {
        public static void Configure()
        {
            int minSize = Bikewale.Utility.BWConfiguration.Instance.MinEnginePoolSize;
            int maxSize = Bikewale.Utility.BWConfiguration.Instance.MaxEnginePoolSize;
            if (Bikewale.Utility.BWConfiguration.Instance.EnablePWA)
            {
                JsEngineSwitcher.Instance.EngineFactories.AddV8();
            }

            ReactSiteConfiguration.Configuration
              .SetLoadBabel(false)
              .SetUseDebugReact(false)
              .AddScriptWithoutTransform("~/Scripts/server.bundle.js")
              .SetStartEngines(minSize)
              .SetMaxEngines(maxSize)
              .SetReuseJavaScriptEngines(true)
              ;

        }
    }
}