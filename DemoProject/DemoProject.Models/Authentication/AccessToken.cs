/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Models
{
    public class AccessToken
    {
        [NotMapped]
        public int AccessTokenID { get; set; }
        public int SystemUserID { get; set; }
        public int RoleId { get; set; } = 0;
        public string Token { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiredOn { get; set; }
        public long SessionId { get; set; }
        public int Status { get; set; }
    }
}
