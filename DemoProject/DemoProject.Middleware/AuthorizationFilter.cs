/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using DemoProject.Common.Constants;
using DemoProject.Models;
using DemoProject.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Middleware
{
    public class AuthorizationFilter
    {
        private readonly RequestDelegate _next;

        public AuthorizationFilter(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IUserSessionService _userSessionService, ISecurityService _securityService, ISystemUserService _systemUserService, ISecurityService securityService)
        {
            RequestMessage objRequestMessage = new RequestMessage();
            int userID = 0;
            string url = httpContext.Request.Path;

            //NOTE: if public URL, no need to check authentication. any one can access.
            if (await securityService.IsPublicURL(url))
            {
                // 1. If Public URL, no need to check authentication. any one can access.
                await _next(httpContext);
            }
            else
            {
                // 1: get UserID from "RequestMessage"
                //using (StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8))
                //{
                //    var obj = await reader.ReadToEndAsync();
                //    objRequestMessage = JsonConvert.DeserializeObject<RequestMessage>(obj);
                //    userID = objRequestMessage.UserID;
                //}

                // 2: Check request url permission
                if (await _systemUserService.CheckPermission(userID, url))
                {
                    // 3 : Update Expire datetime of "User Session"
                    await _userSessionService.UpdateUserSessionExpireTimeBySystemUserId(userID);

                    // 4: go to next middleware.
                    await _next(httpContext);
                }
                else
                {
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    // TODO: 
                    //  await httpContext.Response.WriteAsJsonAsync("You are unauthorized.");
                }
            }


        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthorizationFilterExtensions
    {
        public static IApplicationBuilder UseAuthorizationFilter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationFilter>();
        }
    }
}
