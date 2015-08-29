using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceConfigurator.Core;
using ServiceConfigurator.Models;
using ServiceConfigurator.Singleton;

namespace ServiceConfigurator.Controllers
{
    [RoutePrefix("api/rest/iis")]
    public class IisApiController : ApiController
    {
        // GET api/rest/iis/universe_answer
        [HttpGet]
        [Route("universe_answer")]
        public string AnswerForUniverse()
        {
            Logger.Log.Trace("AnswerForUniverse called.");
            return "42";
        }

        // GET api/rest/iis/activeRestrictionsPlain
        [HttpGet]
        [Route("activeRestrictionsPlain")]
        public List<IisSiteRestrictionPlain> ActiveRestrictionsPlain()
        {
            Logger.Log.Trace("ActiveRestrictionsPlain called.");

            return IisConfigurator.GetActiveSiteRestrictionsPlain();
        }

        // GET api/rest/iis/waitingRestrictionsPlain
        [HttpGet]
        [Route("waitingRestrictionsPlain")]
        public List<IisSiteRestrictionPlain> WaitingRestrictionsPlain()
        {
            Logger.Log.Trace("WaitingRestrictionsPlain called.");

            return IisConfigurator.GetWaitingSiteRestrictionsPlain();
        }

        // GET api/rest/iis/failedRestrictionsPlain
        [HttpGet]
        [Route("failedRestrictionsPlain")]
        public List<IisSiteRestrictionPlain> FailedRestrictionsPlain()
        {
            Logger.Log.Trace("FailedRestrictionsPlain called.");

            return IisConfigurator.GetFailedSiteRestrictionsPlain();
        }

        // GET api/rest/iis/sites
        [HttpGet]
        [Route("sites")]
        public List<IisSite> Sites()
        {
            Logger.Log.Trace("Sites called.");

            return IisConfigurator.GetSites();
        }

        // POST api/rest/iis/siteAdd
        [HttpPost]
        [Route("siteAdd")]
        [ValidateModel]
        public HttpResponseMessage SiteAdd([FromBody] IisSite site)
        {
            Logger.Log.Trace("SiteAdd called.");

            return Request.CreateResponse(HttpStatusCode.OK, IisConfigurator.AddIisSite(site));
        }

        // POST api/rest/iis/siteRestrictionAdd
        [HttpPost]
        [Route("siteRestrictionAdd")]
        [ValidateModel]
        public HttpResponseMessage SiteRestrictionAdd([FromBody] IisSiteRestriction siteRestriction)
        {
            Logger.Log.Trace("SiteRestrictionAdd called.");
            
            return Request.CreateResponse(HttpStatusCode.OK, IisConfigurator.AddSiteRestriction(siteRestriction));
        }

        // POST api/rest/iis/groupRestrictionAdd
        [HttpPost]
        [Route("groupRestrictionAdd")]
        [ValidateModel]
        public HttpResponseMessage GroupRestrictionAdd([FromBody] IisGroupRestriction groupRestriction)
        {
            Logger.Log.Trace("GroupRestrictionAdd called.");

            return Request.CreateResponse(HttpStatusCode.OK, IisConfigurator.AddGroupRestriction(groupRestriction));
        }

        // POST api/rest/iis/rejectRestriction
        [HttpPost]
        [Route("rejectRestriction")]
        [ValidateModel]
        public HttpResponseMessage RejectRestriction(int id, string reason)
        {
            Logger.Log.Trace("RejectRestriction called.");

            IisConfigurator.RejectRestriction(id, reason);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
