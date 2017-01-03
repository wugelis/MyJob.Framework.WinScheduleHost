using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Engine
{
    /// <summary>
    /// 提供 MainThread 的主要共用執行個體服務 Static.
    /// </summary>
    public class MainThreadContext
    {
        /// <summary>
        /// 服務的主執行序
        /// </summary>
        public static MainThread Main;
        /// <summary>
        /// Schedule Job 排程工作的執行序.
        /// </summary>
        public static WorkerThread Worker;
    }
}
