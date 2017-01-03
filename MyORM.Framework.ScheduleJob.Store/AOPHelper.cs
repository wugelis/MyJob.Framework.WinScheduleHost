using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Store
{
    public class AOPHelper
    {
        #region 取得自定義的 Attributes
        /// <summary>
        /// By MethodInfo 取得 CustomAttribute.
        /// </summary>
        /// <typeparam name="T">Custom Attribute 型別.</typeparam>
        /// <param name="mInfo">Method Infor object for Reflection.</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(MethodInfo mInfo)
            where T : class
        {
            var result = from m in mInfo.GetCustomAttributes(true).AsEnumerable()
                         where m.GetType() == typeof(T)
                         select m;

            return (T)result.FirstOrDefault(); //若取不到，回傳 null 值，供呼叫端判斷使用.
        }
        /// <summary>
        /// By Class Type 取得 CustomAttribute.
        /// 條件：傳入的 Type 須為 Class 類型.
        /// </summary>
        /// <typeparam name="T">Custom Attribute 型別.</typeparam>
        /// <param name="loadClass">Type of the Class.</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(Type loadClass)
            where T : class
        {
            if (loadClass.IsClass)
            {
                var result = from m in loadClass.GetCustomAttributes(true).AsEnumerable()
                             where m.GetType() == typeof(T)
                             select m;

                return (T)result.FirstOrDefault();
            }
            return null; //若取不到，回傳 null 值，供呼叫端判斷使用.
        }
        #endregion
    }
}
