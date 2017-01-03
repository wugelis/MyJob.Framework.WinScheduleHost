using GelisFrameworks.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Store
{
    /// <summary>
    /// 封裝與 Job 儲存體 相關的補助方法
    /// </summary>
    public class JobHelper
    {
        /// <summary>
        /// 取得 JobData 的 XML 檔案實體路徑.
        /// </summary>
        public static string JobXmlFile = 
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ListJobDatas.xml");
        /// <summary>
        /// 從 XML 檔中取回 ListJobDatas 物件
        /// </summary>
        /// <returns></returns>
        public static ListJobDatas LoadXmlFromFileSystem()
        {
            return LoadXmlFromFileSystem(string.Empty);
        }
        public static ListJobDatas LoadXmlFromFileSystem(string jobXml)
        {
            FileStream f = null;
            StreamReader sr = null;
            try
            {

                lock (JobXmlFile)
                {
                    if(string.IsNullOrEmpty(jobXml))
                    {
                        bool Error = false;
                        do
                        {
                            try
                            {
                                f = new FileStream(JobXmlFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                Error = false;
                            }
                            catch
                            {
                                Error = true;
                                Thread.Sleep(1000);
                            }
                        } while (Error);
                        
                        
                    }
                    else
                    {
                        f = new FileStream(jobXml, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    }
                    
                    sr = new StreamReader(f);
                    return SerializationHelper.DeSerializeAnObject<ListJobDatas>(sr.ReadToEnd());
                }
            }
            finally
            {
                sr.Close();
                f.Close();
            }
        }
        /// <summary>
        /// 將 ListJobDatas 儲存為 XML 檔案.
        /// </summary>
        /// <param name="jobList">ListJobDatas</param>
        /// <returns></returns>
        public static int SaveXml2FileSystem(ListJobDatas jobList)
        {
            return SaveXml2FileSystem(jobList, string.Empty);
        }
        /// <summary>
        /// 將 ListJobDatas 儲存為 XML 檔案 (指定 JobXml 完整路徑).
        /// </summary>
        /// <param name="jobList">ListJobDatas</param>
        /// <param name="jobXml">指定 JobXml 完整路徑</param>
        /// <returns></returns>
        public static int SaveXml2FileSystem(ListJobDatas jobList, string jobXml)
        {
            FileStream f = null;
            StreamWriter sw = null;
            try
            {
                lock (JobXmlFile)
                {
                    if (string.IsNullOrEmpty(jobXml))
                    {
                        f = new FileStream(JobXmlFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Write);
                    }
                    else
                    {
                        f = new FileStream(jobXml, FileMode.Create, FileAccess.ReadWrite, FileShare.Write);
                    }

                    sw = new StreamWriter(f);
                }
                sw.Write(SerializationHelper.SerializeAnObject(jobList));
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                sw.Close();
                f.Close();
            }
        }
        /// <summary>
        /// 驗證 (傳回 Tuple(true, string.Empty) 表示驗證成功)
        /// </summary>
        /// <param name="jobList">ListJobDatas</param>
        /// <returns></returns>
        public static Tuple<bool, string> Validation(ListJobDatas jobList)
        {
            var result = (from job in jobList.JobDatas.AsEnumerable()
                          select job).GroupBy(c => c.JobId)
                                .Select(c => new { JobId = c.Key, JobCount = c.Count() }).Where(c => c.JobCount > 1);

            if (result.Count() > 0)
            {
                return Tuple.Create(false, "Error！JobId already exists！");
            }
            else
            {
                return Tuple.Create(true, string.Empty);
            }
        }
    }
}