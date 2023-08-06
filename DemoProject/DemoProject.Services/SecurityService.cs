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
using DemoProject.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPagePermissionRepository _pagePermissionRepository;

        public SecurityService(IServiceScopeFactory serviceScopeFactor, IUnitOfWork unitOfWork)
        {
            this._serviceScopeFactory = serviceScopeFactor;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Check if this URL is public or Private
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> IsPublicURL(string url)
        {
            //TODO: If public URLs are many in number, then keep public URLs in DB and check from DB

            if (url?.ToLower() == CommonPath.loginUrl.ToLower() || url?.ToLower() == CommonPath.registerUrl.ToLower())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get all Permitted page list by Role ID
        /// </summary>
        /// <param name="userRoleID"></param>
        /// <returns></returns>
        public async Task<List<PagePermission>> GetAllPagePermissionByRoleID(int userRoleID)
        {
            return await _pagePermissionRepository.GetAllPagePermissionByRoleID(userRoleID);
        }

        //#region "Private Methods"
        //private bool CheckedValidation(VMRegister objVMRegister, ResponseMessage responseMessage)
        //{

        //    SystemUser existingSystemUser = new SystemUser();

        //    if (String.IsNullOrEmpty(objVMRegister.FirstName))
        //    {
        //        responseMessage.Message = MessageConstant.FirstNameRequired;
        //        return false;
        //    }
        //    if (String.IsNullOrEmpty(objVMRegister.LastName))
        //    {
        //        responseMessage.Message = MessageConstant.LastNameRequired;
        //        return false;
        //    }
        //    if (String.IsNullOrEmpty(objVMRegister.PhoneNumber))
        //    {
        //        responseMessage.Message = MessageConstant.PhoneRequired;
        //        return false;
        //    }
        //    if (String.IsNullOrEmpty(objVMRegister.Email))
        //    {
        //        responseMessage.Message = MessageConstant.EmailRequired;
        //        return false;
        //    }
        //    if (String.IsNullOrEmpty(objVMRegister.Password))
        //    {
        //        responseMessage.Message = MessageConstant.PasswordRequired;
        //        return false;
        //    }
        //    existingSystemUser = _crmDbContext.SystemUser.Where(x => x.PhoneNumber.ToLower() == objVMRegister.PhoneNumber.ToLower() && x.Email.ToLower() == objVMRegister.Email.ToLower() && x.Status == (int)Enums.Status.Active).AsNoTracking().FirstOrDefault();
        //    if (existingSystemUser != null)
        //    {
        //        responseMessage.Message = MessageConstant.DuplicatePhoneAndEmail;
        //        return false;
        //    }
        //    //existingSystemUser = _crmDbContext.SystemUser.Where(x => x.Email == objVMRegister.Email && x.Status == (int)Enums.Status.Active).AsNoTracking().FirstOrDefault();
        //    //if (existingSystemUser != null)
        //    //{
        //    //    responseMessage.Message = MessageConstant.EmailAlreadyExist;
        //    //    return false;
        //    //}
        //    //existingSystemUser = _crmDbContext.SystemUser.Where(x => x.PhoneNumber == objVMRegister.PhoneNumber && x.Status == (int)Enums.Status.Active).AsNoTracking().FirstOrDefault();
        //    //if (existingSystemUser != null)
        //    //{
        //    //    responseMessage.Message = MessageConstant.PhoneNumberAlreadyExist;
        //    //    return false;
        //    //}

        //    return true;
        //}

        ///// <summary>
        ///// Generate Token
        ///// </summary>
        ///// <param name="systemUser"></param>
        ///// <returns></returns>
        //private string GenerateToken(SystemUser systemUser)
        //{
        //    string token = string.Empty;

        //    return token;
        //}

        //#endregion 
    }

    public interface ISecurityService
    {
        Task<bool> IsPublicURL(string url);
        Task<List<PagePermission>> GetAllPagePermissionByRoleID(int userRoleID);
    }
}
