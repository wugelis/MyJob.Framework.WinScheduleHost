using MyORM.Framework.ScheduleJob.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Engine
{
    /// <summary>
    /// RunningTable 服務執行狀態
    /// 提供查詢所有有效服務的執行狀態 (閒置：Idle；啟動中：Starting；執行中：Running)
    /// </summary>
    public class RunningTable
    {
        /// <summary>
        /// 所有有效服務
        /// </summary>
        public static RunningJobStatus[] RunningJobStatuss { get; set; }
        /// <summary>
        /// 設定 Job State.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="status"></param>
        public static void SetJobStatus(string jobId, JobStatus status)
        {
            RunningJobStatus rs = RunningJobStatuss.Where(c => c.JobId==jobId).FirstOrDefault();
            if (rs != null)
                rs.Status = status;
        }
    }
}
