#region Author Info.
///@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
/// ````````````````Debanjan Routh````````````````````````
///@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using UniConvert;
using ICTEAS.WinForms.Common;
using ICTEAS.DataComponents.Custom;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using ICTEAS.WinForms.Helpers;

namespace ICTEAS.WinForms.Controls
{
    public enum ScreenGroup
    {
        Product,
        Sector,
        Branch,
        SalesExecutive,
        SalesArea,
        None
    };

    public enum PlanType
    {
        Quarter,
        Month,
        None
    };

    public enum SwapMode
    {
        Up,
        Down,
        Top,
        Bottom
    };

    public enum UnitType
    {
        Volume,
        TurnOver,
        Margin,
        NotApplicable
    };

    public partial class clsWritableGrid : System.Windows.Forms.DataGridView
    {
        public string ValueOnEnter = "";
        public int CursorPosition = -1;

        public bool insidetextchanged = false;
        public int CurrentRowIndex = -1;
        public int columnselected = -1;
        public string captiontext;
        bool isCurrency = true;
        DataGridViewTextBoxEditingControl editingcontrol;
        DataGridViewComboBoxEditingControl comboedit;
        //added by Nilanjan
        private Font captionfont;
		private int intLoadRowCount=0;
		private int intTotalRowCount=0;
		private int intColCount=0;
        private bool captionvisible;
		private const int MAXSTRLENGTH = 1000;
		private string strSQLProcName;
		private bool boolIsSQL = true;
        string strstoretext;
		private string[] strColNames;
		private string[] strHeadings;
		private string[] strLangIDs;
        public delegate void F2KeyHandler(object sender, F2EventArgs args);
        public event F2KeyHandler F2keypressed;
        public delegate void ComboSelectionChangeCommitedhandler(object sender, ComboSelectionChangeCommitedEventArgs args);
        public event ComboSelectionChangeCommitedhandler combocomitted; 
        public  System.Data.DataView dvSource;
        public ComboBox comboforall=new ComboBox();
        public MyGridTextBoxColumn[] dgTxt;
        public KeyTrapTextBox[] DataGridTxtBox;
       // public DataGridComboBoxColumn[] dgComboBox;
        public DataGridDateTimePicker[] dgDateTimePicker;
        public DataGridButtonColumn[] dgButton;
        public string type_cell = "cell";
        private clsSearchGrid _dgThisSearchGrid;
        public TextBox[] tb;

        public delegate void CellTextChangedEventHandler(object sender, DataGridViewCellTextChangedEventArgs args);
        public event CellTextChangedEventHandler DataGridViewCellTextChanged;
        public int currentcol=0;
		
		private DataSet dsSource;
		private DataTable dtSource;

		private ArrayList arlOutParam;
        private ArrayList ProcParamNames = new ArrayList();
        private ArrayList ProcParamValues;
        private ArrayList ProcParamDir;

        //CheckBox
        private int intNumCheckBoxCols;
        private ArrayList arlCheckBoxColumns;
        private string[] arrCheckValues;
        public delegate void DataGridViewCheckChangedHandler(object sender, DataGridViewCheckChangedEventArgs args);
        public event DataGridViewCheckChangedHandler checkChanged;

        public delegate void DataGridViewEditingControlTextChangedEventHandler(object sender,DataGridViewEditingControlTextChanged args);
        public event DataGridViewEditingControlTextChangedEventHandler EditingControlTextChanged;

        //****************************************************************************************
        /// <summary>
        /// Filter State Changed Functions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void DataGridViewFilterStateChanged(object sender, DataGridViewFilterChanged args);

        [Description("This event fires on any change of filter condition on the grid.")]
        public event DataGridViewFilterStateChanged FilterStateChanged;

        public void HandleFilterStateEvent(string _CurrentFilter, int _CurrentRowNumber, bool _IsFiltered)
        {
            if (EnableDataFiltering)
            {
                DataGridViewFilterChanged filterChanged = new DataGridViewFilterChanged(_CurrentFilter, _CurrentRowNumber, _IsFiltered);
                if (FilterStateChanged != null)
                {
                    FilterStateChanged(this, filterChanged);
                }
            }
        }
        //****************************************************************************************

		private int intNumComboCols;
		public  ArrayList arlComboCols;
        public string[][] arrComboValues;
		
		private int intNumDateTimeCols = 0;
		private int intNumButtonCols = 0;

		private string[] strProcOutValue;

		private int[] intDisableColumnNum, intReadOnlyColumnNum, intRightAlignColumnNum;
		private int[] intHiddenCols, intShowCols, intDateTimeColumnNum, intButtonColumnNum;
		
		public ArrayList arlNumericColumNum, arlNumericColumnType, arlNumericColIntSize, arlNumericColFloatSize;
        public ArrayList arlNumericColCurrency, arlStringColumsStrLength, arlStringColumnCollection;
        public ArrayList arlFixedLengthNumCols;

		private int _toggleRow = -1, _toggleCol = -1;
		private ArrayList arlEnableColumns = new ArrayList();
		private ArrayList arlEnableRows = new ArrayList();

		private int noOfRowsToInspectForColumnAutoSizing = 15;
		private bool _boolPrivScreen = false;

		
        public int store_rowno=1;

		private string[] strLangID = new string[0];
        ComboBox filtercombo;
        public bool cellenter = false, cellleave = false;

        #region Constructor
        public clsWritableGrid()
		{

            InitializeComponent();
            Assembly Asm = Assembly.GetExecutingAssembly();
            //Search Image Not Active
            string Resource_name1 = "ICTEAS.WinForms.Resources.ROWHEADERARROW3.gif";
            System.IO.Stream stream1 = Asm.GetManifestResourceStream(Resource_name1);
            _imgSelectedRowHeaderIcon = Image.FromStream(stream1);

            //THIS LINE IS ADDED SO THAT EACH CAN EDITED WITH A SINGLE CLICK
            this.EditMode = DataGridViewEditMode.EditOnEnter;
            //***FOLLOWING LINES ARE ADDED TO REDUCE FLICKER OF THE COMPONENT
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            //****************************************************************
            this.EnableHeadersVisualStyles = true;
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
            this.ScrollBars = ScrollBars.Both;
            ToolStripManager.Renderer = new ICTEAS.WinForms.Helpers.Office2007Renderer();
            //**New*******************************************


            ///*******************************************************************
            //this.EnableHeadersVisualStyles = true;
            //this.ScrollBars = ScrollBars.Both;
            //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            //this.ColumnHeadersHeight = 18;
            //this.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;

            ////Alternating Row Default Style
            //System.Windows.Forms.DataGridViewCellStyle alternatingCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            //alternatingCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            //alternatingCellStyle.BackColor = System.Drawing.Color.Transparent;
            //alternatingCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //alternatingCellStyle.ForeColor = System.Drawing.Color.Black;
            //alternatingCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            //alternatingCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            //this.AlternatingRowsDefaultCellStyle = alternatingCellStyle;

            ////Column Header Default Cell Style
            //System.Windows.Forms.DataGridViewCellStyle columnHeaderCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            //columnHeaderCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            //columnHeaderCellStyle.BackColor = System.Drawing.SystemColors.Control;
            //columnHeaderCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //columnHeaderCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            //columnHeaderCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            //columnHeaderCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            //this.ColumnHeadersDefaultCellStyle = columnHeaderCellStyle;


            ////Default Cell Style
            //System.Windows.Forms.DataGridViewCellStyle defaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            //defaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            //defaultCellStyle.BackColor = System.Drawing.Color.Transparent;
            //defaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //defaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            //defaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            //defaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            //defaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            //this.DefaultCellStyle = defaultCellStyle;

            ////
            //this.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            //this.GridColor = System.Drawing.Color.CadetBlue;
            //this.RowHeadersWidth = 30;
            //this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //this.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            //this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //this.RowTemplate.Height = 18;

            ///*******************************************************************
            withheader.Click += new EventHandler(withheader_Click);
            withoutheader.Click += new EventHandler(withoutheader_Click);
            Filter.Click += new EventHandler(Filter_Click);
            fill.Click += new EventHandler(fill_Click);
            this.CellLeave += new DataGridViewCellEventHandler(clsWritableGrid_CellLeave);
            this.CellEnter += new DataGridViewCellEventHandler(clsWritableGrid_CellEnter);
            //this.AutoGenerateColumns = false;
            intLoadRowCount = 0;
            intColCount = 0;
            intTotalRowCount = 0;
			arlNumericColFloatSize = new ArrayList();
			arlNumericColIntSize = new ArrayList();
			arlNumericColumnType = new ArrayList();
			arlNumericColumNum = new ArrayList();
			arlNumericColCurrency = new ArrayList ();
			arlStringColumsStrLength = new ArrayList ();
			arlStringColumnCollection = new ArrayList ();

            arlFixedLengthNumCols = new ArrayList();

            //Coloring
            //this.EnableHeadersVisualStyles = false;
            //this.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(190,216,250);
            //this.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(190, 216, 250);
            //this.ColumnHeadersDefaultCellStyle.ForeColor = Color.SeaGreen;
            //this.DefaultCellStyle.ForeColor = Color.Red;
            //this.DefaultCellStyle.SelectionForeColor = Color.Green;
            //this.DefaultCellStyle.SelectionBackColor = Color.Honeydew;
            //this.BackgroundColor = Color.FromArgb(190, 216, 250);
            //this.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
            //this.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Honeydew;
            //this.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
            //this.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Honeydew;
            //this.CellValidated += new DataGridViewCellEventHandler(clsWritableGrid_CellValidated);
            //
            //this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.EditMode = DataGridViewEditMode.EditOnEnter;

            this.CurrentCellDirtyStateChanged += new EventHandler(clsWritableGrid_CurrentCellDirtyStateChanged);
            EnableRowAddRemove();
            this.xport.Visible = false;
            this.Copy.Visible = false;
            this.withheader.Visible = false;
            this.withoutheader.Visible = false;
            this.Delete.Visible = false;
            this.fill.Visible = false;
            this.paste.Visible = false;
            //this.AllowSorting = false;
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

        
        private bool _IsFloatSetting = false;

        [Browsable(false)]
        public bool IsFloatSetting
        {
            get { return _IsFloatSetting; }
            set { _IsFloatSetting = value; }
        }


        bool _bool = false;
        int selectionStart = -1;
        int noOfCommas = 0;
        int selectionLength = 0;
        void clsWritableGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                //this.CommitEdit(DataGridViewDataErrorContexts.Commit);
                //selectionStart = 0;
                //selectionLength = 0;
                //if (!_bool)
                //{
                //    selectionStart = this.editingcontrol.SelectionStart;
                //    selectionLength = this.editingcontrol.SelectionLength;
                //    _bool = true;
                //}
                //else
                //{
                //    _bool = false;
                //}
                this.CommitEdit(DataGridViewDataErrorContexts.Commit);

                if (this.editingcontrol != null)
                {
                    if (!IsFloatSetting)
                        this.editingcontrol.SelectionStart = this.CurrentCell.EditedFormattedValue.ToString().Length;
                    else
                    {
                        //this.editingcontrol.SelectionStart = this.CurrentCell.EditedFormattedValue.ToString().Length;
                        //this.editingcontrol.SelectionLength = 0;
                        if (selectionStart != -1)
                        {
                            int i = selectionStart + noOfCommas;
                            this.editingcontrol.SelectionStart = i;
                            this.editingcontrol.SelectionLength = selectionLength;
                            selectionStart = -1;
                            noOfCommas = 0;
                        }
                    }


                }
            }
            catch
            {
 
            }
           // // this.CurrentCell.Selected = false;
        }

        //void clsWritableGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //    //_NewValue = this[e.RowIndex, e.ColumnIndex].ToString();
        //    //_NewValue = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        //    //if (_OldValue != _NewValue)
        //    //{
        //    //    DataGridViewCellTextChangedEventArgs TextChanged = new DataGridViewCellTextChangedEventArgs(true, _OldValue, _NewValue);
        //    //    if (DataGridViewCellTextChanged!= null)
        //    //    {
        //    //        DataGridViewCellTextChanged(this, TextChanged);

        //    //    }
        //    //}
        //}
        #endregion

        #region Row Paint Deprecated
        //protected override void OnRowPrePaint(DataGridViewRowPrePaintEventArgs e)
        //{
        //    if (ReadOnlyRowList.Count > 0)
        //    {
        //        if (ReadOnlyRowList.Contains((object)e.RowIndex))
        //        {
        //            Rectangle rowBounds = new Rectangle(
        //               this.RowHeadersWidth, e.RowBounds.Top,
        //               this.Columns.GetColumnsWidth(
        //                   DataGridViewElementStates.Visible) -
        //               this.HorizontalScrollingOffset + 1,
        //               e.RowBounds.Height);

        //            using (Brush backbrush =
        //           new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
        //               ReadonlyColorStart,
        //               ReadonlyColorEnd,
        //               System.Drawing.Drawing2D.LinearGradientMode.Vertical))
        //            {
        //                e.Graphics.FillRectangle(backbrush, rowBounds);
        //            }
        //            return;
        //        }
        //    }
        //    //********************************************************
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
        #endregion Row Paint Deprecated

