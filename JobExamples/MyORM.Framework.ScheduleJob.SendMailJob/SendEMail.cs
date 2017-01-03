using MyORM.Framework.ScheduleJobBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.SendMailJob
{
    /// <summary>
    /// 發送 EMail 工作.
    /// </summary>
    public class SendEMail: AbstractProcess
    {
        protected override void PrepareData()
        {
            //base.PrepareData();

        }
        protected override void BeforePrepare()
        {
            //base.BeforePrepare();

        }
        protected override void ProcessData()
        {
            throw new NotImplementedException();
        }
    }
}
