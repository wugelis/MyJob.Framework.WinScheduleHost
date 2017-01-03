using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Store
{
    /// <summary>
    /// 提供序列化 Schedule Job 儲存體資料類別.
    /// </summary>
    [Serializable]
    public class ListJobDatas
    {
        /// <summary>
        /// XML 重複多筆 TAG
        /// </summary>
        public List<JobData> JobDatas { get; set; }
    }
}
