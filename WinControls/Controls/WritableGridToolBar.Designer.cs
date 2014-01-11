namespace ICTEAS.WinForms.Controls
{
    partial class WritableGridToolBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WritableGridToolBar));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsBtnUp = new System.Windows.Forms.ToolStripButton();
            this.tsBtnDown = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.AutoSize = false;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnUp,
            this.tsBtnDown});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStrip.Size = new System.Drawing.Size(424, 23);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            this.toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip_ItemClicked);
            // 
            // tsBtnUp
            // 
            this.tsBtnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnUp.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnUp.Image")));
            this.tsBtnUp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsBtnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnUp.Name = "tsBtnUp";
            this.tsBtnUp.Size = new System.Drawing.Size(23, 20);
            this.tsBtnUp.Text = "Shift Row Up";
            // 
            // tsBtnDown
            // 
            this.tsBtnDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnDown.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnDown.Image")));
            this.tsBtnDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsBtnDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnDown.Name = "tsBtnDown";
            this.tsBtnDown.Size = new System.Drawing.Size(23, 20);
            this.tsBtnDown.Text = "Shift Row Down";
            // 
            // WritableGridToolBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip);
            this.Name = "WritableGridToolBar";
            this.Size = new System.Drawing.Size(424, 23);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ToolStrip toolStrip;
        public System.Windows.Forms.ToolStripButton tsBtnUp;
        public System.Windows.Forms.ToolStripButton tsBtnDown;

    }
}
