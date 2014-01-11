using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ICTEAS.WinForms.Common;
using System.Globalization;
using ICTEAS.WinForms.Helpers;
using ICTEAS.WinForms.Helpers;
namespace ICTEAS.WinForms.Controls
{
	
	public class frmHotSearch : clsForm
    {
        private DataTable dthotloadtable;
        private int _CurrentLevel = 0;
		private string strErrMsg;
		private string m_Sql;
		private string m_Caption;
		private string[] storedVal;

		private DataTable dtSelected;
		private DataTable dtBeforeHotSearch;
		private int[] UK_CurrentTable;
		private int[] UK_HotSearch;
		private int[] ReplacedCols;
		private int[] ReplacingCols;
        public bool controldown = false;
		
		private bool boolCancel = false;
		
		private DateTime gridMouseDownTime;
		
		private string[] strLangID;
	
		private int intdgRecordsRows;

		private bool boolMultiLine = false;

		private bool boolLevelSearch = false;

		private bool boolAllowOKBtn = false;

		private bool boolAddCombine = false;

		private bool boolReturnOnlyTable = false;

		//private bool boolRestrictTo999Rows = clsCommonInfo.RestrictTo999Rows;
        private bool boolRestrictTo999Rows = true;
        int currentrow;
        private IContainer components;

        private ArrayList rowsselected;
        private Panel panelMultiBtns;
        private clsCheckBox chkCombined;
        private Panel panelStdBtns;
        private clsButton btnCancel;
        private clsButton btnOK;
        private Panel panelLevel;
        private Label lblMax;
        private Label lblSep;
        private Label lblNo;
        private Label lblcomments;
        private GradientPanel gradientPanel1;
        private Button btnAdd;
        private Button btnRemoveAll;
        private Button btnRemove;
        private Button btnAddAll;
        private ReadnSearchGrid dgRecords;
        private ReadnSearchGrid dgSelectedRecords;
        private GroupBox grpData;
	
		private int _SortByCol = 0;

		public int SortColumn
		{
			get{return _SortByCol;}
			set
			{
				_SortByCol = value;
				dgRecords.DefaultSort = true;
				dgRecords.SortColumn = _SortByCol;
				dgSelectedRecords.DefaultSort = true;
				dgSelectedRecords.SortColumn = _SortByCol;
			}
		}


		public bool Cancelled
		{
			get { return boolCancel; }
		}


		public bool RestrictTo999Rows
		{
			set{ boolRestrictTo999Rows = value; }
		}


		public string HierarchyLevel
		{
			get{return dgRecords.LevelHierarchy;}
		}


		public string HierarchyValue
		{
			get{return dgRecords.ValueHierarchy;}
		}


		public int HierarchyLevelColumn
		{
			get{return dgRecords.LevelHierarchyColumn1;}
			set{dgRecords.LevelHierarchyColumn1 = value;}
		}


		public bool AllowOKBtn
		{
			get{return boolAllowOKBtn;}
			set{boolAllowOKBtn = value;}
		}


		public bool ReturnOnlyTable
		{
			get{return boolReturnOnlyTable;}
			set{boolReturnOnlyTable = value;}
		}


		public DataTable MultiSelectData
		{
			get
			{
				if( boolCancel == false )
					return dtSelected;
				else
					return dtSelected.Clone();
			}
		}


		public DataTable DataAfterHotSearch
		{
			get
			{
				if( dtBeforeHotSearch == null )
					return null;

				System.Data.DataTable dtModified = new DataTable();
				try
				{
					dtModified.Dispose();
                    clsSqlOperations objJoin = new clsSqlOperations();
                    dtModified = objJoin.Union(dtBeforeHotSearch, this.MultiSelectData, UK_CurrentTable, UK_HotSearch, ReplacedCols, ReplacingCols);
                    return dtModified;
				}
				catch
				{
					return null;
				}
				finally
				{
					if( dtModified != null )
						dtModified.Dispose();
				}
			}
		}


		public bool ShowCombined
		{
			get{return boolAddCombine;}
			set
			{
				boolAddCombine = value;
				if(value && boolMultiLine)
					chkCombined.Visible = true;
				else
					chkCombined.Visible = false;
			}
		}


		public string CombinedLangID
		{
			get{return chkCombined.MessageID;}
			set{chkCombined.MessageID = value;}
		}


		public int HierarchyValueColumn
		{
			get{return dgRecords.LevelHierarchyColumn2;}
			set{dgRecords.LevelHierarchyColumn2 = value;}
		}


		public void SetDataBeforeHotSearch(DataTable dtBeforeHotSearch, int[] UK_CurrentTable, int[] UK_HotSearch, int[] ReplacedCols, int[] ReplacingCols )
		{
			this.dtBeforeHotSearch = dtBeforeHotSearch;
			this.UK_CurrentTable = UK_CurrentTable;
			this.UK_HotSearch = UK_HotSearch;
			this.ReplacedCols = ReplacedCols;
			this.ReplacingCols = ReplacingCols;
		}


        public frmHotSearch(string strSearchSql)
		{
			InitializeComponent();
			m_Sql=strSearchSql;
			m_Caption="Search";
			base.MessageID = clsCommonInfo.MsgIDHotSearchCaption;
			btnOK.MessageID = clsCommonInfo.MsgIDHotSearchOkButton;
			btnCancel.MessageID = clsCommonInfo.MsgIDHotSearchCancelButton;
		}


		public frmHotSearch(string strSearchSql,string strCaption)
		{
			InitializeComponent();
			m_Sql=strSearchSql;
			m_Caption=strCaption;
			base.MessageID = clsCommonInfo.MsgIDHotSearchCaption;
			btnOK.MessageID = clsCommonInfo.MsgIDHotSearchOkButton;
			btnCancel.MessageID = clsCommonInfo.MsgIDHotSearchCancelButton;
		}
	

