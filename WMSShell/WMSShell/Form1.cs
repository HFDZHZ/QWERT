using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }       private void btnGeneratePushPath_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tBSidName.Text))
            {
                MessageBox.Show("未输入卖家");
                return;
            }

            Clipboard.SetText("cd /data/apache_logs/wms_api/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + tBSidName.Text.ToString());
            MessageBox.Show("生成到剪切板");
        }

        private void btnGenerate1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("grep 'begin：' ./* | grep '" + dateTimeQPSTime.Value.ToString("yyyy-MM-dd HH:mm:ss").Substring(0, 15) + "'  | awk -F '.log:' '{print $1, $2}' | awk '{print $2,$3}' | cut -c1-16  | awk -F '.'  '{x[$1]++;} END {for(i in x) print(i \" 次数:\" x[i])}'");
            MessageBox.Show("生成到剪切板");
        }

        private void btnGenerate2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("grep 'begin：' ./* | grep '" + dateTimeQPSTime.Value.ToString("yyyy-MM-dd HH:mm:ss").Substring(0, 18) + "'  | awk -F '.log:' '{print $1, $2}' | awk '{print $2,$3}' | cut -c1-19  | awk -F '.'  '{x[$1]++;} END {for(i in x) print(i \" 次数:\" x[i])}'");
            MessageBox.Show("生成到剪切板");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tBSidName.Text))
            {
                MessageBox.Show("未输入卖家");
                return;
            }

            string sql = string.Format(@"select internal_ip, external_ip from cfg_wms_ip_sid where sid ='{0}'", tBSidName.Text.ToString());
            MySqlDataReader sidInfo = Program.S_sqlExecute.GetReader(sql);
            if (sidInfo.FieldCount <= 0)
            {
                MessageBox.Show("未录入卖家");
                return;
            }

            while (sidInfo.Read())
            {
                lblInternalIP.Text = sidInfo.GetString("internal_ip");
                lblExternalIP.Text = sidInfo.GetString("external_ip");
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnSidPushNum_Click(object sender, EventArgs e)
        {
            string startTime = dateTimeSidStart.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimeSidEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "SELECT COUNT(stl.rec_id) FROM sales_trade_log stl WHERE stl.type = 5 AND stl.created >= '" + startTime + "' AND stl.created <= '" + endTime + "';";
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }

        private void btnSidMinPushNum_Click(object sender, EventArgs e)
        {
            string startTime = dateTimeSidStart.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimeSidEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "SELECT DATE_FORMAT(stl.created,'%Y-%m-%d %H:%i') AS time, COUNT(stl.rec_id) FROM sales_trade_log stl WHERE stl.type = 5 AND stl.created >= '" + startTime + "' AND stl.created <= '" + endTime + "' GROUP BY DATE_FORMAT(stl.created,'%Y-%m-%d %H:%i') ORDER BY COUNT(stl.rec_id) DESC;";
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }

        private void btnSidSecPushNum_Click(object sender, EventArgs e)
        {
            string startTime = dateTimeSidStart.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimeSidEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "SELECT stl.created AS time, COUNT(stl.rec_id) FROM sales_trade_log stl WHERE stl.type = 5 AND stl.created >= '" + startTime + "' AND stl.created <= '" + endTime + "' GROUP BY stl.created ORDER BY COUNT(stl.rec_id) DESC;";
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }

        private void btnSidOwnerPushNum_Click(object sender, EventArgs e)
        {
            string startTime = dateTimeSidStart.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimeSidEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "SELECT sys.owner_no, st.owner_id, COUNT(stl.rec_id) FROM sales_trade_log stl" +
                " LEFT JOIN sales_trade st ON st.trade_id = stl.trade_id" +
                " LEFT JOIN sys_goods_owner sgo ON sgo.owner_id = st.owner_id " +
                " WHERE stl.type = 5 AND stl.created >= '" + startTime + "' AND stl.created <= '" + endTime + "' GROUP BY st.owner_id ORDER BY COUNT(stl.rec_id) DESC;";
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }

        private void btnSidOwnerMinPushNum_Click(object sender, EventArgs e)
        {
            string startTime = dateTimeSidStart.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimeSidEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "SELECT sys.owner_no, st.owner_id, date_format(stl.created,'%Y-%m-%d %H:%i') AS time, COUNT(stl.rec_id) FROM sales_trade_log stl" +
                " LEFT JOIN sales_trade st ON st.trade_id = stl.trade_id" +
                " LEFT JOIN sys_goods_owner sgo ON sgo.owner_id = st.owner_id " +
                " WHERE stl.type = 5 AND stl.created >= '" + startTime + "' AND stl.created <= '" + endTime + "' GROUP BY date_format(stl.created,'%Y-%m-%d %H:%i'), st.owner_id ORDER BY COUNT(stl.rec_id) DESC;";
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }

        private void btnSidOwnerSecPushNum_Click(object sender, EventArgs e)
        {
            string startTime = dateTimeSidStart.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimeSidEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "SELECT sys.owner_no, st.owner_id, stl.created AS time, COUNT(stl.rec_id) FROM sales_trade_log stl" +
                " LEFT JOIN sales_trade st ON st.trade_id = stl.trade_id" +
                " LEFT JOIN sys_goods_owner sgo ON sgo.owner_id = st.owner_id " +
                " WHERE stl.type = 5 AND stl.created >= '" + startTime + "' AND stl.created <= '" + endTime + "' GROUP BY stl.created, st.owner_id ORDER BY COUNT(stl.rec_id) DESC;";
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }

        private void btnLogisticsTrack_Click(object sender, EventArgs e)
        {
            string startTime = dateTimeSidStart.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimeSidEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "SELECT COUNT(rec_id) FROM logistics_track_detail WHERE created >= '" + startTime + "' AND created <= '" + endTime + "';";
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }

        private void btnMinLogisticsTrack_Click(object sender, EventArgs e)
        {
            string startTime = dateTimeSidStart.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimeSidEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "SELECT date_format(created,'%Y-%m-%d %H:%i') AS time, COUNT(rec_id) FROM logistics_track_detail WHERE created >= '" + startTime + "' AND created <= '" + endTime + "' GROUP BY date_format(created,'%Y-%m-%d %H:%i') ORDER BT COUNT(rec_id) DESC;";
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }

        private void btnSecLogisticsTrack_Click(object sender, EventArgs e)
        {
            string startTime = dateTimeSidStart.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimeSidEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "SELECT created AS time, COUNT(rec_id) FROM logistics_track_detail WHERE created >= '" + startTime + "' AND created <= '" + endTime + "' GROUP BY created ORDER BT COUNT(rec_id) DESC;";
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }

        private void btnSidTotalPushNum_Click(object sender, EventArgs e)
        {

            if (!rBSidCurrent.Checked && !rBSidThree.Checked && !rBSidFive.Checked && !rBSidSeven.Checked)
            {
                MessageBox.Show("未选择统计天数");
                return;
            }

            int interval = rBSidCurrent.Checked ? 1 : 0;
            interval = interval == 0 ? (rBSidThree.Checked ? 3 : 0) : interval;
            interval = interval == 0 ? (rBSidFive.Checked ? 5 : 0) : interval;
            interval = interval == 0 ? (rBSidSeven.Checked ? 7 : 0) : interval;
            string startTime = DateTime.Now.AddDays(0 - interval + 1).ToString("yyyy-MM-dd");
            string endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string sql = "SELECT COUNT(stl.rec_id) FROM sales_trade_log stl WHERE stl.type = 5 AND stl.created >= '" + startTime + "' AND stl.created <= '" + endTime + "';";
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }

        private void btnSidAveragePushNum_Click(object sender, EventArgs e)
        {
            if (!rBSidCurrent.Checked && !rBSidThree.Checked && !rBSidFive.Checked && !rBSidSeven.Checked)
            {
                MessageBox.Show("未选择统计天数");
                return;
            }

            int interval = rBSidCurrent.Checked ? 1 : 0;
            interval = interval == 0 ? (rBSidThree.Checked ? 3 : 0) : interval;
            interval = interval == 0 ? (rBSidFive.Checked ? 5 : 0) : interval;
            interval = interval == 0 ? (rBSidSeven.Checked ? 7 : 0) : interval;
            string startTime = DateTime.Now.AddDays(0 - interval + 1).ToString("yyyy-MM-dd") + " 00:00:00";
            string endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00";
            string sql = "SELECT DATE_FORMAT(stl.created, '%Y-%m-%d') AS time, COUNT(stl.rec_id) FROM sales_trade_log stl WHERE stl.type = 5 AND stl.created >= '" + startTime + "' AND stl.created <= '" + endTime + "' GROUP BY DATE_FORMAT(stl.created, '%Y-%m-%d') ORDER BY COUNT(stl.rec_id) DESC;";
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tBIP.Text))
            {
                MessageBox.Show("未输入内/外网IP");
                return;
            }

            string sql = string.Format(@"select count(1) AS sid_num from cfg_wms_ip_sid where internal_ip = '{0}' OR external_ip = '{1}';", tBIP.Text.ToString(), tBIP.Text.ToString());
            MySqlDataReader sidCount = Program.S_sqlExecute.GetReader(sql);
            if (sidCount.FieldCount <= 0)
            {
                MessageBox.Show("未输入内/外网IP");
                return;
            }

            while (sidCount.Read())
            {
                lblSidNum.Text = sidCount.GetString("sid_num");
            }
        }

        private void btnIPPushNum_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tBIP.Text))
            {
                MessageBox.Show("未输入内/外网IP");
                return;
            }

            List<string> sidList = new List<string>();
            string startTime = dateTimeIPStart.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimeIPEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");

            string getSid = string.Format(@"SELECT sid FROM cfg_wms_ip_sid where internal_ip = '{0}' OR external_ip = '{1}'", tBIP.Text.ToString(), tBIP.Text.ToString());
            DataRowCollection sidCollection = Program.S_sqlExecute.GetTableRows(getSid);

            foreach (DataRow sidRow in sidCollection)
            {
                sidList.Add(sidRow["sid"].ToString());
            }

            string sql = "";
            sidList.ForEach(sid => sql += ("SELECT 'd_" + sid + "_wms' AS sid, COUNT(stl.rec_id) as sid_order_num FROM d_" + sid + "_wms.sales_trade_log stl WHERE type = 5 AND stl.created >= '" + startTime + "' AND stl.created <= '" + endTime + "' UNION "));
            int length = sql.Length;
            string tmpSql = sql.Substring(0, length - 6);

            sql = "SELECT SUM(sid_order_num) FROM SELECT (" + tmpSql + ") tmp";
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }

        private void btnIPSidPushNum_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tBIP.Text))
            {
                MessageBox.Show("未输入内/外网IP");
                return;
            }

            List<string> sidList = new List<string>();
            string startTime = dateTimeIPStart.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimeIPEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");

            string getSid = string.Format(@"SELECT sid FROM cfg_wms_ip_sid where internal_ip = '{0}' OR external_ip = '{1}'", tBIP.Text.ToString(), tBIP.Text.ToString());
            DataRowCollection sidCollection = Program.S_sqlExecute.GetTableRows(getSid);

            foreach (DataRow sidRow in sidCollection)
            {
                sidList.Add(sidRow["sid"].ToString());
            }

            string sql = "";
            sidList.ForEach(sid => sql += ("SELECT 'd_" + sid + "_wms' AS sid, COUNT(stl.rec_id) as sid_order_num FROM d_" + sid + "_wms.sales_trade_log stl WHERE type = 5 AND stl.created >= '" + startTime + "' AND stl.created <= '" + endTime + "' UNION "));
            int length = sql.Length;
            sql.Substring(0, length - 6);
            Clipboard.SetText(sql);
            MessageBox.Show("生成到剪切板");
        }
    }
}