        protected override void OnRowPrePaint(DataGridViewRowPrePaintEventArgs e)
        {
            if (ReadOnlyRowList.Count > 0)
            {
                if (ReadOnlyRowList.Contains((object)e.RowIndex))
                {
                    Rectangle rowBounds = new Rectangle(
                       this.RowHeadersWidth, e.RowBounds.Top,
                       this.Columns.GetColumnsWidth(
                           DataGridViewElementStates.Visible) -
                       this.HorizontalScrollingOffset + 1,
                       e.RowBounds.Height);

                    using (Brush backbrush =
                   new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
                       ReadonlyColorStart,
                       ReadonlyColorEnd,
                       CstmRowsColorGradientMode))
                    {
                        e.Graphics.FillRectangle(backbrush, rowBounds);
                    }
                    return;
                }
            }
            //********************************************************
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
                        CstmRowsColorGradientMode))
                    {
                        e.Graphics.FillRectangle(backbrush, rowBounds);
                    }
                    return;
                }
                using (Brush backbrush =
                    new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
                        NormalRowColorStart,
                        NormalRowColorEnd,
                        CstmRowsColorGradientMode))
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
                        CstmRowsColorGradientMode))
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
                        CstmRowsColorGradientMode))
                {
                    e.Graphics.FillRectangle(backbrush, rowBounds);
                }
            }

            base.OnRowPrePaint(e);
        }

        string _OldValue = "";
        string _NewValue = "";

        private bool _EnablePaste = false;
        public bool EnablePaste
        {
            get { return _EnablePaste; }
            set { _EnablePaste = value; EnableDisablePasteMenu(); }
        }

        private void EnableDisablePasteMenu()
        {
            this.paste.Enabled = _EnablePaste;
        }

        private bool _EnableAddRemoveRow = false;
        public bool EnableAddRemoveRow
        {
            get { return _EnableAddRemoveRow; }
            set { _EnableAddRemoveRow = value; EnableRowAddRemove(); }
        }

        private void EnableRowAddRemove()
        {
            if (!_EnableAddRemoveRow)
            {
                this.AddRow.Enabled = false;
                this.AddRow.Visible = false;
                this.RemoveRow.Enabled = false;
                this.RemoveRow.Visible = false;
                //this.RowsMenu.Enabled = false;
                //this.RowsMenu.Visible = false;
            }
            else
            {
                this.AddRow.Enabled = true;
                this.AddRow.Visible = true;
                this.RemoveRow.Enabled = true;
                this.RemoveRow.Visible = true;
                //this.RowsMenu.Enabled = true;
                //this.RowsMenu.Visible = true;
            }
        }
        void clsWritableGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ValueOnEnter = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                _OldValue = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            }
            catch (Exception ex)
            {
            }
        }

        void clsWritableGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                currentcol = e.ColumnIndex;

                // _OldValue = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                _NewValue = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (_OldValue != _NewValue)
                {
                    DataGridViewCellTextChangedEventArgs TextChanged = new DataGridViewCellTextChangedEventArgs(true, _OldValue, _NewValue, e.RowIndex, e.ColumnIndex);
                    if (DataGridViewCellTextChanged != null)
                    {
                        DataGridViewCellTextChanged(this, TextChanged);

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

       

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
        public clsSearchGrid AssociatedSearchGrid
        {
            set { _dgThisSearchGrid = value; }
            get { return _dgThisSearchGrid; }
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


      

        public bool AllowSorting
        {
            get
            {
                return false;
            }
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


        public Color LinkColor
        {
            
            set
            {
                
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

      



        //************************



        //Methods from the past

        public void FocusCell(int row, int col)
        {
            try
            {
                this.Rows[row].Cells[col].DataGridView.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        public void Select(int RowNumber)
        {
            try
            {
                this.Rows[RowNumber].Selected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }


        public void UnSelect(int RowNumber)
        {
            try
            {
                this.Rows[RowNumber].Selected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }


        }

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

        public delegate void ColumnHeaderEventHandler(object sender, MouseEventArgs e, HitTestInfo htI);
        public event ColumnHeaderEventHandler ColumnHeaderClick;

      
         public void SetDataBinding(object datasource,string dataMember)
        {
            try
            {

                DataTable dtset = (DataTable)datasource;
                this.DataSource = dtset;

                dvSource = dtset.DefaultView;//Debanjan Added Because Filters Were Not Working After Removing Filter With Data Added But Not Saved.
                this.Columns.Clear();
                for (int i = 0; i < dtset.Columns.Count; i++)
                {
                   
                   
                    if (dtset.Columns[i].ColumnName.ToUpper().IndexOf("MASK") == 0)
                           continue;

                    if (CheckComboBoxColumn(i))
                    {
                        
                        DataGridViewComboBoxColumn dgCBCol = new DataGridViewComboBoxColumn();
                        dgCBCol.HeaderText = dtSource.Columns[i].ColumnName;
                        dgCBCol.DataPropertyName = dtSource.Columns[i].ColumnName;
                        dgCBCol.DataSource = GetComboValue(i);
                        dgCBCol.DefaultCellStyle.NullValue = "Choose a value";
                        dgCBCol.Width = 200;
                        dgCBCol.DisplayStyleForCurrentCellOnly = true;
                        //comboforall = new ComboBox();
                        if (CheckReadOnlyColumn(i))
                        {
                            dgCBCol.ReadOnly = true;
                           
                        }

                        this.Columns.Insert(this.Columns.Count, dgCBCol);
                        
                    }
                    else if (CheckCheckBoxColumn(i))
                    {
                        DataGridViewCheckBoxColumn dgChkCol = new DataGridViewCheckBoxColumn(false);
                        if (dgChkCol.Displayed)
                        {
                        }
                        else
                        {
                            dgChkCol.Visible = true;
                        }
                        dgChkCol.HeaderText = strHeadings[i];
                        dgChkCol.DataPropertyName = dtSource.Columns[i].ColumnName;
                        
                        dgChkCol.CellTemplate = new DataGridViewCheckBoxCell();
                        this.Columns.Add(dgChkCol);
                                               
                    }
                    else if (CheckDateTimeColumn(i))
                    {
                        CalendarColumn dgDTPCol = new CalendarColumn();
                        dgDTPCol.HeaderText = strHeadings[i];
                        dgDTPCol.DataPropertyName = dtSource.Columns[i].ColumnName;
                        if (CheckReadOnlyColumn(i))
                        {
                            dgDTPCol.ReadOnly = true;

                        }

                        this.Columns.Add(dgDTPCol);
                    }
                    else if (CheckButtonColumn(i))
                    {
                        DataGridViewButtonColumn dgBtnCol = new DataGridViewButtonColumn();
                        dgBtnCol.DataPropertyName = dtSource.Columns[i].ColumnName;
                        dgBtnCol.HeaderText = strHeadings[i];
                        this.Columns.Add(dgBtnCol);

                    }
                    else
                    {
                        DataGridViewTextBoxColumn dgTBCol = new DataGridViewTextBoxColumn();
                        dgTBCol.DataPropertyName = dtSource.Columns[i].ColumnName;
                        dgTBCol.HeaderText = strHeadings[i];
                        this.Columns.Insert(this.Columns.Count, dgTBCol);


                        if (CheckReadOnlyColumn(i))
                        {
                            dgTBCol.ReadOnly = true;
                        }

                        if (CheckRightAlignColumn(i))
                            dgTBCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                       

                    }
				

				
			

				//#region changing for ColSizing







                for (int k = 0; k < this.Columns.Count; k++)
                {
                    this.Columns[k].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                this.Font = new Font(this.Font, FontStyle.Regular);

                   
                }

                if (this.AssociatedSearchGrid != null)
                {
                    this.AssociatedSearchGrid.dtFullData = dtset;
                }
                CurrencyManager cm = (CurrencyManager)this.BindingContext[this.DataSource];
                ((DataView)cm.List).AllowNew = false;

                DoCreateFilterHeaders();//Debanjan Added Because Filters Were Not Coming After This Method Is Called.
         }  
  
            catch (Exception ex)
            {
                MessageBox.Show("");
            }
}

   
        //******************
        void fill_Click(object sender, EventArgs e)
        {
            try
            {

                if (columnselected >= 0)
                {

                    if (CheckComboBoxColumn(columnselected))
                    {
                        string[] s = GetComboValue(columnselected);
                        entervalue enter = new entervalue(s, this, columnselected);
                        enter.ShowDialog();
                    }
                    else
                    {
                        if (CheckDateTimeColumn(columnselected))
                        {
                            entervalue enter = new entervalue("date", this, columnselected);
                            enter.ShowDialog();
                        }
                        else
                        {
                            entervalue enter = new entervalue("text", this, columnselected);
                            enter.ShowDialog();
                        }
                    }
                }
                    
            }
            catch (Exception ex)
            {
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

                SetRowStatus(RowNumber);
            }
        }


        #region Replace in ReadOnly Column
         public void ReplaceInReadOnlyColumn(int RowNum, int ColNum, string ReplaceString)
        {
            
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
            NumberFormatInfo nmfi = new NumberFormatInfo();
            nmfi = ci.NumberFormat;
            ci.ClearCachedData();
            this.CurrentRowIndex = RowNum;

            
            int m_intColPos = -1;
            if (CheckNumericColumn(ColNum, ref m_intColPos))
            {
                string rep = "";
                clsTxtBox.TypeEnum objEnum = (clsTxtBox.TypeEnum)arlNumericColumnType[m_intColPos];
                if (objEnum == clsTxtBox.TypeEnum.Float)
                {
                    if (ReplaceString == "")
                        ReplaceString = "0";
                    int m_intPrecision = (int)arlNumericColFloatSize[m_intColPos];

                    Decimal m_decTemp = Math.Round(Convert.ToDecimal(ReplaceString, nmfi), m_intPrecision);
                    string m_strTemp = "0", m_strLocal = nmfi.NumberDecimalSeparator;

                    for (int k = 0; k < m_intPrecision; k++)
                        m_strLocal += "0";

                    m_strTemp += m_strLocal;
                    rep = m_decTemp.ToString(nmfi);

                    int ind = ReplaceString.LastIndexOf(nmfi.NumberDecimalSeparator);
                    if (ind < 0)
                    {
                    }
                    else
                    {
                        string originalIntPortion = ReplaceString.Substring(0, ind);
                        if (originalIntPortion == "") originalIntPortion = "0";
                        string nextIntPortion = rep.Substring(0, rep.LastIndexOf(nmfi.NumberDecimalSeparator));
                        if (nextIntPortion == "") nextIntPortion = "0";
                        rep = rep.Replace(nextIntPortion, originalIntPortion);
                    }
                }
                else
                    rep = ReplaceString;

               
              dvSource[RowNum][ColNum] = rep;

               
            }
            else if (CheckComboBoxColumn(ColNum, ref m_intColPos))
            {
                
                dvSource[RowNum][ColNum] = ReplaceString;
            }
            else if (CheckDateTimeColumn(ColNum, ref m_intColPos))
            {
                
                dvSource[RowNum][ColNum] = ReplaceString;
            }
            else
            {
                
                dvSource[RowNum][ColNum] = ReplaceString;

            }

            
            this.SetRowStatus(RowNum);
            this.SetTxtChange();
            dvSource.Table.AcceptChanges();
            this.Update();
            this.Refresh();
        }
     

        #endregion
        
		#region textChange

		private bool boolTextChange = false;
        public void SetTxtChange()
        {
            boolTextChange = true;
        }
		public bool TxtChange
		{
			get{return boolTextChange;}
		}
		public void RefreshChange()
		{
			boolTextChange = false;
		}
		#endregion

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

		/// <summary>
		/// String Array that gives the translated column header names
		/// </summary>
		public string[] ColumnHeader
		{
			get
			{
				return strHeadings;
			}
		}

        /// <summary>
        /// RowStatus will return the status of the RowNumber -th row.
        /// If the Row is loaded and remains unchanged, it will return "0".
        /// If added, or added and then modified during the session, it will return "1".
        /// If loaded and then modified during the session, it will return "2"
        /// </summary>
        public string RowStatus(int RowNumber)
        {
            if (RowNumber >= 0 && dvSource != null && RowNumber < dvSource.Count)
                return Convert.ToString(dvSource[RowNumber][intColCount]);
            else
                return "0";
        }
        
        public void SetRowStatus(int RowNumber)
        {
            if (RowNumber >= 0 && dvSource != null && RowNumber < dvSource.Count)
            {
                orgStatus = dvSource[RowNumber][intColCount].ToString();
                if (orgStatus == "0" || orgStatus == "2")
                    newStatus = "2";
                else
                    newStatus = "1";
                dvSource[RowNumber][intColCount] = newStatus;
            }
        }


		/// <summary>
		/// This will return initial row count (ie. when loaded)
		/// </summary>
		public int InitialRowCount
		{
			get
			{
				return intLoadRowCount;
			}
		}

        public void RemoveSort()
        {
            if (dvSource != null)
            {
                dvSource.Sort = "";
                dvSource.Table.AcceptChanges();
            }
        }

		/// <summary>
		/// This will return current row total.
		/// </summary>
		public int RowCount
		{
			get
			{
                if (dvSource == null)
                {
                    intTotalRowCount = 0;
                }
                else
                {
                    intTotalRowCount = dvSource.Count;
                }
				return intTotalRowCount;
			}
		}

		/// <summary>
		/// This will return total number of columns.
		/// </summary>
		public int ColumnCount
		{
			get
			{
				return intColCount;
			}
		}
		
		public bool IsPrivilegeScreen
		{
			get{return _boolPrivScreen;}
			set{_boolPrivScreen = value;}
		}

		/// <summary>
		/// This Property will return the executed Procedure's OUT values as string Array.
		/// </summary>
		public string[] ProcedureOUTvalues
		{
			get{return strProcOutValue;}
		}

		/// <summary>
		/// This Method is to set the Grid to execute Stored Procedure.
		/// </summary>
		public string StoredProcedureName
		{
			get{return strSQLProcName;}
			set
			{
                strSQLProcName = value;
                boolIsSQL = false;
                arlOutParam = new ArrayList();
                ProcParamNames = new ArrayList();
                ProcParamValues = new ArrayList();
                ProcParamDir = new ArrayList();
            }	
		}

		/// <summary>
		/// Enter the total number of combo box columns in the Datagrid.
		/// </summary>
		public int NumberOfComboColumns
		{
			get{return intNumComboCols;}
			set
			{
				intNumComboCols = value;
				arrComboValues = new string[intNumComboCols][];
			}
		}

		/// <summary>
		/// Set the Particular Combo Box Column in DataGrid
		/// </summary>
		/// <param name="ColumnNo">Combo Column Number in Datagrid as integer</param>
		/// <param name="ComboValues">Combo Box Values as string array</param>
		public void SetComboColumn(int ColumnNo,string[] ComboValues)
		{
			try
			{
				if(arlComboCols == null)
					arlComboCols = new ArrayList();

				arlComboCols.Add(ColumnNo);
              	arrComboValues[arlComboCols.Count - 1] = ComboValues;
				
			}
			catch(Exception){ ; }
		}
        //CheckBox
        public int NumberOfCheckBoxColumns
        {
            get 
            {
                return intNumCheckBoxCols;
            }
            set
            {
                intNumCheckBoxCols = value;
                arrCheckValues = new string[intNumCheckBoxCols];
            }
        }
        //CheckBox
        public void SetCheckColumn(int ColumnNo, bool CheckValues)
        {
            try
            {
                if (arlCheckBoxColumns == null)
                    arlCheckBoxColumns = new ArrayList();
                arlCheckBoxColumns.Add(ColumnNo);
                arrCheckValues[arlCheckBoxColumns.Count - 1] = CheckValues.ToString();
            }
            catch
            {
 
            }
        }
		
		public int NumberOfRowsToInspectForColumnAutoSizing
		{
			get
			{
				return noOfRowsToInspectForColumnAutoSizing;
			}
			set
			{
				if (value > 15)
					noOfRowsToInspectForColumnAutoSizing = value;
				else
					noOfRowsToInspectForColumnAutoSizing = 15;
			}
		}

		
		private int FirstColumn()
		{
			if(intShowCols == null)
				return -1;

			return intShowCols[0];
		}

		private bool IsLastColumn(int ColNum)
		{
            if (ColNum >= intColCount)
				return true;
			
			return false;
		}

        private bool IsLastRow(int RowNum)
        {
            if (RowNum >= intTotalRowCount - 1)
                return true;

            return false;
        }

        private bool IsLastCell(int RowNum, int ColNum)
		{
            if (RowNum >= intTotalRowCount - 1 && ColNum >= intColCount)
                return true;

			return false;
		}




        protected override void OnMouseDown(MouseEventArgs e)
        {
            HitTestInfo info = this.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
            {
                if (info.Type == DataGridViewHitTestType.Cell || info.Type == DataGridViewHitTestType.RowHeader)
                {
                    
                    this.RemoveRow.Visible = true;
                }
                else
                {
                    this.RemoveRow.Visible = false;
                }
                this.CurrentRowIndex = info.RowIndex;
                if (info.Type == DataGridViewHitTestType.ColumnHeader)
                {
                    this.fill.Enabled = true;
                    this.columnselected = this.get_actual_colum_index(info.ColumnIndex);

                    if (this.columnselected < 0)
                    {
                        return;
                    }
                    if (this.check_column_status())
                    {
                        this.fill.Enabled = false;
                    }
                    clsROGContextmenu.Show(this, e.Location, ToolStripDropDownDirection.Default);

                    return;
                }
                else
                {
                    if (this.check_column_status())
                    {
                        this.fill.Enabled = false;
                    }
                    clsROGContextmenu.Show(this, e.Location, ToolStripDropDownDirection.BelowLeft);
                    return;
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                if (info.Type == DataGridViewHitTestType.ColumnHeader)
                {
                    if (AllowDefaultSorting)
                    {
                        this.ColumnHeaderMouseclickevent(info.ColumnIndex);
                        return;
                    }
                    else
                    {
                        //if (EnableDataFiltering)
                        //    ClickToSearch(e);
                        if (EnableDataFiltering)
                            base.OnMouseDown(e);
                        else
                            return;
                    }
                }
                if (info.Type == DataGridViewHitTestType.RowHeader || info.Type == DataGridViewHitTestType.Cell)
                {
                    this.CurrentRowIndex = info.RowIndex;

                }
            }
            this.columnselected = this.get_actual_colum_index(info.ColumnIndex);
            if (info.Type == DataGridViewHitTestType.Cell)
            {

                this.columnselected = this.get_actual_colum_index(info.ColumnIndex);
                //this.CurrentCell = this.Rows[info.RowIndex].Cells[this.columnselected];
                //this.Refresh();
                //this.PointToClient(new Point(e.X, e.Y));
                ////this.InvalidateRow(info.RowIndex);
                ////this.InvalidateColumn(this.columnselected);
                ////this.InvalidateCell(this.CurrentCell);
                //this.Refresh();
                //this.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(clsSearchGrid_EditingControlShowing);

            }


           base.OnMouseDown(e);
        }

        public void ClickToSearch(MouseEventArgs e)
        {
            HitTestInfo info = this.HitTest(e.X, e.Y);
            WritableGridFilterHeader header = (WritableGridFilterHeader)this.Columns[info.ColumnIndex].HeaderCell;
            DataGridViewCellMouseEventArgs me = new DataGridViewCellMouseEventArgs(info.ColumnIndex, info.RowIndex, e.X, e.Y, e);
            header.CallbackMouseDown(me);
        }

        private bool _AllowDefaultSorting = false;
        public bool AllowDefaultSorting
        {
            get { return _AllowDefaultSorting; }
            set { _AllowDefaultSorting = value; }
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

		


		private int getMaxStrLength(int col)
		{
			for(int i=0;i<arlStringColumnCollection.Count;i++)
			{
				if(arlStringColumnCollection[i].ToString () == col.ToString ())
				{
					return Convert.ToInt32(arlStringColumsStrLength[i].ToString());
				}
			}
			return MAXSTRLENGTH;
		}
		public bool SetStringColumn(int ColumnNumber,int MaxLength)
		{
			try
			{
				arlStringColumnCollection.Add(ColumnNumber);
				arlStringColumsStrLength.Add(MaxLength);
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		public bool SetNumericColumn(int ColumnNumber,clsTxtBox.TypeEnum ColumnType,int IntegerPrecision, int DecimalPrecision)
		{
			try
			{
				arlNumericColumNum.Add(ColumnNumber);
				arlNumericColumnType.Add(ColumnType);
				arlNumericColIntSize.Add(IntegerPrecision);
				arlNumericColFloatSize.Add(DecimalPrecision);
				arlNumericColCurrency .Add (false);
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		public bool SetNumericColumn(int ColumnNumber,clsTxtBox.TypeEnum ColumnType,int IntegerPrecision, int DecimalPrecision,bool isCurrency)
		{
			try
			{
				arlNumericColumNum.Add(ColumnNumber);
				arlNumericColumnType.Add(ColumnType);
				arlNumericColIntSize.Add(IntegerPrecision);
				arlNumericColFloatSize.Add(DecimalPrecision);
				arlNumericColCurrency .Add (isCurrency);
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

        public bool SetNumericColumnFixed(int ColumnNumber, clsTxtBox.TypeEnum ColumnType, int IntegerPrecision, int DecimalPrecision,bool IsFixedLength)
        {
            try
            {
                arlNumericColumNum.Add(ColumnNumber);
                arlNumericColumnType.Add(ColumnType);
                arlNumericColIntSize.Add(IntegerPrecision);
                arlNumericColFloatSize.Add(DecimalPrecision);
                arlNumericColCurrency.Add(isCurrency);
                arlFixedLengthNumCols.Add(IsFixedLength);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        
		
		public bool SetRefreshColumns()
		{
			try
			{
				arlNumericColumNum.Clear();
				arlNumericColumnType.Clear();
				arlNumericColIntSize.Clear();
				arlNumericColFloatSize.Clear();
				arlNumericColCurrency .Clear();
				intDisableColumnNum=null;
				intReadOnlyColumnNum= null; 
				intRightAlignColumnNum= null;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}
		
		public bool SetDisableColumns(params int[] ColumnNums)
		{
			try
			{
				intDisableColumnNum = ColumnNums;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		
		public bool SetReadOnlyColumns(params int[] ColumnNums)
		{
			try
			{
				intReadOnlyColumnNum = ColumnNums;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Set the column numbers you want to make the text right aligned.
		/// Column number starts from 0 (including MASK columns)
		/// </summary>
		/// <param name="ColumnNums">Set the multiple column numbers</param>
		/// <returns>True if it sets properly</returns>
		public bool SetRightAlignColumns(params int[] ColumnNums)
		{
			try
			{
				intRightAlignColumnNum = ColumnNums;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		
		public bool SetDateTimeColumns(params int[] ColumnNums)
		{
			try
			{
				intDateTimeColumnNum = ColumnNums;
				intNumDateTimeCols = ColumnNums.Length;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		
		public bool SetButtonColumns(params int[] ColumnNums)
		{
			try
			{
				intButtonColumnNum = ColumnNums;
				intNumButtonCols = ColumnNums.Length;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		
		public bool SetLangIDs(params string[] LangIDs)
		{
			try
			{
				strLangIDs = LangIDs;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		
		public void SetParam(string ProcParamName,string ProcParamVal,string strIN_OUT)
		{
            if (strIN_OUT.ToUpper() == "IN")
            {
                ProcParamNames.Add(ProcParamName);
                ProcParamValues.Add(ProcParamVal.Trim());
            }
            else
            {
                ProcParamNames.Add(ProcParamName);
                ProcParamValues.Add(null);
                arlOutParam.Add(ProcParamName);
            }
        }

		
		public bool DesignGrid(ref string strErrMsg)
		{
			try
			{
               
				this.ReadOnly = false;
				if(ExecuteProcedure(strSQLProcName,boolIsSQL,ref strErrMsg) == false)
				{
					return false;
				}
				if(SetColumns(ref strErrMsg) == false)
				{
					return false;
				}
				if(DesignTableStyle(ref strErrMsg) == false)
				{
					return false;
				}

                //this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                //for (int i = 0; i < this.Columns.Count; i++)
                //{
                //    if (this.Columns[i].Visible)
                //        this.InvalidateColumn(i);
                //}

                
                CurrencyManager cm = (CurrencyManager)this.BindingContext[this.DataSource];
                ((DataView)cm.List).AllowNew = false;
               // this.CurrentCellChanged += new EventHandler(clsWritableGrid_CurrentCellChanged);
                //if (AllowPreferrentialReorderingOnLoad)
                //    DoPreferrentialReOrdering();
                if (HeaderMapping)
                {
                    this.AssociatedHeaderExtender.Header.RepositionGridHeaders();
                }
                DoCreateFilterHeaders();
				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message;
				return false;
			}
		}

		
		
		public bool DesignGrid(string SQLProcName,ref string strErrMsg)
		{
			try
			{
				this.ReadOnly = false;
				//this.CurrentCellChanged-=new EventHandler(clsWritableGrid_CurrentCellChanged);  
				strSQLProcName = SQLProcName;
				boolIsSQL = true;

				if(ExecuteProcedure(SQLProcName,boolIsSQL,ref strErrMsg) == false)
				{
					return false;
				}
				if(SetColumns(ref strErrMsg) == false)
				{
					return false;
				}
				if(DesignTableStyle(ref strErrMsg) == false)
				{
					return false;
				}
                 CurrencyManager cm = (CurrencyManager)this.BindingContext[this.DataSource];
                ((DataView)cm.List).AllowNew = false;
             //   this.CurrentCellChanged+=new EventHandler(clsWritableGrid_CurrentCellChanged); 

                DoCreateFilterHeaders();
				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Message + " - " + ex.Source;
				return false;
			}
		}

        



		
		
		public bool PopulateGrid(ref string strErrMsg)
		{
			try
			{
				if(ExecuteProcedure(strSQLProcName,boolIsSQL,ref strErrMsg) == false)
				{
					return false;
				}
				if(SetColumns(ref strErrMsg) == false)
				{
					return false;
				}
				if(DesignTableStyle(ref strErrMsg) == false)
				{
					return false;
				}

				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message;
				return false;
			}
		}

		private bool ExecuteProcedure(string SQLProcName,bool isSQL,ref string strErrMsg)
		{
			try
			{
                Query objQuery = new Query();

                if (!isSQL)
                {
                    string[] arrStrProcParamNames = new string[0];
                    string[] arrStrProcParamValues = new string[0];
                    if (ProcParamNames.Count - arlOutParam.Count > 0)
                    {
                        arrStrProcParamNames = new string[ProcParamNames.Count - arlOutParam.Count];
                        arrStrProcParamValues = new string[ProcParamNames.Count - arlOutParam.Count];
                        for (int i = 0; i < ProcParamNames.Count; i++)
                        {
                            if (ProcParamValues[i] != null)
                            {
                                arrStrProcParamNames[i] = ProcParamNames[i].ToString();
                                arrStrProcParamValues[i] = ProcParamValues[i].ToString();
                            }
                        }
                        objQuery.SetInputParameterNames(SQLProcName, arrStrProcParamNames);
                        objQuery.SetInputParameterValues(arrStrProcParamValues);
                    }

                    arrStrProcParamNames = new string[arlOutParam.Count];
                    arrStrProcParamValues = new string[arlOutParam.Count];
                    int iOutParamIndex = 0;
                    for (int i = 0; i < ProcParamNames.Count; i++)
                    {
                        if (ProcParamValues[i] == null)
                        {
                            arrStrProcParamNames[iOutParamIndex++] = ProcParamNames[i].ToString();
                        }
                    }
                    objQuery.SetOutputParameterNames(SQLProcName, arrStrProcParamNames);

                    dsSource = objQuery.ExecuteQueryProcedure(SQLProcName);
                }
                else
                {
                    dsSource = objQuery.ExecuteQueryCommand(SQLProcName);
                }

                dtSource = dsSource.Tables[0].Copy();
                dsSource.Dispose();

                intColCount = dtSource.Columns.Count;
                strColNames = new string[intColCount];

                for (int i = 0; i < intColCount; i++)
                    strColNames[i] = dtSource.Columns[i].ColumnName;


                if (isSQL == false)
                {
                    strProcOutValue = new string[arlOutParam.Count];
                    for (int i = 0; i < arlOutParam.Count; i++)
                    {
                        strProcOutValue[i] = objQuery.GetOutputParameterValue(arlOutParam[i].ToString());
                    }
                }


                dtSource = dsSource.Tables[0];

                dvSource = dtSource.DefaultView;
            
                dvSource.AllowNew = false;
                intTotalRowCount = dvSource.Count;					//INDRANIL

                dtSource.Columns.Add("MASK_LOAD_COL_VALUE");
				
				for(int i=0; i<dtSource.Rows.Count; i++)
                    dtSource.Rows[i]["MASK_LOAD_COL_VALUE"] = "0";

				intColCount = dtSource.Columns.Count - 1;
				
				MyGridTextBoxColumn._RowCount = dtSource.Rows.Count;

				ChangeDataTable();

				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Message + " - " + ex.Source;
				return false;
			}
		}

        DataTable dtFullData;
        public DataTable FullData
        {
            set
            {
                if (value != null)
                    dtFullData = value.Copy();
            }

            get
            {
                if (dtFullData != null)
                    return dtFullData.Copy();
                else
                    return null;
            }
        }

		private void ChangeDataTable()
        {
       

            try
            {
                CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
                NumberFormatInfo nmfi = new NumberFormatInfo();
                nmfi = ci.NumberFormat;
                ci.ClearCachedData();
                int m_intColPos = -1;
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    if (CheckNumericColumn(i, ref m_intColPos))
                    {
                        clsTxtBox.TypeEnum objEnum = (clsTxtBox.TypeEnum)arlNumericColumnType[m_intColPos];
                        if (objEnum == clsTxtBox.TypeEnum.Float)
                        {
                            int m_intFloatSize = (int)arlNumericColFloatSize[m_intColPos];
                            for (int j = 0; j < dvSource.Count; j++)
                            {
                               // changed on 14/05/07 by Nilanjan (inclusion of group seperarors in wrong location
                                if (dvSource[j][i].ToString() != "")
                                {

                                    string prefix = "";
                                    string strFloat = dvSource[j][i].ToString();
                                    strFloat = strFloat.Replace(".", nmfi.NumberDecimalSeparator);
                                    decimal m_decTemp = Math.Round(Convert.ToDecimal(strFloat, nmfi), m_intFloatSize);
                                    string rep = m_decTemp.ToString(nmfi);
                                    int groupsize = nmfi.NumberGroupSizes[0];
                                    int dotIndex = rep.LastIndexOf(Convert.ToChar(nmfi.NumberDecimalSeparator));
                                    string DecimalPart = "", integerPart;
                                    if (dotIndex == -1)
                                    {
                                        dotIndex = rep.Length;
                                        DecimalPart = rep.Substring(dotIndex);
                                        integerPart = rep.Substring(0, dotIndex);

                                        int dpres = nmfi.NumberDecimalDigits;
                                        string tmp = String.Empty;

                                        for (int k = 0; k < dpres; k++)
                                            tmp = tmp + "0";

                                        DecimalPart = tmp;


                                    }
                                    else
                                    {

                                        DecimalPart = rep.Substring(dotIndex+1);
                                        integerPart = rep.Substring(0, dotIndex);
                                        
                                        if (integerPart.Contains(nmfi.NegativeSign))
                                        {
                                            prefix = "-";
                                            integerPart = integerPart.Remove(0, 1);
                                        }

                                        if (integerPart.Length >= nmfi.NumberDecimalDigits)
                                        {
                                            int temp = integerPart.Length;
                                            for (int k = groupsize; k < temp; k += groupsize)
                                            {

                                                integerPart = integerPart.Insert(temp - k, nmfi.NumberGroupSeparator);

                                            }
                                        }

                                        rep = prefix + integerPart + nmfi.NumberDecimalSeparator + DecimalPart;


                                    }
                                    //.......

                                    dvSource[j][i] = rep;
                                }
                            }
                        }
                    }
                    
                }
                dvSource.Table.AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source + " - " + ex.Message);
                return;
            }
		}

		private bool SetColumns(ref string strErrMsg)
		{
			try
			{
				strColNames = new string[dtSource.Columns.Count];
				strHeadings = new string[dtSource.Columns.Count];

				for(int i=0; i < dtSource.Columns.Count; i++)
				{
					strColNames[i] = dtSource.Columns[i].ColumnName;
					strHeadings[i] = strColNames[i].Replace("_"," ") + " ";
				}
				ChangeLanguage();
				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message; 
				return false;
			}
		}

		private void ChangeLanguage()
		{
			try
			{
                string strTmp = "";
                clsLanguage objLang = new clsLanguage();
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    if (strLangID[i] != null)
                    {
                        strTmp = objLang.LanguageString(strLangID[i]);

                        if (strTmp != "*****")
                            strHeadings[i] = strTmp + " ";
                    }


                }
                objLang.Dispose();
            }
            catch (Exception ex)
			{
				string test = ex.Message;
			}
		}

		#region Enabling or Disabling Cell
		ArrayList cellDisableList = new ArrayList();


		#endregion


		//changing cell color
		ArrayList cellColorList = new ArrayList ();
		public void SetCellColor(int row,int col,Brush brus)
		{
			
			DataGridCellColorEventArgs temp = new DataGridCellColorEventArgs (row,col,brus);
			cellColorList.Add (temp);
			

		}

		public void SetRowColor(int row,Brush brus)
		{
			
			for(int i=0;i<ColumnCount ;i++)
			{
				DataGridCellColorEventArgs temp = new DataGridCellColorEventArgs (row,i,brus);
				cellColorList.Add (temp);
				
			}
		}
		public void RemoveCellColor(int row,int col)
		{
			
			for(int i=0;i<cellColorList.Count;i++)
			{
				DataGridCellColorEventArgs t =(DataGridCellColorEventArgs) cellColorList[i];
				if((t.Row == row)&&(t.Column == col))
				{
					cellColorList.RemoveAt (i);
					i--;
				}
			}
		}

		public void RemoveRowColor(int row)
		{
			for(int i=0;i<cellColorList.Count;i++)
			{
				DataGridCellColorEventArgs t =(DataGridCellColorEventArgs) cellColorList[i];
				if(t.Row == row)
				{
					cellColorList.RemoveAt (i);
					i--;
				}
			}
		}

		private void dgTBCol_CheckCellColor(object sender,DataGridCellColorEventArgs e)
		{
			if(cellColorList .Count != 0)
			{
				for(int k=0;k<cellColorList.Count ;k++)
				{
					DataGridCellColorEventArgs  objTemp =(DataGridCellColorEventArgs) cellColorList[k];
					if((e.Column ==objTemp.Column  )&&(e.Row ==objTemp.Row))
					{
						e.EnableColor = objTemp.EnableColor ;
					}
					objTemp = null;
				}
			}
			
		}

		#region Changing for Individual Column Sizing
		ArrayList listOfColumnsForResizing= new ArrayList ();
		public void SetWidthOfColumn(int ColoumnIndex,int Width)
		{
			colWidth objColWidth = new colWidth(ColoumnIndex,Width);
			listOfColumnsForResizing.Add (objColWidth);
		}

		#endregion

		#region for row status

        string oldValue = "", newValue = "", orgStatus = "", newStatus = "";

        private int _colReplacing = -1;
        //private int intLoadRowCount = 0;
        //private int intTotalRowCount = 0;
        //private int intColCount = 0;

        private void SetRowStatus(object sender)
        {
            try
            {
                DataGridViewCell dgcc = this.CurrentCell;
                //TextBox tb = null;

                if (_colReplacing == -1)
                {
                    //_colReplacing = dgcc.ColumnNumber;
                    _colReplacing = dgcc.ColumnIndex;
                }

                newValue = GetNewValue(_colReplacing, sender);

                if (newValue != null)
                {
                    oldValue = this[dgcc.RowIndex, dgcc.ColumnIndex].ToString();
                    orgStatus = dvSource[dgcc.RowIndex][intColCount].ToString();

                    //newValue = tb.Text;
                    //if ((dgcc.RowNumber != lastEditedRow || _colReplacing != lastEditedCol) && orgStatus == "0")
                    //{
                    if (newValue != oldValue)
                    {
                        if (orgStatus == "0")
                            dvSource[dgcc.RowIndex][intColCount] = "2"; //newStatus;

                        //if (orgStatus == "0" || orgStatus == "2")
                        //    newStatus = "2";
                        //else
                        //    newStatus = "1";

                        boolTextChange = true;

                        //lastEditedRow = dgcc.RowNumber;
                        //lastEditedCol = _colReplacing;
                    }
                    //}
                }

                _colReplacing = -1;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
		

        private string GetNewValue(int col, object sender)
		{
			if(CheckComboBoxColumn(get_actual_colum_index(col)))
			{

                //DataGridComboBoxColumn tbc = (DataGridComboBoxColumn)sender;
				
                //if(tbc != null)
                //{
					
                //    return tbc.TextBox.Text;
                //}

                ComboBox tbc = (ComboBox)sender;

                if (tbc != null)
                {

                    return tbc.SelectedItem.ToString();
                }
			}
			else if(CheckDateTimeColumn(col))
			{
                DataGridDateTimePicker tbc = (DataGridDateTimePicker)sender;
				if(tbc != null && tbc.ColumnDateTimePicker.TxtChange)
				{
					return tbc.TextBox.Text;
				}


			}
			else
			{

                TextBox tb = (TextBox)sender;
				if(tb != null)
				{
					return tb.Text;
				}
			}

			return null;
		}

        //string oldValue = "", newValue = "", orgStatus = "", newStatus = "";
       


		#endregion
      
		private bool DesignTableStyle(ref string strErrMsg)
		{
			try
			{
				string m_strMaskCols = "", m_strShowCols = "";
                this.AutoGenerateColumns = false;
                
                this.DataSource = dtSource;
                this.EditingControlShowing -= new DataGridViewEditingControlShowingEventHandler(clsSearchGrid_EditingControlShowing);
				////**** If the Default design is false then setting Colors for that Grid
                this.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(clsSearchGrid_EditingControlShowing);
                //this.ColumnHeadersDefaultCellStyle.BackColor=System.Drawing.Color.Lavender;
                //this.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Navy;
				//this.GridColor=System.Drawing.Color.Silver;
                if (this.Columns.Count > 0)
                {
                    this.Columns.Clear();
                   
                    
                   

                }
				for(int i=0; i < dtSource.Columns.Count; i++)
				{
					int local0 = 0;
                    if (strColNames[i].ToUpper().IndexOf("MASK") == 0)
                        continue;

                    if (CheckComboBoxColumn(i))
                    {
                        
                        DataGridViewComboBoxColumn dgCBCol = new DataGridViewComboBoxColumn();
                        //dgCBCol.HeaderText = dtSource.Columns[i].ColumnName;
                        dgCBCol.HeaderText = strHeadings[i];
                        dgCBCol.DataPropertyName = dtSource.Columns[i].ColumnName;
                        dgCBCol.DataSource = GetComboValue(i);
                        //dgCBCol.DefaultCellStyle.NullValue = "Choose a value";
                        dgCBCol.Width = 200;
                        dgCBCol.DisplayStyleForCurrentCellOnly = true;
                        //comboforall = new ComboBox();
                        if (CheckReadOnlyColumn(i))
                        {
                            dgCBCol.ReadOnly = true; 
                        }

                        this.Columns.Insert(this.Columns.Count, dgCBCol);   
                    }
                    else if (CheckCheckBoxColumn(i))
                    {
                        DataGridViewCheckBoxColumn dgChkCol = new DataGridViewCheckBoxColumn(false);
                        if (dgChkCol.Displayed)
                        {
                        }
                        else
                        {
                            dgChkCol.Visible = true;
                        }
                        dgChkCol.HeaderText = strHeadings[i];
                        dgChkCol.DataPropertyName = dtSource.Columns[i].ColumnName;
                        dgChkCol.CellTemplate = new DataGridViewCheckBoxCell();
                        //this.CellClick += new DataGridViewCellEventHandler(clsWritableGrid_CellClick);
                       // this.CurrentCellDirtyStateChanged += new EventHandler(clsWritableGrid_CurrentCellDirtyStateChanged);
                        this.CellContentClick += new DataGridViewCellEventHandler(clsWritableGrid_CellContentClick);
                        this.Columns.Add(dgChkCol);
                        //for (int j = 0; j < dtSource.Rows.Count; j++)
                        //{
                        //    dgChkCol.CellTemplate.Selected = true;
                        //}                           
                    }
                    else if (CheckDateTimeColumn(i))
                    {
                        CalendarColumn dgDTPCol = new CalendarColumn();
                        dgDTPCol.HeaderText = strHeadings[i];
                        dgDTPCol.DataPropertyName = dtSource.Columns[i].ColumnName;
                        if (CheckReadOnlyColumn(i))
                        {
                            dgDTPCol.ReadOnly = true;

                        }

                        this.Columns.Add(dgDTPCol);
                    }
                    else if (CheckButtonColumn(i))
                    {
                        DataGridViewButtonColumn dgBtnCol = new DataGridViewButtonColumn();
                        dgBtnCol.DataPropertyName = dtSource.Columns[i].ColumnName;
                        dgBtnCol.HeaderText = strHeadings[i];
                        this.Columns.Add(dgBtnCol);

                    }
                    else
                    {
                        DataGridViewTextBoxColumn dgTBCol = new DataGridViewTextBoxColumn();
                        dgTBCol.DataPropertyName = dtSource.Columns[i].ColumnName;
                        dgTBCol.HeaderText = strHeadings[i];
                        
                        


                        if (CheckReadOnlyColumn(i))
                        {
                            dgTBCol.ReadOnly = true;
                        }

                        if (CheckRightAlignColumn(i))
                        {
                            //dgTBCol.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgTBCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            //dgTBCol.DataGridView.AlternatingRowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                        else
                        {
                            //dgTBCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            //dgTBCol.DataGridView.AlternatingRowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        if (CheckNumericColumn(i, ref local0))
                        {
                            dgTBCol.DefaultCellStyle.Format = "N" +arlNumericColFloatSize[local0];
                            CultureInfo inf = new CultureInfo(CultureInfo.CurrentCulture.LCID, true);
                            inf.NumberFormat.NumberGroupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
                            inf.NumberFormat.NumberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                            dgTBCol.DefaultCellStyle.FormatProvider = inf;
                            //dgTBCol.DefaultCellStyle.Format = "F2";

                            ////dgTBCol.deDefaultCellStyle.NullValue = 0.00;
                            ////this[this.CurrentRowIndex, i].DefaultValue = 0.00;

                            //***
                            if ((Boolean)arlFixedLengthNumCols[local0])
                            {
                                dgTBCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dgTBCol.MaxInputLength = 21;
                                dgTBCol.MinimumWidth = 115;
                                dgTBCol.Width = 115;
                                //dgTBCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                            }
                            ////****

                        }
                        else
                        {
                            //dgTBCol.Width = 200;
                            //dgTBCol.Resizable = DataGridViewTriState.False;
                            
                            //dgTBCol.TextType = clsTxtBox.TypeEnum.String;
                            //dgTBCol.TextBox.MaxLength = getMaxStrLength(i);
                            //dgTBCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        }
                        //dgTBCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                        this.Columns.Insert(this.Columns.Count, dgTBCol);
                    }
                    //this.Columns[i].HeaderText = strHeadings[i];
				}

				
			

				//#region changing for ColSizing







                //for (int k = 0; k < this.Columns.Count; k++)
                //{
                //    this.Columns[k].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                //}
                this.Font = new Font(this.Font, FontStyle.Regular);
                this.Invalidate();
                this.Refresh();
			

				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message;
				return false;
			}
		}

        bool b = true;
        void clsWritableGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                bool check = Convert.ToBoolean(((DataGridViewCheckBoxCell)this.Rows[this.CurrentCell.RowIndex].Cells[this.CurrentCell.ColumnIndex]).EditingCellFormattedValue);
                DataGridViewCheckChangedEventArgs changed = new DataGridViewCheckChangedEventArgs(check);
                if (b)
                {
                    b = false;
                    if (checkChanged != null)
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)this.Rows[this.CurrentCell.RowIndex].Cells[this.CurrentCell.ColumnIndex];
                        checkChanged(chk, changed);
                    }
                }
                else
                {
                    b = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        

        void clsWritableGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            try
            {
               
                Type t = e.Control.GetType();
                comboedit = (DataGridViewComboBoxEditingControl)e.Control;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            

        }

        
		private void HorizontalScroll(object sender, EventArgs e)
		{
			this.Select();
		}


        //protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
        //{
        //    e.CellStyle.BackColor = Color.White;
        //    base.OnEditingControlShowing(e);
        //}

		
		

		private void SetEnableValues(object sender, DataGridEnableEventArgs e)
		{
			if(CheckDisableColumn(e.Column) && e.Row < intLoadRowCount)
			{
				e.EnableValue = false;
			}
			else
			{
				e.EnableValue = true;
			}

			#region For Cell Enabling and Disabling




			#endregion
			
		}

		
		public void ReplaceValue(int RowNum, int ColNum,string ReplaceString)
		{
            //Changed 26 06 2008
            
            int _colNum = this.CurrentCell.ColumnIndex;
            int _rowNum = this.CurrentCell.RowIndex;
            dvSource[this.CurrentRow.Index][this.CurrentCell.ColumnIndex] = this.Rows[this.CurrentRow.Index].Cells[this.CurrentCell.ColumnIndex].EditedFormattedValue.ToString();
            dvSource.Table.AcceptChanges();
            this.DataSource = dvSource.Table;
            this.Update();
            //this.Refresh();
            
            //--------------------
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
            NumberFormatInfo nmfi = new NumberFormatInfo();
            nmfi = ci.NumberFormat;
            ci.ClearCachedData();

             
            int m_intColPos = -1;
            if (CheckNumericColumn(ColNum, ref m_intColPos))
            {
                clsTxtBox.TypeEnum objEnum = (clsTxtBox.TypeEnum)arlNumericColumnType[m_intColPos];


                if (objEnum == clsTxtBox.TypeEnum.Float)
                {
                    #region Float
                    if (ReplaceString == "")
                        ReplaceString = "0";
                    int m_intPrecision = (int)arlNumericColFloatSize[m_intColPos];

                    Decimal m_decTemp = Math.Round(Convert.ToDecimal(ReplaceString, nmfi), m_intPrecision);
                    string m_strTemp = "0", m_strLocal = nmfi.NumberDecimalSeparator;

                    for (int k = 0; k < m_intPrecision; k++)
                        m_strLocal += "0";

                    m_strTemp += m_strLocal;
                    //string rep = m_decTemp.ToString("###" + m_strTemp + ";",nmfi); //###" + m_strTemp + ";"+ m_strLocal);
                    string rep = m_decTemp.ToString(nmfi); //###" + m_strTemp + ";"+ m_strLocal);

                    //int ind = ReplaceString.LastIndexOf(".");//Change On 19/1/2007
                    int ind = ReplaceString.LastIndexOf(nmfi.NumberDecimalSeparator);
                    if (ind < 0)
                    {
                    }
                    else
                    {
                        string originalIntPortion = ReplaceString.Substring(0, ind);
                        if (originalIntPortion == "") originalIntPortion = "0";
                        string nextIntPortion = rep.Substring(0, rep.LastIndexOf(nmfi.NumberDecimalSeparator));
                        if (nextIntPortion == "") nextIntPortion = "0";
                        rep = rep.Replace(nextIntPortion, originalIntPortion);
                    }
                    dvSource[RowNum][ColNum] = rep;

                    #endregion
                }

                else
                {
                    dvSource[RowNum][ColNum] = ReplaceString;
                }
            }
            else
            {
                dvSource[RowNum][ColNum] = ReplaceString;
            }
            dvSource.Table.AcceptChanges();
            this.DataSource = dvSource.Table;
            this.Update();
            this.Refresh();
            
            //DataGridViewCell cell=this.
            //this.CurrentCell = this.Rows[RowNum].Cells[get_actual_colum_index(ColNum)];
            //Changed 26 06 2008

            this.CurrentCell = this.Rows[_rowNum].Cells[get_actual_colum_index(_colNum)];
            
            //this.CurrentCell.DataGridView.EditingControl.te
            //EditingControl cntrl = (EditingControl)this.CurrentCell.DataGridView.EditingControl;
            
            
            //-------------------------------------
            this.BeginEdit(true);
            //this.SelectedRows[RowNum].Cells[ColNum].Selected = true;


            CultureInfo inf = new CultureInfo(CultureInfo.CurrentCulture.LCID, true);
            inf.NumberFormat.NumberGroupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            inf.NumberFormat.NumberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
           this.DefaultCellStyle.FormatProvider = inf;

            this.DefaultCellStyle.FormatProvider = inf;
		}

        public void ReplaceVal(int RowNum, int ColNum, string ReplaceString)
        {
            //Changed 26 06 2008

            int _colNum = this.CurrentCell.ColumnIndex;
            int _rowNum = this.CurrentCell.RowIndex;
            //dvSource[this.CurrentRow.Index][this.CurrentCell.ColumnIndex] = this.Rows[this.CurrentRow.Index].Cells[this.CurrentCell.ColumnIndex].EditedFormattedValue.ToString();
            //dvSource.Table.AcceptChanges();
            //this.DataSource = dvSource.Table;
            //this.Update();
            //this.Refresh();

            //--------------------
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
            NumberFormatInfo nmfi = new NumberFormatInfo();
            nmfi = ci.NumberFormat;
            ci.ClearCachedData();


            int m_intColPos = -1;
            if (CheckNumericColumn(ColNum, ref m_intColPos))
            {
                clsTxtBox.TypeEnum objEnum = (clsTxtBox.TypeEnum)arlNumericColumnType[m_intColPos];


                if (objEnum == clsTxtBox.TypeEnum.Float)
                {
                    #region Float
                    if (ReplaceString == "")
                        ReplaceString = "0";
                    int m_intPrecision = (int)arlNumericColFloatSize[m_intColPos];

                    Decimal m_decTemp = Math.Round(Convert.ToDecimal(ReplaceString, nmfi), m_intPrecision);
                    string m_strTemp = "0", m_strLocal = nmfi.NumberDecimalSeparator;

                    for (int k = 0; k < m_intPrecision; k++)
                        m_strLocal += "0";

                    m_strTemp += m_strLocal;
                    //string rep = m_decTemp.ToString("###" + m_strTemp + ";",nmfi); //###" + m_strTemp + ";"+ m_strLocal);
                    string rep = m_decTemp.ToString(nmfi); //###" + m_strTemp + ";"+ m_strLocal);

                    //int ind = ReplaceString.LastIndexOf(".");//Change On 19/1/2007
                    int ind = ReplaceString.LastIndexOf(nmfi.NumberDecimalSeparator);
                    if (ind < 0)
                    {
                    }
                    else
                    {
                        string originalIntPortion = ReplaceString.Substring(0, ind);
                        if (originalIntPortion == "") originalIntPortion = "0";
                        string nextIntPortion = rep.Substring(0, rep.LastIndexOf(nmfi.NumberDecimalSeparator));
                        if (nextIntPortion == "") nextIntPortion = "0";
                        rep = rep.Replace(nextIntPortion, originalIntPortion);
                    }
                    dvSource[RowNum][ColNum] = rep;

                    #endregion
                }

                else
                {
                    dvSource[RowNum][ColNum] = ReplaceString;
                }
            }
            else
            {
                dvSource[RowNum][ColNum] = ReplaceString;
            }
            //dvSource.Table.AcceptChanges();
            this.DataSource = dvSource.Table;
            this.Update();
            this.Refresh();

            //DataGridViewCell cell=this.
            //this.CurrentCell = this.Rows[RowNum].Cells[get_actual_colum_index(ColNum)];
            //Changed 26 06 2008

            this.CurrentCell = this.Rows[_rowNum].Cells[get_actual_colum_index(_colNum)];

            //this.CurrentCell.DataGridView.EditingControl.te
            //EditingControl cntrl = (EditingControl)this.CurrentCell.DataGridView.EditingControl;


            //-------------------------------------
            this.BeginEdit(true);
            //this.SelectedRows[RowNum].Cells[ColNum].Selected = true;


            CultureInfo inf = new CultureInfo(CultureInfo.CurrentCulture.LCID, true);
            inf.NumberFormat.NumberGroupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            inf.NumberFormat.NumberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            this.DefaultCellStyle.FormatProvider = inf;

            this.DefaultCellStyle.FormatProvider = inf;
        }


        public void ReplaceValueWithFocus(int RowNum, int ColNum, string ReplaceString)
        {
            //Changed 26 06 2008
            int _colNum = this.CurrentCell.ColumnIndex;
            int _rowNum = this.CurrentCell.RowIndex;
            dvSource[this.CurrentRow.Index][get_actual_colum_index(this.CurrentCell.ColumnIndex)] = this.Rows[this.CurrentRow.Index].Cells[this.CurrentCell.ColumnIndex].EditedFormattedValue.ToString();
            dvSource.Table.AcceptChanges();
            this.DataSource = dvSource.Table;
            this.Update();
            this.Refresh();

            //--------------------
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
            NumberFormatInfo nmfi = new NumberFormatInfo();
            nmfi = ci.NumberFormat;
            ci.ClearCachedData();
            int m_intColPos = -1;
            if (CheckNumericColumn(ColNum, ref m_intColPos))
            {
                clsTxtBox.TypeEnum objEnum = (clsTxtBox.TypeEnum)arlNumericColumnType[m_intColPos];


                if (objEnum == clsTxtBox.TypeEnum.Float)
                {
                    #region Float
                    if (ReplaceString == "")
                        ReplaceString = "0";
                    int m_intPrecision = (int)arlNumericColFloatSize[m_intColPos];

                    Decimal m_decTemp = Math.Round(Convert.ToDecimal(ReplaceString, nmfi), m_intPrecision);
                    string m_strTemp = "0", m_strLocal = nmfi.NumberDecimalSeparator;

                    for (int k = 0; k < m_intPrecision; k++)
                        m_strLocal += "0";

                    m_strTemp += m_strLocal;
                    //string rep = m_decTemp.ToString("###" + m_strTemp + ";",nmfi); //###" + m_strTemp + ";"+ m_strLocal);
                    string rep = m_decTemp.ToString(nmfi); //###" + m_strTemp + ";"+ m_strLocal);

                    //int ind = ReplaceString.LastIndexOf(".");//Change On 19/1/2007
                    int ind = ReplaceString.LastIndexOf(nmfi.NumberDecimalSeparator);
                    if (ind < 0)
                    {
                    }
                    else
                    {
                        string originalIntPortion = ReplaceString.Substring(0, ind);
                        if (originalIntPortion == "") originalIntPortion = "0";
                        string nextIntPortion = rep.Substring(0, rep.LastIndexOf(nmfi.NumberDecimalSeparator));
                        if (nextIntPortion == "") nextIntPortion = "0";
                        rep = rep.Replace(nextIntPortion, originalIntPortion);
                    }
                    dvSource[RowNum][ColNum] = rep;

                    #endregion
                }

                else
                {
                    dvSource[RowNum][ColNum] = ReplaceString;
                }
            }
            else
            {
                dvSource[RowNum][ColNum] = ReplaceString;
            }
            dvSource.Table.AcceptChanges();
            this.DataSource = dvSource.Table;
            this.Update();
            this.Refresh();

            //DataGridViewCell cell=this.
            this.CurrentCell = this.Rows[RowNum].Cells[get_actual_colum_index(ColNum)];
            //Changed 26 06 2008
            SetRowStatus(RowNum);
            //this.CurrentCell = this.Rows[_rowNum].Cells[get_actual_colum_index(_colNum)];
            ////this.CurrentCell.DataGridView.EditingControl.te
            //EditingControl cntrl = (EditingControl)this.CurrentCell.DataGridView.EditingControl;


            //-------------------------------------
            this.BeginEdit(true);
            //this.SelectedRows[RowNum].Cells[ColNum].Selected = true;
        }

		public void ReplaceValue(int ColNum,string ReplaceString)
		{
            //if(this.CurrentRowIndex == -1)
            //    return;
            if (this.CurrentCell.RowIndex == -1)
                return;
            dvSource[this.CurrentCell.RowIndex][ColNum] = ReplaceString;
            dvSource.Table.AcceptChanges();
            this.Refresh();
            SetRowStatus(this.CurrentCell.RowIndex);
		}

		public bool CheckNumericColumn(int ColumnNum,ref int ColPosition)
		{
			for(int i=0; i < arlNumericColumNum.Count; i++)
			{
				if(arlNumericColumNum[i].ToString() == ColumnNum.ToString())
				{
					ColPosition = i;
					return true;
				}
			}
			ColPosition = -1;
			return false;
		}

		private bool CheckComboBoxColumn(int ColumnNum,ref int Pos)
		{
			Pos = -1;
			if(arlComboCols == null)
				return false;
			
			for(int i=0; i < arlComboCols.Count; i++)
			{
				if(arlComboCols[i].ToString() == ColumnNum.ToString())
				{
					Pos = i;
					return true;
				}
			}

			return false;
		}

		private bool CheckComboBoxColumn(int ColumnNum)
		{
			if(arlComboCols == null)
				return false;
			
			for(int i=0; i < arlComboCols.Count; i++)
			{
                if (arlComboCols[i].ToString() == ColumnNum.ToString())
                {
                    //this.editingcontrol.Cursor = Cursors.Arrow;
                    return true;
                }
			}

			return false;
		}

        //CheckBox Column
        private bool CheckCheckBoxColumn(int ColumnNum)
        {
            if (arlCheckBoxColumns == null)
                return false;

            for (int i = 0; i < arlCheckBoxColumns.Count; i++)
            {
                if (arlCheckBoxColumns[i].ToString() == ColumnNum.ToString())
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckButtonColumn(int ColumnNum)
		{
			if(intButtonColumnNum == null)
				return false;

			for(int i=0; i < intButtonColumnNum.Length; i++)
			{
				if(intButtonColumnNum[i] == ColumnNum)
					return true;
			}

			return false;
		}
	
		private bool CheckDateTimeColumn(int ColumnNum, ref int Pos)
		{
			Pos = -1;
			if(intDateTimeColumnNum == null)
				return false;

			for(int i=0; i < intDateTimeColumnNum.Length; i++)
			{
				if(intDateTimeColumnNum[i] == ColumnNum)
				{
					Pos = i;
					return true;
				}
			}

			return false;
		}
	
		private bool CheckDateTimeColumn(int ColumnNum)
		{
			if(intDateTimeColumnNum == null)
				return false;

			for(int i=0; i < intDateTimeColumnNum.Length; i++)
				if(intDateTimeColumnNum[i] == ColumnNum)
					return true;

			return false;
		}

		private bool CheckReadOnlyColumn(int ColumnNum)
		{
			if(intReadOnlyColumnNum == null)
				return false;

			for(int i=0; i < intReadOnlyColumnNum.Length; i++)
				if(intReadOnlyColumnNum[i] == ColumnNum)
					return true;
			
			return false;		
		}

		private bool CheckRightAlignColumn(int ColumnNum)
		{
			if(intRightAlignColumnNum == null)
				return false;

			for(int i=0; i < intRightAlignColumnNum.Length; i++)
				if(intRightAlignColumnNum[i] == ColumnNum)
					return true;
			
			return false;		
		}

		private bool CheckDisableColumn(int ColumnNum)
		{
			if(intDisableColumnNum == null)
				return false;

			for(int i=0; i < intDisableColumnNum.Length; i++)
				if(intDisableColumnNum[i] == ColumnNum)
					return true;
			
			return false;		
		}

		private clsTxtBox.TypeEnum TxtType(string txtType)
		{
			if(txtType == clsTxtBox.TypeEnum.Integer.ToString())
				return clsTxtBox.TypeEnum.Integer;
			else if(txtType == clsTxtBox.TypeEnum.Float.ToString())
				return clsTxtBox.TypeEnum.Float;
			else
				return clsTxtBox.TypeEnum.String;
		}

		private string[] GetComboValue(int ColumnNum)
		{
			if(arlComboCols == null)
				return null;

			for(int i=0; i < arlComboCols.Count; i++)
			{
				if(arlComboCols[i].ToString() == ColumnNum.ToString())
				{
					return arrComboValues[i];
				}
			}

			return null;
		}

		private Color MyGetColorRowCol(int row, int col)
		{
			Color c = Color.Black;
			if(col != 2)
				return c;
			
			string m_strTemp = this.dvSource[row][3].ToString();
			if(m_strTemp != "1")
				c = Color.Blue;
			else
				c = Color.Red;

			m_strTemp = this.dvSource[row][5].ToString();

			if(m_strTemp.ToUpper() == "NA")
				c = Color.DarkGray;

			return c;
		}

	
		public bool Remove(int RowNum,ref string strErrMsg)
		{
			try
			{
                if ((dvSource.Count - 1) >= RowNum)
                {
                    dvSource.Delete(RowNum);
                    dvSource.Table.AcceptChanges();
                }
				else
				{
					strErrMsg = "The Row Number is not present in the datagrid.";
					return false;
				}
				
				this.Refresh();
				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message;
				return false;
			}
		}

		public bool RemoveAll(ref string strErrMsg)
		{
			try
			{
				strErrMsg = "";

				if( dtSource != null )
				{
					dtSource.Clear();

                    dtSource.AcceptChanges();

                    dvSource = dtSource.DefaultView;
                    dvSource.AllowNew = false;
                }

				intTotalRowCount = 0;
				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Message + "-" + ex.Source;
				return false;
			}
		}


	
		public bool Add(ref string strErrMsg)
		{
			try
			{
				object[] obj = new object[dtSource.Columns.Count];
				for(int i=0; i < obj.Length; i++)
				{
					if(i != obj.Length - 1)
						obj[i] = null;
					else
						obj[i] = "1";
				}

                dvSource.Sort = "";

                dtSource.Rows.Add(obj);
                
                dvSource.Table.AcceptChanges();
				intTotalRowCount++;
			
				this.Refresh();
				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message;
				return false;
			}
		}

		public void DisableColumn(int RowNum,int ColNo)
		{
			int m_intPos = -1;
			_toggleCol = ColNo;
			_toggleRow = RowNum;
			int i = CheckDisableColumn(RowNum,ColNo);
			if( i != -1)
			{
				arlEnableColumns.RemoveAt(i);
				arlEnableRows.RemoveAt(i);
			}
            if (CheckComboBoxColumn(ColNo, ref m_intPos))
            {//dgComboBox[m_intPos].CheckCellEnabled += new EnableCellEventHandler(DisableCell);
            }
            else if (CheckDateTimeColumn(ColNo, ref m_intPos))
                dgDateTimePicker[m_intPos].CheckCellEnabled += new EnableCellEventHandler(DisableCell);
            else
                dgTxt[ColNo].CheckCellEnabled += new EnableCellEventHandler(DisableCell);
			this.Refresh();
		}

		private int CheckDisableColumn(int RowNum,int ColNo)
		{
			for(int i=0; i < arlEnableColumns.Count; i++)
			{
				if(arlEnableColumns[i].ToString() == ColNo.ToString())
				{
					if(arlEnableRows[i].ToString() == RowNum.ToString())
						return i;
				}
			}
			return -1;
		}

		public void EnableColumn(int RowNum, int ColNo)
		{
			int m_intPos = -1;
			_toggleCol = ColNo;
			_toggleRow = RowNum;
			if(CheckDisableColumn(ColNo))
			{
				if(CheckEnableCols(RowNum,ColNo) == false)
				{
					arlEnableColumns.Add(ColNo);
					arlEnableRows.Add(RowNum);
				}
			}
            if (CheckComboBoxColumn(ColNo, ref m_intPos))
            {
             //   dgComboBox[m_intPos].CheckCellEnabled += new EnableCellEventHandler(EnableCell);
            }
            else if (CheckDateTimeColumn(ColNo, ref m_intPos))
                dgDateTimePicker[m_intPos].CheckCellEnabled += new EnableCellEventHandler(EnableCell);
            else
                dgTxt[ColNo].CheckCellEnabled += new EnableCellEventHandler(EnableCell);
			this.Refresh();
		}

		private int[] _DisableImmdCols;
		private int[] _EnableImmdCols;

		public void EnableImmediate(params int[] ColNum)
		{
			try
			{
				_EnableImmdCols = ColNum;


                for (int i = 0; i < ColNum.Length; i++)
                {
                    int colnum = ColNum[i];
                    //colnum = get_actual_colum_index(colnum);
                    this.Columns[colnum].ReadOnly = false;
                    this.Columns[colnum].DefaultCellStyle.BackColor = DefaultCellStyle.BackColor;
                }

                this.Refresh();
			}
			catch(Exception)
			{
				return;
			}
		}

		public void DisableImmediate(params int[] ColNum)
		{
			try
			{
				_DisableImmdCols = ColNum;

				for(int i=0; i < ColNum.Length ; i++)
				{
                    int colnum = ColNum[i];
                    //colnum = get_actual_colum_index(colnum);
                    this.Columns[colnum].ReadOnly = true;
                    //this.Columns[colnum].DefaultCellStyle.BackColor = Color.PapayaWhip;
				}

                this.Refresh();
			}
			catch(Exception)
			{
				return;
			}
		}
		
		private bool CheckDisableImmediate(int ColNum)
		{
			for(int i=0; i < _DisableImmdCols.Length ; i++)
			{
				if(_DisableImmdCols[i] == ColNum)
					return true;
			}
			
			return false;
		}

		private bool CheckEnableImmediate(int ColNum)
		{
			for(int i=0; i < _EnableImmdCols.Length ; i++)
			{
				if(_EnableImmdCols[i] == ColNum)
					return true;
			}
			return false;
		}

		private void DisableImmediateCols(object sender, DataGridEnableEventArgs e)
		{
			if(CheckDisableImmediate(e.Column))
			{
				e.EnableValue = false;
			}
			else
			{
				e.EnableValue = true;
			}
		}	

		private void EnableImmediateCols(object sender, DataGridEnableEventArgs e)
		{
			if(CheckEnableImmediate(e.Column))
			{
				e.EnableValue = true;
			}
		}

		private bool CheckEnableCols(int RowNum,int ColNo)
		{
			for(int i=0; i < arlEnableColumns.Count; i++)
			{
				if(arlEnableColumns[i].ToString() == ColNo.ToString())
				{
					if(arlEnableRows[i].ToString() == RowNum.ToString())
						return true;
				}
			}
			return false;
		}

		private void DisableCell(object sender,DataGridEnableEventArgs e)
		{
			if((e.Column == _toggleCol && e.Row == _toggleRow) || (CheckEnableCols(e.Row,e.Column) == false))
				e.EnableValue = false;
			else
			{
				e.EnableValue = true;
			}
		}

		private void EnableCell(object sender,DataGridEnableEventArgs e)
		{
			if((e.Column == _toggleCol && e.Row == _toggleRow) || CheckEnableCols(e.Row,e.Column))
				e.EnableValue = true;
		}

        //private void _DoNothingOnDataError(object sender, DataGridViewDataErrorEventArgs e)
        //{

        //}

        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            e.ThrowException = false;
            //base.OnDataError(displayErrorDialogIfNoHandler, e);
        }
        private void item_clicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                this.clsROGContextmenu.Hide();

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

                if (e.ClickedItem.Text == "Delete")
                {
                    //int rowtobdeleted = this.CurrentCell.RowIndex;
                    //DataTable dt = (DataTable)this.DataSource;
                    //dt.Rows.RemoveAt(rowtobdeleted);
                    //dt.AcceptChanges();
                    //this.DataSource = dt;
                    
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


        public int get_actual_colum_index(int index)
        {
            try
            {
                DataTable dt = (DataTable)this.DataSource;
                int exactcolumn = 0;
                int store = 0; ;
                for (int i = 0;i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.StartsWith("MASK"))
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





        public void AutoSizeCol()
        {
            try
            {
                if (intDisableColumnNum != null)
                {
                    for (int i = 0; i < intDisableColumnNum.Length; i++)
                    {

                        int col = get_unreal_colum_index(intDisableColumnNum[i]);
                        this.Columns[col].ReadOnly = true;
                        //this.Columns[col].DefaultCellStyle.BackColor = Color.PapayaWhip;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }

        }



        public bool Remove(ref string strErrMsg)
        {
            try
            {
                ArrayList intNums = new ArrayList();

                for (int i = 0; i < dvSource.Count; i++)
                {
                    if (this.IsSelected(i))
                    {
                        intNums.Add(i);
                    }
                }

                if (intNums.Count > 0)
                {
                    intNums.Sort();
                    intNums.Reverse();

                    for (int i = 0; i < intNums.Count; i++)
                    {
                        dvSource.Delete(Convert.ToInt32(intNums[i]));
                    }
                }

                dvSource.Table.AcceptChanges();
                this.Refresh();

                return true;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Source + " - " + ex.Message;
                return false;
            }
        }



        public string strInCommaFormat(string strParamTextinComma,int columnindex,ref int start)
        {
            try
            {
                int realindex =columnindex;
                strstoretext = strParamTextinComma;
                int noofcommas = 0;
                CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
                NumberFormatInfo nmfi = new NumberFormatInfo();
                nmfi = ci.NumberFormat;
                ci.ClearCachedData();
                string strTextinComma = strParamTextinComma;
                bool flag = false;
                string integerPart = "";
                string DecimalPart = "";

                
                int dotIndex = strTextinComma.LastIndexOf(Convert.ToChar(nmfi.NumberDecimalSeparator));
                int columnnumber = 0;
                for (columnnumber = 0; columnnumber < arlNumericColumNum.Count; columnnumber++)
                {
                    if ((int)arlNumericColumNum[columnnumber] == realindex)
                    {
                        break;
                    }
                }
                int DecimalPrecision = (int)arlNumericColFloatSize[columnnumber];
                int CommaPrecision = 3;
                if (dotIndex == -1)
                {
                   
                    dotIndex = strTextinComma.Length;
                    DecimalPart = strTextinComma.Substring(dotIndex);
                    integerPart = strTextinComma.Substring(0, dotIndex);
                    bool test;
                    if (arlNumericColCurrency.Count > columnnumber)
                    {
                        test = (bool)arlNumericColCurrency[columnnumber];

                    }
                    else
                    {
                        test = false;
                    }
                    if (test && (clsTxtBox.TypeEnum)arlNumericColumnType[columnnumber] == clsTxtBox.TypeEnum.Float)
                    {
                        int dpres = 4;
                        string tmp = String.Empty;

                        for (int k = 0; k < dpres; k++)
                            tmp = tmp + "0";

                        DecimalPart = tmp;
                        flag = true;


                    }
                }
                else
                {


                    DecimalPart = strTextinComma.Substring(dotIndex + 1);

                    if (DecimalPrecision < DecimalPart.Length)
                    {
                        DecimalPart = nmfi.NumberDecimalSeparator + DecimalPart;
                        decimal val = Convert.ToDecimal(DecimalPart);
                        int counter = 0;
                        int totalcount = DecimalPart.Length - DecimalPrecision;



                        val = Math.Round(val,DecimalPrecision);
                        DecimalPart = val.ToString();
                        string[] pos = DecimalPart.Split(Convert.ToChar(nmfi.NumberDecimalSeparator));
                        DecimalPart = pos[1];

                    }
                    integerPart = strTextinComma.Substring(0, dotIndex);
                    flag = true;
                }

                integerPart = integerPart.Replace(nmfi.NumberGroupSeparator, "");


                string prefix = "";
                if (integerPart.IndexOf(nmfi.NegativeSign) == 0)
                {
                    integerPart = integerPart.TrimStart(Convert.ToChar(nmfi.NegativeSign));
                    prefix = nmfi.NegativeSign;
                }
                int commasalreadyinintpart = this.find_no_occurences(strParamTextinComma, nmfi.NumberGroupSeparator);
                if (integerPart.Length >= CommaPrecision)
                {
                    int temp = integerPart.Length;
                    for (int k = CommaPrecision; k < temp; k += CommaPrecision)
                    {

                        integerPart = integerPart.Insert(temp - k, nmfi.NumberGroupSeparator);
                        noofcommas++;
                    }
                    noofcommas = noofcommas - commasalreadyinintpart;

                }

                if (flag)
                    strTextinComma = prefix + integerPart + nmfi.NumberDecimalSeparator + DecimalPart;
                else
                    strTextinComma = prefix + integerPart;
               int  position = editingcontrol.SelectionStart;
                if (noofcommas == 0)
                {
                    start= position;
                }
                else
                {
                    start = position + noofcommas;
                }
                return strTextinComma;
            }
            catch (Exception ex)
            {
                return strstoretext;
            }
        }


        private int find_no_occurences(string input, string seperator)
        {
            try
            {
                char c = Convert.ToChar(seperator);
                char[] total = input.ToCharArray();
                int count = 0;
                foreach (char m in total)
                {
                    if (c == m)
                    {
                        count++;
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }


        private bool _AllowRowStatusSettings = true;
        public bool AllowRowStatusSettings
        {
            set { _AllowRowStatusSettings = value; }
        }
       void editingcontrol_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (entrycount < 1)
                //{
                insidetextchanged = true;
                string s = editingcontrol.Text;
                if (selectionStart == -1)
                {
                    selectionStart = editingcontrol.SelectionStart;
                    selectionLength = editingcontrol.SelectionLength;
                }

                //this.CommitEdit(DataGridViewDataErrorContexts.Commit| DataGridViewDataErrorContexts.CurrentCellChange);
                //this.dvSource.Table.AcceptChanges();
                //this.Update();
                //this.Refresh();

                int col = this.CurrentCell.ColumnIndex;
                col = this.get_actual_colum_index(col);
                //int start = 0;
                int start = s.Length;
                int test = 0;
                //int m_intCheckLength = 0;

                if (CheckNumericColumn(col, ref test))
                {
                    //entrycount++;
                    //**
                    string op = this.strInCommaFormat(s, col, ref start);
                    //string[] arr = op.Split(',');
                    string[] arr = op.Split(Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator));
                    int i = arr.Length - 1;

                    //string[] arr2 = editingcontrol.Text.Split(',');
                    string[] arr2 = editingcontrol.Text.Split(Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator));
                    int j = arr2.Length - 1;
                    if (i != j)
                        noOfCommas = 1;

                     //* /**
                    //this.EditingControl.Text = op;
                    //decimal s1 = Convert.ToDecimal(s);
                    this.editingcontrol.Text = s.ToString();
                    //this.CurrentCell.Value = s;
                    //this.editingcontrol.EditingControlFormattedValue = s1;
                   // this.editingcontrol.EditingControlDataGridView.CurrentCell.Value = s1;
                    //this.CurrentCell.Value = s1;
                  // this.editingcontrol.SelectionStart = s.Length;
                }
                else
                {
                    //entrycount++;
                    this.editingcontrol.Text = s;
                    this.CurrentCell.Value = s;

                   //  this.editingcontrol.SelectionStart = start;
                }
                //}


                insidetextchanged = false;
                DataGridViewEditingControlTextChanged _textChanged = new DataGridViewEditingControlTextChanged(this.CurrentCell.RowIndex, this.CurrentCell.ColumnIndex, this.editingcontrol.Text.ToString());
                if (EditingControlTextChanged != null)
                {
                    EditingControlTextChanged(null, _textChanged);
                    
                }

                if (_AllowRowStatusSettings)
                    SetRowStatus(sender);
            }
            catch
            {

            }
           //return;

        }

        //int entrycount = 0;

        void clsSearchGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                //if (!insidetextchanged)
                //{
                    Type type = e.Control.GetType();

                    if (type.Name == "DataGridViewComboBoxEditingControl")
                    {
                        comboforall = null;
                        comboforall = (ComboBox)e.Control;
                        comboboolean = true;
                        comboforall.SelectionChangeCommitted += new EventHandler(comboforall_SelectionChangeCommitted);
                    }
                    else if (type.Name == "DataGridViewTextBoxEditingControl")
                    {
                        EnableF2Event = true;
                        editingcontrol = (DataGridViewTextBoxEditingControl)e.Control;
                        editingcontrol.TextChanged += new EventHandler(editingcontrol_TextChanged);

                        //entrycount = 0;
                        editingcontrol.KeyDown += new KeyEventHandler(editingcontrol_KeyDown);
                    }
                //}
                
            }
            catch (Exception ex)
            {

            }
        }
        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
        {
            //THIS IS ADDED TO AVOID THE BLACKENING OF THE CELLS ON ENTER...DEBANJAN
            e.CellStyle.BackColor = Color.White;
            base.OnEditingControlShowing(e);
        }

        protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
        {
            try
            {
                int ColumnIndex = e.ColumnIndex;
                if (CheckComboBoxColumn(get_actual_colum_index(e.ColumnIndex)))
                {
                    //comboforall.SelectionChangeCommitted -= new EventHandler(comboforall_SelectionChangeCommitted);
                }
                else
                {
                    editingcontrol.TextChanged -= new EventHandler(editingcontrol_TextChanged);
                    editingcontrol.KeyDown -= new KeyEventHandler(editingcontrol_KeyDown);

                }
            }
            catch
            {

            }
            base.OnCellEndEdit(e);
        }
        private bool EnableF2Event ;//= true;
        void editingcontrol_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F2)
                {
                    if (!this.ReadOnly)
                    {
                        if (EnableF2Event)
                        {
                            EnableF2Event = false;
                            // KeyEventArgs ke = new KeyEventArgs(Keys.F2);
                            F2EventArgs ke = new F2EventArgs(this.CurrentCell.RowIndex, this.CurrentCell.ColumnIndex);
                            if (F2keypressed != null)
                            {
                                F2keypressed(this, ke);
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        void comboforall_LostFocus(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        void comboforall_GotFocus(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }


        bool comboboolean = false;
        void comboforall_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            { 
                
                ComboSelectionChangeCommitedEventArgs com = new ComboSelectionChangeCommitedEventArgs();
                if (combocomitted != null)
                {
                    //if (comboboolean)
                    //{
                       
                        ComboBox con = (ComboBox)sender;
                        combocomitted(con, com);
                        comboboolean = false;
                        SetRowStatus(sender);
                    //}

                }
                
            }
            catch (Exception ex)
            {
            }
        }

        
        CheckBox checkBox = new CheckBox();
      
        public int get_unreal_colum_index(int index)
        {
            try
            {
                DataTable dt = (DataTable)this.DataSource;
                int exactcolumn = 0;
                int store = 0; ;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (get_actual_colum_index(i) == index)
                    {
                        return i;
                    }
                }
                return index;
               
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public bool check_column_status()
        {
            try
            {
                bool readonlycol = false;
                if (columnselected >= 0)
                {
                    if (CheckReadOnlyColumn(columnselected))
                    {
                        readonlycol = true;
                    }
                    else
                    {
                        if (CheckDisableColumn(columnselected))
                        {
                            readonlycol = true;
                        }
                        else
                        {
                            if (CheckDisableImmediate(columnselected))
                            {
                                readonlycol = true;
                            }
                            else
                            {
                                readonlycol = false;
                            }
                        }
                    }
                   
                }
                return readonlycol;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        void Filter_Click(object sender, EventArgs e)
        {
            try
            {
                FilterControl control = new FilterControl(this);
                control.Show();
            }
            catch (Exception ex)
            {
            }
        }

        bool _b = true;
        bool bl = true;

        private bool _EnableUserTabKeyRowAdd = true;
        public bool EnableUserTabKeyRowAdd
        {
            get { return _EnableUserTabKeyRowAdd; }
            set { _EnableUserTabKeyRowAdd = value; }
        }


        protected override bool ProcessDialogKey(Keys keyData)
        {
            // Extract the key code from the key value. 
            Keys key = (keyData & Keys.KeyCode);

            // Handle the ENTER key as if it were a RIGHT ARROW key. 
            if (key == Keys.Tab)
            {
                if (_EnableUserTabKeyRowAdd)// Check if Rows adding is enabled by 'TAB' Key.
                {
                    int countr = this.Rows.Count;
                    int countc = this.Columns.Count;
                    if (this.CurrentCell != null)// Check if any row is present
                    {
                        int currentrow = this.CurrentCell.RowIndex;
                        int currentcol = this.CurrentCell.ColumnIndex;

                        if (currentrow == countr - 1 && currentcol == countc - 1)
                        {
                            if (bl)
                            {
                                string s = this.CurrentCell.EditedFormattedValue.ToString();
                                this.CurrentCell.Value = s;
                                DataTable dt = (DataTable)this.DataSource;
                                DataRow dr = dt.NewRow();
                                dt.Rows.Add(dr);
                                dt.AcceptChanges();
                                this.DataSource = dt;
                                countr = dt.Rows.Count;
                                countc = dt.Columns.Count;
                                DataGridViewCell cell = this.Rows[countr - 1].Cells[0];
                                this.CurrentCell = cell;

                                CurrencyManager cur = (CurrencyManager)this.BindingContext[this.DataSource];

                                cur.Position = cur.Position;

                                //  this.en
                                cur.Refresh();
                                this.Refresh();
                                //this.CurrentCell = this.Rows[this.Rows.Count - 1].Cells[-1];
                            }
                            else
                            {
                                bl = true;
                            }
                            return true;
                            //return base.ProcessDialogKey(keyData);
                            //return this.ProcessDialogKey(Keys.Control);
                        }
                        else
                        {
                            return base.ProcessDialogKey(keyData);
                        }
                    }
                    else
                    {

                        //Debug.Assert(false, "COMPONENT ERROR", "No rows are currently available");
                    }
                }
            }

            //return this.ProcessDialogKey(Keys.Shift);
            return base.ProcessDialogKey(keyData);
            // return false;
            //return true;
        }

        
        public override bool PreProcessMessage(ref Message msg)
        {


            Keys keyCode = (Keys)(int)msg.WParam & Keys.KeyCode;

            
                
                if (keyCode == Keys.F2)
                {
                    if (!this.ReadOnly)
                    {
                        // KeyEventArgs ke = new KeyEventArgs(Keys.F2);
                        if (this.CurrentCell != null)
                        {
                            F2EventArgs ke = new F2EventArgs(this.CurrentCell.RowIndex, this.CurrentCell.ColumnIndex);
                            if (F2keypressed != null)
                            {
                                F2keypressed(this, ke);
                            }
                            else
                            {
                                //return false;
                            }
                        }
                    }
                }
                
               
            return base.PreProcessMessage(ref msg);


        }



        private void visibility_changed(object sender, PaintEventArgs e)
        {
            try
            {
                
                //this.EnableHeadersVisualStyles = false;

                this.AllowUserToResizeColumns = true;

                //this.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Beige;
                //this.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Beige;
                //this.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                //Font f = new Font(this.DefaultCellStyle.Font.Name.ToString(), this.DefaultCellStyle.Font.Size, FontStyle.Bold);
                //this.ColumnHeadersDefaultCellStyle.Font = f;

                //this.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                //this.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Red;
                //this.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Gainsboro;
                //this.BackgroundColor = Color.FromArgb(190, 216, 250);
                //this.RowHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Green;
                //this.RowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Honeydew;
                //this.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Green;
                //this.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Honeydew;
                this.EditMode = DataGridViewEditMode.EditOnEnter;

            }
            catch (Exception ex)
            {
            }
        }


        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            DataGridViewColumn col = e.Column;
            Type y = col.GetType();
            base.OnColumnAdded(e);
        }


        public  void SetComboBoxIndex(int RowIndex,int ColumnIndex,string  SelectedValue)
        {
            DataGridViewComboBoxCell dgComboCell = (DataGridViewComboBoxCell)this.Rows[RowIndex].Cells[ColumnIndex];
            dgComboCell.Value = SelectedValue;
        }

        public string GetComboBoxSelectedItem(int RowIndex, int ColumnIndex)
        {
            DataGridViewComboBoxCell dgComboCell = (DataGridViewComboBoxCell)this.Rows[RowIndex].Cells[ColumnIndex];
            DataGridViewComboBoxEditingControl ComboEditingControl = (DataGridViewComboBoxEditingControl)dgComboCell.DataGridView.EditingControl;

            string  tmp = ComboEditingControl.SelectedText.ToString();
            return tmp;
        }

        private Color _ReadonlyColorStart = Color.Empty;
        private Color _ReadonlyColorEnd = Color.Empty;
        private ArrayList _ReadOnlyRowList = new ArrayList();
        private ArrayList _ReadOnlyColumnList = new ArrayList();
        public Color ReadonlyColorStart
        {
            get { return _ReadonlyColorStart; }
            set { _ReadonlyColorStart = value; }
        }

        public Color ReadonlyColorEnd
        {
            get { return _ReadonlyColorEnd; }
            set { _ReadonlyColorEnd = value; }
        }

        public ArrayList ReadOnlyRowList
        {
            get { return _ReadOnlyRowList; }
            set { _ReadOnlyRowList = value; }
        }

        public ArrayList ReadOnlyColumnList
        {
            get { return _ReadOnlyColumnList; }
            set { _ReadOnlyColumnList = value; }
        }

        private Image _imgSelectedRowHeaderIcon;


        #region On Cell Painting
        /// <summary>
        /// On Cell Painting : Custom Coloring On various conditions.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
            //if (e.RowIndex == 1 && e.ColumnIndex>=0)
            //    e.Graphics.DrawString(this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), new Font(this.DefaultCellStyle.Font.FontFamily, this.DefaultCellStyle.Font.Size, FontStyle.Bold), new SolidBrush(Color.Black),new Rectangle(this.PointToScreen(this.Rows[e.RowIndex].Cells[e.ColumnIndex].ContentBounds.Location),this.Rows[e.RowIndex].Cells[e.ColumnIndex].ContentBounds.Size));

            //if (e.RowIndex == 1 && e.ColumnIndex >= 0)
            //    e.Graphics.DrawString(e.FormattedValue.ToString(), new Font(this.DefaultCellStyle.Font.FontFamily, this.DefaultCellStyle.Font.Size, FontStyle.Bold), new SolidBrush(Color.Black), e.ClipBounds);

            ///To add an icon to the row header of the selected row.
            #region Icon To Row Header
            if (e.ColumnIndex == -1)
            {
                if (e.RowIndex != -1)
                {
                    if (this.Rows[e.RowIndex].Selected)
                    {
                        #region Image For Selected Row Icon On Row Header
                        //e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                        //Rectangle rct = e.CellBounds;
                        //rct.Offset(-3, 1);
                        ////GraphicsUnit unit = GraphicsUnit.Point;
                        ////RectangleF rct = _imgSelectedRowHeaderIcon.GetBounds(ref unit);
                        //e.Graphics.DrawImage(_imgSelectedRowHeaderIcon, rct);

                        //e.Handled = true;
                        #endregion Image For Selected Row Icon On Row Header
                    }
                }
            }
            #endregion Icon To Row Header

            ///To give a special color to any column which is in read-only mode.
            #region Read-Only Column List Coloring
            if (ReadOnlyColumnList.Count > 0)
            {
                if (ReadOnlyColumnList.Contains(e.ColumnIndex))
                {
                    Rectangle CellBounds = e.CellBounds;
                    using (Brush backbrush =
                    new System.Drawing.Drawing2D.LinearGradientBrush(CellBounds,
                       ReadonlyColorStart,
                        ReadonlyColorEnd,
                        System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
                    {
                        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                        Brush stringBrush = new SolidBrush(System.Drawing.Color.Black);
                        e.Graphics.FillRectangle(backbrush, CellBounds);

                    }
                }
            }
            #endregion Read-Only Column List Coloring

            ///For First Type Grouping
            #region Type-I Columns Coloring
            if (Type1Columns.Count > 0)
            {
                if (Type1Columns.Contains(e.ColumnIndex))
                {
                    Rectangle CellBounds = e.CellBounds;
                    //using (Brush backbrush =
                    //new System.Drawing.Drawing2D.LinearGradientBrush(CellBounds,
                    //   Color.FromArgb(204, 204, 204),
                    //    Color.White,
                    //    System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
                    {
                        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                        Brush stringBrush = new SolidBrush(System.Drawing.Color.Black);
                        Brush backbrush = new SolidBrush(Color.FromArgb(227, 227, 227));
                        e.Graphics.FillRectangle(backbrush, CellBounds);

                    }
                }
            }
            #endregion Type-I Columns Coloring

            ///For Second Type Grouping
            #region Type-2 Columns Coloring
            if (Type2Columns.Count>0)
            {
                if(Type2Columns.Contains(e.ColumnIndex))
                {
                     Rectangle CellBounds = e.CellBounds;
                     //using (Brush backbrush =
                     //new System.Drawing.Drawing2D.LinearGradientBrush(CellBounds,
                     //   Color.White,
                     //    Color.FromArgb(146, 181, 202),
                     //    System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
                    {
                        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                        Brush backbrush = new SolidBrush(Color.FromArgb(200,215,230));
                        Brush stringBrush = new SolidBrush(System.Drawing.Color.Black);
                        e.Graphics.FillRectangle(backbrush, CellBounds);
                        

                    }
                }
            }
            #endregion Type-2 Columns Coloring

            ///For Coloring of a particular row based on a calculation. eg: Delta Difference in BPS
            #region Preferrential/Calculated Coloring
            if (_RowIndexForPreferrentialColoring.Count > 0 && DoEnableCalculatedRowColor)
            {
                if (_RowIndexForPreferrentialColoring.Contains(e.RowIndex))
                {
                    if (!IsTrueToColor(e.RowIndex) && e.ColumnIndex!=-1)
                    {
                        Rectangle CellBounds = e.CellBounds;
                        using (Brush backbrush =
                        new System.Drawing.Drawing2D.LinearGradientBrush(CellBounds,
                           Color.FromArgb(255, 0, 0),
                            Color.White,
                            System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                        {
                            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                            //Brush backbrush = new SolidBrush(Color.FromArgb(200, 215, 230));
                            Brush stringBrush = new SolidBrush(System.Drawing.Color.Black);
                            e.Graphics.FillRectangle(backbrush, CellBounds);

                        }
                    }
                }
            }
            #endregion Preferrential/Calculated Coloring

        }
        #endregion On Cell Painting

        #region Prefferential Row Coloring
        #region Is True To Color
        /// <summary>
        /// Calculates on the custom condition wheather to color the specified row or not.
        /// </summary>
        /// <param name="RowIndex">Row to run the calculation on.</param>
        /// <returns>True or False wheather to color or not</returns>
        private bool IsTrueToColor(int RowIndex)
        {
            try
            {
                double dbl = 0.00;
                for (int i = 0; i < _ColumnsToCheckForColoring.Count; i++)
                {
                    dbl += Convert.ToDouble(this.Rows[RowIndex].Cells[Convert.ToInt32(_ColumnsToCheckForColoring[i])].Value);
                }
                if (dbl == 0.00)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion Is True To Color

        private bool _DoEnableCalculatedRowColor = false;

        #region Do Enable Calculated Row Color
        /// <summary>
        /// Gets or sets wheather to run calculated preferential coloring or not.
        /// </summary>
        public bool DoEnableCalculatedRowColor
        {
            get { return _DoEnableCalculatedRowColor; }
            set { _DoEnableCalculatedRowColor = value; }
        }
        #endregion

        private ArrayList _RowIndexForPreferrentialColoring=new ArrayList();
        private ArrayList _ColumnsToCheckForColoring = new ArrayList();

        #region AddRowIndexForPreferrentialColoring 
        /// <summary>
        /// Sets the row and corresponding columns to set calculated coloring algo on.
        /// </summary>
        /// <param name="RowNums"></param>
        /// <param name="ColumnsToCheckForColoring"></param>
        public void AddRowIndexForPreferrentialColoring(int[] RowNums, int[] ColumnsToCheckForColoring)
        {
            if (_RowIndexForPreferrentialColoring != null)
            {
                _RowIndexForPreferrentialColoring.AddRange(RowNums);
            }
            if (_ColumnsToCheckForColoring != null)
            {
                _ColumnsToCheckForColoring.AddRange(ColumnsToCheckForColoring);
            }


        }
        #endregion AddRowIndexForPreferrentialColoring
        #endregion Prefferential Row Coloring

        //******************Header Extender*********************************
        private bool _HeaderExtenderEnabled = false;
        private WritableHeaderExtender _WritableHeaderExtender=null;
        private ArrayList _ListHeaderExtenderMap = new ArrayList();

        public bool HeaderExtenderEnabled
        {
            get { return _HeaderExtenderEnabled; }
            set { _HeaderExtenderEnabled = value; }
        }

        public WritableHeaderExtender AssociatedHeaderExtender
        {
            get { return _WritableHeaderExtender; }
            set { _WritableHeaderExtender = value; }
        }

        //[Browsable(false), DefaultValue(null)]
        //public ArrayList HeaderToColumnsMapping
        //{
        //    get { return _ListHeaderExtenderMap; }
        //    set
        //    {
        //         if (_ListHeaderExtenderMap.Count > 0)
        //            _ListHeaderExtenderMap.Clear();
        //        _ListHeaderExtenderMap = value;
        //        _WritableHeaderExtender.Header.CreateGridHeader();
        //    }
        //}

        bool HeaderMapping = false;
        public void SetHeaderToColumnsMapping(ArrayList _List,ArrayList type1,ArrayList type2,ArrayList BorderColumns,int width)
        {
            HeaderMapping = true;
            if (_ListHeaderExtenderMap.Count > 0)
                _ListHeaderExtenderMap.Clear();
            _ListHeaderExtenderMap = _List;
            //_WritableHeaderExtender.Header.CreateGridHeader();
            WritableGridHeader obj = _WritableHeaderExtender.Header;
            obj.DataGridView = this;
            obj.Type1Columns = type1;
            obj.Type2Columns = type2;
            obj.BorderedColumns = BorderColumns;
            obj.DesignGridHeaders(width);
        }

        public ArrayList GetHeaderToColumnsMapping()
        {
            return _ListHeaderExtenderMap;
        }


        //**********Rows Up-Down


        private UnitType _UnitType = UnitType.NotApplicable;

        public UnitType TypeOfUnit
        {
            get { return _UnitType; }
            set { _UnitType = value; }
        }

        private void SetFreezedRows()
        {
            if (listFreezedRows.Count > 0)
                listFreezedRows.Clear();
            if (_UnitType == UnitType.Volume || _UnitType == UnitType.TurnOver || this.SCREEN_NAME == ScreenGroup.Product)
            {
                listFreezedRows.Add(0);
                listFreezedRows.Add(1);
            }
            else if (_UnitType == UnitType.Margin && this.SCREEN_NAME!= ScreenGroup.Product)
            {
                listFreezedRows.Add(0);
                listFreezedRows.Add(1);
                listFreezedRows.Add(2);
            }
            else if (_UnitType == UnitType.Margin)
            {
                listFreezedRows.Clear();
            }
        }
        private ArrayList listFreezedRows = new ArrayList();

        #region SwapRows Old
        //private void swapRows(SwapMode range,ref int SelectedIndex)
        //{
        //    try
        //    {
        //        SetFreezedRows();
        //        //if (listFreezedRows.Count > 0)
        //        //{
        //        //    listFreezedRows.Clear();

        //        //}
        //        //listFreezedRows.Add(0);
        //        //listFreezedRows.Add(1);
        //        int iSelectedRow = -1;
        //        for (int iTmp = 0; iTmp <= this.Rows.Count - 1; iTmp++)
        //        {
        //            if (this.Rows[iTmp].Selected)
        //            {
        //                if (listFreezedRows.Contains(iTmp))
        //                    return;
        //                iSelectedRow = iTmp;
        //                SelectedIndex = iSelectedRow;
        //                break; // TODO: might not be correct. Was : Exit For 
        //            }
        //        }

        //        if (iSelectedRow != -1)
        //        {
        //            string[] sTmp = new string[this.Columns.Count];
        //            for (int iTmp = 0; iTmp <= this.Columns.Count - 1; iTmp++)
        //            {
        //                sTmp[iTmp] = this.Rows[iSelectedRow].Cells[iTmp].Value.ToString();
        //            }

        //            int iNewRow = 0;
        //            if (range == SwapMode.Down)
        //            {
        //                iNewRow = iSelectedRow + 1;
        //            }
        //            else if (range == SwapMode.Up)
        //            {
        //                iNewRow = iSelectedRow - 1;
        //                if (listFreezedRows.Contains(iNewRow))
        //                    iNewRow = -1;
        //            }

        //            if (iNewRow > -1)
        //            {
        //                if (range == SwapMode.Up | range == SwapMode.Down)
        //                {
        //                    this.dvSource.BeginInit();
        //                    for (int iTmp = 0; iTmp <= this.Columns.Count - 1; iTmp++)
        //                    {

        //                        //this.Rows[iSelectedRow].Cells[iTmp].Value = this.Rows[iNewRow].Cells[iTmp].Value;
        //                        this.dvSource[iSelectedRow][iTmp] = this.dvSource[iNewRow][iTmp];
        //                        //this.Rows[iNewRow].Cells[iTmp].Value = sTmp[iTmp];
        //                        this.dvSource[iNewRow][iTmp] = sTmp[iTmp];
        //                    }
        //                    this.dvSource.EndInit();
        //                    dvSource.Table.AcceptChanges();
        //                    this.DataSource = dvSource.Table;
        //                    this.Update();

                            

        //                    toSelect(iNewRow);
        //                }
        //                else if (range == SwapMode.Top | range == SwapMode.Bottom)
        //                {
        //                    reshuffleRows(sTmp, iSelectedRow, range);
        //                }
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
 
        //    }
        //}
        #endregion

        private void swapRows(SwapMode range, ref int SelectedIndex)
        {
            try
            {
                SetFreezedRows();
                //if (listFreezedRows.Count > 0)
                //{
                //    listFreezedRows.Clear();

                //}
                //listFreezedRows.Add(0);
                //listFreezedRows.Add(1);
                int iSelectedRow = -1;
                for (int iTmp = 0; iTmp <= this.Rows.Count - 1; iTmp++)
                {
                    if (this.Rows[iTmp].Selected)
                    {
                        if (listFreezedRows.Contains(iTmp))
                            return;
                        iSelectedRow = iTmp;
                        SelectedIndex = iSelectedRow;
                        break; // TODO: might not be correct. Was : Exit For 
                    }
                }

                if (iSelectedRow != -1)
                {
                    string[] sTmp = new string[this.dvSource.Table.Columns.Count];
                    for (int iTmp = 0; iTmp <= this.dvSource.Table.Columns.Count - 1; iTmp++)
                    {
                        //sTmp[iTmp] = this.Rows[iSelectedRow].Cells[iTmp].Value.ToString();
                        sTmp[iTmp] = this.dvSource[iSelectedRow][iTmp].ToString();
                    }

                    int iNewRow = 0;
                    if (range == SwapMode.Down)
                    {
                        iNewRow = iSelectedRow + 1;
                    }
                    else if (range == SwapMode.Up)
                    {
                        iNewRow = iSelectedRow - 1;
                        if (listFreezedRows.Contains(iNewRow))
                            iNewRow = -1;
                    }

                    if (iNewRow > -1)
                    {
                        if (range == SwapMode.Up | range == SwapMode.Down)
                        {
                            this.dvSource.BeginInit();
                            for (int iTmp = 0; iTmp <= this.dvSource.Table.Columns.Count - 1; iTmp++)
                            {

                                //this.Rows[iSelectedRow].Cells[iTmp].Value = this.Rows[iNewRow].Cells[iTmp].Value;
                                this.dvSource[iSelectedRow][iTmp] = this.dvSource[iNewRow][iTmp];
                                //this.Rows[iNewRow].Cells[iTmp].Value = sTmp[iTmp];
                                this.dvSource[iNewRow][iTmp] = sTmp[iTmp];
                            }
                            this.dvSource.EndInit();
                            dvSource.Table.AcceptChanges();
                            this.DataSource = dvSource.Table;
                            this.Update();



                            toSelect(iNewRow);
                        }
                        else if (range == SwapMode.Top | range == SwapMode.Bottom)
                        {
                            reshuffleRows(sTmp, iSelectedRow, range);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #region swapRows Overload 2 Old
        //private void swapRows(clsWritableGrid _Grid, int iSelectedRow, SwapMode range)
        //{
        //    try
        //    {
        //        SetFreezedRows();
        //        //int iSelectedRow = -1;
        //        //for (int iTmp = 0; iTmp <= _ParentGrid.Rows.Count - 1; iTmp++)
        //        //{
        //        //    if (_ParentGrid.Rows[iTmp].Selected)
        //        //    {
        //        //        if (listFreezedRows.Contains(iTmp))
        //        //            return;
        //        //        iSelectedRow = iTmp;
        //        //        if (range == SwapMode.Up)
        //        //            iSelectedRow--;
        //        //        else
        //        //            iSelectedRow++;
        //        //        break; // TODO: might not be correct. Was : Exit For 
        //        //    }
        //        //}

        //        if (iSelectedRow != -1)
        //        {
        //            string[] sTmp = new string[_Grid.Columns.Count];
        //            for (int iTmp = 0; iTmp <= _Grid.Columns.Count - 1; iTmp++)
        //            {
        //                sTmp[iTmp] = _Grid.Rows[iSelectedRow].Cells[iTmp].Value.ToString();
        //            }

        //            int iNewRow = 0;
        //            if (range == SwapMode.Down)
        //            {
        //                iNewRow = iSelectedRow + 1;
        //            }
        //            else if (range == SwapMode.Up)
        //            {
        //                iNewRow = iSelectedRow - 1;
        //                if (listFreezedRows.Contains(iNewRow))
        //                    iNewRow = -1;
        //            }

        //            if (iNewRow > -1)
        //            {
        //                if (range == SwapMode.Up | range == SwapMode.Down)
        //                {
        //                    _Grid.dvSource.BeginInit();
        //                    for (int iTmp = 0; iTmp <= _Grid.Columns.Count - 1; iTmp++)
        //                    {

        //                        //this.Rows[iSelectedRow].Cells[iTmp].Value = this.Rows[iNewRow].Cells[iTmp].Value;
        //                        _Grid.dvSource[iSelectedRow][iTmp] = _Grid.dvSource[iNewRow][iTmp];
        //                        //this.Rows[iNewRow].Cells[iTmp].Value = sTmp[iTmp];
        //                        _Grid.dvSource[iNewRow][iTmp] = sTmp[iTmp];
        //                    }
        //                    _Grid.dvSource.EndInit();
        //                    _Grid.dvSource.Table.AcceptChanges();
        //                    _Grid.DataSource = _Grid.dvSource.Table;
        //                    _Grid.Update();

        //                    for (int i = 0; i < _Grid.Rows.Count - 1; i++)
        //                    {
        //                        _Grid.Rows[i].Selected = false;
        //                    }
        //                    _Grid.Rows[iNewRow].Selected = true;
        //                }
        //                else if (range == SwapMode.Top | range == SwapMode.Bottom)
        //                {
        //                    reshuffleRows(sTmp, iSelectedRow, range);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        #endregion

        private void swapRows(clsWritableGrid _Grid, int iSelectedRow, SwapMode range)
        {
            try
            {
                SetFreezedRows();
                //int iSelectedRow = -1;
                //for (int iTmp = 0; iTmp <= _ParentGrid.Rows.Count - 1; iTmp++)
                //{
                //    if (_ParentGrid.Rows[iTmp].Selected)
                //    {
                //        if (listFreezedRows.Contains(iTmp))
                //            return;
                //        iSelectedRow = iTmp;
                //        if (range == SwapMode.Up)
                //            iSelectedRow--;
                //        else
                //            iSelectedRow++;
                //        break; // TODO: might not be correct. Was : Exit For 
                //    }
                //}

                if (iSelectedRow != -1)
                {
                    string[] sTmp = new string[_Grid.dvSource.Table.Columns.Count];
                    for (int iTmp = 0; iTmp <= _Grid.dvSource.Table.Columns.Count - 1; iTmp++)
                    {
                        //sTmp[iTmp] = _Grid.Rows[iSelectedRow].Cells[iTmp].Value.ToString();
                        sTmp[iTmp] = _Grid.dvSource[iSelectedRow][iTmp].ToString();
                    }

                    int iNewRow = 0;
                    if (range == SwapMode.Down)
                    {
                        iNewRow = iSelectedRow + 1;
                    }
                    else if (range == SwapMode.Up)
                    {
                        iNewRow = iSelectedRow - 1;
                        if (listFreezedRows.Contains(iNewRow))
                            iNewRow = -1;
                    }

                    if (iNewRow > -1)
                    {
                        if (range == SwapMode.Up | range == SwapMode.Down)
                        {
                            _Grid.dvSource.BeginInit();
                            for (int iTmp = 0; iTmp <= _Grid.dvSource.Table.Columns.Count - 1; iTmp++)
                            {

                                //this.Rows[iSelectedRow].Cells[iTmp].Value = this.Rows[iNewRow].Cells[iTmp].Value;
                                _Grid.dvSource[iSelectedRow][iTmp] = _Grid.dvSource[iNewRow][iTmp];
                                //this.Rows[iNewRow].Cells[iTmp].Value = sTmp[iTmp];
                                _Grid.dvSource[iNewRow][iTmp] = sTmp[iTmp];
                            }
                            _Grid.dvSource.EndInit();
                            _Grid.dvSource.Table.AcceptChanges();
                            _Grid.DataSource = _Grid.dvSource.Table;
                            _Grid.Update();

                            for (int i = 0; i < _Grid.Rows.Count - 1; i++)
                            {
                                _Grid.Rows[i].Selected = false;
                            }
                            _Grid.Rows[iNewRow].Selected = true;
                        }
                        else if (range == SwapMode.Top | range == SwapMode.Bottom)
                        {
                            reshuffleRows(sTmp, iSelectedRow, range);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SwapRowsForAssociatedGrids(SwapMode mode,int iSelected)
        {
            for (int i = 0;i< AssociatedGrids.Length; i++)
            {
                swapRows(AssociatedGrids[i], iSelected, mode);
            }
        }

        private void toSelect(int iNewRow)
        {
            for (int i = 0; i < this.Rows.Count - 1; i++)
            {
                this.Rows[i].Selected = false;
            }
            this.Rows[iNewRow].Selected = true;
        }

        private void reshuffleRows(string[] sTmp, int iSelectedRow, SwapMode Range)
        {
            if (Range == SwapMode.Top)
            {
                //int iFirstRow = 0;
                int iFirstRow = 2;
                if (iSelectedRow > iFirstRow)
                {
                    for (int iTmp = iSelectedRow; iTmp >= 1; iTmp += -1)
                    {
                        for (int iCol = 0; iCol <= this.Columns.Count - 1; iCol++)
                        {
                            if (listFreezedRows.Contains(iTmp))
                                continue;
                            this.Rows[iTmp].Cells[iCol].Value = this.Rows[iTmp - 1].Cells[iCol].Value;
                        }
                    }
                    for (int iCol = 0; iCol <= this.Columns.Count - 1; iCol++)
                    {
                        this.Rows[iFirstRow].Cells[iCol].Value = sTmp[iCol].ToString();
                    }
                    toSelect(iFirstRow);
                }
            }
            else
            {
                int iLastRow = this.Rows.Count - 1;
                if (iSelectedRow < iLastRow)
                {
                    for (int iTmp = iSelectedRow; iTmp <= iLastRow - 1; iTmp++)
                    {
                        for (int iCol = 0; iCol <= this.Columns.Count - 1; iCol++)
                        {
                            
                            this.Rows[iTmp].Cells[iCol].Value = this.Rows[iTmp + 1].Cells[iCol].Value;
                        }
                    }
                    for (int iCol = 0; iCol <= this.Columns.Count - 1; iCol++)
                    {
                        this.Rows[iLastRow].Cells[iCol].Value = sTmp[iCol].ToString();
                    }
                    toSelect(iLastRow);
                }
            }
        }

        public void MoveUp()
        {
            int iSelected = -1;
            swapRows(SwapMode.Up, ref iSelected);
            if (iSelected != -1)
            {
                if (AssociatedGrids != null)
                {
                    if (AssociatedGrids.Length > 0)
                        SwapRowsForAssociatedGrids(SwapMode.Up,iSelected);
                }
            }
        }

        public void MoveDown()
        {
            int iSelected = -1;
            swapRows(SwapMode.Down, ref iSelected);
            if (iSelected != -1)
            {
                if (AssociatedGrids != null)
                {
                    if (AssociatedGrids.Length > 0)
                        SwapRowsForAssociatedGrids(SwapMode.Down, iSelected);
                }
            }
        }
        public void MoveTop()
        {
            int iSelected = -1;
            swapRows(SwapMode.Top, ref iSelected);
        }
        public void MoveBottom()
        {
            int iSelected = -1;
            swapRows(SwapMode.Bottom, ref iSelected);
        }

        //*********
        private ArrayList _Type1Columns = new ArrayList();
        private ArrayList _Type2Columns = new ArrayList();

        public ArrayList Type1Columns
        {
            get { return _Type1Columns; }
            set 
            { 
                _Type1Columns = value;

                if (this.Columns.Count > 0)
                {
                    for (int i = 0; i < _Type1Columns.Count; i++)
                    {
                        int colnum = (int)Type1Columns[i];
                        this.Columns[colnum].ReadOnly = true;
                    }
                    this.Refresh();
                }
            }
        }

        public ArrayList Type2Columns
        {
            get { return _Type2Columns; }
            set 
            {
                _Type2Columns = value;
                if (this.Columns.Count > 0)
                {
                    for (int i = 0; i < _Type2Columns.Count; i++)
                    {
                        int colnum = (int)_Type2Columns[i];
                        this.Columns[colnum].ReadOnly = true;
                    }
                    this.Refresh();
                }
            }
        }

        public void AddType1Columns(int ColNum)
        {
            
            if (_Type1Columns != null)
            {
                _Type1Columns.Add((object)ColNum);
                try
                {
                    this.Columns[ColNum].ReadOnly = true;
                }
                catch
                {
 
                }
            }
        }

        public void AddType2Columns(int ColNum)
        {

            if (_Type2Columns != null)
            {
                _Type2Columns.Add((object)ColNum);
                try
                {
                    this.Columns[ColNum].ReadOnly = true;
                }
                catch
                {

                }
            }
        }

        public void AddType1Columns(int[] ColNum)
        {
            if (_Type1Columns != null)
            {
                _Type1Columns.AddRange(ColNum);
                if (this.Columns.Count > 0)
                {
                    for (int i = 0; i < ColNum.Length; i++)
                    {
                        this.Columns[ColNum[i]].ReadOnly = true;
                    }
                }
            }
        }

        public void AddType2Columns(int[] ColNum)
        {
            if (_Type2Columns != null)
            {
                _Type2Columns.AddRange(ColNum);
                if (this.Columns.Count > 0)
                {
                    for (int i = 0; i < ColNum.Length; i++)
                    {
                        this.Columns[ColNum[i]].ReadOnly = true;
                    }
                }
            }
        }

        public void ClearType1Columns()
        {
            if (this.Columns.Count > 0)
            {
                for (int i = 0; i < _Type1Columns.Count; i++)
                {
                    this.Columns[Convert.ToInt32(_Type1Columns[i])].ReadOnly = true;
                }
            }
            _Type1Columns.Clear();
        }

        public void ClearType2Columns()
        {
            if (this.Columns.Count > 0)
            {
                for (int i = 0; i < _Type2Columns.Count; i++)
                {
                    this.Columns[Convert.ToInt32(_Type2Columns[i])].ReadOnly = true;
                }
            }
            _Type2Columns.Clear();
        }
        //*********

        private clsWritableGrid[] _AssociatedGrids;

        public clsWritableGrid[] AssociatedGrids
        {
            get { return _AssociatedGrids; }
            set 
            {
                if (value != null)
                {
                    _AssociatedGrids = value;
                    for (int i = 0; i < _AssociatedGrids.Length; i++)
                    {
                        _AssociatedGrids[i].TypeOfUnit = this.TypeOfUnit;
                    }
                }
            }
        }

        private bool _AllowPreferrentialReorderingOnLoad = false;
        public bool AllowPreferrentialReorderingOnLoad
        {
            get { return _AllowPreferrentialReorderingOnLoad; }
            set { _AllowPreferrentialReorderingOnLoad = value; }
        }
        private string FILE_NAME = Application.StartupPath + "//RowOrderingInfo.xml";
        private string ENCRYPTED_FILE_NAME = Application.StartupPath + "//RowOrderingInfo.bin";
        private string USER_NAME = "";
        private ScreenGroup SCREEN_NAME = ScreenGroup.None;
        private PlanType PLAN_TYPE = PlanType.None;
        private string YEAR = "";
        private string DIST_ID = "";
        private string DIVN_ID = "";
        private string MULTI_EUS_CODE = "";

        void ProgressCallBackDecrypt(int min, int max, int value)
        {

        }

        private void DoCheckFile()
        {
            if (!File.Exists(FILE_NAME))
            {
                if (!File.Exists(ENCRYPTED_FILE_NAME))
                {
                    return;
                }
                else
                {
                    CryptoProgressCallBack cb = new CryptoProgressCallBack(this.ProgressCallBackDecrypt);
                    FileCrypt.DecryptFile(ENCRYPTED_FILE_NAME, FILE_NAME, "", cb);
                    if (File.Exists(ENCRYPTED_FILE_NAME))
                        File.Delete(ENCRYPTED_FILE_NAME);
                    return;
                    //TODO Decrypt Encrypted File back to xml format

                }
            }

        }


        public bool EncryptOrderingInfoToDisk()
        {
            bool chkVal = true;
            bool RetVal = false;
            while (chkVal)
            {
                try
                {
                    if (!File.Exists(FILE_NAME))
                    {
                        if (!File.Exists(ENCRYPTED_FILE_NAME))
                        {
                            MessageBox.Show("Error in saving personal info.");
                            return false;
                        }
                        else
                        {
                            DoCheckFile();
                            //return false;
                            //TODO Decrypt Encrypted File back to xml format

                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < dvSource.Count; i++)
                    {
                        if (dvSource[i][0].ToString().ToUpper().Trim() == "TOTAL" || dvSource[i][0].ToString().ToUpper().Trim() == "OTHERS" || dvSource[i][0].ToString().ToUpper().Trim() == "DIFFERENCE")
                            continue;
                        if (i == dvSource.Count - 1)
                            sb.Append(dvSource[i][0].ToString());
                        else
                            sb.Append(dvSource[i][0].ToString() + "|");
                    }

                    if (sb.ToString().Contains("&"))
                    {
                        sb=sb.Replace("&", "&amp;");
                    }
                    //System.IO.StreamWriter sw = new System.IO.StreamWriter(FILE_NAME);
                    //sw.Write(sb.ToString());
                    //sw.Flush();
                    //sw.Close();
                    FileStream _stream = new FileStream(FILE_NAME, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                    XmlTextReader reader = new XmlTextReader(_stream);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);
                    reader.Close();
                    _stream.Close();
                    XmlNode _node;
                    XmlElement root = doc.DocumentElement;
                    DoCheckFileConfig();

                    if (EUS_EXIST_IN_DATA_SECTION & SCREEN_EXIST_IN_DATA_SECTION & RECORD_SPEC_EXIST_IN_DATA_SECTION)
                    {
                        XmlDocumentFragment docFrag = doc.CreateDocumentFragment();
                        docFrag = doc.CreateDocumentFragment();
                        //docFrag.InnerXml = "<PlanType Name=\"" + PLAN_TYPE.ToString() + "\">" +
                        //       "<Order>" + sb.ToString() + "</Order>" +
                        //       "</PlanType>";
                        docFrag.InnerXml = "<RecordSpec DistId=\"" + DIST_ID.ToString() + " \" DivnId=\"" + DIVN_ID.ToString() + " \" Year=\"" + YEAR.ToString() + " \"  PlanType=\"" + PLAN_TYPE.ToString() + "\">" +
                               "<Order>" + sb.ToString() + "</Order>" +
                               "</RecordSpec>";
                        XPathNavigator navToReplace = docFrag.CreateNavigator();

                        XPathNavigator nav2 = doc.CreateNavigator();
                        XPathExpression expr2 = nav2.Compile("/TVBI/Data");
                        XPathNodeIterator iterator2 = nav2.Select(expr);
                        while (iterator2.MoveNext())
                        {
                            XPathNavigator nav5 = iterator2.Current.Clone();
                            string sTemp2 = nav5.GetAttribute("EUS_CODE", "");
                            if (sTemp2 == MULTI_EUS_CODE)
                            {
                                XPathNodeIterator iterator4 = nav5.SelectChildren("Screen", "");
                                while (iterator4.MoveNext())
                                {
                                    XPathNavigator nav3 = iterator4.Current.Clone();
                                    string sTemp = nav3.GetAttribute("Name", "");
                                    if (sTemp == SCREEN_NAME.ToString())
                                    {
                                        XPathNodeIterator iterator3 = nav3.SelectChildren("RecordSpec", "");
                                        while (iterator3.MoveNext())
                                        {
                                            XPathNavigator nav4 = iterator3.Current.Clone();
                                            string sTempDistId = nav4.GetAttribute("DistId", "");
                                            string sTempDivnId = nav4.GetAttribute("DivnId", "");
                                            string sTempYear = nav4.GetAttribute("Year", "");
                                            string sTempPlanType = nav4.GetAttribute("PlanType", "");

                                            if ((sTempDistId.Trim().ToUpper() == DIST_ID.ToString().Trim().ToUpper()) && (sTempDivnId.Trim().ToUpper() == DIVN_ID.ToString().Trim().ToUpper()) && (sTempYear.Trim().ToUpper() == YEAR.ToString().Trim().ToUpper()) && (sTempPlanType.Trim().ToUpper() == PLAN_TYPE.ToString().Trim().ToUpper()))
                                            {
                                                nav4.ReplaceSelf(navToReplace);
                                                DoCheckFile();
                                                doc.Save(FILE_NAME);
                                            }
                                        }

                                    }
                                }
                            }


                        }
                    }

                    if (!EUS_EXIST_IN_DATA_SECTION)
                    {
                        XmlDocumentFragment docFrag = doc.CreateDocumentFragment();
                        docFrag = doc.CreateDocumentFragment();
                        docFrag.InnerXml = "<Data EUS_CODE=\"" + MULTI_EUS_CODE + "\">" +
                                 "<Screen Name=\"" + SCREEN_NAME.ToString() + "\">" +
                                "<RecordSpec DistId=\"" + DIST_ID.ToString() + " \" DivnId=\"" + DIVN_ID.ToString() + " \" Year=\"" + YEAR.ToString() + " \"  PlanType=\"" + PLAN_TYPE.ToString() + "\">" +
                               "<Order>" + sb.ToString() + "</Order>" +
                               "</RecordSpec>" +
                               "</Screen>" +
                               "</Data>";
                        //XmlNode _DataNode = root.SelectSingleNode("/TVBI/Data");
                        XmlNode _DataNode = root.SelectSingleNode("/TVBI");
                        _DataNode.InsertAfter(docFrag, _DataNode.LastChild);
                        DoCheckFile();
                        doc.Save(FILE_NAME);
                        SCREEN_EXIST_IN_DATA_SECTION = true;
                        RECORD_SPEC_EXIST_IN_DATA_SECTION = true;
                    }
                    if (!SCREEN_EXIST_IN_DATA_SECTION)
                    {
                        XmlDocumentFragment docFrag = doc.CreateDocumentFragment();
                        docFrag = doc.CreateDocumentFragment();
                        docFrag.InnerXml = "<Screen Name=\"" + SCREEN_NAME.ToString() + "\">" +
                                "<RecordSpec DistId=\"" + DIST_ID.ToString() + " \" DivnId=\"" + DIVN_ID.ToString() + " \" Year=\"" + YEAR.ToString() + " \"  PlanType=\"" + PLAN_TYPE.ToString() + "\">" +
                               "<Order>" + sb.ToString() + "</Order>" +
                               "</RecordSpec>" +
                               "</Screen>";
                        string sToWrite = "<Screen Name=\"" + SCREEN_NAME.ToString() + "\">" +
                        "<RecordSpec DistId=\"" + DIST_ID.ToString() + " \" DivnId=\"" + DIVN_ID.ToString() + " \" Year=\"" + YEAR.ToString() + " \"  PlanType=\"" + PLAN_TYPE.ToString() + "\">" +
                       "<Order>" + sb.ToString() + "</Order>" +
                       "</RecordSpec>" +
                       "</Screen>";
                        XPathNavigator navToInsert = docFrag.CreateNavigator();

                        //XmlNode _DataNode = root.SelectSingleNode("/TVBI/Data");
                        //_DataNode.InsertAfter(docFrag, _DataNode.LastChild);
                        //doc.Save(FILE_NAME);


                        XPathNavigator nav2 = doc.CreateNavigator();
                        XPathExpression expr2 = nav2.Compile("/TVBI/Data");
                        XPathNodeIterator iterator2 = nav2.Select(expr);
                        while (iterator2.MoveNext())
                        {
                            XPathNavigator nav3 = iterator2.Current.Clone();
                            string sTemp = nav3.GetAttribute("EUS_CODE", "");

                            if (sTemp == MULTI_EUS_CODE.ToString())
                            {
                                XmlNode nde = doc.SelectSingleNode("/TVBI/Data[@EUS_CODE='" + MULTI_EUS_CODE + "']");
                                nde.InsertAfter(docFrag, nde.LastChild);
                                DoCheckFile();
                                doc.Save(FILE_NAME);
                                //if (nav3.HasChildren)
                                //{
                                //    nav3.MoveToFirstChild();
                                //    nav3.InsertAfter(sToWrite);
                                //    doc.Save(FILE_NAME);
                                //}
                                //else
                                //{
                                //    nav3.MoveToFirstChild();
                                //    nav3.PrependChildElement(navToInsert.Prefix.ToString(), "Screen",navToInsert.LookupNamespace(navToInsert.Prefix), navToInsert.Value.ToString());
                                //    //nav3.MoveToFirstAttribute();
                                //    //nav3.InsertAfter(sToWrite);
                                //    doc.Save(FILE_NAME);
                                //}
                            }
                        }

                        //XPathNavigator nav2=doc.CreateNavigator();
                        //XPathExpression expr2 = nav2.Compile("/TVBI/Data");
                        //XPathNodeIterator iterator2 = nav2.Select(expr);
                        //while (iterator2.MoveNext())
                        //{
                        //    XPathNavigator nav3 = iterator2.Current.Clone();
                        //    string sTemp = nav3.GetAttribute("ID", "");
                        //    if (sTemp == USER_NAME)
                        //    {
                        //        nav3.MoveToFirstChild();
                        //        nav3.InsertAfter(sToWrite);
                        //        doc.Save(FILE_NAME);
                        //    }
                        //}
                        RECORD_SPEC_EXIST_IN_DATA_SECTION = true;
                    }
                    if (!RECORD_SPEC_EXIST_IN_DATA_SECTION)
                    {
                        string sToWrite = "<RecordSpec DistId=\"" + DIST_ID.ToString() + " \" DivnId=\"" + DIVN_ID.ToString() + " \" Year=\"" + YEAR.ToString() + " \"  PlanType=\"" + PLAN_TYPE.ToString() + "\">" +
                               "<Order>" + sb.ToString() + "</Order>" +
                               "</RecordSpec>";
                        XPathNavigator nav2 = doc.CreateNavigator();
                        XPathExpression expr2 = nav2.Compile("/TVBI/Data");
                        XPathNodeIterator iterator2 = nav2.Select(expr);
                        while (iterator2.MoveNext())
                        {
                            XPathNavigator nav4 = iterator2.Current.Clone();
                            string sTemp2 = nav4.GetAttribute("EUS_CODE", "");
                            if (sTemp2 == MULTI_EUS_CODE.ToString())
                            {
                                XPathNodeIterator iterator3 = nav4.SelectChildren("Screen", "");
                                while (iterator3.MoveNext())
                                {
                                    XPathNavigator nav3 = iterator3.Current.Clone();
                                    string sTemp = nav3.GetAttribute("Name", "");
                                    if (sTemp == SCREEN_NAME.ToString())
                                    {
                                        nav3.MoveToFirstChild();
                                        nav3.InsertAfter(sToWrite);
                                        DoCheckFile();
                                        doc.Save(FILE_NAME);
                                    }
                                }
                            }



                        }
                    }
                    chkVal = false;
                    RetVal = true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString().StartsWith("The process cannot access the file") && ex.Message.ToString().IndexOf("because it is being used by another process")>0)
                    {
                        chkVal = true;
                    }
                    else
                    {
                        chkVal = false;
                        MessageBox.Show(ex.Message.ToString());
                        RetVal = false;
                    }
                }
            }
            return  RetVal  ;
        }

        public bool DoPreferrentialReordering( ScreenGroup _Group,PlanType _PlanType,string _Year,string _DistId,string _DivnId,string _MultiEUSCode)
        {
            try
            {
                //this.USER_NAME = USER_NAME;
                this.SCREEN_NAME = _Group;
                this.PLAN_TYPE = _PlanType;
                this.YEAR = _Year;
                this.DIST_ID = _DistId;
                this.DIVN_ID = _DivnId;
                this.MULTI_EUS_CODE = _MultiEUSCode;
                bool File_Not_Found = false;

                if (!File.Exists(FILE_NAME))
                {
                    if (!File.Exists(ENCRYPTED_FILE_NAME))
                    {
                        DoCreateRowOrderingFile();
                        File_Not_Found = true;
                    }
                    else
                    {
                        CryptoProgressCallBack cb = new CryptoProgressCallBack(this.ProgressCallBackDecrypt);
                        FileCrypt.DecryptFile(ENCRYPTED_FILE_NAME, FILE_NAME, "", cb);
                        if (File.Exists(ENCRYPTED_FILE_NAME))
                            File.Delete(ENCRYPTED_FILE_NAME);
                        File_Not_Found = false;
                    }
                    //DoAddUserToConfigurationFile();
                }

                if (!File_Not_Found)
                {
                    DoCheckFileConfig();
                    if (EUS_EXIST_IN_DATA_SECTION && SCREEN_EXIST_IN_DATA_SECTION && RECORD_SPEC_EXIST_IN_DATA_SECTION)
                    {
                        if (!DoReordering())
                        {
                            //TODO throw exception
                        }
                        //ReorderAssociatedGrids();
                    }
                }
                return false;
            }
            catch
            {
                return true;
            }
        }

        public bool DoReordering(clsWritableGrid _MasterGrid)
        {
            try
            {
                if (this.Rows.Count <= 0)
                    return false;
                //ArrayList _ReorderList = _MasterGrid.ReorderList;
                //UnitType _TypeOfUnit = _MasterGrid.TypeOfUnit;

                ReorderList = _MasterGrid.ReorderList;
                TypeOfUnit = _MasterGrid.TypeOfUnit;

                for (int i = 0; i < this.ReorderList.Count; i++)
                {
                    string str1 = this.ReorderList[i].ToString();
                    int RowNum = -1;
                    if (!DoRowExist(str1, ref RowNum))
                        continue;
                    if (RowNum == -1)
                        continue;
                    if (this.TypeOfUnit == UnitType.Margin && this.SCREEN_NAME!= ScreenGroup.Product && RowNum <= (i + 3))
                        continue;
                    if (RowNum <= (i + 2))
                        continue;
                    int SwapRow1 = RowNum;
                    //int SwapRow2 = RowNum - 1;
                    int SwapRow2 = 0;
                    if (this.TypeOfUnit == UnitType.Margin && this.SCREEN_NAME != ScreenGroup.Product)
                        SwapRow2 = i + 3;
                    else
                        SwapRow2 = i + 2;
                    #region Commented
                    //object[] objArr = new object[this.Columns.Count];
                    //dvSource.BeginInit();
                    //for (int j = 0; j < this.Columns.Count; j++)
                    //{
                    //    objArr[j] = (object)dvSource[SwapRow1][j];
                    //}

                    //for (int j = 0; j < this.Columns.Count; j++)
                    //{
                    //    this.dvSource[SwapRow1][j] = this.dvSource[SwapRow2][j];
                    //}

                    //for (int j = 0; j < this.Columns.Count; j++)
                    //{
                    //    this.dvSource[SwapRow2][j] = objArr[j];
                    //}
                    //dvSource.EndInit();
                    //dvSource.Table.AcceptChanges();
                    //this.DataSource = dvSource.Table;
                    //this.Update();
                    #endregion

                    object[] objArr = new object[this.dvSource.Table.Columns.Count];
                    dvSource.BeginInit();
                    for (int j = 0; j < this.dvSource.Table.Columns.Count; j++)
                    {
                        objArr[j] = (object)dvSource[SwapRow1][j];
                    }

                    for (int j = 0; j < this.dvSource.Table.Columns.Count; j++)
                    {
                        this.dvSource[SwapRow1][j] = this.dvSource[SwapRow2][j];
                    }

                    for (int j = 0; j < this.dvSource.Table.Columns.Count; j++)
                    {
                        this.dvSource[SwapRow2][j] = objArr[j];
                    }
                    dvSource.EndInit();
                    dvSource.Table.AcceptChanges();
                    this.DataSource = dvSource.Table;
                    this.Update();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public  bool DoReordering()
        {
            try
            {
                if (this.Rows.Count <= 0)
                    return false;
                    for (int i = 0; i < ReorderList.Count; i++)
                    {
                        string str1 = ReorderList[i].ToString();
                        int RowNum = -1;
                        if (!DoRowExist(str1,ref RowNum))
                            continue;
                        if (RowNum == -1)
                            continue;
                        if (this.TypeOfUnit == UnitType.Margin && this.SCREEN_NAME != ScreenGroup.Product && RowNum <= (i + 3))
                            continue;
                        if (RowNum <= (i+2))
                            continue;
                        int SwapRow1 = RowNum;
                        //int SwapRow2 = RowNum - 1;
                        int SwapRow2 = 0;
                        if (this.TypeOfUnit == UnitType.Margin && this.SCREEN_NAME != ScreenGroup.Product)
                            SwapRow2 = i + 3;
                        else
                            SwapRow2 = i + 2;
                        #region Commented
                        //object[] objArr = new object[this.Columns.Count];
                        //dvSource.BeginInit();
                        //for (int j = 0; j < this.Columns.Count; j++)
                        //{
                        //    objArr[j] =(object)dvSource[SwapRow1][j];
                        //}

                        //for (int j = 0; j < this.Columns.Count; j++)
                        //{
                        //    this.dvSource[SwapRow1][j] = this.dvSource[SwapRow2][j];
                        //}

                        //for (int j = 0; j < this.Columns.Count; j++)
                        //{
                        //    this.dvSource[SwapRow2][j] = objArr[j];
                        //}
                        //dvSource.EndInit();
                        //dvSource.Table.AcceptChanges();
                        //this.DataSource = dvSource.Table;
                        //this.Update();
                        #endregion Commented

                        object[] objArr = new object[this.dvSource.Table.Columns.Count];
                        dvSource.BeginInit();
                        for (int j = 0; j < this.dvSource.Table.Columns.Count; j++)
                        {
                            objArr[j] = (object)dvSource[SwapRow1][j];
                        }

                        for (int j = 0; j < this.dvSource.Table.Columns.Count; j++)
                        {
                            this.dvSource[SwapRow1][j] = this.dvSource[SwapRow2][j];
                        }

                        for (int j = 0; j < this.dvSource.Table.Columns.Count; j++)
                        {
                            this.dvSource[SwapRow2][j] = objArr[j];
                        }
                        dvSource.EndInit();
                        dvSource.Table.AcceptChanges();
                        this.DataSource = dvSource.Table;
                        this.Update();
                    }
                    
                return true;
            }
            catch
            {
                return false;
            }
        }


        private bool ReorderAssociatedGrids()
        {
            try
            {
                if (AssociatedGrids == null)
                    return true;

                if (AssociatedGrids.Length > 0)
                {
                    for (int i = 0; i < AssociatedGrids.Length; i++)
                    {
                        using (clsWritableGrid _GridToReorder = AssociatedGrids[i])
                        {
                            _GridToReorder.RowListForReordering = this.ReorderList;
                            _GridToReorder.DoReordering();
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool DoRowExist(object value,ref int RowNum)
        {
            bool RetVal = false;
            for (int i = 0; i < dvSource.Count; i++)
            {
                if (value.ToString() == dvSource[i][0].ToString())
                {
                    RowNum = i;
                    RetVal = true;
                    break;
                }
            }
            return RetVal;
        }


        private void DoCreateRowOrderingFile()
        {
            try
            {
            XmlWriter _Writer;
            XmlWriterSettings _Settings = new XmlWriterSettings();
            _Settings.Indent = true;
            _Settings.OmitXmlDeclaration = false;
            _Settings.IndentChars = "\t";

            _Writer = XmlWriter.Create(FILE_NAME, _Settings);
            _Writer.WriteStartElement("TVBI");//1
            //_Writer.WriteStartElement(System.Reflection.Assembly.GetEntryAssembly().GetName().Name.ToString());//1
            _Writer.WriteAttributeString("Version", null, System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());//2

            //_Writer.WriteStartElement("Schema", "");//3
            //_Writer.WriteEndElement();

            _Writer.WriteStartElement("Data", "");
            _Writer.WriteAttributeString("EUS_CODE", MULTI_EUS_CODE);
            _Writer.WriteEndElement();


            _Writer.WriteEndElement();
            _Writer.WriteEndDocument();

            _Writer.Flush();
            _Writer.Close();
            }
            catch(Exception ex)
            {

            }

        }

        private bool DoUserExistInConfigFile()
        {
            XmlTextReader reader = new XmlTextReader(FILE_NAME);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            reader.Close();
            XmlNode _node;
            XmlElement root = doc.DocumentElement;
            _node = root.SelectSingleNode("/TVBI/Schema[USERID='" + USER_NAME + "']");
            if (_node == null)
                return false;
            else
                return true;
        }


        private bool USER_EXIST_IN_DATA_SECTION = false;
        private bool SCREEN_EXIST_IN_DATA_SECTION = false;
        private bool EUS_EXIST_IN_DATA_SECTION = false;
        //private bool GRID_EXIST_IN_DATA_SECTION = false;
        private bool RECORD_SPEC_EXIST_IN_DATA_SECTION = false;

        //private bool DoCheckFileConfig()
        //{
        //    bool RetVal=false;
        //    try
        //    {
        //        XmlTextReader reader = new XmlTextReader(FILE_NAME);
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(reader);
        //        reader.Close();
        //        XmlNode _node;
        //        XmlElement root = doc.DocumentElement;
        //        _node = root.SelectSingleNode("/TVBI/Data/User");

        //        XPathDocument XPathdoc = new XPathDocument(FILE_NAME);
        //        nav = XPathdoc.CreateNavigator();

        //        expr = nav.Compile("/TVBI/Data/User");
        //        iterator = nav.Select(expr);
        //        while (iterator.MoveNext())
        //        {
        //            XPathNavigator nav2 = iterator.Current.Clone();
        //            string sTemp = nav2.GetAttribute("ID", "");
        //            if (sTemp == USER_NAME)
        //            {
        //                USER_EXIST_IN_DATA_SECTION = true;
        //                //nav2.SelectDescendants(XPathNodeType.Attribute, true);
        //                //XPathExpression expr2 = nav2.Compile("/Screen");
        //                XPathNodeIterator iterator2 = nav2.SelectChildren("Screen","");
        //                while (iterator2.MoveNext())
        //                {
        //                    XPathNavigator nav3 = iterator2.Current.Clone();
        //                    string sTemp2 = nav3.GetAttribute("Name", "");
        //                    if (sTemp2 == SCREEN_NAME)
        //                    {
        //                        SCREEN_EXIST_IN_DATA_SECTION = true;
        //                        ////XPathExpression expr3 = nav2.Compile("/TVBI/Data/[User='" + USER_NAME + "']/[Screen='" + SCREEN_NAME + "']/Grid");
        //                        XPathNodeIterator iterator3 = nav3.SelectChildren("Grid", "");
        //                        while (iterator3.MoveNext())
        //                        {
        //                            XPathNavigator nav4 = iterator3.Current.Clone();
        //                            string sTemp3 = nav4.GetAttribute("Name", "");
        //                            if (this.Name == sTemp3)
        //                            {
        //                                GRID_EXIST_IN_DATA_SECTION = true;
        //                                nav4.MoveToFirstChild();
        //                                string str = nav4.Value.ToString();
                                        
        //                                if (!LoadListFromFile(str))
        //                                { RetVal = false; break; }
        //                                else
        //                                { RetVal = true; break; }
        //                            }

        //                        }
        //                    }
        //                }
                        
        //            }
        //        }
        //        return RetVal;
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //}

        private bool DoCheckFileConfig()
        {
            bool RetVal = false;
            try
            {
                SCREEN_EXIST_IN_DATA_SECTION = false;
                RECORD_SPEC_EXIST_IN_DATA_SECTION = false;
                EUS_EXIST_IN_DATA_SECTION = false;
                FileStream _stream = null;
                XmlDocument doc = new XmlDocument();
                XmlTextReader reader =null;
                bool chkVal = true;
                while (chkVal)
                {
                    try
                    {
                        DoCheckFile();
                        _stream = new FileStream(FILE_NAME, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                        reader = new XmlTextReader(_stream);

                        doc.Load(reader);
                        reader.Close();
                        _stream.Close();


                        XmlNode _node;
                        XmlElement root = doc.DocumentElement;
                        _node = root.SelectSingleNode("/TVBI/Data");

                        XPathDocument XPathdoc = new XPathDocument(FILE_NAME);
                        nav = XPathdoc.CreateNavigator();

                        //expr = nav.Compile("/TVBI/Data/Screen");
                        expr = nav.Compile("/TVBI/Data");
                        iterator = nav.Select(expr);
                        while (iterator.MoveNext())
                        {
                            XPathNavigator nav4 = iterator.Current.Clone();
                            string sTempEUSCode = nav4.GetAttribute("EUS_CODE", "");
                            if (sTempEUSCode == MULTI_EUS_CODE)
                            {
                                EUS_EXIST_IN_DATA_SECTION = true;
                                XPathNodeIterator iterator3 = nav4.SelectChildren("Screen", "");
                                while (iterator3.MoveNext())
                                {
                                    XPathNavigator nav2 = iterator3.Current.Clone();
                                    string sTemp = nav2.GetAttribute("Name", "");
                                    if (sTemp == SCREEN_NAME.ToString())
                                    {
                                        SCREEN_EXIST_IN_DATA_SECTION = true;
                                        //nav2.SelectDescendants(XPathNodeType.Attribute, true);
                                        //XPathExpression expr2 = nav2.Compile("/Screen");
                                        XPathNodeIterator iterator2 = nav2.SelectChildren("RecordSpec", "");
                                        while (iterator2.MoveNext())
                                        {
                                            XPathNavigator nav3 = iterator2.Current.Clone();
                                            string sTempDistId = nav3.GetAttribute("DistId", "");
                                            string sTempDivnId = nav3.GetAttribute("DivnId", "");
                                            string sTempYear = nav3.GetAttribute("Year", "");
                                            string sTempPlanType = nav3.GetAttribute("PlanType", "");

                                            if ((sTempDistId.Trim().ToUpper() == DIST_ID.ToString().Trim().ToUpper()) && (sTempDivnId.Trim().ToUpper() == DIVN_ID.ToString().Trim().ToUpper()) && (sTempYear.Trim().ToUpper() == YEAR.ToString().Trim().ToUpper()) && (sTempPlanType.Trim().ToUpper() == PLAN_TYPE.ToString().Trim().ToUpper()))
                                            {
                                                RECORD_SPEC_EXIST_IN_DATA_SECTION = true;
                                                nav3.MoveToFirstChild();
                                                string str = nav3.Value.ToString();

                                                if (!LoadListFromFile(str))
                                                { RetVal = false; break; }
                                                else
                                                { RetVal = true; break; }
                                            }
                                            //if (sTemp2 == PLAN_TYPE.ToString())
                                            //{

                                            //}
                                        }

                                    }
                                }
                            }

                        }
                        
                        chkVal = false;
                    }
                    catch (IOException ex)
                    {
                       // MessageBox.Show(ex.Message.ToString());
                        if (ex.Message.ToString().StartsWith("The process cannot access the file"))
                            chkVal = true;
                        else
                            chkVal = false;
                    }

                }
                return RetVal;
            }
            catch
            {
                return false;
            }

        }

        private   ArrayList ReorderList = new ArrayList();

        public ArrayList RowListForReordering
        {
            get { return ReorderList; }
            set { ReorderList = value; }

        }
        private bool LoadListFromFile(string ExpressionToSplit)
        {
            try
            {
                if (ReorderList.Count > 0)
                    ReorderList.Clear();
                string[] sArr = ExpressionToSplit.Split('|');
                ReorderList.AddRange(sArr);
                return true;
            }
            catch
            {
                return false ;
            }

        }
        //private bool DoScreenExistUserProfile()
        //{
 
        //}


        private bool DoAddUserToConfigurationFile()
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(FILE_NAME);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                reader.Close();

                XmlElement root = doc.DocumentElement;
                XmlDocumentFragment docFrag = doc.CreateDocumentFragment();
                docFrag.InnerXml = "<USERID>" + USER_NAME + "</USERID>";
                XmlNode _SchemaNode = root.SelectSingleNode("/TVBI/Schema");
                _SchemaNode.InsertAfter(docFrag, _SchemaNode.LastChild);
                doc.Save(FILE_NAME);
                return true;
            }
            catch
            {
                return false;
            }
        }

        XPathNavigator nav;
        XPathExpression expr;
        XPathNodeIterator iterator;


        //*****************Data Filtering******************************************
        private bool _EnableDataFiltering = false;
        public bool EnableDataFiltering
        {
            get { return _EnableDataFiltering; }
            set 
            {
                _EnableDataFiltering = value;
                DoCreateFilterHeaders();
            }
        }

        private void DoCreateFilterHeaders()
        {
            if (EnableDataFiltering)
            {
                foreach (DataGridViewColumn col in this.Columns)
                {
                    Type t = col.GetType();
                    if (t.Name.ToString() == "DataGridViewButtonColumn")
                        continue;
                    col.HeaderCell = new
                        WritableGridFilterHeader(col.HeaderCell);
                }
                //FullData = this.dvSource.Table;
            }
        }

        public void RemoveFilter()
        {
            foreach (DataGridViewColumn col in this.Columns)
            {
                WritableGridFilterHeader header = (WritableGridFilterHeader)col.HeaderCell;
                if (header.Filtered)
                {
                    header.RemoveFilter();
                }
            }
        }


        
    }
}
