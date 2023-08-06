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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Models
{
    public class AccessToken
    {
        [Key]
        [Required]
        public int AccessTokenID { get; set; }
        [Required]
        public int SystemUserID { get; set; }
        [Required]
        public int RoleId { get; set; } = 0;

        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime IssuedOn { get; set; }
        [Required]
        public DateTime ExpiredOn { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
