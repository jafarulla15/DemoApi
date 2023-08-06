/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using DemoProject.Models;
using DemoProject.ServiceManager;
using Microsoft.AspNetCore.Mvc;

namespace DemoProject.Api.Controllers
{
    [Route("api/Security")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityServiceManager _securitySM;
        public SecurityController(ISecurityServiceManager securitySM)
        {
            this._securitySM = securitySM;
        }

        /// <summary>
        /// Authorized User Login
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     {
        ///        "Email": "test@gmail.com",
		///			"Password": "123456",
		///			"SSOLogin": false
        ///     }
        ///
        /// </remarks>
        [HttpPost("Login")]
        public async Task<ResponseMessage> Login(RequestMessage requestMessage)
        {
            return await _securitySM.Login(requestMessage, this.HttpContext, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
        }

        /// <summary>
        /// Register New User
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     {
        ///          "RequestObj": {
        ///           "FirstName": "jafar",
        ///           "LastName": "ulla",
        ///           "Email": "jafar@inneed.cloud",
	    ///          "PhoneNumber": "123456",
        ///          "Password": "123",
        ///          },
        ///          "PageRecordSize": 0,
        ///          "PageNumber": 0,
        ///         "UserID": 0
        ///      }
    ///
    /// </remarks>
    [HttpPost("Register")]
        public async Task<ResponseMessage> Register(RequestMessage requestMessage)
        {
            return await _securitySM.Register(requestMessage, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
        }

        /// <summary>
        /// Logout Currect Session
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task<ResponseMessage> Logout(RequestMessage requestMessage)
        {
            return await _securitySM.Logout(requestMessage, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
        }
    }
}
