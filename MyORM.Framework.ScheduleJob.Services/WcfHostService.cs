using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Services
{
    /// <summary>
    /// WCF 的 ScheduleJob 服務
    /// </summary>
    public class WcfHostService
    {
        public static ServiceHost WcfHost = null;
        /// <summary>
        /// 啟動 WCF 的 ScheduleJob 服務.
        /// </summary>
        public static void StartWcfHost()
        {
            string TcpPort = ConfigurationManager.AppSettings["JobManagePort"];

            Uri baseAddress = new Uri(string.Format("http://localhost:{0}/ScheduleJobService", TcpPort));

            WcfHost = new ServiceHost(typeof(ScheduleJobWcfService), baseAddress);
            // Enable metadata publishing.
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            WcfHost.Description.Behaviors.Add(smb);
            //啟動 WCF 服務
            WcfHost.Open();
        }
        /// <summary>
        /// 停止 WCF 服務
        /// </summary>
        public static void StopWcfHost()
        {
            WcfHost.Close();
        }
    }
}
