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
    
    public partial class WritableGridToolBar : UserControl,ISupportInitialize
    {
        public WritableGridToolBar()
        {
            InitializeComponent();
        }
        private clsWritableGrid _grid;
        private int _initCounter = 0;
        /// <summary>
        /// Gets and sets the <see cref="DataGridView"/> instance to use.
        /// </summary>
        internal clsWritableGrid DataGridView
        {
            get { return _grid; }
            set
            {
                if (_grid != null)
                {
                    //_grid.DataSourceChanged -= new System.EventHandler(this.OnDataSourceChanged);
                    //_grid.DataMemberChanged -= new System.EventHandler(this.OnDataSourceChanged);
                    ///_grid.ColumnWidthChanged -= new DataGridViewColumnEventHandler(OnGridColumnsChanged);
                    //_grid.ColumnDisplayIndexChanged -= new DataGridViewColumnEventHandler(OnGridColumnsChanged);
                    //_grid.ColumnAdded -= new DataGridViewColumnEventHandler(OnGridColumnsAddedRemoved);
                    //_grid.ColumnRemoved -= new DataGridViewColumnEventHandler(OnGridColumnsAddedRemoved);
                    //_grid.ColumnStateChanged -= new DataGridViewColumnStateChangedEventHandler(OnGridColumnsStateChanged);
                    //_grid.Scroll -= new ScrollEventHandler(OnGridScroll);
                }

                _grid = value;

                if (_grid != null)
                {
                    //_grid.DataSourceChanged += new System.EventHandler(this.OnDataSourceChanged);
                    //_grid.DataMemberChanged += new System.EventHandler(this.OnDataSourceChanged);
                    //_grid.ColumnWidthChanged += new DataGridViewColumnEventHandler(OnGridColumnsChanged);
                    //_grid.ColumnDisplayIndexChanged += new DataGridViewColumnEventHandler(OnGridColumnsChanged);
                    //_grid.ColumnAdded += new DataGridViewColumnEventHandler(OnGridColumnsAddedRemoved);
                    //_grid.ColumnRemoved += new DataGridViewColumnEventHandler(OnGridColumnsAddedRemoved);
                    //_grid.ColumnStateChanged += new DataGridViewColumnStateChangedEventHandler(OnGridColumnsStateChanged);
                   // _grid.Scroll += new ScrollEventHandler(OnGridScroll);
                }
                //RecreateGridFilters();
            }
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

        private void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string str = e.ClickedItem.Name.ToString();

            switch (str)
            {
                case "tsBtnUp":
                    _grid.MoveUp();
                    _grid.EncryptOrderingInfoToDisk();
                    //FindParentForm((Control)_grid);
                    break;

                case "tsBtnDown":
                    _grid.MoveDown();
                    _grid.EncryptOrderingInfoToDisk();
                    //FindParentForm((Control)_grid);
                    break;

            }
        }

        private void FindParentForm(Control cntrl)
        {
            if (cntrl.Parent.GetType().BaseType.Name == "clsForm")
            {
                clsForm frm = (clsForm)cntrl.Parent;
                IBps IForm = (IBps)frm;
                ScreenGroup _group = ScreenGroup.None;
                PlanType _type = PlanType.None;
                string _year = "";
                string _distId = "";
                string _divn_Id = "";

                IForm.GetRecordSpec(ref _group, ref _type, ref _year, ref _distId, ref _divn_Id);
                //_grid.DoPreferrentialReordering(_group, _type, _year, _distId, _divn_Id);
                _grid.EncryptOrderingInfoToDisk();
            }
            else
            {
                FindParentForm((Control)cntrl.Parent);
            }
        }
    }
}
