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
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DPDbContext _dbContext;
        public AccessTokenService(IUnitOfWork unitOfWork, DPDbContext dbContext)
        {
            _unitOfWork = _unitOfWork;
            _dbContext = dbContext;
        }
        
        /// <summary>
        /// Get token for user
        /// </summary>
        /// <param name="systemUserID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public async Task<string> GetAccessTokenForUser(int systemUserID, int roleID)
        {
            AccessToken objAccessToken = new AccessToken();
            objAccessToken.SystemUserID = systemUserID;
            objAccessToken.RoleId = roleID;
            objAccessToken.ExpiredOn = DateTime.Now.AddDays(1);
            objAccessToken.IssuedOn = DateTime.Now;

            //for creating sesssion token
            AccessToken accessToken = await GetExistingORCreateNewToken(objAccessToken);
            return (accessToken != null) ? accessToken.Token : String.Empty;
        }

        #region "Private methods",

        /// <summary>
        /// Get Existing Token OR Create new token
        /// </summary>
        /// <param name="objAccessToken"></param>
        /// <returns></returns>
        private async Task<AccessToken> GetExistingORCreateNewToken(AccessToken objAccessToken)
        {
            UserSession userSession = new UserSession();
            string token = BuildToken(objAccessToken);
            objAccessToken.Token = token;

            // NOTE: IF already exist session, just update expire time. OR Create a new session

            //List<UserSession> lstAllPreviousSession3 = (await _unitOfWork.Repository<UserSession>()
            //.GetAsync(predicate: x => x.Token == token
            //&& x.Status == (int)Enums.Status.Active
            //&& x.SessionEnd > DateTime.Now, disableTracking: true)).ToList<UserSession>();

            List<UserSession> lstAllPreviousSession2 = await _dbContext.UserSession.Where(x => x.Token == token 
            && x.Status == (int)Enums.Status.Active
            && x.SessionEnd > DateTime.Now).AsNoTracking().OrderByDescending(x => x.UserSessionID).OrderByDescending(x => x.UserSessionID).ToListAsync();


            if (lstAllPreviousSession2 != null && lstAllPreviousSession2.Count  > 0)
            {
                // 1. Update expire time of existing one.
                DateTime dateTime = DateTime.Now.AddMinutes(CommonConstant.SessionExpired);
                lstAllPreviousSession2[0].SessionEnd = dateTime;
                _unitOfWork.Repository<UserSession>().Update(lstAllPreviousSession2[0]);

                //set return session infomaiton 
                objAccessToken.IssuedOn = lstAllPreviousSession2[0].SessionStart.Value;
                objAccessToken.ExpiredOn = lstAllPreviousSession2[0].SessionEnd.Value;
            }
            else
            {
                // 2. Create new session and insert.

                // 2.a. remove previously active session 
            //    List<UserSession> lstAllPreviousSession = await _dbContext.UserSession.Where(x => x.Token == token
            //&& x.Status == (int)Enums.Status.Active
            //&& x.SessionEnd > DateTime.Now).AsNoTracking().OrderByDescending(x => x.UserSessionID).OrderByDescending(x => x.UserSessionID).ToListAsync();

                List<UserSession> lstAllPreviousSession = await _dbContext.UserSession.Where(x => x.Token == token
           && x.Status == (int)Enums.Status.Active).AsNoTracking().OrderByDescending(x => x.UserSessionID).OrderByDescending(x => x.UserSessionID).ToListAsync();


                foreach (var item in lstAllPreviousSession)
                {
                    item.Status = (int)Enums.Status.Inactive;
                }

                // TODO: do below later.
              //  _unitOfWork.Repository<UserSession>().UpdateRange(lstAllPreviousSession);

                //----------------------//
                // 2.b Insert new session.
                userSession = new UserSession();
                DateTime now = DateTime.Now;
                userSession.SessionStart = now;
                userSession.SessionEnd = now.AddMinutes(CommonConstant.SessionExpired);
                userSession.Token = token;
                userSession.RoleId = objAccessToken.RoleId;
                userSession.SystemUserID = objAccessToken.SystemUserID;
                userSession.Status = (int)Enums.Status.Active;

                await _dbContext.UserSession.AddAsync(userSession);
                _dbContext.SaveChangesAsync();

                //  await _unitOfWork.Repository<UserSession>().InsertAsync(userSession);

                //set return session infomaiton 
                objAccessToken.IssuedOn = userSession.SessionStart.Value;
                objAccessToken.ExpiredOn = userSession.SessionEnd.Value;

            }
          //  await _unitOfWork.SaveChangesAsync();

            return objAccessToken;
        }

        /// <summary>
        /// Generate jwt token with symetric security
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private static string BuildToken(AccessToken accessToken)
        {
            Claim[] claims = GetClaims(accessToken);
            string secretKey = "<RSAKeyValue><Modulus>xo8s7Hm6CjgRk0+lfBY7LOsErmL/i/tuscBAGWgxVaWysbE=</D></RSAKeyValue>";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("DemoProject", "DemoProject", claims, expires: accessToken.ExpiredOn, signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// get user claim principal
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private static Claim[] GetClaims(AccessToken accessToken)
        {
            return new[] {
                    new Claim(JwtClaim.UserId, accessToken.SystemUserID.ToString()),
                    new Claim(JwtClaim.UserType, accessToken.RoleId.ToString()),
                    new Claim(JwtClaim.ExpiresOn,accessToken.ExpiredOn.ToString()),
                    new Claim(JwtClaim.IssuedOn, accessToken.IssuedOn.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
        }

        #endregion
    }

    public interface IAccessTokenService
    {
        /// <summary>
        /// Generate new token. if exist any token remove that token
        /// </summary>
        /// <param name="systemUserID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        Task<string> GetAccessTokenForUser(int systemUserID, int roleID);
    }
}
