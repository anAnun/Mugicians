using Organizer.Interfaces;
using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Organizer.Controllers
{
    public class UsersController : ApiController
    {
        readonly IUsersService usersService;
        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        [Route("api/users"), HttpGet]
        public HttpResponseMessage GetAll()
        {
            var results = usersService.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        //[Route("api/users/{id:int}"), HttpGet()]
        //public HttpResponseMessage GetById(int Id)
        //{
        //    var result = usersService.GetById(Id);
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        [Route("api/users"), HttpPost, AllowAnonymous]
        public HttpResponseMessage Create(UserCreateRequest model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "You did not send any body data!");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                ItemResponse<int> itemResponse = new ItemResponse<int>();
                itemResponse.Item = usersService.Create(model);
                return Request.CreateResponse(HttpStatusCode.Created, itemResponse);
            }
            catch (DuplicateUserException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email already exists");
            }
        }

        [Route("api/users/{id:int}"), HttpPut]
        public HttpResponseMessage Update(int id, UserUpdateRequest model)
        {
            // TODO: this needs to check the user ID in the cookie and make sure the calling user
            // is actually allowed to update this user

            if (model == null)
            {
                ModelState.AddModelError("", "You did not send any body data!");
            }
            if (model.Id != id)
            {
                ModelState.AddModelError("", "Id in the URL does not match the Id in the body");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            usersService.Update(model);
            return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
        }

        [Route("api/users/{id:int}"), HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            User user = usersService.GetById(id);
            ItemResponse<User> itemResponse = new ItemResponse<User>();
            itemResponse.Item = user;
            return Request.CreateResponse(HttpStatusCode.OK, itemResponse);
        }

        [Route("api/users/login"), HttpPost, AllowAnonymous]
        public HttpResponseMessage LogIn(UserLogInRequest model)
        {
            bool isSuccessful = usersService.LogIn(model);
            if (model == null)
            {
                ModelState.AddModelError("", "You did not send any body data!");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            if (!isSuccessful)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Password does not match");
            }
            return Request.CreateResponse(HttpStatusCode.OK, isSuccessful);
        }

        [Route("api/users/getcurrentuser"), HttpGet]
        public HttpResponseMessage GetCurrentUser()
        {
            var getCurrentUser = User.Identity.GetId().Value;
            var currentUser = usersService.GetCurrentUser(getCurrentUser);
            return Request.CreateResponse(HttpStatusCode.OK, currentUser);
        }
    }
}
