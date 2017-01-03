using MyORM.Framework.ScheduleJobBase;
using Dummies.ServerComponent;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyORM.Framework.ScheduleJob.WriteLogTest
{
    /// <summary>
    /// (測試使用) 提供 NLog 寫入.
    /// </summary>
    public class NWriteLog: AbstractProcess
    {
        private Logger logger = null;

        #region 實作 AbstractProcess 的方法
        /// <summary>
        /// 
        /// </summary>
        [WriteLog(UseLogType.ToFileSystem)]
        protected override void BeforePrepare()
        {
            //"Work BeforePrepare()".Dump();
        }
        /// <summary>
        /// 準備 JOB 相關資料
        /// </summary>
        [WriteLog(UseLogType.ToFileSystem)]
        protected override void PrepareData()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        public static Form myForm = null;
        /// <summary>
        /// 處理資料(只針對完整的檔案進行處理)
        /// 處理成功後會對來源檔案進行 Rename or Delete
        /// </summary>
        [WriteLog(UseLogType.ToFileSystem)]
        [WriteExceptionLog(UseLogType.ToFileSystem)]
        [ErrorLevel(LevelType.Fatal)]
        protected override void ProcessData()
        {
            //執行紀錄 Log 的方法 (使用 NLog).
            //logger.Info(string.Format("執行了一次的 {0} 物件的 {1} 方法.", this.GetType().Name, "Execute()"));
            //Thread.Sleep(1500);
            //throw new Exception("這個 Schedule Job 發生了錯誤！..........");
            myForm = new Form();
            myForm.StartPosition = FormStartPosition.CenterScreen;
            myForm.Show();
        }
        protected override void AfterProcess()
        {
            //"Work AfterProcess()".Dump();
        }
        #endregion
    }
}
