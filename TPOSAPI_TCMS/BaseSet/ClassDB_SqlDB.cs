using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.ComponentModel;

namespace BaseSet
{
    /// <summary>
    /// 資料庫Class
    /// </summary>
    public class ClassDB_SqlDB
    {
        /// <summary>
        /// 資料庫Class
        /// </summary>
        /// <param name="strConnectionString">SQL連線字串</param>
        public ClassDB_SqlDB(string strConnectionString)
        {
            strConn = strConnectionString;
            BaseFunc = new BaseFunc();
            //1.15.12.30
            //1.調整，當ClassDB建構時，一併將oldbcn初始化。
            //1.16.1.5
            //3.調整，允許初始時，連線字串可為空。
            if (!string.IsNullOrEmpty(strConn))
                oldbcn = new SqlConnection(strConn);
        }

        /// <summary>
        /// DB Connection
        /// </summary>
        public string strConn = "";

        /// <summary>
        /// Ole Db Connection
        /// </summary>
        public SqlConnection oldbcn;

        /// <summary>
        /// BaseFunc
        /// </summary>
        BaseFunc BaseFunc;

        /// <summary>
        /// 測試Connection(回傳true=可以通訊)
        /// </summary>
        /// <returns></returns>
        public bool TryConnection()
        {
            oldbcn = new SqlConnection(strConn);
            try
            {
                oldbcn.Open();
                return true;
            }
            catch(Exception ex)
            {
                BaseFunc.funWriteLog("DB連線失敗:" + strConn + "\r\n" + ex.ToString());
                return false;
            }
            finally
            {
                //1.13.12.3
                //1.修正,TryConnection,當第一次連線,第二次斷線時,會無效的bug
                //oldbcn.Close();
            }
        }

        /// <summary>
        /// 取得資料(SELECT用) 
        /// </summary>
        /// <param name="strSql">Sql Command</param>
        /// <returns></returns>
        public DataTable GetData(string strSql)
        {
            DataTable dt1 = new DataTable();
            
            if (oldbcn == null)
                oldbcn = new SqlConnection(strConn);

            SqlCommand oldbcm = new SqlCommand();
            oldbcm.Connection = oldbcn;
            oldbcm.CommandType = CommandType.Text;
            oldbcm.CommandText = strSql;

            SqlDataAdapter oldba = new SqlDataAdapter(oldbcm);
            oldba.SelectCommand.CommandTimeout = 6000;  //100分鐘

            try
            {
                //2.14.11.06
                // 調整讀取速度，當olecn是open，就不會自動關閉
                if (oldbcn.State == ConnectionState.Open)
                {
                    oldba.Fill(dt1);
                }
                else
                {
                    try
                    {
                        oldbcn.Open();
                        oldba.Fill(dt1);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        oldbcn.Close();
                        oldbcn = null;
                    }
                }
            }
            catch (Exception ex)
            {
                BaseFunc.funWriteLog("SQL SELECT錯誤: " + strSql + "\r\n" + ex.ToString());
                dt1 = null;
            }
            finally
            {
                //v2.13.11.11
                //增加，取得資料後，將資源回收。
                oldbcm = null;
                oldba = null;
                //GC.Collect();
            }

            return dt1;       
        }

        /// <summary>
        /// 取得資料(SELECT用)批次
        /// </summary>
        /// <param name="strSql">Sql Command</param>
        /// <returns></returns>
        public DataTable GetData(string strSql, int startRecord, int maxRecords)
        {
            oldbcn = new SqlConnection(strConn);

            DataTable dt1 = new DataTable();
            oldbcn.ConnectionString = strConn;

            SqlCommand oldbcm = new SqlCommand();
            oldbcm.Connection = oldbcn;
            oldbcm.CommandType = CommandType.Text;
            oldbcm.CommandText = strSql;

            SqlDataAdapter oldba = new SqlDataAdapter(oldbcm);
            oldba.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            oldba.SelectCommand.CommandTimeout = 6000; //100分鐘

            try
            {
                oldbcn.Open();
                oldba.Fill(startRecord, maxRecords, dt1);
            }
            catch (Exception ex)
            {
                BaseFunc.funWriteLog("SQL SELECT錯誤: " + strSql + "\r\n" + ex.ToString());
                dt1 = null;
            }
            finally
            {
                oldbcn.Close();
            }
            return dt1;
        }

