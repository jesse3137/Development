﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WinSCP;

namespace BaseSet
{
    public class BaseFunc
    {
        /// <summary>
        /// DBLog路徑
        /// </summary>
        public string strPathDBLog = @"bin\ErrLog";

        /// <summary>
        /// 寫入Log(錯誤用)
        /// </summary>
        /// <param name="strLog">LOG內容</param>
        public void funWriteLog(string strLog)
        {
            //string strPath = strPathDBLog + @"\ErrLog_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            //1.14.2.12 Log記錄區分年月
            string strPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, strPathDBLog, DateTime.Today.ToString("yyyy"), DateTime.Today.ToString("MM"));
            funAutoDirectory(strPath);
            strPath = Path.Combine(strPath, String.Format("ErrLog_{0}.log", DateTime.Today.ToString("yyyyMMdd")));

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
        /// 使用Log路徑
        /// </summary>
        public string strPathUseLog = @"bin\UseLog";

        /// <summary>
        /// 寫入Log(錯誤用)
        /// </summary>
        /// <param name="strLog">LOG內容</param>
        public void funWriteUseLog(string strLog)
        {
            //string strPath = strPathDBLog + @"\UseLog_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            //1.14.2.12 Log記錄區分年月
            string strPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, strPathUseLog, DateTime.Today.ToString("yyyy"), DateTime.Today.ToString("MM"));
            funAutoDirectory(strPath);
            strPath = Path.Combine(strPath, String.Format("UseLog_{0}.log", DateTime.Today.ToString("yyyyMMdd")));

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

        public string GetMD5(string Data)
        {
            System.Security.Cryptography.MD5 MD5Provider = System.Security.Cryptography.MD5.Create();
            byte[] source = System.Text.Encoding.Default.GetBytes(Data);//將字串轉為Byte[]
            byte[] crypto = MD5Provider.ComputeHash(source);//進行MD5加密

            string result = "";
            for (int i = 0; i < crypto.Length; i++)
            {
                result += crypto[i].ToString("x2");//把加密後的字串從Byte[]轉為字串
            }

            return result;
        }

        /// <summary>
        /// FTP上傳(單檔)
        /// </summary>
        /// <param name="strPath">檔案完整路徑</param>
        /// <returns></returns>
        public bool FileUpLoadToFTP(string strPath, string backuppath, string ftpFolder, string ftpAddress, string ftpUserName, string ftpPassword, string ftpPort, string ftpSecure)
        {
            return FileUpLoadToFTP(new List<FileInfo> { new FileInfo(strPath) } , backuppath, ftpFolder, ftpAddress, ftpUserName, ftpPassword, ftpPort, ftpSecure);
        }

