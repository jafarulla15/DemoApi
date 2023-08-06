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

namespace DemoProject.Common.Constants
{
	public static class CommonConstant
	{
		public static DateTime DeafultDate = Convert.ToDateTime("1900/01/01");
		public static int SessionExpired = 30;
	}

	public static class CommonPath
	{
		public const string loginUrl = "/api/security/login";
        public const string registerUrl = "/api/Security/Register";   //  "/api/SystemUser/registerSystemUser";  // "/api/security/register";
	}
	public class HttpHeaders
	{
		public const string Token = "Authorization";
		public const string AuthenticationSchema = "Bearer";
	}

    public class JwtClaims
    {
        public const string UserId = "UserId";
        public const string CompanyUserId = "CompanyUserId";
        public const string UserTypeId = "UserTypeId";
        public const string UserName = "UserName";
        public const string CompanyName = "CompanyName";
        public const string CompanyId = "CompanyId";
        public const string Name = "Name";
        public const string Email = "Email";
        public const string IsSuperAdmin = "IsSuperAdmin";
        public const string IssuedOn = "IssuedOn";
        public const string ExpiresDate = "ExpiresDate";
        public const string AccessRight = "AccessRight";
        public const string UniqueName = "UniqueName";
        public const string UniqueID = "UniqueID";
        public const string SessionId = "SessionId";
    }

}
