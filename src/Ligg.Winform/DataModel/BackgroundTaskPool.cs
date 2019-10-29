using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.Winform.Resources;

namespace Ligg.Winform.DataModel
{
    //http://www.cnblogs.com/grenet/archive/2011/12/21/2289014.html
    public class BackgroundTaskPool
    {
        public delegate Object WorkDelegate(Object paramObj);

        private readonly int _workerCount;
        private readonly BackgroundWorker[] _workers;
        private int _id;
        private readonly Object _threadLock;

        private readonly List<Task> _tasks;
        public List<Task> Tasks { get { return _tasks; } }

        public event EventHandler OnTaskJoin;
        public event EventHandler OnTaskCancel;
        public event EventHandler OnTaskRestart;
        public event EventHandler OnTaskDelete;
        public event EventHandler OnTaskStart;
        public event EventHandler OnTaskComplete;
        public event EventHandler OnAllTasksComplete;


        public BackgroundTaskPool(int threadCount)
        {
            _workerCount = threadCount;
            _workers = new BackgroundWorker[threadCount];
            int i;
            for (i = 0; i < threadCount; i++)
            {
                _workers[i] = new BackgroundWorker();
                _workers[i].DoWork += new DoWorkEventHandler(DoWork);
                _workers[i].RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);
                _workers[i].WorkerSupportsCancellation = true;

            }
            _tasks = new List<Task>();
            _id = 0;
            _threadLock = new Object();
        }

        //#method
        public int Join(WorkDelegate workDelegate, Object paramObj)
        {
            lock (_threadLock)
            {
                _id++;
                var task = new Task(_id, paramObj, workDelegate);
                task.JoinTime = SystemTimeHelper.Now();
                task.Status = TaskStatus.Waiting;
                _tasks.Add(task);

                if (OnTaskJoin != null)
                {
                    var args = new TaskIdParamObjEventArgs(_id, paramObj);
                    OnTaskJoin(this, args);
                }

                var processingTasks = _tasks.Where(x => x.Status == TaskStatus.Processing);
                var processingTaskIdentityFlagList = processingTasks.Where(x=>!x.IdentityFlag.IsNullOrEmpty()).Select(x => x.IdentityFlag).Distinct().ToList();
                if (!task.IdentityFlag.IsNullOrEmpty() & processingTaskIdentityFlagList.Contains(task.IdentityFlag))
                {
                    return _id;
                }

                int i;
                for (i = 0; i < _workerCount; i++)
                {
                    if (!_workers[i].IsBusy)
                    {
                        task.WorkerId = i;
                        task.Status = TaskStatus.Processing;
                        RaiseDoWork(_workers[i], task);
                        break;
                    }
                }
                return _id;
            }
        }

        public void Delete(List<int> idList)
        {
            var tasks = _tasks.FindAll(x => (
                x.Status == TaskStatus.Failed | x.Status == TaskStatus.Completed | x.Status == TaskStatus.Canceled) 
                & idList.Contains(x.Id));
            if (tasks.Count>0)
            {
                foreach (var task in tasks)
                {
                    _tasks.Remove(task);
                }

                if (OnTaskDelete != null)
                {
                    var args = new TaskIdEventArgs(0);
                    OnTaskDelete(this, args);
                }
            }
        }

        public void Cancel(int id)
        {
            var task = _tasks.Find(x => (x.Status == TaskStatus.Waiting) & x.Id == id);
            if (task != null)
            {
                task.Status = TaskStatus.Canceled;
                if (OnTaskCancel != null)
                {
                    var args = new TaskIdEventArgs(task.Id);
                    OnTaskCancel(this, args);
                }
            }

        }

