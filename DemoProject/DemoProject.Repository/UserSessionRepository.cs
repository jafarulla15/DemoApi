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
using Microsoft.Extensions.DependencyInjection;

namespace DemoProject.Repository
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DPDbContext _dpDbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public UserSessionRepository(IUnitOfWork unitOfWork, DPDbContext dpDbContext, IServiceScopeFactory serviceScopeFactor)
        {
            _unitOfWork = unitOfWork;
            _dpDbContext = dpDbContext;
            _serviceScopeFactory = serviceScopeFactor;
        }

        public async Task<UserSession> AddNewUserSession(UserSession userSession)
        {
            await _unitOfWork.Repository<UserSession>().InsertAsync(userSession);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return userSession;
            }

            return null;
        }

        public async Task<UserSession> GetUserSessionBySystemUserId(int systemUserId)
        {
            UserSession objUserSession = await _dpDbContext.UserSession.AsNoTracking().OrderBy(u => u.UserSessionID)
                .LastOrDefaultAsync(x => x.SystemUserID == systemUserId && x.Status == (int)Enums.Status.Active && x.SessionEnd > DateTime.Now);

            return objUserSession;
        }

        public async Task<bool > EndAllSessionByUserID(int SystemUserID)
        {
            List<UserSession> lstUserSession = await _dpDbContext.UserSession.AsNoTracking().Where(x => x.SystemUserID == SystemUserID && x.Status == (int)Enums.Status.Active).ToListAsync();
            if (lstUserSession.Count > 0)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetService<DPDbContext>();
                    foreach (var item in lstUserSession)
                    {
                        item.SessionEnd = DateTime.Now;
                        item.Status = (int)Enums.Status.Inactive;
                    }
                    db.UserSession.UpdateRange(lstUserSession);
                    await db.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        public async Task<UserSession> SaveUserSession(UserSession objUserSession, int SystemUserID)
        {
            if (objUserSession != null)
            {
                if (objUserSession.UserSessionID > 0)
                {
                    //UserSession existingUserSession = await this._dpDbContext.UserSession.AsNoTracking().FirstOrDefaultAsync(x => x.UserSessionID == objUserSession.UserSessionID && x.Status == (int)Enums.Status.Active);
                    if (objUserSession != null)
                    {
                        _dpDbContext.UserSession.Update(objUserSession);
                    }
                }
                else
                {
                    objUserSession.Status = (int)Enums.Status.Active;
                    objUserSession.CreatedDate = DateTime.Now;
                    objUserSession.CreatedBy = SystemUserID;
                    await _dpDbContext.UserSession.AddAsync(objUserSession);
                }

                await _dpDbContext.SaveChangesAsync();
            }

            return null;
        }




    }

    public interface IUserSessionRepository
    {
        Task<UserSession> AddNewUserSession(UserSession userSession);
        Task<bool> EndAllSessionByUserID(int SystemUserID);
        Task<UserSession> GetUserSessionBySystemUserId(int systemUserId);
        Task<UserSession> SaveUserSession(UserSession objUserSession, int SystemUserID);
    }
}
