/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using DemoProject.Common.Constants;
using DemoProject.Common.Enums;
using DemoProject.DataAccess;
using DemoProject.Models;
using DemoProject.Services;
using DemoProject.Utilities;
using DemoProject.Utilities.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static DemoProject.Common.Enums.Enums;

namespace DemoProject.ServiceManager
{
    public class SecurityServiceManager : ISecurityServiceManager
    {
        private readonly ISecurityService _securityService;
        private readonly ISystemUserService _systemUserService;
        private readonly IAccessTokenService _accessTokenService;
        private readonly ILoginInfoService _loginInfoService;
        private readonly IUserSessionService _userSessionService;
        private readonly IMailService _mailService;
        private readonly IAuditLogService _auditLogService;
        private readonly IExceptionLogService _exceptionLog;

        public SecurityServiceManager(ISecurityService securityService, ISystemUserService systemUserService, IAccessTokenService accessTokenService
            , ILoginInfoService loginInfoService, IAuditLogService auditLogService, IUserSessionService userSessionService, IMailService mailService)
        {
            this._securityService = securityService;
            this._systemUserService = systemUserService;
            this._accessTokenService = accessTokenService;
            this._loginInfoService = loginInfoService;
            this._auditLogService = auditLogService;
            this._userSessionService = userSessionService;
            this._mailService = mailService;
        }
        
        /// <summary>
        /// Login Method
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="httpContext"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> Login(RequestMessage requestMessage, Microsoft.AspNetCore.Http.HttpContext httpContext, string controllerName, string actionName)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            VMLogin objVMLogin = new VMLogin();
            try
            {
                try
                {
                    objVMLogin = JsonConvert.DeserializeObject<VMLogin>(requestMessage.RequestObj.ToString());
                }
                catch (Exception ex)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = WarningMessage.InvalidUserNameOrPassword;
                    return responseMessage;
                }

                // 1. Check data validation
                if (objVMLogin == null || string.IsNullOrEmpty(objVMLogin.Email) || string.IsNullOrEmpty(objVMLogin.Password))
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = WarningMessage.InvalidUserNameOrPassword;
                    return responseMessage;
                }

                // 2. SSO login works
                if (objVMLogin.SSOLogin)
                {
                    //NOTE: SSO user track
                }
                else
                {
                    // 3. IF no user with the email and password, then invalid user.
                    SystemUser existingSystemUser = await _systemUserService.GetSystemUserByEmailAndPassword(objVMLogin.Email, objVMLogin.Password);
                    //if (existingSystemUser == null || !BCrypt.Net.BCrypt.Verify(objVMLogin.Password, existingSystemUser.Password))
                    if (existingSystemUser == null)
                    {
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                        responseMessage.Message = "Invalid username or password";
                        return responseMessage;
                    }
                    else
                    {
                        objVMLogin.FirstName = existingSystemUser.FirstName;
                        objVMLogin.LastName = existingSystemUser.LastName;
                        objVMLogin.PhoneNumber = existingSystemUser.PhoneNumber;
                        objVMLogin.SystemUserID = existingSystemUser.SystemUserID;
                        objVMLogin.RoleID = existingSystemUser.RoleID;
                    }
                }

                // 4. Get Page permission of the user by user ROLE, (which pages will to show in Frontend CLient side.)
                if (objVMLogin != null && objVMLogin.RoleID > 0)
                {
                    objVMLogin.lstPagePermissions = await _securityService.GetAllPagePermissionByRoleID(objVMLogin.RoleID);
                }

                // 5. Get and Set Role Name to show in client side.
                objVMLogin.RoleName = await _systemUserService.GetRoleNameByRoleID(objVMLogin.RoleID);

                // 6. Get/Set token (and Insert session into session table)
                objVMLogin.Token = await _accessTokenService.GetAccessTokenForUser(objVMLogin.SystemUserID, objVMLogin.RoleID);

                // 7. Save into LoginInfo table for Login Log
                await _loginInfoService.SaveLoginInfo("", objVMLogin.SystemUserID,
                        "", 0, "", httpContext.Request.Host.Value,
                        httpContext.Request.Scheme, "");

                // 8. Create Response message
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.Message = SuccessMessage.LoginSuccess;

                objVMLogin.Password = String.Empty;
                responseMessage.ResponseObj = objVMLogin;

