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
            try
            {
                return await _pagePermissionRepository.GetAllPagePermissionByRoleID(userRoleID);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public interface ISecurityService
    {
        Task<bool> IsPublicURL(string url);
        Task<List<PagePermission>> GetAllPagePermissionByRoleID(int userRoleID);
    }
}
