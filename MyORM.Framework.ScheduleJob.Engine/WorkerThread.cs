using MyORM.Framework.ScheduleJob.Core;
using MyORM.Framework.ScheduleJob.Store;
using Dummies.ServerComponent;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Engine
{
    /// <summary>
    /// Schedule Job 排程工作的執行序.
    /// Create by Gelis at 2014/11/01.
    /// </summary>
    public class WorkerThread
    {
        /// <summary>
        /// 用以指出目前的 Thread 是否已經收到停止的動作 (事件).
        /// </summary>
        ManualResetEvent _EventStopThread;

        private MethodInfo _jobMethod = null;
        private object _targetJobInstance = null;

        public WorkerThread()
        {
            //初始化執行序信號狀態物件.
            _EventStopThread = new ManualResetEvent(false);
        }

        /// <summary>
        /// 使用 Thread 產生一個新的 Schedule Job 執行個體.
        /// <param name="job">Schedule Job 排程相關資料</param>
        /// </summary>
        public void StartNewScheduleJob(JobData job)
        {
            #region NLog 相關物件
            Logger logger = NLogHelper.GetFileLogConfig(LogLevel.Info);
            Logger loggerErr = NLogHelper.GetErrorFileLogConfig(LogLevel.Error);
            #endregion

            LogAttributeData<WriteLogAttribute> writeLogAttr = null;            //建立 傳遞 WriteLogAttribute 的容器
            LogAttributeData<WriteExceptionLogAttribute> writeExAttr = null;    //建立 傳遞 WriteExceptionLogAttribute 的容器
            LogAttributeData<ErrorLevelAttribute> errorLevelAttr = null;        //建立 傳遞 ErrorLevelAttribute 的容器

            try
            {
                var resultValue = LoadAssemblyHelper.GetExecuteJobMethod(job);
                _jobMethod = resultValue.Item1;         // Job DLL Instance of Method.
                _targetJobInstance = resultValue.Item2; // Job DLL Instance.
                writeLogAttr = resultValue.Item3;
                writeExAttr = resultValue.Item4;
                errorLevelAttr = resultValue.Item5;

            }
            catch (JobDLLNotFoundException dex) { 
                NLogHelper.LoggerExWriter(loggerErr, job.JobName, job.DLLName, string.Format("[{0}]", job.JobName), dex.Message);
            }
            catch (JobMethodNotFoundException mex) {
                NLogHelper.LoggerExWriter(loggerErr, job.JobName, job.DLLName, string.Format("[{0}]", job.JobName), mex.Message);
            }
            catch (IActionNotImplementException aex) {
                NLogHelper.LoggerExWriter(loggerErr, job.JobName, job.DLLName, string.Format("[{0}]", job.JobName), aex.Message);
            }
            catch (Exception ex) {
                NLogHelper.LoggerExWriter(loggerErr, job.JobName, job.DLLName, string.Format("[{0}]", job.JobName), ex.Message);
            }

            try
            {
                //設定此工作目前執中.
                RunningTable.SetJobStatus(job.JobId, JobStatus.Running);
                //重設 Thread 事件狀態.
                _EventStopThread.Reset();
                //m_EventThreadStopped.Reset();

                //產生一個新的 WorkThreadFunction 的 Thread 執行個體.
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    //紀錄 LOG 工作開始
                    logger = NLogHelper.GetFileLogConfig(LogLevel.Info);
                    NLogHelper.LoggerWriter(logger, job.JobName, job.DLLName, string.Format("[{0}]", job.JobName), "[JobStart]", "[JobStart]");
                    //工作開始
                    Thread_Exec_Reault result = WorkerThreadFunction(_jobMethod, _targetJobInstance, job, false, writeLogAttr, writeExAttr, errorLevelAttr, logger, loggerErr);
                    //如果前一個 WorkerThreadFunction 的執行是失敗的，就進行 ReTry 機制
                    if (result == Thread_Exec_Reault.FAIL)
                    {
                        int ReTrySec = 5000; //若未設定 config，則預設每 5 秒鐘進行 ReTry 機制
                        if(ConfigurationManager.AppSettings["ReTrySec"]!=null)
                        {
                            try
                            {
                                ReTrySec = int.Parse(ConfigurationManager.AppSettings["ReTrySec"]) * 1000;
                            }
                            catch 
                            { 
                                loggerErr = NLogHelper.GetErrorFileLogConfig(LogLevel.Error); 
                                NLogHelper.LoggerExWriter(loggerErr, job.JobName, job.DLLName, "[ScheduleJob]", "app.config in the ReTrySec parameter setting is incorrect！"); 
                            }
                        }

                        if (job.ReTry > 0)
                        {
                            //Thread t = new Thread((state) => { WorkerThreadFunctionReTry(iReTry, _jobMethod, _targetJobInstance, job); });
                            //t.Start();
                            // 進行 ReTry 機制.
                            // *** 變數說明 ***
                            // iReTry：要進行 ReTry 的次數
                            // ExecCount：ReRey 的次數.
                            // ReTrySec：ReRey 的秒數.
                            int iReTry = job.ReTry;
                            Task.Factory.StartNew(() =>
                            {
                                int ExecCount = 1;
                                do
                                {
                                    Logger loggerRecord = NLogHelper.GetFileLogConfig(LogLevel.Info);
                                    NLogHelper.LoggerWriter(loggerRecord, job.JobName, job.DLLName, string.Format("[{0}]", job.JobName), "[ReTry]", string.Format("Execute repeat steps {0}.....", ExecCount));
                                    Thread_Exec_Reault ReTryResult = WorkerThreadFunction(_jobMethod, _targetJobInstance, job, true, writeLogAttr, writeExAttr, errorLevelAttr, logger, loggerErr);
                                    if (ReTryResult == Thread_Exec_Reault.SUCCESS)
                                        break; //若執行成功，隨即跳出迴圈 與 Currenthread.

                                    Thread.Sleep(ReTrySec);
                                    iReTry--;
                                    ExecCount++;

                                } while (iReTry > 0);
                            }
                            );
                        }
                    }
                }, job);
                //設定最後執行時間.
                RunningJobs.SetLastExecuteDateTime(job);
            }
            catch(Exception ex)
            {                
                //logger = NLogHelper.GetErrorFileLogConfig(LogLevel.Error);
                NLogHelper.LoggerExWriter(loggerErr, job.JobName, job.DLLName, string.Format("[{0}]", job.JobName), ex.Message);
            }
            finally
            {
                //等待執行序收到結束信號
                _EventStopThread.WaitOne();
                //設定服務為閒置狀態
                RunningTable.SetJobStatus(job.JobId, JobStatus.Idle);
            }
        }
        /// <summary>
        /// 實際的 Schedule Job 執行序工作的方法 WorkThreadFunction.
        /// <param name="JobMethod">Job MethodInfo</param>
        /// <param name="TargetJobIns">要呼叫的主 Job 物件的執行個體</param>
        /// <param name="job">JobData</param>
        /// <param name="IsReTry">目前是否為 ReTry 的呼叫</param>
        /// <param name="writeLogAttr"></param>
        /// <param name="writeExAttr"></param>
        /// <param name="errorLevelAttr"></param>
        /// </summary>
        private Thread_Exec_Reault WorkerThreadFunction(
            MethodInfo JobMethod, 
            object TargetJobIns, 
            JobData job, 
            bool IsReTry,
            LogAttributeData<WriteLogAttribute> writeLogAttr,
            LogAttributeData<WriteExceptionLogAttribute> writeExAttr,
            LogAttributeData<ErrorLevelAttribute> errorLevelAttr,
            Logger loggerInner,
            Logger loggerInnerErr)
        {
            Thread_Exec_Reault result = Thread_Exec_Reault.SUCCESS;

            #region NLog 相關物件
            //Logger loggerInner = NLogHelper.GetFileLogConfig(LogLevel.Info);
            //Logger loggerInnerErr = NLogHelper.GetErrorFileLogConfig(LogLevel.Error);
            #endregion

            try
            {
                ////紀錄 LOG 工作開始
                //NLogHelper.LoggerWriter(loggerInner, job.JobName, job.DLLName, "[ScheduleJob]", "[工作開始]", "");
                //執行工作
                JobMethod.Invoke(TargetJobIns, new object[] {job, writeLogAttr, writeExAttr, errorLevelAttr, loggerInner, loggerInnerErr});
                //執行成功後，紀錄 [執行紀錄] 表示執行過此 Job，若發生錯則只在下方的 catch 中記錄 錯誤資訊.
                NLogHelper.LoggerWriter(loggerInner, job.JobName, job.DLLName, string.Format("[{0}]", job.JobName));
            }
            catch(Exception ex)
            {
                result = Thread_Exec_Reault.FAIL;
                //如果是被呼叫的 DLL內發生錯誤，就將錯誤 LOG 起來
                //throw ex;
                //當然，先決條件就是，DLL 必須存在，否則 TargetJobIns 會是 null.
                if(JobMethod!=null && TargetJobIns!=null)
                {
                    string ErrMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    if(IsReTry)
                    {
                        NLogHelper.LoggerExWriter(loggerInnerErr, job.JobName, job.DLLName, string.Format("[{0}]", job.JobName), string.Format(" [ReTry] {0}", ErrMessage));
                    }
                    else
                    {
                        NLogHelper.LoggerExWriter(loggerInnerErr, job.JobName, job.DLLName, string.Format("[{0}]", job.JobName), ErrMessage);
                    }
                }
            }
            finally
            {
                //不管執行序內是正常執行結束、或是發生錯誤後才結束，都將執行序設定為執行序結束
                _EventStopThread.Set();
            }
            return result;
        }
    }
    /// <summary>
    /// WorkerThread 的執行結果
    /// </summary>
    public enum Thread_Exec_Reault
    {
        /// <summary>
        /// 執行成功
        /// </summary>
        SUCCESS,
        /// <summary>
        /// 執行失敗
        /// </summary>
        FAIL
    }
}