		public frmHotSearch(string strSearchSql,string msgIDCaption,string msgIDOKButton,string msgIDCancelButton)
		{
			InitializeComponent();
			m_Sql=strSearchSql;
			
			base.MessageID = msgIDCaption;
			btnOK.MessageID = msgIDOKButton;
			btnCancel.MessageID = msgIDCancelButton;
		}	


		

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHotSearch));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelMultiBtns = new System.Windows.Forms.Panel();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.chkCombined = new ICTEAS.WinForms.Controls.clsCheckBox();
            this.panelStdBtns = new System.Windows.Forms.Panel();
            this.btnCancel = new ICTEAS.WinForms.Controls.clsButton();
            this.btnOK = new ICTEAS.WinForms.Controls.clsButton();
            this.panelLevel = new System.Windows.Forms.Panel();
            this.lblMax = new System.Windows.Forms.Label();
            this.lblSep = new System.Windows.Forms.Label();
            this.lblNo = new System.Windows.Forms.Label();
            this.lblcomments = new System.Windows.Forms.Label();
            this.gradientPanel1 = new ICTEAS.WinForms.Helpers.GradientPanel(this.components);
            this.grpData = new System.Windows.Forms.GroupBox();
            this.dgRecords = new ICTEAS.WinForms.Controls.ReadnSearchGrid();
            this.dgSelectedRecords = new ICTEAS.WinForms.Controls.ReadnSearchGrid();
            this.panelMultiBtns.SuspendLayout();
            this.panelStdBtns.SuspendLayout();
            this.panelLevel.SuspendLayout();
            this.gradientPanel1.SuspendLayout();
            this.grpData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRecords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSelectedRecords)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMultiBtns
            // 
            this.panelMultiBtns.BackColor = System.Drawing.Color.Transparent;
            this.panelMultiBtns.Controls.Add(this.btnRemoveAll);
            this.panelMultiBtns.Controls.Add(this.btnRemove);
            this.panelMultiBtns.Controls.Add(this.btnAddAll);
            this.panelMultiBtns.Controls.Add(this.btnAdd);
            this.panelMultiBtns.Location = new System.Drawing.Point(331, 55);
            this.panelMultiBtns.Name = "panelMultiBtns";
            this.panelMultiBtns.Size = new System.Drawing.Size(36, 198);
            this.panelMultiBtns.TabIndex = 11;
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnRemoveAll.FlatAppearance.BorderSize = 0;
            this.btnRemoveAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveAll.Location = new System.Drawing.Point(3, 126);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(30, 20);
            this.btnRemoveAll.TabIndex = 23;
            this.btnRemoveAll.Text = "<<";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.SystemColors.Control;
            this.btnRemove.FlatAppearance.BorderSize = 0;
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Location = new System.Drawing.Point(3, 103);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(30, 20);
            this.btnRemove.TabIndex = 23;
            this.btnRemove.Text = "<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddAll.FlatAppearance.BorderSize = 0;
            this.btnAddAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddAll.Location = new System.Drawing.Point(3, 45);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(30, 20);
            this.btnAddAll.TabIndex = 22;
            this.btnAddAll.Text = ">>";
            this.btnAddAll.UseVisualStyleBackColor = true;
            this.btnAddAll.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(3, 22);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(30, 20);
            this.btnAdd.TabIndex = 21;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click_1);
            // 
            // chkCombined
            // 
            this.chkCombined.BackColor = System.Drawing.Color.Transparent;
            this.chkCombined.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCombined.Location = new System.Drawing.Point(293, 5);
            this.chkCombined.MessageID = "";
            this.chkCombined.Name = "chkCombined";
            this.chkCombined.ResourceName = "";
            this.chkCombined.Size = new System.Drawing.Size(186, 21);
            this.chkCombined.TabIndex = 17;
            this.chkCombined.UseVisualStyleBackColor = false;
            this.chkCombined.Visible = false;
            this.chkCombined.CheckedChanged += new System.EventHandler(this.chkCombined_CheckedChanged);
            // 
            // panelStdBtns
            // 
            this.panelStdBtns.BackColor = System.Drawing.Color.Transparent;
            this.panelStdBtns.Controls.Add(this.btnCancel);
            this.panelStdBtns.Controls.Add(this.btnOK);
            this.panelStdBtns.Location = new System.Drawing.Point(296, 302);
            this.panelStdBtns.Name = "panelStdBtns";
            this.panelStdBtns.Size = new System.Drawing.Size(126, 28);
            this.panelStdBtns.TabIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(65, 2);
            this.btnCancel.MessageID = "";
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ResourceName = "";
            this.btnCancel.Size = new System.Drawing.Size(59, 22);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(3, 2);
            this.btnOK.MessageID = "";
            this.btnOK.Name = "btnOK";
            this.btnOK.ResourceName = "";
            this.btnOK.Size = new System.Drawing.Size(59, 22);
            this.btnOK.TabIndex = 7;
            this.btnOK.TabStop = false;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panelLevel
            // 
            this.panelLevel.BackColor = System.Drawing.Color.Transparent;
            this.panelLevel.Controls.Add(this.lblMax);
            this.panelLevel.Controls.Add(this.lblSep);
            this.panelLevel.Controls.Add(this.lblNo);
            this.panelLevel.Location = new System.Drawing.Point(11, 302);
            this.panelLevel.Name = "panelLevel";
            this.panelLevel.Size = new System.Drawing.Size(44, 20);
            this.panelLevel.TabIndex = 16;
            this.panelLevel.Visible = false;
            this.panelLevel.Paint += new System.Windows.Forms.PaintEventHandler(this.panelLevel_Paint);
            // 
            // lblMax
            // 
            this.lblMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMax.Location = new System.Drawing.Point(28, 3);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(14, 14);
            this.lblMax.TabIndex = 18;
            this.lblMax.Text = "0";
            // 
            // lblSep
            // 
            this.lblSep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSep.Location = new System.Drawing.Point(15, 3);
            this.lblSep.Name = "lblSep";
            this.lblSep.Size = new System.Drawing.Size(14, 14);
            this.lblSep.TabIndex = 17;
            this.lblSep.Text = "/";
            // 
            // lblNo
            // 
            this.lblNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNo.Location = new System.Drawing.Point(3, 3);
            this.lblNo.Name = "lblNo";
            this.lblNo.Size = new System.Drawing.Size(14, 14);
            this.lblNo.TabIndex = 16;
            this.lblNo.Text = "0";
            // 
            // lblcomments
            // 
            this.lblcomments.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblcomments.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcomments.Location = new System.Drawing.Point(266, 16);
            this.lblcomments.Name = "lblcomments";
            this.lblcomments.Size = new System.Drawing.Size(10, 24);
            this.lblcomments.TabIndex = 19;
            this.lblcomments.Visible = false;
            this.lblcomments.Click += new System.EventHandler(this.lblcomments_Click);
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel1.BackgroundImage")));
            this.gradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gradientPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientPanel1.Controls.Add(this.grpData);
            this.gradientPanel1.Controls.Add(this.panelStdBtns);
            this.gradientPanel1.Controls.Add(this.panelLevel);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientPanel1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.PageEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(147)))), ((int)(((byte)(191)))));
            this.gradientPanel1.PageStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(224)))), ((int)(((byte)(248)))));
            this.gradientPanel1.Size = new System.Drawing.Size(704, 335);
            this.gradientPanel1.TabIndex = 20;
            // 
            // grpData
            // 
            this.grpData.BackColor = System.Drawing.Color.Transparent;
            this.grpData.Controls.Add(this.dgRecords);
            this.grpData.Controls.Add(this.lblcomments);
            this.grpData.Controls.Add(this.dgSelectedRecords);
            this.grpData.Controls.Add(this.panelMultiBtns);
            this.grpData.Controls.Add(this.chkCombined);
            this.grpData.Location = new System.Drawing.Point(3, 0);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(696, 298);
            this.grpData.TabIndex = 23;
            this.grpData.TabStop = false;
            // 
            // dgRecords
            // 
            this.dgRecords.AllowUserToDeleteRows = false;
            this.dgRecords.AllowUserToResizeRows = false;
            this.dgRecords.AlternatingBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.dgRecords.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgRecords.AssociatedSearchGrid = null;
            this.dgRecords.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgRecords.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            this.dgRecords.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgRecords.CaptionFont = null;
            this.dgRecords.CaptionText = null;
            this.dgRecords.CaptionVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgRecords.ContextMenuVisible = false;
            this.dgRecords.CstmAlternateRowColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.dgRecords.CstmAlternateRowColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(220)))), ((int)(((byte)(234)))));
            this.dgRecords.CstmCellsColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.dgRecords.CstmEnableSelectedCellColors = true;
            this.dgRecords.CstmNormalRowColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgRecords.CstmNormalRowColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgRecords.CstmRowsColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.dgRecords.CstmSelectedCellColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgRecords.CstmSelectedCellColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(213)))), ((int)(((byte)(103)))));
            this.dgRecords.CstmSelectedRowColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(228)))), ((int)(((byte)(145)))));
            this.dgRecords.CstmSelectedRowColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(213)))), ((int)(((byte)(103)))));
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgRecords.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgRecords.DefaultSort = false;
            this.dgRecords.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgRecords.EnableDataFiltering = true;
            this.dgRecords.GridColor = System.Drawing.Color.CadetBlue;
            this.dgRecords.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgRecords.HeaderForeColor = System.Drawing.SystemColors.WindowText;
            this.dgRecords.HotSearchChild = true;
            this.dgRecords.LangIDs = new string[0];
            this.dgRecords.LevelHierarchyColumn1 = -1;
            this.dgRecords.LevelHierarchyColumn2 = -1;
            this.dgRecords.LevelSearch = false;
            this.dgRecords.Location = new System.Drawing.Point(8, 11);
            this.dgRecords.MessageID = "";
            this.dgRecords.MultiLine = false;
            this.dgRecords.Name = "dgRecords";
            this.dgRecords.ReadOnly = true;
            this.dgRecords.RowHeadersWidth = 15;
            this.dgRecords.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgRecords.RowHeaderWidth = 15;
            this.dgRecords.RowTemplate.Height = 16;
            this.dgRecords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgRecords.Size = new System.Drawing.Size(323, 281);
            this.dgRecords.SortColumn = 0;
            this.dgRecords.TabIndex = 21;
            this.dgRecords.VirtualMode = true;
            this.dgRecords.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgRecords_MouseDown_1);
            this.dgRecords.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgRecords_CellDoubleClick);
            this.dgRecords.DoubleClick += new System.EventHandler(this.dgRecords_DoubleClick);
            this.dgRecords.DataSourceChanged += new System.EventHandler(this.dgRecords_DataSourceChanged);
            // 
            // dgSelectedRecords
            // 
            this.dgSelectedRecords.AllowUserToDeleteRows = false;
            this.dgSelectedRecords.AllowUserToResizeRows = false;
            this.dgSelectedRecords.AlternatingBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this.dgSelectedRecords.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgSelectedRecords.AssociatedSearchGrid = null;
            this.dgSelectedRecords.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgSelectedRecords.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            this.dgSelectedRecords.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgSelectedRecords.CaptionFont = null;
            this.dgSelectedRecords.CaptionText = null;
            this.dgSelectedRecords.CaptionVisible = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgSelectedRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgSelectedRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSelectedRecords.ContextMenuVisible = false;
            this.dgSelectedRecords.CstmAlternateRowColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.dgSelectedRecords.CstmAlternateRowColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(220)))), ((int)(((byte)(234)))));
            this.dgSelectedRecords.CstmCellsColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.dgSelectedRecords.CstmEnableSelectedCellColors = true;
            this.dgSelectedRecords.CstmNormalRowColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgSelectedRecords.CstmNormalRowColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgSelectedRecords.CstmRowsColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.dgSelectedRecords.CstmSelectedCellColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgSelectedRecords.CstmSelectedCellColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(213)))), ((int)(((byte)(103)))));
            this.dgSelectedRecords.CstmSelectedRowColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(228)))), ((int)(((byte)(145)))));
            this.dgSelectedRecords.CstmSelectedRowColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(213)))), ((int)(((byte)(103)))));
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgSelectedRecords.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgSelectedRecords.DefaultSort = false;
            this.dgSelectedRecords.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgSelectedRecords.EnableDataFiltering = false;
            this.dgSelectedRecords.GridColor = System.Drawing.Color.CadetBlue;
            this.dgSelectedRecords.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgSelectedRecords.HeaderForeColor = System.Drawing.Color.Black;
            this.dgSelectedRecords.HotSearchChild = false;
            this.dgSelectedRecords.LangIDs = new string[0];
            this.dgSelectedRecords.LevelHierarchyColumn1 = -1;
            this.dgSelectedRecords.LevelHierarchyColumn2 = -1;
            this.dgSelectedRecords.LevelSearch = false;
            this.dgSelectedRecords.Location = new System.Drawing.Point(367, 11);
            this.dgSelectedRecords.MessageID = "";
            this.dgSelectedRecords.MultiLine = false;
            this.dgSelectedRecords.Name = "dgSelectedRecords";
            this.dgSelectedRecords.ReadOnly = true;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgSelectedRecords.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgSelectedRecords.RowHeadersWidth = 15;
            this.dgSelectedRecords.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgSelectedRecords.RowHeaderWidth = 15;
            this.dgSelectedRecords.RowTemplate.Height = 16;
            this.dgSelectedRecords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSelectedRecords.Size = new System.Drawing.Size(323, 281);
            this.dgSelectedRecords.SortColumn = 0;
            this.dgSelectedRecords.TabIndex = 22;
            this.dgSelectedRecords.VirtualMode = true;
            this.dgSelectedRecords.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgSelectedRecords_CellDoubleClick);
            this.dgSelectedRecords.DoubleClick += new System.EventHandler(this.dgSelectedRecords_DoubleClick_1);
            // 
            // frmHotSearch
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(704, 335);
            this.Controls.Add(this.gradientPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(710, 359);
            this.MessageID = "";
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(352, 359);
            this.Name = "frmHotSearch";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TabText = "Search Records";
            this.Text = "Search Records";
            this.Load += new System.EventHandler(this.frmHotSearch_Load);
            this.Closed += new System.EventHandler(this.frmHotSearch_Closed);
            this.panelMultiBtns.ResumeLayout(false);
            this.panelStdBtns.ResumeLayout(false);
            this.panelLevel.ResumeLayout(false);
            this.gradientPanel1.ResumeLayout(false);
            this.grpData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgRecords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSelectedRecords)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmHotSearch_Load(object sender, System.EventArgs e)
		{

            if (this.MultiLine == true)
            {
                dgSelectedRecords.LangIDs = strLangID;
               
                if (dgSelectedRecords.DesignGrid(dgRecords, ref strErrMsg) == false)
                {
                    MessageBox.Show(strErrMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dgSelectedRecords.MultiLine = true;
                dtSelected = (DataTable)dgSelectedRecords.DataSource;
                dtSelected.Rows.Clear();
                dgSelectedRecords.DataSource = dtSelected;
                dgSelectedRecords.ReadOnly = true;
                dgSelectedRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


            }
            else
            {

                //Color color = Color.FromArgb(198, 240, 191);
               
                //this.BackColor = Color.SeaGreen;
                //dgSearch.EnableHeadersVisualStyles = true;

                ////dgSearch.ColumnHeadersDefaultCellStyle.BackColor = Color.Honeydew;
                //dgSearch.RowHeadersDefaultCellStyle.BackColor = Color.Honeydew;
                //dgSearch.ColumnHeadersDefaultCellStyle.ForeColor = Color.SeaGreen;
                //dgSearch.DefaultCellStyle.ForeColor = Color.Red;
                //dgSearch.DefaultCellStyle.SelectionForeColor = Color.Green;
                //dgSearch.DefaultCellStyle.SelectionBackColor = Color.Honeydew;
                //dgSearch.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
                //dgSearch.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Honeydew;
                //dgSearch.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
                //dgSearch.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Honeydew;
                //dgSearch.BackgroundColor = Color.Honeydew;
                
                //dgRecords.EnableHeadersVisualStyles = false;
                //dgRecords.ColumnHeadersDefaultCellStyle.BackColor = Color.Honeydew;
                //dgRecords.RowHeadersDefaultCellStyle.BackColor = Color.Honeydew;
                //dgRecords.ColumnHeadersDefaultCellStyle.ForeColor = Color.SeaGreen;
                //dgRecords.DefaultCellStyle.ForeColor = Color.Red;
                //dgRecords.DefaultCellStyle.SelectionForeColor = Color.Green;
                //dgRecords.DefaultCellStyle.SelectionBackColor = Color.Honeydew;
                //dgRecords.BackgroundColor = Color.Honeydew;
                //dgRecords.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
                //dgRecords.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Honeydew;
                //dgRecords.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
                //dgRecords.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Honeydew;

            }

            if (boolLevelSearch)
            {
                dgRecords.ShowLevelData(dgRecords, 1);
                dgRecords.IncreseCount();
                lblMax.Text = dgRecords.MaxLevel.ToString();
                lblNo.Text = dgRecords.CurrentLevel.ToString();
            }

            
			dgSelectedRecords.DataSourceChanged += new EventHandler(dgSelectedRecords_DataSourceChanged);
            dgRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            int flags = WinAPI.AW_ACTIVATE | WinAPI.AW_CENTER;
            int animationTime = 250;
            dgRecords.RowHeadersWidth = 18;
            dgSelectedRecords.RowHeadersWidth = 18;
            dgRecords.dtCurrentData = dgRecords.dtFullData.Copy();
            WinAPI.AnimateWindow(this.Handle, animationTime, flags);
            //if (dgSearch.Rows.Count >= 0)
            //    this.dgSearch.Rows[0].Selected = true;
            this.Invalidate(true);
            dgRecords.Focus();

		}


		public bool MultiLine
		{
			get
			{
				return boolMultiLine;
			}
			set
			{
				if(boolLevelSearch)
				{
					boolMultiLine = false;
					dgRecords.MultiLine = false;
				}
				else
				{
					boolMultiLine = value;
					dgRecords.MultiLine = value;
				}
			}
		}


		public bool LevelSearch
		{
			get
			{
				return boolLevelSearch;
			}
			set
			{
				boolLevelSearch = value;
				dgRecords.LevelSearch = value;
				if(value == true)
				{
					boolMultiLine = false;
					dgRecords.MultiLine = false;
				}
			}
		}


		public bool ShowHotSearch(ref string ErrMsg, params string[] LangIDs)
		{
			try
			{
				ErrMsg="";
				this.Text=m_Caption;
		
				strLangID = LangIDs;

                if (boolMultiLine == false)
                {
                    dgSelectedRecords.Visible = false;
                    panelMultiBtns.Visible = false;
                    if (boolLevelSearch)
                        panelLevel.Visible = true;

                    //dgRecords.Width = dgRecords.Width + 100;
                    //dgSearch.Width = dgSearch.Width + 100;
                    //panelLevel.Left = dgSearch.Width - panelLevel.Width;
                    this.panelStdBtns.Location = new System.Drawing.Point(93, 302);
                    this.Size = new System.Drawing.Size(352, 359);
                    this.grpData.Size = new Size(339, 298);
                    lblcomments.Visible = false;
                    //dgRecords.Width = dgSearch.Width;
                    btnRemove.Enabled = btnRemoveAll.Enabled = false;
                    
                }
                else
                {
                    btnRemove.Enabled = btnRemoveAll.Enabled = false;
                    //dgRecords.MaximumSize = new Size(314, 296);
                    //dgSelectedRecords.MaximumSize = new Size(314, 296);
                    //this.BackColor = Color.SeaGreen;
                }
				dgRecords.SetDefault=false;
                //** To Prevent Sorting Special Functionality In HotSearcg Grids**/
                dgRecords.HandleGridSort = false;
                dgSelectedRecords.HandleGridSort = false;
                //** To Prevent Sorting Special Functionality In HotSearcg Grids**/
				dgRecords.LangIDs = LangIDs;
				if(dgRecords.DesignGridFromSQL( m_Sql,ref strErrMsg )==false)
				{
					ErrMsg=strErrMsg;
					return false;
				}

                //if (boolLevelSearch == true)
                //{
                //    //this.KeyDown += new KeyEventHandler(BackSpaceKeyDown);
                //    arlHierarchyLevel = new ArrayList();
                //    arlHierarchyValue = new ArrayList();
                //    arlPrimKey = new ArrayList();
                //}

				if( dtBeforeHotSearch != null )
				{
					DataTable dtRecords = (DataTable)(dgRecords.DataSource);
					clsSqlOperations objMinus = new clsSqlOperations();
					System.Data.DataTable dtModified = objMinus.Minus( dtRecords, dtBeforeHotSearch, UK_HotSearch, UK_CurrentTable );
                    dgRecords.SetDataBinding(dtModified,"");
					objMinus = null;
				}

                //if(dgSearch.DesignGrid(dgRecords,ref strErrMsg)==false)
                //{
                //    ErrMsg=strErrMsg;
                //    return false;
                //}
                                

                dgRecords.MultiLine = boolMultiLine;
				dgSelectedRecords.MultiLine = boolMultiLine;

                dgSelectedRecords.SetDataBinding(((DataTable)dgRecords.DataSource).Clone(), "");
                //Debanjan Routh
                //The following code is added because the header text of the selected 
                //grid was not coming coorect.The language converted text was not coming
                //Here the header text of the Records grid is simply copied to the 
                //header text of the Selected Grid.
                for (int i = 0; i < dgSelectedRecords.Columns.Count; i++)
                {
                    dgSelectedRecords.Columns[i].HeaderText = dgRecords.Columns[i].HeaderText;
                }
                /////
                //dgRecords.AssociatedSearchGrid = dgSearch;
				//dgSearch.Focus();
				this.ShowDialog();
				return true;
			}
			catch(Exception ex)
			{
				ErrMsg=ex.Source + " - " + ex.Message;
				return false;
			}
		}
		private void EnterKeyDown(object sender,KeyEventArgs e)
		{
			if(e.KeyData ==Keys.Enter )
			{
				//dgSearch.IsSelected(
				btnAdd_Click(sender,null);
			}
		}


		

    

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message m,Keys keyData) 
        { 
            try
            {


                if (Control.ModifierKeys == Keys.Control)
                {
                    if (keyData == (Keys.LButton | Keys.Back | Keys.Control))
                    {
                        dgRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        if (dgRecords.Focused)
                        {
                            dgSelectedRecords.Select();
                            dgSelectedRecords.Focus();
                        }
                        else
                        {
                            dgRecords.Select(0);
                            dgRecords.Focus();
                        }
                        //dgSearch.GetNextControl((Control)dgRecords, true);
                       // dgRecords.Select(0);

                    }
                }



                //if (keyData == Keys.Escape)
                //{
                //    boolCancel = true;
                //    SetReturn();
                //    this.Close();
                //}

                if (keyData == Keys.Enter)
                {
                    if (!this.MultiLine)
                    {
                        if (boolLevelSearch && (boolAllowOKBtn == false))
                        {
                            if (dgRecords.MaxLevel != dgRecords.CurrentLevel)
                            {

                                dgRecords.IncreseCount();
                                lblMax.Text = dgRecords.MaxLevel.ToString();
                                lblNo.Text = dgRecords.CurrentLevel.ToString();
                                dgRecords.CurrentRowIndex = dgRecords.CurrentCell.RowIndex;
                                dgRecords.ShowLevelData(dgRecords, dgRecords.CurrentLevel);
                                //dgSearch.Focus();
                                dgRecords.Rows[0].Selected = true;
                            }
                            else
                            {
                                btnOK_Click(null, null);
                            }
                        }
                        else
                            btnOK_Click(null, null);
                    }
                    else
                    {
                        if (dgRecords.IsSelected(dgRecords.CurrentRow.Index))
                        {
                            btnOK_Click(null, null);
                        }
                    }
                    //if (dgRecords.IsSelected(dgRecords.CurrentRow.Index))
                    //{
                    //    btnOK_Click(null, null);
                    //}
                }

                //------Add Row 
                if ((this.MultiLine == true) && (keyData == Keys.Right))
                {
                    if (dgRecords.RowCount == 999 && boolRestrictTo999Rows)
                    {
                    }
                    else
                    {
                        if (dgRecords.IsSelected(dgRecords.CurrentRow.Index) == true)
                            btnAdd_Click_1(null, null);
                    }
                }

                //-----Remove Row
                if ((this.MultiLine == true) && (keyData == Keys.Left))
                {
                    try
                    {
                        if (dgSelectedRecords.IsSelected(dgSelectedRecords.CurrentRow.Index) == true)
                            btnRemove_Click(null, null);

                        //dgRecords.Select(0);
                    }
                    finally
                    {
                        dgRecords.Select(0);
                    }
                }


                if ((this.MultiLine == true) && ((Control.ModifierKeys & Keys.Control) | Keys.Right) == keyData && keyData != Keys.Right)
                {
                    if (dgRecords.RowCount == 999 && boolRestrictTo999Rows)
                    {
                    }
                    else
                    {
                        btnAdd_Click(null, null);
                    }
                }

                if ((this.MultiLine == true) && ((Control.ModifierKeys & Keys.Control) | Keys.Left) == keyData && keyData != Keys.Left)
                {
                    btnRemoveAll_Click(null, null);
                }


                //if (keyData == Keys.Up)
                //{
                //    if (dgRecords.CurrentRowIndex > 0)
                //    {
                //        if (dgRecords.IsSelected(dgRecords.CurrentRowIndex))
                //        {
                //            int row = dgRecords.CurrentRowIndex;
                //            dgRecords.UnSelect(row);
                //            row = row - 1;
                //            dgRecords.Select(row);
                //            while (!dgRecords.IsSelected(row))
                //            {
                //                dgRecords.Select(row);
                //            }
                //            dgRecords.CurrentRowIndex = row;
                //            this.currentrow = row;
                //            dgRecords.SelectionBackColor = Color.Blue;
                //            dgRecords.SelectionForeColor = Color.Blue;
                //            dgRecords.Update();
                //            return true;


                //        }
                //    }
                //}
                //if (keyData == Keys.Down)
                //{
                //    if (dgRecords.CurrentRowIndex < dgRecords.RowCount)
                //    {
                //        if (dgRecords.IsSelected(dgRecords.CurrentRowIndex))
                //        {
                //            int row = dgRecords.CurrentRowIndex;
                //            dgRecords.UnSelect(dgRecords.CurrentRowIndex);
                //            row = row + 1;
                //            dgRecords.Select(row);
                //            while (!dgRecords.IsSelected(row))
                //            {
                //                dgRecords.Select(row);
                //            }
                //            dgRecords.CurrentRowIndex = row;
                //            this.currentrow = row;
                //            dgRecords.SelectionBackColor = Color.Blue;
                //            dgRecords.SelectionForeColor = Color.Blue;
                //            dgRecords.Update();
                //            return true;


                //        }
                //    }
                //}

                return base.ProcessCmdKey (ref m, keyData);
            }
            catch(Exception)
            {
                return false;
            }
        }

		public void SetReturn()
		{
			try
			{
				
				if(boolCancel == true)
					return;


                CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
                NumberFormatInfo nmfi = new NumberFormatInfo();
                nmfi = ci.NumberFormat;
				if(boolMultiLine == false)
				{
                    
                    CurrencyManager cm = (CurrencyManager)dgRecords.BindingContext[dgRecords.DataSource, dgRecords.DataMember];
                    DataRowView drv = (DataRowView)cm.Current;
                    storedVal=new string[dgRecords.intColCount];
					if ((dgRecords.CurrentRowIndex >= 0) || dgRecords.CurrentCell.RowIndex>=0 )
					{
						int index=dgRecords.CurrentRowIndex;
                        DataRow drselected = (DataRow)drv.Row;
                        for (int i = 0; i < dgRecords.intColCount; i++)
                        {
                            storedVal[i] = drselected.ItemArray[i].ToString();
                        }
					}
					else
					{
						for(int i=0;i<dgRecords.intColCount;i++)
							storedVal[i] = "";
					}
				}
				else
				{
					DataTable selected=(DataTable)dgSelectedRecords.DataSource;
                    if (selected.Rows.Count == 0)
                    {
                        return;
                    }
                    if(boolAddCombine && chkCombined.Checked)
					{
						storedVal = new string[2];
						storedVal[0] = "Combined";
						storedVal[1] = chkCombined.Text;
					}
					else
					{
                        if (boolReturnOnlyTable == false)
                        {
                            storedVal = new string[dgRecords.intColCount];
                            CurrencyManager cm = (CurrencyManager)this.BindingContext[dgSelectedRecords.DataSource, dgSelectedRecords.DataMember];
                            DataRowView drv1 = (DataRowView)cm.Current;
                            DataView dv = (DataView)drv1.DataView;

                            for (int i = 0; i < dgSelectedRecords.intColCount; i++)
                            {
                                storedVal[i] = "";
                            }

                            DataTable dtTemp = dv.Table;

                            for (int j = 0; j < dgSelectedRecords.intColCount; j++)
                            {
                                for (int i = 0; i < dv.Count; ++i)
                                {
                                    storedVal[j] += dtTemp.Rows[i][j].ToString() + Convert.ToChar(2);
                                }
                                if (storedVal[j] != "")
                                    storedVal[j] = storedVal[j].Substring(0, storedVal[j].Length - 1);
                            }
                        }
                        else
                        {
                            //// CurrencyManager cm = (CurrencyManager)this.BindingContext[dgSelectedRecords.DataSource, dgSelectedRecords.DataMember];
                            ////DataRowView drv1 = (DataRowView)cm.Current;
                            ////DataView dv = (DataView)drv1.DataView;
                            //// DataTable dtTemp = dv.Table;
                            //// this.DataAfterHotSearch = dtTemp;
                        }
					}
				}
				this.Close();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Source + " - " + ex.Message);
				return;
			}
		}


		public string[] retStr()
		{
			if(storedVal!=null)
				return storedVal;
			else
			{
				storedVal=new string[dgRecords.intColCount];
				for(int i=0;i<dgRecords.intColCount;i++)
					storedVal[i]="";
				return storedVal;
			}

		}


        private void frmHotSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                this.Close();



        }


    	private void btnCancel_Click(object sender, System.EventArgs e)
		{
			boolCancel = true;
			SetReturn();
            this.DialogResult = DialogResult.Cancel; 
            this.Close();
			return;
		}


		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if(boolLevelSearch && (boolAllowOKBtn == false))
			{
				if(dgRecords.MaxLevel != dgRecords.CurrentLevel)
				{
					return;
				}
			}
			boolCancel = false;
            bool selectedflag = false;
            if (this.MultiLine)
            {
                CurrencyManager cm = (CurrencyManager)this.BindingContext[dgSelectedRecords.DataSource, dgSelectedRecords.DataMember];
                DataView dv=(DataView)cm.List;
                if (dv.Table.Rows.Count > 0)
                {
                    selectedflag = true;
                }
            }
            else
            {
                //BindingSource bsource=
                //DataTable dttest;
                //if (!boolLevelSearch)
                //    dttest = (DataTable)((BindingSource)dgRecords.DataSource).DataSource;
                //else
                //    dttest = (DataTable)(dgRecords.DataSource);
                DataTable dttest = (DataTable)dgRecords.DataSource;
                for (int q = 0; q < dttest.Rows.Count; q++)
                {
                    //if (dgRecords.IsSelected(q))
                    {
                        selectedflag = true;
                    }
                }
            }
            if (selectedflag)
            {
                this.DialogResult = DialogResult.OK;
                SetReturn();

            }
            else
            {
                this.Close();
                this.DialogResult = DialogResult.OK;
                return;
            }
		}


		private void frmHotSearch_Closed(object sender, System.EventArgs e)
		{
			bool blLocalCancel = boolCancel;
			boolCancel = true;
			SetReturn();
			boolCancel = blLocalCancel;
			this.Close();
		}


        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                object[] obj;
                CurrencyManager cmanag = (CurrencyManager)dgRecords.BindingContext[dgRecords.DataSource];
                DataRowView view = (DataRowView)cmanag.Current;
                DataTable dtnew = view.DataView.ToTable();
                DataTable dtMain = ((DataTable)dgRecords.DataSource).Clone();
                dtMain.BeginLoadData();
                dtMain.Merge(dtnew, true, MissingSchemaAction.Error);
                dtMain.EndLoadData();
                dtSelected = ((DataTable)dgSelectedRecords.DataSource).Copy();
                DataTable dtToBeRemoved = dtMain.Clone();
                ArrayList list = new ArrayList();



                for (int i = 0; i < dgRecords.Rows.Count; i++)
                {
                    if (boolRestrictTo999Rows == true && dtSelected.Rows.Count == 999)
                        break;

                    obj = new object[dtMain.Columns.Count];
                    for (int k = 0; k < dtMain.Columns.Count; k++)
                    {
                        obj[k] = dtMain.Rows[i][k].ToString();
                    }
                    dtToBeRemoved.LoadDataRow(obj, true);
                    dtSelected.LoadDataRow(obj, true);
                    list.Add(dtMain.Rows[i]);
                    dgRecords.Update();
                    obj = null;

                }



                clsSqlOperations objMinus = new clsSqlOperations();
                DataTable dtOutPut = objMinus.Minus(dtMain, dtToBeRemoved);
                objMinus = null;


                this.btnRemove.Enabled = true;
                this.btnRemoveAll.Enabled = true;
                this.btnAddAll.Enabled = false;
                this.btnAdd.Enabled = false;
                if (boolRestrictTo999Rows == true)
                {
                    if (dtSelected.Rows.Count == 999)
                        DisableEnableBtn(true);
                    else
                        DisableEnableBtn(false);
                }

                dtnew.AcceptChanges();
                dtMain.AcceptChanges();
                dgRecords.DataSource = dtOutPut;
                //dgRecords.dtFullData = dtOutPut;
                
                dgSelectedRecords.DataSource = reverse_datatable(dtSelected);
                //dgSearch.dataUpgrade(dtSelected);
                //lblcomments.Text = dtSelected.Rows.Count + " out of " + dgSearch.FullData.Rows.Count + " rows selected";
                dtMain.Dispose();
                dtSelected.Dispose();
                dtOutPut.Dispose();
                dtnew.Dispose();
                dtToBeRemoved.Dispose();
                dgRecords.Refresh();
                dgSelectedRecords.Refresh();

                GenerateCurrentData();

                this.btnRemove.Enabled = true;
                this.btnRemoveAll.Enabled = true;
                btnAdd.Enabled = false;
                btnAddAll.Enabled = false;
                dgSelectedRecords.Focus();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Source + " - " + ex.Message);
            }
        }


		private void DisableEnableBtn(bool isDisable)
		{
			if(isDisable)
			{
				btnAdd.Enabled = false;
				btnAddAll.Enabled =false;
                btnRemove.Enabled = true;
                btnRemoveAll.Enabled = true;

			}
			else
			{
				btnAdd.Enabled = true;
				btnAddAll.Enabled = true;
                btnRemove.Enabled = true;
                btnRemoveAll.Enabled = true;
			}
		}


		private void btnAddAll_Click(object sender, System.EventArgs e)
		{
			try
			{
                object[] obj;
                CurrencyManager cmanag = (CurrencyManager)dgRecords.BindingContext[dgRecords.DataSource];
                DataRowView view = (DataRowView)cmanag.Current;
                DataTable dtnew = view.DataView.ToTable();
                DataTable dtMain = ((DataTable)dgRecords.DataSource).Clone();
                dtMain.BeginLoadData();
                dtMain.Merge(dtnew, true, MissingSchemaAction.Error);
                dtMain.EndLoadData();
                dtSelected = ((DataTable)dgSelectedRecords.DataSource).Copy();
                DataTable dtToBeRemoved = dtMain.Clone();
                ArrayList list = new ArrayList();



                for (int i = 0; i < dgRecords.Rows.Count; i++)
                {
                    if (boolRestrictTo999Rows == true && dtSelected.Rows.Count == 999)
                        break;

                    obj = new object[dtMain.Columns.Count];
                    for (int k = 0; k < dtMain.Columns.Count; k++)
                    {
                        obj[k] = dtMain.Rows[i][k].ToString();
                    }
                    dtToBeRemoved.LoadDataRow(obj, true);
                    dtSelected.LoadDataRow(obj, true);
                    list.Add(dtMain.Rows[i]);
                    dgRecords.Update();
                    obj = null;

                }



                clsSqlOperations objMinus = new clsSqlOperations();
                DataTable dtOutPut = objMinus.Minus(dtMain, dtToBeRemoved);
                objMinus = null;


                this.btnRemove.Enabled = true;
                this.btnRemoveAll.Enabled = true;
                this.btnAddAll.Enabled = false;
                this.btnAdd.Enabled = false;
                if (boolRestrictTo999Rows == true)
                {
                    if (dtSelected.Rows.Count == 999)
                        DisableEnableBtn(true);
                    else
                        DisableEnableBtn(false);
                }

                dtnew.AcceptChanges();
                dtMain.AcceptChanges();
                dgRecords.DataSource = dtOutPut;
                dgSelectedRecords.DataSource = reverse_datatable(dtSelected);
                //dgSearch.dataUpgrade(dtSelected);
              //  lblcomments.Text = dtSelected.Rows.Count + " out of " + dgSearch.FullData.Rows.Count + " rows selected";
                dtMain.Dispose();
                dtSelected.Dispose();
                dtOutPut.Dispose();
                dtnew.Dispose();
                dtToBeRemoved.Dispose();
                dgRecords.Refresh();
                dgSelectedRecords.Refresh();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Source + " - " + ex.Message);
			}
		}


        private void btnRemove_Click(object sender, System.EventArgs e)
        {

            try
            {
                DataTable dtMain = ((DataTable)dgRecords.DataSource).Copy();
                CurrencyManager cmanag = (CurrencyManager)dgSelectedRecords.BindingContext[dgSelectedRecords.DataSource];
                DataRowView view = (DataRowView)cmanag.Current;
                dtSelected = view.DataView.ToTable();

                
                ArrayList list = new ArrayList();

                for (int i = 0; i < dgSelectedRecords.SelectedRows.Count; i++)
                {
                    DataGridViewRow selectedrow = dgSelectedRecords.SelectedRows[i]; ;
                    int index = selectedrow.Index;
                    object[] obj = new object[dtSelected.Columns.Count];
                    for (int k = 0; k < dtSelected.Columns.Count; k++)
                    {
                        obj[k] = dtSelected.Rows[index][k].ToString();
                    }

                    dtMain.LoadDataRow(obj, true);
                    list.Add(dtSelected.Rows[index]);
                    obj = null;


                }
               
                for (int i = 0; i < list.Count; i++)
                {
                    dtSelected.Rows.Remove((DataRow)list[i]);
                }
                //dgSearch.dataUpgrade(dtSelected);
                //lblcomments.Text = dtSelected.Rows.Count + " out of " + dgSearch.FullData.Rows.Count + " rows selected";
                dtMain.AcceptChanges();
                dtSelected.AcceptChanges();

                if (boolRestrictTo999Rows == true)
                {
                    if (dtSelected.Rows.Count == 999)
                        DisableEnableBtn(true);
                    else
                        DisableEnableBtn(false);
                }

                dgRecords.DataSource = dtMain;
                //dgRecords.dtFullData = dtMain;
                
                dgSelectedRecords.DataSource = dtSelected;


                dtMain.Dispose();
                dtSelected.Dispose();

                if (dgSelectedRecords.Rows.Count == 0)
                {
                    this.btnRemove.Enabled = false;
                    this.btnRemoveAll.Enabled = false;
                }
                this.btnAdd.Enabled = true;
                this.btnAddAll.Enabled = true;
                dgRecords.Refresh();
                dgSelectedRecords.Refresh();
                dgSelectedRecords.Focus();
                GenerateCurrentData();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }


        private void btnRemoveAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                DataTable dtMain = ((DataTable)dgRecords.DataSource).Copy();
                CurrencyManager cmanag = (CurrencyManager)dgSelectedRecords.BindingContext[dgSelectedRecords.DataSource];
                DataRowView view = (DataRowView)cmanag.Current;
                dtSelected = view.DataView.ToTable();


                ArrayList list = new ArrayList();

                for (int i = 0; i < dgSelectedRecords.Rows.Count; i++)
                {
                    object[] obj = new object[dtSelected.Columns.Count];
                    for (int k = 0; k < dtSelected.Columns.Count; k++)
                    {
                        obj[k] = dtSelected.Rows[i][k].ToString();
                    }

                    dtMain.LoadDataRow(obj, true);
                    list.Add(dtSelected.Rows[i]);
                    obj = null;


                }

                for (int i = 0; i < list.Count; i++)
                {
                    dtSelected.Rows.Remove((DataRow)list[i]);
                }
               // dgSearch.dataUpgrade(dtSelected);
               // lblcomments.Text = dtSelected.Rows.Count + " out of " + dgSearch.FullData.Rows.Count + " rows selected";
                dtMain.AcceptChanges();
                dtSelected.AcceptChanges();

                if (boolRestrictTo999Rows == true)
                {
                    if (dtSelected.Rows.Count == 999)
                        DisableEnableBtn(true);
                    else
                        DisableEnableBtn(false);
                }

                dgRecords.DataSource = dtMain;
                //dgRecords.dtFullData = dtMain;
                
                dgSelectedRecords.DataSource = dtSelected;


                dtMain.Dispose();
                dtSelected.Dispose();
                this.btnRemove.Enabled = false;
                this.btnRemoveAll.Enabled = false;
                this.btnAdd.Enabled = true;
                this.btnAddAll.Enabled = true;

                dgRecords.Refresh();
                dgSelectedRecords.Refresh();
                dgSelectedRecords.Focus();
                GenerateCurrentData();
                dgRecords.Focus();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }


		private void dgSelectedRecords_DataSourceChanged(object sender, EventArgs e)
		{
			if( boolRestrictTo999Rows == true )
			{
				if(dgSelectedRecords.RowCount >= 999)
				{
					btnAdd.Enabled = false;
					btnAddAll.Enabled = false;
				}
				else
				{
					btnAdd.Enabled = true;
					btnAddAll.Enabled = true;
				}
			}
		}


		private void chkCombined_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkCombined.Checked == true)
			{
				btnAdd.Enabled = false;
				btnAddAll.Enabled = false;
				btnRemove.Enabled = false;
				btnRemoveAll.Enabled = false;
			}
			else
			{
				btnAdd.Enabled = true;
				btnAddAll.Enabled = true;
				btnRemove.Enabled = true;
				btnRemoveAll.Enabled = true;
			}
		}



		private void dgSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
