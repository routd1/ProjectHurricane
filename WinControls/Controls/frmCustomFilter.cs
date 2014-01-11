using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ICTEAS.WinForms.Controls
{
    public partial class frmCustomFilter : Form
    {
        

        #region Variables
        string strColumnName;
        #endregion

        #region Controls
        private System.ComponentModel.IContainer components = null;
        private GroupBox grpMain;
        private RadioButton rdbAnd;
        private ComboBox cmbCondition2;
        private ComboBox cmbCondition1;
        private ComboBox cmbWhere2;
        private ComboBox cmbWhere1;
        private RadioButton rdbOr;
        private Button btnOK;
        private Button btnCancel;
        private Label labelHeader;
        #endregion

        #region Declarations
        DataGridView _dataGridView;
        IBindingListView data;
        private Label label1;
        DataTable _dTable = new DataTable();
        #endregion

        #region Overidden From Object
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpMain = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rdbOr = new System.Windows.Forms.RadioButton();
            this.rdbAnd = new System.Windows.Forms.RadioButton();
            this.cmbCondition2 = new System.Windows.Forms.ComboBox();
            this.cmbCondition1 = new System.Windows.Forms.ComboBox();
            this.cmbWhere2 = new System.Windows.Forms.ComboBox();
            this.cmbWhere1 = new System.Windows.Forms.ComboBox();
            this.labelHeader = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMain
            // 
            this.grpMain.Controls.Add(this.label1);
            this.grpMain.Controls.Add(this.rdbOr);
            this.grpMain.Controls.Add(this.rdbAnd);
            this.grpMain.Controls.Add(this.cmbCondition2);
            this.grpMain.Controls.Add(this.cmbCondition1);
            this.grpMain.Controls.Add(this.cmbWhere2);
            this.grpMain.Controls.Add(this.cmbWhere1);
            this.grpMain.Location = new System.Drawing.Point(0, 25);
            this.grpMain.Name = "grpMain";
            this.grpMain.Size = new System.Drawing.Size(448, 115);
            this.grpMain.TabIndex = 0;
            this.grpMain.TabStop = false;
            this.grpMain.Text = "Column Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Use % to represent any series of characters";
            // 
            // rdbOr
            // 
            this.rdbOr.AutoSize = true;
            this.rdbOr.Location = new System.Drawing.Point(81, 47);
            this.rdbOr.Name = "rdbOr";
            this.rdbOr.Size = new System.Drawing.Size(37, 17);
            this.rdbOr.TabIndex = 5;
            this.rdbOr.TabStop = true;
            this.rdbOr.Text = "&Or";
            this.rdbOr.UseVisualStyleBackColor = true;
            // 
            // rdbAnd
            // 
            this.rdbAnd.AutoSize = true;
            this.rdbAnd.Location = new System.Drawing.Point(12, 47);
            this.rdbAnd.Name = "rdbAnd";
            this.rdbAnd.Size = new System.Drawing.Size(44, 17);
            this.rdbAnd.TabIndex = 4;
            this.rdbAnd.TabStop = true;
            this.rdbAnd.Text = "&And";
            this.rdbAnd.UseVisualStyleBackColor = true;
            // 
            // cmbCondition2
            // 
            this.cmbCondition2.FormattingEnabled = true;
            this.cmbCondition2.Location = new System.Drawing.Point(216, 69);
            this.cmbCondition2.Name = "cmbCondition2";
            this.cmbCondition2.Size = new System.Drawing.Size(210, 21);
            this.cmbCondition2.TabIndex = 3;
            // 
            // cmbCondition1
            // 
            this.cmbCondition1.FormattingEnabled = true;
            this.cmbCondition1.Location = new System.Drawing.Point(216, 20);
            this.cmbCondition1.Name = "cmbCondition1";
            this.cmbCondition1.Size = new System.Drawing.Size(210, 21);
            this.cmbCondition1.TabIndex = 2;
            // 
            // cmbWhere2
            // 
            this.cmbWhere2.FormattingEnabled = true;
            this.cmbWhere2.Items.AddRange(new object[] {
            "",
            "equals",
            "does not equal",
            "is greater than",
            "is greater than or equals to",
            "is less than",
            "LIKE"});
            this.cmbWhere2.Location = new System.Drawing.Point(12, 70);
            this.cmbWhere2.Name = "cmbWhere2";
            this.cmbWhere2.Size = new System.Drawing.Size(155, 21);
            this.cmbWhere2.TabIndex = 1;
            // 
            // cmbWhere1
            // 
            this.cmbWhere1.FormattingEnabled = true;
            this.cmbWhere1.Items.AddRange(new object[] {
            "",
            "equals",
            "does not equal",
            "is greater than",
            "is greater than or equals to",
            "is less than",
            "LIKE"});
            this.cmbWhere1.Location = new System.Drawing.Point(12, 20);
            this.cmbWhere1.Name = "cmbWhere1";
            this.cmbWhere1.Size = new System.Drawing.Size(155, 21);
            this.cmbWhere1.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Location = new System.Drawing.Point(9, 9);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(99, 13);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Show rows where :";
            // 
            // btnOK
            // 
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(260, 145);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(351, 145);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmCustomFilter
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(448, 170);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.grpMain);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmCustomFilter";
            this.Text = "Custom Filter";
            this.Load += new System.EventHandler(this.frmCustomFilter_Load);
            this.grpMain.ResumeLayout(false);
            this.grpMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructor
        public frmCustomFilter(string ColumnName,DataGridView dgView)
        {
            InitializeComponent();
            strColumnName = ColumnName;
            _dataGridView = dgView;

            // Cast the data source to an IBindingListView.
            data =_dataGridView.DataSource as IBindingListView;
            BindingSource s =(BindingSource)_dataGridView.DataSource;
            _dTable =(DataTable)s.DataSource;
            LoadComboBox();

        }


        private void LoadComboBox()
        {
            ArrayList _list = new ArrayList();
            for (int i = 0; i < _dTable.Columns.Count; i++)
            {
                if (_dTable.Columns[i].ColumnName.ToString() == strColumnName)
                {
                    for (int j = 0; j < _dTable.Rows.Count; j++)
                    {
                        
                        string tmp = _dTable.Rows[j][i].ToString();
                        
                        if (tmp == "")
                            continue;
                        if (_list.Contains(tmp))
                            continue;
                        cmbCondition1.Items.Add(tmp);
                        cmbCondition2.Items.Add(tmp);
                        _list.Add(tmp);
                    }
                }
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            grpMain.Text = strColumnName;
            base.OnLoad(e);
        }
        #endregion

        private void GetFilter_1()
        {
 
        }

        private string strFilter;
        private void btnOK_Click(object sender, EventArgs e)
        {
            string _condition_1="";
            string _condition_2="";

            string _opearator="";

            string _where_1="";
            string _where_2="";

            if (cmbWhere1.SelectedIndex == 1)
            {
                _where_1 = "=";
            }
            else if (cmbWhere1.SelectedIndex == 2)
            {
                _where_1 = "<>";
            }
            else if (cmbWhere1.SelectedIndex == 3)
            {
                _where_1 = ">";
            }
            else if (cmbWhere1.SelectedIndex == 4)
            {
                _where_1 = ">=";
            }
            else if (cmbWhere1.SelectedIndex == 5)
            {
                _where_1 = "<";
            }
            else if (cmbWhere1.SelectedIndex == 6)
            {
                _where_1 = "LIKE";
            }


            if (cmbWhere2.SelectedIndex == 1)
            {
                _where_2 = "=";
            }
            else if (cmbWhere2.SelectedIndex == 2)
            {
                _where_2 = "<>";
            }
            else if (cmbWhere2.SelectedIndex == 3)
            {
                _where_2 = ">";
            }
            else if (cmbWhere2.SelectedIndex == 4)
            {
                _where_2 = ">=";
            }
            else if (cmbWhere2.SelectedIndex == 5)
            {
                _where_2 = "<";
            }
            else if (cmbWhere2.SelectedIndex == 6)
            {
                _where_2 = "LIKE";
            }

            if (cmbCondition1.SelectedIndex < 0)
                _condition_1 = cmbCondition1.Text.Trim();
            else
                _condition_1 = (String)cmbCondition1.SelectedItem;

            if (cmbCondition2.SelectedIndex < 0)
                _condition_2 = cmbCondition2.Text.Trim();
            else
                _condition_2 = (String)cmbCondition2.SelectedItem;

            strFilter = String.Format("[{0}]{2}'{1}'",
                        strColumnName,
                        _condition_1,_where_1);


            if (rdbAnd.Checked)
                _opearator = " AND ";
            else if (rdbOr.Checked)
                _opearator += " OR ";
            if (_opearator != "")
                strFilter += _opearator;
            if (_opearator != "")
            {
                strFilter += String.Format("[{0}]{2}'{1}'",
                            strColumnName,
                            _condition_2, _where_2);
                this.Close();
            }
            else
            {
                if ((_condition_2 != "") && (_condition_2 != ""))
                {
                    MessageBox.Show("Plese select a joining condition (And or Or).", "Custom AutoFilter", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.Close();
                }

            }
            

        }

        public string Filter
        {
            get { return strFilter; }
        }

        private void frmCustomFilter_Load(object sender, EventArgs e)
        {

        }

    }
}