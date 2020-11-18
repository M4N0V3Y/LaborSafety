using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaborSafety.Utils.Security;

namespace LaborSafety.Login
{
    public class Login
    {
        public static Boolean Validate(string id, string password)
        {
            return ADSettings.AuthAD(id, password);
        }
    }
}