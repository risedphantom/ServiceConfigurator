using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceConfigurator.Core;
using ServiceConfigurator.Singleton;
using RazorEngine;
using RazorEngine.Templating;

namespace ServiceConfigurator.Controllers
{
    [RoutePrefix("iis")]
    public class IisViewController : ApiController
    {
        // GET /iis
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Index()
        {
            Logger.Log.Debug("Web page Index called");

            var result = Engine.Razor.RunCompile("Shared/_Layout", null, new object());

            return Request.CreateHtmlResponse(HttpStatusCode.OK, result);
        }

        // GET /iis/activeRestrictions
        [HttpGet]
        [Route("activeRestrictions")]
        public HttpResponseMessage ActiveRestrictions()
        {
            Logger.Log.Debug("Web page ActiveRestrictions called");

            var result = Engine.Razor.RunCompile("Iis/ActiveRestrictions", null, new object());

            return Request.CreateHtmlResponse(HttpStatusCode.OK, result);
        }

        // GET /iis/waitingRestrictions
        [HttpGet]
        [Route("waitingRestrictions")]
        public HttpResponseMessage WaitingRestrictions()
        {
            Logger.Log.Debug("Web page WaitingRestrictions called");

            var result = Engine.Razor.RunCompile("Iis/WaitingRestrictions", null, new object());

            return Request.CreateHtmlResponse(HttpStatusCode.OK, result);
        }

        // GET /iis/failedRestrictions
        [HttpGet]
        [Route("failedRestrictions")]
        public HttpResponseMessage FailedRestrictions()
        {
            Logger.Log.Debug("Web page FailedRestrictions called");

            var result = Engine.Razor.RunCompile("Iis/FailedRestrictions", null, new object());

            return Request.CreateHtmlResponse(HttpStatusCode.OK, result);
        }

        // GET /iis/sites
        [HttpGet]
        [Route("sites")]
        public HttpResponseMessage Sites()
        {
            Logger.Log.Debug("Web page Sites called");

            var result = Engine.Razor.RunCompile("Iis/Sites", null, new object());

            return Request.CreateHtmlResponse(HttpStatusCode.OK, result);
        }
    }
}
    