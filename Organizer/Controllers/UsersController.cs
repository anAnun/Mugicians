using Newtonsoft.Json.Linq;
using Organizer.Interfaces;
using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

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
            List<User> users = usersService.GetAll();
            ItemsResponse<User> itemsResponse = new ItemsResponse<User>();
            itemsResponse.Items = users;
            return Request.CreateResponse(HttpStatusCode.OK, itemsResponse);
        }

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

        [Route("api/users-basic-info/{id:int}"), HttpPut]
        public HttpResponseMessage UpdateBasicInfo(int id, UserUpdateBasicInfoRequest model)
        {
            int userId = User.Identity.GetId().Value;

            if (model == null)
            {
                ModelState.AddModelError("", "You did not send any body data!");
            }
            if (model.Id != id)
            {
                ModelState.AddModelError("", "Id in the URL does not match the Id in the body.");
            }
            if (model.Id != userId)
            {
                ModelState.AddModelError("", "User Id is not the same as Id in the body.");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            usersService.UpdateBasicInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
        }

        [Route("api/users/usertypeid/{id:int}"), HttpPut, AllowAnonymous]
        public HttpResponseMessage Update(int id, UserTypeUpdateRequest model)
        {
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
            usersService.UpdateUserTypeId(model);
            return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
        }

        [Route("api/users/{id:int}"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            usersService.Delete(id);
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


        [Route("api/userprofiles/{id:int?}"), HttpGet]
        public HttpResponseMessage GetProfileById(int? id = null)
        {
            var viewerId = User.Identity.GetId().Value;
            if (id == null)
            {
                id = viewerId;
            }
            var newObject = usersService.GetProfileById(id.Value, viewerId);
            ItemResponse<JRaw> itemResponse = new ItemResponse<JRaw>();
            itemResponse.Item = new JRaw(newObject);
            return Request.CreateResponse(HttpStatusCode.OK, itemResponse);
        }

        [Route("api/userprofiles"), HttpPut]
        public HttpResponseMessage UpdateProfile(UserProfileUpdateRequest model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "You did not send any body data!");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            usersService.UpdateProfile(model);
            return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
        }

        [Route("api/users/logout"), HttpPost]
        public HttpResponseMessage LogOut()
        {
            usersService.LogOut();
            return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
        }

        [Route("api/users/googlelogin"), HttpPost, AllowAnonymous]
        public HttpResponseMessage GoogleLogin(GoogleLogInRequest model)
        {
            bool authToken = usersService.GoogleLogin(model);
            if (!authToken)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User cannot be authenticated");
            }
            return Request.CreateResponse(HttpStatusCode.OK, authToken);
        }

        //[Route("api/users/multiplexed-avatar"), HttpPost]
        //public HttpResponseMessage MultiplexedGet(MultiplexedAvatarGetRequest req)
        //{
        //    var result = usersService.GetUjjuikserAvatarsByIds(req.Ids);

        //    var itemResponse = new ItemResponse<Dictionary<int, UserAvatarResponse>>();
        //    itemResponse.Item = result;

        //    return Request.CreateResponse(HttpStatusCode.OK, itemResponse);
        //}

        [Route("api/users/update-avatar-url"), HttpPut]
        public HttpResponseMessage UpdateAvatarUrl(UsersUpdateAvatarUrlRequest url)
        {
            var id = User.Identity.GetId().Value;

            usersService.UpdateAvatarUrl(id, url);

            return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());

        }
        [Route("api/users/updatepassword"), HttpPut]
        public HttpResponseMessage UpdatePassword(UserPasswordUpdateRequest model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "You did not send any body data!");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            model.UserId = User.Identity.GetId().Value;
            bool validPassword = usersService.UpdatePassword(model);
            if (!validPassword)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The password does not match.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
        }

        [Route("api/users/createpassword"), HttpPut]
        public HttpResponseMessage CreatePassword(UserPasswordUpdateRequest model)
        {
            model.UserId = User.Identity.GetId().Value;
            usersService.CreatePassword(model);
            return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
        }

        [Route("api/users/passwordnullcheck"), HttpPut]
        public HttpResponseMessage PasswordNullCHeck()
        {
            bool passwordIsNull = usersService.PasswordNullCheck(User.Identity.GetId().Value);
            if (passwordIsNull)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Password is null");
            }
            return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
        }
    }
}


