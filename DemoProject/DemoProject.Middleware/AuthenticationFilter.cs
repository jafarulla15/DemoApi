/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using DemoProject.Common.Constants;
using DemoProject.Common.Enums;
using DemoProject.Models;
using DemoProject.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using DemoProject.Utilities;

namespace DemoProject.Middleware
{
    public class AuthenticationFilter
    {
        private readonly RequestDelegate _next;
        //private readonly ISecurityService _securityService;
        //private readonly ICommonServices _commonServices;
        //private readonly IUserSessionService _userSessionServices;

        public AuthenticationFilter(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IAccessTokenService accessTokenService, ISecurityService securityService, ICommonServices commonServices, IUserSessionService userSessionServices)
        {
            string url = httpContext.Request.Path;

            //NOTE: if public URL, no need to check authentication. any one can access.
            if (await securityService.IsPublicURL(url))
            {
                // 1. If Public URL, no need to check authentication. any one can access.
                await _next(httpContext);
            }
            else
            {
                // ** 2. Private URL, then check

                //NOTE: if no token in header, then unauthenticated request.
                var handler = new JwtSecurityTokenHandler();
                string headerToken = SubstringToken(httpContext.Request.Headers[HttpHeaders.Token]);
                if (! string.IsNullOrEmpty(headerToken))
                {
                    var token = handler.ReadToken(headerToken) as JwtSecurityToken;

                    if (token != null)
                    {
                        int userID = await userSessionServices.GetUserIDWithValidToken(token);
                        if (userID > 0)
                        {
                            RequestMessage objRequestMessage = new RequestMessage();
                            ResponseMessage objResponseMessage = new ResponseMessage();
                            using (StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8))
                            {
                                var obj = await reader.ReadToEndAsync();
                                objRequestMessage = JsonConvert.DeserializeObject<RequestMessage>(obj);
                                objRequestMessage.UserID = userID;
                            }

                            //NOTE: TO set user id in request body, that will be used on user tracking.                           
                            var requestData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objRequestMessage));
                            httpContext.Request.Body = new MemoryStream(requestData);
                            httpContext.Request.ContentLength = httpContext.Request.Body.Length;

                            await _next(httpContext);
                        }
                        else
                        {
                            httpContext.Response.ContentType = "application/json";
                            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            // TODO: 
                            // await httpContext.Response.WriteAsJsonAsync("Session expired.");
                        }
                    }
                    else
                    {
                        httpContext.Response.ContentType = "application/json";
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        // TODO: 
                        // await httpContext.Response.WriteAsJsonAsync("Unauthorize");
                    }
                }
                else
                {
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    // TODO: 
                    // await httpContext.Response.WriteAsJsonAsync("Unauthorize");
                }

            }
        }

        private string SubstringToken(string fullToken)
        {
            if (string.IsNullOrEmpty(fullToken))
            {
                return string.Empty;
            }
            return fullToken.Replace("Bearer ", "");
        }
    }

    #region "Extension CLass"

    //NOTE: Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationFilterExtensions
    {
        public static IApplicationBuilder UseAuthenticationFilter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationFilter>();
        }
    }

    #endregion
}
