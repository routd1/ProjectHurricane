using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ICTEAS.WinForms.Common;
using System.Runtime.InteropServices;

namespace ICTEAS.WinForms.Controls
{
    public partial class TreeGridViewHeader: UserControl, ISupportInitialize
    {
        private ICTEAS.WinForms.Utilities.FxPanel pnlBackGorund;
        private int _initCounter = 0;
        private string _LabelHeaderMessageId = "";
        private TreeGridView _grid;
        private ArrayList _ListHeaderMapping = new ArrayList();
        private Hashtable _columnToGridFilterHash = new Hashtable();

        #region "     Designer.cs     "
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WritableGridHeader));
            this.pnlBackGorund = new ICTEAS.WinForms.Utilities.FxPanel(this.components);
            this.SuspendLayout();
            // 
            // pnlBackGorund
            // 
            this.pnlBackGorund.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlBackGorund.BackgroundImage")));
            this.pnlBackGorund.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlBackGorund.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackGorund.GradientStyle = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlBackGorund.Location = new System.Drawing.Point(0, 0);
            this.pnlBackGorund.Name = "pnlBackGorund";
            this.pnlBackGorund.PageEndColor = System.Drawing.Color.Silver;
            this.pnlBackGorund.PageStartColor = System.Drawing.Color.White;
            this.pnlBackGorund.Size = new System.Drawing.Size(307, 15);
            this.pnlBackGorund.TabIndex = 0;
            // 
            // WritableGridHeader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlBackGorund);
            this.DoubleBuffered = true;
            this.Name = "WritableGridHeader";
            this.Size = new System.Drawing.Size(307, 15);
            this.ResumeLayout(false);

        }

        #endregion
        #endregion

        #region "     Constructor    "
        public TreeGridViewHeader()
        {
            InitializeComponent();
        }
        #endregion

        #region "     Properties     "
        //public string LabelHeaderMessageId
        //{
        //    get { return _LabelHeaderMessageId; }
        //    set { _LabelHeaderMessageId = value; if (value != null) lblHeader.MessageID = value; }
        //}
        #endregion

        #region Internals

        /// <summary>
        /// Gets and sets the <see cref="DataGridView"/> instance to use.
        /// </summary>
        internal TreeGridView DataGridView
        {
            get { return _grid; }
            set
            {
                if (_grid != null)
                {
                    _grid.DataSourceChanged -= new System.EventHandler(this.OnDataSourceChanged);
                    //_grid.DataMemberChanged -= new System.EventHandler(this.OnDataSourceChanged);
                    _grid.ColumnWidthChanged -= new DataGridViewColumnEventHandler(OnGridColumnsChanged);
                    _grid.ColumnDisplayIndexChanged -= new DataGridViewColumnEventHandler(OnGridColumnsChanged);
                    //_grid.ColumnAdded -= new DataGridViewColumnEventHandler(OnGridColumnsAddedRemoved);
                    //_grid.ColumnRemoved -= new DataGridViewColumnEventHandler(OnGridColumnsAddedRemoved);
                    //_grid.ColumnStateChanged -= new DataGridViewColumnStateChangedEventHandler(OnGridColumnsStateChanged);
                    _grid.Scroll -= new ScrollEventHandler(OnGridScroll);
                    _grid.Resize -= new EventHandler(_grid_Resize);
                }

                _grid = value;

                if (_grid != null)
                {
                    _grid.DataSourceChanged += new System.EventHandler(this.OnDataSourceChanged);
                    //_grid.DataMemberChanged += new System.EventHandler(this.OnDataSourceChanged);
                    _grid.ColumnWidthChanged += new DataGridViewColumnEventHandler(OnGridColumnsChanged);
                    _grid.ColumnDisplayIndexChanged += new DataGridViewColumnEventHandler(OnGridColumnsChanged);
                    //_grid.ColumnAdded += new DataGridViewColumnEventHandler(OnGridColumnsAddedRemoved);
                    //_grid.ColumnRemoved += new DataGridViewColumnEventHandler(OnGridColumnsAddedRemoved);
                    _grid.ColumnStateChanged += new DataGridViewColumnStateChangedEventHandler(OnGridColumnsStateChanged);
                    _grid.Scroll += new ScrollEventHandler(OnGridScroll);
                    _grid.Resize += new EventHandler(_grid_Resize);
                }
                //RecreateGridFilters();
            }
        }

        private void OnDataSourceChanged(object sender, System.EventArgs e)
        {
            //RepositionGridHeaders();
        }

        void _grid_Resize(object sender, EventArgs e)
        {
            RepositionGridHeaders();
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            //CreateGridHeader();
            base.OnLoad(e);
        }


        public  void CreateGridHeader()
        {
            _ListHeaderMapping = _grid.GetHeaderToColumnsMapping();
             _ListHeaderMapping.Reverse();
            //this.Controls.Remove(lblHeader);
            if (_ListHeaderMapping != null)
            {
                if (_ListHeaderMapping.Count > 0)
                {
                    IEnumerator _Enumerate = _ListHeaderMapping.GetEnumerator();
                    while (_Enumerate.MoveNext())
                    {
                        object[] objArr = (object[])_Enumerate.Current;
                        if (objArr.Length != 3)
                        {
                            //this.Visible = false;
                            //return;
                        }
                       // clsLabel _label = LabelGenerator(objArr[0].ToString(), objArr[1].ToString(), Convert.ToInt32(objArr[2]));
                        //this.Controls.Add(_label);

                    }
                }
                else
                {
                    //this.Visible = false;
                    //return;
                }
            }
            else
            {
                //this.Visible = false;
                //return;
            }
        }
        private ArrayList _Type1Columns = new ArrayList();
        private ArrayList _Type2Columns = new ArrayList();
        private ArrayList _BorderedColumns = new ArrayList();
        public ArrayList Type1Columns
        {
            get { return _Type1Columns; }
            set { _Type1Columns=value; }
        }

        public ArrayList Type2Columns
        {
            get { return _Type2Columns; }
            set { _Type2Columns = value; }
        }

        public ArrayList BorderedColumns
        {
            get { return _BorderedColumns; }
            set { _BorderedColumns = value; }
        }

        int w = 0;
        private bool GridDeisgnComplete = false;
        public bool DesignGridHeaders(int width)
        {
            _ListHeaderMapping = _grid.GetHeaderToColumnsMapping();
            _ListHeaderMapping.Reverse();
            //this.Controls.Remove(lblHeader);
            //this.Width = _grid.Width;
            //this.Width = 928;
            //w = 928;
            this.Width = width;
            this.Width = width;
            w = width;
            _columnToGridFilterHash.Clear();

            //adjust the position for the filter GUI
            //this.Height = _refBox.Height;

            if (_grid == null)
                return false;

            int rowHeadersWidth = _grid.RowHeadersVisible ? _grid.RowHeadersWidth : 0;
            //lblHeader.Width = rowHeadersWidth;
            //lblHeader.Visible = true;

            //_filterFactory.BeginGridFilterCreation();
            try
            {
                //for (int i = 0; i < _grid.Columns.Count; i++)
                for (int i = _grid.Columns.Count - 1; i >= 0; i--)
                {
                    int Index = -1;
                    if (!HasColumnHeader(i, ref Index))
                    {
                        //continue;
                        DataGridViewColumn column = _grid.Columns[i];
                        PaintType type = PaintType.None;
                        BorderType _borderType = BorderType.None;
                        if (Type1Columns.Contains(i))
                            type = PaintType.TypeI;
                        else if (Type2Columns.Contains(i))
                            type = PaintType.TypeII;

                        if (BorderedColumns.Contains(i))
                        {
                            _borderType = BorderType.Right;
                        }
                        clsLabel _label = LabelGenerator("", "", 0, true, column.Width, type, _borderType);
                        //if (i == 0)
                        //    _label.Size = new Size(164, this.Height);
                        if (i == 0)
                            _label.Size = new Size(164, this.Height);
                        this.Controls.Add(_label);
                        //added to hash to provider fast access
                        _columnToGridFilterHash.Add(column, _label);
                    }
                    else
                    {

                        DataGridViewColumn column = _grid.Columns[i];
                        object[] objArr = (object[])_ListHeaderMapping[Index];
                        PaintType type = PaintType.None;
                        BorderType _borderType = BorderType.None;
                        if (Type1Columns.Contains(i))
                            type = PaintType.TypeI;
                        else if (Type2Columns.Contains(i))
                            type = PaintType.TypeII;
                        if (BorderedColumns.Contains(i))
                        {
                            _borderType = BorderType.Right;
                        }
                        clsLabel _label = LabelGenerator(objArr[0].ToString(), objArr[1].ToString(), Convert.ToInt32(objArr[2]), false, column.Width, type, _borderType);
                        //if (i == 0)
                        //    _label.Size = new Size(164, this.Height);
                        //if (i == 0)
                        //    _label.Size = new Size(164, this.Height);
                        //_label.ShowToolTips = true;
                        this.Controls.Add(_label);
                        //added to hash to provider fast access
                        _columnToGridFilterHash.Add(column, _label);
                    }
                }
            }
            finally
            {
                //_filterFactory.EndGridFilterCreation();
            }
            //if (_keepFilters && _keepFiltersHash.ContainsKey(GetTableName()))
            //    SetFilters((string[])_keepFiltersHash[GetTableName()]);

            SetSortedColumns();
            GridDeisgnComplete = true;
            RepositionGridHeaders();
            this.Invalidate();
            return true;
        }

        public void RepositionGridHeaders()
        {
            if (GridDeisgnComplete)
            {
                _grid.Invalidate(true);
                if (_initCounter > 0)
                    return;

                if (_grid == null || _grid.Columns == null || _grid.Columns.Count == 0)
                    return;

                try
                {
                    this.SuspendLayout();

                    //int rowHeadersWidth = _grid.RowHeadersVisible ? _grid.RowHeadersWidth : 0;
                    //int filterWidth = _grid.RowHeadersVisible ? _grid.RowHeadersWidth - 1 : 0;
                    //int curPos = rowHeadersWidth;

                    int rowHeadersWidth =  0;
                    int filterWidth =  0;
                    int curPos = rowHeadersWidth;

                    if (filterWidth > 0)
                    {
                        //lblHeader.Width = filterWidth;
                        //lblHeader.Visible = true;
                        curPos++;
                        if (base.RightToLeft == RightToLeft.Yes)
                        {
                            //if (lblHeader.Dock != DockStyle.Right)
                            //    lblHeader.Dock = DockStyle.Right;
                        }
                        else
                        {
                            //if (lblHeader.Dock != DockStyle.Left)
                            //    lblHeader.Dock = DockStyle.Left;
                        }
                    }
                    else
                    {
                        //if (lblHeader.Visible)
                        //    lblHeader.Visible = false;
                    }

                    //this loop goes through all column styles and iteratively sets 
                    //their horizontal positions and widths
                    List<DataGridViewColumn> sortedColumns = result1;
                    for (int i = 0; i < sortedColumns.Count; i++)
                    {
                        DataGridViewColumn column = sortedColumns[i];

                        clsLabel label = _columnToGridFilterHash[column] as clsLabel;
                        if (label != null)
                        {
                            if (!column.Visible)
                            {
                                if (label.Visible)
                                    label.Visible = false;
                                continue;
                            }
                            int from = curPos - _grid.HorizontalScrollingOffset;
                            int width = column.Width + (i == 0 ? 1 : 0);

                            if (from < rowHeadersWidth)
                            {
                                width -= rowHeadersWidth - from;
                                from = rowHeadersWidth;
                            }

                            if (from + width > base.Width)
                                width = base.Width - from;

                            if (width < 4)
                            {
                                if (label.Visible)
                                    label.Visible = false;
                            }
                            else
                            {
                                if (base.RightToLeft == RightToLeft.Yes)
                                    from = base.Width - from - width;

                                if (label.Left != from || label.Width != width)
                                    label.SetBounds(from, 0, width, 0, BoundsSpecified.X | BoundsSpecified.Width);

                                if (!label.Visible)
                                    label.Visible = true;
                            }
                        }
                        curPos += column.Width + (i == 0 ? 1 : 0);
                    }
                }
                finally
                {
                    this.ResumeLayout();
                }

                this.Invalidate();


                //************************************************************
                //try
                //{

                //    if (_initCounter > 0)
                //        return;

                //    if (_grid == null || _grid.Columns == null || _grid.Columns.Count == 0)
                //        return;

                //    try
                //    {
                //        this.SuspendLayout();

                //        int rowHeadersWidth = _grid.RowHeadersVisible ? _grid.RowHeadersWidth : 0;
                //        int filterWidth = _grid.RowHeadersVisible ? _grid.RowHeadersWidth - 1 : 0;
                //        int curPos = rowHeadersWidth;

                //        if (filterWidth > 0)
                //        {
                //            lblHeader.Width = filterWidth;
                //            lblHeader.Visible = true;
                //            curPos++;
                //            if (base.RightToLeft == RightToLeft.Yes)
                //            {
                //                if (lblHeader.Dock != DockStyle.Right)
                //                    lblHeader.Dock = DockStyle.Right;
                //            }
                //            else
                //            {
                //                if (lblHeader.Dock != DockStyle.Left)
                //                    lblHeader.Dock = DockStyle.Left;
                //            }
                //        }
                //        else
                //        {
                //            if (lblHeader.Visible)
                //                lblHeader.Visible = false;
                //        }
                //        lblHeader.Visible = false;
                //        //this loop goes through all column styles and iteratively sets 
                //        //their horizontal positions and widths
                //        List<DataGridViewColumn> sortedColumns = SortedColumns;
                //        for (int i = 0; i < sortedColumns.Count; i++)
                //        {
                //            DataGridViewColumn column = sortedColumns[i];
                //            int Index = -1;
                //            //if (!column.Displayed)
                //            //{
                //            //    continue;
                //            //}
                //            //if (!HasColumnHeader(i, ref Index))
                //            //    continue;
                //            //DataGridViewHeaderCell cell = column.HeaderCell;
                //            //Rectangle r = cell.ContentBounds;
                //            //int x = this.PointToClient(r.Location).X;
                //            clsLabel _clsLabel = _columnToGridFilterHash[column] as clsLabel;
                //            if (_clsLabel != null)
                //            {
                //                if (!column.Visible)
                //                {
                //                    if (_clsLabel.Visible)
                //                        _clsLabel.Visible = false;
                //                    continue;
                //                }
                //                int from = curPos - _grid.HorizontalScrollingOffset;
                //                //int width = column.Width + (i == 0 ? 1 : 0);
                //                int width = _clsLabel.Width ;

                //                if (from < rowHeadersWidth)
                //                {
                //                    //if (from < 0)
                //                    //    width -= rowHeadersWidth + from;
                //                    //else
                //                    //width -= rowHeadersWidth - from;
                //                    width -=  - from;
                //                    from = rowHeadersWidth;
                //                }

                //                if (from + width > w)
                //                    width = w - from;

                //                if (width < 4)
                //                {
                //                    if (_clsLabel.Visible)
                //                        _clsLabel.Visible = false;
                //                }
                //                else
                //                {
                //                    if (base.RightToLeft == RightToLeft.Yes)
                //                        from = w - from - width;
                //                    _clsLabel.SetBounds(from, 0, _clsLabel.Width, 0, BoundsSpecified.X | BoundsSpecified.Width);

                //                    //if (_clsLabel.Left != from || _clsLabel.Width != width)
                //                    //if (_clsLabel.Width != width)
                //                    //{
                //                    //    _clsLabel.SetBounds(from, 0, _clsLabel.Width, 0, BoundsSpecified.X | BoundsSpecified.Width);
                //                    //    //_clsLabel.SetBounds(r.X, 0, _clsLabel.Width, 0, BoundsSpecified.X | BoundsSpecified.Width);
                //                    //}

                //                    if (!_clsLabel.Visible)
                //                        _clsLabel.Visible = true;
                //                }
                //                //curPos += column.Width + (i == 0 ? 1 : 0);
                //            }
                //            //curPos += column.Width + (i == 0 ? 1 : 0);
                //            if (_clsLabel != null)
                //                curPos += _clsLabel.Width + (i == 0 ? 1 : 0);

                //        }
                //    }
                //    finally
                //    {
                //        this.ResumeLayout();
                //    }

                //    this.Invalidate();
                //}
                //catch(Exception ex)
                //{
                //    MessageBox.Show(ex.Message.ToString());
                //}

                //************************************************************
            }
        }

        private void RepositionRelativeCell(int Col)
        {
            //DataGridViewColumn column = sortedColumns[Col];
            //DataGridViewHeaderCell cell = column.HeaderCell;
            //Rectangle r = cell.ContentBounds;
            //int x = this.PointToClient(r.Location).X;
            //clsLabel _clsLabel = _columnToGridFilterHash[column] as clsLabel;
            //if (_clsLabel != null)
            //    _clsLabel.SetBounds(r.X, 0, _clsLabel.Width, 0, BoundsSpecified.X | BoundsSpecified.Width);
        }
        private List<DataGridViewColumn> SortedColumns
        {
            get
            {
                List<DataGridViewColumn> result = new List<DataGridViewColumn>();
                DataGridViewColumn column = _grid.Columns.GetFirstColumn(DataGridViewElementStates.None);
                if (column == null)
                    return result;
                result.Add(column);
                while ((column = _grid.Columns.GetNextColumn(column, DataGridViewElementStates.None, DataGridViewElementStates.None)) != null)
                    result.Add(column);

                return result;
            }
        }

        List<DataGridViewColumn> result1;
        private void SetSortedColumns()
        {
            result1 = new List<DataGridViewColumn>();
            DataGridViewColumn column = _grid.Columns.GetFirstColumn(DataGridViewElementStates.None);
            if (column == null)
                return ;
            result1.Add(column);
            while ((column = _grid.Columns.GetNextColumn(column, DataGridViewElementStates.None, DataGridViewElementStates.None)) != null)
                result1.Add(column);
        }

        private bool HasColumnHeader(int ColumnNum,ref int Index)
        {
            if (_ListHeaderMapping != null)
            {
                if (_ListHeaderMapping.Count > 0)
                {
                    int i = -1;
                    IEnumerator _Enumerate = _ListHeaderMapping.GetEnumerator();
                    while (_Enumerate.MoveNext())
                    {
                        i++;
                        object[] objArr = (object[])_Enumerate.Current;
                        if (objArr.Length != 4)
                        {
                            return false;
                        }
                        if (objArr[3].ToString() == ColumnNum.ToString())
                        {
                            Index = i;
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private clsLabel LabelGenerator(string MessageId, string Text,int Width, bool IsBlank,int colWidth, PaintType type,BorderType _BorderType)
        {
            try
            {
                clsLabel _Label = new clsLabel();
                _Label.TypeOfPaint = type;
                _Label.TypeOfBorder = _BorderType;
                _Label.BackColor = System.Drawing.Color.Transparent;
                _Label.Dock = System.Windows.Forms.DockStyle.Left;
                _Label.FieldType = ICTEAS.WinForms.Controls.LabelType.Optional;

                //if (!IsBlank)
                //    _Label.BorderStyle = BorderStyle.FixedSingle;
                //else
                   // _Label.BorderStyle = BorderStyle.FixedSingle;
                _Label.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                _Label.Location = new System.Drawing.Point(0, 0);
                _Label.MessageID = MessageId;
                _Label.Name = "lbl" + Text;
                _Label.NormalFont = true;
                _Label.ResourceName = "";
                _Label.Size = new Size(colWidth, this.Height);
                //if (!IsBlank)
                //    _Label.Size = new System.Drawing.Size(Width, this.Height);
                //else
                //    _Label.Size = new Size(colWidth, this.Height);
                _Label.TabIndex = 6;
                _Label.Text = Text;
                if (!IsBlank)
                    SetMessageForLabel(_Label, MessageId);
                else
                    _Label.Text = "";
                _Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                return _Label;
            }
            catch
            {
                return null;
            }
        }

        private void SetMessageForLabel(clsLabel _label, string strMsgID)
        {
            clsLanguage objMessage;

            if (strMsgID.StartsWith("$"))
            {
                _label.Text = strMsgID.Substring(1, strMsgID.Length - 1).ToString();
                return;
            }
            if (strMsgID.Trim() == "")
            {
                if (_label.Text.Trim() != "")
                {
                    _label.Text = "** " + _label.Text;
                }
                return;
            }

            objMessage = new clsLanguage();

            string strTemp = objMessage.LanguageString(strMsgID);

            if (strTemp == "*****")
            {
                _label.ForeColor = System.Drawing.Color.Orange;
                _label.Text = "**" + _label.Text;
            }
            else
            {
                _label.Text = strTemp;
            }

            objMessage.Dispose();
            objMessage = null;
        }

        private void OnGridScroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                RepositionGridHeaders();
            }
            
        }

        private void OnGridColumnsChanged(object sender, DataGridViewColumnEventArgs e)
        {
            RepositionGridHeaders();
        }

        private void OnColumnStyleWidthChanged(object sender, EventArgs e)
        {
            RepositionGridHeaders();
        }



        private void OnGridColumnsStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.Visible)
                RepositionGridHeaders();
        }
        #region ISupportInitialize Members

        public void BeginInit()
        {
            _initCounter++;
        }

        public void EndInit()
        {
            --_initCounter;
        }

        #endregion


    }
}
