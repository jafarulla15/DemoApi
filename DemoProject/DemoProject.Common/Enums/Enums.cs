/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Common.Enums
{
    public class Enums
    {
        public enum ResponseCode
        {
            Success = 1,
            Failed = 2,
            Warning = 3
        }
        public enum Status
        {
            Active = 1,
            Inactive = 2,
            Deleted = 3
        }

        public enum ActionType
        {
            Insert = 1,
            Update = 2,
            View = 3,
            Delete = 4,
            Login = 5,
            Register = 6,
            Logout = 7,
        }
        public enum LoginStatus
        {
            Loggedin = 1,
            Loggedout = 0
        }

        #region "Audit Log"

        public enum UserAccessRight
        {
            SuperAdmin = 1,
            Admin = 2,
            Agent = 3,
            User = 4,
            Anonymous = 5
        }

        public enum UserType
        {
            APPSUSER = 1,
            DEVICEUSER = 2,
            WEBUSER = 3
        }
        public enum LogActions
        {
            insert = 1,
            Update = 2,
            Deleted = 3,
            Modified = 4,
            View = 5,
            Reviewed = 6

        }

        public enum LogType
        {
            SecurityLog = 1,
            ErrorLog = 2,
            SystemLog = 3,
            DbQuery = 4,
            Other = 5

        }

        #endregion

        #region "Exception Log"

        public enum ExceptionModule
        {
            AdminPanel = 1,
            App = 2,
            Sentra = 3
        }

        public enum ActionName
        {
            User = 1,
            Dashboard = 2
        }

        public enum LogFixPriority
        {
            Low = 1,
            Medium = 2,
            High = 3
        }

        public enum WebSocketType
        {
            Low = 1,
            Medium = 2,
        }


        #endregion        
    }
}