                // 9. Audit Log writing.
                //ControllerContext.ActionDescriptor.ActionName
                _auditLogService.SaveAuditLogs(0, "", actionName,
                   (int)Enums.LogActions.Reviewed, (int)Enums.UserAccessRight.Anonymous,
                   (int)Enums.UserType.APPSUSER, requestMessage, "", true, (int)Enums.LogType.SystemLog);
            }
            catch (Exception ex)
            {
                _exceptionLog.SaveExceptionLog((int)LogFixPriority.Medium, 0, ex.Message, ex.StackTrace.ToString(),
                    objVMLogin.SystemUserID, controllerName,
                     actionName, (int)Enums.ActionType.View, "SecurityServiceManager");

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Method for log out.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> Logout(RequestMessage requestMessage, string controllerName, string actionName)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                bool endSessionResult = await _userSessionService.EndUserSessionByUserID(requestMessage.UserID);
                if (endSessionResult)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                    responseMessage.Message = SuccessMessage.LogOutSuccessfully;
                }
                else
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = ErrorMessage.LogOutFailed;
                }

                // Audit Log writing.
                _auditLogService.SaveAuditLogs(0, "", actionName,
                   (int)Enums.LogActions.Update, (int)Enums.UserAccessRight.User,
                   (int)Enums.UserType.APPSUSER, requestMessage, "", true, (int)Enums.LogType.SystemLog);

                return responseMessage;
            }
            catch (Exception ex)
            {
                _exceptionLog.SaveExceptionLog((int)LogFixPriority.Medium, 0, ex.Message, ex.StackTrace.ToString(),
                requestMessage, controllerName, actionName, (int)Enums.ActionType.Update, "SecurityServiceManager");

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }


        /// <summary>
        /// Method for register user.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> Register(RequestMessage requestMessage, string controllerName, string actionName)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                // 1. Check Object exist or not
                VMRegister objVMRegister = JsonConvert.DeserializeObject<VMRegister>(requestMessage.RequestObj.ToString());
                if (objVMRegister == null)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = ErrorMessage.Invaliddatafound;
                    return responseMessage;
                }
                else
                {
                    // 2. Object validation check.
                    string validationMessage = await _systemUserService.CheckedValidation(objVMRegister);
                    if (string.IsNullOrEmpty(validationMessage))
                    {
                        //** Validation OK

                        // 3. Save system user into DB.
                        SystemUser objSystemUser = await _systemUserService.SaveSystemUser(objVMRegister);
                        if (objSystemUser != null)
                        {
                            // success
                            objSystemUser.Password = string.Empty;
                            responseMessage.ResponseObj = objSystemUser;
                            responseMessage.Message = SuccessMessage.RegisterSuccessfully;
                            responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                            // ** Send Mail
                             _mailService.SendEmailAsync(new MailRequest() { ToEmail = objVMRegister.Email, Subject = "@Thank you from Demo Project", Body = $"Thank you for being with us" });
                        }
                        else
                        {
                            // failed
                            responseMessage.ResponseObj = null;
                            responseMessage.Message = ErrorMessage.RegisterFailed;
                            responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                        }
                    }
                    else
                    {
                        // Validation check failed response
                        responseMessage.Message = validationMessage;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    }
                }

                // Audit Log writing.
                _auditLogService.SaveAuditLogs(0, "", actionName,
                   (int)Enums.LogActions.insert, (int)Enums.UserAccessRight.Anonymous,
                   (int)Enums.UserType.APPSUSER, requestMessage, "", true, (int)Enums.LogType.SystemLog);
            }
            catch (Exception ex)
            {
                _exceptionLog.SaveExceptionLog((int)LogFixPriority.Medium, 0, ex.Message, ex.StackTrace.ToString(),
                requestMessage, controllerName, actionName, (int)Enums.ActionType.Insert, "SecurityServiceManager");

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

    }

    public interface ISecurityServiceManager
    {
        Task<ResponseMessage> Login(RequestMessage requestMessage, Microsoft.AspNetCore.Http.HttpContext httpContext, string controllerName, string actionName);
        Task<ResponseMessage> Logout(RequestMessage requestMessage, string controllerName, string actionName);
        
        /// <summary>
        /// User Register Method
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        Task<ResponseMessage> Register(RequestMessage requestMessage, string controllerName, string actionName);
    }
}
