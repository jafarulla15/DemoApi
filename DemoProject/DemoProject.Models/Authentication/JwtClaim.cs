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

namespace DemoProject.Models
{
    public class JwtClaim
    {
        public const string UserId = "SystemUserID";
        public const string UserType = "UserType";
        public const string ExpiresOn = "ExpiresOn";
        public const string IssuedOn = "IssuedOn";
    }
}
