using DemoProject.Models;
using DemoProject.Repository;
using Newtonsoft.Json;

namespace DemoProject.Services
{
    public class ExceptionLogService : BaseService, IExceptionLogService
    {
        //private readonly ICustomDbContextFactory<DPDbContext> _customDbContextFactory;
        private IUnitOfWork _unitOfWork;
        public ExceptionLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SaveExceptionLog(int priority, int moduleID, string exceptionMessege, string exceptionDetail, object objectData, string controllerName, string actionName, int actionType, string managerName)
        {
            try
            {
                ExceptionLog exceptionLog = new ExceptionLog();

                exceptionLog.Priority = priority;
                exceptionLog.ModuleID = moduleID;
                exceptionLog.ExceptionMessege = exceptionMessege;
                exceptionLog.ExceptionDetail = exceptionDetail;
                exceptionLog.ObjectData = JsonConvert.SerializeObject(objectData).ToString();
                exceptionLog.ControllerName = controllerName;
                exceptionLog.ActionName = actionName;
                exceptionLog.ActionType = actionType;
                exceptionLog.ManagerName = managerName;
                exceptionLog.ExceptionTime = DateTime.UtcNow;

                await _unitOfWork.Repository<ExceptionLog>().InsertAsync(exceptionLog);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                base.SaveExceptionIntoFile(ex);
            }
        }
    }

    public interface IExceptionLogService
    {
        Task SaveExceptionLog(int priority, int moduleID, string exceptionMessege, string exceptionDetail, object objectData, string controllerName, string actionName, int actionType, string managerName);
    }
}
