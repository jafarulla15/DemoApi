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
using DemoProject.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace DemoProject.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly ICommonServices _commonServices;
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserSessionRepository _sessionRepository;

        public UserSessionService(DPDbContext ctx, ICommonServices commonServices, IUserSessionRepository userSessionRepository, IUnitOfWork unitOfWork, IUserSessionRepository sessionRepository)
        {
            //this._dpDbContext = ctx;
            _commonServices = commonServices;
            this._userSessionRepository = userSessionRepository;
            _unitOfWork = unitOfWork;
            _sessionRepository = sessionRepository;
        }

        /// <summary>
        /// Get User with Token(if valid)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<int> GetUserIDWithValidToken(JwtSecurityToken token)
        {
            try
            {
                int userID = _commonServices.GetSystemUserId(token);
                UserSession objUserSession = await GetUserSessionBySystemUserId(userID);

                if (objUserSession != null)
                {
                    TimeSpan ts = DateTime.Now - objUserSession.SessionEnd.Value;
                    int min = ts.Minutes;
                    if (min <= CommonConstant.SessionExpired)
                    {
                        return userID;
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get User session by system user ID
        /// </summary>
        /// <param name="systemUserId"></param>
        /// <returns></returns>
        public async Task<UserSession> GetUserSessionBySystemUserId(int systemUserId)
        {
            try
            {
                UserSession userSession = await _userSessionRepository.GetUserSessionBySystemUserId(systemUserId);
                return userSession;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Update user session expire time by system user id
        /// </summary>
        /// <param name="systemUserId"></param>
        /// <returns></returns>
        public async Task<UserSession> UpdateUserSessionExpireTimeBySystemUserId(int systemUserId)
        {
            try
            {
                // 1. Get user session
                //UserSession objUserSession = await _dpDbContext.UserSession.AsNoTracking().OrderBy(u => u.UserSessionID)
                //    .LastOrDefaultAsync(x => x.SystemUserID == systemUserId && x.Status == (int)Enums.Status.Active && x.SessionEnd > DateTime.Now);

                UserSession objUserSession = await GetUserSessionBySystemUserId(systemUserId);

                // 2. update session
                if (objUserSession != null)
                {
                    DateTime dateTime = DateTime.Now.AddMinutes(CommonConstant.SessionExpired);
                    objUserSession.SessionEnd = dateTime;
                    //objRequestMessageNew.RequestObj = JsonConvert.SerializeObject(objUserSession);

                    await _userSessionRepository.SaveUserSession(objUserSession, systemUserId);
                }


                return objUserSession;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// End all session of an user by user id
        /// </summary>
        /// <param name="systemUserID"></param>
        /// <returns></returns>
        public async Task<bool> EndUserSessionByUserID(int systemUserID)
        {
            try
            {
                return await _sessionRepository.EndAllSessionByUserID(systemUserID);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public interface IUserSessionService
    {
        Task<int> GetUserIDWithValidToken(JwtSecurityToken token);
        Task<UserSession> UpdateUserSessionExpireTimeBySystemUserId(int systemUserId);
        Task<bool> EndUserSessionByUserID(int systemUserID);
    }



}
