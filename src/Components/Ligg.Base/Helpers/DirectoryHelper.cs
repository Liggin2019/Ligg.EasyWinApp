
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


    }
}
