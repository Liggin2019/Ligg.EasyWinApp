using System;
using System.Collections.Generic;
using System.Linq;
using Ligg.Base.DataModel;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Handlers;
using Ligg.Base.Helpers;
using Ligg.EasyWinApp.ImplInterface;

namespace Ligg.EasyWinApp.Implementation.Services
{
    internal class JobService
    {
        internal void InitData()
        {
            try
            {
                if (JobServiceData.Jobs is null)
                {
                    JobServiceData.Init();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitData Error: " + ex.Message);
            }
        }

        internal void InitCurrentJob(int jobId, string sGuid)
        {
            try
            {
                if (JobServiceData.Jobs is null)
                {
                    JobServiceData.Init();
                }
                var curJob = new CurrentJob { Id = jobId, ShortGuid = sGuid };

                JobServiceData.CurrentJobs.Add(curJob);
                foreach (var task in JobServiceData.Tasks.Where(x=>x.JobId== jobId))
                {
                    var curTask = new CurrentTask { Id = task.Id, JobId = jobId, ShortGuid = sGuid, No = task.No, Status = (int)TaskStatus.Waiting };
                    JobServiceData.CurrentTasks.Add(curTask);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitCurrentJob Error: " + ex.Message);
            }
        }

        internal string GetJobExecType(int jobId)
        {
            try
            {
                var job = JobServiceData.Jobs.Find(x => x.Id == jobId);
                return job.ExecType.ToString();

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetJobExecType Error: " + ex.Message);
            }
        }

        internal string GetJobExecParams(int jobId)
        {
            try
            {
                var job = JobServiceData.Jobs.Find(x => x.Id == jobId);
                return job.ExecParams.ToString();

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetJobExecParams Error: " + ex.Message);
            }
        }

        internal string GetJobDisplayName(int jobId)
        {
            try
            {
                var job = JobServiceData.Jobs.Find(x => x.Id == jobId);
                var txt = "";
                if (GlobalConfiguration.SupportMutiCultures)
                {
                    txt = AnnexHelper.GetText("", job.Id, JobServiceData.JobAnnexes, AnnexTextType.DisplayName, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.OnlyByCurLang);
                }

                if (txt.IsNullOrEmpty()) txt = job.DisplayName;
                return txt;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetJobDisplayName Error: " + ex.Message);
            }
        }


        internal int GetJobTaskCount(int jobId)
        {
            try
            {
                return JobServiceData.Tasks.Where(x => x.JobId == jobId).ToList().Count;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetJobTaskCount Error: " + ex.Message);
            }
        }

        internal string GetJobCurrentTaskAction(int jobId, int taskNo)
        {
            try
            {
                var task = JobServiceData.Tasks.Find(x => x.JobId == jobId & x.No == taskNo);
                return task.Action;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetCurrentJobCurrentTaskAction Error: " + ex.Message);
            }
        }

        internal string GetCurrentJobTaskList(int jobId, string sGuid)
        {
            try
            {
                var curTasks = JobServiceData.CurrentTasks.Where(x => x.JobId == jobId & x.ShortGuid == sGuid).ToList();
                foreach (var curTask in curTasks)
                {
                    if (curTask.Status == (int)TaskStatus.Processing) curTask.ListOrder = "a";
                    if (curTask.Status == (int)TaskStatus.Waiting) curTask.ListOrder = "b";
                    if (curTask.Status == (int)TaskStatus.Completed) curTask.ListOrder = "c";
                }

                var tasks = JobServiceData.Tasks;
                var query = curTasks.SelectMany(
                        x => tasks.Where(y => y.Id == x.Id).DefaultIfEmpty(),
                        (x, y) =>
                            new
                            {
                                x.Id,
                                x.ListOrder,
                                x.Status,
                                y.DisplayName,
                            }
                    ).ToList();

                var tasklist = "";
                var tasks1 = query.OrderBy(x => x.ListOrder);
                foreach (var task in tasks1)
                {
                    tasklist = tasklist + task.DisplayName + " - " + EnumHelper.GetNameById<TaskStatus>(task.Status) + "\n";
                }
                return tasklist;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetCurrentJobTaskList Error: " + ex.Message);
            }
        }

        internal void ClearCurrentJobTasksStatuses(int jobId, string sGuid)
        {
            try
            {
                var curTasks = JobServiceData.CurrentTasks.Where(x => x.JobId == jobId & x.ShortGuid == sGuid).ToList();

                foreach (var task in curTasks)
                {
                    task.Status = (int)TaskStatus.Waiting;
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".ClearCurrentJobTasksStatuses Error: " + ex.Message);
            }
        }

        internal void SetCurrentJobCurrentTaskCompleted(int jobId, string sGuid, int no)
        {
            try
            {
                var curTask = JobServiceData.CurrentTasks.Find(x => x.JobId == jobId & x.ShortGuid == sGuid & x.No == no);
                curTask.Status = (int)Ligg.Base.DataModel.Enums.TaskStatus.Completed;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SetCurrentJobCurrentTaskCompleted Error: " + ex.Message);
            }
        }

        internal void SetCurrentJobCurrentTaskProcessing(int jobId, string sGuid, int no)
        {
            try
            {
                var curTask = JobServiceData.CurrentTasks.Find(x => x.JobId == jobId & x.ShortGuid == sGuid & x.No == no);
                curTask.Status = (int)Ligg.Base.DataModel.Enums.TaskStatus.Processing;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SetCurrentJobCurrentTaskProcessing Error: " + ex.Message);
            }
        }




    }

    internal static class JobServiceData
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        public static List<Job> Jobs;
        public static List<Annex> JobAnnexes;
        public static List<Task> Tasks;
        public static List<CurrentJob> CurrentJobs = new List<CurrentJob>();
        public static List<CurrentTask> CurrentTasks = new List<CurrentTask>();

        public static void Init()
        {
            try
            {
                var xmlPath = Configuration.DataDir + "\\JobService\\Jobs";
                var xmlMgr = new XmlHandler(xmlPath);
                Jobs = xmlMgr.ConvertToObject<List<Job>>();

                var xmlPath1 = Configuration.DataDir + "\\JobService\\JobAnnexes";
                var xmlMgr1 = new XmlHandler(xmlPath1);
                JobAnnexes = xmlMgr1.ConvertToObject<List<Annex>>();

                var xmlPath2 = Configuration.DataDir + "\\JobService\\Tasks";
                var xmlMgr2 = new XmlHandler(xmlPath2);
                Tasks = xmlMgr2.ConvertToObject<List<Task>>().OrderBy(x => x.ActOrder).ToList();
                foreach (var job in Jobs)
                {
                    var i = 0;
                    foreach (var task in Tasks.Where(x => x.JobId == job.Id))
                    {
                        task.No = i;
                        i++;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Init Error: " + ex.Message);
            }
        }
    }

    public class Job
    {
        public int Id;
        public string Name;
        public string DisplayName;
        public int ExecType;
        public string ExecParams;
    }

    public class Task
    {
        public int Id;
        public int JobId;
        public int No;
        public string Name;
        public string DisplayName;
        public string Action;
        public string ActOrder;
        public string ListOrder;
    }

    public class CurrentJob
    {
        public int Id;
        public string ShortGuid;
    }

    public class CurrentTask
    {
        public int Id;
        public int JobId;
        public string ShortGuid;
        public int Status;
        public int No;
        public string ListOrder;
    }

}