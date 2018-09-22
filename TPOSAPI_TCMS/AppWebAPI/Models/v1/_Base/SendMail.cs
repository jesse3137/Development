using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace AppWebAPI.Models.v1
{
/// <summary>
/// 
/// </summary>
    public partial class SendMail
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">寄送的內容</param>
        /// <param name="mysubject">主旨</param>
        /// <param name="address">收信者MAIL</param>
        public void send_gmail(string msg, string mysubject, string address)
        {
            AppWebAPI.Adapters.WebDB.mailconfig mailset = new AppWebAPI.Adapters.WebDB.mailconfig();

            MailMessage message = new MailMessage(mailset.sendaddress, address);//MailMessage(寄信者, 收信者)

            message.IsBodyHtml = true;

            message.BodyEncoding = System.Text.Encoding.UTF8;//E-mail編碼
            message.SubjectEncoding = System.Text.Encoding.UTF8;//E-mail編碼
            message.Priority = MailPriority.Normal;//設定優先權
            message.Subject = mysubject;//E-mail主旨
            message.Body = msg;//E-mail內容

            SmtpClient MySmtp = new SmtpClient(mailset.smtp, int.Parse(mailset.port));//設定gmail的smtp
            MySmtp.Credentials = new System.Net.NetworkCredential(mailset.smtpaccount, mailset.smtppassword);//gmail的帳號密碼System.Net.NetworkCredential(帳號,密碼)
            MySmtp.EnableSsl = bool.Parse(mailset.enablessl);//開啟ssl
            MySmtp.Send(message);

            
            MySmtp = null;
            message.Dispose();
        }
    }
}