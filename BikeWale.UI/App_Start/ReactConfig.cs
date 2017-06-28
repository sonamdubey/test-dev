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
            if (!Bikewale.Utility.BWConfiguration.Instance.DisablePWA)
            {
                JsEngineSwitcher.Instance.EngineFactories.AddV8();
                IJsEngine engine = JsEngineSwitcher.Instance.CreateEngine(V8JsEngine.EngineName);
            }

            ReactSiteConfiguration.Configuration
              .SetLoadBabel(false)
              .SetUseDebugReact(false)
              .AddScriptWithoutTransform("~/Scripts/server.bundle.js")
              .SetStartEngines(25)
              .SetMaxEngines(100)
              .SetReuseJavaScriptEngines(true)
              //.SetAllowMsieEngine(false);
              ;

        }
    }
}