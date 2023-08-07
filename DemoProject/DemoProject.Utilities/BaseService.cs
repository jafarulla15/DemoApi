using DemoProject.Common;

namespace DemoProject.Utilities
{
    public class BaseService
    {
        public void SaveExceptionIntoFile(Exception ex)
        {
            CommonMethods.SaveExceptionIntoFile(ex);
        }
    }
}
