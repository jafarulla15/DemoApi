﻿/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using DemoProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Services
{
    public class BaseService
    {
        public void SaveExceptionIntoFile(Exception ex)
        {
            CommonMethods.SaveExceptionIntoFile(ex);
        }
    }
}
