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
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Utilities
{
    public class CommonServices : ICommonServices
    {
        public CommonServices()
        {

        }

        #region "Public Methods"

        /// <summary>
        /// Get token form http header
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public JwtSecurityToken GetTokenFromHeader(HttpContext httpContext)
        {
            var handler = new JwtSecurityTokenHandler();
            var headerToken = SubstringToken(httpContext.Request.Headers[HttpHeaders.Token]);
            var token = handler.ReadToken(headerToken) as JwtSecurityToken;

            return token;
        }

        /// <summary>
        /// Get system user id from token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public int GetSystemUserId(JwtSecurityToken token)
        {
            return Convert.ToInt32(token.Claims.First(claim => claim.Type == JwtClaim.UserId).Value);
        }

        #endregion

        #region "Private Methods"

        /// <summary>
        /// Substring token
        /// </summary>
        /// <param name="fullToken"></param>
        /// <returns></returns>
        private string SubstringToken(string fullToken)
        {
            return fullToken.Replace("Bearer ", "");
        }

        #endregion

    }

    public interface ICommonServices
    {
        JwtSecurityToken GetTokenFromHeader(HttpContext httpContext);
        int GetSystemUserId(JwtSecurityToken token);
    }
}