        public void ReStart(int id)
        {
            var task = _tasks.Find(x => (x.Status == TaskStatus.Canceled | x.Status == TaskStatus.Failed | x.Status == TaskStatus.Completed) & x.Id == id);
            if (task != null)
            {
                var processingTasks = _tasks.Where(x => x.Status == TaskStatus.Processing);
                var processingTaskIdentityFlagList = processingTasks.Where(x => !x.IdentityFlag.IsNullOrEmpty()).Select(x => x.IdentityFlag).Distinct().ToList();
                if(task.IdentityFlag.IsNullOrEmpty() | !processingTaskIdentityFlagList.Contains(task.IdentityFlag))
                {
                    int i;
                    bool isWorkerPoolBusy = true;
                    for (i = 0; i < _workerCount; i++)
                    {
                        if (!_workers[i].IsBusy)
                        {
                            RaiseDoWork(_workers[i], task);
                            task.WorkerId = i;
                            isWorkerPoolBusy = false;
                            break;
                        }
                    }

                    if (isWorkerPoolBusy)
                    {
                        task.Status = TaskStatus.Waiting;
                    }
                }
                else
                {
                    task.Status = TaskStatus.Waiting;
                }

                if (OnTaskRestart != null)
                {
                    var args = new TaskIdEventArgs(_id);
                    OnTaskRestart(this, args);
                }
            }
        }

