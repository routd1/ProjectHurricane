using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using UniConvert;
using ICTEAS.WinForms.Common;
using ICTEAS.DataComponents.Custom;
using System.Globalization;
using System.Reflection;

namespace ICTEAS.WinForms.Controls
{
    public class ReadnSearchGrid : System.Windows.Forms.DataGridView
    {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.clsROGContextmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.xport = new System.Windows.Forms.ToolStripMenuItem();
            this.Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.withheader = new System.Windows.Forms.ToolStripMenuItem();
            this.withoutheader = new System.Windows.Forms.ToolStripMenuItem();
            this.paste = new System.Windows.Forms.ToolStripMenuItem();
            this.Color = new System.Windows.Forms.ToolStripMenuItem();
            this.EnableFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.show = new System.Windows.Forms.ToolStripMenuItem();
            this.LineNumbers = new System.Windows.Forms.ToolStripMenuItem();
            this.showExistingItems = new ToolStripMenuItem();
            this.separator1 = new ToolStripSeparator();
            this.separator2 = new ToolStripSeparator();
            this.separator3 = new ToolStripSeparator();
            this.separator4 = new ToolStripSeparator();
            this.clsROGContextmenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // clsROGContextmenu
            // 
            this.clsROGContextmenu.AllowDrop = true;
            this.clsROGContextmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xport,
            this.separator1,
            this.Copy,
            this.paste,
            this.separator2,
            this.Color,
           // this.separator3,
            this.EnableFilter,
            this.separator4,
            this.show});
            this.clsROGContextmenu.Name = "clsROGContextmenu";
            this.clsROGContextmenu.Size = new System.Drawing.Size(134, 136);
            this.clsROGContextmenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.item_clicked);
            // 
            // xport
            // 
            this.xport.Name = "xport";
            this.xport.Size = new System.Drawing.Size(133, 22);
            this.xport.Text = "Export Data";
            // 
            // Copy
            // 
            this.Copy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.withheader,
            this.withoutheader});
            this.Copy.Name = "Copy";
            this.Copy.Size = new System.Drawing.Size(133, 22);
            this.Copy.Text = "Copy";
            // 
            // withheader
            // 
            this.withheader.Name = "withheader";
            this.withheader.Size = new System.Drawing.Size(165, 22);
            this.withheader.Text = "With Colum header";
            // 
            // withoutheader
            // 
            this.withoutheader.Name = "withoutheader";
            this.withoutheader.Size = new System.Drawing.Size(165, 22);
            this.withoutheader.Text = "Data Only";
            // 
            // paste
            // 
            this.paste.Name = "paste";
            this.paste.Size = new System.Drawing.Size(133, 22);
            this.paste.Text = "Paste";
            // 
            // Color
            // 
            this.Color.Name = "Color";
            this.Color.Size = new System.Drawing.Size(133, 22);
            this.Color.Text = "Color";
            // 
            // EnableFilter
            // 
            this.EnableFilter.Name = "EnableFilter";
            this.EnableFilter.Size = new System.Drawing.Size(133, 22);
            this.EnableFilter.Text = "Enable Filter";
            // 
            // show
            // 
            this.show.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LineNumbers,
            this.showExistingItems});
            this.show.Name = "show";
            this.show.Size = new System.Drawing.Size(133, 22);
            this.show.Text = "Show";
            // 
            // LineNumbers
            // 
            this.LineNumbers.CheckOnClick = true;
            this.LineNumbers.Name = "LineNumbers";
            this.LineNumbers.Size = new System.Drawing.Size(138, 22);
            this.LineNumbers.Text = "Line Numbers";
            this.LineNumbers.Click += new System.EventHandler(this.LineNumbers_Click);
            //
            //showExistingItems
            //
            this.showExistingItems.Name = "showExistingItems";
            this.showExistingItems.Size = new System.Drawing.Size(138, 22);
            this.showExistingItems.Text = "Show Existing Items";
            //
            //separator1
            //
            this.separator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Left;
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(138, 22);
            //
            //separator2
            //
            this.separator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Left;
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(138, 22);
            //
            //separator3
            //
            this.separator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Left;
            this.separator3.Name = "separator3";
            this.separator3.Size = new System.Drawing.Size(138, 22);
            //
            //separator4
            //
            this.separator4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Left;
            this.separator4.Name = "separator4";
            this.separator4.Size = new System.Drawing.Size(138, 22);
            // 
            // clsReadOnlyGrid
            // 
            this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.ContextMenuStrip = this.clsROGContextmenu;
            this.ReadOnly = true;
            this.VirtualMode = true;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.painting);
            this.clsROGContextmenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        void LineNumbers_Click(object sender, EventArgs e)
        {
            if (LineNumbers.Checked)
            {
                EnablRowNumbering = true;
            }
            else
            {
                EnablRowNumbering = false;
            }
        }

        #endregion

        public System.Windows.Forms.ContextMenuStrip clsROGContextmenu;
        private System.Windows.Forms.ToolStripMenuItem Copy;
        private System.Windows.Forms.ToolStripMenuItem paste;
        private System.Windows.Forms.ToolStripMenuItem xport;
        private System.Windows.Forms.ToolStripMenuItem withheader;
        private System.Windows.Forms.ToolStripMenuItem withoutheader;
        private System.Windows.Forms.ToolStripMenuItem Color;
        private System.Windows.Forms.ToolStripMenuItem EnableFilter;
        private System.Windows.Forms.ToolStripMenuItem show;
        private System.Windows.Forms.ToolStripMenuItem LineNumbers;
        private System.Windows.Forms.ToolStripSeparator separator1;
        private System.Windows.Forms.ToolStripSeparator separator2;
        private System.Windows.Forms.ToolStripSeparator separator3;
        private System.Windows.Forms.ToolStripSeparator separator4;
        private System.Windows.Forms.ToolStripMenuItem showExistingItems;
        //added by Nilanjan
        public int CurrentRowIndex = -1;
        BindingSource bsource = new BindingSource();
        public DataGridViewTextBoxColumn[] dgText;
        private DataGridView dgSearchLevel;
        private Font captionfont;
        bool captionvisible;
        public string captiontext;
        //*********
        public TextBox[] tb;

        public bool SetDefault = true;


        private bool boolLevelSearch = false;

        private bool boolMutliLine = false;

        private string strProcName = "";
        private ArrayList ProcParamNames;
        private ArrayList ProcParamValues;

        private DataTable dtReadOnlyGrid;

        public int intColCount;
        public int intVisibleColumnCount;
        private string strSQL;
        private string[] strColNames;
        public int RowCount;

        private string[] strLangID = new string[0];

        private int _MaxLevel = 0;
        private int _CurrentLevel = 0;

        private clsSearchGrid _dgThisSearchGrid;

        private DataTable dtLevelData;



        private string strMsgID = "";

        private string strLevelHier = "";
        private string strValueHier = "";

        private int _ReqColNo1 = -1;
        private int _ReqColNo2 = -1;

        private bool _DefaultSort = false;
        private int _SortCol = 0;



        private ArrayList arlHierarchyLevel;
        private ArrayList arlHierarchyValue;
        private ArrayList arlPrimKey;

        private ContextMenu _ContxtMnu;
        private MenuItem _mnuExportData;

        //
        private Image _imgRefreshData;

        public ReadnSearchGrid()
        {
            InitializeComponent();
            blHandleGridSort = true;
            Assembly Asm = Assembly.GetExecutingAssembly();
            //Search Image Not Active
            string Resource_name1 = "ICTEAS.WinForms.Resources.refresh.png";
            System.IO.Stream stream1 = Asm.GetManifestResourceStream(Resource_name1);
            _imgRefreshData = Image.FromStream(stream1);

            ProcParamNames = new ArrayList();
            ProcParamValues = new ArrayList();
            this.AutoGenerateColumns = false;
            //just to test
            withheader.Click += new EventHandler(withheader_Click);
            withoutheader.Click += new EventHandler(withoutheader_Click);
            EnableFilter.Click += new EventHandler(EnableFilter_Click);

            //***********************************
            //Coloring
            //this.EnableHeadersVisualStyles = true;
            //this.ColumnHeadersDefaultCellStyle.BackColor = Color.Honeydew;
            //this.RowHeadersDefaultCellStyle.BackColor = Color.Honeydew;
            //this.ColumnHeadersDefaultCellStyle.ForeColor = Color.SeaGreen;
            //this.DefaultCellStyle.ForeColor = Color.Red;
            //this.DefaultCellStyle.SelectionForeColor = Color.Green;
            //this.DefaultCellStyle.SelectionBackColor = Color.Honeydew;
            //this.BackgroundColor = Color.Honeydew;
            //this.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
            //this.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Honeydew;
            //this.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
            //this.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Honeydew;

            //THIS LINE IS ADDED SO THAT EACH CAN EDITED WITH A SINGLE CLICK
            this.EditMode = DataGridViewEditMode.EditOnEnter;
            //***FOLLOWING LINES ARE ADDED TO REDUCE FLICKER OF THE COMPONENT
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            //****************************************************************
            //this.EnableHeadersVisualStyles = true;
            this.ScrollBars = ScrollBars.Both;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.ColumnHeadersHeight = 18;
            this.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;

            //Alternating Row Default Style
            System.Windows.Forms.DataGridViewCellStyle alternatingCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            alternatingCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            alternatingCellStyle.BackColor = System.Drawing.Color.Transparent;
            alternatingCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            alternatingCellStyle.ForeColor = System.Drawing.Color.Black;
            alternatingCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(247, 192, 91);
            alternatingCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.AlternatingRowsDefaultCellStyle = alternatingCellStyle;

            //Column Header Default Cell Style
            System.Windows.Forms.DataGridViewCellStyle columnHeaderCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            columnHeaderCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            columnHeaderCellStyle.BackColor = System.Drawing.SystemColors.Control;
            columnHeaderCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            columnHeaderCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            columnHeaderCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            columnHeaderCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.ColumnHeadersDefaultCellStyle = columnHeaderCellStyle;


            //Default Cell Style
            System.Windows.Forms.DataGridViewCellStyle defaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            defaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            defaultCellStyle.BackColor = System.Drawing.Color.Transparent;
            defaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            defaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            defaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(247, 192, 91);
            defaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            defaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DefaultCellStyle = defaultCellStyle;

            //
            this.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.GridColor = System.Drawing.Color.CadetBlue;
            this.RowHeadersWidth = 30;
            this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.RowTemplate.Height = 16;
            this.ColumnHeadersHeight = 18;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.BackgroundColor = System.Drawing.Color.FromArgb(105, 185, 243);

            //*************************************
            this.BindingContextChanged += new EventHandler(clsReadOnlyGrid_BindingContextChanged);
            this.KeyDown += new KeyEventHandler(clsReadOnlyGrid_KeyDown);
            this.MouseDown += new MouseEventHandler(clsReadOnlyGrid_MouseDown);
            this.ScrollBars = ScrollBars.Both;
            ToolStripManager.Renderer = new ICTEAS.WinForms.Helpers.Office2007Renderer();
            this.EnableFilter.Visible = false;
            this.showExistingItems.Visible = false;
            this.EnableDataFiltering = true;
            this.paste.Enabled = false;
            //***********************************************************
            //this.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.RoyalBlue;
            //this.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Honeydew;
            //this.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            //this.DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            //this.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Green;
            //this.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Honeydew;
            //this.BackgroundColor = System.Drawing.Color.Honeydew;
            //this.RowHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            //this.RowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            //this.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Green;******
            //this.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Honeydew;********

            //this.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            //this.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.RoyalBlue;

            //
            this.paste.Visible = false;
            this.xport.Visible = false;
            this.Copy.Visible = false;
            this.withheader.Visible = false;
            this.withoutheader.Visible = false;
            this.separator1.Visible = false;
            this.separator2.Visible = false;
        }

        private bool _EnablRowNumbering = false;
        private bool EnablRowNumbering
        {
            get { return _EnablRowNumbering; }
            set { _EnablRowNumbering = value; this.Invalidate(); }
        }
        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            if (_EnablRowNumbering)
            {
                this.RowHeadersWidth = 40;
                using (SolidBrush brush = new SolidBrush(this.RowHeadersDefaultCellStyle.ForeColor))
                {
                    e.Graphics.DrawString(Convert.ToString(e.RowIndex + 1, System.Globalization.CultureInfo.CurrentUICulture), this.RowHeadersDefaultCellStyle.Font, brush, e.RowBounds.Location.X + 7, e.RowBounds.Location.Y + 2);

                }
            }
            else
            {
                this.RowHeadersWidth = 30;
            }
            base.OnRowPostPaint(e);
        }
        void EnableFilter_Click(object sender, EventArgs e)
        {
            try
            {
                //this.clsROGContextmenu.Hide();
                this.Cursor = Cursors.WaitCursor;
                if (EnableFilter.Text == "Enable Filter")
                {
                    EnableDataFiltering = true;
                    EnableFilter.Text = "Disable Filter";
                }
                else
                {
                    EnableDataFiltering = false;
                    EnableFilter.Text = "Enable Filter";
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private bool _EnableDataFiltering = false;

        public bool EnableDataFiltering
        {
            get { return _EnableDataFiltering; }
            set
            {
                _EnableDataFiltering = value;

                if (value)
                {
                    string RefErr = "";
                    DesignSeachGrid(this, ref RefErr);
                    clsReadOnlyGrid_BindingContextChanged(null, null);
                }

                if (!value)
                {
                    this.RemoveFilter();
                    this.Invalidate();
                }
            }
        }
        #region RemoveFilter
        public void RemoveFilter()
        {
            //AutoFilterHeader.RemoveFilter(this);
            foreach (DataGridViewColumn col in this.Columns)
            {
                Type t = col.GetType();
                if (t.Name.ToString() == "DataGridViewButtonColumn")
                    continue;
                AutoFilterHeader header = (AutoFilterHeader)col.HeaderCell;
                col.HeaderCell = new NormalHeaderCell(header);
            }

            // Resize the columns to fit their contents.
            this.AutoResizeColumns();
        }
        #endregion


        void clsReadOnlyGrid_MouseDown(object sender, MouseEventArgs e)
        {
            HitTestInfo info = this.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right && info.RowIndex >= 0 && info.ColumnIndex >= 0)
            {
                if (ContextMenuVisible)
                {
                    Point CntxtMenuLocation = this.PointToScreen(new Point(e.X, e.Y));

                    //** Context Menu Not Shown //As told By ArunavaB.
                    //clsROGContextmenu.Show(CntxtMenuLocation);
                }
                return;
            }
            else
            {
                //** Context Menu Not Shown //As told By ArunavaB.
                //clsROGContextmenu.Hide();
            }
            if (e.Button == MouseButtons.Left)
            {
                //if (info.Type == DataGridViewHitTestType.ColumnHeader)
                //{
                //    this.ColumnHeaderMouseclickevent(info.ColumnIndex);
                //    return;
                //}
                if (info.Type == DataGridViewHitTestType.RowHeader || info.Type == DataGridViewHitTestType.Cell)
                {
                    this.CurrentRowIndex = info.RowIndex;

                }
            }

            //base.OnMouseDown(e);
        }

        void clsReadOnlyGrid_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Alt && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
            //{
            //    AutoFilterHeader filterCell =
            //        this.CurrentCell.OwningColumn.HeaderCell as
            //        AutoFilterHeader;
            //    if (filterCell != null)
            //    {
            //        filterCell.ShowDropDownList();
            //        e.Handled = true;
            //    }
            //}
        }

        void clsReadOnlyGrid_BindingContextChanged(object sender, EventArgs e)
        {
            //if (BindingCompleted & EnableExcelStyleFiltering)
            //{
            //    if (this.DataSource == null)
            //    {
            //        return;
            //    }

            //    // Add the AutoFilter header cell to each column.
            if (EnableDataFiltering)
            {
                foreach (DataGridViewColumn col in this.Columns)
                {
                    Type t = col.GetType();
                    if (t.Name.ToString() == "DataGridViewButtonColumn")
                        continue;
                    col.HeaderCell = new
                        AutoFilterHeader(col.HeaderCell);
                }
            }

            //    // Resize the columns to fit their contents.
            //    this.AutoResizeColumns();
            //    //base.OnBindingContextChanged(e);
            //}
        }

        #region Color Variables & Properties
        private Color SelectedRowColorStart = System.Drawing.Color.FromArgb(255, 213, 103);
        public Color CstmSelectedRowColorStart
        {
            get { return SelectedRowColorStart; }
            set { SelectedRowColorStart = value; Invalidate(); }
        }

        private Color SelectedRowColorEnd = System.Drawing.Color.FromArgb(255, 228, 145);
        public Color CstmSelectedRowColorEnd
        {
            get { return SelectedRowColorEnd; }
            set { SelectedRowColorEnd = value; Invalidate(); }
        }

        private Color NormalRowColorStart = System.Drawing.Color.FromArgb(255, 255, 255);
        public Color CstmNormalRowColorStart
        {
            get { return NormalRowColorStart; }
            set { NormalRowColorStart = value; Invalidate(); }
        }

        private Color NormalRowColorEnd = System.Drawing.Color.FromArgb(255, 255, 255);
        public Color CstmNormalRowColorEnd
        {
            get { return NormalRowColorEnd; }
            set { NormalRowColorEnd = value; Invalidate(); }
        }

        private Color AlternateRowColorStart = System.Drawing.Color.FromArgb(201, 220, 234);
        public Color CstmAlternateRowColorStart
        {
            get { return AlternateRowColorStart; }
            set { AlternateRowColorStart = value; Invalidate(); }
        }
        
        private Color AlternateRowColorEnd = System.Drawing.Color.FromArgb(237, 244, 248);
        public Color CstmAlternateRowColorEnd
        {
            get { return AlternateRowColorEnd; }
            set { AlternateRowColorEnd = value; Invalidate(); }
        }

        private LinearGradientMode RowsColorGradientMode = LinearGradientMode.Vertical;
        public LinearGradientMode CstmRowsColorGradientMode
        {
            get { return RowsColorGradientMode; }
            set { RowsColorGradientMode = value; Invalidate(); }
        }

        private Color SelectedCellColorStart = System.Drawing.Color.FromArgb(255, 213, 103);
        public Color CstmSelectedCellColorStart
        {
            get { return SelectedCellColorStart; }
            set { SelectedCellColorStart = value; Invalidate(); }
        }

        private Color SelectedCellColorEnd = System.Drawing.Color.FromArgb(255, 255, 255);
        public Color CstmSelectedCellColorEnd
        {
            get { return SelectedCellColorEnd; }
            set { SelectedCellColorEnd = value; Invalidate(); }
        }

        private LinearGradientMode CellsColorGradientMode = LinearGradientMode.Horizontal;
        public LinearGradientMode CstmCellsColorGradientMode
        {
            get { return CellsColorGradientMode; }
            set { CellsColorGradientMode = value; Invalidate(); }
        }

        private bool EnableSelectedCellColoring = true;
        public bool CstmEnableSelectedCellColors
        {
            get { return EnableSelectedCellColoring; }
            set { EnableSelectedCellColoring = value; }
        }
        #endregion







        //Adding For New Component Debanjan

        protected override void OnRowPrePaint(DataGridViewRowPrePaintEventArgs e)
        {

            if (e.RowIndex % 2 == 0)
            {
                Rectangle rowBounds = new Rectangle(
                       this.RowHeadersWidth, e.RowBounds.Top,
                       this.Columns.GetColumnsWidth(
                           DataGridViewElementStates.Visible) -
                       this.HorizontalScrollingOffset + 1,
                       e.RowBounds.Height);

                // Paint the custom selection background.
                if (this.Rows[e.RowIndex].Selected)
                {
                    using (Brush backbrush =
                    new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
                        SelectedRowColorStart,
                        SelectedRowColorEnd,
                        RowsColorGradientMode))
                    {
                        e.Graphics.FillRectangle(backbrush, rowBounds);
                    }
                    return;
                }
                using (Brush backbrush =
                    new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
                        NormalRowColorStart,
                        NormalRowColorEnd,
                        RowsColorGradientMode))
                {
                    e.Graphics.FillRectangle(backbrush, rowBounds);
                }


            }
            else
            {
                Rectangle rowBounds = new Rectangle(
                      this.RowHeadersWidth, e.RowBounds.Top,
                      this.Columns.GetColumnsWidth(
                          DataGridViewElementStates.Visible) -
                      this.HorizontalScrollingOffset + 1,
                      e.RowBounds.Height);

                // Paint the custom selection background.
                if (this.Rows[e.RowIndex].Selected)
                {
                    using (Brush backbrush =
                    new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
                        SelectedRowColorStart,
                        SelectedRowColorEnd,
                        RowsColorGradientMode))
                    {
                        e.Graphics.FillRectangle(backbrush, rowBounds);
                    }
                    return;
                }
                using (Brush backbrush =
                    new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
                    //Color.FromArgb(203, 225, 252),
                        AlternateRowColorStart,
                        AlternateRowColorEnd,
                        RowsColorGradientMode))
                {
                    e.Graphics.FillRectangle(backbrush, rowBounds);
                }
            }

            base.OnRowPrePaint(e);
        }

        #region Old Codes
        //protected override void OnRowPrePaint(DataGridViewRowPrePaintEventArgs e)
        //{

        //    if (e.RowIndex % 2 == 0)
        //    {
        //        Rectangle rowBounds = new Rectangle(
        //               this.RowHeadersWidth, e.RowBounds.Top,
        //               this.Columns.GetColumnsWidth(
        //                   DataGridViewElementStates.Visible) -
        //               this.HorizontalScrollingOffset + 1,
        //               e.RowBounds.Height);

        //        // Paint the custom selection background.
        //        if (this.Rows[e.RowIndex].Selected)
        //        {
        //            using (Brush backbrush =
        //            new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
        //                System.Drawing.Color.FromArgb(255, 213, 103),
        //                System.Drawing.Color.FromArgb(255, 228, 145),
        //                System.Drawing.Drawing2D.LinearGradientMode.Vertical))
        //            {
        //                e.Graphics.FillRectangle(backbrush, rowBounds);
        //            }
        //            return;
        //        }
        //        using (Brush backbrush =
        //            new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
        //                System.Drawing.Color.FromArgb(255, 255, 255),
        //                System.Drawing.Color.FromArgb(255, 255, 255),
        //                System.Drawing.Drawing2D.LinearGradientMode.Vertical))
        //        {
        //            e.Graphics.FillRectangle(backbrush, rowBounds);
        //        }


        //    }
        //    else
        //    {
        //        Rectangle rowBounds = new Rectangle(
        //              this.RowHeadersWidth, e.RowBounds.Top,
        //              this.Columns.GetColumnsWidth(
        //                  DataGridViewElementStates.Visible) -
        //              this.HorizontalScrollingOffset + 1,
        //              e.RowBounds.Height);

        //        // Paint the custom selection background.
        //        if (this.Rows[e.RowIndex].Selected)
        //        {
        //            using (Brush backbrush =
        //            new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
        //                System.Drawing.Color.FromArgb(255, 213, 103),
        //                System.Drawing.Color.FromArgb(255, 228, 145),
        //                System.Drawing.Drawing2D.LinearGradientMode.Vertical))
        //            {
        //                e.Graphics.FillRectangle(backbrush, rowBounds);
        //            }
        //            return;
        //        }
        //        using (Brush backbrush =
        //            new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
        //            //Color.FromArgb(203, 225, 252),
        //                System.Drawing.Color.FromArgb(201, 220, 234),
        //                System.Drawing.Color.FromArgb(237, 244, 248),
        //                System.Drawing.Drawing2D.LinearGradientMode.Vertical))
        //        {
        //            e.Graphics.FillRectangle(backbrush, rowBounds);
        //        }
        //    }

        //    base.OnRowPrePaint(e);
        //}
        #endregion Old Codes

        void withoutheader_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
                if (this.GetCellCount(DataGridViewElementStates.Selected) > 0)
                {
                    System.Windows.Forms.Clipboard.SetDataObject(this.GetClipboardContent());
                }

            }
            catch (Exception ex)
            {
            }
        }
        

        void withheader_Click(object sender, EventArgs e)
        {
            try
            {

                this.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                if (this.GetCellCount(DataGridViewElementStates.Selected) > 0)
                {
                    System.Windows.Forms.Clipboard.SetDataObject(this.GetClipboardContent());
                }

            }
            catch (Exception ex)
            {
            }

        }



        //protected override void OnPaint(PaintEventArgs pe)
        //{
        //    // TODO: Add custom paint code here

        //    // Calling the base class OnPaint

        //    base.OnPaint(pe);
        //}


        //properties from the past

        public Font CaptionFont
        {
            get
            {
                return (this.captionfont);
            }
            set
            {
                this.captionfont = value;
            }


        }


        public Font HeaderFont
        {
            get
            {
                return (this.ColumnHeadersDefaultCellStyle.Font);
            }
            set
            {
                this.ColumnHeadersDefaultCellStyle.Font = value;
            }

        }

        public Color HeaderForeColor
        {
            get
            {
                return (this.ColumnHeadersDefaultCellStyle.ForeColor);
            }
            set
            {
                this.ColumnHeadersDefaultCellStyle.ForeColor = value;
            }

        }


        public int RowHeaderWidth
        {
            get
            {
                return (this.RowHeadersWidth);
            }
            set
            {
                this.RowHeadersWidth = value;
            }

        }




        public bool CaptionVisible
        {
            get
            {
                return this.captionvisible;
            }
            set
            {
                this.captionvisible = value;
            }
        }


        //public bool AllowSorting
        //{
        //    get
        //    {
        //        return false;
        //    }
        //    set
        //    {

        //    }
        //}

        public string CaptionText
        {
            get
            {
                return (this.captiontext);
            }
            set
            {
                this.captiontext = value;

            }
        }

        public Color CaptionForeColor
        {
            set
            {
            }
        }

        public Color AlternatingBackColor
        {
            get
            {
                return (this.AlternatingRowsDefaultCellStyle.BackColor);
            }
            set
            {
                this.AlternatingRowsDefaultCellStyle.BackColor = value;
            }
        }


        public int VisibleRowCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < this.Rows.Count; i++)
                {
                    if (this.Rows[i].Visible)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        //**************************

        //methods from the past

        public bool IsSelected(int RowNumber)
        {
            try
            {
                foreach (DataGridViewRow dr in this.SelectedRows)
                {
                    if (dr.Index == RowNumber)
                    {
                        return true;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return false;
            }
        }



        public void Select(int RowNumber)
        {
            try
            {
                this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.Rows[RowNumber].Selected = true;

                // this.Columns[this.CurrentCell.ColumnIndex].State = DataGridViewElementStates.ReadOnly ;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }



        //**********************

        public bool MultiLine
        {
            get { return boolMutliLine; }
            set { boolMutliLine = value; }
        }


        public bool DefaultSort
        {
            get { return _DefaultSort; }
            set { _DefaultSort = value; }
        }


        public int SortColumn
        {
            get { return _SortCol; }
            set { _SortCol = value; }
        }


        public bool LevelSearch
        {
            get { return boolLevelSearch; }
            set { boolLevelSearch = value; }
        }


        public int LevelHierarchyColumn1
        {
            get { return _ReqColNo1; }
            set { _ReqColNo1 = value; }
        }


        public int LevelHierarchyColumn2
        {
            get { return _ReqColNo2; }
            set { _ReqColNo2 = value; }
        }


        public string LevelHierarchy
        {
            get
            {
                if (arlHierarchyLevel != null)
                {
                    int i;
                    strLevelHier = "";
                    for (i = 0; i < arlHierarchyLevel.Count; i++)
                        strLevelHier += arlHierarchyLevel[i].ToString() + "--";
                    if (strLevelHier != "")
                        strLevelHier = strLevelHier.Substring(0, strLevelHier.Length - 2);
                }
                else
                    strLevelHier = "";

                return strLevelHier;
            }
        }


        public string ValueHierarchy
        {
            get
            {
                if (arlHierarchyValue != null)
                {
                    int i;
                    strValueHier = "";
                    for (i = 0; i < arlHierarchyValue.Count; i++)
                        strValueHier += arlHierarchyValue[i].ToString() + "--";
                    if (strValueHier != "")
                        strValueHier = strValueHier.Substring(0, strValueHier.Length - 2);
                }
                else
                    strValueHier = "";

                return strValueHier;
            }
        }





        public clsSearchGrid AssociatedSearchGrid
        {
            set { _dgThisSearchGrid = value; }
            get { return _dgThisSearchGrid; }
        }


        public string MessageID
        {
            get { return strMsgID; }
            set { strMsgID = value; }
        }


        public int MaxLevel
        {
            get { return _MaxLevel; }
        }


        public int CurrentLevel
        {
            get { return _CurrentLevel; }
        }


        public string CurrentID
        {
            get
            {
                if (arlPrimKey == null)
                    return "";
                return arlPrimKey[_CurrentLevel - 2].ToString();
            }
        }


        public string[] LangIDs
        {
            get
            {
                return strLangID;
            }
            set
            {
                strLangID = value;
            }
        }

        public new object this[int RowNumber, int ColNumber]
        {
            get
            {

                DataRowView v = ((DataRowView)this.BindingContext[this.DataSource, this.DataMember].Current);
                return v.DataView[RowNumber][ColNumber];

            }
            set
            {

                DataRowView v = ((DataRowView)this.BindingContext[this.DataSource, this.DataMember].Current);
                v.DataView[RowNumber][ColNumber] = value;
                v.EndEdit();
            }
        }




        public void ColumnHeaderMouseclickevent(int columnindex)
        {

            string sortorder = this.SortOrder.ToString();
            DataGridViewColumn col = this.Columns[columnindex];
            if (col.DataGridView.SortOrder.ToString().ToUpper() == "ASCENDING")
            {

                this.Sort(col, ListSortDirection.Descending);
            }
            else
            {
                if (col.DataGridView.SortOrder.ToString().ToUpper() == "DESCENDING")
                {
                    this.Sort(col, ListSortDirection.Ascending);

                }
                else
                {
                    this.Sort(col, ListSortDirection.Descending);
                }
            }


        }

        protected override void OnDataSourceChanged(EventArgs e)
        {
            try
            {
                CurrencyManager cm = (CurrencyManager)this.BindingContext[this.DataSource];
                //CurrencyManager cm = (CurrencyManager)this.BindingContext[_BSource.DataSource];
                ((DataView)cm.List).AllowNew = false;
                base.OnDataSourceChanged(e);
            }
            catch
            {
 
            }
        }

        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    HitTestInfo info = this.HitTest(e.X, e.Y);
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        clsROGContextmenu.Show();
        //        return;
        //    }

        //    if (e.Button == MouseButtons.Left)
        //    {
        //        if (info.Type == DataGridViewHitTestType.ColumnHeader)
        //        {
        //            this.ColumnHeaderMouseclickevent(info.ColumnIndex);
        //            return;
        //        }
        //        if (info.Type == DataGridViewHitTestType.RowHeader || info.Type == DataGridViewHitTestType.Cell)
        //        {
        //            this.CurrentRowIndex = info.RowIndex;

        //        }
        //    }

        //    base.OnMouseDown(e);
        //}


        public void SetParam(string ProcParamName, string ProcParamVal)
        {
            ProcParamNames.Add(ProcParamName);
            ProcParamValues.Add(ProcParamVal.Trim());
        }



        /// <summary>
        /// Clear the Procedure Parametersvfor fresh entry
        /// </summary>
        public void ClearParams()
        {
            ProcParamNames.Clear();
            ProcParamValues.Clear();
        }


        private bool blHandleGridSort = true;
        public bool HandleGridSort
        {
            get { return blHandleGridSort; }
            set { blHandleGridSort = value; }
        }


        #region Design Grid
        //private Hashtable objSortHash = new Hashtable();
        public bool DesignGrid(string strProcedureName, ref string strErr)
        {
            Hashtable objSortHash = new Hashtable();
            try
            {
                foreach (DataGridViewColumn objCol in this.Columns)
                {
                    if (objCol.HeaderText.ToString().ToUpper().StartsWith("MASK_"))
                        continue;
                    AutoFilterHeader objHeader = (AutoFilterHeader)objCol.HeaderCell;
                    objSortHash.Add(objCol.HeaderText.ToString().ToUpper(), objHeader.SortGlyphDirection);
                }
                this.AutoGenerateColumns = false;
                strProcName = strProcedureName;

                Query objQuery = new Query();

                if (ProcParamNames.Count > 0)
                {
                    string[] arrStrProcParamNames = new string[ProcParamNames.Count];
                    string[] arrStrProcParamValues = new string[ProcParamNames.Count];
                    for (int i = 0; i < ProcParamNames.Count; i++)
                    {
                        arrStrProcParamNames[i] = ProcParamNames[i].ToString();
                        arrStrProcParamValues[i] = ProcParamValues[i].ToString();
                    }
                    objQuery.SetInputParameterNames(strProcedureName, arrStrProcParamNames);
                    objQuery.SetInputParameterValues(arrStrProcParamValues);
                }
                objQuery.SetOutputParameterNames(strProcedureName, "CRITERIA");

                DataSet ds = objQuery.ExecuteQueryProcedure(strProcedureName);

                dtReadOnlyGrid = ds.Tables[0].Copy();
                ds.Dispose();
                //Changing For New Component
                //_BSource = new BindingSource(dtReadOnlyGrid, null);
                intColCount = dtReadOnlyGrid.Columns.Count;
                strColNames = new string[intColCount];

                for (int i = 0; i < intColCount; i++)
                    strColNames[i] = dtReadOnlyGrid.Columns[i].ColumnName;

                string[] strHeadings = new string[intColCount];

                ////**** Creating the Header text by replacing the Underscore(_)
                ////**** Symbol with the Space ( ).

                clsLanguage objLang = new clsLanguage();

                for (int i = 0; i < intColCount; i++)
                {
                    strHeadings[i] = strColNames[i].Replace("_", " ");

                    if (strLangID != null && strColNames[i].IndexOf("MASK_") < 0)
                    {
                        if (strLangID.Length >= intColCount)
                        {
                            string strTmp = objLang.LanguageString(strLangID[i]);
                            if (strTmp == "*****")
                                strHeadings[i] = "** " + strHeadings[i];
                            else
                                strHeadings[i] = strTmp;
                        }
                        else
                        {
                            if (strLangID.Length > i)
                            {
                                string strTmp = objLang.LanguageString(strLangID[i]);

                                if (strTmp == "*****")
                                    strHeadings[i] = "** " + strHeadings[i];
                                else
                                    strHeadings[i] = strTmp;
                            }
                            else
                            {
                                strHeadings[i] = "** " + strHeadings[i];
                            }
                        }
                    }
                    else
                    {
                        strHeadings[i] = "** " + strHeadings[i];
                    }
                }

                if (objLang != null)
                    objLang.Dispose();




                ////**** If the Default design is false then setting Colors for that Grid

                //this.reEntrent = true;
                //Changing For New Component
                // this.DataSource = dtReadOnlyGrid;
                //this.DataSource = dtReadOnlyGrid;
                //this.reEntrent = false;
                if (SetDefault == true)
                {

                    //DataGridViewCellStyle headerstyle=new DataGridViewCellStyle();
                    //headerstyle.BackColor= System.Drawing.Color.Linen;
                    //headerstyle.ForeColor=System.Drawing.Color.Navy;
                    //this.GridColor = System.Drawing.Color.Silver;
                    //this.ColumnHeadersDefaultCellStyle.ApplyStyle(headerstyle);

                }
                else
                {
                    //this.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                    //this.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Navy;
                    //this.GridColor = System.Drawing.Color.Silver;

                }

                ////**** To incorporate the Font the in DataGrid Cell.

                intVisibleColumnCount = 0;
                if (this.Columns.Count > 0)
                {
                    this.Columns.Clear();
                }
                for (int i = 0; i < intColCount; i++)
                {
                    if (strColNames[i].ToUpper().IndexOf("MASK") == 0)
                        continue;

                    intVisibleColumnCount++;
                    DataGridViewTextBoxColumn dgTBCol = new DataGridViewTextBoxColumn();
                    dgTBCol.DataPropertyName = dtReadOnlyGrid.Columns[i].ColumnName;
                    dgTBCol.HeaderText = strHeadings[i];
                    this.Columns.Add(dgTBCol);

                    //this.Columns[this.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;



                }
                //Changing For New Component

                this.ScrollBars = ScrollBars.Both;
                //this.AutoSize = true;
                this.DataSource = dtReadOnlyGrid;

                if (EnableDataFiltering)
                {
                    //if (PreviousSearchStub.Count > 0)
                    //{
                    //    MessageBox.Show(PreviousSearchStub.Count.ToString());
                    //}
                    string RefErr = "";
                    DesignSeachGrid(this, ref RefErr);
                    clsReadOnlyGrid_BindingContextChanged(null, null);

                    SearchStubCollection<SearchStub> objTempSearchStub = PreviousSearchStub;
                    int iColIndex = -1;
                    foreach (DataGridViewColumn objCol in this.Columns)
                    {
                        if (objCol.HeaderText.ToString().ToUpper().StartsWith("MASK_"))
                            continue;
                        iColIndex++;
                        if (!objTempSearchStub.Contains(new SearchStub(objCol.HeaderText)))
                            continue;
                        if (!objTempSearchStub[objTempSearchStub.IndexOf(new SearchStub(objCol.HeaderText))].Filtered)
                            continue;
                        ((AutoFilterHeader)objCol.HeaderCell).PopUpText.SearchText = objTempSearchStub[objTempSearchStub.IndexOf(new SearchStub(objCol.HeaderText))].SearchParam;
                        //AutoFilterHeader objFilter = (AutoFilterHeader)objCol.HeaderCell;
                        //objFilter.PopUpText.SearchText = PreviousSearchStub[PreviousSearchStub.IndexOf(new SearchStub(objCol.HeaderText))].SearchParam;
                        ((AutoFilterHeader)objCol.HeaderCell).PerformSearchInvoke(objTempSearchStub[objTempSearchStub.IndexOf(new SearchStub(objCol.HeaderText))].SearchParam, iColIndex);

                    }

                    foreach (DataGridViewColumn objCol in this.Columns)
                    {
                        if (objCol.HeaderText.ToString().ToUpper().StartsWith("MASK_"))
                            continue;
                        if (objSortHash.ContainsKey(objCol.HeaderText.ToString().ToUpper()))
                        {
                            SortOrder objSort = (SortOrder)objSortHash[objCol.HeaderText.ToString().ToUpper()];
                            if (objSort == SortOrder.None)
                                continue;

                            if (objSort == SortOrder.Ascending)
                            {
                                this.Sort(objCol, ListSortDirection.Ascending);
                                ((AutoFilterHeader)objCol.HeaderCell).SortGlyphDirection = SortOrder.Ascending;
                            }

                            if (objSort == SortOrder.Descending)
                            {
                                this.Sort(objCol, ListSortDirection.Descending);
                                ((AutoFilterHeader)objCol.HeaderCell).SortGlyphDirection = SortOrder.Descending;
                            }

                            if (this.Tag != null && objSort!= SortOrder.None)
                            {
                                iColIndex = get_actual_colum_index(objCol.Index, (DataTable)this.Tag);
                                if (objSort == SortOrder.Ascending)
                                    ((DataTable)this.Tag).DefaultView.Sort = ((DataTable)this.Tag).Columns[iColIndex].ColumnName + " ASC";
                                else
                                    ((DataTable)this.Tag).DefaultView.Sort = ((DataTable)this.Tag).Columns[iColIndex].ColumnName + " DESC";
                            }
                        }

                    }

                    
                    strErr = RefErr;
                    if (RefErr == "")
                        return true;
                    else
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                strErr = ex.Message + " - " + ex.Source;
                //return false;
                if (strErr == "Operation is not valid because it results in a reentrant call to the SetCurrentCellAddressCore function. - System.Windows.Forms")
                {
                    strErr = "";
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private Hashtable iPrimaryKeyColumnIndices;
        public Hashtable PrimaryKeyColumnIndices
        {
            get { return iPrimaryKeyColumnIndices; }
            set { iPrimaryKeyColumnIndices = value; }
        }

        private Hashtable objHashPrimaryKeyColumnNames;
        public Hashtable PrimaryKeyColumnNames
        {
            get { return objHashPrimaryKeyColumnNames; }
            set { objHashPrimaryKeyColumnNames = value; }
        }
        private bool blPerformSpclActionAftrSort = false;
        public void SetPrimaryKeyValues()
        {
            List<string> keys = new List<string>();
            foreach (System.Collections.DictionaryEntry de in objHashPrimaryKeyColumnNames)
                keys.Add(de.Key.ToString());

            if (this.Rows.Count != 0 && this.CurrentCell != null)
            {
                foreach (string key in keys)
                {
                    objHashPrimaryKeyColumnNames[key.ToString()] = this[this.CurrentCell.RowIndex, Convert.ToInt32(key.ToString())];
                }
                blPerformSpclActionAftrSort = true;
            }
            else
            {
                blPerformSpclActionAftrSort = false;
            }
        }

        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            if (blPerformSpclActionAftrSort)
            {
                //DataTable dtSource = null;
                //if (Tag != null)
                //{
                //    dtSource = ((DataTable)this.Tag).DefaultView.ToTable();
                //}
                //else
                //{
                //    dtSource = (DataTable)this.DataSource;
                //}

                //string strExpression = "";
                //int iLoop = 0;
                //foreach (object key in iPrimaryKeyColumnIndices.Keys)
                //{
                //    if (objHashPrimaryKeyColumnNames[key] == null)
                //        continue;
                //    strExpression += iPrimaryKeyColumnIndices[key].ToString() + "= '" + objHashPrimaryKeyColumnNames[key].ToString() + "'";
                //    if (iLoop < (iPrimaryKeyColumnIndices.Keys.Count - 1))
                //        strExpression += " AND ";
                //    iLoop++;
                //}

                //if (strExpression != "" && dtSource != null)
                //{
                //    int iSelected = dtSource.Rows.IndexOf(dtSource.Select(strExpression)[0]);
                //    this.FirstDisplayedScrollingRowIndex = iSelected;
                //    this.Select(iSelected);
                //}

            }
            base.OnDataBindingComplete(e);
        }
        public int get_actual_colum_index(int index, DataTable _dtFullData)
        {
            try
            {

                int exactcolumn = 0;
                int store = 0; ;
                for (int i = 0; i < _dtFullData.Columns.Count; i++)
                {
                    if (_dtFullData.Columns[i].ColumnName.StartsWith("MASK"))
                    {

                        continue;

                    }
                    else
                    {
                        if (exactcolumn == index)
                        {
                            store = i;
                            break;
                        }
                        exactcolumn++;
                    }
                }
                return store;

            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public BindingSource _BSource;
        private bool BindingCompleted = false;
        public bool DesignGridFromDataTable(DataTable tableToBeDesigned, ref string strErr)
        {
            try
            {
                //Changing For New Component
                // _BSource = new BindingSource(tableToBeDesigned, null);

                this.AutoGenerateColumns = false;
                strProcName = null;
                intColCount = tableToBeDesigned.Columns.Count;
                strColNames = new string[intColCount];
                string[] strHeadings = new string[intColCount];
                for (int i = 0; i < intColCount; i++)
                {
                    strColNames[i] = tableToBeDesigned.Columns[i].ColumnName;
                }

                #region Comments
                ////**** Creating the Header text by replacing the Underscore(_)
                ////**** Symbol with the Space ( ).
                #endregion Comments
                #region Header Text
                for (int i = 0; i < intColCount; i++)
                {
                    string tmp = "";
                    string[] m_strSplit = strColNames[i].Split(new char[] { '_' }, strColNames[i].Length);
                    for (int m = 0; m <= m_strSplit.GetUpperBound(0); m++)
                        tmp = tmp.Trim() + " " + m_strSplit[m].Trim();
                    strHeadings[i] = tmp;

                    if (strLangID != null)
                    {
                        if (strLangID.Length >= intColCount)
                        {
                            clsLanguage objLang = new clsLanguage();

                            string strTmp = objLang.LanguageString(strLangID[i]);
                            if (strTmp == "*****")
                                strHeadings[i] = "** " + strHeadings[i];
                            else
                                strHeadings[i] = strTmp;

                            objLang.Dispose();
                        }
                        else
                        {
                            if (strLangID.Length > i)
                            {
                                clsLanguage objLang = new clsLanguage();
                                string strTmp = objLang.LanguageString(strLangID[i]);

                                if (strTmp == "*****")
                                    strHeadings[i] = "** " + strHeadings[i];
                                else
                                    strHeadings[i] = strTmp;

                                objLang.Dispose();
                            }
                            else
                            {
                                strHeadings[i] = "** " + strHeadings[i];
                            }
                        }
                    }
                    else
                    {
                        strHeadings[i] = "** " + strHeadings[i];
                    }
                }

                #endregion
                ////**** Creating a new Table Style to include the TextBoxes in the Datagrid

                ////**** If the Default design is false then setting Colors for that Grid
                //Changing For New Component
                //this.DataSource = _BSource;

                this.DataSource = tableToBeDesigned;
                //this.DataSource = tableToBeDesigned;
                #region Set Default Not in Use
                if (SetDefault == true)
                {

                    //DataGridViewCellStyle headerstyle = new DataGridViewCellStyle();
                    //headerstyle.BackColor = System.Drawing.Color.Linen;
                    //headerstyle.ForeColor = System.Drawing.Color.Navy;
                    //this.GridColor = System.Drawing.Color.Silver;
                    //this.ColumnHeadersDefaultCellStyle.ApplyStyle(headerstyle);

                }
                else
                {
                    //this.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                    //this.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Navy;
                    //this.GridColor = System.Drawing.Color.Silver;

                }
                #endregion Set Default Not in Use
                ////**** To incorporate the Font the in DataGrid Cell.

                intVisibleColumnCount = 0;
                //Added By Debajyoti
                if (this.Columns.Count > 0)
                {

                    this.Columns.Clear();

                }

                for (int i = 0; i < intColCount; i++)
                {
                    if (strColNames[i].ToUpper().IndexOf("MASK") == 0)
                        continue;

                    intVisibleColumnCount++;

                    DataGridViewTextBoxColumn dgTBCol = new DataGridViewTextBoxColumn();
                    dgTBCol.DataPropertyName = tableToBeDesigned.Columns[i].ColumnName;
                    dgTBCol.HeaderText = strHeadings[i];
                    this.Columns.Add(dgTBCol);


                }
                //BindingCompleted = true;
                //this.Font = new Font(this.Font, FontStyle.Regular);

                this.AutoSize = true;
                string RefErr = "";
                DesignSeachGrid(this, ref RefErr);
                clsReadOnlyGrid_BindingContextChanged(null, null);
                strErr = RefErr;
                if (RefErr == "")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                strErr = ex.Message + " - " + ex.Source;
                return false;
            }
        }





        public bool DesignGridFromSQL(string sqlParam, ref string strErr)
        {
            try
            {
                strSQL = sqlParam;

                Query objQuery = new Query();

                DataSet ds = objQuery.ExecuteQueryCommand(strSQL);

                dtReadOnlyGrid = ds.Tables[0].Copy();
                dtReadOnlyGrid.TableName = "Table";
                ds.Dispose();
                //Changing For New Component
                //_BSource = new BindingSource(dtReadOnlyGrid, null);

                intColCount = dtReadOnlyGrid.Columns.Count;
                strColNames = new string[intColCount];

                for (int i = 0; i < intColCount; i++)
                    strColNames[i] = dtReadOnlyGrid.Columns[i].ColumnName;

                ///***Displaying data from a Table to a Grid and aligning the Columns***
                intColCount = dtReadOnlyGrid.Columns.Count;
                strColNames = new string[intColCount];

                for (int i = 0; i < intColCount; i++)
                    strColNames[i] = dtReadOnlyGrid.Columns[i].ColumnName;

                string[] strHeadings = new string[intColCount];

                ////**** Creating the Header text by replacing the Underscore(_)
                ////**** Symbol with the Space ( ).

                clsLanguage objLang = new clsLanguage();

                for (int i = 0; i < intColCount; i++)
                {
                    strHeadings[i] = strColNames[i].Replace("_", " ");

                    if (strLangID != null && strColNames[i].IndexOf("MASK_") < 0)
                    {
                        if (strLangID.Length >= intColCount)
                        {
                            string strTmp = objLang.LanguageString(strLangID[i]);
                            if (strTmp == "*****")
                                strHeadings[i] = "** " + strHeadings[i];
                            else
                                strHeadings[i] = strTmp;
                        }
                        else
                        {
                            if (strLangID.Length > i)
                            {
                                string strTmp = objLang.LanguageString(strLangID[i]);

                                if (strTmp == "*****")
                                    strHeadings[i] = "** " + strHeadings[i];
                                else
                                    strHeadings[i] = strTmp;
                            }
                            else
                            {
                                strHeadings[i] = "** " + strHeadings[i];
                            }
                        }
                    }
                    else
                    {
                        strHeadings[i] = "** " + strHeadings[i];
                    }
                }

                if (objLang != null)
                    objLang.Dispose();

                ////**** Creating a new Table Style to include the TextBoxes in the Datagrid
                //this.DataSource =_BSource;
                this.DataSource = dtReadOnlyGrid;
                if (SetDefault == true)
                {

                    //DataGridViewCellStyle headerstyle = new DataGridViewCellStyle();
                    //headerstyle.BackColor = System.Drawing.Color.Linen;
                    //headerstyle.ForeColor = System.Drawing.Color.Navy;
                    //this.GridColor = System.Drawing.Color.Silver;
                    //this.ColumnHeadersDefaultCellStyle.ApplyStyle(headerstyle);

                }
                else
                {
                    //this.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                    //this.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Navy;
                    //this.GridColor = System.Drawing.Color.Silver;

                }

                ////**** To incorporate the Font the in DataGrid Cell.

                intVisibleColumnCount = 0;
                if (this.Columns.Count > 0)
                {
                    this.Columns.Clear();
                }
                for (int i = 0; i < intColCount; i++)
                {
                    if (strColNames[i].ToUpper().IndexOf("MASK") == 0)
                        continue;

                    intVisibleColumnCount++;

                    DataGridViewTextBoxColumn dgTBCol = new DataGridViewTextBoxColumn();
                    dgTBCol.DataPropertyName = dtReadOnlyGrid.Columns[i].ColumnName;
                    dgTBCol.HeaderText = strHeadings[i];
                    this.Columns.Add(dgTBCol);



                }
                //*****************************
                AutoSizeCol();
                if (boolLevelSearch == true)
                {
                    //this.KeyDown += new KeyEventHandler(BackSpaceKeyDown);
                    arlHierarchyLevel = new ArrayList();
                    arlHierarchyValue = new ArrayList();
                    arlPrimKey = new ArrayList();
                }

                if (EnableDataFiltering)
                {
                    string RefErr = "";
                    DesignSeachGrid(this, ref RefErr);
                    clsReadOnlyGrid_BindingContextChanged(null, null);
                    strErr = RefErr;
                    if (RefErr == "")
                        return true;
                    else
                        return false;
                }

                //***************************
                //BindingCompleted = true;
                //clsReadOnlyGrid_BindingContextChanged(null, null);
                //this.Font = new Font(this.Font, FontStyle.Regular);
                // this.AutoSize = true;
                return true;
            }
            catch (Exception ex)
            {
                strErr = ex.Message + " - " + ex.Source;
                return false;
            }
        }

        private bool _ContextMenuVisible=true;
        public bool ContextMenuVisible
        {
            get { return this._ContextMenuVisible; }
            set { this._ContextMenuVisible = value; }
        }

        public bool DesignGrid(DataGridView dgOriginal, ref string strErrMsg)
        {
            try
            {


                DataTable dtOriginal = (DataTable)dgOriginal.DataSource;
                dtReadOnlyGrid = dtOriginal.Clone();
                intColCount = dtReadOnlyGrid.Columns.Count;
                for (int i = 0; i < intVisibleColumnCount; i++)
                {
                    DataGridViewColumn column = (DataGridViewColumn)dgOriginal.Columns[i];
                    this.Columns.Add(column);
                }

                this.DataSource = dtReadOnlyGrid;
                //this.Font = new Font(this.Font, FontStyle.Regular);
                return true;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message + " - " + ex.Source;
                return false;
            }
        }




        public bool PopulateGrid(string sqlParam, ref string strErr)
        {
            try
            {
                strSQL = sqlParam;

                ///***Displaying data from a Table to a Grid and aligning the Columns***
                Query objQuery = new Query();

                DataSet ds = objQuery.ExecuteQueryCommand(strSQL);

                dtReadOnlyGrid = ds.Tables[0].Copy();
                ds.Dispose();

                intColCount = dtReadOnlyGrid.Columns.Count;
                strColNames = new string[intColCount];

                for (int i = 0; i < intColCount; i++)
                    strColNames[i] = dtReadOnlyGrid.Columns[i].ColumnName;

                this.DataSource = dtReadOnlyGrid;

                this.AutoSize = true;

                RowCount = dtReadOnlyGrid.Rows.Count;

                return true;
            }
            catch (Exception ex)
            {
                strErr = ex.Message + " - " + ex.Source;
                return false;
            }
        }


        public bool PopulateGrid(ref string strErr)
        {
            try
            {
                Query objQuery = new Query();

                string[] arrStrProcParamNames = new string[ProcParamNames.Count];
                string[] arrStrProcParamValues = new string[ProcParamNames.Count];
                for (int i = 0; i < ProcParamNames.Count; i++)
                {
                    arrStrProcParamNames[i] = ProcParamNames[i].ToString();
                    arrStrProcParamValues[i] = ProcParamValues[i].ToString();
                }
                objQuery.SetInputParameterNames(strProcName, arrStrProcParamNames);
                objQuery.SetInputParameterValues(arrStrProcParamValues);
                objQuery.SetOutputParameterNames(strProcName, "CRITERIA");

                DataSet ds = objQuery.ExecuteQueryProcedure(strProcName);

                dtReadOnlyGrid = ds.Tables[0].Copy();


                intColCount = dtReadOnlyGrid.Columns.Count;
                strColNames = new string[intColCount];

                for (int i = 0; i < intColCount; i++)
                    strColNames[i] = dtReadOnlyGrid.Columns[i].ColumnName;
                // CurrencyManager cm = (CurrencyManager)this.BindingContext[this.DataSource];
                //((DataView)cm.List).AllowNew = true;
                //((DataView)cm.List).AllowNew = false;
                this.DataBindings.Clear();
                //this.Rows.Clear();
                //this.AllowUserToAddRows = false;
                this.reEntrent = true;
                this.MultiSelect = false;
                //this.DataSource = null;
                this.DataSource = dtReadOnlyGrid;
                this.reEntrent = false;
                //this.AllowUserToAddRows = true;
                //((DataView)cm.List).AllowNew = false;
                ds.Dispose();

                this.AutoSize = true;

                RowCount = dtReadOnlyGrid.Rows.Count;
                return true;
            }
            catch (Exception ex)
            {
                strErr = ex.Message + " - " + ex.Source;
                return false;
            }
        }

        public bool reEntrent = false;

        //protected override bool SetCurrentCellAddressCore(int columnIndex, int rowIndex, bool setAnchorCellAddress, bool validateCurrentCell, bool throughMouseClick)
        //{
        //    bool rv = true;
        //    try
        //    {

        //        if (!reEntrent)
        //        {
        //            reEntrent = true;
        //            rv = base.SetCurrentCellAddressCore(columnIndex, rowIndex, setAnchorCellAddress, validateCurrentCell, throughMouseClick);
        //        }
        //        return rv;
        //    }
        //    catch
        //    {
        //        return rv;
        //    }
        //}
        void clsReadOnlyGrid_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        protected override void OnCellValidating(DataGridViewCellValidatingEventArgs e)
        {
            //base.OnCellValidating(e);
            return;
        }

        #endregion

        private void item_clicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                //this.clsROGContextmenu.Hide();


                if (e.ClickedItem.Text == "Paste")
                {
                    IDataObject d = Clipboard.GetDataObject();
                    if (d.GetDataPresent("System.String"))
                    {
                        this.CurrentCell.Value = (string)d.GetData("System.String");
                    }
                }

                if (e.ClickedItem.Text == "Export Data")
                {
                    ExportData();
                }

                if (e.ClickedItem.Text == "Color")
                {
                    ColorDialog cdialog = new ColorDialog();
                    DialogResult res = cdialog.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        this.BackgroundColor = cdialog.Color;
                        this.DefaultCellStyle.BackColor = cdialog.Color;
                        this.ColumnHeadersDefaultCellStyle.BackColor = cdialog.Color;
                        this.RowHeadersDefaultCellStyle.BackColor = cdialog.Color;
                        this.Refresh();
                    }
                }


            }
            catch (Exception ex)
            {
            }
        }


        private void ExportData()
        {
            string m_strErr = "";

            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Title = "Save File As";
                dlg.InitialDirectory = @"C:\";
                dlg.Filter = "Text Files (*.txt)|*.txt|Comma Separated Text (*.csv)|*.csv|Microsoft Excel (*.xls)|*.xls";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (dlg.FilterIndex == 1)
                    {
                        cnvTag cnv = new cnvTag(dlg.FileName.Trim());
                        DataTable m_dtTemp = (DataTable)this.DataSource;
                        if (cnv.CreateTextFile(m_dtTemp, true, ref m_strErr) == false)
                        {
                            MessageBox.Show(m_strErr);
                            return;
                        }
                    }
                    else if (dlg.FilterIndex == 2)
                    {
                        cnvTag cnv = new cnvTag(dlg.FileName.Trim());
                        DataTable m_dtTemp = (DataTable)this.DataSource;
                        if (cnv.CreateTextFile(m_dtTemp, false, ref m_strErr) == false)
                        {
                            MessageBox.Show(m_strErr);
                            return;
                        }
                    }
                    else if (dlg.FilterIndex == 3)
                    {
                        cnvTag cnv = new cnvTag(dlg.FileName.Trim());
                        if (cnv.CreateExcelFile((DataTable)this.DataSource, ref m_strErr) == false)
                        {
                            MessageBox.Show(m_strErr);
                            return;
                        }
                    }
                    dlg.Dispose();
                    MessageBox.Show("Data Exported Successfully.");
                }
            }
            catch (Exception ex)
            {
                m_strErr = ex.Source + " - " + ex.Message;
                MessageBox.Show(m_strErr);
                return;
            }
        }



        public void ShowLevelData(ReadnSearchGrid dgParam, int CountVal)
        {
            dgSearchLevel = dgParam;

            dtLevelData = FilterLevel(CountVal);

            if (dtLevelData != null)
            {
                dgParam.DataSource = dtLevelData;

                //dgParam.AssociatedSearchGrid.FullData = dtLevelData;

                dgParam.Tag = dtLevelData.Copy();
                dtFullData = dtLevelData;


                if (dtLevelData.Rows.Count > 0)
                    dgParam.Rows[0].Selected = true;
                dtLevelData.Dispose();
                dtCurrentData = dtFullData;
            }
        }

        private DataTable FilterLevel(int CountVal)
        {
            FindMaxLevel();

            if (CountVal > _MaxLevel)
            {
                _CurrentLevel = _MaxLevel;
                return (DataTable)dgSearchLevel.Tag;
            }

            DataTable dtFilt = dtReadOnlyGrid.Clone();

            DataTable dtTemp = (DataTable)dgSearchLevel.Tag;

            if (dtTemp != null && _ReqColNo1 != -1)
            {
                if (arlHierarchyLevel != null)
                {
                    if (arlHierarchyLevel.Count <= _CurrentLevel - 2)
                        arlHierarchyLevel.Add(dtTemp.Rows[CurrentRowIndex][_ReqColNo1].ToString());
                    else if (_CurrentLevel >= 2)
                        arlHierarchyLevel[_CurrentLevel - 2] = dtTemp.Rows[CurrentRowIndex][_ReqColNo1].ToString();
                }
            }

            if (dtTemp != null && _ReqColNo2 != -1)
            {
                if (arlHierarchyValue != null)
                {
                    if (arlHierarchyValue.Count <= _CurrentLevel - 2)
                        arlHierarchyValue.Add(dtTemp.Rows[CurrentRowIndex][_ReqColNo2].ToString());
                    else if (_CurrentLevel >= 2)
                        arlHierarchyValue[_CurrentLevel - 2] = dtTemp.Rows[CurrentRowIndex][_ReqColNo2].ToString();
                }
            }

            if (dtTemp != null && arlPrimKey != null)
            {
                if (arlPrimKey.Count < MaxLevel)
                {
                    if (arlPrimKey.Count <= _CurrentLevel - 2)
                        arlPrimKey.Add(dtTemp.Rows[CurrentRowIndex][1].ToString());
                    else if (_CurrentLevel >= 2)
                        arlPrimKey[_CurrentLevel - 2] = dtTemp.Rows[CurrentRowIndex][1].ToString();
                }
            }

            string str = "";
            int i;
            // All except the last column
            for (i = 0; i < dtReadOnlyGrid.Columns.Count - 1; i++)
            {
                if (CountVal == 1)
                {
                    if (i == 0)
                        str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + CountVal + "' AND ";
                    else
                        str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '' AND ";
                }
                else
                {
                    if (i == 0)
                        str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + CountVal + "' AND ";
                    else if (i == 2)
                        str += "(" + dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + dtTemp.Rows[CurrentRowIndex][1].ToString() + "' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') AND ";
                    else
                        str += "(" + dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') AND ";
                }
            }

            // For the last column, so that there is no trailing AND
            if (CountVal == 1)
            {
                if (i == 0)
                    str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + CountVal + "' ";
                else
                    str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '' ";
            }
            else
            {
                if (i == 0)
                    str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + CountVal + "' ";
                else if (i == 2)
                    str += "(" + dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + dtTemp.Rows[CurrentRowIndex][1].ToString() + "' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') ";
                else
                    str += "(" + dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') ";
            }

            DataRow[] drTemp = dtReadOnlyGrid.Select(str, dtReadOnlyGrid.Columns[CurrentCell.ColumnIndex].ColumnName, DataViewRowState.OriginalRows);

            for (i = 0; i < drTemp.Length; i++)
                dtFilt.ImportRow(drTemp[i]);

            if (dtTemp != null)
                dtTemp.Dispose();

            return dtFilt;
        }


        private void FindMaxLevel()
        {
            int temp = 0, temp1 = 0;

            for (int i = 0; i < dtReadOnlyGrid.Rows.Count; i++)
            {
                temp1 = Convert.ToInt32(dtReadOnlyGrid.Rows[i][0].ToString());
                if (temp1 > temp)
                    temp = temp1;
            }

            _MaxLevel = temp;
        }

        public void SetDataBinding(object datasource, string dataMember)
        {
            try
            {

                DataTable dtset = (DataTable)datasource;
                if (dtset.Rows.Count > 0)
                {
                    CurrencyManager cm = (CurrencyManager)this.BindingContext[this.DataSource];
                    ((DataView)cm.List).AllowNew = false;

                }
                this.DataSource = dtset;
                this.Columns.Clear();
                for (int i = 0; i < dtset.Columns.Count; i++)
                {


                    if (dtset.Columns[i].ColumnName.ToUpper().IndexOf("MASK") == 0)
                        continue;

                    DataGridViewTextBoxColumn dgTBCol = new DataGridViewTextBoxColumn();
                    dgTBCol.DataPropertyName = dtset.Columns[i].ColumnName;
                    dgTBCol.HeaderText = dtset.Columns[i].ColumnName;
                    this.Columns.Add(dgTBCol);

                }

                if (this.AssociatedSearchGrid != null)
                {
                    this.AssociatedSearchGrid.dtFullData = dtset;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("");
            }
        }


        public void AutoSizeCol()
        {
            try
            {

                //this.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }

        }

        private void painting(object sender, PaintEventArgs e)
        {
            try
            {
                
                //this.EnableHeadersVisualStyles = true;//Changed

                this.AllowUserToResizeColumns = true;
                //this.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Beige;
                //this.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Beige;
                //this.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                //Font f = new Font(this.DefaultCellStyle.Font.Name.ToString(), this.DefaultCellStyle.Font.Size, FontStyle.Bold);
                //this.ColumnHeadersDefaultCellStyle.Font = f;
                //this.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                //this.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Red;
                //this.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Linen;
                //this.BackgroundColor = System.Drawing.Color.Honeydew;
                //this.RowHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Green;
                //this.RowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Beige;
                //this.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Green;
                //this.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Honeydew;
                this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
            }
        }

        public void IncreseCount()
        {
            _CurrentLevel++;
        }


        private void DecreaseCount()
        {
            _CurrentLevel--;
        }

        //Design Search Functionalties
        private DataGridView dgGrid;
        public DataTable dtFullData;
        public DataTable dtCurrentData;
        public DataTable dtFilterData;
        private DataTable dtSelected;
        private bool _HotSearchChild = false;

        public bool HotSearchChild
        {
            get { return _HotSearchChild; }
            set { _HotSearchChild = value; }
        }


        public bool DesignSeachGrid(DataGridView dgRecords, ref string strErrMsg)
        {
            try
            {
                dgGrid = dgRecords;
                dtFullData = ((DataTable)dgGrid.DataSource).Copy();

                ////**** Creating a new Table Style to include the TextBoxes in the Datagrid

                object[] obj = new object[dtFullData.Columns.Count];
                for (int i = 0; i < obj.Length; i++)
                {
                    obj[i] = null;
                }
                DataTable dtSingleRow = dtFullData.Clone();
                dtSingleRow.Rows.Add(obj);
                dtSingleRow.AcceptChanges();
                dtSelected = dtSingleRow.Clone();
                //this.SetColumns();


                CurrencyManager cm = (CurrencyManager)this.BindingContext[this.DataSource];

                ((DataView)cm.List).AllowNew = false;

                dtSingleRow.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message + " - " + ex.Source;
                return false;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.F))
            {
                AutoFilterHeader _HeaderCell =
                    this.CurrentCell.OwningColumn.HeaderCell as
                    AutoFilterHeader;
                if (_HeaderCell != null)
                {
                    _HeaderCell.ShowDropDownList(-1, this.CurrentCell.OwningColumn.Index,false);
                }
            }
            base.OnKeyDown(e);
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {

            
            base.OnCellPainting(e);
            if (this.Rows.Count > 0 && CstmEnableSelectedCellColors)
            {
                Rectangle CellBounds = e.CellBounds;
            //    if (e.RowIndex == this.CurrentCell.RowIndex)
            //    {
            //        using (Brush backbrush =
            //            new System.Drawing.Drawing2D.LinearGradientBrush(CellBounds,
            //                System.Drawing.Color.FromArgb(255, 213, 103),
            //                System.Drawing.Color.FromArgb(255, 228, 145),
            //                System.Drawing.Drawing2D.LinearGradientMode.Vertical))
            //        {
            //            e.Graphics.FillRectangle(backbrush, CellBounds);
            //        }
            //    }
                if (e.RowIndex == this.CurrentCell.RowIndex && e.ColumnIndex == this.CurrentCell.ColumnIndex)
                {
                    // Paint the custom selection background.

                    //using (Brush backbrush =
                    //new System.Drawing.Drawing2D.LinearGradientBrush(CellBounds,
                    //   System.Drawing.Color.FromArgb(255, 213, 103),
                    //        System.Drawing.Color.White,
                    //    System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
                    using (Brush backbrush =
                   new System.Drawing.Drawing2D.LinearGradientBrush(CellBounds,
                      SelectedCellColorStart,
                       SelectedCellColorEnd,
                       CellsColorGradientMode))
                    {
                        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                        Brush stringBrush = new SolidBrush(System.Drawing.Color.Black);
                        e.Graphics.FillRectangle(backbrush, CellBounds);

                        #region Unused String drawing & Rectangle Inflating on Cell Bounds
                        //StringFormat flags = new StringFormat();
                        //flags.LineAlignment = StringAlignment.Center;
                        //flags.Alignment = StringAlignment.Center;

                        //e.Graphics.DrawString(e.FormattedValue.ToString(),this.DefaultCellStyle.Font,stringBrush,CellBounds, flags);
                        //e.Graphics.DrawRectangle(new Pen(System.Drawing.Color.CadetBlue), CellBounds);
                        //System.Drawing.Size sz = new Size(25, 25);
                        //CellBounds.Inflate(sz);
                        //e.Graphics.DrawRectangle(new Pen(System.Drawing.Color.CadetBlue), CellBounds);
                        #endregion Unused String drawing & Rectangle Inflating on Cell Bounds
                    }
                }
            }
        }


        public void RemoveAllFilter()
        {
            foreach (DataGridViewColumn col in this.Columns)
            {
                Type t = col.GetType();
                if (t.Name.ToString() == "DataGridViewButtonColumn")
                    continue;
                //col.HeaderCell = new
                //    AutoFilterHeader(col.HeaderCell);
                AutoFilterHeader _Header = (AutoFilterHeader)col.HeaderCell;
                if (_Header.Filtered)
                    _Header.RemoveFilter();
            }
        }

        private SearchStubCollection<SearchStub> objPreviousSearchStub = new SearchStubCollection<SearchStub>();

        public SearchStubCollection<SearchStub> PreviousSearchStub
        {
            get { return objPreviousSearchStub; }
            set { objPreviousSearchStub = value; }
        }
    }

    public class SearchStub
    {
        public SearchStub()
        {
 
        }

        public SearchStub(string strColName)
        {
            this.strColName = strColName;
        }

        public SearchStub(string strSearchParam, AutoFilterHeader objOwingHeaderCell, string strColName)
        {
            this.strSearchParam = strSearchParam;
            if (strSearchParam != "")
            {
                blFiltered = true;
            }
            else
            {
                blFiltered = false;
            }
            //blFiltered = strSearchParam == "";
            this.objOwingHeaderCell = objOwingHeaderCell;
            this.strColName = strColName;
        }

        private bool blFiltered = false;
        private string strSearchParam = String.Empty;
        private AutoFilterHeader objOwingHeaderCell = null;
        private string strColName = String.Empty;

        public bool Filtered
        {
            get { return blFiltered; }
        }


        public string ColumnName
        {
            get { return strColName; }
            set { strColName = value; }
        }

        public string SearchParam
        {
            get { return strSearchParam; }
            set 
            { 
                strSearchParam = value;
                if (value.ToString() != "")
                {
                    blFiltered = true;
                }
                else
                {
                    blFiltered = false;
                }
                //blFiltered = value.ToString() == "";
            }
        }

        public AutoFilterHeader OwingHeaderCell
        {
            get { return objOwingHeaderCell; }
            set { objOwingHeaderCell = value; }
        }

        
    }

    public class SearchStubCollection<T> : ICollection<T> where T : SearchStub
    {
        #region "                   Variables                   "
        /// <summary>
        /// 
        /// </summary>
        protected ArrayList lstSearchStubCollection;
        /// <summary>
        /// Internal flag to hold whether collection is read-only or not.
        /// </summary>
        protected bool blIsReadOnly;
        #endregion "                   Variables                   "

        #region "                   Constructor                   "
        public SearchStubCollection()
        {
            lstSearchStubCollection = new ArrayList();
        }
        #endregion "                   Constructor                   "

        #region "           Indexer         "
        /// <summary>
        /// Gets or sets an element in the instance of RangesCollection<typeparam name="T">clsRange</typeparam> class .
        /// </summary>
        /// <param name="index">Index of the operation</param>
        /// <returns>Retrurns clsRange instance at specified index</returns>
        public virtual T this[int index]
        {
            get
            {
                return (T)lstSearchStubCollection[index];
            }
            set
            {
                lstSearchStubCollection[index] = value;
            }
        }
        #endregion "           Indexer         "

        #region ICollection<T> Members

        /// <summary>
        /// Adds an clsRange object to the end of the RangesCollection.
        /// </summary>
        /// <param name="item">clsRange object to be added.</param>
        public void Add(T item)
        {
            lstSearchStubCollection.Add(item);
        }

        /// <summary>
        /// Removes all elements from the RangesCollection.
        /// </summary>
        public void Clear()
        {
            lstSearchStubCollection.Clear();
        }

        /// <summary>
        /// Determines whether an element is in the RangesCollection.
        /// </summary>
        /// <param name="item">The clsRange object to locate in the RangesCollection. The value can be a null reference.</param>
        /// <returns>true if item is found in the RangesCollection; otherwise, false.</returns>
        public bool Contains(T item)
        {
            foreach (T obj in lstSearchStubCollection)
            {
                if ((obj.ColumnName.Equals(item.ColumnName)))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Copies the RangesCollection or a portion of it to a one-dimensional clsRange array.
        /// </summary>
        /// <param name="array">The one-dimensional clsRange array that is the destination of the elements copied from ArrayList. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            //
        }

        /// <summary>
        /// Gets the number of elements actually contained in the RangesCollection.
        /// </summary>
        public int Count
        {
            get { return lstSearchStubCollection.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the RangesCollection is read-only.(Not implemented)
        /// </summary>
        public bool IsReadOnly
        {
            get { return blIsReadOnly; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific clsRange object from the RangesCollection.
        /// </summary>
        /// <param name="item">The clsRange object to remove from the RangesCollection. The value can be a null reference.</param>
        /// <returns>true if item is found in the RangesCollection and removed; otherwise, false.</returns>
        public bool Remove(T item)
        {

            bool result = false;

            //loop through the inner array's indices
            for (int i = 0; i < lstSearchStubCollection.Count; i++)
            {
                //store current index being checked
                T obj = (T)lstSearchStubCollection[i];

                //compare the BusinessObjectBase UniqueId property
                if ((obj.ColumnName.Equals(item.ColumnName)))
                {
                    //remove item from inner ArrayList at index i
                    lstSearchStubCollection.RemoveAt(i);
                    result = true;
                    break;
                }
            }

            return result;
        }
        #endregion

        public int IndexOf(T item)
        {
            int result = -1;

            //loop through the inner array's indices
            for (int i = 0; i < lstSearchStubCollection.Count; i++)
            {
                //store current index being checked
                T obj = (T)lstSearchStubCollection[i];

                //compare the BusinessObjectBase UniqueId property
                if ((obj.ColumnName.Equals(item.ColumnName)))
                {
                    //remove item from inner ArrayList at index i

                    result = i;
                    break;
                }
            }

            return result;

        }

        #region IEnumerable<T> Members

        /// <summary>
        /// Overloaded. Returns an enumerator that can iterate through the RangesCollection.
        /// </summary>
        /// <returns>An IEnumerator<typeparamref name="clsRange"/> for the entire RangesCollection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new SearchStubEnumerator<T>(this);
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Overloaded. Returns an enumerator that can iterate through the RangesCollection.
        /// </summary>
        /// <returns>An IEnumerator for the entire ArrayList.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SearchStubEnumerator<T>(this);
        }


        #endregion
    }

    public class SearchStubEnumerator<T> : IEnumerator<T> where T : SearchStub
    {
        #region "           Member Variables            "

        /// <summary>
        /// Enumerated RangesCollection collection.
        /// </summary>
        protected SearchStubCollection<T> _SearchStubCollection;
        /// <summary>
        /// Current index of the collection.
        /// </summary>
        protected int index;
        /// <summary>
        /// Current enumerated object in the collection
        /// </summary>
        protected T _current;

        #endregion

        #region "           Ctor            "
        /// <summary>
        /// Overloaded.Empty constructor of the class.Initializes a new instance of the RangeEnumerator class .
        /// </summary>
        public SearchStubEnumerator()
        {
            //nothing
        }

        /// <summary>
        /// Overloaded.Initializes a new instance of the RangeEnumerator class .
        /// </summary>
        /// <param name="collection">RangesCollection instance which is to be iterated through.</param>
        public SearchStubEnumerator(SearchStubCollection<T> _SearchStubCollection)
        {
            this._SearchStubCollection = _SearchStubCollection;
            index = -1;
            _current = default(T);
        }
        #endregion "           Ctor            "

        #region "           IEnumerator<T> Members          "
        /// <summary>
        /// Gets the current enumerated object in the collection
        /// </summary>
        public T Current
        {
            get { return _current; }
        }

        #endregion "           IEnumerator<T> Members          "

        #region "           IDisposable Members         "
        /// <summary>
        /// Realeses all resources used.
        /// </summary>
        public void Dispose()
        {
            _SearchStubCollection = null;
            _current = default(T);
            index = -1;
        }

        #endregion "           IDisposable Members         "

        #region "           IEnumerator Members         "
        /// <summary>
        /// Gets the current enumerated object in the collection
        /// </summary>
        object IEnumerator.Current
        {
            get { return _current; }
        }

        /// <summary>
        /// Advances the RangeEnumerator to the next element of the RangesCollection collection
        /// </summary>
        /// <returns>Returns true if iterating is successfull,else false.</returns>
        public bool MoveNext()
        {
            //make sure we are within the bounds of the collection
            if (++index >= _SearchStubCollection.Count)
            {
                //if not return false
                return false;
            }
            else
            {
                //if we are, then set the current element to the next object in the collection
                _current = _SearchStubCollection[index];
            }

            //return true
            return true;
        }

        /// <summary>
        /// Sets the RangeEnumerator to its default position,which is before the first element in the RangesCollection collection.
        /// </summary>
        public void Reset()
        {
            _current = default(T); //reset current object
            index = -1;
        }

        #endregion "           IEnumerator Members         "
    }
    
}
