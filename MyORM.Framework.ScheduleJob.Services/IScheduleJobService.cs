using MyORM.Framework.ScheduleJob.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Services
{
    /// <summary>
    /// 提供遠端監控、管理使用的 WCF Services 介面.
    /// </summary>
    [ServiceContract]
    public interface IScheduleJobService
    {
        /// <summary>
        /// 提供遠端啟動 Job (手動啟動)
        /// </summary>
        /// <param name="jobData"></param>
        [OperationContract]
        void StartNewScheduleJob(JobData jobData);
        /// <summary>
        /// 查詢目前所有有效服務的執行狀態 (閒置：Idle；啟動中：Starting；執行中：Running)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<RunningJobStatus> GetCurrentRunningJobStatus();
    }
}
