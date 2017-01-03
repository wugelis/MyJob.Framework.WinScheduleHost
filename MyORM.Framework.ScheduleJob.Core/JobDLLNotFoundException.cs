using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Core
{
    /// <summary>
    /// 找不到 Job DLL 所引發的錯誤.
    /// </summary>
    public class JobDLLNotFoundException: JobExecuteException
    {
        /// <summary>
        /// JobDLLNotFoundException Constructor
        /// </summary>
        /// <param name="Message"></param>
        public JobDLLNotFoundException(string Message):base(Message) { }
        /// <summary>
        /// JobDLLNotFoundException Constructor
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="InnerException"></param>
        public JobDLLNotFoundException(string Message, Exception InnerException) : base(Message, InnerException) { }
    }
}
