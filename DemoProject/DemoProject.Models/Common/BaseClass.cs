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
    public class BaseClass
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [MaxLength(30)]
        public int CreatedBy { get; set; }
        [MaxLength(30)]
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public int Status { get; set; } = 1;
    }
}
