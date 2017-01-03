namespace MyORM.Framework.WinScheduleUI
{
    partial class frmScheduleJobUI
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScheduleJobUI));
            this.btnAddJob = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gvJobGridView = new System.Windows.Forms.DataGridView();
            this.ColJobId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColJobName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDLLName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNameSpace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColUsePeriodType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColIsPeriodJob = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColExecuteMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColExceuteWeek = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColExecuteDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLastExecuteDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnEditJob = new System.Windows.Forms.Button();
            this.btnDelJob = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStartNewJob = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gvJobGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddJob
            // 
            this.btnAddJob.Location = new System.Drawing.Point(263, 12);
            this.btnAddJob.Name = "btnAddJob";
            this.btnAddJob.Size = new System.Drawing.Size(110, 33);
            this.btnAddJob.TabIndex = 0;
            this.btnAddJob.Text = "新增工作(&A)";
            this.btnAddJob.UseVisualStyleBackColor = true;
            this.btnAddJob.Click += new System.EventHandler(this.btnAddJob_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(13, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "旺宏電子 ScheduleJob 編輯器";
            // 
            // gvJobGridView
            // 
            this.gvJobGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvJobGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColJobId,
            this.ColJobName,
            this.ColDLLName,
            this.ColNameSpace,
            this.ColClassName,
            this.ColUsePeriodType,
            this.ColIsPeriodJob,
            this.ColExecuteMonth,
            this.ColExceuteWeek,
            this.ColExecuteDay,
            this.ColStartTime,
            this.ColLastExecuteDateTime});
            this.gvJobGridView.Location = new System.Drawing.Point(16, 55);
            this.gvJobGridView.MultiSelect = false;
            this.gvJobGridView.Name = "gvJobGridView";
            this.gvJobGridView.ReadOnly = true;
            this.gvJobGridView.RowTemplate.Height = 24;
            this.gvJobGridView.Size = new System.Drawing.Size(1113, 452);
            this.gvJobGridView.TabIndex = 2;
            this.gvJobGridView.DoubleClick += new System.EventHandler(this.btnEditJob_Click);
            // 
            // ColJobId
            // 
            this.ColJobId.DataPropertyName = "JobId";
            this.ColJobId.HeaderText = "工作代碼";
            this.ColJobId.Name = "ColJobId";
            this.ColJobId.ReadOnly = true;
            this.ColJobId.Width = 120;
            // 
            // ColJobName
            // 
            this.ColJobName.DataPropertyName = "JobName";
            this.ColJobName.HeaderText = "工作名稱";
            this.ColJobName.Name = "ColJobName";
            this.ColJobName.ReadOnly = true;
            this.ColJobName.Width = 150;
            // 
            // ColDLLName
            // 
            this.ColDLLName.DataPropertyName = "DLLName";
            this.ColDLLName.HeaderText = "DLL 名稱";
            this.ColDLLName.Name = "ColDLLName";
            this.ColDLLName.ReadOnly = true;
            this.ColDLLName.Width = 130;
            // 
            // ColNameSpace
            // 
            this.ColNameSpace.DataPropertyName = "NameSpace";
            this.ColNameSpace.HeaderText = "命名空間";
            this.ColNameSpace.Name = "ColNameSpace";
            this.ColNameSpace.ReadOnly = true;
            this.ColNameSpace.Width = 150;
            // 
            // ColClassName
            // 
            this.ColClassName.DataPropertyName = "ClassName";
            this.ColClassName.HeaderText = "類別名稱";
            this.ColClassName.Name = "ColClassName";
            this.ColClassName.ReadOnly = true;
            // 
            // ColUsePeriodType
            // 
            this.ColUsePeriodType.DataPropertyName = "UsePeriodType";
            this.ColUsePeriodType.HeaderText = "排程工作類型";
            this.ColUsePeriodType.Name = "ColUsePeriodType";
            this.ColUsePeriodType.ReadOnly = true;
            this.ColUsePeriodType.Visible = false;
            this.ColUsePeriodType.Width = 150;
            // 
            // ColIsPeriodJob
            // 
            this.ColIsPeriodJob.DataPropertyName = "IsPeriodJob";
            this.ColIsPeriodJob.HeaderText = "是否為週期性工作";
            this.ColIsPeriodJob.Name = "ColIsPeriodJob";
            this.ColIsPeriodJob.ReadOnly = true;
            this.ColIsPeriodJob.Width = 130;
            // 
            // ColExecuteMonth
            // 
            this.ColExecuteMonth.DataPropertyName = "ExecuteMonth";
            this.ColExecuteMonth.HeaderText = "每執行月";
            this.ColExecuteMonth.Name = "ColExecuteMonth";
            this.ColExecuteMonth.ReadOnly = true;
            this.ColExecuteMonth.Visible = false;
            // 
            // ColExceuteWeek
            // 
            this.ColExceuteWeek.DataPropertyName = "ExecuteWeek";
            this.ColExceuteWeek.HeaderText = "每執行週";
            this.ColExceuteWeek.Name = "ColExceuteWeek";
            this.ColExceuteWeek.ReadOnly = true;
            this.ColExceuteWeek.Visible = false;
            // 
            // ColExecuteDay
            // 
            this.ColExecuteDay.DataPropertyName = "ExecuteDay";
            this.ColExecuteDay.HeaderText = "每執行天";
            this.ColExecuteDay.Name = "ColExecuteDay";
            this.ColExecuteDay.ReadOnly = true;
            this.ColExecuteDay.Visible = false;
            // 
            // ColStartTime
            // 
            this.ColStartTime.DataPropertyName = "StartTime";
            this.ColStartTime.HeaderText = "開始執行時間";
            this.ColStartTime.Name = "ColStartTime";
            this.ColStartTime.ReadOnly = true;
            this.ColStartTime.Width = 110;
            // 
            // ColLastExecuteDateTime
            // 
            this.ColLastExecuteDateTime.DataPropertyName = "LastExecuteDateTime";
            dataGridViewCellStyle1.Format = "yyyy/MM/dd HH:ss";
            dataGridViewCellStyle1.NullValue = null;
            this.ColLastExecuteDateTime.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColLastExecuteDateTime.HeaderText = "服務上一次最後執行時間";
            this.ColLastExecuteDateTime.Name = "ColLastExecuteDateTime";
            this.ColLastExecuteDateTime.ReadOnly = true;
            this.ColLastExecuteDateTime.Width = 180;
            // 
            // btnEditJob
            // 
            this.btnEditJob.Location = new System.Drawing.Point(379, 12);
            this.btnEditJob.Name = "btnEditJob";
            this.btnEditJob.Size = new System.Drawing.Size(110, 33);
            this.btnEditJob.TabIndex = 3;
            this.btnEditJob.Text = "編輯工作(&E)";
            this.btnEditJob.UseVisualStyleBackColor = true;
            this.btnEditJob.Click += new System.EventHandler(this.btnEditJob_Click);
            // 
            // btnDelJob
            // 
            this.btnDelJob.Location = new System.Drawing.Point(495, 12);
            this.btnDelJob.Name = "btnDelJob";
            this.btnDelJob.Size = new System.Drawing.Size(110, 33);
            this.btnDelJob.TabIndex = 4;
            this.btnDelJob.Text = "刪除工作(&D)";
            this.btnDelJob.UseVisualStyleBackColor = true;
            this.btnDelJob.Click += new System.EventHandler(this.btnDelJob_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1019, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 33);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "存檔(&S)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "ListJobDatas.xml";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(909, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "存檔才會生效 -->";
            // 
            // btnStartNewJob
            // 
            this.btnStartNewJob.ForeColor = System.Drawing.Color.Black;
            this.btnStartNewJob.Location = new System.Drawing.Point(611, 13);
            this.btnStartNewJob.Name = "btnStartNewJob";
            this.btnStartNewJob.Size = new System.Drawing.Size(110, 32);
            this.btnStartNewJob.TabIndex = 7;
            this.btnStartNewJob.Text = "手動執行工作(&H)";
            this.btnStartNewJob.UseVisualStyleBackColor = true;
            this.btnStartNewJob.Click += new System.EventHandler(this.btnStartNewJob_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(727, 13);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(110, 32);
            this.btnSetting.TabIndex = 8;
            this.btnSetting.Text = "排程執行狀態(&S)";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // frmScheduleJobUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1141, 519);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.btnStartNewJob);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDelJob);
            this.Controls.Add(this.btnEditJob);
            this.Controls.Add(this.gvJobGridView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddJob);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmScheduleJobUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScheduleJob Editor v1.0 (旺宏電子 ScheduleJob 編輯器)";
            this.Load += new System.EventHandler(this.frmScheduleJobUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvJobGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddJob;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView gvJobGridView;
        private System.Windows.Forms.Button btnEditJob;
        private System.Windows.Forms.Button btnDelJob;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStartNewJob;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColJobId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColJobName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDLLName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNameSpace;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColUsePeriodType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColIsPeriodJob;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColExecuteMonth;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColExceuteWeek;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColExecuteDay;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLastExecuteDateTime;
    }
}

