using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.V8;
using JavaScriptEngineSwitcher.ChakraCore;
using React;
using Bikewale.Utility;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Bikewale.ReactConfig), "Configure")]

namespace Bikewale
{
    public static class ReactConfig
    {
        public static void Configure()
        {
            int minSize = BWConfiguration.Instance.MinEnginePoolSize;
            int maxSize = BWConfiguration.Instance.MaxEnginePoolSize;

            if (BWConfiguration.Instance.EnablePWA)
            {
                if(BWConfiguration.Instance.UseV8Engine)
                    JsEngineSwitcher.Instance.EngineFactories.AddV8();
                else
                    JsEngineSwitcher.Instance.EngineFactories.AddChakraCore();
            }

            ReactSiteConfiguration.Configuration
              .SetLoadBabel(false)
              .SetUseDebugReact(false)
              .AddScriptWithoutTransform("~/pwa/server/server.bundle.js")
              .SetStartEngines(minSize)
              .SetMaxEngines(maxSize)
              .SetReuseJavaScriptEngines(true)
              ;

        }
    }
}