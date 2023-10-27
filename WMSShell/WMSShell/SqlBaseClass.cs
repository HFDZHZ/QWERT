using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql;
using MySql.Data.Common;
using MySql.Data.Types;
using System.Text.RegularExpressions;
using System.IO;

namespace SYMC_MIS.ItemClass
{
    class SqlBaseClass
    {
        #region   代码中用到的变量
        MySqlDataAdapter S_Da;    //声明数据适配器对象
        DataSet S_Ds;           //声明数据集对象
        MySqlCommand S_Com;

        MySqlConnection S_Con;  //声明链接对象

        private string server = null;      //声明服务器名   
        private string userid = null;      //声明用户名
        private string password = null;    //声明密码
        private string datebase = null;    //声明数据库名
        private string port = "3306";        //声明端口号
        private string charset = "UTF-8";     //声明编码方式
        #endregion

        #region 字段属性
        public string Server
        {
            get { return server; }
            set { server = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }
        public string DateBase
        {
            get { return datebase; }
            set { datebase = value; }
        }
        public string Port
        {
            get { return port; }
            set { port = value; }
        }
        public string CharSet
        {
            get { return charset; }
            set { charset = value; }
        }
        #endregion

        #region 构造函数
        public SqlBaseClass(string m_server, string m_userid, string m_password, string m_database)
        {
            this.server = m_server;
            this.userid = m_userid;
            this.password = m_password;
            this.datebase = m_database;
        }

        public SqlBaseClass()
        { }
        #endregion

        #region   连接数据库
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns></returns>
        public MySqlConnection GetCon()
        {
            string C_string = "server=" + Server + ";user id=" + UserID + ";password=" + Password + ";database=" + DateBase + ";";
            S_Con = new MySqlConnection(C_string);
            S_Con.Open();
            return S_Con;
        }

        #endregion

        #region 执行SQL语句
        public bool SetExecute(string cmdtxt)
        {
            S_Com = new MySqlCommand(cmdtxt, GetCon());
            
            try
            {
                S_Com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message, "错误提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (GetCon().State == ConnectionState.Open)
                {
                    GetCon().Close();
                    GetCon().Dispose();
                }
            }
        }
        #endregion

        #region    返回数据集类型
        /// <summary>
        /// 返回数据集类型
        /// </summary>
        /// <param name="cmdtxt">需要查询的SQL语句</param>
        /// <returns></returns>
        public DataSet GetDs(string cmdtxt, string strTable)
        {
            try
            {
                S_Da = new MySqlDataAdapter(cmdtxt, GetCon());
                S_Ds = new DataSet();
                S_Da.Fill(S_Ds, strTable);
                return S_Ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message, "错误提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (GetCon().State == ConnectionState.Open)
                {
                    GetCon().Close();
                    GetCon().Dispose();
                }
            }
        }
        #endregion

        #region    返回数据表类型
        /// <summary>
        /// 返回数据表
        /// </summary>
        /// <param name="cmdtxt"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public DataRowCollection GetTableRows(string cmdtxt)
        {
            System.Data.DataTable P_tbl;   //声明一个DataTable对象
            DataRowCollection T_rows;
            try
            {
                S_Da = new MySqlDataAdapter(cmdtxt, GetCon());
                P_tbl = new System.Data.DataTable();
                S_Da.Fill(P_tbl);   //将表中对象放入P_tbl中
                T_rows = P_tbl.Rows;
                return T_rows;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message, "错误提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (GetCon().State == ConnectionState.Open)
                {
                    GetCon().Close();
                    GetCon().Dispose();
                }
            }
        }
        #endregion

        #region 返回DataTableL类型数据
        public DataTable GetDataTable(string cmdtxt)
        {
            System.Data.DataTable P_tbl;   //声明一个DataTable对象
            try
            {
                S_Da = new MySqlDataAdapter(cmdtxt, GetCon());
                P_tbl = new System.Data.DataTable();
                S_Da.Fill(P_tbl);   //将表中对象放入P_tbl中
                return P_tbl;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message, "错误提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (GetCon().State == ConnectionState.Open)
                {
                    GetCon().Close();
                    GetCon().Dispose();
                }
            }
        }
        #endregion

        #region   返回MySqlDataReader类型数据

        /// <summary>
        /// 返回SqlDataReader类型数据
        /// </summary>
        /// <param name="cmdtxt">要执行的SQL语句</param>
        /// <returns></returns>
        public MySqlDataReader GetReader(string cmdtxt)
        {
            S_Com = new MySqlCommand(cmdtxt, GetCon());  //声明SqlCommand对象
            MySqlDataReader P_Dr;
            try
            {
                P_Dr = S_Com.ExecuteReader();
                return P_Dr;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message, "错误提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (GetCon().State == ConnectionState.Open)
                {
                    GetCon().Close();
                    GetCon().Dispose();
                }
            }
        }
        #endregion
    }
}