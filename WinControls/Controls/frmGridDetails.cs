using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ICTEAS.WinForms.Controls
{

	/// <summary>
	/// Summary description for frmGridDetails.
	/// </summary>
	public class frmGridDetails :clsForm
	{
		protected System.Windows.Forms.ToolStrip tlbDetails;
		private System.Windows.Forms.ToolStripButton btnFirst;
		private System.Windows.Forms.ToolStripButton btnPrevious;
		private System.Windows.Forms.ToolStripButton btnNext;
		private System.Windows.Forms.ToolStripButton btnLast;
		private System.Windows.Forms.ToolStripButton btnAdd;
		private System.Windows.Forms.ToolStripButton btnRemove;
		private System.Windows.Forms.ToolStripButton btnClose;
		protected System.Windows.Forms.Panel pnlDetails;
		private System.Windows.Forms.ImageList imageListDetails;
		private System.ComponentModel.IContainer components = null;

		public frmGridDetails()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGridDetails));
            this.tlbDetails = new System.Windows.Forms.ToolStrip();
            this.imageListDetails = new System.Windows.Forms.ImageList(this.components);
            this.btnFirst = new System.Windows.Forms.ToolStripButton();
            this.btnPrevious = new System.Windows.Forms.ToolStripButton();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            this.btnLast = new System.Windows.Forms.ToolStripButton();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnRemove = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tlbDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlbDetails
            // 
            this.tlbDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tlbDetails.ImageList = this.imageListDetails;
            this.tlbDetails.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFirst,
            this.btnPrevious,
            this.btnNext,
            this.btnLast,
            this.btnAdd,
            this.btnRemove,
            this.btnClose});
            this.tlbDetails.Location = new System.Drawing.Point(0, 0);
            this.tlbDetails.Name = "tlbDetails";
            this.tlbDetails.Size = new System.Drawing.Size(578, 25);
            this.tlbDetails.TabIndex = 284;
            this.tlbDetails.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tlbDetails_ButtonClick);
            // 
            // imageListDetails
            // 
            this.imageListDetails.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDetails.ImageStream")));
            this.imageListDetails.TransparentColor = System.Drawing.Color.White;
            this.imageListDetails.Images.SetKeyName(0, "");
            this.imageListDetails.Images.SetKeyName(1, "");
            this.imageListDetails.Images.SetKeyName(2, "");
            this.imageListDetails.Images.SetKeyName(3, "");
            this.imageListDetails.Images.SetKeyName(4, "");
            this.imageListDetails.Images.SetKeyName(5, "");
            this.imageListDetails.Images.SetKeyName(6, "");
            // 
            // btnFirst
            // 
            this.btnFirst.ImageIndex = 0;
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(23, 22);
            this.btnFirst.ToolTipText = "First Record (Ctrl+F)";
            // 
            // btnPrevious
            // 
            this.btnPrevious.ImageIndex = 1;
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(23, 22);
            this.btnPrevious.ToolTipText = "Previous Record (Ctrl+P)";
            // 
            // btnNext
            // 
            this.btnNext.ImageIndex = 2;
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(23, 22);
            this.btnNext.ToolTipText = "Next Record (Ctrl+N)";
            // 
            // btnLast
            // 
            this.btnLast.ImageIndex = 3;
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(23, 22);
            this.btnLast.ToolTipText = "Last Record (Ctrl+L)";
            // 
            // btnAdd
            // 
            this.btnAdd.ImageIndex = 5;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 22);
            this.btnAdd.ToolTipText = "Add Record (Ctrl+A)";
            // 
            // btnRemove
            // 
            this.btnRemove.ImageIndex = 6;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(23, 22);
            this.btnRemove.ToolTipText = "Delete Record (Ctrl+D)";
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 22);
            this.btnClose.ToolTipText = "Close Screen (Ctrl+F4)";
            // 
            // pnlDetails
            // 
            this.pnlDetails.AutoScroll = true;
            this.pnlDetails.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetails.Location = new System.Drawing.Point(6, 38);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(566, 314);
            this.pnlDetails.TabIndex = 285;
            // 
            // frmGridDetails
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(578, 358);
            this.ControlBox = false;
            this.Controls.Add(this.pnlDetails);
            this.Controls.Add(this.tlbDetails);
            this.Name = "frmGridDetails";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "** ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tlbDetails.ResumeLayout(false);
            this.tlbDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		protected sealed override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(((Control.ModifierKeys & Keys.Control) | Keys.F) == keyData && keyData != Keys.F)
			{
				FirstRow();
				return true;
			}

			else if(((Control.ModifierKeys & Keys.Control) | Keys.P) == keyData && keyData != Keys.P)
			{
				PrevRow();
				return true;
			}

			else if(((Control.ModifierKeys & Keys.Control) | Keys.N) == keyData && keyData != Keys.N)
			{
				NextRow();
				return true;
			}

			else if(((Control.ModifierKeys & Keys.Control) | Keys.L) == keyData && keyData != Keys.L)
			{
				LastRow();
				return true;
			}

			else if(((Control.ModifierKeys & Keys.Control) | Keys.A) == keyData && keyData != Keys.A)
			{
				AddRow();
				return true;
			}

			else if(((Control.ModifierKeys & Keys.Control) | Keys.D) == keyData && keyData != Keys.D)
			{
				DeleteRow();
				return true;
			}			

			else if(((Control.ModifierKeys & Keys.Control) | Keys.F4) == keyData && keyData != Keys.F4)
			{
				CloseForm();
				return true;
			}

			return base.ProcessCmdKey (ref msg, keyData);
		}


		#region ButtonClick
		/// <summary>
		/// This function implements different actions on clicking of buttons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void tlbDetails_ButtonClick(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
		{
            if (e.ClickedItem == btnFirst)
            {
                // FIRST RECORD
                FirstRow();
            }
            else if (e.ClickedItem == btnPrevious)
            {
                //PREVIOUS RECORD
                PrevRow();
            }
            else if (e.ClickedItem == btnNext)
            {
                //NEXT RECORD
                NextRow();
            }
            else if (e.ClickedItem == btnLast)
            {

                //LAST RECORD
                LastRow();
            }
            else if (e.ClickedItem == btnAdd)
            {
                //ADD
                AddRow();
            }
            else if (e.ClickedItem == btnRemove)
            {
                //REMOVE
                DeleteRow();
            }
            else if (e.ClickedItem == btnClose)
            {
                //CLOSE
                CloseForm();
            }
		}
		#endregion
		
		#region VIRTUAL FUNCTIONS OVERRIDDEN IN INHERITED FORMS

		protected virtual void FirstRow()
		{
		}

		protected virtual void PrevRow()
		{
		}

		protected virtual void NextRow()
		{
		}

		protected virtual void LastRow()
		{
		}

		protected virtual void AddRow()
		{
		}

		protected virtual void DeleteRow()
		{
		}

		protected virtual void CloseForm()
		{
		}

		#endregion


		
	}
}
