//using Organizer.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Web;

//namespace Organizer.Services
//{
//        public class OwinAuthenticationService : IAuthenticationService
//        {
//            private static string _title = null;

//            static OwinAuthenticationService()
//            {
//                _title = GetApplicationName();
//            }

//            public OwinAuthenticationService()
//            {

//            }

//            public void LogIn(IUserAuthData user, bool rememberMe, params Claim[] extraClaims)
//            {
//                ClaimsIdentity identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie
//                                                                , ClaimsIdentity.DefaultNameClaimType
//                                                                , ClaimsIdentity.DefaultRoleClaimType);

//                identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider"
//                                    , _title
//                                    , ClaimValueTypes.String));

//                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));

//                identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString(), ClaimValueTypes.String));

//                if (user.Roles != null && user.Roles.Any())
//                {
//                    foreach (string singleRole in user.Roles)
//                    {
//                        identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, singleRole, ClaimValueTypes.String));
//                    }

//                }

//                identity.AddClaims(extraClaims);

//                if (rememberMe)
//                {
//                    AuthenticationProperties props = new AuthenticationProperties
//                    {
//                        IsPersistent = true,
//                        IssuedUtc = DateTime.UtcNow,
//                        ExpiresUtc = DateTime.UtcNow.AddDays(60),
//                        AllowRefresh = true
//                    };
//                    HttpContext.Current.GetOwinContext().Authentication.SignIn(props, identity);
//                }
//                else
//                {
//                    AuthenticationProperties props = new AuthenticationProperties
//                    {
//                        IsPersistent = false
//                    };
//                    HttpContext.Current.GetOwinContext().Authentication.SignIn(props, identity);
//                }

//            }

//            public void LogOut()
//            {
//                Microsoft.Owin.IOwinContext owinContext = System.Web.HttpContext.Current.Request.GetOwinContext();
//                IEnumerable<AuthenticationDescription> authenticationTypes = owinContext.Authentication.GetAuthenticationTypes();
//                owinContext.Authentication.SignOut(authenticationTypes.Select(o => o.AuthenticationType).ToArray());

//                //HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
//            }


//            public IUserAuthData GetCurrentUser()
//            {
//                return HttpContext.Current.User.Identity.GetCurrentUser();
//            }

//            private static string GetApplicationName()
//            {
//                var entryAssembly = Assembly.GetExecutingAssembly();

//                var titleAttribute = entryAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false).FirstOrDefault() as AssemblyTitleAttribute;
//                //if (string.IsNullOrWhiteSpace(applicationTitle))
//                //{
//                //    applicationTitle = entryAssembly.GetName().Name;
//                //}
//                return titleAttribute == null ? entryAssembly.GetName().Name : titleAttribute.Title;

//            }
//        }
//    }
//}