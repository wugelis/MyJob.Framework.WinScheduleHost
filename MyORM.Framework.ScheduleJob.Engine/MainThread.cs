using MyORM.Framework.ScheduleJob.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyORM.Framework.ScheduleJob.Engine
{
    /// <summary>
    /// 服務的主執行序
    /// 存放在 ThreadPool 中.
    /// Create by Gelis at 2014/11/04.
    /// </summary>
    public class MainThread
    {
        #region Members
        /// <summary>
        /// 服務旗標
        /// </summary>
        private bool stopping = true;
        /// <summary>
        /// 當服務啟動時，是否要執行服務停止時，未執行的過期的工作
        /// </summary>
        private bool _doExpiredJob = false;
        /// <summary>
        /// 從服務開始執行起，是否已經執行過未執行的過期的工作，已執行過就會被設為 true.
        /// </summary>
        private bool _isDoExpiredJob = false;
        private bool _isDoExpiredJobCurrent = false;
        /// <summary>
        /// 服務狀態信號物件.
        /// </summary>
        private ManualResetEvent stoppedEvent;
        /// <summary>
        /// 手動執行使用的 WorkerThread 物件.
        /// </summary>
        private WorkerThread _worker = null;
        #endregion

        /// <summary>
        /// MainThread Constructor.
        /// </summary>
        public MainThread()
        {
            //初始 停止旗標 
            this.stopping = false;
            //初始化執行序信號狀態物件
            this.stoppedEvent = new ManualResetEvent(false);
        }
        /// <summary>
        /// 初始化 RunningTable
        /// </summary>
        private void InitialRunningTable()
        {
            UpdateRunningJobStatus();
        }

        private static void UpdateRunningJobStatus()
        {
            RunningTable.RunningJobStatuss = (from run in RunningJobs.GetAllJobs()
                                              where run.Enabled
                                              select new RunningJobStatus()
                                              {
                                                  JobId = run.JobId,
                                                  Status = JobStatus.Idle
                                              }).ToArray();
        }
        /// <summary>
        /// 初始化 & 讀取 Config
        /// </summary>
        private void InitialConfig()
        {
            _doExpiredJob = 
                string.IsNullOrEmpty(ConfigurationManager.AppSettings["DoExpiredJob"]) ? 
                    false : bool.Parse(ConfigurationManager.AppSettings["DoExpiredJob"]);
        }
        /// <summary>
        /// 啟動服務
        /// </summary>
        /// <returns></returns>
        public bool StartService()
        {
            InitialRunningTable();

            InitialConfig();

            this.stopping = false;
            return ThreadPool.QueueUserWorkItem((state) => { ServiceWorkerThread(state); });
        }

        /// <summary>
        /// 停止服務
        /// </summary>
        public void StopService()
        {
            this.stopping = true;
            this.stoppedEvent.WaitOne();
        }
        int icount = 0;
        /// <summary>
        /// Schedule Job 主要執行序 (Main Thread).
        /// </summary>
        /// <param name="state"></param>
        protected virtual void ServiceWorkerThread(object state)
        {
            while (!this.stopping)
            {
                if(_doExpiredJob)
                {
                    if(!_isDoExpiredJob)
                    {
                        _isDoExpiredJobCurrent = true;

                        JobData jobByNonExec = RunningJobs.GetCurrentRunningJobByNonExec(); //取得上一次未執行的排程工作
                        if(jobByNonExec != null)
                        {
                            WorkerThread workerNonExec = new WorkerThread();
                            RunningTable.SetJobStatus(jobByNonExec.JobId, JobStatus.Starting);
                            workerNonExec.StartNewScheduleJob(jobByNonExec);
                        }
                        else
                        {
                            _isDoExpiredJob = true;
                            _isDoExpiredJobCurrent = false;
                        }
                    }
                }

                if (_isDoExpiredJobCurrent) //如果系統還在執行上次未執行工作，就先不執行當前的工作.
                    continue;

                //檢查 RunningJob Table 裡是否有符合目前條件，可執行的工作.
                JobData job = RunningJobs.GetCurrentRunningJob();
                if(job!=null)
                {
                    //MessageBox.Show(string.Format("有一個工作 {0}", job.JobName));
                    WorkerThread worker = new WorkerThread();
                    RunningTable.SetJobStatus(job.JobId, JobStatus.Starting);
                    worker.StartNewScheduleJob(job);
                }
                
                //在 Main Thread 中，每五秒鐘更新一次 RunningJobStatus.
                UpdateRunningJobStatus();

                Thread.Sleep(5000);  // Simulate some lengthy operations.

            }
            //迴圈停止，解除執行序鎖定
            this.stoppedEvent.Set();
        }

        public void StartNewScheduleJob(JobData jobData)
        {
            if (_worker == null)
                _worker = new WorkerThread();

            _worker.StartNewScheduleJob(jobData);
        }
    }
}
