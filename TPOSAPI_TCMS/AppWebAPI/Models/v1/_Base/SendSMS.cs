using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
namespace AppWebAPI.Models.v1
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SendSMS
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">寄送的內容</param>
        /// <param name="number">手機號碼</param>
        /// <param name="SendUrl">URL</param>
        public void send_sms(string SendUrl, string PostData)
        {
            //呼叫發送簡訊使用的URL                              
            Stream responseFromServer = Fun_POST_Url_GetStream(SendUrl, PostData);
            StreamReader reader = new StreamReader(responseFromServer);
            //未來可以紀錄傳送結果
            string isSuccess = reader.ReadToEnd();

            responseFromServer.Close();
            reader.Close();
        }

        public Stream Fun_POST_Url_GetStream(string strUrl, string strPostData)
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create(strUrl);
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            // ex.postData = "?FrmTest=ValTest&FrmTest2=ValTest2";
            byte[] byteArray = Encoding.UTF8.GetBytes(strPostData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            //SendMessage(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();

            return dataStream;
        }
    }
}