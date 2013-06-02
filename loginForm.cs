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
    public partial class frmLogin : Form
    {
        public static Boolean admin=false;
        static string ConnStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        OleDbConnection MyConn = new OleDbConnection(ConnStr);
        public frmLogin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }
            if (MyConn.State == ConnectionState.Open) MyConn.Close();
            MyConn.Open();
            string StrCmd = "";
            OleDbCommand Cmd = new OleDbCommand();
            StrCmd = "select * from admin where USERNAME='"+txtUsername.Text+"' and PASSWORD='"+txtPassword.Text+"'";
            Cmd = new OleDbCommand(StrCmd, MyConn);
            OleDbDataReader dr = Cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                MessageBox.Show("Username or Password is invalid!");
                return;
            }
            else
            {
                this.Close();
                admin = true;
            }


        }

       
    }
}
