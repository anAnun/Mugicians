using Organizer.Interfaces;
using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Security;

namespace Organizer.Services
{
    public class UsersService : IUsersService
    {
        private IAuthenticationService authenticationService;
        private IDataProvider dataProvider;

        public UsersService(IAuthenticationService authService, IDataProvider dataProvider)
        {
            authenticationService = authService;
            this.dataProvider = dataProvider;
        }



        public bool LogIn(UserLogInRequest model)
        {
            int Id = 0;
            string passwordHash = null;
            dataProvider.ExecuteCmd(
                "Users_GetPasswordFromEmail",
                inputParamMapper: parameters =>
                {
                    parameters.AddWithValue("@Email", model.Email);
                },
                singleRecordMapper: (reader, resultsSetNumber) =>
                {
                    Id = (int)reader["Id"];
                    passwordHash = (string)reader["password"];
                });
            bool isSuccessful = BCrypt.Net.BCrypt.Verify(model.Password, passwordHash);
            if (isSuccessful)
            {
                FormsAuthentication.SetAuthCookie(Convert.ToString(Id), true);
            }
            return isSuccessful;
        }
        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        public UserWithRole GetCurrentUser(string id)
        {
            var convertStr = Convert.ToInt32(id);
            UserWithRole result = new UserWithRole();
            dataProvider.ExecuteCmd(
                "Users_GetCurrentUser",
                inputParamMapper: parameters =>
                {
                    parameters.AddWithValue("@Id", convertStr);
                },
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    result.Id = (int)reader["Id"];
                    result.UserName = (string)reader["UserName"];
                    result.Email = (string)reader["Email"];
                    result.UserTypeId = (int)reader["UserTypeId"];
                });
            return result;
        }






        //public void LogOut()
        //{
        //    authenticationService.LogOut();
        //}
        public List<User> GetAll()
        {
            List<User> results = new List<User>();
            dataProvider.ExecuteCmd(
                "Users_GetAll",
                inputParamMapper: null,
                singleRecordMapper: (reader, resultsSetNumber) =>
                {
                    User user = new User();
                    user.Id = (int)reader["Id"];
                    user.UserName = (string)reader["UserName"];
                    user.Email = (string)reader["Email"];
                    user.UserTypeId = (int)reader["UserTypeId"];
                    user.DateCreated = (DateTime)reader["DateCreated"];
                    user.DateModified = (DateTime)reader["DateModified"];
                    results.Add(user);
                });
            return results;
        }

        public int Create(UserCreateRequest model)
        {
            int userId = 0;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            try
            {
                dataProvider.ExecuteNonQuery(
                    "Users_Create",
                    inputParamMapper: (parameters) =>
                    {
                        parameters.AddWithValue("@UserName", model.UserName);
                        parameters.AddWithValue("@Email", model.Email);
                        parameters.AddWithValue("@UserTypeId", model.UserTypeId);
                        parameters.AddWithValue("@Password", passwordHash);
                        parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    },
                    returnParameters: (parameters) =>
                    {
                        userId = (int)parameters["@Id"].Value;
                    });
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                throw new DuplicateUserException();
            }

            UserAuthData userAuthData = new UserAuthData();
            userAuthData.Id = userId;
            authenticationService.LogIn(userAuthData, false);

            return userId;
        }
        
        public void Update(UserUpdateRequest model)
        {
            dataProvider.ExecuteNonQuery(
                "Users_Update",
                inputParamMapper: (parameters) =>
                {
                    parameters.AddWithValue("@Id", model.Id);
                    parameters.AddWithValue("@UserName", model.UserName);
                    parameters.AddWithValue("@Email", model.Email);
                    parameters.AddWithValue("@UserTypeId", model.UserTypeId);
                },
                returnParameters: null);
        }
        //public void UpdateUserTypeId(UserTypeUpdateRequest model)
        //{
        //    dataProvider.ExecuteNonQuery(
        //        "Users_GoogleUserTypeIdUpdate",
        //        inputParamMapper: (parameters) =>
        //        {
        //            parameters.AddWithValue("@Id", model.Id);
        //            parameters.AddWithValue("@UserTypeId", model.UserTypeId);
        //        },
        //        returnParameters: null);
        //}
        public void Delete(int id)
        {
            dataProvider.ExecuteNonQuery(
                "Users_Delete",
                inputParamMapper: (parameters) =>
                {
                    parameters.AddWithValue("@Id", id);
                },
                returnParameters: null);
        }
        public User GetById(int id)
        {
            User result = new User();
            dataProvider.ExecuteCmd(
                "Users_GetById",
                inputParamMapper: parameters =>
                {
                    parameters.AddWithValue("@Id", id);
                },
                singleRecordMapper: (reader, resultsSetNumber) =>
                {
                    result.Id = (int)reader["Id"];
                    result.UserName = (string)reader["UserName"];
                    result.Email = (string)reader["Email"];
                    result.UserTypeId = reader.GetSafeInt32Nullable("UserTypeId");
                    result.DateCreated = (DateTime)reader["DateCreated"];
                    result.DateModified = (DateTime)reader["DateModified"];
                });
            return result;
        }