        //#proc
        private void RaiseDoWork(BackgroundWorker worker, Task task)
        {
            task.Status = TaskStatus.Processing;
            task.StartTime = SystemTimeHelper.Now();
            worker.RunWorkerAsync(task);
            if (OnTaskStart != null)
            {
                OnTaskStart(this, new TaskIdParamObjEventArgs(task.Id, task.ParamObj));
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            var task = (Task)(e.Argument);
            e.Result = new TaskResult(task.Id, task.WorkerId, task.Work.Invoke(task.ParamObj));
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (_threadLock)
            {
                var result = (TaskResult)(e.Result);
                var completedTask = _tasks.Find(x => x.Id == result.TaskId);
                completedTask.Status = TaskStatus.Completed;
                completedTask.CompleteTime = SystemTimeHelper.Now();
                if (OnTaskComplete != null)
                {
                    OnTaskComplete(this, new TaskIdParamObjResultObjEventArgs(result.TaskId, completedTask.ParamObj, result.ResultObj));
                }

                var processingTasks = _tasks.Where(x => x.Status == TaskStatus.Processing);
                var processingTaskIdentityFlagList = processingTasks.Where(x=>!x.IdentityFlag.IsNullOrEmpty()).Select(x => x.IdentityFlag).Distinct().ToList();
                var toBeProcessedTasks = _tasks.FindAll(x => (x.Status == TaskStatus.Waiting) & (x.IdentityFlag.IsNullOrEmpty()|!processingTaskIdentityFlagList.Contains(x.IdentityFlag)));
                if (toBeProcessedTasks.Count > 0)
                {
                    var worker = (BackgroundWorker)(sender);
                    if (worker.IsBusy == false)
                    {
                        var toBeProcessedTask = toBeProcessedTasks.FirstOrDefault();
                        if (toBeProcessedTask != null)
                        {
                            toBeProcessedTask.WorkerId = result.WorkerId;
                            RaiseDoWork(worker, toBeProcessedTask);
                        }
                    }
                }

                var waitingOrProcessingTasks = _tasks.FindAll(x => x.Status == TaskStatus.Waiting | x.Status == TaskStatus.Processing);
                if (waitingOrProcessingTasks.Count == 0)
                {
                    if (OnAllTasksComplete != null)
                    {
                        OnAllTasksComplete(this, new EventArgs());
                    }
                }
            }
        }

        //#func
        public void SetTaskFailed(int id, string text)
        {
            var task = _tasks.Find(x => x.Id == id);
            if (task != null)
            {
                task.Status = TaskStatus.Failed;
                task.FailureReason = text;
            }
        }

        public void SetTaskParams(int id, int type, string des, string des1)
        {
            var task = _tasks.Find(x => x.Id == id);
            if (task != null)
            {
                task.Type = type;
                task.Description = des;
                task.Description1 = des1;
            }
        }

        public void SetTaskIdentityFlag(int id, string text)
        {
            var task = _tasks.Find(x => x.Id == id);
            if (task != null)
            {
                task.IdentityFlag = text;
            }
        }

        public void SetTaskRemark(int id, string text)
        {
            var task = _tasks.Find(x => x.Id == id);
            if (task != null)
            {
                task.Remark = text;
            }
        }


        public List<BackgroundTask> GetTasks()
        {
            try
            {
                var bgTasks = new List<BackgroundTask>();
                foreach (var task in _tasks)
                {
                    var bgTask = new BackgroundTask();
                    bgTask.Id = task.Id;
                    bgTask.Type = task.Type;
                    bgTask.Description = task.Description;
                    bgTask.Description1 = task.Description1;

                    bgTask.Remark = task.Remark;
                    bgTask.Status = (int)task.Status;
                    var statusName = EnumHelper.GetNameById<TaskStatus>(bgTask.Status);
                    var statusDspName = "";
                    if (!statusName.IsNullOrEmpty())
                    {
                        statusDspName = WinformRes.ResourceManager.GetString("BackgroundTaskStatus_" + statusName);
                        if (statusDspName.IsNullOrEmpty()) statusDspName = statusName;
                    }
                    else
                    {
                        statusDspName = WinformRes.Unknown;
                    }
                    bgTask.StatusName = statusDspName;

                    bgTask.JoinTime = task.JoinTime;
                    bgTask.StartTime = task.StartTime;
                    bgTask.CompleteTime = task.CompleteTime;

                    bgTasks.Add(bgTask);
                }
                return bgTasks;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetTasks Error: " + ex.Message);
            }
        }

        //#internal class
        public class BackgroundTask
        {
            public int Id;
            public int Type;
            public string TypeName;
            public string Description;
            public string Description1;
            public string Remark;
            public int Status;
            public string StatusName;
            public DateTime? JoinTime;
            public DateTime? StartTime;
            public DateTime? CompleteTime;
            public string FailureReason;
        }

        public class Task
        {
            public int Id { get; private set; }
            public int WorkerId { get; set; }
            public int Type { get; set; }
            public string Description { get; set; }
            public string Description1 { get; set; }
            public string Remark { get; set; }
            public string FailureReason { get; set; }
            public string IdentityFlag { get; set; }
            public TaskStatus Status { get; set; }
            public DateTime? JoinTime { get; set; }
            public DateTime? StartTime { get; set; }
            public DateTime? CompleteTime { get; set; }

            public Object ParamObj { get; private set; }
            public readonly WorkDelegate Work;
            public Task(int id, Object param, WorkDelegate work)
            {
                Work = work;
                ParamObj = param;
                Id = id;
            }
        }

        private class TaskResult
        {
            public int TaskId { get; private set; }
            public int WorkerId { get; set; }
            public Object ResultObj { get; private set; }
            public TaskResult(int taskId, int workerId, Object resultObj)
            {
                ResultObj = resultObj;
                TaskId = taskId;
                WorkerId = workerId;
            }
        }

        public class TaskIdEventArgs : EventArgs
        {
            public int Id { get; private set; }
            public TaskIdEventArgs(int id)
            {
                Id = id;
            }
        }


        public class TaskIdParamObjEventArgs : EventArgs
        {
            public int Id { get; private set; }
            public Object ParamObj { get; private set; }
            public TaskIdParamObjEventArgs(int id, object param)
            {
                Id = id;
                ParamObj = param;
            }
        }

        public class TaskIdParamObjResultObjEventArgs : EventArgs
        {
            public int Id { get; private set; }
            public Object ParamObj { get; private set; }
            public Object ResultObj { get; private set; }
            public TaskIdParamObjResultObjEventArgs(int id, object param, object result)
            {
                Id = id;
                ParamObj = param;
                ResultObj = result;
            }
        }

        //public enum TaskStatus
        //{
        //    Waiting = 0,
        //    Processing = 1,
        //    Failed = 2,
        //    Completed = 3,
        //    Canceled = 4,
        //}


    }
}
