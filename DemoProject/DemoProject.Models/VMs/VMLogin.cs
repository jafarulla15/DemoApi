/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Models
{
    public class VMLogin
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool SSOLogin { get; set; }
        public string SSOAccessToken { get; set; }
        public string Token { get; set; }
        public int SystemUserID { get; set; } = 0;
        public int RoleID { get; set; } = 0;
        public string RoleName { get; set; }

        [NotMapped]
        public List<PagePermission> lstPagePermissions { get; set; } = new List<PagePermission>();
    }
}
