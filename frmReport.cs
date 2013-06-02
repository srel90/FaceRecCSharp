using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;
using System.Data.OleDb;
using System.Data;
using System.Configuration;

namespace MultiFaceRec
{
    public partial class frmReport : Form
    {
        static string ConnStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        OleDbConnection MyConn = new OleDbConnection(ConnStr);
        public static DataSet1 dsx = new DataSet1();
        public frmReport()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (MyConn.State == ConnectionState.Open) MyConn.Close();
            MyConn.Open();
            string StrCmd = "";
            OleDbCommand Cmd = new OleDbCommand();
            StrCmd = "SELECT DISTINCT NAME,EMPID FROM InOutTimeTable where NAME like '%"+ txtSearch.Text+"%';";
            OleDbDataAdapter da = new OleDbDataAdapter(StrCmd, MyConn);
            DataSet ds = new DataSet();
            da.Fill(ds, "InOutTimeTable");
            dgv1.DataSource = ds;
            dgv1.DataMember = "InOutTimeTable";
            
            
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dgv1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount<=0) 
            {
                MessageBox.Show("Please select record first.");
                return;
            }
            string StrCmd = "";
            string EMPID = dgv1.Rows[dgv1.CurrentCell.RowIndex].Cells["dgv1EMPID"].Value.ToString();
            if (rdo1.Checked == true)
            {
                StrCmd = "select EMPID,NAME,DATETIMEIN,DATETIMEOUT from InOutTimeTable where DateDiff('d',DATETIMEIN,'" + dtp1.Value + "')=0 and EMPID=" + EMPID;
            }
            else if (rdo2.Checked == true)
            {
                StrCmd = "select EMPID,NAME,DATETIMEIN,DATETIMEOUT from InOutTimeTable where DateDiff('m',DATETIMEIN,'" + dtp1.Value + "')=0 and EMPID=" + EMPID;
            }
            OleDbDataAdapter da = new OleDbDataAdapter(StrCmd, MyConn);
            DataSet ds = new DataSet();
            da.Fill(ds, "ReportTable");
            DataTable dt = new DataTable();
            dt = ds.Tables["ReportTable"];
            
            DataRow dr;
            dsx = new DataSet1();
            foreach(DataRow _item in dt.Rows)
            {
                dr = dsx.ReportTable.NewRow();
                dr["EMPID"] = _item[0];
                dr["NAME"] = _item[1];
                dr["DATETIMEIN"] = _item[2];
                dr["DATETIMEOUT"] = _item[3];
                TimeSpan span = (DateTime.Parse(_item[3].ToString()).Subtract(DateTime.Parse(_item[2].ToString())));
                dr["HOUR"] = span.Hours.ToString() + ":" + span.Minutes.ToString();
                dsx.ReportTable.Rows.Add(dr);
            }
            frmReportViewer ReportViewerForm = new frmReportViewer();
            ReportViewerForm.Show();
            
            
           


        }
    }
}
