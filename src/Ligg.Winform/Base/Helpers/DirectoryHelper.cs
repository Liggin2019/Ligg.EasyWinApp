
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Ligg.Base.Extension;
using Microsoft.Win32;

namespace Ligg.Base.Helpers
{
    public static class DirectoryHelper
    {
        //#set
        public static string DeleteLastSlashes(string dir)
        {
            if (dir.EndsWith("\\"))
            {
                dir = dir.Substring(0, dir.Length - 1);
            }
            else
            {
                return dir;
            }
            return dir;
        }

        public static void CopyTo(string originalDir, string containerDir)
        {
        }
        public static void CopyContentTo(string originalDir, string containerDir)
        {
        }

        public static void BackupTo(string originalDir, string containerDir)
        {
        }

        //#get

        public static int GetRecursiveSubFileNo(string dir)
        {
            if (!Directory.Exists(dir))
            {
                throw new ArgumentException("Directory: " + dir + " does not exsit!");
            }
            var no = Directory.GetFiles(dir).Length;
            var subDirs = Directory.GetDirectories(dir);
            foreach (var subDir in subDirs)
            {
                no = no + GetRecursiveSubFileNo(subDir);
            }
            return no;
        }

        public static string GetLastFolderName(string dir)
        {
            var newDir = DeleteLastSlashes(dir);
            if (newDir.Contains("\\"))
            {
                var newDirArray = newDir.Split('\\');
                return newDirArray[newDirArray.Length - 1];
            }
            else
            {
                return "";
            }
            return dir;
        }


        //public static string GetDeskDir()
        //{
        //    return GetSpecialFolderPath(CSIDL_COMMON_DESKTOPDIRECTORY);
        //}

        //public static string GetProgramsDir()
        //{
        //    return GetSpecialFolderPath(CSIDL_COMMON_PROGRAMS);
        //}

        //public static string GetFavoriteDir()
        //{
        //    return GetSpecialFolderPath(CSIDL_COMMON_FAVORITES);
        //}

        public static string GetSpecialDir(string flag)
        {

            switch (flag.ToLower())
            {

                case "systemdrive": //C:
                    return Environment.ExpandEnvironmentVariables("%SystemDrive%");

                case "systemroot": //C:\WINDOWS
                    {
                        RegistryKey currentVersionKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                        return currentVersionKey.GetValue("SystemRoot").ToString();
                    }


                case "systemdirectory": //C:\WINDOWS\system32
                    return Environment.SystemDirectory;

                case "myprofile": //win7 C:\Users\chris.li	xp C:\Documents and Settings\Administrator
                    {
                        var tempStr = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        return tempStr.Substring(0, tempStr.LastIndexOf("\\"));
                        break;
                    }
                case "personal"
                    : //win7 C:\Users\chris.li\Documents; xp C:\Documents and Settings\Administrator\My Documents 
                    {
                        return System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    }

                case "commonapplicationdata"
                    : //win7 C:\ProgramData; xp C:\Documents and Settings\All Users\Application Data
                    {
                        return System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    }

                case "Localapplicationdata"
                    : //win7 C:\Users\chris.li\AppData\Local; xp C:\Documents and Settings\Administrator\Local Settings\Application Data	
                    {
                        return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        ;
                        break;
                    }

                case "applicationdata" //roamingappdatadirectory
                    : //win7 C:\Users\chris.li\AppData\Roaming; xp C:\Documents and Settings\Administrator\Application Data 
                    {
                        return System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    }
                case "programfiles":
                    {
                        return System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    }
            }

            return string.Empty;
        }

        //#check
        public static bool HasRecursiveSubFile(string dir)
        {
            if (!Directory.Exists(dir)) return false;

            if (Directory.GetFiles(dir).Length > 0) return true;
            var subDirs = Directory.GetDirectories(dir);
            foreach (var subDir in subDirs)
            {
                return HasRecursiveSubFile(subDir);
            }
            return false;
        }

        public static void CheckBeforeOpen(string dir)
        {
            if (!System.IO.Directory.Exists(dir))
            {
                throw new ArgumentException("Directory does not exist! dir=" + dir);
            }
        }

        //#common
        //[DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        //internal static extern void SHGetFolderPathW(
        //    IntPtr hwndOwner,
        //    int nFolder,
        //    IntPtr hToken,
        //    uint dwFlags,
        //    IntPtr pszPath);

        //internal static string SHGetFolderPath(int nFolder)
        //{
        //    string pszPath = new string(' ', MAX_PATH);
        //    IntPtr bstr = Marshal.StringToBSTR(pszPath);
        //    SHGetFolderPathW(IntPtr.Zero, nFolder, IntPtr.Zero, SHGFP_TYPE_CURRENT, bstr);
        //    string path = Marshal.PtrToStringBSTR(bstr);
        //    int index = path.IndexOf('\0');
        //    string path2 = path.Substring(0, index);
        //    Marshal.FreeBSTR(bstr);
        //    return path2;
        //}


        //public static string GetSpecialFolderPath(uint csidl)
        //{
        //    return SHGetFolderPath((int)(csidl | CSIDL_FLAG_CREATE));
        //}

        //internal const uint SHGFP_TYPE_CURRENT = 0;
        //internal const int MAX_PATH = 260;
        //internal const uint CSIDL_COMMON_STARTMENU = 0x0016;              // All Users\Start Menu
        //internal const uint CSIDL_COMMON_PROGRAMS = 0x0017;               // All Users\Start Menu\Programs
        //internal const uint CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019;       // All Users\Desktop
        //internal const uint CSIDL_PROGRAM_FILES = 0x0026;                 // C:\Program Files
        //internal const uint CSIDL_FLAG_CREATE = 0x8000;                   // new for Win2K, or this in to force creation of folder
        //internal const uint CSIDL_COMMON_FAVORITES = 0x001f;              // All Users Favorites
    }
}