        public UserWithRole GetCurrentUser(int id)
        {
            UserWithRole result = new UserWithRole();
            dataProvider.ExecuteCmd(
                "Users_GetCurrentUser",
                inputParamMapper: parameters =>
                {
                    parameters.AddWithValue("@Id", id);
                },
                singleRecordMapper: (reader, resultsSetNumber) =>
                {
                    result.Id = (int)reader["Id"];
                    result.UserName = (string)reader["UserName"];
                    result.Email = (string)reader["Email"];
                    result.UserTypeId = reader.GetSafeInt32Nullable("UserTypeId");
                    result.Role = reader.GetSafeInt32Nullable("Role");
                    result.DateCreated = (DateTime)reader["DateCreated"];
                    result.DateModified = (DateTime)reader["DateModified"];
                });
            return result;
        }

        //public UserWithRole GetCurrentUser(int id)
        //{
        //    UserWithRole result = new UserWithRole();
        //    dataProvider.ExecuteCmd(
        //        "Users_GetCurrentUser",
        //        inputParamMapper: parameters =>
        //        {
        //            parameters.AddWithValue("@Id", id);
        //        },
        //        singleRecordMapper: (reader, resultsSetNumber) =>
        //        {
        //            result.Id = (int)reader["Id"];
        //            result.FirstName = (string)reader["FirstName"];
        //            result.LastName = (string)reader["LastName"];
        //            result.Email = (string)reader["Email"];
        //            result.UserTypeId = reader.GetSafeInt32Nullable("UserTypeId");
        //            result.UserTypeName = (string)reader["UserTypeName"];
        //            result.Role = reader.GetSafeInt32Nullable("Role");
        //            result.AvatarUrl = reader["AvatarUrl"] as string ?? default(string);
        //            result.DateCreated = (DateTime)reader["DateCreated"];
        //            result.DateModified = (DateTime)reader["DateModified"];
        //            result.DisplayName = reader.GetSafeString("DisplayName");
        //        });
        //    return result;
        //}

        //public string GetProfileById(int id, int viewersUserId)
        //{
        //    var jsonResult = new StringBuilder();
        //    dataProvider.ExecuteCmd(
        //        "UserProfile_GetById",
        //        inputParamMapper: parameters =>
        //        {
        //            parameters.AddWithValue("@Id", id);
        //            parameters.AddWithValue("@ViewersUserId", viewersUserId);
        //        },
        //        singleRecordMapper: (reader, resultSetId) =>
        //        {
        //            jsonResult.Append(reader.GetString(0));
        //        });
        //    return jsonResult.ToString();
        //}
        //public void UpdateProfile(UserProfileUpdateRequest model)
        //{
        //    var json = JsonConvert.SerializeObject(model);
        //    dataProvider.ExecuteNonQuery(
        //        "UserProfile_Update",
        //        inputParamMapper: (parameters) =>
        //        {
        //            parameters.AddWithValue("@Data", json);
        //        },
        //        returnParameters: null);
        //}

        //public bool GoogleLogin(GoogleLogInRequest model)
        //{
        //    bool userAuthenticated = false;
        //    int userId = 0;

        //    //TODO: Update with RecruitHub Client Id
        //    string googleClientId = "58772775873-oma31jtiqhph7os62h7i9a37makcilfr.apps.googleusercontent.com";
        //    string gapiRespObject;
        //    string gapiAuthUrl = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=";
        //    HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(gapiAuthUrl + model.GoogleToken);
        //    webReq.Method = "GET";
        //    HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();
        //    using (Stream stream = webResp.GetResponseStream())
        //    {
        //        StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
        //        gapiRespObject = reader.ReadToEnd();
        //    }

        //    var gapiRespString = (JObject)JsonConvert.DeserializeObject(gapiRespObject);
        //    string authEmail = gapiRespString["email"].Value<string>();
        //    string authAud = gapiRespString["aud"].Value<string>();
        //    string authFirstName = gapiRespString["given_name"].Value<string>();
        //    string authLastName = gapiRespString["family_name"].Value<string>();
        //    string authPassword = gapiRespString["sub"].Value<string>();

        //    if (authAud == googleClientId)
        //    {
        //        userAuthenticated = true;

