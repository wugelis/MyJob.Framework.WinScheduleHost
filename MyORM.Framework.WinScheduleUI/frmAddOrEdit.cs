using MyORM.Framework.ScheduleJob.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyORM.Framework.WinScheduleUI
{
    public partial class frmAddOrEdit : Form
    {
        /// <summary>
        /// 目前編輯的 JobId
        /// </summary>
        private string _JobId = string.Empty;
        public frmAddOrEdit()
        {
            InitializeComponent();

            PrepareData();

            NewJob2Screen();
        }

        private void NewJob2Screen()
        {
            txtJobId.Enabled = true;
        }

        public frmAddOrEdit(string JobId)
        {
            InitializeComponent();

            PrepareData();
            //保存一份目前正在編輯的 JobId.
            _JobId = JobId;
            //將 該 JobId 資料讀取到畫面上.
            ReadJob2Screen(JobId);
        }
        /// <summary>
        /// 讀取資料到畫面上
        /// </summary>
        private void ReadJob2Screen(string JobId)
        {
            try
            {
                txtJobId.Text = JobId;

                JobData job = frmScheduleJobUI.Jobs.JobDatas.AsEnumerable().Where(c => c.JobId == JobId).FirstOrDefault();
                if (job != null)
                {
                    txtJobName.Text = job.JobName;
                    labDLLName.Text = job.DLLName;
                    labClassName.Text = job.ClassName;
                    txtNameSpace.Text = job.NameSpace;

                    switch (job.UsePeriodType)
                    {
                        case PeriodType.BY_DAILY:
                            if (!string.IsNullOrEmpty(job.ExecuteTime) && job.ExecuteTime != "00:00") //為時間區隔的反覆性週期工作
                            {
                                if (!TimePattern.Match(job.ExecuteTime).Success)
                                {
                                    numHourHH.Focus();
                                    throw new Exception("每 HH:ss 反覆執行中的 (小時:分鐘) 設定不正確。");
                                }
                                //屬於每日，且週期性的固定在 HH:ss 中反覆執行.
                                rbHHss.Enabled = true;
                                SetHHssByJobTime(job.ExecuteTime, numHourHH, numMinuteHH);
                                cbEnableTime.Checked = job.Enabled;
                            }
                            else
                            {
                                //屬於每天執行的排程
                                rbDay.Checked = true;
                                if (!TimePattern.Match(job.StartTime).Success)
                                {
                                    numHourDay.Focus();
                                    throw new Exception("(天) 排程中的 (小時:分鐘) 設定不正確。");
                                }
                                SetHHssByJobTime(job.StartTime, numHourDay, numMinuteDay);
                                cbPeriodDay.Checked = job.IsPeriodJob == PeriodJob.IS_PERIOD;
                                cbEnableDay.Checked = job.Enabled;
                            }
                            break;

                        case PeriodType.BY_WEEKLY:
                            rbWeek.Checked = true;
                            cbExecWeek.SelectedIndex 
                                = cbExecWeek.Items.IndexOf(cbExecWeek.Items.OfType<ListItem>()
                                    .Where(c => c.ValueMember == job.ExceuteWeek.ToString()).FirstOrDefault());

                            SetHHssByJobTime(job.StartTime, numHourWeek, numMinuteWeek);
                            cbPeriodWeek.Checked = job.IsPeriodJob == PeriodJob.IS_PERIOD;
                            cbEnableWeek.Checked = job.Enabled;
                            break;

                        case PeriodType.BY_MONTHLY:
                            rbMonth.Checked = true;
                            cbExecDay.SelectedIndex
                                = cbExecDay.Items.IndexOf(cbExecDay.Items.OfType<ListItem>()
                                    .Where(c => c.ValueMember == job.ExecuteDay.ToString()).FirstOrDefault());

                            SetHHssByJobTime(job.StartTime, numHourMonth, numMinuteMonth);
                            cbPeriodMonth.Checked = job.IsPeriodJob == PeriodJob.IS_PERIOD;
                            cbEnableMonth.Checked = job.Enabled;
                            break;
                    }
                    txtEMailAddress.Text = job.EMailAddress;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("顯示排程工作資料時，發生錯誤！詳細訊息：{0}", ex.Message), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void SetHHssByJobTime(string jobTime, NumericUpDown numHH, NumericUpDown numSS)
        {
            if (!string.IsNullOrEmpty(jobTime))
            {
                string[] STime = jobTime.Split(':');
                numHH.Value = decimal.Parse(STime[0]);
                numSS.Value = decimal.Parse(STime[1]);
            }
        }
        /// <summary>
        /// 將目前畫面上的 Job 資料更新到 frmScheduleJobUI 的 static 變數中.
        /// </summary>
        private void UpdateJob2JobListData()
        {
            try
            {
                if (string.IsNullOrEmpty(_JobId)) //如果在存檔時候，私有變數 _JobId 為空值的話，表示目前為 [新增] 模式
                {
                    //[新增模式]
                    JobData job = new JobData()
                    {
                        JobId = txtJobId.Text,
                        JobName = txtJobName.Text,
                        Enabled = true
                    };

                    //讀取畫面資料到 Job 執行個體 中
                    job = ReadScreen2Job(job);
                    //若 txtDLL.Text 有資料，才更新 DLLName, NameSpace, ClassName 等相關資訊.
                    UpdateJobDLLInfo(job);

                    frmScheduleJobUI.Jobs.JobDatas.Add(job);
                }
                else
                {
                    //[修改模式]
                    //從儲存體取出目前編輯的那一筆 JobId 的資料。
                    JobData job = frmScheduleJobUI.Jobs.JobDatas.AsEnumerable().Where(c => c.JobId == _JobId).FirstOrDefault();
                    job.JobName = txtJobName.Text;
                    //讀取畫面資料到 Job 執行個體 中
                    job = ReadScreen2Job(job);
                    //若 txtDLL.Text 有資料，才更新 DLLName, NameSpace, ClassName 等相關資訊.
                    UpdateJobDLLInfo(job);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("讀取畫面資料時發生錯誤！錯誤訊息={0}", ex.Message), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private JobData ReadScreen2Job(JobData job)
        {
            //job.JobId = txtJobId.Text;
            //job.JobName = txtJobName.Text;

            if (rbMonth.Checked) //月
            {
                job.UsePeriodType = PeriodType.BY_MONTHLY;
                int Day = int.Parse(cbExecDay.Text);
                if (Day > 28)
                {
                    MessageBox.Show(string.Format("注意，您將執行天設為 {0} 號，這有可能會在 2 月時，因為沒有 {0} 號，而導致排程不會被執行。", Day));
                }
                job.ExecuteDay = Day;
                job.StartTime = string.Format("{0}:{1}", numHourMonth.Value.ToString("##").PadLeft(2, '0'), numMinuteMonth.Value.ToString("##").PadLeft(2, '0'));
                job.IsPeriodJob = cbPeriodMonth.Checked ? PeriodJob.IS_PERIOD : PeriodJob.NONE;
                job.Enabled = cbEnableMonth.Checked;
                job.ExecuteTime = "00:00";
            }
            else if (rbWeek.Checked) //週
            {
                job.UsePeriodType = PeriodType.BY_WEEKLY;
                job.ExceuteWeek = int.Parse(cbExecWeek.SelectedValue.ToString());
                job.StartTime = string.Format("{0}:{1}", numHourWeek.Value.ToString("##").PadLeft(2, '0'), numMinuteWeek.Value.ToString("##").PadLeft(2, '0'));
                job.IsPeriodJob = cbPeriodWeek.Checked ? PeriodJob.IS_PERIOD : PeriodJob.NONE;
                job.Enabled = cbEnableWeek.Checked;
                job.ExecuteTime = "00:00";
            }
            else if (rbDay.Checked) //天
            {
                job.UsePeriodType = PeriodType.BY_DAILY;
                job.StartTime = string.Format("{0}:{1}", numHourDay.Value.ToString("##").PadLeft(2, '0'), numMinuteDay.Value.ToString("##").PadLeft(2, '0'));
                job.IsPeriodJob = cbPeriodDay.Checked ? PeriodJob.IS_PERIOD : PeriodJob.NONE;
                job.Enabled = cbEnableDay.Checked;
                job.ExecuteTime = "00:00";
            }
            else if (rbHHss.Checked) //反覆執行時間 (HH:mm)
            {
                job.UsePeriodType = PeriodType.BY_DAILY;
                job.ExecuteTime = string.Format("{0}:{1}", numHourHH.Value.ToString("##").PadLeft(2, '0'), numMinuteHH.Value.ToString("##").PadLeft(2, '0'));
                job.IsPeriodJob = cbPeriodTime.Checked ? PeriodJob.IS_PERIOD : PeriodJob.NONE;
                job.Enabled = cbEnableTime.Checked;
            }
            job.EMailAddress = txtEMailAddress.Text;

            return job;
        }
        /// <summary>
        /// 從 UI 上的資料，重新指定 DLL DLLName, NameSpace, ClassName 等相關資訊.
        /// </summary>
        /// <param name="job"></param>
        private void UpdateJobDLLInfo(JobData job)
        {
            job.DLLName = labDLLName.Text;
            job.ClassName = labClassName.Text;
            job.NameSpace = txtNameSpace.Text;
        }
        /// <summary>
        /// 檢查 Time Format 的 Regular Expression Pattern.
        /// </summary>
        private Regex TimePattern = new Regex("^[0-2][0-9]:[0-5][0-9]");
        /// <summary>
        /// 檢查畫面輸入
        /// </summary>
        /// <returns></returns>
        private string ValidateData()
        {
            Regex rule = null;
            //string ErrMessage = string.Empty;
            if (string.IsNullOrEmpty(txtJobId.Text))
            {
                txtJobId.Focus();
                return "必須輸入 JobId！";
            }

            if (string.IsNullOrEmpty(txtJobName.Text))
            {
                txtJobName.Focus();
                return "必須輸入 JobName！";
            }            
            
            if (labDLLName.Text == "[DLLName]")
                return "請選擇你的 DLL";

            if (labClassName.Text == "[ClassName]")
                return "請選擇你的 DLL";

            if (string.IsNullOrEmpty(txtNameSpace.Text))
                return "命名空間不可以空白。命名空間是指：主要 Job 類別上方定義的 namespace [你的命名空間]";

            if(rbMonth.Checked)
            {
                rule = new Regex(@"^[0-9]*$");
                if(!rule.Match(cbExecDay.Text).Success)
                {
                    cbExecDay.Focus();
                    return "月的天數必須是數字！";
                }
            }
            else if(rbWeek.Checked)
            {

            }
            else if(rbDay.Checked)
            {

            }
            else if(rbHHss.Checked)
            {
                if (numMinuteHH.Value == 0)
                {
                    numMinuteHH.Focus();
                    return "反覆執行的（時：分）不可以都為 0！";
                }
            }
            return string.Empty;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            string ErrMessage = ValidateData();
            if(!string.IsNullOrEmpty(ErrMessage))
            {
                MessageBox.Show(ErrMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            UpdateJob2JobListData();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOpenDLL_Click(object sender, EventArgs e)
        {
            bool IsEditMode = !string.IsNullOrEmpty(_JobId); //如果不為空值，表示 [修改] 模式
            bool IsReset = false;

            if (IsEditMode)
                IsReset = MessageBox.Show("在修改的時候，可以不需要重新指定 DLL，重新指定會覆蓋掉原本的DLL！您確定要重新指定？", this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;
            else
                IsReset = true;

            if (!IsReset)
                return;

            bool Found = false;
            string ClassName = string.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Assembly ass = Assembly.LoadFrom(openFileDialog1.FileName);
                foreach (Type ClassT in ass.GetTypes())
                {
                    if (ClassT.BaseType.Name == "AbstractProcess")
                    {
                        Found = true;
                        ClassName = ClassT.Name;
                    }
                }
                if (!Found)
                {
                    MessageBox.Show("不是正確的 ScheduleJob DLL，找不到實作 AbstractProcess 的類別。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                txtDLL.Text = openFileDialog1.FileName;
                labDLLName.Text = ass.GetName().Name;
                labClassName.Text = ClassName;
            }
        }

        #region 切換 RadionButton
        private void rbMonth_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            switch(rb.Tag.ToString())
            {
                case "Month":
                    gbMonth.Enabled = true;
                    gbHH.Enabled = gbDay.Enabled = gbWeek.Enabled = false;
                    break;
                case "Week":
                    gbWeek.Enabled = true;
                    gbMonth.Enabled = gbDay.Enabled = gbHH.Enabled = false;
                    break;
                case "Day":
                    gbDay.Enabled = true;
                    gbMonth.Enabled = gbWeek.Enabled = gbHH.Enabled = false;
                    break;
                case "HH":
                    gbHH.Enabled = true;
                    gbMonth.Enabled = gbWeek.Enabled = gbDay.Enabled = false;
                    break;
            }
        }
        #endregion

        class ListItem
        {
            public string DisplayMember { get; set; }
            public string ValueMember { get; set; }
        }

        #region 初始化畫面相關資料
        /// <summary>
        /// 初始化畫面相關資料
        /// </summary>
        private void PrepareData()
        {
            cbExecDay.Items.Clear();
            cbExecDay.DisplayMember = "DisplayMember";
            cbExecDay.ValueMember = "ValueMember";
            List<ListItem> result = new List<ListItem>();
            for (int i = 1; i <= 31; i++)
                result.Add(new ListItem() { DisplayMember = i.ToString(), ValueMember = i.ToString()});
            cbExecDay.DataSource = result;

            cbExecWeek.Items.Clear();
            cbExecWeek.DisplayMember = "DisplayMember";
            cbExecWeek.ValueMember = "ValueMember";
            cbExecWeek.DataSource = new List<ListItem>(new ListItem[] {
                new ListItem() {DisplayMember ="星期一", ValueMember = "1"},
                new ListItem() {DisplayMember ="星期二", ValueMember = "2"},
                new ListItem() {DisplayMember ="星期三", ValueMember = "3"},
                new ListItem() {DisplayMember ="星期四", ValueMember = "4"},
                new ListItem() {DisplayMember ="星期五", ValueMember = "5"},
                new ListItem() {DisplayMember ="星期六", ValueMember = "6"},
                new ListItem() {DisplayMember ="星期日", ValueMember = "0"}
            });
        }
        #endregion
    }
}
