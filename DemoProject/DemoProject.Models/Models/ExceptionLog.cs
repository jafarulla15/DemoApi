using System.ComponentModel.DataAnnotations;

namespace DemoProject.Models
{
    public class ExceptionLog
    {
        [Key]
        public int ExceptionLogID { get; set; }
        public int Priority { get; set; }
        public int ModuleID { get; set; }
        public string ExceptionMessege { get; set; }
        public string ExceptionDetail { get; set; }
        public string ObjectData { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int ActionType { get; set; }
        public string ManagerName { get; set; }
        public DateTime ExceptionTime { get; set; }
        public int FixStatus { get; set; }
    }
}
