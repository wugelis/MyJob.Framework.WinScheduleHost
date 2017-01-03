using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Store
{
    /// <summary>
    /// 動態取得 EventLog 的 Configuration.
    /// </summary>
    public class NLogHelper
    {
        /// <summary>
        /// 動態取得 EventLog 的 NLog Configuration.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public static Logger GetEventLogConfig(LogLevel logLevel)
        {
            LoggingConfiguration config = new LoggingConfiguration();

            //Create targets and add them to the configuration 
            var eventLogTarget = new EventLogTarget();
            eventLogTarget.Name = "eventLog1";

            //Set target properties 
            eventLogTarget.Layout = @"${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${onexception:${newline}${exception:format=tostring}}";
            eventLogTarget.Source = "[[應用程式名稱]]";
            eventLogTarget.Log = "Application";
            config.AddTarget("eventLog1", eventLogTarget);

            LoggingRule rule = new LoggingRule("*", logLevel, eventLogTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;

            Logger logger = LogManager.GetLogger("NLogHelper");

            return logger;
        }
        /// <summary>
        /// 動態取得 FileLog 的 NLog Configuration.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public static Logger GetFileLogConfig(LogLevel logLevel)
        {
            LoggingConfiguration config = new LoggingConfiguration();

            var fileLogTarget = new FileTarget();
            fileLogTarget.Name = "file1";

            fileLogTarget.Layout = @"${longdate} ${uppercase:${level}} ${message}";
            fileLogTarget.FileName = "${basedir}/logs/${shortdate}.log";

            config.AddTarget("file1", fileLogTarget);

            LoggingRule rule = new LoggingRule("*", logLevel, fileLogTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;

            Logger logger = LogManager.GetLogger("NLogHelper");

            return logger;
        }
        /// <summary>
        /// 動態取得 FileLog 的 NLog Configuration. (for ERROR 使用)
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public static Logger GetErrorFileLogConfig(LogLevel logLevel)
        {
            LoggingConfiguration config = new LoggingConfiguration();

            var fileLogTarget = new FileTarget();
            fileLogTarget.Name = "file2";

            fileLogTarget.Layout = @"${longdate} ${uppercase:${level}} ${message}";
            fileLogTarget.FileName = "${basedir}/logs/${shortdate}_ERROR.log";

            config.AddTarget("file2", fileLogTarget);

            LoggingRule rule = new LoggingRule("*", logLevel, fileLogTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;

            Logger logger = LogManager.GetLogger("NLogHelper");

            return logger;
        }
        /// <summary>
        /// 動態取得 EMailLog 的 NLog Configuration.
        /// </summary>
        /// <returns></returns>
        public static Logger GetEMailLogConfig(LogLevel logLevel)
        {
            LoggingConfiguration config = new LoggingConfiguration();

            var mailLogTarget = new MailTarget();
            mailLogTarget.Name = "mail";

            mailLogTarget.SmtpServer = "smtp.gmail.com";
            mailLogTarget.SmtpPort = 25;
            mailLogTarget.SmtpAuthentication = SmtpAuthenticationMode.Basic;
            mailLogTarget.SmtpUserName = "gelis.wu@mentortrust.com";
            mailLogTarget.SmtpPassword = "GELISpass01!";
            mailLogTarget.EnableSsl = false;
            mailLogTarget.From = "MIX Web.MxicWebHost &lt;gelis.wu@mentortrust.com&gt;";
            mailLogTarget.To = "gelis.wu@mentortrust.com";
            mailLogTarget.Html = true;
            mailLogTarget.Encoding = Encoding.UTF8;
            mailLogTarget.AddNewLines = true;
            mailLogTarget.Subject = "System message for Web.MxicWebHost：${machinename} 於 ${shortdate} ${time} Create ${level} record of level.";
            mailLogTarget.Header = "=========================================================================";
            mailLogTarget.Body = @"Time：${longdate}
                ${newline}Log Level：${level:uppercase=true}
                ${newline}Logger：${logger}
                ${newline}Source：${callsite:className=true}
                ${newline}Exception Type：${exception:format=type}
                ${newline}Error Message：${message}
                ${newline}${onexception:${exception:format=tostring}} ${newline}";

            mailLogTarget.Footer = "=========================================================================";

            config.AddTarget("mail", mailLogTarget);

            LoggingRule rule = new LoggingRule("*", logLevel, mailLogTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;

            Logger logger = LogManager.GetLogger("NLogHelper");

            return logger;
        }
        public static void LoggerWriter(Logger logger, string JobName, string DLLName, string LoginName)
        {
            LoggerWriter(logger, JobName, DLLName, LoginName, string.Empty, string.Empty);
        }
        public static void LoggerWriter(Logger logger, string JobName, string DLLName, string LoginName, string Action, string Message)
        {
            //將 Log 寫入到 FileSystem 中或 EventLog 事件檢視器中.
            if(!string.IsNullOrEmpty(Message) && !string.IsNullOrEmpty(Action))
            {
                //lock (logger)
                //{
                    logger.Info(string.Format("[{0}] {3} {1}.{2} {4}",
                        LoginName,
                        DLLName,
                        JobName,
                        Action,
                        Message));
                //}
            }
            else
            {
                //lock (logger)
                //{
                    logger.Info(string.Format("[{0}] 執行 {1}.{2}",
                        LoginName,
                        DLLName,
                        JobName));
                //}
            }
            
        }
        public static void LoggerExWriter(
            Logger logger, 
            string JobName, 
            string DLLName, 
            string LoginName,
            string ErrorMsg)
        {
            //將 Error Log 寫入到 FileSystem 中或 EventLog 事件檢視器中 或 透過 EMail 的方式寄出.
            //if (errorLevel.ErrorLevelType == LevelType.Error)
            //{
            lock(logger)
            {
                logger.Error(string.Format("[{0}] Executed fail... {1}.{2}, Error message：{3} ",
                        LoginName,
                        DLLName,
                        JobName,
                        ErrorMsg));
            }
            //}
            //else if (errorLevel.ErrorLevelType == LevelType.Fatal)
            //{
            //    logger.Fatal(string.Format("[{0}] 執行失敗 {1}.{2} [parame:{3}] {4}",
            //            LoginName,
            //            loadClass.GetType().Name,
            //            MethodName,
            //            ParamName,
            //            ex));
            //}
        }
    }
}
