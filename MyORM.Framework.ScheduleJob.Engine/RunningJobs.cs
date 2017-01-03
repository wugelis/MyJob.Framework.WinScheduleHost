using MyORM.Framework.ScheduleJob.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SP = System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MyORM.Framework.ScheduleJob.Engine
{
    /// <summary>
    /// 用以處理、每次重新計算出當前未執行的工作.
    /// Runtime 的 Job Table 由 FileWatcher 的 Changed 事件所監控，當 ListJobDatas.xml 檔案被異動時，動態 Load 到 Static 變數中.
    /// </summary>
    public class RunningJobs
    {
        /// <summary>
        /// Constructor for RunningJobs.
        /// </summary>
        static RunningJobs()
        {
            if (_FileSystemWatcher == null)
            {
                _FileSystemWatcher
                    = new FileSystemWatcher(Path.GetDirectoryName(JobHelper.JobXmlFile), Path.GetFileName(JobHelper.JobXmlFile))
                    {
                        NotifyFilter = NotifyFilters.LastWrite
                    };
                _FileSystemWatcher.Changed += new FileSystemEventHandler(_FileSystemWatcher_Changed);
            }

            _iFileWatcherCount = 0;

            _JobDatas = GetAllJobsFromFile();

            if (_ChangedTimer == null)
            {
                _ChangedTimer = new Timer(1500);
                _ChangedTimer.Elapsed += _ChangedTimer_Elapsed;
                _ChangedTimer.Enabled = true;
            }
        }
        /// <summary>
        /// static 變數，會在程式第一次啟動時執行，提供存放現有有效的 Running Job 工作.
        /// </summary>
        private static IEnumerable<JobData> _JobDatas;
        /// <summary>
        /// 從檔案系統中所有有效的 Schedule Job (預設濾掉 job.Enabled=false 以及 job.StartTime 未設定者)
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<JobData> GetAllJobsFromFile()
        {
            RunningJobs._FileSystemWatcher.EnableRaisingEvents = true;

            var result = from job in JobHelper.LoadXmlFromFileSystem().JobDatas.AsEnumerable()
                         select job;
            return result;
        }
        /// <summary>
        /// 監控 ListJobDatas.xml 是否被修改
        /// </summary>
        private static FileSystemWatcher _FileSystemWatcher;
        /// <summary>
        /// 公開私有的 FileSystemWatcher 物件工外部設定是否啟用 FileWatcher.
        /// </summary>
        public static bool EnabledFileWatcher
        {
            get {
                return RunningJobs._FileSystemWatcher.EnableRaisingEvents;
            }
            set {
                RunningJobs._FileSystemWatcher.EnableRaisingEvents = value;
            }
        }
        /// <summary>
        /// 如果 FileWatcher 的 Changed 事件是否在短時間被觸發兩次就將 _iFileWatcherCount 清除為 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void _ChangedTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _iFileWatcherCount = 0;
        }
        private static int _iFileWatcherCount;
        /// <summary>
        /// 提供判斷 FileWatcher 的 Changed 事件是否在短時間被觸發兩次.
        /// </summary>
        private static Timer _ChangedTimer;
        private static void _FileSystemWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            _iFileWatcherCount++;
            if (_iFileWatcherCount > 1)
                return;

            _JobDatas = GetAllJobsFromFile();
        } 
        
        /// <summary>
        /// 從記憶體中取回所有有效的 Schedule Job，以 IEnumerable 型態傳回.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<JobData> GetAllJobs()
        {
            return _JobDatas;
        }

        #region 取得當前的工作，每次只回傳一個工作
        /// <summary>
        /// 取得當前的工作，每次只回傳一個工作
        /// </summary>
        /// <returns>JobData</returns>
        public static JobData GetCurrentRunningJob()
        {
            //先找 (短期性的週期性工作) 再找 "天" 以上的週期性工作.
            //先找出非週期性工作
            JobData result = GetTimelyJob().FirstOrDefault();
            if (result != null)
                return result;

            //先找出 Daily 非週期性工作
            result = GetDailyJobs(PeriodJob.NONE).FirstOrDefault();
            if (result != null)
                return result;

            //再找出 Daily 週期性工作
            result = GetDailyJobs(PeriodJob.IS_PERIOD).FirstOrDefault();
            if (result != null)
                return result;

            //先找出 Weekly 非週期性工作
            result = GetWeeklyJobs(PeriodJob.NONE).FirstOrDefault();
            if (result != null)
                return result;

            //再找出 Weekly 週期性工作
            result = GetWeeklyJobs(PeriodJob.IS_PERIOD).FirstOrDefault();
            if (result != null)
                return result;

            //先找出 Monthly 非週期性工作
            result = GetMonthlyJobs(PeriodJob.NONE).FirstOrDefault();
            if (result != null)
                return result;

            //再找出 Monthly 週期性工作
            result = GetMonthlyJobs(PeriodJob.IS_PERIOD).FirstOrDefault();

            return result;
        }
        #endregion

        #region 取得短期性的週期性工作
        /// <summary>
        /// 取得短期性的週期性工作
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<JobData> GetTimelyJob()
        {
            var result = from job in GetAllJobs()
                         where job.UsePeriodType == PeriodType.BY_DAILY
                            && job.Enabled
                         select job;

            var result2 = result.Select(c => c);
            result2 = result2.Where(c => c.IsPeriodJob == PeriodJob.IS_PERIOD
                && (c.LastExecuteDateTime.HasValue ? ((DateTime.Now - c.LastExecuteDateTime.Value).Minutes >= int.Parse(c.ExecuteTime.Split(':')[1])) : true)
                && c.ExecuteTime != "00:00"
                );

            return result2;
        }
        #endregion

        #region 取得 Daily Job 工作
        /// <summary>
        /// 取得 Daily Job 工作
        /// </summary>
        /// <param name="PeriodJob">是否為週期性工作</param>
        /// <returns></returns>
        public static IEnumerable<JobData> GetDailyJobs(PeriodJob UsePeriod)
        {
            var result = from job in GetAllJobs()
                         where //(DateTime.Now - job.LastExecuteDateTime.Value).TotalDays>=1
                             job.UsePeriodType == PeriodType.BY_DAILY
                             && job.Enabled
                         select job;

            var result2 = result.Select(c => c);
            if (UsePeriod == PeriodJob.IS_PERIOD)
            {
                result2 = result2.Where(c => c.IsPeriodJob == UsePeriod
                    && (c.LastExecuteDateTime.HasValue ? ((DateTime.Now - c.LastExecuteDateTime.Value).TotalDays >= 1) : true)
                    && c.ExecuteDay == DateTime.Now.Day
                    && c.StartTime == DateTime.Now.ToString("HH:mm"));
            }
            else if (UsePeriod == PeriodJob.NONE)
            {
                result2 = result2.Where(c => !c.LastExecuteDateTime.HasValue
                    && c.ExecuteDay == DateTime.Now.Day
                    && c.StartTime == DateTime.Now.ToString("HH:mm"));
            }
            return result2;
        }
        #endregion

        #region 取得 Weekly Job 工作
        /// <summary>
        /// 取得 Weekly Job 工作
        /// </summary>
        /// <param name="UsePeriod">是否為週期性工作</param>
        /// <returns></returns>
        public static IEnumerable<JobData> GetWeeklyJobs(PeriodJob UsePeriod)
        {
            var result = from job in GetAllJobs()
                         where //Math.Round(((DateTime.Now - job.LastExecuteDateTime.Value).TotalDays % 7)) >= 7
                             job.UsePeriodType == PeriodType.BY_WEEKLY
                             && job.Enabled
                         select job;

            var result2 = result.Select(c => c);
            if (UsePeriod == PeriodJob.IS_PERIOD)
            {
                result2 = result2.Where(c => c.IsPeriodJob == UsePeriod
				    && (c.LastExecuteDateTime.HasValue?(((DateTime.Now - c.LastExecuteDateTime.Value).TotalDays) >= 7):true)
                    && c.ExceuteWeek == (DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Now.DayOfWeek)
                    && c.StartTime == DateTime.Now.ToString("HH:mm"));
            }
            else if (UsePeriod == PeriodJob.NONE)
            {
                result2 = result2.Where(c => !c.LastExecuteDateTime.HasValue
                    && c.ExceuteWeek == (DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Now.DayOfWeek)
                    && c.StartTime == DateTime.Now.ToString("HH:mm"));
            }
            return result2;
        }
        #endregion

        #region 取得 Monthly Job 工作
        /// <summary>
        /// 取得 Monthly Job 工作
        /// </summary>
        /// <param name="UsePeriod"></param>
        /// <returns></returns>
        public static IEnumerable<JobData> GetMonthlyJobs(PeriodJob UsePeriod)
        {
            var result = from job in GetAllJobs()
                         where job.UsePeriodType == PeriodType.BY_MONTHLY
                            && job.Enabled
                         //&& job.IsPeriodJob == UsePeriod
                         select job;

            var result2 = result.Select(c => c);
            if (UsePeriod == PeriodJob.IS_PERIOD)
            {
                result2 = result2.Where(c => c.IsPeriodJob == UsePeriod
                    && (c.LastExecuteDateTime.HasValue ? ((DateTime.Now - c.LastExecuteDateTime.Value).TotalDays >= (c.LastExecuteDateTime.Value.AddMonths(1) - c.LastExecuteDateTime.Value).TotalDays) : true)
                    && c.ExecuteDay == DateTime.Now.Day
                    && c.StartTime == DateTime.Now.ToString("HH:mm"));
            }
            else if (UsePeriod == PeriodJob.NONE)
            {
                result2 = result2.Where(c => !c.LastExecuteDateTime.HasValue
                && c.ExecuteDay == DateTime.Now.Day
                && c.StartTime == DateTime.Now.ToString("HH:mm"));
            }
            return result2;
        }
        #endregion

        #region 設定最後存取的時間
        /// <summary>
        /// 設定最後存取的時間.
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public static bool SetLastExecuteDateTime(JobData job)
        {
            bool state =  false;
            var result = _JobDatas.Where(c => c.JobId == job.JobId).FirstOrDefault();
            if(result!=null)
            {
                RunningJobs._FileSystemWatcher.EnableRaisingEvents = state;
                result.LastExecuteDateTime = DateTime.Now;
                SP.Thread.Sleep(500);
                JobHelper.SaveXml2FileSystem(new ListJobDatas() { JobDatas = _JobDatas.ToList() });
                SP.Thread.Sleep(500);
                RunningJobs._FileSystemWatcher.EnableRaisingEvents = state = true;
            }
            return state;
        }
        #endregion

        #region 取得是否有上一次未執行的週期性排程
        /// <summary>
        /// 取得是否有上一次未執行的週期性排程
        /// </summary>
        /// <returns>JobData</returns>
        public static JobData GetCurrentRunningJobByNonExec()
        {
            //先找 (短期性的週期性工作) 再找 "天" 以上的週期性工作.            
            //先找出非週期性工作
            JobData result = GetDailyJobsByNonExec(PeriodJob.NONE).FirstOrDefault();
            if (result != null)
                return result;

            result = GetDailyJobsByNonExec(PeriodJob.IS_PERIOD).FirstOrDefault();
            if (result != null)
                return result;

            //先找出非週期性工作
            result = GetWeeklyJobsByNonExec(PeriodJob.NONE).FirstOrDefault();
            if (result != null)
                return result;

            //再找出 Weekly 週期性工作
            result = GetWeeklyJobsByNonExec(PeriodJob.IS_PERIOD).FirstOrDefault();
            if (result != null)
                return result;

            //先找出 Monthly 非週期性工作
            result = GetMonthlyJobsByNonExec(PeriodJob.NONE).FirstOrDefault();
            if (result != null)
                return result;

            //再找出 Monthly 週期性工作
            result = GetMonthlyJobsByNonExec(PeriodJob.IS_PERIOD).FirstOrDefault();

            return result;
        }
        #endregion

        #region 取得是否有上一次未執行的 Daily 週期性排程
        //取得 Daily Job 直接存取 XML 資料
        public static IEnumerable<JobData> GetDailyJobsByNonExec(PeriodJob UsePeriod)
        {
            var result = from job in GetAllJobs()
                         where
                             job.UsePeriodType == PeriodType.BY_DAILY
                         select job;

            var result2 = result.Select(c => c);
            if (UsePeriod == PeriodJob.IS_PERIOD)
            {
                result2 = result2.Where(c => c.IsPeriodJob == UsePeriod
                    && (c.LastExecuteDateTime.HasValue ? ((DateTime.Now - c.LastExecuteDateTime.Value).TotalDays >= 1) : true)
                    );
            }
            else if (UsePeriod == PeriodJob.NONE)
            {
                result2 = result2.Where(c => (c.LastExecuteDateTime.HasValue ? ((DateTime.Now - c.LastExecuteDateTime.Value).TotalDays >= 1) : true));
            }
            return result2;
        }
        #endregion

        #region 是否有上一次未執行的 Weekly 週期性排程
        //是否有上一次未執行的週期性排程
        public static IEnumerable<JobData> GetWeeklyJobsByNonExec(PeriodJob UsePeriod)
        {
            var result = from job in GetAllJobs()
                         where job.UsePeriodType == PeriodType.BY_WEEKLY
                         select job;

            var result2 = result.Select(c => c);
            if (UsePeriod == PeriodJob.IS_PERIOD)
            {
                result2 = result2.Where(c => c.IsPeriodJob == UsePeriod
                    && (c.LastExecuteDateTime.HasValue ? (((DateTime.Now - c.LastExecuteDateTime.Value).TotalDays) >= 7) : true)
                    );
            }
            else if (UsePeriod == PeriodJob.NONE)
            {
                result2 = result2.Where(c => !c.LastExecuteDateTime.HasValue
                    && c.ExceuteWeek == (DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Now.DayOfWeek)
                    );
            }
            return result2;
        }
        #endregion

        #region 是否有上一次未執行的 Monthly 週期性排程
        //取得 Monthly Job
        public static IEnumerable<JobData> GetMonthlyJobsByNonExec(PeriodJob UsePeriod)
        {
            var result = from job in GetAllJobs()
                         where
                             job.UsePeriodType == PeriodType.BY_MONTHLY
                         select job;

            var result2 = result.Select(c => c);
            if (UsePeriod == PeriodJob.IS_PERIOD)
            {
                result2 = result2.Where(c => c.IsPeriodJob == UsePeriod
                    && (c.LastExecuteDateTime.HasValue ? ((DateTime.Now - c.LastExecuteDateTime.Value).TotalDays >= (c.LastExecuteDateTime.Value.AddMonths(1) - c.LastExecuteDateTime.Value).TotalDays) : true)
                    );
            }
            else if (UsePeriod == PeriodJob.NONE)
            {
                result2 = result2.Where(c => (c.LastExecuteDateTime.HasValue ? ((DateTime.Now - c.LastExecuteDateTime.Value).TotalDays >= (c.LastExecuteDateTime.Value.AddMonths(1) - c.LastExecuteDateTime.Value).TotalDays) : true));
            }
            return result2;
        }
        #endregion
    }
}