        /// <summary>
        /// FTP上傳
        /// </summary>
        /// <param name="FileInfoArr">上傳檔案陣列</param>
        /// <returns></returns>
        public bool FileUpLoadToFTP(List<FileInfo> FileInfoArr, string backuppath, string ftpFolder, string ftpAddress, string ftpUserName, string ftpPassword, string ftpPort, string ftpSecure)
        {
            //BaseSet.BaseFunc bf = new BaseSet.BaseFunc();

            FtpSecure setftpSecure;

            int iftpPort = int.Parse(ftpPort == "" ? "21" : ftpPort);

            switch (ftpSecure)
            {
                case "Implicit": setftpSecure = FtpSecure.Implicit; break;
                case "Explicit": setftpSecure = FtpSecure.Explicit; break;
                default: setftpSecure = FtpSecure.None; break;
            }

            DateTime dtBegin = DateTime.Now;
            bool bolSuccess = true;

            //2.15.10.27
            //FTP上傳後備份資料夾
            if (!string.IsNullOrEmpty(backuppath))
            {
                if (!(Directory.Exists(backuppath)))
                {
                    Directory.CreateDirectory(backuppath);
                }
            }

            //v2.15.9.25
            //7.調整，FTP改為使用WinSCP套件
            using (Session ftpSession = new Session())
            {
                // Connect
                bool bolConnect = false;    //是否已連線
                int intRetryMax = 3;        //重試上限
                int intRetryCount = 0;      //重試次數
                while (!bolConnect && intRetryCount < intRetryMax)
                {
                    //v2.15.9.25
                    //10.增加，FTP連線重試機制，最多3次。
                    try
                    {
                        intRetryCount++;

                        if (ftpSecure == "")
                        {
                            ftpSession.Open(new SessionOptions
                            {
                                //FTP
                                Protocol = Protocol.Ftp,
                                HostName = ftpAddress,
                                UserName = ftpUserName,
                                Password = ftpPassword,
                                PortNumber = iftpPort,
                                FtpSecure = setftpSecure,
                                GiveUpSecurityAndAcceptAnySshHostKey = (setftpSecure == FtpSecure.None) ? false : true
                            });
                        }
                        else
                        {
                            //FTPS
                            ftpSession.Open(new SessionOptions
                            {
                                Protocol = Protocol.Ftp,
                                HostName = ftpAddress,
                                UserName = ftpUserName,
                                Password = ftpPassword,
                                PortNumber = iftpPort,
                                FtpSecure = setftpSecure,
                                GiveUpSecurityAndAcceptAnyTlsHostCertificate = (setftpSecure == FtpSecure.None) ? false : true
                            });
                        }

                        //v2.15.10.27 
                        //檢查ftp資料夾不存在時，自動建立ftp資料夾
                        if (ftpFolder != "")
                            if (!ftpSession.FileExists(ftpFolder.TrimEnd('/'))) ftpSession.CreateDirectory(ftpFolder.TrimEnd('/'));

                        bolConnect = true;
                    }
                    catch (Exception ex)
                    {
                        bolConnect = false;
                        funWriteLog(
                            String.Format(@"連線FTP失敗,重試第{0}/{1}次.", intRetryCount, intRetryMax)
                            );
                        funWriteLog(
                            String.Format(@"連線FTP失敗,重試第{0}/{1}次.\r\n{2}", intRetryCount, intRetryMax, ex.Message)
                            );
                    }
                }

                if (bolConnect)
                {
                    //上傳檔案
                    TransferOperationResult transferResult;
                    foreach (FileInfo myFI in FileInfoArr)
                    {
                        string strlocalpath = "";
                        try
                        {
                            strlocalpath = myFI.FullName;
                            // Upload file
                            transferResult = ftpSession.PutFiles(strlocalpath, ftpFolder.TrimEnd('/') + '/', false, new TransferOptions { TransferMode = TransferMode.Binary });
                            // Throw on any error
                            transferResult.Check();

                            //2.15.12.30
                            //7.FTP上傳，記錄成功上傳的檔案名稱
                            funWriteUseLog(string.Format(@"上傳FTP檔案[{0}]成功!", myFI.Name));
                        }
                        catch (Exception ex)
                        {
                            funWriteLog(String.Format("上傳至FTP時錯誤:檔名={0}\r\n{1}", strlocalpath, ex.Message));
                            bolSuccess = false;
                            break;
                        }

                        //2.15.10.27
                        //FTP上傳後移動檔案至備份資料夾
                        try
                        {
                            if (!string.IsNullOrEmpty(backuppath))
                            {
                                string destFileName = Path.Combine(backuppath, myFI.Name);
                                string sourceFileName = myFI.FullName;
                                if (File.Exists(destFileName)) destFileName += "." + DateTime.Now.ToString("yyyyMMddHHmmss");
                                File.Move(sourceFileName, destFileName);
                            }

                            //2.15.12.30
                            //8.FTP上傳，記錄FTP上傳後移動檔案的檔案名稱
                            funWriteUseLog(string.Format(@"FTP上傳後移動檔案[{0}]至備份資料夾成功!", myFI.Name));
                        }
                        catch (Exception ex)
                        {
                            funWriteLog(String.Format("FTP上傳後移動檔案至備份資料夾時,發生錯誤:檔名={0}\r\n{1}", myFI.FullName, ex.Message));
                            bolSuccess = false;
                            break;
                        }
                    }

                    ftpSession.Close();
                }
                else
                {
                    bolSuccess = false;
                    funWriteLog(@"連線FTP失敗,無法傳送檔案.");

                }
            }

            //2.15.12.30
            //6.FTP上傳，正常執行的Log改為背景記錄至DBLog資料夾
            //Form_Main.WriteLog("結束FTP上傳,耗時:" + (DateTime.Now - dtBegin).ToString());
            funWriteUseLog("結束FTP上傳,耗時:" + (DateTime.Now - dtBegin).ToString());
            return bolSuccess;
        }