        //        dataProvider.ExecuteCmd(
        //        "Users_GoogleLogin",
        //        inputParamMapper: (parameters) =>
        //        {
        //            parameters.AddWithValue("@Email", authEmail);
        //            parameters.AddWithValue("@FirstName", authFirstName);
        //            parameters.AddWithValue("@LastName", authLastName);
        //            parameters.AddWithValue("@UserTypeId", (object)DBNull.Value);
        //            parameters.AddWithValue("@Password", authPassword);
        //        },
        //        singleRecordMapper: (reader, resultsSetNumber) =>
        //        {
        //            userId = (int)reader["Id"];
        //        });

        //        UserAuthData userAuthData = new UserAuthData()
        //        {
        //            Id = userId
        //        };
        //        authenticationService.LogIn(userAuthData, true);
        //    }
        //    return userAuthenticated;
        //}

        //public Dictionary<int, UserAvatarResponse> GetUserAvatarsByIds(int[] ids)
        //{
        //    Dictionary<int, UserAvatarResponse> result = new Dictionary<int, UserAvatarResponse>();

        //    dataProvider.ExecuteCmd(
        //        "UserAvatar_GetByIds",
        //        inputParamMapper: p =>
        //        {
        //            p.AddWithValue("@Ids", JsonConvert.SerializeObject(ids));
        //        },
        //        singleRecordMapper: (reader, resultSetIndex) =>
        //        {
        //            UserAvatarResponse avatar = new UserAvatarResponse();
        //            avatar.Id = (int)reader["Id"];
        //            avatar.AvatarUrl = reader["AvatarUrl"] as string ?? default(string);
        //            avatar.UserTypeId = (int)reader["UserTypeId"];
        //            avatar.FullName = (string)reader["FullName"];
        //            result.Add(avatar.Id, avatar);
        //        });

        //    return result;
        //}

        //public void UpdateAvatarUrl(int id, UsersUpdateAvatarUrlRequest url)
        //{
        //    dataProvider.ExecuteNonQuery(
        //        "Users_UpdateAvatarUrl",
        //         inputParamMapper: parameters =>
        //         {
        //             parameters.AddWithValue("@Id", id);
        //             parameters.AddWithValue("@Url", url.Url);
        //         },
        //         returnParameters: null
        //         );
        //}

        public bool UpdatePassword(UserPasswordUpdateRequest model)
        {
            bool passwordUpdateResult = false;
            string passwordHash = null;

            dataProvider.ExecuteCmd(
                "Users_GetPasswordHash",
                inputParamMapper: parameters =>
                {
                    parameters.AddWithValue("@Id", model.Id);
                },
                singleRecordMapper: (reader, resultsSetNumber) =>
                {
                    passwordHash = (string)reader["Password"];
                });

            bool validPassword = BCrypt.Net.BCrypt.Verify(model.Password, passwordHash);
            if (validPassword)
            {
                string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

                dataProvider.ExecuteNonQuery(
                    "Users_PasswordUpdate",
                    inputParamMapper: parameters =>
                    {
                        parameters.AddWithValue("@Id", model.Id);
                        parameters.AddWithValue("@NewPassword", newPasswordHash);
                    },
                    returnParameters: null);

                passwordUpdateResult = true;
            }
            return passwordUpdateResult;
        }

        public void CreatePassword(UserPasswordUpdateRequest model)
        {
            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            dataProvider.ExecuteNonQuery(
                "Users_PasswordUpdate",
                inputParamMapper: parameters =>
                {
                    parameters.AddWithValue("@Id", model.Id);
                    parameters.AddWithValue("@NewPassword", newPasswordHash);
                },
                returnParameters: null);
        }

        public bool PasswordNullCheck(int Id)
        {
            bool passwordIsNull = false;
            string password = "";

            dataProvider.ExecuteCmd(
                "Users_GetPasswordHash",
                inputParamMapper: parameters =>
                {
                    parameters.AddWithValue("@Id", Id);
                },
                singleRecordMapper: (reader, resultsSetNumber) =>
                {
                    password = (string)reader["Password"];
                });

            if (password == null)
            {
                passwordIsNull = true;
            }
            return passwordIsNull;
        }

        //public void UpdateBasicInfo(UserUpdateBasicInfoRequest model)
        //{
        //    dataProvider.ExecuteNonQuery(
        //        "Users_UpdateBasicInfo",
        //        inputParamMapper: param =>
        //        {
        //            param.AddWithValue("@Id", model.Id);
        //            param.AddWithValue("@DisplayName", model.DisplayName);
        //            param.AddWithValue("@AvatarUrl", model.AvatarUrl);
        //        });
        //}
    }
}