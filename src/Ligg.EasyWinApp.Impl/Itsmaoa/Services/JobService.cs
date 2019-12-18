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
        //#Init
        internal void Init()
        {
            try
            {
                InitData();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".Init Error: " + ex.Message);
            }
        }

        internal void InitCurrentJob(int jobId, string sGuid)
        {
            try
            {
                Init();

                var job = JobServiceData.Jobs.Find(x => x.Id == jobId);
                var curJob = new CurrentJob { Id = jobId, ShortGuid = sGuid, TaskListIds = job.TaskListIds };

                JobServiceData.CurrentJobs.Add(curJob);
                var taskIdList = job.TaskListIds.ConvertIdsStringToIntegerList<Int32>(',');
                var i = 0;
                foreach (var task in JobServiceData.Tasks.Where(x => taskIdList.Contains(x.Id)))
                {
                    var curTask = new CurrentTask { Id = task.Id, JobId = jobId, ShortGuid = sGuid, No = i, Status = (int)TaskStatus.Waiting };
                    JobServiceData.CurrentTasks.Add(curTask);
                    i++;
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitCurrentJob Error: " + ex.Message);
            }
        }

        internal int GetJobExecType(int jobId)
        {
            try
            {
                var job = JobServiceData.Jobs.Find(x => x.Id == jobId);
                return job.ExecType;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetJobExecType Error: " + ex.Message);
            }
        }


        internal int GetJobExecMode(int jobId)
        {
            try
            {
                var job = JobServiceData.Jobs.Find(x => x.Id == jobId);
                return job.ExecMode;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetJobExecMode Error: " + ex.Message);
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
                if (GlobalConfiguration.SupportMultiCultures)
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


        internal int GetJobTaskCount(int jobId, string sGuid)
        {
            try
            {
                return JobServiceData.CurrentTasks.Where(x => x.JobId == jobId & x.ShortGuid == sGuid).ToList().Count;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetJobTaskCount Error: " + ex.Message);
            }
        }

        internal string GetJobCurrentTaskAction(int jobId, string sGuid, int taskNo)
        {
            try
            {
                var currentTask = JobServiceData.CurrentTasks.Find(x => x.JobId == jobId & x.ShortGuid == sGuid & x.No == taskNo);
                var task = JobServiceData.Tasks.Find(x => x.Id == currentTask.Id);
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

        private void InitData()
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
        public int ExecMode;
        public string ExecParams;
        public string TaskListIds;
    }

    public class Task
    {
        public int Id;
        public string Name;
        public string DisplayName;
        public string Action;
        public string ActOrder;
    }

    public class CurrentJob
    {
        public int Id;
        public string ShortGuid;
        public string TaskListIds;
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