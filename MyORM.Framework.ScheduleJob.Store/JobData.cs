using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Store
{
    /// <summary>
    /// 是否為週期性工作
    /// 注意：若不是週期性工作，則該工作只會執行一次，且只會在 StartTime 符合時執行。
    /// </summary>
    public enum PeriodJob
    {
        /// <summary>
        /// 非週期性工作 (只會執行一次)
        /// </summary>
        NONE = 0,
        /// <summary>
        /// 週期性工作 (此時 PeriodType 的設定才有效)
        /// </summary>
        IS_PERIOD = 1
    }
    /// <summary>
    /// 週期性的類型
    /// </summary>
    public enum PeriodType
    {
        /// <summary>
        /// BY 天執行
        /// </summary>
        BY_DAILY = 0,
        /// <summary>
        /// BY 週執行一次
        /// </summary>
        BY_WEEKLY = 1,
        /// <summary>
        /// BY 月執行一次
        /// </summary>
        BY_MONTHLY = 2
    }
    /// <summary>
    /// Schedule Job 的定義
    /// 注意：只有實作 IJob 
    /// </summary>
    //[Serializable]
    [DataContract]
    public class JobData
    {
        /// <summary>
        /// Schedule Job 的流水號 (不可重複)
        /// </summary>
        [DataMember]
        public string JobId { get; set; }
        /// <summary>
        /// Schedule Job 的工作名稱.
        /// </summary>
        [DataMember]
        public string JobName { get; set; }
        /// <summary>
        /// Schedule Job 的 DLL 的檔案名稱
        /// </summary>
        [DataMember]
        public string DLLName { get; set; }
        /// <summary>
        /// Schedule Job 內的命名空間 (不包括類別名稱)
        /// </summary>
        [DataMember]
        public string NameSpace { get; set; }
        /// <summary>
        /// Schedule Job 的 類別名稱
        /// </summary>
        [DataMember]
        public string ClassName { get; set; }
        /// <summary>
        /// 週期性執行的時間 Time，搭配 IsPeriodJob 一起使用
        /// 注意：
        /// 1. 需要 IsPeriodJob 設為 IS_PERIOD 以及 ExecuteTime 不為 00:00 才有效
        /// 2. 若有設定此時間，此時間的優先權最高，格式必須為 HH:mm，否則系統會發生錯誤
        /// 3. 
        /// </summary>
        [DataMember]
        public string ExecuteTime { get; set; }
        /// <summary>
        /// 每個月的某一天執行
        /// 注意：只在當月有效的天數內有效
        /// </summary>
        [DataMember]
        public int ExecuteDay { get; set; }
        /// <summary>
        /// 固定每個星期幾的 StartTime 會執行
        /// 注意：只有當週期性工作 PeriodJob 為 IS_PERIOD 時，以及 PeriodType = BY_WEEKLY && StartTime 不為空值時有效
        /// </summary>
        [DataMember]
        public int ExceuteWeek { get; set; }
        /// <summary>
        /// 固定每個月的 StartTime 會執行
        /// 注意：只有當週期性工作 PeriodJob 為 IS_PERIOD 時，以及 PeriodType = BY_MONTHLY && StartTime 不為空值時有效
        /// </summary>
        [DataMember]
        public int ExecuteMonth { get; set; }
        /// <summary>
        /// Schedule Job 的開始執行時間，只有時間的部分.
        /// 注意：格式必須為 HH:mm
        /// </summary>
        [DataMember]
        public string StartTime { get; set; }
        /// <summary>
        /// 是否為週期性工作
        /// </summary>
        [DataMember]
        public PeriodJob IsPeriodJob { get; set; }
        /// <summary>
        /// 週期性的類型 (月、週、天)
        /// </summary>
        [DataMember]
        public PeriodType UsePeriodType { get; set; }
        /// <summary>
        /// 上一次執行的時間 (若為 null 則表示從未執行過)
        /// Engine 會使用這個值來判斷上一次執行過的時間
        ///     如果是 Daily Job：當天執行過就不再執行。
        ///     如果是 Weekly Job：則判別與上一次執行日是否滿一週，滿一週才執行。
        /// </summary>
        [DataMember]
        public DateTime? LastExecuteDateTime { get; set; }
        /// <summary>
        /// 是否啟用，啟用的 Job 才會被載入到 Running Table
        /// </summar
        [DataMember]
        public bool Enabled { get; set; }
        /// <summary>
        /// 是否進行 ReTry?
        /// 預設：0 不進行 ReTry
        /// </summary>
        [DataMember]
        public int ReTry { get; set; }
        /// <summary>
        /// 該 Job 的（負責人／被通知人） EMail.
        /// </summary>
        [DataMember]
        public string EMailAddress { get; set; }
    }
}
