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
#pragma warning disable CS8600
    public class SystemUserService : ISystemUserService
    {

        //private readonly DPDbContext _dpDbContext;
        private readonly ISystemUserRepository _systemUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SystemUserService(ISystemUserRepository systemUserRepository, IUnitOfWork unitOfWork)
        {
            _systemUserRepository = systemUserRepository;
            _unitOfWork = unitOfWork;
            //_dpDbContext = dpDbContext;
        }
        
        /// <summary>
        /// Get User- By email address and password
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<SystemUser> GetSystemUserByEmailAndPassword(string userEmail, string password)
        {
            List<SystemUser> lstexistingSystemUser = (await _unitOfWork.Repository<SystemUser>()
                 .GetAsync(predicate: x => x.Email == userEmail && x.Status == (int)Enums.Status.Active, disableTracking: true)).ToList();

            if (lstexistingSystemUser.Count > 0)
            {
                if (!BCrypt.Net.BCrypt.Verify(password, lstexistingSystemUser[0].Password))
                {
                    return null;
                }

                return lstexistingSystemUser[0];
            }

            return null;
        }

        /// <summary>
        /// Check user's access permission (Actions permission)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="actionKey"></param>
        /// <returns></returns>
        public async Task<bool> CheckPermission(int userID, string actionKey)
        {
            //Remark: to make simple, below hardcode.
            if (actionKey == "/api/Security/Logout" || actionKey == "/api/SystemUser/GetAllUsers")
            {
                return true;
            }


            List<VMUserAction> lstVMUserActions = await _systemUserRepository.GetAllActionsMappedWithTheUserByActionKey(userID, actionKey);
            if (lstVMUserActions !=null && lstVMUserActions.Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get Role name by Role ID
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public async Task<string> GetRoleNameByRoleID(int roleID)
        {
            //Roles role = await _unitOfWork.Repository<Roles>()
            //        .GetFirstOrDefaultAsync(predicate: x => x.RoleID == roleID && x.Status == (int)Enums.Status.Active, disableTracking: true);

            List<Roles> lstRole = (await _unitOfWork.Repository<Roles>()
                .GetAsync(predicate: x => x.RoleID == roleID && x.Status == (int)Enums.Status.Active, disableTracking: true)).ToList<Roles>();

            if (lstRole.Count > 0)
            {
                return lstRole[0].RoleName;
            }

            return string.Empty;
        }

        /// <summary>
        /// Save system user 
        /// </summary>
        /// <param name="objVMRegister"></param>
        /// <returns></returns>
        public async Task<SystemUser> SaveSystemUser(VMRegister objVMRegister)
        {
            try
            {
                SystemUser objSystemUser = new SystemUser();

                objSystemUser.Status = (int)Enums.Status.Active;
                objSystemUser.CreatedDate = DateTime.Now;
                objSystemUser.CreatedBy = 0;
                objSystemUser.UpdatedDate  = DateTime.Now;
                objSystemUser.UpdatedBy = 0;

                objSystemUser.FirstName = objVMRegister.FirstName;
                objSystemUser.LastName = objVMRegister.LastName;
                objSystemUser.PhoneNumber = objVMRegister.PhoneNumber;
                objSystemUser.Email = objVMRegister.Email;
                objSystemUser.Password = BCrypt.Net.BCrypt.HashPassword(objVMRegister.Password);

                objSystemUser.RoleID = 0;
                objSystemUser.IsApproved = false;
                objSystemUser.StatusOfUser = (int)Enums.Status.Active;  

                await _unitOfWork.Repository<SystemUser>().InsertAsync(objSystemUser);
                await _unitOfWork.SaveChangesAsync();

                objSystemUser.Password = string.Empty;
                return objSystemUser;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get all active system users
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemUser>> GetAllActiveSystemUser()
        {
            List<SystemUser> lstSystemUser = (await _unitOfWork.Repository<SystemUser>()
                .GetAsync(predicate: x => x.Status == (int)Enums.Status.Active, disableTracking: true)).ToList();

            foreach (SystemUser user in lstSystemUser)
            {
                user.Password = string.Empty;
            }

            return lstSystemUser;
        }

        /// <summary>
        /// Check registration object validation
        /// </summary>
        /// <param name="objVMRegister"></param>
        /// <returns></returns>
        public async Task<string> CheckedValidation(VMRegister objVMRegister)
        {
            SystemUser existingSystemUser = new SystemUser();

            if (String.IsNullOrEmpty(objVMRegister.FirstName))
            {
                return WarningMessage.FirstnameRequired;
            }
            if (String.IsNullOrEmpty(objVMRegister.Email))
            {
                return WarningMessage.EmailRequired;
            }
            if (String.IsNullOrEmpty(objVMRegister.Password))
            {
                return WarningMessage.PasswordRequired;
            }

            existingSystemUser = (await _unitOfWork.Repository<SystemUser>()
               .GetAsync(predicate: x => x.Email == objVMRegister.Email && x.Status == (int)Enums.Status.Active, disableTracking: true))
               .ToList<SystemUser>().FirstOrDefault<SystemUser>();

            if (existingSystemUser != null)
            {
                return WarningMessage.EmailAlreadyExist;
            }

            return string.Empty;
        }

    }

    public interface ISystemUserService
    {
        Task<SystemUser> GetSystemUserByEmailAndPassword(string userEmail, string password);
        Task<bool> CheckPermission(int userID, string actionKey);
        Task<string> GetRoleNameByRoleID(int roleID);
        Task<SystemUser> SaveSystemUser(VMRegister objVMRegister);
        Task<List<SystemUser>> GetAllActiveSystemUser();
        Task<string> CheckedValidation(VMRegister objVMRegister);

        //Task<string> GetRoleNameByRoleID(int roleID);
        //Task<ResponseMessage> Login(RequestMessage requestMessage);
        //Task<ResponseMessage> Logout(RequestMessage requestMessage);
        //Task<ResponseMessage> Register(RequestMessage requestMessage);
        //Task<bool> CheckPermission(int userID, string actionKey);
        //Task<SystemUser> GetSystemUserByEmailAndPassword(string userEmail, string password);
        //Task<string> CheckedValidation(VMRegister objVMRegister);
        //Task<SystemUser> SaveSystemUser(VMRegister objVMRegister);
        //Task<List<SystemUser>> GetAllActiveSystemUser();
    }
}
