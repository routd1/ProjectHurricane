namespace ICTEAS.WinForms.Controls
{
    partial class clsWritableGrid
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
            try
            {
                if (disposing && (components != null))
                {
                    if (components.Components.Count > 0)
                    {
                        components.Dispose();
                    }
                }
                base.Dispose(disposing);
            }
            catch
            {
 
            }
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.clsROGContextmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.xport = new System.Windows.Forms.ToolStripMenuItem();
            this.Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.withheader = new System.Windows.Forms.ToolStripMenuItem();
            this.withoutheader = new System.Windows.Forms.ToolStripMenuItem();
            this.paste = new System.Windows.Forms.ToolStripMenuItem();
            this.fill = new System.Windows.Forms.ToolStripMenuItem();
            this.Filter = new System.Windows.Forms.ToolStripMenuItem();
            this.Delete = new System.Windows.Forms.ToolStripMenuItem();
            RowsMenu = new  System.Windows.Forms.ToolStripMenuItem();
            AddRow = new System.Windows.Forms.ToolStripMenuItem();
            RemoveRow = new System.Windows.Forms.ToolStripMenuItem();
            this.clsROGContextmenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // clsROGContextmenu
            // 
            this.clsROGContextmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xport,
            this.Copy,
            this.paste,
            this.fill,
            //this.Filter,
            this.Delete,
            this.AddRow,
            this.RemoveRow});
            this.clsROGContextmenu.Name = "contextMenuStrip1";
            this.clsROGContextmenu.ShowCheckMargin = true;
            this.clsROGContextmenu.Size = new System.Drawing.Size(166, 136);
            this.clsROGContextmenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.item_clicked);
            // 
            // xport
            // 
            this.xport.Name = "xport";
            this.xport.Size = new System.Drawing.Size(165, 22);
            this.xport.Text = "Export Data";
            // 
            // Copy
            // 
            this.Copy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.withheader,
            this.withoutheader});
            this.Copy.Name = "Copy";
            this.Copy.Size = new System.Drawing.Size(165, 22);
            this.Copy.Text = "Copy";
            // 
            // withheader
            // 
            this.withheader.Name = "withheader";
            this.withheader.Size = new System.Drawing.Size(183, 22);
            this.withheader.Text = "With Column Header";
            // 
            // withoutheader
            // 
            this.withoutheader.Name = "withoutheader";
            this.withoutheader.Size = new System.Drawing.Size(183, 22);
            this.withoutheader.Text = "Data Only";
            // 
            // paste
            // 
            this.paste.Name = "paste";
            this.paste.Size = new System.Drawing.Size(165, 22);
            this.paste.Text = "Paste";
            // 
            // fill
            // 
            this.fill.Enabled = false;
            this.fill.Name = "fill";
            this.fill.Size = new System.Drawing.Size(165, 22);
            this.fill.Text = "Fill Column";
            // 
            // Filter
            // 
            //this.Filter.Name = "Filter";
            //this.Filter.Size = new System.Drawing.Size(165, 22);
            //this.Filter.Text = "Filter";
            // 
            // Delete
            // 
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(165, 22);
            this.Delete.Text = "Delete";
            ////
            ////RowsMenu
            ////
            //this.RowsMenu.Name = "Rows";
            //this.RowsMenu.Size = new System.Drawing.Size(183, 22);
            //this.RowsMenu.Text = "Rows";
            //this.RowsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.AddRow,
            //this.RemoveRow});
            //
            //AddRow
            //
            this.AddRow.Name = "AddRow";
            this.AddRow.Size = new System.Drawing.Size(183, 22);
            this.AddRow.Text = "Add Row";
            //
            //RemoveRow
            //
            this.RemoveRow.Name = "RemoveRow";
            this.RemoveRow.Size = new System.Drawing.Size(183, 22);
            this.RemoveRow.Text = "Remove Row";
            // 
            // clsWritableGrid
            // 
            this.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.AllowUserToResizeColumns = true;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.visibility_changed);
            //this.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this._DoNothingOnDataError);
            this.clsROGContextmenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public  System.Windows.Forms.ContextMenuStrip clsROGContextmenu;
        public System.Windows.Forms.ToolStripMenuItem xport;
        public System.Windows.Forms.ToolStripMenuItem Copy;
        public System.Windows.Forms.ToolStripMenuItem paste;
        public System.Windows.Forms.ToolStripMenuItem withheader;
        public System.Windows.Forms.ToolStripMenuItem withoutheader;
        public System.Windows.Forms.ToolStripMenuItem fill;
        public System.Windows.Forms.ToolStripMenuItem Filter;
        public System.Windows.Forms.ToolStripMenuItem Delete;
        public System.Windows.Forms.ToolStripMenuItem RowsMenu;
        public System.Windows.Forms.ToolStripMenuItem AddRow;
        public System.Windows.Forms.ToolStripMenuItem RemoveRow;
    }
}
