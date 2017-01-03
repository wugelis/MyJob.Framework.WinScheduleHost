using MyORM.Framework.ScheduleJob.Engine;
using MyORM.Framework.ScheduleJob.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Services
{
    /// <summary>
    /// 遠端管理使用的 WCF 公開介面 (For Admin)
    /// 功能：
    ///     1. 提供遠端啟動 Job.
    ///     2. 查詢目前所有有效服務的執行狀態 (閒置：Idle；啟動中：Starting；執行中：Running)
    /// </summary>
    public class ScheduleJobWcfService : IScheduleJobService
    {
        /// <summary>
        /// 提供遠端啟動 Job (手動啟動)
        /// </summary>
        /// <param name="jobData"></param>
        public void StartNewScheduleJob(JobData jobData)
        {
            MainThreadContext.Main.StartNewScheduleJob(jobData);
        }

        /// <summary>
        /// 查詢目前所有有效服務的執行狀態 (閒置：Idle；啟動中：Starting；執行中：Running)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RunningJobStatus> GetCurrentRunningJobStatus()
        {
            return RunningTable.RunningJobStatuss.AsEnumerable();
        }
    }
}
