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

        //#list
        public static List<WinServiceInfo> List(string machineName, List<string> serviceNameList)
        {
            ServiceController[] serviceControllers = null;
            serviceControllers = string.IsNullOrEmpty(machineName)
                                     ? ServiceController.GetServices()
                                     : ServiceController.GetServices(machineName);
            var winServices = new List<WinServiceInfo>();
            try
            {
                var serviceControllerList =
                    serviceControllers.Where(x => !x.ServiceName.IsNullOrEmpty());
                if (serviceNameList != null)
                {
                    serviceControllerList = serviceControllerList.Where(x => serviceNameList.Contains(x.ServiceName));
                }

                foreach (ServiceController svcCtrl in serviceControllerList)
                {
                    var winSvc = new WinServiceInfo();
                    winSvc.Name = svcCtrl.ServiceName;
                    winSvc.Type = (int)svcCtrl.ServiceType; //GetServiceTypeName(controller.ServiceType);
                    winSvc.DisplayName = svcCtrl.DisplayName;
                    winSvc.Status = (int)svcCtrl.Status;
                    try
                    {//for that case you do not have auth to open registry
                        using (var regKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\services\\" + svcCtrl.ServiceName))
                        {
                            if (regKey != null)
                            {
                                winSvc.StartMode = Convert.ToInt16(regKey.GetValue("Start").ToString());
                                //winSvc.Description = regKey.GetValue("Description").ToString();
                                //winSvc.ImageUrl = regKey.GetValue("ImagePath").ToString();
                                regKey.Close();
                            }
                        }
                    }
                    catch
                    {

                    }
                    winServices.Add(winSvc);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".List Error: " + ex.Message);
            }
            return winServices;
        }

        //#set
        public static void Reset(string name)
        {
            try
            {
                var controller = new ServiceController(name);
                var status = GetStatus(name);
                if (status != (int)ServiceControllerStatus.Stopped)
                    controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped);
                controller.Start();
                controller.WaitForStatus(ServiceControllerStatus.Running);
                controller.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Reset Error: " + ex.Message);
            }
        }

        public static void Start(string name)
        {
            try
            {
                var controller = new ServiceController(name);
                var status = GetStatus(name);
                if (status != (int)ServiceControllerStatus.Running)
                    controller.Start();
                controller.WaitForStatus(ServiceControllerStatus.Running);
                controller.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Start Error: " + ex.Message);
            }
        }


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