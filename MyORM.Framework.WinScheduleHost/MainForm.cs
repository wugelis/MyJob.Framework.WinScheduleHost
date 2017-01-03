using MyORM.Framework.ScheduleJob.Core;
using MyORM.Framework.ScheduleJob.Engine;
using MyORM.Framework.ScheduleJob.Services;
using MyORM.Framework.ScheduleJob.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyORM.Framework.WinScheduleHost
{
    /// <summary>
    /// Schedule Job 管理介面 UI
    /// 可提供 Win32 服務 相關設定 (ReTry 次數Store儲存位置等)
    /// Create by Gelis at 2014/11/01.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Members
        /// <summary>
        /// 服務的主執行序
        /// </summary>
        //private MainThread main = new MainThread();
        /// <summary>
        /// Schedule Job 排程工作的執行序.
        /// </summary>
        //private WorkerThread worker = new WorkerThread();
        #endregion

        public MainForm()
        {
            InitializeComponent();

            MainThreadContext.Main = new MainThread();
            MainThreadContext.Worker = new WorkerThread();
        }

        /// <summary>
        /// 手動產生一個 排程 工作.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewJobThreadPool_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Factory.StartNew(() =>
                    {
                        JobData job = new JobData()
                        {
                            JobId = "WriteLogJob",
                            JobName = "WriteLogJob",
                            DLLName = "MyORM.Framework.ScheduleJob.WriteLogTest",
                            NameSpace = "MyORM.Framework.ScheduleJob.WriteLogTest",
                            ClassName = "NWriteLog"
                        };
                        MainThreadContext.Worker.StartNewScheduleJob(job);
                    });
            }
            catch(JobDLLNotFoundException dex)
            {
                MessageBox.Show(dex.Message);
            }
            catch(JobMethodNotFoundException mex)
            {
                MessageBox.Show(mex.Message);
            }
            catch(IActionNotImplementException aex)
            {
                MessageBox.Show(aex.Message);
            }
            catch(JobExecuteException jex)
            {
                //StopThread();
                MessageBox.Show(jex.Message);
            }
            catch(Exception ex)
            {
                //StopThread();
                MessageBox.Show(ex.Message);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void btnStartService_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = !MainThreadContext.Main.StartService();
            timerJobUpdate.Enabled = btnStopService.Enabled = true;
            //啟動 WCF 服務
            WcfHostService.StartWcfHost();
        }

        private void btnStopService_Click(object sender, EventArgs e)
        {
            MainThreadContext.Main.StopService();
            //停止 WCF 服務
            WcfHostService.StopWcfHost();
            btnStartService.Enabled = true;
            timerJobUpdate.Enabled = (sender as Button).Enabled = false;
        }

        private void btnQueryJob_Click(object sender, EventArgs e)
        {
            QueryCurrentRuningJob();
        }

        private void QueryCurrentRuningJob()
        {
            if (RunningTable.RunningJobStatuss == null)
                return;

            var result = RunningTable.RunningJobStatuss.AsEnumerable()
                .Where(c => c.Status == JobStatus.Running);

            var resultAll = RunningTable.RunningJobStatuss.AsEnumerable();

            labCurrent.Text = result.Count().ToString();
            labAvarible.Text = resultAll.Count().ToString();
        }

        private void timerJobUpdate_Tick(object sender, EventArgs e)
        {
            QueryCurrentRuningJob();
        }
    }
}