        /// <summary>
        /// FTP下載
        /// </summary>
        /// <param name="FileInfoArr">下載檔案陣列</param>
        /// <returns></returns>
        public bool FileDownLoadToFTP(string backuppath, string ftpFolder, string ftpAddress, string ftpUserName, string ftpPassword, string ftpPort, string ftpSecure)
        {
            //BaseSet.BaseFunc bf = new BaseSet.BaseFunc();

            FtpSecure setftpSecure;

            int iftpPort = int.Parse(ftpPort == "" ? "21" : ftpPort);

            switch (ftpSecure)
            {
                case "Implicit": setftpSecure = FtpSecure.Implicit; break;
                case "Explicit": setftpSecure = FtpSecure.Explicit; break;
                default: setftpSecure = FtpSecure.None; break;
            }

            DateTime dtBegin = DateTime.Now;
            bool bolSuccess = true;

            //2.15.10.27
            //FTP下傳接收資料夾
            if (!string.IsNullOrEmpty(backuppath))
            {
                if (!(Directory.Exists(backuppath)))
                {
                    Directory.CreateDirectory(backuppath);
                }
            }

            //v2.15.9.25
            //7.調整，FTP改為使用WinSCP套件
            using (Session ftpSession = new Session())
            {
                // Connect
                bool bolConnect = false;    //是否已連線
                int intRetryMax = 3;        //重試上限
                int intRetryCount = 0;      //重試次數
                while (!bolConnect && intRetryCount < intRetryMax)
                {
                    //v2.15.9.25
                    //10.增加，FTP連線重試機制，最多3次。
                    try
                    {
                        intRetryCount++;

                        if (ftpSecure == "")
                        {
                            ftpSession.Open(new SessionOptions
                            {
                                //FTP
                                Protocol = Protocol.Ftp,
                                HostName = ftpAddress,
                                UserName = ftpUserName,
                                Password = ftpPassword,
                                PortNumber = iftpPort,
                                FtpSecure = setftpSecure,
                                GiveUpSecurityAndAcceptAnySshHostKey = (setftpSecure == FtpSecure.None) ? false : true
                            });
                        }
                        else
                        {
                            //FTPS
                            ftpSession.Open(new SessionOptions
                            {
                                Protocol = Protocol.Ftp,
                                HostName = ftpAddress,
                                UserName = ftpUserName,
                                Password = ftpPassword,
                                PortNumber = iftpPort,
                                FtpSecure = setftpSecure,
                                GiveUpSecurityAndAcceptAnyTlsHostCertificate = (setftpSecure == FtpSecure.None) ? false : true
                            });
                        }

                        bolConnect = true;
                    }
                    catch (Exception ex)
                    {
                        bolConnect = false;
                        funWriteLog(
                            String.Format(@"連線FTP失敗,重試第{0}/{1}次.", intRetryCount, intRetryMax)
                            );
                        funWriteLog(
                            String.Format(@"連線FTP失敗,重試第{0}/{1}次.\r\n{2}", intRetryCount, intRetryMax, ex.Message)
                            );
                    }
                }

                if (bolConnect)
                {
                    //下傳檔案
                    TransferOperationResult transferResult;
                    string strlocalpath = "";
                    try
                    {
                        strlocalpath = "";
                        // Upload file
                        transferResult = ftpSession.GetFiles(ftpFolder.TrimEnd('/') + "*", backuppath, false, new TransferOptions { TransferMode = TransferMode.Binary });//*表示全部檔案;
                        // Throw on any error
                        transferResult.Check();

                        funWriteUseLog(string.Format(@"下傳FTP檔案成功!"));
                    }
                    catch (Exception ex)
                    {
                        funWriteLog(String.Format("下傳FTP錯誤:{0}", strlocalpath, ex.Message));
                        bolSuccess = false;
                    }
                    ftpSession.Close();
                }
                else
                {
                    bolSuccess = false;
                    funWriteLog(@"連線FTP失敗,無法傳送檔案.");

                }
            }

            //2.15.12.30
            //6.FTP上傳，正常執行的Log改為背景記錄至DBLog資料夾
            //Form_Main.WriteLog("結束FTP上傳,耗時:" + (DateTime.Now - dtBegin).ToString());
            funWriteUseLog("結束FTP下傳,耗時:" + (DateTime.Now - dtBegin).ToString());
            return bolSuccess;
        }

    }
}
