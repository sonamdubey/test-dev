using React;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Bikewale.ReactConfig), "Configure")]

namespace Bikewale
{
	public static class ReactConfig
	{
		public static void Configure()
		{
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