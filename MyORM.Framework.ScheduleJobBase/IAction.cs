using MyORM.Framework.ScheduleJob.Store;
using Dummies.ServerComponent;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJobBase
{
    /// <summary>
    /// Schedule Job 動作處理介面.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Job 工作執行開始的方法.
        /// </summary>
        /// <param name="job">目前執行的 Job 相關資料.</param>
        /// <param name="writeLogAttr">WriteLog 的設定相關資料</param>
        /// <param name="writeExAttr">WriteExLog 的設定相關資料</param>
        /// <param name="errorLevelAttr">ErrorLevel 的設定相關資料</param>
        void DoJob(
            JobData job,
            LogAttributeData<WriteLogAttribute> writeLogAttr, 
            LogAttributeData<WriteExceptionLogAttribute> writeExAttr,
            LogAttributeData<ErrorLevelAttribute> errorLevelAttr,
            Logger loggerInner,
            Logger loggerInnerErr);
    }
}
