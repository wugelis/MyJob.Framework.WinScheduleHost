using MyORM.Framework.ScheduleJob.Store;
using MyORM.Framework.ScheduleJobBase;
using Dummies.ServerComponent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Core
{
    /// <summary>
    /// 動態讀取 Assembly DLL 的反射類別
    /// </summary>
    public class LoadAssemblyHelper
    {
        /// <summary>
        /// 呼叫的排程的主要方法名稱 (Main)
        /// </summary>
        private const string JOB_METHOD_NAME = "DoJob";
        private const string BEFORE_PREPARE_METHOD_NAME = "BeforePrepare";
        private const string PREPARE_DATA_METHOD_NAME = "PrepareData";
        private const string PROCESS_DATA_METHOD_NAME = "ProcessData";
        private const string AFTER_PROCESS_METHOD_NAME = "AfterProcess";

        private static string dllPath = string.Empty;

        /// <summary>
        /// 傳入 Assembly 物件，從 Assembly 指定的 NameSpace & ClassName 到 Assembly 中取得並回傳 MethodInfo.
        /// </summary>
        /// <param name="ass">Assembly for Job DLL</param>
        /// <param name="Job">JobData 物件</param>
        /// <returns>Tuple 物件 (包含：MethodIfo, new Job Instance)</returns>
        public static Tuple<MethodInfo, object, LogAttributeData<WriteLogAttribute>, LogAttributeData<WriteExceptionLogAttribute>, LogAttributeData<ErrorLevelAttribute>> Common(
            Assembly ass,
            JobData Job)
        {
            object jobClassObject = null;
            LogAttributeData<WriteLogAttribute> writeLogAttr = new LogAttributeData<WriteLogAttribute>();
            LogAttributeData<WriteExceptionLogAttribute> writeExAttr = new LogAttributeData<WriteExceptionLogAttribute>();
            LogAttributeData<ErrorLevelAttribute> errorLevelAttr = new LogAttributeData<ErrorLevelAttribute>();

            MethodInfo jobMethod = null;
            MethodInfo BeforePrepareMethod = null;
            MethodInfo PrepareDataMethod = null;
            MethodInfo ProcessDataMethod = null;
            MethodInfo AfterProcessMethod = null;

            Type magicType = ass.GetType(string.Format("{0}.{1}", Job.NameSpace, Job.ClassName));

            if (magicType != null)
            {
                //判別 Load 進來的 Class 型態是不是實作 IAction 的介面，若不是，則 throw Exception.
                if (magicType.GetInterfaces().AsEnumerable<Type>().Where(c => c == typeof(IAction)).FirstOrDefault()==null)
                {
                    throw new IActionNotImplementException(string.Format("The {0} of class {1} is must be to inheritance by interface of IAtion.", Job.DLLName, Job.ClassName))
                    {
                        Datetime = DateTime.Now,
                        MethodInfo = string.Format("DLLPath={0}, DLLName={1}, NameSpace={2}, ClassName={3}", dllPath, Job.DLLName, Job.NameSpace, Job.ClassName)
                    };
                }

                ConstructorInfo jobConstructor = magicType.GetConstructor(Type.EmptyTypes);
                jobClassObject = jobConstructor.Invoke(null);

                jobMethod = magicType.GetMethod(JOB_METHOD_NAME);
                BeforePrepareMethod = magicType.GetMethod(BEFORE_PREPARE_METHOD_NAME, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic);
                PrepareDataMethod = magicType.GetMethod(PREPARE_DATA_METHOD_NAME, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic);
                ProcessDataMethod = magicType.GetMethod(PROCESS_DATA_METHOD_NAME, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic);
                AfterProcessMethod = magicType.GetMethod(AFTER_PROCESS_METHOD_NAME, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic);

                #region 包裝 WriteLogAttribute 容器
                writeLogAttr.SetBeforePrepareMethod(BeforePrepareMethod);
                writeLogAttr.SetPrepareDataMethod(PrepareDataMethod);
                writeLogAttr.SetProcessDataMethod(ProcessDataMethod);
                writeLogAttr.SetAfterProcessMethod(AfterProcessMethod);
                #endregion

                #region 包裝 WriteLogExAttribute 容器
                writeExAttr.SetBeforePrepareMethod(BeforePrepareMethod);
                writeExAttr.SetPrepareDataMethod(PrepareDataMethod);
                writeExAttr.SetProcessDataMethod(ProcessDataMethod);
                writeExAttr.SetAfterProcessMethod(AfterProcessMethod);
                #endregion

                #region 包裝 ErrorLevelAttribute 容器
                errorLevelAttr.SetBeforePrepareMethod(BeforePrepareMethod);
                errorLevelAttr.SetPrepareDataMethod(PrepareDataMethod);
                errorLevelAttr.SetProcessDataMethod(ProcessDataMethod);
                errorLevelAttr.SetAfterProcessMethod(AfterProcessMethod);
                #endregion

                if (jobMethod == null)
                {
                    throw new JobMethodNotFoundException("Please check the method is correct!")
                    {
                        Datetime = DateTime.Now,
                        MethodInfo = string.Format("DLLPath={0}, DLLName={1}, NameSpace={2}, ClassName={3}", dllPath, Job.DLLName, Job.NameSpace, Job.ClassName)
                    };
                }
            }
            else
            {
                throw new JobMethodNotFoundException("Please check the namespace or class name is correct!")
                {
                    Datetime = DateTime.Now,
                    MethodInfo = string.Format("DLLPath={0}, DLLName={1}, NameSpace={2}, ClassName={3}", dllPath, Job.DLLName, Job.NameSpace, Job.ClassName)
                }; 
            }

            return Tuple.Create(jobMethod, jobClassObject, writeLogAttr, writeExAttr, errorLevelAttr);
        }

        /// <summary>
        /// 傳入 JobData 物件，從 JobData 指定的 NameSpace & ClassName 到 Assembly 中取得並回傳 MethodInfo.
        /// </summary>
        /// <param name="Job">JobData 物件</param>
        public static Tuple<MethodInfo, object, LogAttributeData<WriteLogAttribute>, LogAttributeData<WriteExceptionLogAttribute>, LogAttributeData<ErrorLevelAttribute>> GetExecuteJobMethod(JobData Job)
        {
            MethodInfo result = null;
            object jobClassObject = null;
            LogAttributeData<WriteLogAttribute> writeLogAttr = new LogAttributeData<WriteLogAttribute>();
            LogAttributeData<WriteExceptionLogAttribute> writeExAttr = new LogAttributeData<WriteExceptionLogAttribute>();
            LogAttributeData<ErrorLevelAttribute> errorLevelAttr = new LogAttributeData<ErrorLevelAttribute>();

            string appPath = string.Empty;

            string nameSapce = string.Empty;

            try
            {
                Assembly ExecuteAssembly = Assembly.GetExecutingAssembly();
                appPath = Path.GetDirectoryName(ExecuteAssembly.Location);

                dllPath = string.Format("{0}.dll", Job.DLLName);

                // 判斷上傳的實體路徑下是否有此文件，沒有提示錯誤信息
                if (!System.IO.File.Exists(Path.Combine(appPath, dllPath)))
                {
                    throw new JobDLLNotFoundException(string.Format("The {0} DLL is not exist!", dllPath))
                    {
                        Datetime = DateTime.Now,
                        MethodInfo = string.Format("DLLPath={0}, DLLName={1}, NameSpace={2}, ClassName={3}", dllPath, Job.DLLName, Job.NameSpace, Job.ClassName)
                    };
                }
                else
                {
                    //要讀取的 DLL 路徑.
                    string assPath = Path.Combine(appPath, dllPath);
                    string dllConfigPath = string.Format("{0}.config", assPath);
                    Assembly ass = null;

                    //先判斷是否有這個 DLL 的 dll.config
                    if(File.Exists(dllConfigPath))
                    {
                        var resultValue = GetCustomConfig(assPath, dllConfigPath, "connectionStrings");
                        ass = resultValue.Item1;
                    }
                    else
                    {
                        //載入指定路徑中的 Assembly.
                        ass = Assembly.LoadFile(assPath);
                    }

                    if (ass != null)
                    {
                        Type magicType = ass.GetType(string.Format(".", Job.NameSpace, Job.ClassName));
                        var resultValue = Common(ass, Job);
                        result = resultValue.Item1;
                        jobClassObject = resultValue.Item2;
                        writeLogAttr = resultValue.Item3;
                        writeExAttr = resultValue.Item4;
                        errorLevelAttr = resultValue.Item5;
                    }
                    else
                    {
                        throw new JobDLLNotFoundException(string.Format("The {0} DLL is not exist!", dllPath))
                        {
                            Datetime = DateTime.Now,
                            MethodInfo = string.Format("DLLPath={0}, DLLName={1}, NameSpace={2}, ClassName={3}", dllPath, Job.DLLName, Job.NameSpace, Job.ClassName)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new JobDLLNotFoundException(ex.Message)
                {
                    Datetime = DateTime.Now,
                    MethodInfo = string.Format("DLLPath={0}, DLLName={1}, NameSpace={2}, ClassName={3}", dllPath, Job.DLLName, Job.NameSpace, Job.ClassName)
                };
            }

            return Tuple.Create(result, jobClassObject, writeLogAttr, writeExAttr, errorLevelAttr);
        }
        /// <summary>
        /// 
        /// </summary>
        private static Assembly configurationDefiningAss = null;
        protected static Tuple<Assembly, ConfigurationSection> GetCustomConfig(
            string configDefiningAssemblyPath,
            string configFilePath,
            string sectionName)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new
                ResolveEventHandler(ConfigResolveEventHandler);

            configurationDefiningAss = Assembly.LoadFrom(configDefiningAssemblyPath);
            var exeFileMap = new ExeConfigurationFileMap();
            exeFileMap.ExeConfigFilename = configFilePath;
            var customConfig = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap,
                ConfigurationUserLevel.None, true);

            ConfigurationSection returnConfig = customConfig.GetSection(sectionName);
            AppDomain.CurrentDomain.AssemblyResolve -= ConfigResolveEventHandler;

            return Tuple.Create(configurationDefiningAss, returnConfig);
        }

        private static Assembly ConfigResolveEventHandler(object sender, ResolveEventArgs args)
        {
            return configurationDefiningAss;
        }
    }
}
