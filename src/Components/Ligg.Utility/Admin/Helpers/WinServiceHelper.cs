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
            public int Type { get; set; } //ָ����������͡�������������ڹ���Ľ����С��ڹ���Ľ����У��������ʹ��ͬһ����(Win32ShareProcess)�����⣬����Ҳ����������ֻ����һ������Ľ���(Win32OwnProcess)�С����������������潻���������;���InteractiveProcess
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