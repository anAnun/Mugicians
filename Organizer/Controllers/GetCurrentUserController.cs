using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace Organizer.Controllers
{
    public class GetCurrentUserController : ApiController
    {
        [Route("users/getcurrentuser"), HttpGet]
        public HttpResponseMessage GetCurrentUser()
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            var cookie = Request.Headers.GetCookies(cookieName).SingleOrDefault();
            var currentUser = new UsersRole();

            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(cookie[cookieName].Value))
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie[cookieName].Value);
                    currentUser = usersService.GetCurrentUser(ticket.Name);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, currentUser);
        }
    }
}