//			if(e.KeyData == Keys.Down)
//				dgRecords.Focus();
		}

      

        private DataTable reverse_datatable(DataTable dt)
        {
            try
            {
                
                DataTable dtreversed=dt.Clone();
                for (int i = dt.Rows.Count-1; i>=0;i-- )
                {
                    object[] data = dt.Rows[i].ItemArray;
                    DataRow dr = dtreversed.NewRow();
                    dr.ItemArray = data;
                    dtreversed.Rows.Add(dr);
                    dtreversed.AcceptChanges();
                }
                return dtreversed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void lblcomments_Click(object sender, EventArgs e)
        {
          //  MessageBox.Show("Here Comes The Comments");
        }

        private void panelLevel_Paint(object sender, PaintEventArgs e)
        {
           // MessageBox.Show("Panel for Level");
        }

        private void dgSelectedRecords_DoubleClick_1(object sender, EventArgs e)
        {
            
        }

        private void dgRecords_DoubleClick(object sender, EventArgs e)
        {
            
           
        }

        private void dgRecords_MouseDown_1(object sender, MouseEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            try
            {
                object[] obj;
                CurrencyManager cmanag = (CurrencyManager)dgRecords.BindingContext[dgRecords.DataSource];
                if (cmanag.List.Count == 0)
                {
                    return;
                }
                DataRowView view = (DataRowView)cmanag.Current;
                DataTable dtnew = view.DataView.ToTable();
                DataTable dtMain = ((DataTable)dgRecords.DataSource).Clone();
                dtMain.BeginLoadData();
                dtMain.Merge(dtnew, true, MissingSchemaAction.Error);
                dtMain.EndLoadData();
                dtSelected = ((DataTable)dgSelectedRecords.DataSource).Copy();
                DataTable dtToBeRemoved = dtMain.Clone();
                ArrayList list = new ArrayList();
                for (int i = 0; i < dgRecords.SelectedRows.Count; i++)
                {
                    if (boolRestrictTo999Rows == true && dtSelected.Rows.Count == 999)
                        break;

                    DataGridViewRow selectedrow = dgRecords.SelectedRows[i];
                    int index = selectedrow.Index;
                    obj = new object[dtMain.Columns.Count];
                    for (int k = 0; k < dtMain.Columns.Count; k++)
                    {
                        obj[k] = dtMain.Rows[index][k].ToString();
                    }
                    dtToBeRemoved.LoadDataRow(obj, true);
                    dtSelected.LoadDataRow(obj, true);
                    list.Add(dtMain.Rows[i]);
                    dgRecords.Update();
                    obj = null;

                }



                clsSqlOperations objMinus = new clsSqlOperations();
                DataTable dtOutPut = objMinus.Minus(dtMain, dtToBeRemoved);
                objMinus = null;



                if (boolRestrictTo999Rows == true)
                {
                    if (dtSelected.Rows.Count == 999)
                        DisableEnableBtn(true);
                    else
                        DisableEnableBtn(false);
                }
                
                dtnew.AcceptChanges();
                dtMain.AcceptChanges();
                dgRecords.DataSource = dtOutPut;
                //dgRecords.dtFullData = dtOutPut;
                
                dgSelectedRecords.DataSource = reverse_datatable(dtSelected);
                //dgSearch.dataUpgrade(dtSelected);
                //lblcomments.Text = dtSelected.Rows.Count + " out of " + dgSearch.FullData.Rows.Count + " rows selected";
                dtMain.Dispose();
                dtSelected.Dispose();
                dtOutPut.Dispose();
                dtnew.Dispose();
                dtToBeRemoved.Dispose();
                dgRecords.Refresh();
                dgSelectedRecords.Refresh();
                GenerateCurrentData();
                this.btnRemove.Enabled = true;
                this.btnRemoveAll.Enabled = true;
                if (dgRecords.Rows.Count == 0)
                {
                    btnAdd.Enabled = false;
                    btnAddAll.Enabled = false;
                }
             }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void GenerateCurrentData()
        {
            //clsSqlOperations _oper;
            //DataTable dtTemp ;
            ////DataTable dtTemp = ((DataTable)dgRecords.DataSource).Clone();

            //DataTable dtSelectedData = (DataTable)dgSelectedRecords.DataSource;
            //if (dtSelectedData.Rows.Count >0)
            //{
            //    _oper = new clsSqlOperations();
            //    dtTemp = _oper.Minus(dgRecords.dtFullData, dtSelectedData);
            //    dgRecords.dtCurrentData = dtTemp;
            //}
            //else
            //{
            //    dgRecords.dtCurrentData = dgRecords.dtFullData.Copy(); ;
            //}

                clsSqlOperations _oper;
                DataTable dtTemp;
                DataTable dtSelectedDataTemp = (DataTable)dgSelectedRecords.DataSource;
                DataTable dtSelectedData = dgRecords.dtFullData.Clone();
                object[] objRow;
                int i = 0;
                foreach (DataRow _row in dtSelectedDataTemp.Rows)
                {

                    objRow = new object[dtSelectedDataTemp.Columns.Count];
                    for (int k = 0; k < dtSelectedDataTemp.Columns.Count; k++)
                    {
                        //objRow[k] =(object)dgSelectedRecords.Rows[i].Cells[k].Value;
                        objRow[k] = (object)_row[k];
                    }
                    dtSelectedData.LoadDataRow(objRow, true);
                    i++;
                }


                if (dtSelectedData.Rows.Count > 0)
                {
                    _oper = new clsSqlOperations();
                    dtTemp = _oper.Minus(dgRecords.dtFullData, dtSelectedData);
                    dgRecords.dtCurrentData = dtTemp;
                }
                else
                {
                    dgRecords.dtCurrentData = dgRecords.dtFullData.Copy(); ;
                }         
        }

        private void dgRecords_DataSourceChanged(object sender, EventArgs e)
        {
            if (dgRecords.Rows.Count > 0)
            {
                btnAddAll.Enabled = true;
                btnAdd.Enabled = true;
            }
        }

        private void dgRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                try
                {
                    if (!this.MultiLine)
                    {
                        if (boolLevelSearch && (boolAllowOKBtn == false))
                        {
                            if (dgRecords.MaxLevel != dgRecords.CurrentLevel)
                            {

                                dgRecords.IncreseCount();
                                lblMax.Text = dgRecords.MaxLevel.ToString();
                                lblNo.Text = dgRecords.CurrentLevel.ToString();
                                dgRecords.ShowLevelData(dgRecords, dgRecords.CurrentLevel);
                                dgRecords.RemoveAllFilter();
                                //  dgSearch.Focus();
                                return;
                            }
                        }
                        boolCancel = false;
                        bool selectedflag = false;
                        if (this.MultiLine)
                        {
                            CurrencyManager cm = (CurrencyManager)this.BindingContext[dgSelectedRecords.DataSource, dgSelectedRecords.DataMember];
                            DataView dv = (DataView)cm.List;
                            if (dv.Table.Rows.Count > 0)
                            {
                                selectedflag = true;
                            }
                        }
                        else
                        {
                            DataTable dttest = (DataTable)dgRecords.DataSource;
                            for (int q = 0; q < dttest.Rows.Count; q++)
                            {
                                //if (dgRecords.IsSelected(q))
                                {
                                    selectedflag = true;
                                }
                            }
                        }
                        if (selectedflag)
                        {
                            this.DialogResult = DialogResult.OK;
                            SetReturn();

                        }
                        else
                        {
                            this.Close();
                            this.DialogResult = DialogResult.OK;
                            return;
                        }
                    }
                    else
                    {
                        this.btnAdd_Click_1(null, null);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            
        }

        private void dgSelectedRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                try
                {
                    if (!this.MultiLine)
                    {

                    }
                    else
                    {
                        btnRemove_Click(null, null);
                    }
                }
                catch
                {

                }
            }
        }

	}

}
