using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.ComponentModel;

namespace BaseSet
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
        public bool send_gmail(string msg, string mysubject, string address)
        {
            AppWebAPI.Adapters.WebDB.mailconfig mailset = new AppWebAPI.Adapters.WebDB.mailconfig();

            /*MailMessage message = new MailMessage("GLORIA OUTLETS 華泰名品城<" + mailset.sendaddress + ">", address);//MailMessage(寄信者, 收信者)

            message.IsBodyHtml = true;

            message.BodyEncoding = System.Text.Encoding.UTF8;//E-mail編碼
            message.SubjectEncoding = System.Text.Encoding.UTF8;//E-mail編碼
            message.Priority = MailPriority.Normal;//設定優先權
            message.Subject = mysubject;//E-mail主旨
            message.Body = msg;//E-mail內容

            SmtpClient MySmtp = new SmtpClient(mailset.smtp, int.Parse(mailset.port));//設定gmail的smtp
            MySmtp.Credentials = new System.Net.NetworkCredential(mailset.smtpaccount, mailset.smtppassword);//gmail的帳號密碼System.Net.NetworkCredential(帳號,密碼)
            MySmtp.EnableSsl = bool.Parse(mailset.enablessl);//開啟ssl

            try
            {
                MySmtp.Send(message);

                MySmtp = null;
                message.Dispose();
                return true;
            }
            catch
            {
                MySmtp = null;
                message.Dispose();
                return false;
            }*/

            /*EASendMail.SmtpClient SMTP = new EASendMail.SmtpClient();
            System.Net.Mail.MailMessage Msg = new System.Net.Mail.MailMessage();
            String SMTPServer = mailset.smtp;

            EASendMail.SmtpMail oMail = new EASendMail.SmtpMail("TryIt");
            oMail.Subject = mysubject;
            oMail.From = "GLORIA OUTLETS 華泰名品城<" + mailset.sendaddress + ">";
            oMail.To = address;
            oMail.HtmlBody = msg;

            EASendMail.SmtpServer oSmtpServer = new EASendMail.SmtpServer(SMTPServer);
            oSmtpServer.Protocol = EASendMail.ServerProtocol.SMTP;
            oSmtpServer.ConnectType = EASendMail.SmtpConnectType.ConnectNormal;
            oSmtpServer.User = mailset.smtpaccount;
            oSmtpServer.Password = mailset.smtppassword;

            try
            {
                SMTP.SendMail(oSmtpServer, oMail);
                oSmtpServer = null;
                Msg.Dispose();
                return true;
            }
            catch
            {
                oSmtpServer = null;
                Msg.Dispose();
                return false;
            }*/

            //jmail.Message jmessage = new jmail.Message();
            //jmessage.Charset = "utf-8";
            //jmessage.ContentType = "text/html";
            //jmessage.From = "<" + mailset.sendaddress + ">";
            //jmessage.FromName = "GLORIA OUTLETS 華泰名品城";
            //jmessage.Subject = mysubject;
            //jmessage.AddRecipient(address, "", "");
            //jmessage.Body = msg;
            //jmessage.MailServerUserName = mailset.smtpaccount;
            //jmessage.MailServerPassWord = mailset.smtppassword;

            //try
            //{
            //    jmessage.Send(mailset.smtp, false);
            //    jmessage.Close();
            //    return true;
            //}
            //catch
            //{
            //    jmessage.Close();
            //    return false;
            //}

            return false;

        }
    }
}