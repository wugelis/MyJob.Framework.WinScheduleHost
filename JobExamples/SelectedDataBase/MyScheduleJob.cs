using Mxic.Framework.ScheduleJobBase;
using Mxic.Framework.ServerComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelectedDataBase
{
    /// <summary>
    /// Schedule Job 元件
    /// </summary>
    public class MyScheduleJob: AbstractProcess
    {
        #region 實作 AbstractProcess 的方法
        /// <summary>
        /// 在開始Prepare之前要處理的事情
        /// </summary>
        [WriteLog(UseLogType.ToFileSystem)]
        protected override void BeforePrepare()
        {
        }
        #endregion

        #region 實作 PrepareData 的方法
        /// <summary>
        /// 準備 JOB 相關資料
        /// </summary>
        [WriteLog(UseLogType.ToFileSystem)]
        protected override void PrepareData()
        {
        }
        #endregion

        #region 實作 ProcessData 的方法
        /// <summary>
        /// 實際 JOB 處理資料
        /// </summary>
        [WriteLog(UseLogType.ToFileSystem)]
        [ErrorLevel(LevelType.Fatal)]
        protected override void ProcessData()
        {
            //在這裡寫實際 Job 要進行的內容.
            NorthwindEntities context = new NorthwindEntities();
            var result = from cus in context.Customers
                         where cus.CustomerID == "ALFKI"
                         select cus;

            List<Customers> list = result.ToList<Customers>();
        }
        #endregion

        #region 實作 AfterProcess 的方法
        /// <summary>
        /// 在開始AfterProcess之後要處理的事情
        /// </summary>
        protected override void AfterProcess()
        {
        }
        #endregion
    }
}
