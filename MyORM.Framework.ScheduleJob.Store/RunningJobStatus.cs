using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Store
{
    /// <summary>
    /// 管理 Runtime Job 執行狀態
    /// </summary>
    [DataContract]
    public class RunningJobStatus
    {
        /// <summary>
        /// Job Id (工作識別碼)
        /// </summary>
        [DataMember]
        public string JobId { get; set; }
        /// <summary>
        /// Job 執行狀態
        /// </summary>
        [DataMember]
        public JobStatus Status { get; set; }
    }
    /// <summary>
    /// 服務狀態 enumerable
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// 服務閒置狀態.
        /// </summary>
        Idle = 0,
        /// <summary>
        /// 服務啟動中.
        /// </summary>
        Starting = 1,
        /// <summary>
        /// 服務執行中.
        /// </summary>
        Running = 2
    }
}
