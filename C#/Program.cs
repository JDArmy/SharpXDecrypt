using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace SharpXDecrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Xshell全版本凭证一键导出工具!(支持Xshell 7.0+版本)");
            Console.WriteLine("Author: 0pen1");
            Console.WriteLine("Github: https://github.com/JDArmy");
            Console.WriteLine("[!] WARNING: For learning purposes only,please delete it within 24 hours after downloading!");
            Console.WriteLine();
            if (args.Length == 1)
            {
                XClass.Decrypt(args[0]);
            }
            else
            {
                XClass.Decrypt();
            }
            Console.WriteLine("[*] read done!");
        }

    }
}
