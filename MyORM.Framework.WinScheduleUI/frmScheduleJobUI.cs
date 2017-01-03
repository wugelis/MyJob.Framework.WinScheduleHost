using MyORM.Framework.ScheduleJob.Store;
using MyORM.Framework.WinScheduleUI.ScheduleJobServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyORM.Framework.WinScheduleUI
{
    public partial class frmScheduleJobUI : Form
    {
        /// <summary>
        /// 靜態 的 公開 資料
        /// </summary>
        public static ListJobDatas Jobs = null;
        public frmScheduleJobUI()
        {
            InitializeComponent();
        }
        private string JobXmlFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ListJobDatas.xml");
        private void frmScheduleJobUI_Load(object sender, EventArgs e)
        {
            if(!File.Exists(JobXmlFile))
            {
                if(DialogResult.Yes == MessageBox.Show("找不到 ListJobDatas.xml 檔案，要自己尋找嗎？", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    DialogResult result = openFileDialog1.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        JobXmlFile = openFileDialog1.FileName;
                    }
                }
            }
            Jobs  = JobHelper.LoadXmlFromFileSystem(JobXmlFile);

            gvJobGridView.AutoGenerateColumns = false;
            gvJobGridView.DataSource = Jobs.JobDatas;
        }

        private void btnAddJob_Click(object sender, EventArgs e)
        {
            frmAddOrEdit frmAdd = new frmAddOrEdit();
            frmAdd.ShowDialog();
            //重新顯示資料
            gvJobGridView.DataSource = null;
            gvJobGridView.DataSource = Jobs.JobDatas;
        }

        private void btnEditJob_Click(object sender, EventArgs e)
        {
            string JobId = GetJobIdFromSelectRow();
            if(!string.IsNullOrEmpty(JobId))
            {
                frmAddOrEdit frmAdd = new frmAddOrEdit(JobId);
                frmAdd.ShowDialog();
                gvJobGridView.DataSource = null;
                gvJobGridView.DataSource = Jobs.JobDatas;
            }
            else
            {
                MessageBox.Show("請選擇一筆 Job 資料。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStartNewJob_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("確定執行？", this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)==DialogResult.OK)
            {
                btnStartNewJob.Enabled = false;

                JobData SelectJobData = GetJobDataBySelectRow();

                ScheduleJobServiceClient context = new ScheduleJobServiceClient();
                try
                {
                    if (SelectJobData == null)
                    {
                        MessageBox.Show("請選擇一筆 Job 資料。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    context.StartNewScheduleJob(SelectJobData);
                    MessageBox.Show(string.Format("Job \"{0}\" 執行成功！", SelectJobData.JobId), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Job \"{0}\" 執行失敗！錯誤訊息：{1}", SelectJobData.JobId, ex.Message), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btnStartNewJob.Enabled = true;
                }
            }
        }

        private void btnDelJob_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(GetJobIdFromSelectRow()))
            {
                MessageBox.Show("請選擇一筆 Job 資料。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ;
            }
            if(DialogResult.OK == MessageBox.Show("是否要刪除此項工作？", this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                frmScheduleJobUI.Jobs.JobDatas.RemoveAt(gvJobGridView.CurrentRow.Index);
                gvJobGridView.DataSource = null;
                gvJobGridView.DataSource = Jobs.JobDatas;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int result = JobHelper.SaveXml2FileSystem(Jobs, JobXmlFile);
            if (result > 0)
                MessageBox.Show("儲存成功！", this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            else
                MessageBox.Show("儲存失敗！", this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        private string GetJobIdFromSelectRow()
        {
            string JobId = string.Empty;

            JobData job = GetJobDataBySelectRow();
            if(job!=null)
            {
                JobId = job.JobId;
            }
            return JobId;
        }

        private JobData GetJobDataBySelectRow()
        {
            JobData job = null;

            DataGridViewSelectedRowCollection myRow = gvJobGridView.SelectedRows;
            if (myRow != null && myRow.Count>0)
            {
                //預設只能抓到第一筆資料
                DataGridViewRow row = myRow[0] as DataGridViewRow;
                JobData CurrentJob = Jobs.JobDatas.Where(c => c.JobId == row.Cells[0].Value.ToString()).FirstOrDefault();
                job = CurrentJob;
            }
            return job;
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            frmSysSetting setting = new frmSysSetting();
            setting.Show();
        }
    }
}
