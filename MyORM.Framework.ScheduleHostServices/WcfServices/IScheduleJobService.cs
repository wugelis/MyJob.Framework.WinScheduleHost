using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mxic.Framework.ScheduleHostServices.WcfServices
{
    [ServiceContract]
    public interface IScheduleJobService
    {
        [OperationContract]
        void StartNewScheduleJob(JobData jobData);
    }
}
