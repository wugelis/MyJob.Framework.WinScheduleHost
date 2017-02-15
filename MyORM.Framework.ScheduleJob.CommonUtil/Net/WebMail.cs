using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJob.CommonUtil.Net
{
    /// <summary>
    /// 傳送EMail物件.
    /// </summary>
    public class WebMail
    {
        private string mailServer = string.Empty;
        private string mailAddress = string.Empty;
        private string mailUserID = string.Empty;
        private string mailUserPW = string.Empty;
        private bool mailCredential = false;
        public static int timeOut = 30000;
        public string MailSubject { get; set; }
        public string MailBody { get; set; }

        private event SendCompletedEventHandler EventSendCompleted;
        /// <summary>
        /// 傳送EMail物件的Initialize.
        /// </summary>
        /// <param name="smtpServer">SMTP Server 位址</param>
        /// <param name="mailAccount">EMail 帳號</param>
        /// <param name="mailPassword">EMail 密碼</param>
        /// <param name="fromAddr">寄件者EMail位址</param>
        /// <param name="toAddr">收件者EMail位址</param>
        public WebMail(
            string smtpServer,
            string mailAccount,
            string mailPassword,
            string fromAddr,
            string[] toAddr)
        {
            SMTPServer = smtpServer;
            MailAccount = mailAccount;
            MailPassword = mailPassword;
            string _toAddr = toAddr.Length > 0 ? toAddr[0] : string.Empty; //以第一筆資料當作主要的to Address
            InitialMailMsg(
                new MailAddress(fromAddr, null),
                new MailAddress(_toAddr, null));

            mailMsg = new MailMessage();
            mailMsg.From = new MailAddress(fromAddr);

            for (int i = 0; i < toAddr.Length; i++)
            {
                mailMsg.To.Add(toAddr[i]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="smtpServer">SMTP Server 位址</param>
        /// <param name="mailAccount">EMail 帳號</param>
        /// <param name="mailPassword">EMail 密碼</param>
        public WebMail(string smtpServer, string mailAccount, string mailPassword)
        {
            SMTPServer = smtpServer;
            MailAccount = mailAccount;
            MailPassword = mailPassword;
        }
        /// <summary>
        /// 傳送EMail物件的Initialize.
        /// </summary>
        /// <param name="smtpServer"></param>
        public WebMail(string smtpServer)
        {
            SMTPServer = smtpServer;
        }

        public SendCompletedEventHandler SendCompleted
        {
            set { EventSendCompleted = value; }
            get { return EventSendCompleted; }
        }
        /// <summary>
        /// 寄送的 SMTP Server
        /// </summary>
        public string SMTPServer
        {
            set { mailServer = value; }
            get { return mailServer; }
        }
        /// <summary>
        /// 發送MAIL的帳號
        /// </summary>
        public string MailAccount
        {
            set { mailUserID = value; }
            get { return mailUserID; }
        }
        /// <summary>
        /// 發送MAIL的密碼
        /// </summary>
        public string MailPassword
        {
            set { mailUserPW = value; }
            get { return mailUserPW; }
        }
        /// <summary>
        /// 寄送MAIL是否需要 (帳／密)
        /// </summary>
        public bool Credential
        {
            set { mailCredential = value; }
            get { return mailCredential; }
        }
        /// <summary>
        /// 嘗試連線SMTP後多久 Timeout.
        /// </summary>
        public bool Timeout
        {
            set { mailCredential = value; }
            get { return mailCredential; }
        }
        /// <summary>
        /// 郵件是否使用 HTML 格式.
        /// </summary>
        public bool IsBodyHtml
        {
            get
            {
                return mailMsg.IsBodyHtml;
            }
            set { mailMsg.IsBodyHtml = value; }
        }

        MailAddress MailFrom = null;
        MailAddress MailTo = null;
        MailMessage mailMsg = null;
        /// <summary>
        /// EMail 的附加檔案.
        /// </summary>
        public AttachmentCollection MailAttachments
        {
            get
            {
                if (mailMsg != null)
                    return mailMsg.Attachments;
                return null;
            }
            //			set{ mailMsg.Attachments = value; }
        }

        void InitialMailMsg(MailAddress From, MailAddress To)
        {
            MailFrom = From;
            MailTo = To;
        }
        /// <summary>
        /// 寄送MAIL
        /// </summary>
        public void SendMail()
        {
            mailMsg.Subject = MailSubject;
            mailMsg.Body = MailBody;
            mailMsg.BodyEncoding = System.Text.Encoding.UTF8;

            SmtpClient smtpClient = new SmtpClient(SMTPServer);
            if (Credential)
                smtpClient.Credentials = new System.Net.NetworkCredential(MailAccount, MailPassword);  //若不需要驗證, 請將它去掉

            smtpClient.Timeout = timeOut;
            smtpClient.Send(mailMsg);
        }
    }
}
