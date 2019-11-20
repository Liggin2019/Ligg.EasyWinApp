using System;
using System.Diagnostics;
using Ligg.Base.DataModel.Enums;

namespace Ligg.Base.Helpers
{
    public static class ProcessHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        public static string Cmd(string inputStr, ExecCmdMode execCmdMode)
        {
            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            //process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//does not function, the windows shown or hidden depends on  "process.StartInfo.CreateNoWindow"

            var returnOutput = false;
            if (execCmdMode == ExecCmdMode.AsyncWindow)
            {
                var exeFileDir = FileHelper.GetFileDetailByOption(Process.GetCurrentProcess().MainModule.FileName, FilePathComposition.Directory);
                inputStr = "start " + exeFileDir + "\\Resources\\start.bat " + inputStr;
                process.StartInfo.CreateNoWindow = true;
                returnOutput = false;

            }
            else if (execCmdMode == ExecCmdMode.SyncWindow)
            {
                var exeFileDir = FileHelper.GetFileDetailByOption(Process.GetCurrentProcess().MainModule.FileName, FilePathComposition.Directory);
                inputStr = "start " + exeFileDir + "\\Resources\\start.bat " + inputStr;
                process.StartInfo.CreateNoWindow = true;
                returnOutput = true;
            }
            else if (execCmdMode == ExecCmdMode.NoWindow)
            {
                process.StartInfo.CreateNoWindow = true;
                returnOutput = false;
            }
            else if (execCmdMode == ExecCmdMode.NoWindowWithOutput)
            {
                process.StartInfo.CreateNoWindow = true;
                returnOutput = true;
            }
            else if (execCmdMode == ExecCmdMode.EmptyWindow)
            {
                process.StartInfo.CreateNoWindow = false;
                //when CreateNoWindow = false, popup a empty cmd window, no any exec result appears. 
                returnOutput = false;
            }
            else if (execCmdMode == ExecCmdMode.EmptyWindowWithOutput)
            {
                process.StartInfo.CreateNoWindow = false;
                returnOutput = true;
            }



