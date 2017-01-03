using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Store
{
    /// <summary>
    /// Attribute 資料保存容器.
    /// Create by Gelis at 2014/12/15.
    /// </summary>
    /// <typeparam name="T">Attribute 類型 T</typeparam>
    public class LogAttributeData<T>
        where T : class
    {
        #region BeforePrepareMethod
        private T _beforPrepareMethodAttr;
        /// <summary>
        /// 保存 beforPrepareMethod
        /// </summary>
        /// <param name="beforPrepareMethod"></param>
        public void SetBeforePrepareMethod(MethodInfo beforPrepareMethod)
        {
            _beforPrepareMethodAttr = AOPHelper.GetCustomAttribute<T>(beforPrepareMethod);
        }
        /// <summary>
        /// 取得 BeforePrepare 方法中的型別 T 的 Attribute 實體.
        /// </summary>
        /// <returns></returns>
        public T GetBeforePrepareAttr()
        {
            return _beforPrepareMethodAttr;
        }
        #endregion

        #region PrepareDataMethod
        private T _prepareDataMethodAttr;
        /// <summary>
        /// 保存 PrepareDataMethod
        /// </summary>
        /// <param name="prepareDataMethod"></param>
        public void SetPrepareDataMethod(MethodInfo prepareDataMethod)
        {
            _prepareDataMethodAttr = AOPHelper.GetCustomAttribute<T>(prepareDataMethod);
        }
        /// <summary>
        /// 取得 PrepareData 方法中的型別 T 的 Attribute 實體.
        /// </summary>
        /// <returns></returns>
        public T GetPrepareDataMethodAttr()
        {
            return _prepareDataMethodAttr;
        }
        #endregion

        #region ProcessDataMethod
        private T _processDataMethodAttr;
        /// <summary>
        /// 保存 ProcessDataMethod
        /// </summary>
        /// <param name="processDataMethod"></param>
        public void SetProcessDataMethod(MethodInfo processDataMethod)
        {
            _processDataMethodAttr = AOPHelper.GetCustomAttribute<T>(processDataMethod);
        }
        /// <summary>
        /// 取得 ProcessData 方法中的型別 T 的 Attribute 實體.
        /// </summary>
        /// <returns></returns>
        public T GetProcessDataMethodAttr()
        {
            return _processDataMethodAttr;
        }
        #endregion

        #region AfterProcessMethod
        private T _afterProcessMethodAttr;
        /// <summary>
        /// 保存 AfterProcessMethod
        /// </summary>
        /// <param name="afterProcessMethod"></param>
        public void SetAfterProcessMethod(MethodInfo afterProcessMethod)
        {
            _afterProcessMethodAttr = AOPHelper.GetCustomAttribute<T>(afterProcessMethod);
        }
        /// <summary>
        /// 取得 AfterProcess 方法中的型別 T 的 Attribute 實體.
        /// </summary>
        /// <returns></returns>
        public T GetAfterProcessMethodAttr()
        {
            return _afterProcessMethodAttr;
        }
        #endregion
    }
}
