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
    [Route("api/SystemUser")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServiceManager _userSM;
        public UserController(IUserServiceManager userSM)
        {
            this._userSM = userSM;
        }

        /// <summary>
        /// Get All Active Users
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        [HttpPost("GetAllUsers")]
        public async Task<ResponseMessage> GetAllUsers(RequestMessage requestMessage)
        {
            return await _userSM.GetAllUsers(requestMessage, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
        }

        /// <summary>
        /// Get All Admin Users ( Only SuperAdmin are authorized for this api)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        [HttpPost("GetAllAdminUsers")]
        public async Task<ResponseMessage> GetAllAdminUsers(RequestMessage requestMessage)
        {
            return await _userSM.GetAllUsers(requestMessage, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
        }

    }
}
