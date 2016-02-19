using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BB.UI.Web.MVC.Controllers.Web_API
{
    public class OrganisationsController : ApiController
    {
        // GET: api/Organisations
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Organisations/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Organisations
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Organisations/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Organisations/5
        public void Delete(int id)
        {
        }
    }
}
