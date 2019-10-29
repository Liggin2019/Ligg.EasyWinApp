using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Microsoft.Win32;

namespace Ligg.Utility.Admin.Helpers
{

    public static class WinServiceHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;


        public static int GetStatus(string name)
        {
            try
            {
                var controller = new ServiceController(name);
                return (int)controller.Status;
            }
            catch
            {
                return 0;
            }
        }

 
        public class WinServiceInfo
        {
            public string Name { get; set; }
            public int Type { get; set; } //指定服务的类型。服务可以运行在共享的进程中。在共享的进程中，多个服务使用同一进程(Win32ShareProcess)，此外，服务也可以运行在只包含一个服务的进程(Win32OwnProcess)中。如果服务可以与桌面交互，则类型就是InteractiveProcess
            public string DisplayName { get; set; }
            public int StartMode { get; set; }//System.ServiceProcess.ServiceStartMode:Automatic = 2 Manual = 3  Disabled = 4 
            public int Status { get; set; }
            //System.ServiceProcess.ServiceControllerStatus
            //Stopped = 1 StartPending = 2  StopPending = 3 Running = 4 
            //ContinuePending = 5 PausePending = 6 Paused = 7 
            //wmic service where name='seclogon' call ChangeStartMode Manual
            //wmic service where name='seclogon' call startservice
        }
    }

}