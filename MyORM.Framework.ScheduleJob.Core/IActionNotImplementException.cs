using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Core
{
    /// <summary>
    /// 沒有實作 IAction 介面時引發的錯誤.
    /// </summary>
    public class IActionNotImplementException: JobExecuteException
    {
        /// <summary>
        /// IActionNotImplementException Constructor
        /// </summary>
        /// <param name="Message"></param>
        public IActionNotImplementException(string Message):base(Message) { }
        /// <summary>
        /// IActionNotImplementException Constructor
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="InnerException"></param>
        public IActionNotImplementException(string Message, Exception InnerException) : base(Message, InnerException) { }
    }
}