        /// <summary>
        /// 更新資料(DELETE,UPDATE,INSERT用)
        /// </summary>
        /// <param name="strSql">Sql Command</param>
        /// <returns></returns>
        public bool UpdData(string strSql)
        {
            oldbcn = new SqlConnection(strConn);

            SqlCommand oldbcm = new SqlCommand();
            oldbcm.CommandType = CommandType.Text;

            SqlTransaction oldbTran = null;
            
            try
            {
                oldbcn.Open();
                oldbTran = oldbcn.BeginTransaction(IsolationLevel.ReadCommitted);
                oldbcm.Connection = oldbcn;
                oldbcm.Transaction = oldbTran;
                oldbcm.CommandText = strSql;
                oldbcm.ExecuteNonQuery();
                oldbTran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                //1.15.12.30
                //2.修正，當Update發生Exception時，無法Rollback會造成的錯誤。
                try { if (!(oldbTran == null)) oldbTran.Rollback(); }
                catch { }
                BaseFunc.funWriteLog("SQL EXECUTE錯誤: " + strSql + "\r\n" + ex.ToString());
                return false;
            }
            finally
            {
                oldbcn.Close();
            }
        }

        /// <summary>
        /// 更新資料(DELETE,UPDATE,INSERT用)
        /// </summary>
        /// <param name="List_strSql"></param>
        /// <returns></returns>
        public bool UpdData(List<string> strSql_list)
        {
            oldbcn = new SqlConnection(strConn);

            SqlCommand oldbcm = new SqlCommand();
            oldbcm.CommandType = CommandType.Text;

            SqlTransaction oldbTran = null;

            string strSql = "";
            try
            {
                oldbcn.Open();
                oldbTran = oldbcn.BeginTransaction(IsolationLevel.ReadCommitted);
                oldbcm.Connection = oldbcn;
                oldbcm.Transaction = oldbTran;
                foreach(var s in strSql_list)
                {
                    strSql = s;
                    oldbcm.CommandText = strSql;
                    oldbcm.ExecuteNonQuery();
                }
                oldbTran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                //1.15.12.30
                //2.修正，當Update發生Exception時，無法Rollback會造成的錯誤。
                try { if (!(oldbTran == null)) oldbTran.Rollback(); }
                catch { }
                BaseFunc.funWriteLog("SQL EXECUTE錯誤: " + strSql + "\r\n" + ex.ToString());
                return false;
            }
            finally
            {
                oldbcn.Close();
            }
        }

