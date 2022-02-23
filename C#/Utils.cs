using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace SharpXDecrypt
{
    class Utils
    {
        public struct UserSID
        {
            public string Name;
            public string SID;
        }
        public static UserSID GetUserSID()
        {
            UserSID userSID;
            Console.WriteLine("[*] Start GetUserSID....");
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            userSID.Name = current.Name.ToString().Split('\\')[1];
            userSID.SID = current.User.ToString();
            Console.WriteLine("  Username: " + userSID.Name);
            Console.WriteLine("  userSID: " + userSID.SID);
            Console.WriteLine("[*] GetUserSID Success !");
            Console.WriteLine();
            return userSID;
        }
    }
}
