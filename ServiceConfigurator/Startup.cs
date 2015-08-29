using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Owin;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace ServiceConfigurator
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Ignore certificate errors
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            
            // Configure Razor
            var config = new TemplateServiceConfiguration
            {
                DisableTempFileLocking = true,
                TemplateManager = new ResolvePathTemplateManager(new List<string> { "Views/" }),
                CachingProvider = new DefaultCachingProvider(t => { })
            };

            Engine.Razor = RazorEngineService.Create(config);
            var httpConfig = new HttpConfiguration();

            // Configure Web API Routes:
            // - Enable Attribute Mapping
            // - Enable Default routes at /api.
            httpConfig.MapHttpAttributeRoutes();
            
            app.UseWebApi(httpConfig);
            app.UseStaticFiles("/Content");
        }
    }
}
