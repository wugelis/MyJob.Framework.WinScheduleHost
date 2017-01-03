using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Core
{
    /// <summary>
    /// Schedule Job Method 內執行時所發生的錯誤.
    /// </summary>
    public class JobExecuteException: Exception
    {
        /// <summary>
        /// JobExecuteException Constructor
        /// </summary>
        /// <param name="Message"></param>
        public JobExecuteException(string Message):base(Message) { }
        /// <summary>
        /// JobExecuteException Constructor
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="InnerException"></param>
        public JobExecuteException(string Message, Exception InnerException) : base(Message, InnerException) { }
        /// <summary>
        /// 錯誤發生時間
        /// </summary>
        public DateTime Datetime { get; set; }
        /// <summary>
        /// Current Identity username for Web Handler.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用戶端要求的 Job DLL 的實體位置.
        /// </summary>
        public string HostLocation { get; set; }
        /// <summary>
        /// 呼叫的 DLL 資訊.
        /// </summary>
        public string MethodInfo { get; set; }
    }
}
