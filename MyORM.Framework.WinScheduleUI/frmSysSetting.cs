using MyORM.Framework.ScheduleJob.Store;
using MyORM.Framework.WinScheduleUI.ScheduleJobServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyORM.Framework.WinScheduleUI
{
    public partial class frmSysSetting : Form
    {
        private ScheduleJobServiceClient _jobServiceClient = null;

        public frmSysSetting()
        {
            InitializeComponent();

            timerUpdateData.Enabled = true;

            _jobServiceClient = new ScheduleJobServiceClient();
        }

        ~frmSysSetting()
        {
            timerUpdateData.Enabled = false;
        }

        private void timerUpdateData_Tick(object sender, EventArgs e)
        {
            try
            {
                QueryCurrentRuningJob();
            }
            catch(Exception ex)
            {
                timerUpdateData.Enabled = false;
                MessageBox.Show(string.Format("連結不到 WCF 遠端管理服務. 錯誤訊息：{0}", ex.Message), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void QueryCurrentRuningJob()
        {
            IEnumerable<RunningJobStatus> JobStatus = _jobServiceClient.GetCurrentRunningJobStatus().AsEnumerable();
            labAvariableJobs.Text = JobStatus.Count().ToString();
            labCurrentRunning.Text = JobStatus.Where(c => c.Status == ScheduleJob.Store.JobStatus.Running).Count().ToString();
            gvJobStatus.DataSource = JobStatus;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
