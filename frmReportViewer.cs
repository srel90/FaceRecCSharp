using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class frmReportViewer : Form
    {
        public frmReportViewer()
        {
            InitializeComponent();
        }

        private void frmReportViewer_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.InOutTimeTable' table. You can move, or remove it, as needed.
            
            DataTable dt = new DataTable();
            dt = frmReport.dsx.Tables["ReportTable"];
            this.reportTableBindingSource.DataSource = dt;
            this.reportViewer1.RefreshReport();

        }
    }
}
