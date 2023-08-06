/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using DemoProject.Common.Enums;
using DemoProject.DataAccess;
using DemoProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Repository
{
    public class PagePermissionRepository : IPagePermissionRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DPDbContext _dpDbContext;
        
        public PagePermissionRepository(IUnitOfWork unitOfWork, DPDbContext dpDbContext)
        {
            _unitOfWork = unitOfWork;
            _dpDbContext = dpDbContext;
        }

        #region "Public methods"
        public async Task<PagePermission> AddPagePermission(PagePermission pagePermission)
        {
            await _unitOfWork.Repository<PagePermission>().InsertAsync(pagePermission);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return pagePermission;
            }

            return null;
        }

        public async Task<List<PagePermission>> GetAllPagePermissionByRoleID(int userRoleID)
        {
            string sql = $@"  select pp.* from {nameof(RolePagePermissionMapping)} as rppm
                        left join {nameof(PagePermission)} as pp on rppm.PagePermissionID = pp.PagePermissionID 
                        where {nameof(RolePagePermissionMapping.RoleID)} = {0} ";
            sql = string.Format(sql, userRoleID);
            List<PagePermission> lstPagePermission = await _unitOfWork.RawSqlQueryAsync<PagePermission>(sql);

            return lstPagePermission;
        }

        #endregion

    }

    public interface IPagePermissionRepository
    {
        Task<PagePermission> AddPagePermission(PagePermission pagePermission);
        Task<List<PagePermission>> GetAllPagePermissionByRoleID(int userRoleID);
    }
}
