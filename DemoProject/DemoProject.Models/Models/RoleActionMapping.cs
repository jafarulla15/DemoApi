﻿/*
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
    public class RoleActionMapping
    {
        public int RoleActionMappingID { get; set; }

        public int RoleID { get; set; } = 0;

        public int ActionID { get; set; } = 0;

    }
}
