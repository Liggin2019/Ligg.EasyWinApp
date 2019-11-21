using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;

namespace Ligg.Base.Helpers
{
    public static class FileHelper
    {
        //#get
        public static string GetFilePath(string url, string defaultLocaton)
        {
            var retStr = "";
            if (url.StartsWith("\\\\"))
            {
                retStr = url;
            }
            else if (url.StartsWith("\\"))
            {
                retStr = defaultLocaton + url;
            }
            else
            {
                retStr = url;
            }
            return retStr;
        }


        public static string GetFileDetailByOption(string path, FilePathComposition returnOpt)  //1:dir;2:fileName;
        {
            int lastIndex = path.LastIndexOf("\\");
            var dir = path.Substring(0, lastIndex);
            var fileName = path.Substring(lastIndex + 1, path.Length - lastIndex - 1);
            if (returnOpt == FilePathComposition.Directory) return dir;//dir
            else if (returnOpt == FilePathComposition.FileName) return fileName; //file name

            else if (returnOpt == FilePathComposition.FileTitle)
            {
                if (fileName.Contains('.'))
                    return GetFilePathOrNameDetailByOption(path, FilePathComposition.FileTitle);
                else
                {
                    return fileName;
                }
            }
            else if (returnOpt == FilePathComposition.Postfix)
            {
                if (fileName.Contains('.')) return GetFilePathOrNameDetailByOption(path, FilePathComposition.Postfix);
                else return string.Empty;
            }
            else if (returnOpt == FilePathComposition.Suffix) ;
            {
                if (fileName.Contains('.')) return GetFilePathOrNameDetailByOption(path, FilePathComposition.Suffix);
                else return string.Empty;
            }
            return string.Empty;
        }

        private static string GetFilePathOrNameDetailByOption(string filePathOrName, FilePathComposition returnOpt)  //1:fileTitle;2:postfix;3:suffix;
        {
            int lastIndexOfDot = filePathOrName.LastIndexOf(".");
            if (lastIndexOfDot == -1)
            {
                throw new ArgumentException("Filename must contains separator  '.'");
            }
            string fileTitle = filePathOrName.Substring(0, lastIndexOfDot);
            int lastIndexOfDoubleSlash = fileTitle.LastIndexOf("\\");
            if (lastIndexOfDoubleSlash != -1)
            {
                fileTitle = fileTitle.Substring(lastIndexOfDoubleSlash + 1, fileTitle.Length - lastIndexOfDoubleSlash - 1);
            }
            string suffix = filePathOrName.Substring(lastIndexOfDot + 1, filePathOrName.Length - lastIndexOfDot - 1);
            string postfix = "." + suffix;
            if (returnOpt == FilePathComposition.FileTitle)
            {
                return fileTitle;
            }
            else if (returnOpt == FilePathComposition.Postfix)
            {
                return postfix;
            }
            else if (returnOpt == FilePathComposition.Suffix)
            {
                return suffix;
            }
            else
            {
                return string.Empty;
            }
        }


        public static void CheckFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("FilePath can't be null! ");
            }

            var pathFist2Letters = filePath.Substring(0, 2);
            if (pathFist2Letters == "\\\\")
            {
                if (filePath.Contains(":"))
                {
                    throw new ArgumentException("FilePath should not contains both '\\' and ':', ! filePath=" + filePath);
                }
            }
            else
            {
                if (!filePath.Contains(":"))
                {
                    throw new ArgumentException("FilePath should contains ':', ! filePath=" + filePath);
                }
            }

        }

        public static void CheckFilePath(string filePath, bool isDotForced)
        {
            CheckFilePath(filePath);
            int lastIndexOfDot = filePath.LastIndexOf(".");
            if (isDotForced)
            {
                if (lastIndexOfDot == -1)
                {
                    throw new ArgumentException("FilePath must contains separator  '.'! filePath=" + filePath);
                }
            }
        }


        public static void CheckFilePathBeforeSave(string filePath, bool isDotForced, bool ifCreateDir)
        {
            CheckFilePath(filePath, isDotForced);
            var dir = GetFileDetailByOption(filePath, FilePathComposition.Directory);
            if (ifCreateDir)
            {
                if (!System.IO.Directory.Exists(dir))
                {
                    var dirArry = dir.Split('\\');
                    var dir1 = "";
                    var i = 0;
                    foreach (var v in dirArry)
                    {
                        dir1 = i == 0 ? v : dir1 + "\\" + v;
                        if (!System.IO.Directory.Exists(dir) && 1 != 0)
                        {
                            Directory.CreateDirectory(dir1);
                        }
                        else
                        {
                            throw new ArgumentException("FileHelper.ResoveFilePath Error; driver or network sharing does not exsit! driver or network sharing=" + dir1);
                        }
                        i++;
                    }

                }
            }
            else
            {
                if (!System.IO.Directory.Exists(dir))
                {
                    throw new ArgumentException("FileHelper.CheckFilePathBeforeSave Error; Directory does not exsit! Directory=" + dir);
                }
            }
        }





    }



}