            try
            {
                process.Start();
                //process.StandardInput.WriteLine("cd " + @"C:\windows\system32");
                process.StandardInput.WriteLine(inputStr);
                process.StandardInput.WriteLine("exit");
                var outputStr = "";
                if (returnOutput)
                {
                    outputStr = process.StandardOutput.ReadToEnd();//When use 'start' to start a real new window, the  Output is on the new window, not this empty window. so if you want a return output , don't use 'start'
                }
                process.WaitForExit();
                process.Close();
                return outputStr;
            }

            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Cmd Error: " + ex.Message);
            }
        }

        public static string Cmd(string inputStr, ExecCmdMode execCmdMode, string domainName, string useName, string password)
        {
            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//does not function, the windows shown or hidden depends on  "process.StartInfo.CreateNoWindow"

            var returnOutput = false;
            if (execCmdMode == ExecCmdMode.AsyncWindow)
            {
                var exeFileDir = FileHelper.GetFileDetailByOption(Process.GetCurrentProcess().MainModule.FileName, FilePathComposition.Directory);
                inputStr = "start " + exeFileDir + "\\Resources\\start.bat " + inputStr;
                process.StartInfo.CreateNoWindow = true;
                returnOutput = false;

            }
            else if (execCmdMode == ExecCmdMode.SyncWindow)
            {
                var exeFileDir = FileHelper.GetFileDetailByOption(Process.GetCurrentProcess().MainModule.FileName, FilePathComposition.Directory);
                inputStr = "start " + exeFileDir + "\\Resources\\start.bat " + inputStr;
                process.StartInfo.CreateNoWindow = true;
                returnOutput = true;
            }
            else if (execCmdMode == ExecCmdMode.NoWindow)
            {
                process.StartInfo.CreateNoWindow = true;
                returnOutput = false;
            }
            else if (execCmdMode == ExecCmdMode.NoWindowWithOutput)
            {
                process.StartInfo.CreateNoWindow = true;
                returnOutput = true;
            }
            else if (execCmdMode == ExecCmdMode.EmptyWindow)
            {
                process.StartInfo.CreateNoWindow = false;
                //when CreateNoWindow = false, popup a empty cmd window, no any exec result appears. 
                returnOutput = false;
            }
            else if (execCmdMode == ExecCmdMode.EmptyWindowWithOutput)
            {
                process.StartInfo.CreateNoWindow = false;
                returnOutput = true;
            }

            process.StartInfo.Domain = domainName;
            process.StartInfo.UserName = useName;
            var pw = new System.Security.SecureString();
            foreach (var v in password.ToCharArray())
            {
                pw.AppendChar(v);
            }
            process.StartInfo.Password = pw;
            try
            {
                process.Start();
                process.StandardInput.WriteLine(inputStr);
                process.StandardInput.WriteLine("exit");
                var outputStr = "";
                if (returnOutput)
                {
                    outputStr = process.StandardOutput.ReadToEnd();
                }
                process.WaitForExit();
                process.Close();
                return outputStr;
            }

            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Cmd Error: " + ex.Message);
            }
        }

        public static void Run(string path, string args)
        {
            try
            {
                var process = new Process();
                if (System.IO.File.Exists(path))
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = path;
                    if (!string.IsNullOrEmpty(args))
                    {
                        process.StartInfo.Arguments = args;//to "explorer.exe", args="D:\\Readme.txt"; to "iexplore.exe",args=http://www.baidu.com
                    }
                    process.Start();
                }
                else
                {
                    throw new ArgumentException("File: '" + path + "' does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Run Error: " + ex.Message);
            }
        }

        public static void Run(string path, string args, string domainName, string useName, string password)
        {
            try
            {
                var process = new Process();
                if (System.IO.File.Exists(path))
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = path;
                    if (!string.IsNullOrEmpty(args))
                    {
                        process.StartInfo.Arguments = args;
                    }

                    process.StartInfo.UserName = useName;
                    process.StartInfo.Domain = domainName;
                    var pw = new System.Security.SecureString();
                    foreach (var v in password.ToCharArray())
                    {
                        pw.AppendChar(v);
                    }
                    process.StartInfo.Password = pw;
                    process.Start();
                }
                else
                {
                    throw new ArgumentException("File:"+ path+" does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Run Error: " + ex.Message);
            }
        }

        public static void OpenFile(string path, string args)
        {
            try
            {
                var process = new Process();
                if (System.IO.File.Exists(path))
                {
                    process.StartInfo.FileName = path;
                    if (!string.IsNullOrEmpty(args))
                    {
                        process.StartInfo.Arguments = args;
                    }
                    process.Start();
                }
                else
                {
                    throw new ArgumentException("File: '" + path + "' does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Edit Error: " + ex.Message);
            }
        }

        public static void OpenFile(string path, string args, string domainName, string useName, string password)
        {
            try
            {
                var process = new Process();
                if (System.IO.File.Exists(path))
                {
                    if (string.IsNullOrEmpty(args))
                    {
                        process.StartInfo.FileName = path;
                    }
                    else
                    {
                        process.StartInfo = new ProcessStartInfo(path, args);
                    }

                    process.StartInfo.UserName = useName;
                    process.StartInfo.Domain = domainName;
                    var pw = new System.Security.SecureString();
                    foreach (var v in password.ToCharArray())
                    {
                        pw.AppendChar(v);
                    }
                    process.StartInfo.Password = pw;
                    process.Start();
                }
                else
                {
                    throw new ArgumentException("File: '" + path + "' does not exist!");
                }
            }

            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Edit Error: " + ex.Message);
            }
        }

        public static void OpenTempFile(string path, string args)
        {
            try
            {
                var process = new Process();
                if (System.IO.File.Exists(path))
                {
                    var fileName = FileHelper.GetFileDetailByOption(path, FilePathComposition.FileName);
                    var tmpFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "Local Settings\\Temp\\" + fileName;
                    FileHelper.CheckFilePathBeforeSave(tmpFilePath, false, true);
                    System.IO.File.Copy(path, tmpFilePath, true);
                    //FileInfo fi = new FileInfo(tmpFilePath);
                    //fi.Attributes = FileAttributes.ReadOnly;
                    process.StartInfo.FileName = tmpFilePath;
                    if (!string.IsNullOrEmpty(args))
                    {
                        process.StartInfo.Arguments = args;
                    }
                    process.Start();
                }
                else
                {
                    throw new ArgumentException("File:" + path + " does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Open Error: " + ex.Message);
            }
        }

        public static void OpenTempFile(string path, string args, string domainName, string useName, string password)
        {
            try
            {
                var process = new Process();
                if (System.IO.File.Exists(path))
                {
                    var fileName = FileHelper.GetFileDetailByOption(path, FilePathComposition.FileName);
                    var tmpFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "Local Settings\\Temp\\" + fileName;
                    FileHelper.CheckFilePathBeforeSave(tmpFilePath, false, true);
                    System.IO.File.Copy(path, tmpFilePath, true);

                    process.StartInfo.FileName = tmpFilePath;
                    if (!string.IsNullOrEmpty(args))
                    {
                        process.StartInfo.Arguments = args;
                    }

                    process.StartInfo.UserName = useName;
                    process.StartInfo.Domain = domainName;
                    var pw = new System.Security.SecureString();
                    foreach (var v in password.ToCharArray())
                    {
                        pw.AppendChar(v);
                    }
                    process.StartInfo.Password = pw;
                    process.Start();
                }
                else
                {
                    throw new ArgumentException("File:" + path + " does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Open Error: " + ex.Message);
            }
        }

        //it is OK
        public static void OpenFolder(string dir)
        {
            try
            {
                var process = new Process();
                if (System.IO.Directory.Exists(dir))
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = "Explorer.exe";
                    process.StartInfo.Arguments = dir;
                    process.Start();
                }
                else
                {
                    throw new ArgumentException("File: '" + dir + "' does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".OpenFolder Error: " + ex.Message);
            }
        }

        //open dir  by user can't not work,it is strange
        //also same runasadmin , execcmdasadmin

        public static void OpenFolder(string dir, string domainName, string useName, string password)
        {
            try
            {
                var process = new Process();
                if (System.IO.Directory.Exists(dir))
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = @"c:\windows\Explorer.exe";
                    process.StartInfo.Arguments = dir;
                    process.StartInfo.UserName = useName;
                    process.StartInfo.Domain = domainName;
                    var pw = new System.Security.SecureString();
                    foreach (var v in password.ToCharArray())
                    {
                        pw.AppendChar(v);
                    }
                    process.StartInfo.Password = pw;
                    process.Start();
                }
                else
                {
                    throw new ArgumentException("File: '" + dir + "' does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".OpenFolder Error: " + ex.Message);
            }
        }


        public static void Redirect(string url)
        {
            try
            {
                var process = new Process();
                process.SynchronizingObject = null;
                process.StartInfo.LoadUserProfile = false;
                process.StartInfo.StandardErrorEncoding = null;
                process.StartInfo.StandardOutputEncoding = null;
                process.StartInfo.FileName = url;
                process.StartInfo.Domain = "";
                process.StartInfo.UserName = "";
                process.StartInfo.Password = null;
                process.Start();
            }

            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Redirect Error: " + ex.Message);
            }
        }

        public static void Redirect(string url, string domainName, string useName, string password)
        {
            try
            {
                var process = new Process();
                process.SynchronizingObject = null;
                process.StartInfo.LoadUserProfile = false;
                process.StartInfo.Password = null;
                process.StartInfo.StandardErrorEncoding = null;
                process.StartInfo.StandardOutputEncoding = null;
                process.StartInfo.FileName = url;
                process.StartInfo.Domain = domainName;
                process.StartInfo.UserName = useName;
                var pw = new System.Security.SecureString();
                foreach (var v in password.ToCharArray())
                {
                    pw.AppendChar(v);
                }
                process.StartInfo.Password = pw;

                process.Start();
            }

            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Redirect Error: " + ex.Message);
            }
        }

    }

    public enum ExecCmdMode
    {
        //to 0, 1:  popup a window, can see exec result(after one cmd finish exce) except for lgcmda, till closing the window manually(pause);
        //          to lgcmda, will popup another lgcmda window, where can see exec result ;
        //          can not get exec result at calling location 
        AsyncWindow = 0,  //asynchro;
        SyncWindow = 1,//synchro;

        //to 2, 3:  popup no window;
        //          to lgcmda, will popup another lgcmda window;
        //          not fit for endless cmd.(although for lgcmda can end the process by close lgcmda for a endless cmd, but can see nothing on lgcmda window)
        NoWindow = 2,//synchro; 
        NoWindowWithOutput = 3,//synchro; can get exec result at calling location except for lgcmda

        //to 4, 5: popup a empty window, can not see exec result;
        //         to lgcmda, will popup another lgcmda window
        EmptyWindow = 4,//synchro; 
        EmptyWindowWithOutput = 5,//synchro; can get exec result at calling location except for lgcmda
    }
}
