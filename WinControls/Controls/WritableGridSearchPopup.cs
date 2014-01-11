using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using ICTEAS.WinForms.Utilities;

namespace ICTEAS.WinForms.Controls
{
    [ToolboxItem(false)]
    public class WritableGridSearchPopup : UserControl
    {
        #region "    Designer.cs     "

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
            this.pnlControl = new System.Windows.Forms.Panel();
            this.txtSearch = new ICTEAS.WinForms.Utilities.FxTextBox();
            this.pbSearch = new System.Windows.Forms.PictureBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlControl
            // 
            this.pnlControl.Controls.Add(this.txtSearch);
            this.pnlControl.Controls.Add(this.pbSearch);
            this.pnlControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControl.Location = new System.Drawing.Point(0, 0);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(182, 21);
            this.pnlControl.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtSearch.Location = new System.Drawing.Point(25, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(157, 20);
            this.txtSearch.TabIndex = 1;
            this.toolTip.SetToolTip(this.txtSearch, "Enter text to filter data");
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // pbSearch
            // 
            this.pbSearch.BackgroundImage = global::ICTEAS.WinForms.Properties.Resources.search50b;
            this.pbSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbSearch.Location = new System.Drawing.Point(0, 0);
            this.pbSearch.Name = "pbSearch";
            this.pbSearch.Size = new System.Drawing.Size(25, 21);
            this.pbSearch.TabIndex = 0;
            this.pbSearch.TabStop = false;
            // 
            // toolTip
            // 
            this.toolTip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(19)))), ((int)(((byte)(205)))));
            this.toolTip.IsBalloon = true;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "TurfView BI...Quick Find";
            // 
            // WritableGridSearchPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlControl);
            this.Name = "WritableGridSearchPopup";
            this.Size = new System.Drawing.Size(182, 21);
            this.Load += new System.EventHandler(this.ReadnSearchPopUp_Load);
            this.pnlControl.ResumeLayout(false);
            this.pnlControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSearch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlControl;
        private System.Windows.Forms.PictureBox pbSearch;
        public FxTextBox txtSearch;
        private ToolTip toolTip;

        #endregion

        #region "     Constructor     "
        public WritableGridSearchPopup(string HeaderText)
        {
            InitializeComponent();
            //lblHeader.Text = HeaderText;
             //Region = new Region(graphicsPath = CreateRoundRectangle(Width - 1, Height - 1, 6));
        }
        #endregion


        #region Rendering Events & Methods
        private static GraphicsPath CreateRoundRectangle(int w, int h, int r)
        {
            int d = r << 1;
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, d, d), 180, 90);
            path.AddLine(r, 0, w - r, 0);
            path.AddArc(new Rectangle(w - d, 0, d, d), 270, 90);
            path.AddLine(w + 1, r, w + 1, h - r);
            path.AddArc(new Rectangle(w - d, h - d, d, d), 0, 90);
            path.AddLine(w - r, h + 1, r, h + 1);
            path.AddArc(new Rectangle(0, h - d, d, d), 90, 90);
            path.AddLine(0, h - r, 0, r);
            path.CloseFigure();
            return path;
        }

        private GraphicsPath graphicsPath;

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                e.Graphics.TranslateTransform(-1, -1);
                using (Pen p = new Pen(Color.DarkGreen, 10))
                {
                    e.Graphics.DrawPath(p, graphicsPath);
                }
                e.Graphics.ResetTransform();
            }
            catch
            {
 
            }

        }
        #endregion

        private DataGridView _DgView;
        private int _CurrentColumnIndex = -1;
        private bool boolWildCardSearch =false;
        private DataTable dtFilterData;
        public void OnFilterRemoved(DataGridView parentGrid, int _ColumnIndex, int _RowIndex, Popup _popup)
        {
            _DgView = parentGrid;
            _CurrentColumnIndex = _ColumnIndex;
            _popup.OnFilterRemoved(_DgView);
        }

        public void ShowSearch(DataGridView parentGrid, int _ColumnIndex, int _RowIndex,Popup _popup)
        {
            _DgView = parentGrid;
            _CurrentColumnIndex = _ColumnIndex;
            Rectangle Rect = _DgView.GetCellDisplayRectangle(_ColumnIndex, _RowIndex, false);
            Rectangle RectToDiplay = new Rectangle(Rect.X, Rect.Y - this.ClientRectangle.Height, this.ClientRectangle.Width, this.ClientRectangle.Height);
            _popup.Show(_DgView, RectToDiplay);
        }

        private string _SearchText = "";

        public string SearchText
        {
            //get { return _SearchText; }
            get { return txtSearch.Text; }
            set { _SearchText = value; }
        }

        private string old;
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchText = txtSearch.Text;
            FilterData(SearchText);
            //string s = editingcontrol.Text;
            //if (s != old)
            //{
            //    this.FilterData(s);
            //    old = s;
            //}
        }

        #region Filter Data Backup
        //private void FilterData(string value)
        //{
        //    try
        //    {
        //        string m_strFilter = "";
        //        System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();



        //        dtFilterData = new DataTable();
        //        DataRow[] dr;
        //        //int index = _CurrentColumnIndex;

        //        clsWritableGrid _grid = (clsWritableGrid)_DgView;
        //        sBuilder = get_filter(_grid);
        //        m_strFilter = sBuilder.ToString();
        //        int index = get_actual_colum_index(_CurrentColumnIndex, _grid.dvSource.Table);
        //        //if (!_grid.HotSearchChild)
        //        dr = _grid.dvSource.Table.Select(m_strFilter, _grid.dvSource.Table.Columns[index].ColumnName, DataViewRowState.CurrentRows);
        //        //else
        //        //    dr = _grid.dtCurrentData.Select(m_strFilter, _grid.dtCurrentData.Columns[index].ColumnName, DataViewRowState.CurrentRows);
        //        //dr = ((DataTable)(_grid.DataSource)).Select(m_strFilter, _grid.dtFullData.Columns[index].ColumnName, DataViewRowState.CurrentRows);
        //        dtFilterData = _grid.dvSource.Table.Clone();
        //        dtFilterData.Rows.Clear();
        //        for (int i = 0; i < dr.Length; i++)
        //        {
        //            dtFilterData.ImportRow(dr[i]);
        //        }
        //        _DgView.DataSource = dtFilterData;
        //        _DgView.Tag = dtFilterData;
        //        CurrencyManager heidi = (CurrencyManager)_DgView.BindingContext[_DgView.DataSource];
        //        //heidi.Position = -1;
        //        heidi.Refresh();

        //        dtFilterData.Dispose();
        //        //filterpresent = true;
        //        dr = null;
        //        ((DataView)heidi.List).RowFilter = "";
        //        ((DataView)heidi.List).AllowNew = false;

        //        _grid.HandleFilterStateEvent(m_strFilter, dtFilterData.Rows.Count, txtSearch.Text == "" ? false : true);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        //    }
        //}
        #endregion Filter Data Backup

        private void FilterData(string value)
        {
            try
            {
                string m_strFilter = "";
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();



                dtFilterData = new DataTable();
                DataRow[] dr;
                //int index = _CurrentColumnIndex;

                clsWritableGrid _grid = (clsWritableGrid)_DgView;
                sBuilder = get_filter(_grid);
                m_strFilter = sBuilder.ToString();
                int index = get_actual_colum_index(_CurrentColumnIndex, _grid.dvSource.Table);
                //if (!_grid.HotSearchChild)
                //***********
                //Changes For Decimal Column Filtering
                //The Decimal datatype columns are converted into System.String type and then filtered.
                DataTable dtConverted = new DataTable();
                dtConverted = _grid.dvSource.Table.Clone();
                foreach (DataColumn col in dtConverted.Columns)
                {
                    if (col.DataType.FullName.ToString() == "System.Decimal")
                    {
                        col.DataType = Type.GetType("System.String");
                        //dtConverted.Columns.Add(col);
                    }
                    else
                    {
                        //dtConverted.Columns.Add(col);
                    }
                }

                foreach (DataRow dRow in _grid.dvSource.Table.Rows)
                {
                    dtConverted.ImportRow(dRow);
                }
                //**************

                dr = dtConverted.Select(m_strFilter, _grid.dvSource.Table.Columns[index].ColumnName, DataViewRowState.CurrentRows);

                //dr = _grid.dvSource.Table.Select(m_strFilter, _grid.dvSource.Table.Columns[index].ColumnName, DataViewRowState.CurrentRows);

                dtFilterData = _grid.dvSource.Table.Clone();
                dtFilterData.Rows.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    dtFilterData.ImportRow(dr[i]);
                }
                _DgView.DataSource = dtFilterData;
                _DgView.Tag = dtFilterData;
                CurrencyManager heidi = (CurrencyManager)_DgView.BindingContext[_DgView.DataSource];
                //heidi.Position = -1;
                heidi.Refresh();

                dtFilterData.Dispose();
                //filterpresent = true;
                dr = null;
                ((DataView)heidi.List).RowFilter = "";
                ((DataView)heidi.List).AllowNew = false;

                _grid.HandleFilterStateEvent(m_strFilter, dtFilterData.Rows.Count, txtSearch.Text == "" ? false : true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        #region Filter Data
        //private void FilterData(string value)
        //{
        //    try
        //    {
        //        string m_strFilter = "";
        //        System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();



        //        dtFilterData = new DataTable();
        //        DataRow[] dr;
        //        //int index = _CurrentColumnIndex;

        //        clsWritableGrid _grid = (clsWritableGrid)_DgView;
        //        sBuilder = get_filter(_grid);
        //        m_strFilter = sBuilder.ToString();
        //        int index = get_actual_colum_index(_CurrentColumnIndex, _grid.dvSource.Table);
        //        //if (!_grid.HotSearchChild)
        //        dr = _grid.FullData.Select(m_strFilter, _grid.dvSource.Table.Columns[index].ColumnName, DataViewRowState.CurrentRows);
        //        //else
        //        //    dr = _grid.dtCurrentData.Select(m_strFilter, _grid.dtCurrentData.Columns[index].ColumnName, DataViewRowState.CurrentRows);
        //        //dr = ((DataTable)(_grid.DataSource)).Select(m_strFilter, _grid.dtFullData.Columns[index].ColumnName, DataViewRowState.CurrentRows);
        //        dtFilterData = _grid.dvSource.Table.Clone();
        //        dtFilterData.Rows.Clear();
        //        for (int i = 0; i < dr.Length; i++)
        //        {
        //            dtFilterData.ImportRow(dr[i]);
        //        }
        //        _DgView.DataSource = dtFilterData;
        //        _DgView.Tag = dtFilterData;
        //        CurrencyManager heidi = (CurrencyManager)_DgView.BindingContext[_DgView.DataSource];
        //        //heidi.Position = -1;
        //        heidi.Refresh();

        //        dtFilterData.Dispose();
        //        //filterpresent = true;
        //        dr = null;
        //        ((DataView)heidi.List).RowFilter = "";
        //        ((DataView)heidi.List).AllowNew = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        //    }
        //}
        #endregion Filter Data

        public void RemoveFilter()
        {
            this.txtSearch.Text = "";
            txtSearch_TextChanged(null, null);

            SearchText = txtSearch.Text;
        }


        public StringBuilder get_filter(clsWritableGrid _grid)
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                DataTable dt = (DataTable)_DgView.DataSource;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToString().ToUpper().StartsWith("MASK_"))
                        continue;
                    string s = String.Empty;
                    WritableGridFilterHeader filter;
                    if (i == get_actual_colum_index(_CurrentColumnIndex, _grid.dvSource.Table))
                    {
                        s = txtSearch.Text;
                    }
                    else
                    {
                        filter = (WritableGridFilterHeader)_DgView.Columns[get_unreal_colum_index(i, _grid.dvSource.Table)].HeaderCell;

                        Type p = ((object)filter.SearchText).GetType();
                        if (p.ToString() == "System.String")
                        {
                            s = (string)filter.SearchText;
                        }
                    }

                    if (s == String.Empty)
                    {
                        continue;
                    }
                    else
                    {
                        if (!boolWildCardSearch)
                        {
                            sb.Append("(" + dt.Columns[i].ColumnName + " LIKE  '" + s + "%' ) AND ");
                        }
                        else
                        {
                            sb.Append("(" + dt.Columns[i].ColumnName + " LIKE  '%" + s + "%' ) AND ");
                            boolWildCardSearch = false;
                        }

                        //if (!boolWildCardSearch)
                        //{
                        //    sb.Append("(" + convert/( dt.Columns[i].ColumnName,"System.String") + " LIKE  '" + s + "%' ) AND ");
                        //}
                        //else
                        //{
                        //    sb.Append("(" + dt.Columns[i].ColumnName + " LIKE  '%" + s + "%' ) AND ");
                        //    boolWildCardSearch = false;
                        //}
                    }


                    //if (s == String.Empty)
                    //{
                    //    continue;
                    //}
                    //else
                    //{
                    //    if (!boolWildCardSearch)
                    //    {
                    //        sb.Append("(" + dt.Columns[i].ColumnName + " LIKE  '" + s + "%' ) AND ");
                    //    }
                    //    else
                    //    {
                    //        sb.Append("(" + dt.Columns[i].ColumnName + " LIKE  '%" + s + "%' ) AND ");
                    //        boolWildCardSearch = false;
                    //    }
                    //}
                }
                if (sb.ToString() != "")
                {
                    sb.Replace(" AND ", "", sb.Length - 5, 5);
                }
                return sb;
            }
            catch (Exception ex)
            {
                return null;
            }
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

        public int get_unreal_colum_index(int index, DataTable _dtFullData)
        {
            try
            {
                int exactcolumn = 0;
                int store = 0; ;
                for (int i = 0; i < _dtFullData.Columns.Count; i++)
                {
                    if (get_actual_colum_index(i,_dtFullData) == index)
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

        private void ReadnSearchPopUp_Load(object sender, EventArgs e)
        {

            if (txtSearch.Text == "")
                txtSearch.DrawTextPrompt(txtSearch.CreateGraphics());
        }


        public void DrawText()
        {
            if (txtSearch.Text == "")
                txtSearch.DrawTextPrompt(txtSearch.CreateGraphics());
        }
        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData == ((Control.ModifierKeys & Keys.Control) | Keys.Enter)) && (keyData != Keys.Enter))
            {
                boolWildCardSearch = true;
                FilterData(txtSearch.Text.ToString());
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
