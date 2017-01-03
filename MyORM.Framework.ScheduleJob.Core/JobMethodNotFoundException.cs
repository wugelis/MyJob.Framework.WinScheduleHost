using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Core
{
    /// <summary>
    /// 找不到 Job Method 所引發的錯誤
    /// </summary>
    public class JobMethodNotFoundException: JobExecuteException
    {
        /// <summary>
        /// JobMethodNotFoundException Constructor.
        /// </summary>
        /// <param name="Message"></param>
        public JobMethodNotFoundException(string Message):base(Message) { }
        /// <summary>
        /// JobMethodNotFoundException Constructor.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="InnerException"></param>
        public JobMethodNotFoundException(string Message, Exception InnerException) : base(Message, InnerException) { }
    }
}
