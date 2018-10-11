using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Net;
using System.Collections;
using System.Reflection;

namespace TC.EDI.UTIL
{
    /// <summary>
    /// 公用功能Class
    /// </summary>
    public class ClassFunction
    {
        string assemblyFolder;

        /// <summary>
        /// 公用功能Class
        /// </summary>
        public ClassFunction()
        {
            //assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string codeBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            UriBuilder uri = new UriBuilder(codeBase);
            assemblyFolder = Uri.UnescapeDataString(uri.Path);

            strPathLog = assemblyFolder;
            strPathDBLog = Path.Combine(assemblyFolder, @"DBLog");

            funAutoDirectory(strPathDBLog);            
        }

        /// <summary>
        /// DBLog路徑
        /// </summary>
        public string strPathDBLog = @"DBLog";

        /// <summary>
        /// Log位置
        /// </summary>
        public string strPathLog = "";

        /// <summary>
        /// 寫入Log(錯誤用)
        /// </summary>
        /// <param name="strLog">LOG內容</param>
        public void funWriteLog(string strLog)
        {
            //string strPath = strPathDBLog + @"\ErrLog_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            //1.14.2.12 Log記錄區分年月
            string strPath = Path.Combine(strPathDBLog, DateTime.Today.ToString("yyyy"), DateTime.Today.ToString("MM"));
            funAutoDirectory(strPath);
            strPath = Path.Combine(strPath, String.Format("ErrLog_{0}.log",DateTime.Today.ToString("yyyyMMdd")));

            try
            {
                using (StreamWriter sw = new StreamWriter(strPath, true))
                {
                    string strMessage = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ">" + strLog;
                    Console.WriteLine(strMessage);
                    sw.WriteLine(strMessage);
                }
            }
            catch { }

        }

        /// <summary>
        /// 寫入Log(正確用)
        /// </summary>
        /// <param name="strLog">LOG內容</param>
        public void funWriteRunLog(string strLog)
        {
            //string strPath = strPathDBLog + @"\RunLog_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            //1.14.2.12 Log記錄區分年月
            string strPath = Path.Combine(strPathDBLog, DateTime.Today.ToString("yyyy"), DateTime.Today.ToString("MM"));
            funAutoDirectory(strPath);
            strPath = Path.Combine(strPath, String.Format("RunLog_{0}.log",DateTime.Today.ToString("yyyyMMdd")));

            try
            {
                using (StreamWriter sw = new StreamWriter(strPath, true))
                {
                    string strMessage = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ">" + strLog;
                    Console.WriteLine(strMessage);
                    sw.WriteLine(strMessage);
                }
            }
            catch { }

        }

        /// <summary>
        /// 寫入Log
        /// </summary>
        /// <param name="strLog">LOG內容</param>
        /// <param name="strFileName">檔案名稱</param>
        public void funWriteLog(string strLog,string strFileName)
        {
            //string strPath = strPathDBLog + @"\" + strFileName + "_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            //1.14.2.12 Log記錄區分年月
            string strPath = Path.Combine(strPathDBLog, DateTime.Today.ToString("yyyy"), DateTime.Today.ToString("MM"));
            funAutoDirectory(strPath);
            strPath = Path.Combine(strPath, String.Format("{0}_{1}.log", strFileName, DateTime.Today.ToString("yyyyMMdd")));

            try
            {
                using (StreamWriter sw = new StreamWriter(strPath, true))
                {
                    string strMessage = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ">" + strLog;
                    Console.WriteLine(strMessage);
                    sw.WriteLine(strMessage);
                }
            }
            catch { }
        }

        /// <summary>
        /// 自動建立資料夾
        /// </summary>
        /// <param name="sFolderName"></param>
        public DirectoryInfo funAutoDirectory(string sFolderName)
        {
            if (!Directory.Exists(sFolderName))
            {
                return Directory.CreateDirectory(sFolderName);
            }
            else
            {
                return new DirectoryInfo(sFolderName);
            }
        }

        /// <summary>
        /// 讀取檔案(指定目錄)
        /// </summary>
        /// <param name="sPath">檔案存放位置 ex: @"Import\"</param>
        /// <param name="sSearchPattern">檔案篩選方式 ex: "ATT*.TXT"</param>
        public FileInfo[] funReadFiles(string sPath, string sSearchPattern)
        {
            //取目錄資訊
            DirectoryInfo myDI = new DirectoryInfo(sPath);
            FileInfo[] myFI = myDI.GetFiles(sSearchPattern);
            funWriteLog("讀取檔案-Path:" + sPath, "RunLog");                   //寫入LOG-讀取檔案
            funWriteLog("讀取檔案-SearchPattern:" + sSearchPattern, "RunLog"); //寫入LOG-讀取檔案
            funWriteLog("讀取檔案-檔案數量:" + myFI.Length, "RunLog");         //寫入LOG-讀取檔案
            return myFI;
        }

