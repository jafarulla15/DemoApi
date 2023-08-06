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
    public class PagePermission : BaseClass
    {
        public int PagePermissionID { get; set; }
        public string PagePermissionName { get; set; }
        public string PageDisplayName { get; set; }
        public int Sequence { get; set; }

    }
}