        /// <summary>
        /// 更新資料(DELETE,UPDATE,INSERT用)
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <param name="I_oldbcn">MySqlConnection</param>
        /// <param name="I_oldbTran">MySqlTransaction</param>
        /// <returns></returns>
        public bool UpdData(string strSql, SqlConnection I_oldbcn, SqlTransaction I_oldbTran)
        {
            SqlCommand oldbcm = new SqlCommand();
            oldbcm.CommandType = CommandType.Text;
            try
            {
                oldbcm.Connection = I_oldbcn;
                oldbcm.Transaction = I_oldbTran;
                oldbcm.CommandText = strSql;
                oldbcm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                BaseFunc.funWriteLog("SQL EXECUTE錯誤: " + strSql + "\r\n" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 更新資料(INSERT用)
        /// </summary>
        /// <param name="UpdateDT">要更新的資料表</param>
        /// <param name="strSelectSql">要更新的Sql Command</param>
        /// <returns></returns>
        public bool UpdData(DataTable UpdateDT, string strSelectSql)
        {
            //MySql連線
            SqlConnection oConnection = new SqlConnection(strConn);
            //資料橋接器
            SqlDataAdapter oDataAdapter = new SqlDataAdapter();
            oDataAdapter.SelectCommand = new SqlCommand(strSelectSql, oConnection);
            //SQL Command產生器
            SqlCommandBuilder oCommandBuilder = new SqlCommandBuilder(oDataAdapter);
            //Trans
            SqlTransaction oldbTran = null;
            //Command
            SqlCommand oldbcm = new SqlCommand();
            oldbcm.CommandType = CommandType.Text;
            oldbcm.Connection = oConnection;
            oldbcm.CommandText = oCommandBuilder.GetInsertCommand().CommandText;
            //Temp DataRow for Debug Message
            DataRow dr = UpdateDT.NewRow();
            try
            {
                //oConnection.Open();
                //oldbTran = oConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                //oDataAdapter.SelectCommand.Transaction = oldbTran;
                //oDataAdapter.Update(UpdateDT);
                //oldbTran.Commit();
                
                //1.15.9.25 -> 1.15.10.28
                //1.調整, UpdData(DataTable UpdateDT, string strSelectSql)，改為MySqlCommand Insert
                oConnection.Open();
                oldbTran = oConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                oldbcm.Transaction = oldbTran;
                foreach (DataRow mydr in UpdateDT.Rows)
                {
                    dr = mydr;
                    foreach (DataColumn mydc in UpdateDT.Columns)
                    {
                        oldbcm.Parameters.AddWithValue("@" + mydc.ColumnName, mydr[mydc.ColumnName]);
                    }
                    oldbcm.ExecuteNonQuery();
                    oldbcm.Parameters.Clear();
                }
                oldbTran.Commit();
            }
            catch (Exception ex)
            {
                try { if (!(oldbTran == null)) oldbTran.Rollback(); }
                catch { }
                //BaseFunc.funWriteLog("SQL EXECUTE錯誤: " + strSelectSql + "\r\n" + ex.ToString());
                BaseFunc.funWriteLog("SQL EXECUTE錯誤: " + oCommandBuilder.GetInsertCommand().CommandText + "\r\n"
                    + string.Join(",", dr.ItemArray) + "\r\n"
                    + ex.ToString());
                return false;
            }
            finally
            {
                oConnection.Close();
            }
            return true;
        }

        /// <summary>
        /// 更新資料(DELETE,UPDATE,INSERT用)
        /// </summary>
        /// <param name="strSql">Sql Command</param>
        /// <returns></returns>
        public bool UpdData(DataTable UpdateDT, SqlDataAdapter oDataAdapter)
        {
            SqlConnection oConnection = oDataAdapter.SelectCommand.Connection;
            
            try
            {
                oConnection.Open();
                oDataAdapter.Update(UpdateDT);
                oConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                BaseFunc.funWriteLog("SQL EXECUTE錯誤: " + "\r\n" + ex.ToString());
                return false;
            }
            finally
            {
                oConnection.Close();
            }
        }

        /// <summary>
        /// 取得資料表Schema
        /// </summary>
        /// <param name="strSql">Sql Command(SELECT)</param>
        /// <returns></returns>
        public DataTable GetSchema(string strSql)
        {
            oldbcn = new SqlConnection(strConn);

            SqlDataAdapter oDataAdapter = new SqlDataAdapter();
            //SQL Command產生器
            SqlCommandBuilder oCommandBuilder = new SqlCommandBuilder();
            oDataAdapter.SelectCommand = new SqlCommand(strSql, oldbcn);
            oCommandBuilder = new SqlCommandBuilder(oDataAdapter);

            DataTable myDT = new DataTable();
            //開啟Connection
            try
            {
                oldbcn.Open();
                oDataAdapter.FillSchema(myDT, SchemaType.Mapped);
            }  
            catch (Exception ex)
            {
                //1.15.12.30
                //3.調整，GetSchema錯誤，寫入err_log。
                //ex.ToString();
                BaseFunc.funWriteLog("SQL GetSchema錯誤: " + strSql + "\r\n" + ex.ToString());
                return myDT;
            }
            finally
            {
                oldbcn.Close();
            }
            return myDT;
        }

        /// <summary>
        /// 執行預存程式(Procedure)-直接執行CommandText
        /// </summary>
        /// <param name="Procedure_Command">Procedure Command</param>
        /// <returns></returns>
        public bool RunProcedure_Command(string Procedure_Command)
        {
            SqlTransaction oldbTran = null;
            try
            {
                oldbcn = new SqlConnection(strConn);
                SqlCommand oldbcm = new SqlCommand();
                oldbcm = oldbcn.CreateCommand();

                oldbcm.CommandType = CommandType.Text;
                oldbcm.CommandText = Procedure_Command;                
                
                oldbcn.Open();
                oldbTran = oldbcn.BeginTransaction(IsolationLevel.ReadCommitted);
                oldbcm.Transaction = oldbTran;
                oldbcm.ExecuteNonQuery();
                oldbTran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                //1.14.11.06 -> 1.15.7.21
                //1.修正, RunProcedure_CommandThis, 問題:「MySqlTransaction has completed; it is no longer usable.」
                try { if (!(oldbTran == null)) oldbTran.Rollback(); }
                catch { }
                BaseFunc.funWriteLog("SQL Procedure錯誤: " + Procedure_Command + "\r\n" + ex.ToString());
                return false;
            }
            finally
            {
                oldbcn.Close();
            }
        }

        /// <summary>
        /// 執行預存程式(Procedure)
        /// </summary>
        /// <param name="ProcedureName">預存程式名稱</param>
        /// <param name="oParam">預存程式參數</param>
        /// <returns></returns>
        public bool RunProcedure(string ProcedureName, params SqlParameter[] oParam)
        {
            oldbcn = new SqlConnection(strConn);
            SqlCommand oldbcm = new SqlCommand();
            oldbcm = oldbcn.CreateCommand();

            oldbcm.CommandType = CommandType.StoredProcedure;
            oldbcm.CommandText = ProcedureName;

            SqlTransaction oldbTran = null;

            if (oParam != null)
            {
                foreach (SqlParameter myParam in oParam)
                {
                    oldbcm.Parameters.Add(myParam);
                }
            }

            string strParam = "";
            //如果使用Oracle
            if (strConn.ToUpper().IndexOf("ORACLE") > -1)
            {                
                if (oParam != null)
                {
                    oldbcm.CommandType = CommandType.Text;
                    foreach (SqlParameter myParam in oParam)
                    {
                        strParam += "'" + myParam.Value + "'" + ",";
                    }
                    strParam = strParam.TrimEnd(',');
                    oldbcm.CommandText = @"begin " + ProcedureName + "(" + strParam + "); end;";
                }
            }

            try
            {
                oldbcn.Open();
                oldbTran = oldbcn.BeginTransaction(IsolationLevel.ReadCommitted);
                oldbcm.Transaction = oldbTran;
                oldbcm.ExecuteNonQuery();
                oldbTran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                //1.15.12.30
                //2.修正，當Update發生Exception時，無法Rollback會造成的錯誤。
                try { if (!(oldbTran == null)) oldbTran.Rollback(); }
                catch { }
                BaseFunc.funWriteLog("SQL Procedure錯誤: " + ProcedureName + "(" + strParam + ")\r\n" + ex.ToString());
                return false;
            }
            finally
            {
                oldbcn.Close();
            }
        }

        /// <summary>
        /// Get Value From IMS3_BASE.PARAM
        /// </summary>
        /// <param name="strTitle">TITLE</param>
        /// <param name="strTag">TAG</param>
        /// <returns></returns>
        public object GetParamValue(string strTitle, string strTag)
        {
            return GetTableValue("CONTENT", "IMS3_BASE.PARAM", "WHERE TITLE = '" + strTitle + "' AND TAG = '" + strTag + "'");
        }

        /// <summary>
        /// Get Value From Table
        /// </summary>
        /// <param name="I_chaForeignFieldName">Select [?]</param>
        /// <param name="I_chaForeignTableName">From [?]</param>
        /// <param name="I_chaWhere">Where [?]</param>
        /// <returns></returns>
        public object GetTableValue(string I_chaForeignFieldName,
            string I_chaForeignTableName,
            string I_chaWhere)
        {
            string strSql = "SELECT " + I_chaForeignFieldName;
            strSql = strSql + " FROM " + I_chaForeignTableName;
            strSql = strSql + " WHERE " + I_chaWhere;

            DataTable myDT = GetData(strSql);
            int CountDT = myDT.Rows.Count;            
            if (CountDT > 0)
            {
                object retObj = myDT.Rows[0][0];
                myDT.Dispose();
                return retObj;
            }
            else
            {
                myDT.Dispose();
                return "";
            }
        }

        /// <summary>
        /// DataTable to Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="connString"></param>
        public void DataTableToExcel(DataTable dt, string connString)
        {
            //先算出欄位及列數
            int rows = dt.Rows.Count;
            int cols = dt.Columns.Count;
            //用來建立命令 
            StringBuilder sb = new StringBuilder();
            //檢查TableName是否為空
            if (dt.TableName == "") dt.TableName = "sheet1";
            sb.Append("CREATE TABLE ");
            sb.Append(dt.TableName + " ( ");
            //用來做開TABLE的欄名資訊
            for (int i = 0; i < cols; i++)
            {
                if (i < cols - 1)
                    sb.Append(string.Format("{0} NTEXT,", dt.Columns[i].ColumnName));
                else
                    sb.Append(string.Format("{0} NTEXT)", dt.Columns[i].ColumnName));
            }
            //把要開啟的臨時Excel建立起來
            using (SqlConnection objConn = new SqlConnection(connString))
            {
                SqlCommand objCmd = new SqlCommand();
                objCmd.Connection = objConn;

                objCmd.CommandText = sb.ToString();


                objConn.Open();
                //先執行CreateTable的任務
                objCmd.ExecuteNonQuery();


                //開始處理資料內容的新增
                #region 開始處理資料內容的新增
                //把之前 CreateTable 清空
                sb.Remove(0, sb.Length);
                sb.Append("INSERT INTO ");
                sb.Append(dt.TableName + " ( ");
                //這邊開始組該Excel欄位順序
                for (int i = 0; i < cols; i++)
                {
                    if (i < cols - 1)
                        sb.Append(dt.Columns[i].ColumnName + ",");
                    else
                        sb.Append(dt.Columns[i].ColumnName + ") values (");
                }
                //這邊組 DataTable裡面的值要給到Excel欄位的
                for (int i = 0; i < cols; i++)
                {
                    if (i < cols - 1)
                        sb.Append("@" + dt.Columns[i].ColumnName + ",");
                    else
                        sb.Append("@" + dt.Columns[i].ColumnName + ")");
                }
                #endregion


                //建立插入動作的Command
                objCmd.CommandText = sb.ToString();
                SqlParameterCollection param = objCmd.Parameters;

                for (int i = 0; i < cols; i++)
                {
                    param.Add(new SqlParameter("@" + dt.Columns[i].ColumnName, SqlDbType.VarChar));
                }

                //使用參數化的方式來給予值
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < param.Count; i++)
                    {
                        param[i].Value = row[i];
                    }
                    //執行這一筆的給值
                    objCmd.ExecuteNonQuery();
                }


            }//end using
        }
    }
}