        /// <summary>
        /// 讀取檔案(指定目錄)
        /// </summary>
        /// <param name="sPath">檔案存放位置 ex: @"Import\"</param>
        /// <param name="sSearchPattern">檔案篩選方式 ex: "ATT*.TXT"</param>
        /// <param name="so">搜尋目前目錄或子目錄</param>
        public FileInfo[] funReadFiles(string sPath, string sSearchPattern, SearchOption so)
        {
            //取目錄資訊
            DirectoryInfo myDI = new DirectoryInfo(sPath);
            FileInfo[] myFI = myDI.GetFiles(sSearchPattern, so);
            funWriteLog("讀取檔案-Path:" + sPath, "RunLog");                   //寫入LOG-讀取檔案
            funWriteLog("讀取檔案-SearchPattern:" + sSearchPattern, "RunLog"); //寫入LOG-讀取檔案
            funWriteLog("讀取檔案-檔案數量:" + myFI.Length, "RunLog");         //寫入LOG-讀取檔案
            return myFI;
        }

        /// <summary>
        /// 將某字串截取某bytes的字串，解決中文字串問題 
        /// </summary>
        /// <param name="a_SrcStr">將截取之字串</param>
        /// <param name="a_StartIndex">這個執行個體中子字串之以零起始的起始字元位置。</param>
        /// <param name="a_Cnt">子字串中的字元數。</param>
        /// <returns></returns>
        public string SubStr(string a_SrcStr, int a_StartIndex, int a_Cnt, Encoding FileEncoding)
        {            
            //Encoding l_Encoding = Encoding.GetEncoding("big5", new EncoderExceptionFallback(), new DecoderReplacementFallback(""));
            Encoding l_Encoding = FileEncoding;
            byte[] l_byte = l_Encoding.GetBytes(a_SrcStr);
            if (a_Cnt <= 0)
                return "";
            //例若長度10 
            //若a_StartIndex傳入9 -> ok, 10 ->不行 
            if (a_StartIndex + 1 > l_byte.Length)
                return "";
            else
            {
                //若a_StartIndex傳入9 , a_Cnt 傳入2 -> 不行 -> 改成 9,1 
                if (a_StartIndex + a_Cnt > l_byte.Length)
                    a_Cnt = l_byte.Length - a_StartIndex;
            }
            return l_Encoding.GetString(l_byte, a_StartIndex, a_Cnt);
        }

        /// <summary>
        /// 上傳檔案至FTP
        /// </summary>
        /// <param name="chaftp_address">FTP地址 ex: "ftp.rsl.com.tw"</param>
        /// <param name="chaftp_username">使用者帳號 ex: "RSL"</param>
        /// <param name="chaftp_password">使用者密碼 ex: "rsl1234"</param>
        /// <param name="chaftp_remote_directory">FTP遠端路徑 ex: "IMS/MATSU/VENDOR"</param>
        /// <param name="chaftp_files_to_put">檔案名稱(含副檔名) ex: "file.txt"</param>
        /// <param name="chaftp_local_directory">本機路徑 ex: "Z:\ASP\TransSAP\SYSB200\CSV"</param>
        /// <param name="FileEncoding">檔案編碼</param>
        /// <param name="_strPathLog">Path Log</param>
        /// <returns>FTP回應訊息</returns>
        public string FunFileUploadToFTP(
            string chaftp_address,
            string chaftp_username,
            string chaftp_password,
            string chaftp_remote_directory,
            string chaftp_files_to_put,
            string chaftp_local_directory,
            Encoding FileEncoding,
            out string _strPathLog
            )
        {
            //FTP執行LOG檔的檔名
            string strFtpRunLog = "FtpRunLog";
            //執行Log
            string strExLog = "";

            //設定file URI ex: ftp://ftp.rsl.com.tw/IMS/MATSU/VENDOR/file.txt
            string uriString = "";
            if (chaftp_remote_directory.Trim() == "")
                uriString = @"ftp://" + chaftp_address + @"/" + chaftp_files_to_put;
            else
                uriString = @"ftp://" + chaftp_address + @"/" + chaftp_remote_directory + "/" + chaftp_files_to_put;

            Uri myFileUri = new Uri(uriString);

            // 設定上傳物件
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(myFileUri);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            
            // --- 寫傳送Log --- 
            strExLog = "傳送URI:" + myFileUri.AbsoluteUri;
            funWriteLog(strExLog, strFtpRunLog);

            // 設定帳號及密碼
            request.Credentials = new NetworkCredential(chaftp_username, chaftp_password);

            // --- 寫傳送Log --- 
            strExLog = "FTP帳號:" + chaftp_username;
            funWriteLog(strExLog, strFtpRunLog);

            // 將檔案複製到資料流內
            string strLocalPath = chaftp_local_directory + @"\" + chaftp_files_to_put;
            StreamReader sourceStream = new StreamReader(strLocalPath, FileEncoding);
            byte[] fileContents = FileEncoding.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            // --- 寫傳送Log --- 
            strExLog = "傳送本機檔案:" + Path.GetFullPath(strLocalPath);
            funWriteLog(strExLog, strFtpRunLog);

            // 開始上傳
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            // 回傳訊息
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            string strResponse = response.StatusDescription;
            response.Close();

            // --- 寫傳送Log ---   
            strExLog = "FTP Server 回傳訊息:" + strResponse;
            funWriteLog(strExLog, strFtpRunLog);

            //設定FTP Log位置
            _strPathLog = strPathLog;

            return strResponse;
        }

