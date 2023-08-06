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
    public class LoginInfo
    {
        [Key]
        [DataType("bigint")]
        public long SessionID { get; set; }

        [DataType("nvarchar")]
        public string SessionKey { get; set; }

        [DataType("nvarchar")]
        public string UserID { get; set; }

        [DataType("datetime")]
        public DateTime LoginDateTime { get; set; }

        [DataType("datetime")]
        public DateTime LogoutDate { get; set; }

        [DataType("nvarchar")]
        public string IPAddress { get; set; }

        [DataType("int")]
        public int? CompanyId { get; set; }

        [DataType("int")]
        public int Status { get; set; }

        [DataType("nvarchar")]
        public string MACAddress { get; set; }

        [DataType("nvarchar")]
        public string HostName { get; set; }

        [DataType("nvarchar")]
        public string InterfaceName { get; set; }

        [DataType("nvarchar")]
        public string Protocol { get; set; }

        [DataType("nvarchar")]
        public string PublicIP { get; set; }

        [DataType("nvarchar")]
        public string InterfaceDescription { get; set; }
    }
}
