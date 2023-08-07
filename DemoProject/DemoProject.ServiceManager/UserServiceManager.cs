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
using static DemoProject.Common.Enums.Enums;

namespace DemoProject.ServiceManager
{
    public  class UserServiceManager : IUserServiceManager
    {
        private readonly ISecurityService _securityService;
        private readonly ISystemUserService _systemUserService;
        private readonly IAccessTokenService _accessTokenService;
        private readonly ILoginInfoService _loginInfoService;
        private readonly IAuditLogService _auditLogService;
        private readonly IExceptionLogService _exceptionLog;

        public UserServiceManager(ISecurityService securityService, ISystemUserService systemUserService, IAccessTokenService accessTokenService, ILoginInfoService loginInfoService, IAuditLogService auditLogService, IExceptionLogService exceptionLog)
        {
            this._securityService = securityService;
            this._systemUserService = systemUserService;
            this._accessTokenService = accessTokenService;
            this._loginInfoService = loginInfoService;
            this._auditLogService = auditLogService;
            this._exceptionLog = exceptionLog;
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllUsers(RequestMessage requestMessage, string controllerName, string actionName)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                // 1. Get all active system users
                List<SystemUser> lstSystemUser = await  _systemUserService.GetAllActiveSystemUser();

                // 2. Create Response message
                if (lstSystemUser == null)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = ErrorMessage.FailedGetAllUsers;
                    return responseMessage;
                }

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.Message = SuccessMessage.LoginSuccess;
                responseMessage.ResponseObj = lstSystemUser;

                // 3. Audit Log writing.
                _auditLogService.SaveAuditLogs(0, "", "GetAllUsers",
                   (int)Enums.LogActions.View, (int)Enums.UserAccessRight.Admin,
                   (int)Enums.UserType.APPSUSER, requestMessage.ToString(), "", true, (int)Enums.LogType.SystemLog);
            }
            catch (Exception ex)
            {
                _exceptionLog.SaveExceptionLog((int)LogFixPriority.Medium, 0, ex.Message, ex.StackTrace.ToString(),
                  requestMessage.UserID, controllerName,
                     actionName, (int)Enums.ActionType.View, "SecurityServiceManager");

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }
    }
    public interface IUserServiceManager
    {
        Task<ResponseMessage> GetAllUsers(RequestMessage requestMessage, string controllerName, string actionName);
    }
}
