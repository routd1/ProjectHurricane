
namespace ICTEAS.WinForms.Controls
{
    #region "                   Namespaces                  "
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Diagnostics;
    using System.Windows.Forms.VisualStyles;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.ComponentModel.Design.Serialization;
    using System.Drawing.Design;
    using System.Collections;
    using ICTEAS.DataComponents.Base;
    using ICTEAS.DataComponents.Custom;
    using System.Data;
    using System.Globalization;
    using ICTEAS.WinForms.Common;
    using System.Drawing.Drawing2D;
    #endregion

    [System.ComponentModel.DesignerCategory("code"),
    Designer(typeof(System.Windows.Forms.Design.ControlDesigner)),
    ComplexBindingProperties(),
    Docking(DockingBehavior.Ask)]
    public class TreeGridView : DataGridView
    {
        #region "               Variables & Declarations                    "
        private int _parentRowHeight = 18;
        private int _childRowHeight = 18;
        private int _indentWidth;
        private TreeGridNode _root;
        private TreeGridColumn _expandableColumn;
        private bool _disposing = false;
        internal ImageList _imageList;
        private bool _inExpandCollapse = false;
        internal bool _inExpandCollapseMouseCapture = false;
        private Control hideScrollBarControl;
        private bool _showLines = true;
        private bool _virtualNodes = false;
        public ContextMenuStrip objContextMenu = new ContextMenuStrip();
        public ToolStripMenuItem objAddItem = new ToolStripMenuItem();
        public ToolStripMenuItem objRemoveItem = new ToolStripMenuItem();

        internal VisualStyleRenderer rOpen; //= new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Opened);
        internal VisualStyleRenderer rClosed;// = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Closed);
        #endregion

        #region "                   Constructor                 "
        public TreeGridView()
        {
            //if (Application.VisualStyleState == VisualStyleState.ClientAndNonClientAreasEnabled)
            //{
            //    rOpen = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Opened);
            //    rClosed = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Closed);
            //}
            // Control when edit occurs because edit mode shouldn't start when expanding/collapsing
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            clrParentRowFont = this.Font;
            clrChildRowFont = this.Font;
            this.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.RowTemplate = new TreeGridNode() as DataGridViewRow;
            // This sample does not support adding or deleting rows by the user.
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this._root = new TreeGridNode(this);
            this._root.IsRoot = true;

            // Ensures that all rows are added unshared by listening to the CollectionChanged event.
            base.Rows.CollectionChanged += delegate(object sender, System.ComponentModel.CollectionChangeEventArgs e) { };



            arlNumericColFloatSize = new ArrayList();
            arlNumericColIntSize = new ArrayList();
            arlNumericColumnType = new ArrayList();
            arlNumericColumNum = new ArrayList();
            arlNumericColCurrency = new ArrayList();
            arlStringColumsStrLength = new ArrayList();
            arlStringColumnCollection = new ArrayList();

            arlFixedLengthNumCols = new ArrayList();
            this.CurrentCellDirtyStateChanged += new EventHandler(TreeGridView_CurrentCellDirtyStateChanged);

            this.objContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.objAddItem,
            this.objRemoveItem});
            this.objContextMenu.Name = "contextMenuStrip1";
            this.objContextMenu.ShowCheckMargin = false;
            this.objContextMenu.Size = new System.Drawing.Size(166, 22);
            objContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;

            this.objAddItem.Name = "objAddItem";
            this.objAddItem.Size = new System.Drawing.Size(166, 22);
            this.objAddItem.Text = "Add Row";
            this.objAddItem.Visible = true;

