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
    public class SystemUser : BaseClass
    {
        public int SystemUserID { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int RoleID { get; set; } = 0;

        public bool IsApproved { get; set; } = false;

        public int StatusOfUser { get; set; } = 0;
    }

}
