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
    public  class RequestMessage
    {
        public object? RequestObj { get; set; }
        public int PageRecordSize { get; set; } = 0;
        public int PageNumber { get; set; } = 0;
        public int UserID { get; set; }
    }
}