            this.objRemoveItem.Name = "objRemoveItem";
            this.objRemoveItem.Size = new System.Drawing.Size(166, 22);
            this.objRemoveItem.Text = "Remove Row";
            this.objRemoveItem.Visible = true;


        }

        private bool blEnableContextMenu = true;
        public bool EnableContextMenu
        {
            get { return blEnableContextMenu; }
            set { blEnableContextMenu = value; }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            HitTestInfo info = this.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right && EnableContextMenu)
            {
                if (info.Type == DataGridViewHitTestType.Cell || info.Type == DataGridViewHitTestType.RowHeader)
                {
                    this.objRemoveItem.Visible = true;
                }
                else
                {
                    this.objRemoveItem.Visible = false;
                }
                objContextMenu.Show(this, e.Location, ToolStripDropDownDirection.Default);
            }
            base.OnMouseDown(e);
        }
        private bool _IsFloatSetting = false;

        [Browsable(false)]
        public bool IsFloatSetting
        {
            get { return _IsFloatSetting; }
            set { _IsFloatSetting = value; }
        }

        void TreeGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                this.CommitEdit(DataGridViewDataErrorContexts.Commit);

                if (this.objEditingcontrol != null)
                {
                    if (!IsFloatSetting)
                        this.objEditingcontrol.SelectionStart = this.CurrentCell.EditedFormattedValue.ToString().Length;
                    else
                    {
                        //this.editingcontrol.SelectionStart = this.CurrentCell.EditedFormattedValue.ToString().Length;
                        //this.editingcontrol.SelectionLength = 0;
                        if (selectionStart != -1)
                        {
                            int i = selectionStart + noOfCommas;
                            this.objEditingcontrol.SelectionStart = i;
                            this.objEditingcontrol.SelectionLength = selectionLength;
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

        public void ReplaceValue(int RowNum, int ColNum, string ReplaceString)
        {
            //Changed 26 06 2008

            int _colNum = this.CurrentCell.ColumnIndex;
            int _rowNum = this.CurrentCell.RowIndex;
            
            //this.Refresh();

            //--------------------
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
            NumberFormatInfo nmfi = new NumberFormatInfo();
            nmfi = ci.NumberFormat;
            ci.ClearCachedData();


            int m_intColPos = -1;
            if (CheckNumericColumn(get_actual_colum_index(ColNum), ref m_intColPos))
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
                    //dvSource[RowNum][ColNum] = rep;
                    //this.Nodes[RowNum].Cells[ColNum].Value = rep;
                    this.GetNodeForRow(RowNum).Cells[ColNum].Value = rep;
                    #endregion
                }

                else
                {
                    //this.Nodes[RowNum].Cells[ColNum].Value = ReplaceString;
                    this.GetNodeForRow(RowNum).Cells[ColNum].Value = Convert.ToDouble(ReplaceString);// String.Format("{0:0,0}", ReplaceString); //ReplaceString;
                }
            }
            else
            {
               //this.Nodes[RowNum].Cells[ColNum].Value =ReplaceString;
               this.GetNodeForRow(RowNum).Cells[ColNum].Value = ReplaceString;
                
            }

            //this.CurrentCell = this.Rows[_rowNum].Cells[get_actual_colum_index(_colNum)];


            //-------------------------------------
            this.BeginEdit(true);

            CultureInfo inf = new CultureInfo(CultureInfo.CurrentCulture.LCID, true);
            inf.NumberFormat.NumberGroupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            inf.NumberFormat.NumberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            this.DefaultCellStyle.FormatProvider = inf;

            this.DefaultCellStyle.FormatProvider = inf;
        }

        #endregion

        #region "                   Keyboard F2 to begin edit support                   "
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Cause edit mode to begin since edit mode is disabled to support 
            // expanding/collapsing 
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.F2)
                objEditingcontrol_KeyDown(this.CurrentCell, e);
            //if (!e.Handled)
            //{
            //    if (e.KeyCode == Keys.F2 && this.CurrentCellAddress.X > -1 && this.CurrentCellAddress.Y > -1)
            //    {
            //        if (!this.CurrentCell.Displayed)
            //        {
            //            this.FirstDisplayedScrollingRowIndex = this.CurrentCellAddress.Y;
            //        }
            //        else
            //        {
            //            // TODO:calculate if the cell is partially offscreen and if so scroll into view
            //        }
            //        this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //        this.BeginEdit(true);
            //    }
            //    else if ((e.KeyCode == Keys.Enter && !this.IsCurrentCellInEditMode)||this.EnableHotSearch)
            //    {
            //        this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //        this.CurrentCell.OwningRow.Selected = true;
            //    }
            //}
        }
        #endregion

        #region "                   Shadow and hide DGV properties                  "

        // This sample does not support databinding
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new object DataSource
        {
            get { return null; }
            set { throw new NotSupportedException("The TreeGridView does not support databinding"); }
        }

        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new object DataMember
        {
            get { return null; }
            set { throw new NotSupportedException("The TreeGridView does not support databinding"); }
        }

        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new DataGridViewRowCollection Rows
        {
            get { return base.Rows; }
        }

        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new bool VirtualMode
        {
            get { return false; }
            set { throw new NotSupportedException("The TreeGridView does not support virtual mode"); }
        }

        // none of the rows/nodes created use the row template, so it is hidden.
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new DataGridViewRow RowTemplate
        {
            get { return base.RowTemplate; }
            set { base.RowTemplate = value; }
        }

        #endregion

        #region "                   Public methods                  "
        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public TreeGridNode GetNodeForRow(DataGridViewRow row)
        {
            return row as TreeGridNode;
        }

        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public TreeGridNode GetNodeForRow(int index)
        {
            return GetNodeForRow(base.Rows[index]);
        }
        #endregion

        #region "                   Public properties                   "
        [Category("Data"),
        Description("The collection of root nodes in the treelist."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public TreeGridNodeCollection Nodes
        {
            get
            {
                return this._root.Nodes;
            }
        }

        public int ParentRowHeight
        {
            get{ return _parentRowHeight;}
            set { _parentRowHeight = value; }
        }

        public int ChildRowHeight
        {
            get { return _childRowHeight; }
            set { _childRowHeight = value; }
        }

        public new TreeGridNode CurrentRow
        {
            get
            {
                return base.CurrentRow as TreeGridNode;
            }
        }

        [DefaultValue(false),
        Description("Causes nodes to always show as expandable. Use the NodeExpanding event to add nodes.")]
        public bool VirtualNodes
        {
            get { return _virtualNodes; }
            set { _virtualNodes = value; }
        }

        public TreeGridNode CurrentNode
        {
            get
            {
                return this.CurrentRow;
            }
        }

        [DefaultValue(true)]
        public bool ShowLines
        {
            get { return this._showLines; }
            set
            {
                if (value != this._showLines)
                {
                    this._showLines = value;
                    this.Invalidate();
                }
            }
        }

        public ImageList ImageList
        {
            get { return this._imageList; }
            set
            {
                this._imageList = value;
                //TODO: should we invalidate cell styles when setting the image list?

            }
        }

        public new int RowCount
        {
            get { return this.Nodes.Count; }
            set
            {
                for (int i = 0; i < value; i++)
                    this.Nodes.Add(new TreeGridNode());

            }
        }
        #endregion

        #region "                   Site nodes and collapse/expand support                  "
        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            // Notify the row when it is added to the base grid 
            int count = e.RowCount - 1;
            TreeGridNode row;
            while (count >= 0)
            {
                row = base.Rows[e.RowIndex + count] as TreeGridNode;
                if (row != null)
                {
                    row.Sited();
                }
                count--;
            }
        }

        internal protected void UnSiteAll()
        {
            this.UnSiteNode(this._root);
        }

        internal protected virtual void UnSiteNode(TreeGridNode node)
        {
            try
            {
                if (node.IsSited || node.IsRoot)
                {
                    // remove child rows first
                    foreach (TreeGridNode childNode in node.Nodes)
                    {
                        this.UnSiteNode(childNode);
                    }

                    // now remove this row except for the root
                    if (!node.IsRoot)
                    {
                        base.Rows.Remove(node);
                        // Row isn't sited in the grid anymore after remove. Note that we cannot
                        // Use the RowRemoved event since we cannot map from the row index to
                        // the index of the expandable row/node.
                        node.UnSited();
                    }
                }
            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
            }
        }

        internal protected virtual bool CollapseNode(TreeGridNode node)
        {
            try
            {
            if (node.IsExpanded)
            {
                CollapsingEventArgs exp = new CollapsingEventArgs(node);
                this.OnNodeCollapsing(exp);

                if (!exp.Cancel)
                {
                    this.LockVerticalScrollBarUpdate(true);
                    this.SuspendLayout();
                    _inExpandCollapse = true;
                    node.IsExpanded = false;

                    foreach (TreeGridNode childNode in node.Nodes)
                    {
                        Debug.Assert(childNode.RowIndex != -1, "Row is NOT in the grid.");
                        this.UnSiteNode(childNode);
                    }

                    CollapsedEventArgs exped = new CollapsedEventArgs(node);
                    this.OnNodeCollapsed(exped);
                    //TODO: Convert this to a specific NodeCell property
                    _inExpandCollapse = false;
                    this.LockVerticalScrollBarUpdate(false);
                    this.ResumeLayout(true);
                    this.InvalidateCell(node.Cells[0]);

                }

                return !exp.Cancel;
            }
            else
            {
                // row isn't expanded, so we didn't do anything.				
                return false;
            }
        }
        catch
        {
            MessageBox.Show("COMPONENT ERROR");
            return false;
        }
        }

        internal protected virtual void SiteNode(TreeGridNode node)
        {
            try
            {
                //TODO: Raise exception if parent node is not the root or is not sited.
                int rowIndex = -1;
                TreeGridNode currentRow;
                node._grid = this;

                if (node.Parent != null && node.Parent.IsRoot == false)
                {
                    // row is a child
                    Debug.Assert(node.Parent != null && node.Parent.IsExpanded == true);

                    if (node.Index > 0)
                    {
                        currentRow = node.Parent.Nodes[node.Index - 1];
                    }
                    else
                    {
                        currentRow = node.Parent;
                    }
                }
                else
                {
                    // row is being added to the root
                    if (node.Index > 0)
                    {
                        currentRow = node.Parent.Nodes[node.Index - 1];
                    }
                    else
                    {
                        currentRow = null;
                    }

                }

                if (currentRow != null)
                {
                    while (currentRow.Level >= node.Level)
                    {
                        if (currentRow.RowIndex < base.Rows.Count - 1)
                        {
                            currentRow = base.Rows[currentRow.RowIndex + 1] as TreeGridNode;
                            Debug.Assert(currentRow != null);
                        }
                        else
                            // no more rows, site this node at the end.
                            break;

                    }
                    if (currentRow == node.Parent)
                        rowIndex = currentRow.RowIndex + 1;
                    else if (currentRow.Level < node.Level)
                        rowIndex = currentRow.RowIndex;
                    else
                        rowIndex = currentRow.RowIndex + 1;
                }
                else
                    rowIndex = 0;


                Debug.Assert(rowIndex != -1);
                this.SiteNode(node, rowIndex);

                Debug.Assert(node.IsSited);
                if (node.IsExpanded)
                {
                    // add all child rows to display
                    foreach (TreeGridNode childNode in node.Nodes)
                    {
                        //TODO: could use the more efficient SiteRow with index.
                        this.SiteNode(childNode);
                    }
                }
            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
            }
        }


        internal protected virtual void SiteNode(TreeGridNode node, int index)
        {
            try
            {
                if (index < base.Rows.Count)
                {
                    base.Rows.Insert(index, node);
                }
                else
                {
                    // for the last item.
                    base.Rows.Add(node);
                }
            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
            }
        }

        internal protected virtual bool ExpandNode(TreeGridNode node)
        {
            try
            {
                if (!node.IsExpanded || this._virtualNodes)
                {
                    ExpandingEventArgs exp = new ExpandingEventArgs(node);
                    this.OnNodeExpanding(exp);

                    if (!exp.Cancel)
                    {
                        this.LockVerticalScrollBarUpdate(true);
                        this.SuspendLayout();
                        _inExpandCollapse = true;
                        node.IsExpanded = true;

                        //TODO Convert this to a InsertRange
                        foreach (TreeGridNode childNode in node.Nodes)
                        {
                            Debug.Assert(childNode.RowIndex == -1, "Row is already in the grid.");

                            this.SiteNode(childNode);
                            //this.BaseRows.Insert(rowIndex + 1, childRow);
                            //TODO : remove -- just a test.
                            //childNode.Cells[0].Value = "child";
                        }

                        ExpandedEventArgs exped = new ExpandedEventArgs(node);
                        this.OnNodeExpanded(exped);
                        //TODO: Convert this to a specific NodeCell property
                        _inExpandCollapse = false;
                        this.LockVerticalScrollBarUpdate(false);
                        this.ResumeLayout(true);
                        this.InvalidateCell(node.Cells[0]);
                    }

                    return !exp.Cancel;
                }
                else
                {
                    // row is already expanded, so we didn't do anything.
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
                return false;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // used to keep extra mouse moves from selecting more rows when collapsing
            try
            {
                base.OnMouseUp(e);
                this._inExpandCollapseMouseCapture = false;
            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // while we are expanding and collapsing a node mouse moves are
            // supressed to keep selections from being messed up.
            try
            {
                if (!this._inExpandCollapseMouseCapture)
                    base.OnMouseMove(e);
            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
            }


        }
        #endregion

        #region "                   Collapse/Expand events                  "
        public event ExpandingEventHandler NodeExpanding;
        public event ExpandedEventHandler NodeExpanded;
        public event CollapsingEventHandler NodeCollapsing;
        public event CollapsedEventHandler NodeCollapsed;

        protected virtual void OnNodeExpanding(ExpandingEventArgs e)
        {
            if (this.NodeExpanding != null)
            {
                NodeExpanding(this, e);
            }
        }
        protected virtual void OnNodeExpanded(ExpandedEventArgs e)
        {
            if (this.NodeExpanded != null)
            {
                NodeExpanded(this, e);
            }
        }
        protected virtual void OnNodeCollapsing(CollapsingEventArgs e)
        {
            if (this.NodeCollapsing != null)
            {
                NodeCollapsing(this, e);
            }

        }
        protected virtual void OnNodeCollapsed(CollapsedEventArgs e)
        {
            if (this.NodeCollapsed != null)
            {
                NodeCollapsed(this, e);
            }
        }
        #endregion

        #region "                   Helper methods                  "
        protected override void Dispose(bool disposing)
        {
            this._disposing = true;
            base.Dispose(Disposing);
            this.UnSiteAll();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            try
            {

                // this control is used to temporarly hide the vertical scroll bar
                hideScrollBarControl = new Control();
                hideScrollBarControl.Visible = false;
                hideScrollBarControl.Enabled = false;
                hideScrollBarControl.TabStop = false;
                // control is disposed automatically when the grid is disposed
                this.Controls.Add(hideScrollBarControl);
            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
            }
        }

        protected override void OnRowEnter(DataGridViewCellEventArgs e)
        {
            // ensure full row select
            try
            {
                base.OnRowEnter(e);
                if (this.SelectionMode == DataGridViewSelectionMode.CellSelect ||
                    (this.SelectionMode == DataGridViewSelectionMode.FullRowSelect &&
                    base.Rows[e.RowIndex].Selected == false))
                {
                    this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    base.Rows[e.RowIndex].Selected = true;
                }
            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
            }
        }

        private void LockVerticalScrollBarUpdate(bool lockUpdate/*, bool delayed*/)
        {
            // Temporarly hide/show the vertical scroll bar by changing its parent
            try
            {
                if (!this._inExpandCollapse)
                {
                    if (lockUpdate)
                    {
                        this.VerticalScrollBar.Parent = hideScrollBarControl;
                    }
                    else
                    {
                        this.VerticalScrollBar.Parent = this;
                    }
                }
            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
            }
        }

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            try
            {
                if (typeof(TreeGridColumn).IsAssignableFrom(e.Column.GetType()))
                {
                    if (_expandableColumn == null)
                    {
                        // identify the expanding column.			
                        _expandableColumn = (TreeGridColumn)e.Column;
                    }
                    else
                    {
                        // this.Columns.Remove(e.Column);
                        //throw new InvalidOperationException("Only one TreeGridColumn per TreeGridView is supported.");
                    }
                }

                // Expandable Grid doesn't support sorting. This is just a limitation of the sample.
                e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;

                base.OnColumnAdded(e);
            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
            }
        }

        private static class Win32Helper
        {
            public const int WM_SYSKEYDOWN = 0x0104,
                             WM_KEYDOWN = 0x0100,
                             WM_SETREDRAW = 0x000B;

            [System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern IntPtr SendMessage(System.Runtime.InteropServices.HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern IntPtr SendMessage(System.Runtime.InteropServices.HandleRef hWnd, int msg, int wParam, int lParam);

            [System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern bool PostMessage(System.Runtime.InteropServices.HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);

        }
        #endregion

        //***************************** ICTEAS [Debanjan Routh] *****************************************

        #region "                   Variables & Declarations                    "
        private int[] intDisableColumnNum, intReadOnlyColumnNum, intRightAlignColumnNum;
        public ArrayList arlNumericColCurrency, arlStringColumsStrLength, arlStringColumnCollection;
        public ArrayList arlNumericColumNum, arlNumericColumnType, arlNumericColIntSize, arlNumericColFloatSize;
        public ArrayList arlFixedLengthNumCols;
        private string strSQLProcName;
        private ArrayList arlOutParam;
        private ArrayList ProcParamNames = new ArrayList();
        private ArrayList ProcParamValues;
        private ArrayList ProcParamDir;
        private bool isCurrency = true;
        private bool boolIsSQL = true;
        private DataSet dsSource;
        private DataTable dtSource;
        public DataView dvSource;
        public int intTreeGridColumn = -1;
        //private string[] strLangID = new string[0];
        private int intColCount = 0;
        private string[] strColNames;
        private string[] strHeadings;
        //private string[] strLangIDs;
        private string[] strProcOutValue;
        private int intLoadRowCount = 0;
        private int intTotalRowCount = 0;
        public delegate void F2KeyHandler(object sender, F2EventArgs args);
        public event F2KeyHandler F2keypressed;
        private string[] strLangID = new string[0];

        public bool blStopEventPropagation = false;
        public double dblPrevVal = 0.0;
        public delegate void DataGridViewEditingControlTextChangedEventHandler(object sender, DataGridViewEditingControlTextChanged args);
        public event DataGridViewEditingControlTextChangedEventHandler EditingControlTextChanged;

        private bool blEnableCalculatedRowColor = false;

        private ArrayList arrRowIndexForPreferrentialColoring = new ArrayList();
        private ArrayList arrLevelsForPreferrentialColoring = new ArrayList();
        private ArrayList arrColumnsToCheckForColoring = new ArrayList();

        public bool insidetextchanged = false;
        private bool _bool = false;
        private int selectionStart = -1;
        private int noOfCommas = 0;
        private int selectionLength = 0;
        private DataGridViewTextBoxEditingControl objEditingcontrol;
        private bool blEnableF2Event;

        private int[] iPrimaryKeyColNums;
        private int[] iRefKeyColNums;
        private int iRankColNum = -1;
        private int[] iStoreColNums;
        private int iVisibleColumnCount = -1;
        private Font clrParentRowFont;
        private Font clrChildRowFont;
        private Color clrParentRowForeColor = Color.Black;
        private Color clrChildRowForeColor = Color.Black;
        private Color clrNormalRowForeColor = Color.Black;
        private Color clrLineColor = SystemColors.ControlDark;
        private DashStyle clrLineStyle = DashStyle.Dot;
        private ArrayList _Type1Columns = new ArrayList();
        private ArrayList _Type2Columns = new ArrayList();
        private Color clrParentRowBackColor = Color.FromArgb(192, 192, 192);
        private Color clrChildRowBackColor = Color.FromArgb(149, 207, 253);
        private bool _HeaderExtenderEnabled = false;
        private TreeGridViewHeaderExtender _TreeGridViewHeaderExtender = null;
        private ArrayList _ListHeaderExtenderMap = new ArrayList();
        private bool HeaderMapping = false;
        #endregion

        #region "                   Methods                 "

        /// <summary>
        /// Clears all data and columns from treegridview
        /// </summary>
        public void RefreshGrid()
        {
            this.Nodes.Clear();
            this.Columns.Clear();
            this.dtSource = null;
            this.dsSource = null;
        }

        /// <summary>
        /// Adds row indices adn their respective levels where calculated row coloring is to be done
        /// </summary>
        /// <param name="RowNums">Indices of the rows where calculated row coloring is to be done</param>
        /// <param name="Levels">Levels for each row</param>
        /// <param name="ColumnsToCheckForColoring">Column indices for which calculation is to be done</param>
        public void AddRowIndexForPreferrentialColoring(int[] RowNums, int[] Levels, int[] ColumnsToCheckForColoring)
        {
            if (arrRowIndexForPreferrentialColoring != null)
            {
                arrRowIndexForPreferrentialColoring.AddRange(RowNums);
            }
            if (arrLevelsForPreferrentialColoring != null)
            {
                arrLevelsForPreferrentialColoring.AddRange(Levels);
            }
            if (arrColumnsToCheckForColoring != null)
            {
                arrColumnsToCheckForColoring.AddRange(ColumnsToCheckForColoring);
            }
        }

        /// <summary>
        /// Checks whether the supplied level exists in arraylist of prefferential row color index
        /// </summary>
        /// <param name="iCheckLevel">Level to check</param>
        /// <returns>True if level exists; else false</returns>
        private bool DoCheckForValidLevel(int iCheckLevel)
        {
            bool blRetunVal = false;
            try
            {
                foreach (object objTmp in arrRowIndexForPreferrentialColoring)
                {
                    int[,] iTmpArr;
                    iTmpArr = (int[,])objTmp;
                    if (iCheckLevel == iTmpArr[0, 1])
                        return true;

                }
                return blRetunVal;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Calculates whether the row is to be colored or not
        /// </summary>
        /// <param name="RowIndex">Row Index for which calculation is to be done</param>
        /// <returns>True if row is to be colored;else false</returns>
        private bool IsTrueToColor(int RowIndex)
        {
            try
            {
                double dbl = 0.00;
                for (int i = 0; i < arrColumnsToCheckForColoring.Count; i++)
                {
                    dbl += Convert.ToDouble(this.Rows[RowIndex].Cells[Convert.ToInt32(arrColumnsToCheckForColoring[i])].Value);
                }
                if (dbl == 0.00)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Error:"+ex.Message.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sets column at supplied column number to a string column
        /// </summary>
        /// <param name="ColumnNumber">Index of the column</param>
        /// <param name="MaxLength">Maximum length of the string column</param>
        /// <returns>True if opertaion is successfull;else false</returns>
        public bool SetStringColumn(int ColumnNumber, int MaxLength)
        {
            try
            {
                arlStringColumnCollection.Add(ColumnNumber);
                arlStringColumsStrLength.Add(MaxLength);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets column at supplied column number to a numeric column
        /// </summary>
        /// <param name="ColumnNumber">Index of the column</param>
        ///  <param name="ColumnType">Type of the numeric column</param>
        ///  <param name="IntegerPrecision">Number of allowed digits before decimal separator</param>
        /// <param name="DecimalPrecision">Number of allowed digits after decimal separator</param>
        /// <returns>True if opertaion is successfull;else false</returns>
        public bool SetNumericColumn(int ColumnNumber, clsTxtBox.TypeEnum ColumnType, int IntegerPrecision, int DecimalPrecision)
        {
            try
            {
                arlNumericColumNum.Add(ColumnNumber);
                arlNumericColumnType.Add(ColumnType);
                arlNumericColIntSize.Add(IntegerPrecision);
                arlNumericColFloatSize.Add(DecimalPrecision);
                arlNumericColCurrency.Add(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets column at supplied column number to a numeric column(Overloaded)
        /// </summary>
        /// <param name="ColumnNumber">Index of the column</param>
        /// <param name="ColumnType">Type of the numeric column</param>
        /// <param name="IntegerPrecision">Number of allowed digits before decimal separator</param>
        /// <param name="DecimalPrecision">Number of allowed digits after decimal separator</param>
        /// <param name="isCurrency">True if currency;else false</param>
        /// <returns>True if opertaion is successfull;else false</returns>
        public bool SetNumericColumn(int ColumnNumber, clsTxtBox.TypeEnum ColumnType, int IntegerPrecision, int DecimalPrecision, bool isCurrency)
        {
            try
            {
                arlNumericColumNum.Add(ColumnNumber);
                arlNumericColumnType.Add(ColumnType);
                arlNumericColIntSize.Add(IntegerPrecision);
                arlNumericColFloatSize.Add(DecimalPrecision);
                arlNumericColCurrency.Add(isCurrency);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets column at supplied column number to a numeric column
        /// </summary>
        /// <param name="ColumnNumber">Index of the column</param>
        /// <param name="ColumnType">Type of the numeric column</param>
        /// <param name="IntegerPrecision">Number of allowed digits before decimal separator</param>
        /// <param name="DecimalPrecision">Number of allowed digits after decimal separator</param>
        /// <param name="IsFixedLength">True if fixed length;else false</param>
        /// <returns>True if opertaion is successfull;else false</returns>
        public bool SetNumericColumnFixed(int ColumnNumber, clsTxtBox.TypeEnum ColumnType, int IntegerPrecision, int DecimalPrecision, bool IsFixedLength)
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

        /// <summary>
        /// Set column to be right alligned
        /// </summary>
        /// <param name="ColumnNums">Index of the column</param>
        /// <returns>True if opertaion is successfull;else false</returns>
        public bool SetRightAlignColumns(params int[] ColumnNums)
        {
            try
            {
                intRightAlignColumnNum = ColumnNums;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Set column to be readonly
        /// </summary>
        /// <param name="ColumnNums">array of column numbers</param>
        /// <returns>True if opertaion is successfull;else false</returns>
        public bool SetReadOnlyColumns(params int[] ColumnNums)
        {
            try
            {
                intReadOnlyColumnNum = ColumnNums;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets paramter value for procedure
        /// </summary>
        /// <param name="ProcParamName">Parameter Name</param>
        /// <param name="ProcParamVal">Parameter Value</param>
        /// <param name="strIN_OUT">"IN" if the parameter is input parameter; "OUT" if parameter is out parameter</param>
        public void SetParam(string ProcParamName, string ProcParamVal, string strIN_OUT)
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

        /// <summary>
        /// Designs the TreeGridView based on the procedure names and parameters names/values supplied
        /// </summary>
        /// <param name="strErrMsg">Error message to be returned</param>
        /// <returns>True if opertaion is successfull;else false</returns>
        public bool DesignGrid(ref string strErrMsg)
        {
            try
            {

                this.ReadOnly = false;
                if (ExecuteProcedure(strSQLProcName, boolIsSQL, ref strErrMsg) == false)
                {
                    return false;
                }
                if (SetColumns(ref strErrMsg) == false)
                {
                    return false;
                }
                if (DesignTableStyle(ref strErrMsg) == false)
                {
                    return false;
                }
                if (!DoLoadData(ref strErrMsg))
                {
                    return false;
                }

                //this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                //for (int i = 0; i < this.Columns.Count; i++)
                //{
                //    if (this.Columns[i].Visible)
                //        this.InvalidateColumn(i);
                //}


                //CurrencyManager cm = (CurrencyManager)this.BindingContext[this.DataSource];
                //((DataView)cm.List).AllowNew = false;
                // this.CurrentCellChanged += new EventHandler(clsWritableGrid_CurrentCellChanged);
                //if (AllowPreferrentialReorderingOnLoad)
                //    DoPreferrentialReOrdering();
                //if (HeaderMapping)
                //{
                //    this.AssociatedHeaderExtender.Header.RepositionGridHeaders();
                //}
                //DoCreateFilterHeaders();
                return true;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Source + " - " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Column index of the treegrid column
        /// </summary>
        /// <param name="ColumnNum">Index of the column to be checked</param>
        /// <returns>True if supplied column index is treegridview;else false</returns>
        private bool CheckTreeGridColumn(int ColumnNum)
        {
            if (intTreeGridColumn == -1)
                return false;

            if (intTreeGridColumn == ColumnNum)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks whether the column at supplied index is readonly or not
        /// </summary>
        /// <param name="ColumnNum">Index of the column to be checked</param>
        /// <returns>True if supplied column index is read only;else false</returns>
        private bool CheckReadOnlyColumn(int ColumnNum)
        {
            if (intReadOnlyColumnNum == null)
                return false;

            for (int i = 0; i < intReadOnlyColumnNum.Length; i++)
                if (intReadOnlyColumnNum[i] == ColumnNum)
                    return true;

            return false;
        }

        /// <summary>
        /// Checks whether the column at supplied index is right alligned or not
        /// </summary>
        /// <param name="ColumnNum">Index of the column to be checked</param>
        /// <returns>True if supplied column index is right alligned;else false</returns>
        private bool CheckRightAlignColumn(int ColumnNum)
        {
            if (intRightAlignColumnNum == null)
                return false;

            for (int i = 0; i < intRightAlignColumnNum.Length; i++)
                if (intRightAlignColumnNum[i] == ColumnNum)
                    return true;

            return false;
        }

        /// <summary>
        /// Constructs all the columns for the treegridview
        /// </summary>
        /// <param name="strErrMsg">Error message to be returned</param>
        /// <returns>True if opertaion is successfull;else false</returns>
        private bool DesignTableStyle(ref string strErrMsg)
        {
            try
            {
                string m_strMaskCols = "", m_strShowCols = "";
                this.AutoGenerateColumns = false;

                //this.DataSource = dtSource;
                this.EditingControlShowing -= new DataGridViewEditingControlShowingEventHandler(TreeGridView_EditingControlShowing);
                //this.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(clsSearchGrid_EditingControlShowing);
                this.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(TreeGridView_EditingControlShowing);
                if (this.Columns.Count > 0)
                    this.Columns.Clear();
                iVisibleColumnCount = 0;
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    int local0 = 0;
                    if (strColNames[i].ToUpper().IndexOf("MASK") == 0)
                        continue;
                    iVisibleColumnCount++;
                    if (TreeColumnIndex == i)
                    {

                        TreeGridColumn objTreeCol = new TreeGridColumn();
                        objTreeCol.HeaderText = strHeadings[i];
                        objTreeCol.DataPropertyName = dtSource.Columns[i].ColumnName;
                        if (CheckReadOnlyColumn(i))
                        {
                            objTreeCol.ReadOnly = true;
                        }

                        this.Columns.Insert(this.Columns.Count, objTreeCol);
                    }
                    else
                    {
                        DataGridViewTextBoxColumn dgTBCol = new DataGridViewTextBoxColumn();
                        dgTBCol.DataPropertyName = dtSource.Columns[i].ColumnName;
                        dgTBCol.HeaderText = strHeadings[i];
                        if (strColNames[i].ToUpper().IndexOf("MASK") == 0)
                            continue;
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
                            dgTBCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.NotSet;
                            //dgTBCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            //dgTBCol.DataGridView.AlternatingRowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        if (CheckNumericColumn(i, ref local0))
                        {
                            dgTBCol.DefaultCellStyle.Format = "N" + arlNumericColFloatSize[local0];
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

                        this.Columns.Insert(this.Columns.Count, dgTBCol);
                    }
                }


                this.Font = new Font(this.Font, FontStyle.Regular);
                this.Invalidate();
                this.Refresh();


                return true;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Source + " - " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Returns the number of occurences of the separator char in the supplied string
        /// </summary>
        /// <param name="input">string to be searched</param>
        /// <param name="seperator">char to be searched</param>
        /// <returns>Number of occurences</returns>
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

        /// <summary>
        /// Processes a string input and returns it in comma separated format
        /// </summary>
        /// <param name="strParamTextinComma"></param>
        /// <param name="columnindex"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public string strInCommaFormat(string strParamTextinComma, int columnindex, ref int start)
        {
            string strstoretext = "";
            try
            {
                int realindex = columnindex;
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



                        val = Math.Round(val, DecimalPrecision);
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
                int position = objEditingcontrol.SelectionStart;
                if (noofcommas == 0)
                {
                    start = position;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        private bool SetColumns(ref string strErrMsg)
        {
            try
            {
                strColNames = new string[dtSource.Columns.Count];
                strHeadings = new string[dtSource.Columns.Count];

                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    strColNames[i] = dtSource.Columns[i].ColumnName;
                    strHeadings[i] = strColNames[i].Replace("_", " ") + " ";
                }
                ChangeLanguage();
                return true;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Source + " - " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Converts the column header texts for language conversion
        /// </summary>
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

        /// <summary>
        /// Executes a procedure for populating the treegridview
        /// </summary>
        /// <param name="SQLProcName">Procedure Name/SQL query string</param>
        /// <param name="isSQL">true if the supplied 'SQLProcName' if SQL query;else false</param>
        /// <param name="strErrMsg">Error message to be returned</param>
        /// <returns></returns>
        private bool ExecuteProcedure(string SQLProcName, bool isSQL, ref string strErrMsg)
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

                //dtSource.Columns.Add("MASK_LOAD_COL_VALUE");

                //for (int i = 0; i < dtSource.Rows.Count; i++)
                //    dtSource.Rows[i]["MASK_LOAD_COL_VALUE"] = "0";

                intColCount = dtSource.Columns.Count - 1;

                //MyGridTextBoxColumn._RowCount = dtSource.Rows.Count;

                ChangeDataTable();

                return true;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message + " - " + ex.Source;
                return false;
            }
        }

        /// <summary>
        /// Checks whether the column at supplied index is numeric or not
        /// </summary>
        /// <param name="ColumnNum">Index of the column to be checked</param>
        /// <param name="ColPosition">Actual column index;returns -1 if column at supplied index is not numeric</param>
        /// <returns>True if column at supplied index is numeric ;else false</returns>
        public bool CheckNumericColumn(int ColumnNum, ref int ColPosition)
        {
            for (int i = 0; i < arlNumericColumNum.Count; i++)
            {
                if (arlNumericColumNum[i].ToString() == ColumnNum.ToString())
                {
                    ColPosition = i;
                    return true;
                }
            }
            ColPosition = -1;
            return false;
        }

        /// <summary>
        /// Changes all the values at numeric column indices into comma-separated value if the datasource/datable
        /// </summary>
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

                                        DecimalPart = rep.Substring(dotIndex + 1);
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

        /// <summary>
        /// Adds index of column into Type I column list and makes the column readonly. 
        /// </summary>
        /// <param name="ColNum">Index of the column to be added</param>
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

        /// <summary>
        /// Adds index of column into Type 2 column list and makes the column readonly. 
        /// </summary>
        /// <param name="ColNum">Index of the column to be added</param>
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

        /// <summary>
        /// Adds indices of columns into Type I column list and makes the columns readonly. 
        /// </summary>
        /// <param name="ColNum">Indices of the columns to be added</param>
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

        /// <summary>
        /// Adds indices of columns into Type 2 column list and makes the columns readonly. 
        /// </summary>
        /// <param name="ColNum">Indices of the columns to be added</param>
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

        /// <summary>
        /// Clears the list of previuosly added columns in Tpe 1 column list
        /// </summary>
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

        /// <summary>
        /// Clears the list of previuosly added columns in Tpe 2 column list
        /// </summary>
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

        /// <summary>
        /// Loads data into treegridview 
        /// </summary>
        /// <param name="strErrMsg">Error message to be returned</param>
        /// <returns>True if data loading is successfull;else false</returns>
        private bool DoLoadData(ref string strErrMsg)
        {
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                if (dtSource.Rows[i][iRankColNum] == null)
                {
                    strErrMsg = "Error: All rows must have 'Rank' values. Row at position " + i.ToString() + " has missing 'Rank' Value.";
                    return false;
                }

                if (dtSource.Rows[i][iRankColNum].ToString() != "1")
                    continue;

                object[] objRowVals = new object[iVisibleColumnCount];
                int iCol = 0;
                foreach (DataColumn objDataCol in dtSource.Columns)
                {
                    if (objDataCol.ColumnName.ToUpper().StartsWith("MASK_"))
                        continue;
                    objRowVals[iCol] = dtSource.Rows[i][objDataCol.ColumnName.ToString()];
                    iCol++;
                }


                object[] objPrimaryKeyVals = new object[iPrimaryKeyColNums.Length];
                for (int j = 0; j < objPrimaryKeyVals.Length; j++)
                {
                    objPrimaryKeyVals[j] = dtSource.Rows[i][iPrimaryKeyColNums[j]];
                }

                Hashtable objNodeHash = new Hashtable();
                if (iStoreColNums != null)
                {
                    for (int j = 0; j < iStoreColNums.Length; j++)
                        objNodeHash.Add(dtSource.Columns[iStoreColNums[j]].ColumnName, dtSource.Rows[i][iStoreColNums[j]]);
                }


                TreeGridNode objRootNode = this.Nodes.Add(objPrimaryKeyVals, objNodeHash, objRowVals);
                objRootNode.Image = ICTEAS.WinForms.Properties.Resources.master;

                DoLoadChildRecords(ref strErrMsg, ref objRootNode, Convert.ToInt32(dtSource.Rows[i][iRankColNum]), objPrimaryKeyVals);
                if (objRootNode.HasChildren)
                {
                    objRootNode.DefaultCellStyle.ForeColor = objRootNode.DefaultCellStyle.SelectionForeColor = clrParentRowForeColor;
                }
                //else
                //{
                //    objRootNode.DefaultCellStyle.ForeColor = objRootNode.DefaultCellStyle.SelectionForeColor = clrNormalRowForeColor;
                //}
            }

            return true;
        }

        /// <summary>
        /// Creates child data nodes for the supplied tree node
        /// </summary>
        /// <param name="strErrMsg">Error Message to be returned</param>
        /// <param name="objParentNode">Node to which child nodes are to be added</param>
        /// <param name="ParentRank">Rank/Level of the parent node</param>
        /// <param name="PrimaryKeyVals">params of primary key values</param>
        /// <returns>True if data loading is successfull;else false</returns>
        private bool DoLoadChildRecords(ref string strErrMsg, ref TreeGridNode objParentNode, int ParentRank, params object[] PrimaryKeyVals)
        {
            try
            {
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    if (dtSource.Rows[i][iRankColNum] == null)
                    {
                        strErrMsg = "Error: All rows must have 'Rank' values. Row at position " + i.ToString() + " has missing 'Rank' Value.";
                        return false;
                    }

                    if (Convert.ToInt32(dtSource.Rows[i][iRankColNum]) <= ParentRank)
                        continue;

                    object[] objRefColVals = new object[iRefKeyColNums.Length];
                    for (int j = 0; j < objRefColVals.Length; j++)
                    {
                        objRefColVals[j] = dtSource.Rows[i][iRefKeyColNums[j]];
                    }
                    bool blChildRecord = true;
                    for (int j = 0; j < objRefColVals.Length; j++)
                    {
                        if (!PrimaryKeyVals[j].Equals(objRefColVals[j]))
                        {
                            blChildRecord = false;
                            break;
                        }
                    }
                    if (!blChildRecord)
                        continue;

                    object[] objRowVals = new object[iVisibleColumnCount];
                    int iCol = 0;
                    foreach (DataColumn objDataCol in dtSource.Columns)
                    {
                        if (objDataCol.ColumnName.ToUpper().StartsWith("MASK_"))
                            continue;
                        objRowVals[iCol] = dtSource.Rows[i][objDataCol.ColumnName.ToString()];
                        iCol++;
                    }


                    object[] objPrimaryKeyVals = new object[iPrimaryKeyColNums.Length];
                    for (int j = 0; j < objPrimaryKeyVals.Length; j++)
                    {
                        objPrimaryKeyVals[j] = dtSource.Rows[i][iPrimaryKeyColNums[j]];
                    }

                    Hashtable objNodeHash = new Hashtable();
                    if (iStoreColNums != null)
                    {
                        for (int j = 0; j < iStoreColNums.Length; j++)
                            objNodeHash.Add(dtSource.Columns[iStoreColNums[j]].ColumnName, dtSource.Rows[i][iStoreColNums[j]]);
                    }

                    TreeGridNode objTempNode = objParentNode.Nodes.Add(objPrimaryKeyVals, objNodeHash, objRowVals);
                    objTempNode.Image = ICTEAS.WinForms.Properties.Resources.detail;
                    objTempNode.DefaultCellStyle.ForeColor = objTempNode.DefaultCellStyle.SelectionForeColor = clrChildRowForeColor;
                    DoLoadChildRecords(ref strErrMsg, ref objTempNode, Convert.ToInt32(dtSource.Rows[i][iRankColNum]), objPrimaryKeyVals);

                }
                return true;
            }
            catch (Exception ex)
            {
                strErrMsg = "Error: " + ex.Message.ToString();
                return false;
            }
        }
        
        /// <summary>
        /// Finds out the row number by searching the treegridview rows based on the array of primary key values
        /// </summary>
        /// <param name="objPrimaryKeyVals">primry key values for which row is to be searched</param>
        /// <returns>Row index of the found row;-1 if row is not found</returns>
        private int GetRowNum(object[] objPrimaryKeyVals)
        {
            int iRow = 0;
            foreach (DataRow objRow in dtSource.Rows)
            {
                bool blResult = true;
                for (int i = 0; i < objPrimaryKeyVals.Length; i++)
                {
                    if (!objRow[PrimaryKeyColNums[i]].Equals(objPrimaryKeyVals[i]))
                    {
                        iRow++;
                        blResult = false;
                        continue;
                    }
                }

                if (blResult)
                {
                    return iRow;
                }
                iRow++;
            }
            return -1;
        }

        /// <summary>
        /// Returns the un-real column(index including the mask column)
        /// </summary>
        /// <param name="index">Index of the column for which index is to be found</param>
        /// <returns>un real column index(index including the mask column)</returns>
        public int get_unreal_colum_index(int index)
        {
            try
            {
                DataTable dt = (DataTable)this.dtSource;
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

        /// <summary>
        /// Returns the real column index(index excluding the mask column)
        /// </summary>
        /// <param name="index">Index of the column for which index is to be found</param>
        /// <returns>real column index(index excluding the mask column)</returns>
        public int get_actual_colum_index(int index)
        {
            try
            {
                DataTable dt = (DataTable)this.dtSource;
                int exactcolumn = 0;
                int store = 0; ;
                for (int i = 0; i < dt.Columns.Count; i++)
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

        /// <summary>
        /// Sets and designs the header to column header mapping
        /// </summary>
        /// <param name="_List"></param>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <param name="BorderColumns"></param>
        /// <param name="width"></param>
        public void SetHeaderToColumnsMapping(ArrayList _List, ArrayList type1, ArrayList type2, ArrayList BorderColumns, int width)
        {
            HeaderMapping = true;
            if (_ListHeaderExtenderMap.Count > 0)
                _ListHeaderExtenderMap.Clear();
            _ListHeaderExtenderMap = _List;
            //_WritableHeaderExtender.Header.CreateGridHeader();
            TreeGridViewHeader obj = _TreeGridViewHeaderExtender.Header;
            obj.DataGridView = this;
            obj.Type1Columns = type1;
            obj.Type2Columns = type2;
            obj.BorderedColumns = BorderColumns;
            obj.DesignGridHeaders(width);
        }

        /// <summary>
        /// Returns an arraylist containing header mapping
        /// </summary>
        /// <returns></returns>
        public ArrayList GetHeaderToColumnsMapping()
        {
            return _ListHeaderExtenderMap;
        }
        #endregion

        #region "                   Public Properties                   "
        /// <summary>
        /// Gets or sets a value indicating whether calculated row color is enabled or not
        /// </summary>
        public bool DoEnableCalculatedRowColor
        {
            get { return blEnableCalculatedRowColor; }
            set { blEnableCalculatedRowColor = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether F2 key press(Hot Search functionality) is enabled or not
        /// </summary>
        public bool EnableHotSearch
        {
            get { return blEnableF2Event; }
            set { blEnableF2Event = true; }
        }

        /// <summary>
        /// Gets or sets the column index of TreeGridColumn
        /// </summary>
        public int TreeColumnIndex
        {
            get { return intTreeGridColumn; }
            set { intTreeGridColumn = value; }
        }
        
        /// <summary>
        /// Gets or sets a fore color of normal rows
        /// </summary>
        public Color NormalRowForeColor
        {
            get { return clrNormalRowForeColor; }
            set { clrNormalRowForeColor = value; Invalidate(); }
        }

        /// <summary>
        /// Gets or sets a fore color of parent rows
        /// </summary>
        public Color ParentRowForeColor
        {
            get { return clrParentRowForeColor; }
            set { clrParentRowForeColor = value; Invalidate(); }
        }

        public Font ParentRowFont
        {
            get { return clrParentRowFont; }
            set { clrParentRowFont = value; Invalidate(); }
        }

        public Font ChildRowFont
        {
            get { return clrChildRowFont; }
            set { clrChildRowFont = value; Invalidate(); }
        }

        /// <summary>
        /// Gets or sets a fore color of child rows
        /// </summary>
        public Color ChildRowForeColor
        {
            get { return clrChildRowForeColor; }
            set { clrChildRowForeColor = value; Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the name of the stored procedure used to populate the treegridview
        /// </summary>
        public string StoredProcedureName
        {
            get { return strSQLProcName; }
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
        /// Gets or sets the string array of Language IDs for header texts of columns
        /// </summary>
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
        /// Gets or sets primary key column indices
        /// </summary>
        public int[] PrimaryKeyColNums
        {
            get { return iPrimaryKeyColNums; }
            set { iPrimaryKeyColNums = value; }
        }

        /// <summary>
        /// Gets or sets referenced key column indices
        /// </summary>
        public int[] RefKeyColNums
        {
            get { return iRefKeyColNums; }
            set { iRefKeyColNums = value; }
        }

        /// <summary>
        /// Gets or sets rank column indicex
        /// </summary>
        public int RankColNum
        {
            get { return iRankColNum; }
            set { iRankColNum = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int[] StoreColNums
        {
            get { return iStoreColNums; }
            set { iStoreColNums = value; }
        }

        /// <summary>
        /// Gets or sets the line color of tree hierarchy
        /// </summary>
        public Color LineColor
        {
            get { return clrLineColor; }
            set { clrLineColor = value; Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the line style of tree hierarchy
        /// </summary>
        public DashStyle LineStyle
        {
            get { return clrLineStyle; }
            set { clrLineStyle = value; Invalidate(); }
        }

        /// <summary>
        /// Gets or sets a back color of parent rows
        /// </summary>
        public Color ParentRowBackColor
        {
            get { return clrParentRowBackColor; }
            set { clrParentRowBackColor = value; Invalidate(); }
        }

        /// <summary>
        /// Gets or sets a back color of child rows
        /// </summary>
        public Color ChildRowBackColor
        {
            get { return clrChildRowBackColor; }
            set { clrChildRowBackColor = value; Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the inidces of the TYPE I columns
        /// </summary>
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

        /// <summary>
        /// Gets or sets the inidces of the TYPE 2 columns
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether Header Extender(multiple column header) is enabled or not
        /// </summary>
        public bool HeaderExtenderEnabled
        {
            get { return _HeaderExtenderEnabled; }
            set { _HeaderExtenderEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the associated TreeGridViewHeaderExtender control for this treegridview control
        /// </summary>
        public TreeGridViewHeaderExtender AssociatedHeaderExtender
        {
            get { return _TreeGridViewHeaderExtender; }
            set { _TreeGridViewHeaderExtender = value; }
        }


        #endregion

        #region "                   Events                  "
        /// <summary>
        /// This event handles the MS BUG of CELL color turing black on entry
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
        {
            //THIS IS ADDED TO AVOID THE BLACKENING OF THE CELLS ON ENTER...DEBANJAN
            e.CellStyle.BackColor = Color.White;
            base.OnEditingControlShowing(e);
        }

        /// <summary>
        /// Handles all custom row color and fore color functionality
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowPrePaint(DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                bool blIsParent = this.GetNodeForRow(this.Rows[e.RowIndex]).HasChildren;
                if (blIsParent)
                {
                    Rectangle rowBounds = new Rectangle(
                               this.RowHeadersWidth, e.RowBounds.Top,
                               this.Columns.GetColumnsWidth(
                                   DataGridViewElementStates.Visible) -
                               this.HorizontalScrollingOffset + 1,
                               e.RowBounds.Height);

                    // Paint the custom selection background.
                    using (Brush backbrush =
                        new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
                            ParentRowBackColor,
                            ParentRowBackColor, LinearGradientMode.Horizontal
                             ))
                    {
                        e.Graphics.FillRectangle(backbrush, rowBounds);
                    }
                    this.GetNodeForRow(this.Rows[e.RowIndex]).DefaultCellStyle.ForeColor = ParentRowForeColor;// Color.Red;
                    this.GetNodeForRow(this.Rows[e.RowIndex]).DefaultCellStyle.Font = ParentRowFont;// new Font(this.Font.Name.ToString(), this.Font.Size, FontStyle.Regular);
                }
                else
                {
                    this.GetNodeForRow(this.Rows[e.RowIndex]).DefaultCellStyle.ForeColor = NormalRowForeColor;
                    this.GetNodeForRow(this.Rows[e.RowIndex]).DefaultCellStyle.Font = ChildRowFont;


                    if (this.GetNodeForRow(this.Rows[e.RowIndex]).Index % 2 == 0)
                    {
                        Rectangle rowBounds = new Rectangle(
                               this.RowHeadersWidth, e.RowBounds.Top,
                               this.Columns.GetColumnsWidth(
                                   DataGridViewElementStates.Visible) -
                               this.HorizontalScrollingOffset + 1,
                               e.RowBounds.Height);

                        // Paint the custom selection background.
                        if (this.GetNodeForRow(this.Rows[e.RowIndex]).Selected)
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
                        else
                        {
                            using (Brush backbrush =
                                new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
                                    NormalRowColorStart,
                                    NormalRowColorEnd,
                                    CstmRowsColorGradientMode))
                            {
                                e.Graphics.FillRectangle(backbrush, rowBounds);
                            }
                            return;
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
                        if (this.GetNodeForRow(this.Rows[e.RowIndex]).Selected)
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
                        else
                        {
                            using (Brush backbrush =
                                new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
                                //Color.FromArgb(203, 225, 252),
                                    AlternateRowColorStart,
                                    AlternateRowColorEnd,
                                    CstmRowsColorGradientMode))
                            {
                                e.Graphics.FillRectangle(backbrush, rowBounds);
                            }
                            return;
                        }
                    }
                }

            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
            }
            base.OnRowPrePaint(e);
        }

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

        /// <summary>
        /// Fires off the 'EditingControlTextChanged' event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ////private void objEditingcontrol_TextChanged(object sender, EventArgs e)
        ////{
        ////    try
        ////    {                
        ////        this.GetNodeForRow(this.CurrentCell.RowIndex).NodeEditingMode = NodeEditMode.Updated;
        ////        insidetextchanged = true;
        ////        string s = objEditingcontrol.Text;
        ////        if (selectionStart == -1)
        ////        {
        ////            selectionStart = objEditingcontrol.SelectionStart;
        ////            selectionLength = objEditingcontrol.SelectionLength;
        ////        }

        ////        //this.CommitEdit(DataGridViewDataErrorContexts.Commit| DataGridViewDataErrorContexts.CurrentCellChange);
        ////        //this.dvSource.Table.AcceptChanges();
        ////        //this.Update();
        ////        //this.Refresh();

        ////        int col = this.CurrentCell.ColumnIndex;
        ////        col = this.get_actual_colum_index(col);
        ////        //int start = 0;
        ////        int start = s.Length;
        ////        int test = 0;
        ////        //int m_intCheckLength = 0;

        ////        if (CheckNumericColumn(col, ref test))
        ////        {
        ////            //entrycount++;
        ////            //**
        ////            string op = this.strInCommaFormat(s, col, ref start);
        ////            //string[] arr = op.Split(',');
        ////            string[] arr = op.Split(Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator));
        ////            int i = arr.Length - 1;

        ////            //string[] arr2 = editingcontrol.Text.Split(',');
        ////            string[] arr2 = objEditingcontrol.Text.Split(Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator));
        ////            int j = arr2.Length - 1;
        ////            if (i != j)
        ////                noOfCommas = 1;

        ////            //* /**
        ////            //this.EditingControl.Text = op;
        ////            //decimal s1 = Convert.ToDecimal(s);
        ////            this.objEditingcontrol.Text = op.ToString();//****************
        ////            //this.CurrentCell.Value = s;
        ////            //this.editingcontrol.EditingControlFormattedValue = s1;
        ////            // this.editingcontrol.EditingControlDataGridView.CurrentCell.Value = s1;
        ////            //this.CurrentCell.Value = s1;
        ////            // this.editingcontrol.SelectionStart = s.Length;
        ////        }
        ////        else
        ////        {
        ////            //entrycount++;
        ////            this.objEditingcontrol.Text = s;
        ////            this.CurrentCell.Value = s;

        ////            //  this.editingcontrol.SelectionStart = start;
        ////        }
        ////        //}


        ////        insidetextchanged = false;


        ////        DataGridViewEditingControlTextChanged _textChanged = new DataGridViewEditingControlTextChanged(this.CurrentCell.RowIndex, this.CurrentCell.ColumnIndex, this.CurrentCell.EditedFormattedValue.ToString());// this.CurrentCell.Value.ToString());
        ////        if (EditingControlTextChanged != null)
        ////        {
        ////            EditingControlTextChanged(null, _textChanged);

        ////        }

        ////    }
        ////    catch
        ////    {
        ////        MessageBox.Show("error");
        ////    }
        ////}
        private void objEditingcontrol_TextChanged(object sender, EventArgs e)
        {
            objEditingcontrol.TextChanged -= new EventHandler(objEditingcontrol_TextChanged);
            try
            {
                this.GetNodeForRow(this.CurrentCell.RowIndex).NodeEditingMode = NodeEditMode.Updated;
                insidetextchanged = true;
                string s = objEditingcontrol.Text;
                if (selectionStart == -1)
                {
                    selectionStart = objEditingcontrol.SelectionStart;
                    selectionLength = objEditingcontrol.SelectionLength;
                }

                //this.CommitEdit(DataGridViewDataErrorContexts.Commit| DataGridViewDataErrorContexts.CurrentCellChange);
                //this.dvSource.Table.AcceptChanges();
                //this.Update();
                //this.Refresh();
                DataGridViewEditingControlTextChanged _textChanged = new DataGridViewEditingControlTextChanged(this.CurrentCell.RowIndex, this.CurrentCell.ColumnIndex, this.CurrentCell.EditedFormattedValue.ToString());// this.CurrentCell.Value.ToString());
                if (EditingControlTextChanged != null)
                {
                    EditingControlTextChanged(this, _textChanged);

                }
                if (!blStopEventPropagation)
                {
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
                        string[] arr2 = objEditingcontrol.Text.Split(Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator));
                        int j = arr2.Length - 1;
                        if (i != j)
                            noOfCommas = 1;

                        //* /**
                        //this.EditingControl.Text = op;
                        //decimal s1 = Convert.ToDecimal(s);
                        this.objEditingcontrol.Text = op.ToString();//****************
                        //this.CurrentCell.Value = s;
                        //this.editingcontrol.EditingControlFormattedValue = s1;
                        // this.editingcontrol.EditingControlDataGridView.CurrentCell.Value = s1;
                        //this.CurrentCell.Value = s1;
                        // this.editingcontrol.SelectionStart = s.Length;
                    }
                    else
                    {
                        //entrycount++;
                        this.objEditingcontrol.Text = s;
                        this.CurrentCell.Value = s;

                        //  this.editingcontrol.SelectionStart = start;
                    }
                    //}


                    insidetextchanged = false;
                }
                else
                {
                    int col = this.CurrentCell.ColumnIndex;
                    col = this.get_actual_colum_index(col);
                    int start = dblPrevVal.ToString().Length;
                    int test = 0;
                    string op = this.strInCommaFormat(dblPrevVal.ToString(), col, ref start);
                    string[] arr = op.Split(Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator));
                    int i = arr.Length - 1;
                    string[] arr2 = objEditingcontrol.Text.Split(Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator));
                    int j = arr2.Length - 1;
                    if (i != j)
                        noOfCommas = 1;
                    this.objEditingcontrol.Text = op.ToString();


                    blStopEventPropagation = false;
                    //this.objEditingcontrol.Text = dblPrevVal.ToString();
                }

                //DataGridViewEditingControlTextChanged _textChanged = new DataGridViewEditingControlTextChanged(this.CurrentCell.RowIndex, this.CurrentCell.ColumnIndex, this.CurrentCell.EditedFormattedValue.ToString());// this.CurrentCell.Value.ToString());
                //if (EditingControlTextChanged != null)
                //{
                //    EditingControlTextChanged(null, _textChanged);

                //}

            }
            catch
            {
                MessageBox.Show("error");
            }
            finally
            {
                objEditingcontrol.TextChanged += new EventHandler(objEditingcontrol_TextChanged);
            }
        }
        /// <summary>
        /// Handles prefferential row colors, TYPE 1 & TYPE 2 row colors
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            
            try
            {
                base.OnCellPainting(e);
                if (arrRowIndexForPreferrentialColoring.Count > 0 && blEnableCalculatedRowColor && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    TreeGridNode objNd = this.GetNodeForRow(e.RowIndex);
                    if (arrLevelsForPreferrentialColoring.Contains(objNd.Level))
                    {
                        if (objNd.Level == 1)
                        {
                            int index = arrLevelsForPreferrentialColoring.IndexOf(objNd.Level);
                            int iTmp = e.RowIndex;// objNdParent.Nodes.IndexOf(objNd);

                            if (Convert.ToInt32(arrRowIndexForPreferrentialColoring[index]) == iTmp)
                            {
                                if (!IsTrueToColor(e.RowIndex) && e.ColumnIndex != -1)
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
                        else if (objNd.Level > 1)
                        {
                            int index = arrLevelsForPreferrentialColoring.IndexOf(objNd.Level);
                            TreeGridNode objNdParent = objNd.Parent;
                            if (objNdParent != null)
                            {
                                if (objNdParent.Index > -1)
                                {
                                    int iTmp = objNdParent.Nodes.IndexOf(objNd);

                                    if (Convert.ToInt32(arrRowIndexForPreferrentialColoring[index]) == iTmp)
                                    {
                                        if (!IsTrueToColor(e.RowIndex) && e.ColumnIndex != -1)
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

                            }
                        }
                    }

                }

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

                if (Type2Columns.Count > 0)
                {
                    if (Type2Columns.Contains(e.ColumnIndex))
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

                            Brush backbrush = new SolidBrush(Color.FromArgb(200, 215, 230));
                            Brush stringBrush = new SolidBrush(System.Drawing.Color.Black);
                            e.Graphics.FillRectangle(backbrush, CellBounds);


                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
            }

        }
        
        /// <summary>
        /// Handles 'EditingControlTextChanged' & 'EditingControlKeyDown' event firing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                Type type = e.Control.GetType();
                if (type.Name == "DataGridViewTextBoxEditingControl")
                {
                    objEditingcontrol = (DataGridViewTextBoxEditingControl)e.Control;
                    objEditingcontrol.TextChanged -= new EventHandler(objEditingcontrol_TextChanged);
                    objEditingcontrol.KeyDown -= new KeyEventHandler(objEditingcontrol_KeyDown);
                    objEditingcontrol.TextChanged += new EventHandler(objEditingcontrol_TextChanged);
                    objEditingcontrol.KeyDown += new KeyEventHandler(objEditingcontrol_KeyDown);
                    //objEditingcontrol.BackColor = Color.White;
                    //message
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Handles data error in the treegridview
        /// </summary>
        /// <param name="displayErrorDialogIfNoHandler"></param>
        /// <param name="e"></param>
        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            e.ThrowException = false;
            base.OnDataError(false, e);
            //base.OnDataError(displayErrorDialogIfNoHandler, e);
        }

        /// <summary>
        /// Fires off the 'F2keypressed' event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objEditingcontrol_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F2)
                {
                    if (!this.ReadOnly)
                    {
                        if (blEnableF2Event)
                        {
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

        /// <summary>
        /// Forcibly commits the changes in treegridcell & datagridviewcell changes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
        {
            //TreeGridNode objCurrentNode = this.GetNodeForRow(e.RowIndex);
            //int iChangingRowNum = GetRowNum(objCurrentNode.PrimaryKeyVal);
            //dtSource.Rows[iChangingRowNum].BeginEdit();
            //dtSource.Rows[iChangingRowNum].ItemArray[get_unreal_colum_index(e.ColumnIndex)] = this.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
            //dtSource.Rows[iChangingRowNum].ItemArray[dtSource.Columns.Count-1] = "2";
            //dtSource.Rows[iChangingRowNum].EndEdit();
            //dtSource.Rows[iChangingRowNum].AcceptChanges();
            try
            {
                base.OnCellEndEdit(e);
            }
            catch
            {
                MessageBox.Show("COMPONENT ERROR");
            }
        }
        #endregion

        //************************************        

    }
}


