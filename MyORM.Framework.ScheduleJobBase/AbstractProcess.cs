using MyORM.Framework.ScheduleJob.Store;
using Dummies.ServerComponent;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJobBase
{
    /// <summary>
    /// 系統排程服務的 abstract class.
    /// 需繼承後使用，
    /// </summary>
    public abstract class AbstractProcess : IAction
    {
        //protected static Logger LOG = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 主要執行的 Method 入口
        /// <param name="job">目前執行的 Job 相關資料.</param>
        /// <param name="writeLogAttr">WriteLog 的設定相關資料</param>
        /// <param name="writeExAttr">WriteExLog 的設定相關資料</param>
        /// </summary>
        public virtual void DoJob(
            JobData job,
            LogAttributeData<WriteLogAttribute> writeLogAttr,
            LogAttributeData<WriteExceptionLogAttribute> writeExAttr,
            LogAttributeData<ErrorLevelAttribute> errorLevelAttr,
            Logger loggerInner,
            Logger loggerInnerErr)
        {
            #region NLog 相關物件
            //Logger loggerInner = NLogHelper.GetFileLogConfig(LogLevel.Info);
            //Logger loggerInnerErr = NLogHelper.GetErrorFileLogConfig(LogLevel.Error);
            #endregion

            WriteLogAttribute LogAttr = null;
            WriteExceptionLogAttribute LogExAttr = null;
            ErrorLevelAttribute ErrLevelAttr = null;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Reset();//碼表歸零
            sw.Start();//碼表開始計時

            //loggerInner.Info("[DoJob] Start");

            try
            {
                LogAttr = writeLogAttr.GetBeforePrepareAttr();          //取得當前的 BeforPrepare 方法是否有設定 WriteLogAttribute.
                if(LogAttr!=null)
                    WriteInfo(LogAttr, job, "[BeforePrepare] Start...");

                BeforePrepare();
                if (LogAttr != null)
                    loggerInner.Info(string.Format("[{0}].{1}.{2}  Completed... total {3}", job.JobName, "[DotJob]", "[BeforePrepare]", sw.Elapsed.TotalMilliseconds.ToString()));
            }
            catch(Exception ex)
            {
                ErrLevelAttr = errorLevelAttr.GetBeforePrepareAttr();   //取得當前的 BeforPrepare 方法是否有設定 ErrorLevelAttribute.
                LogExAttr = writeExAttr.GetBeforePrepareAttr();         //取得當前的 BeforPrepare 方法是否有設定 WriteExceptionLogAttribute.
                if (LogExAttr != null)
                    WriteExLog(LogExAttr, job, "[BeforePrepare]", ex, ErrLevelAttr);

                throw ex;
            }

            try
            {
                LogAttr = writeLogAttr.GetPrepareDataMethodAttr();
                if (LogAttr != null)
                    WriteInfo(LogAttr, job, "[PrepareData] Start...");

                PrepareData();
                if (LogAttr != null)
                    loggerInner.Info(string.Format("[{0}].{1}.{2}  Completed... total {3}", job.JobName, "[DotJob]", "[PrepareData]", sw.Elapsed.TotalMilliseconds.ToString()));
            }
            catch(Exception ex)
            {
                ErrLevelAttr = errorLevelAttr.GetPrepareDataMethodAttr();//取得當前的 PrepareData 方法是否有設定 ErrorLevelAttribute.
                LogExAttr = writeExAttr.GetPrepareDataMethodAttr();     //取得當前的 PrepareData 方法是否有設定 WriteExceptionLogAttribute.
                if (LogExAttr != null)
                    WriteExLog(LogExAttr, job, "[PrepareData]", ex, ErrLevelAttr);

                throw ex;
            }

            try
            {
                LogAttr = writeLogAttr.GetProcessDataMethodAttr();
                if (LogAttr != null)
                    WriteInfo(LogAttr, job, "[ProcessData] Start...");

                ProcessData();
                if(LogAttr != null)
                    loggerInner.Info(string.Format("[{0}].{1}.{2}  Completed... total {3}", job.JobName, "[DotJob]", "[ProcessData]", sw.Elapsed.TotalMilliseconds.ToString()));
            }
            catch(Exception ex)
            {
                ErrLevelAttr = errorLevelAttr.GetProcessDataMethodAttr();//取得當前的 ProcessData 方法是否有設定 ErrorLevelAttribute.
                LogExAttr = writeExAttr.GetProcessDataMethodAttr();     //取得當前的 ProcessData 方法是否有設定 WriteExceptionLogAttribute.
                if (LogExAttr != null)
                    WriteExLog(LogExAttr, job, "[ProcessData]", ex, ErrLevelAttr);

                throw ex;
            }

            try
            {
                LogAttr = writeLogAttr.GetAfterProcessMethodAttr();
                if (LogAttr != null)
                    WriteInfo(LogAttr, job, "[AfterProcess] Start...");

                AfterProcess();
                if(LogAttr != null)
                    loggerInner.Info(string.Format("[{0}].{1}.{2}  End... total {3}", job.JobName, "[DotJob]", "[AfterProcess]", sw.Elapsed.TotalMilliseconds.ToString()));
            }
            catch(Exception ex)
            {
                ErrLevelAttr = errorLevelAttr.GetAfterProcessMethodAttr();//取得當前的 AfterProcess 方法是否有設定 ErrorLevelAttribute.
                LogExAttr = writeExAttr.GetAfterProcessMethodAttr();     //取得當前的 AfterProcess 方法是否有設定 WriteExceptionLogAttribute.
                if (LogExAttr != null)
                    WriteExLog(LogExAttr, job, "[AfterProcess]", ex, ErrLevelAttr);

                throw ex;
            }

            sw.Stop();//碼錶停止
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="job"></param>
        /// <param name="MethodName"></param>
        private void WriteInfo(WriteLogAttribute attr, JobData job, string MethodName)
        {
            Logger loggerInner = null;
            int entry = (int)attr.WriteLogType;
            switch (entry)
            {
                case 1:
                    //動態取得 FileLog 的 NLog Configuration.
                    loggerInner = NLogHelper.GetFileLogConfig(LogLevel.Info);
                    LoggerWriter(job, loggerInner, MethodName);
                    break;
                case 2:
                    //動態取得 EventLog 的 NLog Configuration.
                    loggerInner = NLogHelper.GetEventLogConfig(LogLevel.Info);
                    LoggerWriter(job, loggerInner, MethodName);
                    break;
                case 4:
                    //動態取得 EMailLog 的 NLog Configuration.
                    loggerInner = NLogHelper.GetEMailLogConfig(LogLevel.Info);
                    LoggerWriter(job, loggerInner, MethodName);
                    break;
                case 3: //同時寫 FileLog & 寫 EventLog.
                    ////動態取得 EventLog 的 NLog Configuration.
                    loggerInner = NLogHelper.GetEventLogConfig(LogLevel.Info);
                    LoggerWriter(job, loggerInner, MethodName);
                    break;
                case 6: //同時寫 EventLog & Send EMail.
                    ////動態取得 EMailLog 的 NLog Configuration.
                    loggerInner = NLogHelper.GetEMailLogConfig(LogLevel.Info);
                    LoggerWriter(job, loggerInner, MethodName);
                    break;
                case 7: //同時寫 FileLog & EventLog & Send EMail.
                    ////動態取得 EMailLog 的 NLog Configuration.
                    loggerInner = NLogHelper.GetEMailLogConfig(LogLevel.Info);
                    LoggerWriter(job, loggerInner, MethodName);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mExLogAttr"></param>
        /// <param name="job"></param>
        /// <param name="MethodName"></param>
        /// <param name="ex"></param>
        /// <param name="errorLevel"></param>
        private void WriteExLog(
            WriteExceptionLogAttribute mExLogAttr,
            JobData job,
            string MethodName,
            Exception ex,
            ErrorLevelAttribute errorLevel)
        {
            Logger loggerInnerEx = null;
            int entry = (int)mExLogAttr.WriteLogType;
            switch (entry)
            {
                case 1:
                    //動態取得 FileLog 的 NLog Configuration.
                    loggerInnerEx = NLogHelper.GetErrorFileLogConfig(LogLevel.Error);
                    LoggerExWriter(job, loggerInnerEx, MethodName, ex, errorLevel);
                    break;
                case 2:
                    //動態取得 EventLog 的 NLog Configuration.
                    loggerInnerEx = NLogHelper.GetEventLogConfig(LogLevel.Error);
                    LoggerExWriter(job, loggerInnerEx, MethodName, ex, errorLevel);
                    break;
                case 4:
                    //動態取得 EMailLog 的 NLog Configuration.
                    loggerInnerEx = NLogHelper.GetEMailLogConfig(LogLevel.Error);
                    LoggerExWriter(job, loggerInnerEx, MethodName, ex, errorLevel);
                    break;
                case 3: //同時寫 FileLog & 寫 EventLog.
                    //動態取得 FileLog 的 NLog Configuration.
                    //logger = NLogHelper.GetErrorFileLogConfig(LogLevel.Error);
                    ////寫入錯誤的 FileLog
                    //LoggerExWriter(loadClass, MethodName, ex, errorLevel, LoginName, ParamName);
                    ////動態取得 EventLog 的 NLog Configuration.
                    loggerInnerEx = NLogHelper.GetEventLogConfig(LogLevel.Error);
                    LoggerExWriter(job, loggerInnerEx, MethodName, ex, errorLevel);
                    break;
                case 6: //同時寫 EventLog & Send EMail.
                    //動態取得 EventLog 的 NLog Configuration.
                    //logger = NLogHelper.GetEventLogConfig(LogLevel.Error);
                    //LoggerExWriter(loadClass, MethodName, ex, errorLevel, LoginName, ParamName);
                    ////動態取得 EMailLog 的 NLog Configuration.
                    loggerInnerEx = NLogHelper.GetEMailLogConfig(LogLevel.Error);
                    LoggerExWriter(job, loggerInnerEx, MethodName, ex, errorLevel);
                    break;
                case 7: //同時寫 FileLog & EventLog & Send EMail.

                    break;
            }
        }
        private void LoggerWriter(JobData job, Logger logger, string MethodName)
        {
            //將 Log 寫入到 FileSystem 中或 EventLog 事件檢視器中.
            logger.Info(string.Format("[{0}].{1}.{2} ", job.JobName, "[DotJob]", MethodName));
        }

        private void LoggerExWriter(
            JobData job, 
            Logger logger,
            string MethodName,
            Exception ex,
            ErrorLevelAttribute errorLevel)
        {
            //將 Error Log 寫入到 FileSystem 中或 EventLog 事件檢視器中 或 透過 EMail 的方式寄出.
            if (errorLevel.ErrorLevelType == LevelType.Error)
            {
                logger.Error(string.Format("[{0}].{1}.{2} Execute fail... Detailed error：{3}", job.JobName, "[DotJob]", MethodName, ex));

            }
            else if (errorLevel.ErrorLevelType == LevelType.Fatal)
            {
                logger.Fatal(string.Format("[{0}].{1}.{2} Execute fail... Detailed error：{3}", job.JobName, "[DotJob]", MethodName, ex));
            }
        }
        /// <summary>
        /// 來源資料準備與取得
        /// Ex: 檢查 MQ 是否有資料或是否有待處理的檔案
        /// </summary>
        protected virtual void BeforePrepare()
        {
        }
        /// <summary>
        /// 待處理資料的準備
        /// Ex: 若需參照其它資料時進行資料的準備
        /// </summary>
        protected virtual void PrepareData()
        {
        }
        /// <summary>
        /// 資料處理與邏輯計算
        /// Ex: 資料檢查與運算後進行資料庫的更新或 Report 的產生
        /// </summary>
        protected abstract void ProcessData();
        /// <summary>
        /// 進行 Proceess 的結束處理
        /// Ex: 資源 Release 或 LOG 的記錄
        /// </summary>
        protected virtual void AfterProcess()
        {
        }
    }
}
