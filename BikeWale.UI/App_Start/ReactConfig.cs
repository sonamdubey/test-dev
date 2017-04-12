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
              .AddScriptWithoutTransform("~/Scripts/server.bundle.js");
        }
	}
}