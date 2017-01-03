using Mxic.Framework.ScheduleJob.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mxic.Framework.ScheduleJobBase
{
    /// <summary>
    /// 提供 Schedule Job 排程工作開發的介面
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// 提供執行 Job 實作的方法.
        /// </summary>
        void Execute();
    }
}
