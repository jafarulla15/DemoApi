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
    public class RoleRepository : IRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DPDbContext _dpDbContext;

        public RoleRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            //_dpDbContext = dpDbContext;
        }

        #region "Public methods"

        public async Task<List<PagePermission>> GetAllPagePermissionByRoleID(int userRoleID)
        {
            string sql = $@"  select pp.* from {nameof(RolePagePermissionMapping)}  as rppm
                        left join {nameof(PagePermission)} as pp on rppm.PagePermissionID = pp.PagePermissionID 
                        where {nameof(RolePagePermissionMapping.RoleID)} = {0} ";
            sql = string.Format(sql, userRoleID);
            List<PagePermission> lstPagePermission = await _unitOfWork.RawSqlQueryAsync<PagePermission>(sql);

            return lstPagePermission;
        }

        //public async Task<Roles> GetRoleNameByRoleID(int roleID)
        //{
        //    //Roles role = await _unitOfWork.Repository<Roles>()
        //    //         .GetFirstOrDefaultAsync(predicate: x => x.RoleID == roleID && x.Status == (int)Enums.Status.Active, disableTracking: true);


        //    return null;
        //}

            #endregion

        }

        public interface IRoleRepository
    {
        Task<List<PagePermission>> GetAllPagePermissionByRoleID(int userRoleID);
    }
}
