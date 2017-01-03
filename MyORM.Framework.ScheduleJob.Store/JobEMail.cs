using GelisFrameworks.Net.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.Store
{
    /// <summary>
    /// 
    /// </summary>
    public class JobEMail
    {
        private string _fromAddr = string.Empty;
        private string[] _toAddr = null;

        private WebMail _webMail = null;
        /// <summary>
        /// Constructor for Mail.
        /// </summary>
        /// <param name="toAddr"></param>
        public JobEMail(string[] toAddr)
        {
            _webMail = new WebMail(ConfigurationManager.AppSettings["SmtpServer"],
                ConfigurationManager.AppSettings["MailAccount"],
                ConfigurationManager.AppSettings["MailPwd"],
                ConfigurationManager.AppSettings["FromAddr"],
                toAddr);
        }
        /// <summary>
        /// 寄送通知信件
        /// </summary>
        public void SendMail()
        {
            _webMail.SendMail();
        }
    }
}
