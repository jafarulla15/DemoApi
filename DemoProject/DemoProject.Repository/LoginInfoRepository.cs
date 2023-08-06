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
using System.Net.NetworkInformation;

namespace DemoProject.Repository
{
    public class LoginInfoRepository : ILoginInfoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DPDbContext _dpDbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public LoginInfoRepository(IUnitOfWork unitOfWork, DPDbContext dpDbContext, IServiceScopeFactory serviceScopeFactor)
        {
            _unitOfWork = unitOfWork;
            _dpDbContext = dpDbContext;
            _serviceScopeFactory = serviceScopeFactor;
        }

        public async Task<long> SaveLoginInfo(string sessionKey, int userID,
           string iPAddress, int companyId, string mACAddress, string hostName,
           string protocol, string publicIP)
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            var intefaceName = "";
            var intefaceDescription = "";
            foreach (NetworkInterface adapter in interfaces)
            {
                string operationStatus = adapter.OperationalStatus.ToString();
                if (operationStatus == "Up")
                {
                    intefaceName = adapter.Name;
                    intefaceDescription = adapter.Description;
                    break;
                }
            }

            LoginInfo log = new LoginInfo();
            try
            {
                log.SessionKey = sessionKey;
                log.UserID = userID.ToString();
                log.LoginDateTime = DateTime.Now;
                log.LogoutDate = new DateTime(1900, 1, 1);
                log.IPAddress = iPAddress;
                log.CompanyId = companyId;
                log.Status = (int)Enums.LoginStatus.Loggedin;
                log.MACAddress = mACAddress;
                log.HostName = hostName;
                log.InterfaceName = intefaceName;
                log.Protocol = protocol;
                log.PublicIP = publicIP;
                log.InterfaceDescription = intefaceDescription;

                await _unitOfWork.Repository<LoginInfo>().InsertAsync(log);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //TODO:Later will write file here
            }
            return log.SessionID;
        }

        public async Task UpdateLoginInfo(long sessionId)
        {
            try
            {
                //TODO: uncomment

                //var result = await _unitOfWork.Repository<LoginInfo>().GetAsync(sessionId);
                //if (result != null)
                //{
                //    result.LogoutDate = DateTime.Now;
                //    result.Status = (int)Enums.LoginStatus.Loggedout;
                //    _unitOfWork.Repository<LoginInfo>().InsertAsync(result);
                //    _unitOfWork.DbContext.Entry(result).State = EntityState.Modified;
                //    _unitOfWork.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                //TODO:Later will write file here
            }
        }

        private bool _disposed = false;

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                }
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public interface ILoginInfoRepository
    {
        Task<long> SaveLoginInfo(string sessionKey, int userID, string iPAddress,
            int companyId, string mACAddress, string hostName, string protocol, string publicIP);
        Task UpdateLoginInfo(long sessionId);
    }
}
