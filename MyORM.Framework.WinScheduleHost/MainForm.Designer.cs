namespace MyORM.Framework.WinScheduleHost
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.btnNewJobThreadPool = new System.Windows.Forms.Button();
            this.btnStartService = new System.Windows.Forms.Button();
            this.btnStopService = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labCurrent = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labAvarible = new System.Windows.Forms.Label();
            this.btnQueryJob = new System.Windows.Forms.Button();
            this.timerJobUpdate = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnNewJobThreadPool
            // 
            this.btnNewJobThreadPool.Location = new System.Drawing.Point(163, 28);
            this.btnNewJobThreadPool.Name = "btnNewJobThreadPool";
            this.btnNewJobThreadPool.Size = new System.Drawing.Size(134, 65);
            this.btnNewJobThreadPool.TabIndex = 2;
            this.btnNewJobThreadPool.Text = "NewJobThreadPool";
            this.btnNewJobThreadPool.UseVisualStyleBackColor = true;
            this.btnNewJobThreadPool.Click += new System.EventHandler(this.btnNewJobThreadPool_Click);
            // 
            // btnStartService
            // 
            this.btnStartService.Location = new System.Drawing.Point(23, 28);
            this.btnStartService.Name = "btnStartService";
            this.btnStartService.Size = new System.Drawing.Size(134, 65);
            this.btnStartService.TabIndex = 3;
            this.btnStartService.Text = "Start Service";
            this.btnStartService.UseVisualStyleBackColor = true;
            this.btnStartService.Click += new System.EventHandler(this.btnStartService_Click);
            // 
            // btnStopService
            // 
            this.btnStopService.Enabled = false;
            this.btnStopService.Location = new System.Drawing.Point(23, 113);
            this.btnStopService.Name = "btnStopService";
            this.btnStopService.Size = new System.Drawing.Size(134, 66);
            this.btnStopService.TabIndex = 4;
            this.btnStopService.Text = "Stop Service";
            this.btnStopService.UseVisualStyleBackColor = true;
            this.btnStopService.Click += new System.EventHandler(this.btnStopService_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(314, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current Job：";
            // 
            // labCurrent
            // 
            this.labCurrent.AutoSize = true;
            this.labCurrent.Location = new System.Drawing.Point(440, 41);
            this.labCurrent.Name = "labCurrent";
            this.labCurrent.Size = new System.Drawing.Size(11, 12);
            this.labCurrent.TabIndex = 6;
            this.labCurrent.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(316, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "All Avarible Job：";
            // 
            // labAvarible
            // 
            this.labAvarible.AutoSize = true;
            this.labAvarible.Location = new System.Drawing.Point(440, 67);
            this.labAvarible.Name = "labAvarible";
            this.labAvarible.Size = new System.Drawing.Size(11, 12);
            this.labAvarible.TabIndex = 8;
            this.labAvarible.Text = "0";
            // 
            // btnQueryJob
            // 
            this.btnQueryJob.Location = new System.Drawing.Point(316, 129);
            this.btnQueryJob.Name = "btnQueryJob";
            this.btnQueryJob.Size = new System.Drawing.Size(93, 36);
            this.btnQueryJob.TabIndex = 9;
            this.btnQueryJob.Text = "Query Job";
            this.btnQueryJob.UseVisualStyleBackColor = true;
            this.btnQueryJob.Click += new System.EventHandler(this.btnQueryJob_Click);
            // 
            // timerJobUpdate
            // 
            this.timerJobUpdate.Interval = 1000;
            this.timerJobUpdate.Tick += new System.EventHandler(this.timerJobUpdate_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 290);
            this.Controls.Add(this.btnQueryJob);
            this.Controls.Add(this.labAvarible);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labCurrent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStopService);
            this.Controls.Add(this.btnStartService);
            this.Controls.Add(this.btnNewJobThreadPool);
            this.Name = "MainForm";
            this.Text = "Schedule Job Services 管理介面";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNewJobThreadPool;
        private System.Windows.Forms.Button btnStartService;
        private System.Windows.Forms.Button btnStopService;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labCurrent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labAvarible;
        private System.Windows.Forms.Button btnQueryJob;
        private System.Windows.Forms.Timer timerJobUpdate;
    }
}

