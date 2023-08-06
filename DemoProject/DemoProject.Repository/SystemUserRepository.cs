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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Repository
{
    public class SystemUserRepository : ISystemUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DPDbContext _dpDbContext;
        public SystemUserRepository(IUnitOfWork unitOfWork, DPDbContext dpDbContext)
        {
            _unitOfWork = unitOfWork;
            _dpDbContext = dpDbContext;
        }

        public async Task<SystemUser> AddSystemUser(SystemUser user)
        {
            await _unitOfWork.Repository<SystemUser>().InsertAsync(user);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return user;
            }

            return null;
        }

        public async Task<SystemUser> GetSystemUserByEmailAndPassword(string email, string password)
        {
            SystemUser existingSystemUser = await _dpDbContext.SystemUser.Where(x => x.Email == email && x.Status == (int)Enums.Status.Active).AsNoTracking().FirstOrDefaultAsync();

            if (existingSystemUser == null || !BCrypt.Net.BCrypt.Verify(password, existingSystemUser.Password))
            {
                return null;
            }

            return existingSystemUser;
        }

        public async Task<List<VMUserAction>> GetAllActionsMappedWithTheUserByActionKey(int userID, string actionKey)
        {
            string sql = $@"  select ac.ActionKey, ac.ActionName from {nameof(RoleActionMapping)} as ram
                        left join {nameof(Actions)} as ac on ram.ActionID = ac.ActionID 
                        where {nameof(RoleActionMapping.RoleID)} = {0} and  {nameof(Actions.ActionKey)} = '{1}' ";

            sql = string.Format(sql, userID, actionKey);

            //TODO: uncomment below code
            // return  _dpDbContext.RawSqlQuery<VMUserAction>(sql).ToList();
            return null;
        }

    }

    public interface ISystemUserRepository
    {
        Task<SystemUser> AddSystemUser(SystemUser user);
        Task<SystemUser> GetSystemUserByEmailAndPassword(string email, string password);
        Task<List<VMUserAction>> GetAllActionsMappedWithTheUserByActionKey(int userID, string actionKey);
    }



}
