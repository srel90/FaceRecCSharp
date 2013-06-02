namespace MultiFaceRec
{
    partial class frmReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.rdo1 = new System.Windows.Forms.RadioButton();
            this.rdo2 = new System.Windows.Forms.RadioButton();
            this.dtp1 = new System.Windows.Forms.DateTimePicker();
            this.btnReport = new System.Windows.Forms.Button();
            this.dgv1EMPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv1NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(90, 5);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(100, 20);
            this.txtSearch.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ค้นหาพนักงาน";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(196, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 20);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "ค้นหา";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgv1
            // 
            this.dgv1.AllowUserToAddRows = false;
            this.dgv1.AllowUserToDeleteRows = false;
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv1EMPID,
            this.dgv1NAME});
            this.dgv1.Location = new System.Drawing.Point(12, 31);
            this.dgv1.MultiSelect = false;
            this.dgv1.Name = "dgv1";
            this.dgv1.ReadOnly = true;
            this.dgv1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv1.Size = new System.Drawing.Size(274, 150);
            this.dgv1.TabIndex = 3;
            // 
            // rdo1
            // 
            this.rdo1.AutoSize = true;
            this.rdo1.Checked = true;
            this.rdo1.Location = new System.Drawing.Point(12, 217);
            this.rdo1.Name = "rdo1";
            this.rdo1.Size = new System.Drawing.Size(93, 17);
            this.rdo1.TabIndex = 4;
            this.rdo1.TabStop = true;
            this.rdo1.Text = "รายงานรายวัน";
            this.rdo1.UseVisualStyleBackColor = true;
            // 
            // rdo2
            // 
            this.rdo2.AutoSize = true;
            this.rdo2.Location = new System.Drawing.Point(12, 240);
            this.rdo2.Name = "rdo2";
            this.rdo2.Size = new System.Drawing.Size(105, 17);
            this.rdo2.TabIndex = 4;
            this.rdo2.Text = "รายงานรายเดือน";
            this.rdo2.UseVisualStyleBackColor = true;
            // 
            // dtp1
            // 
            this.dtp1.Location = new System.Drawing.Point(12, 187);
            this.dtp1.Name = "dtp1";
            this.dtp1.Size = new System.Drawing.Size(274, 20);
            this.dtp1.TabIndex = 5;
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(196, 213);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(90, 44);
            this.btnReport.TabIndex = 2;
            this.btnReport.Text = "รายงาน";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // dgv1EMPID
            // 
            this.dgv1EMPID.DataPropertyName = "EMPID";
            this.dgv1EMPID.HeaderText = "Employee ID";
            this.dgv1EMPID.Name = "dgv1EMPID";
            this.dgv1EMPID.ReadOnly = true;
            // 
            // dgv1NAME
            // 
            this.dgv1NAME.DataPropertyName = "NAME";
            this.dgv1NAME.HeaderText = "Name";
            this.dgv1NAME.Name = "dgv1NAME";
            this.dgv1NAME.ReadOnly = true;
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 271);
            this.Controls.Add(this.dtp1);
            this.Controls.Add(this.rdo2);
            this.Controls.Add(this.rdo1);
            this.Controls.Add(this.dgv1);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSearch);
            this.Name = "frmReport";
            this.Text = "Report";
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.RadioButton rdo1;
        private System.Windows.Forms.RadioButton rdo2;
        private System.Windows.Forms.DateTimePicker dtp1;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv1EMPID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv1NAME;
    }
}