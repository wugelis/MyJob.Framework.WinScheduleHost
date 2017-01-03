using Mxic.Framework.ScheduleJob.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mxic.Framework.ScheduleHostServices.WcfServices
{
    public class ScheduleJobWcfService : IScheduleJobService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobData"></param>
        public void IScheduleJobService.StartNewScheduleJob(JobData jobData)
        {
            MxicScheduleJobServices.main.StartNewScheduleJob(jobData);
        }
    }
}
