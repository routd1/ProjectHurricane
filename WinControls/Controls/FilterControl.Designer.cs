namespace ICTEAS.WinForms.Controls
{
    partial class FilterControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.clsSearchGrid1 = new ICTEAS.WinForms.Controls.clsSearchGrid();
            ((System.ComponentModel.ISupportInitialize)(this.clsSearchGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // clsSearchGrid1
            // 
            this.clsSearchGrid1.AlternatingBackColor = System.Drawing.Color.Empty;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Empty;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.clsSearchGrid1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.clsSearchGrid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.clsSearchGrid1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.clsSearchGrid1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(202)))), ((int)(((byte)(223)))));
            this.clsSearchGrid1.CaptionFont = null;
            this.clsSearchGrid1.CaptionText = null;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.clsSearchGrid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.clsSearchGrid1.ColumnHeadersHeight = 18;
            this.clsSearchGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.clsSearchGrid1.DefaultCellStyle = dataGridViewCellStyle3;
            this.clsSearchGrid1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.clsSearchGrid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.clsSearchGrid1.FullData = null;
            this.clsSearchGrid1.GridColor = System.Drawing.Color.CadetBlue;
            this.clsSearchGrid1.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clsSearchGrid1.HeaderForeColor = System.Drawing.SystemColors.WindowText;
            this.clsSearchGrid1.Location = new System.Drawing.Point(0, 0);
            this.clsSearchGrid1.MessageID = "";
            this.clsSearchGrid1.Name = "clsSearchGrid1";
            this.clsSearchGrid1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.clsSearchGrid1.RowHeaderWidth = 41;
            this.clsSearchGrid1.RowTemplate.Height = 16;
            this.clsSearchGrid1.Size = new System.Drawing.Size(292, 48);
            this.clsSearchGrid1.TabIndex = 0;
            // 
            // FilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(292, 48);
            this.Controls.Add(this.clsSearchGrid1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 82);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 82);
            this.Name = "FilterControl";
            this.ShowIcon = false;
            this.Text = "FlexiFilter";
            ((System.ComponentModel.ISupportInitialize)(this.clsSearchGrid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private clsSearchGrid clsSearchGrid1;
    }
}