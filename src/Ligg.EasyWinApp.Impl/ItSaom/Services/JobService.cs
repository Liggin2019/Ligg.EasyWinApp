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
        internal void InitData(int jobId)
        {
            try
            {
                if (JobServiceData.Job is null)

                {
                    JobServiceData.Init(jobId);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitData Error: " + ex.Message);
            }
        }

        internal string GetTaskList()
        {
            try
            {
                var tasks = JobServiceData.Tasks;

                foreach (var task in tasks)
                {
                    if (task.Status == (int)TaskStatus.Processing) task.ListOrder = "a";
                    if (task.Status == (int)TaskStatus.Waiting) task.ListOrder = "b";
                    if (task.Status == (int)TaskStatus.Completed) task.ListOrder = "c";
                }

                var taskList = "";
                var tasks1 = tasks.OrderBy(x => x.ListOrder);
                foreach (var task in tasks1)
                {
                    taskList = taskList + task.DisplayName + " - " + EnumHelper.GetNameById<TaskStatus>(task.Status) + "\n";
                }
                return taskList;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetTaskList Error: " + ex.Message);
            }
        }

        internal void ClearTasksStatus()
        {
            try
            {
                var tasks = JobServiceData.Tasks;

                foreach (var task in tasks)
                {
                    task.Status = (int)TaskStatus.Waiting;
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetTaskList Error: " + ex.Message);
            }
        }

        internal string GetJobExecType()
        {
            try
            {
                var job = JobServiceData.Job;
                return job.ExecType.ToString();

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetExecType Error: " + ex.Message);
            }
        }

        internal string GetJobExecParams()
        {
            try
            {
                var job = JobServiceData.Job;
                return job.ExecParams.ToString();

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetExecParams Error: " + ex.Message);
            }
        }

        internal string GetJobDisplayName()
        {
            try
            {
                var job = JobServiceData.Job;
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

        internal string GetTaskAction(int taskNo)
        {
            try
            {
                var task = JobServiceData.Tasks.Find(x => x.No == taskNo);
                return task.Action;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetTaskAction Error: " + ex.Message);
            }
        }

        internal int GetTaskCount()
        {
            try
            {
                return JobServiceData.Tasks.Count;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetTaskCount Error: " + ex.Message);
            }
        }

        internal void SetTaskCompleted(int no)
        {
            try
            {
                var task = JobServiceData.Tasks.Find(x => x.No == no);
                task.Status = (int)Ligg.Base.DataModel.Enums.TaskStatus.Completed;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SetTaskCompleted Error: " + ex.Message);
            }
        }

        internal void SetTaskProcessing(int no)
        {
            try
            {
                var task = JobServiceData.Tasks.Find(x => x.No == no);
                task.Status = (int)Ligg.Base.DataModel.Enums.TaskStatus.Processing;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SetTaskProcessing Error: " + ex.Message);
            }
        }

    }

    internal static class JobServiceData
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        public static Job Job;
        public static List<Annex> JobAnnexes;
        public static List<Task> Tasks;

        public static void Init(int jobId)
        {
            try
            {
                if (Job is null)
                {
                    var xmlPath = Configuration.DataDir + "\\JobService\\Jobs";
                    var xmlMgr = new XmlHandler(xmlPath);
                    var jobs = xmlMgr.ConvertToObject<List<Job>>();
                    Job = jobs.Find(x => x.Id == jobId);

                    var xmlPath1 = Configuration.DataDir + "\\JobService\\JobAnnexes";
                    var xmlMgr1 = new XmlHandler(xmlPath1);
                    var jobAnnexes = xmlMgr1.ConvertToObject<List<Annex>>();
                    JobAnnexes = jobAnnexes.FindAll(x => x.MasterId == jobId);

                    var xmlPath2 = Configuration.DataDir + "\\JobService\\Tasks";
                    var xmlMgr2 = new XmlHandler(xmlPath2);
                    var tasks = xmlMgr2.ConvertToObject<List<Task>>();
                    Tasks = tasks.Where(x => x.ParentId == jobId).OrderBy(x => x.ActOrder).ToList();
                    var i = 0;
                    foreach (var task in Tasks)
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
        public int ParentId;
        public int No;
        public int Status;
        public string Name;
        public string DisplayName;
        public string Action;
        public string ActOrder;
        public string ListOrder;
    }

}