        /// <summary>
        /// FTP檔案下載
        /// </summary>
        /// <param name="downloadUrl">下載FTP的目錄ex : ftp//127.0.0.1/abc.xml</param>
        /// <param name="TargetPath">本機存檔目錄</param>
        /// <param name="UserName">使用者FTP登入帳號</param>
        /// <param name="Password">使用者登入密碼</param>
        /// <param name="FileEncoding">檔案編碼</param>
        /// <param name="IS_Delete">是否刪除FTP上的檔案(true=刪除)</param>
        /// <returns></returns>
        public FileInfo FunFileDownload(string downloadUrl, string TargetPath, string UserName, string Password, Encoding FileEncoding, bool IS_Delete)
        {
            funWriteRunLog(String.Format("FTP檔案下載:[downloadUrl]={0},[TargetPath]={1},[UserName]={2},[FileEncoding]={3},[IS_Delete]={4}"
                                        , downloadUrl, TargetPath, UserName, FileEncoding.ToString(), IS_Delete.ToString()));
            Stream responseStream = null;
            FileStream fileStream = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(downloadUrl);
                downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile; //設定Method下載檔案
                if (UserName.Length > 0)//如果需要帳號登入
                {
                    NetworkCredential nc = new NetworkCredential(UserName, Password);
                    downloadRequest.Credentials = nc;//設定帳號
                }
                FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();
                responseStream = downloadResponse.GetResponseStream();//取得FTP伺服器回傳的資料流
                string fileName = Path.GetFileName(downloadRequest.RequestUri.AbsolutePath);
                if (fileName.Length == 0)
                {
                    reader = new StreamReader(responseStream, FileEncoding);
                    throw new Exception(reader.ReadToEnd());
                }
                else
                {
                    fileStream = File.Create(TargetPath + @"\" + fileName);
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    while (true)
                    {//開始將資料流寫入到本機
                        bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                            break;
                        fileStream.Write(buffer, 0, bytesRead);
                    }

                    //刪除檔案
                    if (IS_Delete)
                    {
                        FtpWebRequest delRequest = (FtpWebRequest)WebRequest.Create(downloadUrl);
                        delRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                        if (UserName.Length > 0)//如果需要帳號登入
                        {
                            NetworkCredential nc = new NetworkCredential(UserName, Password);
                            delRequest.Credentials = nc;//設定帳號
                        }
                        delRequest.GetResponse();
                    }
                }
                
                downloadResponse.Close();
                return new FileInfo(TargetPath + @"\" + fileName);
            }
            catch (Exception ex)
            {
                funWriteLog("FTP檔案下載錯誤:" + ex.ToString());
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                else if (responseStream != null)
                    responseStream.Close();
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        /// <summary>
        /// FTP檔案清單
        /// </summary>
        /// <param name="listUrl">FTP伺服器路徑 ftp://127.0.0.1</param>
        /// <param name="UserName">使用者FTP登入帳號</param>
        /// <param name="Password">使用者登入密碼</param>
        /// <param name="FileEncoding">檔案編碼</param>
        /// <returns></returns>
        public Hashtable FunFTPList(string listUrl, string UserName, string Password, Encoding FileEncoding)
        {
            funWriteRunLog(String.Format("FTP檔案清單:[listUrl]={0},[UserName]={1},[FileEncoding]={2}"
                                        , listUrl, UserName, FileEncoding.ToString()));
            StreamReader reader = null;
            try
            {
                FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(listUrl);
                listRequest.Method = WebRequestMethods.Ftp.ListDirectory;// //設定Method取得目錄資訊
                if (UserName.Length > 0)//如果需要帳號登入
                {
                    NetworkCredential nc = new NetworkCredential(UserName, Password);
                    listRequest.Credentials = nc; //設定帳號
                }
                FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse();
                reader = new StreamReader(listResponse.GetResponseStream(), FileEncoding);

                Hashtable ht = new Hashtable();
                if (reader != null)
                {
                    // Display the data received from the server.
                    string strReadToEnd = reader.ReadToEnd();
                    if (strReadToEnd != "")
                    {
                        string[] strArrayReadToEnd = strReadToEnd.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < strArrayReadToEnd.Length; i++)
                        {
                            ht[i] = strArrayReadToEnd[i];
                        }
                    }
                } 

                listResponse.Close();
                return ht;//回傳目前清單
            }
            catch (Exception ex)
            {
                funWriteLog("FTP檔案清單取得錯誤:" + ex.ToString());
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
