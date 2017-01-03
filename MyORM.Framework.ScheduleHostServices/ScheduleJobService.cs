using MyORM.Framework.ScheduleJob.Engine;
using MyORM.Framework.ScheduleJob.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleHostServices
{
    public partial class MxicScheduleJobServices : ServiceBase
    {
        public MxicScheduleJobServices()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            MainThreadContext.Main = new MainThread();
            MainThreadContext.Main.StartService();
            //啟動 WCF 服務
            WcfHostService.StartWcfHost();
        }

        protected override void OnStop()
        {
            MainThreadContext.Main.StopService();
            //停止 WCF 服務
            WcfHostService.StopWcfHost();
        }
    }
}
