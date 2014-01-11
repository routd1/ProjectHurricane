using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ICTEAS.WinForms.Controls
{
    public enum HeaderPosition
    {
        Top,
        Bottom
    };

    public partial class WritableHeaderExtender : Component, ISupportInitialize
    {
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
            components = new System.ComponentModel.Container();
        }

        #endregion
        #endregion

        #region "     Constructor     "
        public WritableHeaderExtender()
        {
            InitializeComponent();
            _Header = new WritableGridHeader();
        }
        #endregion

        #region "     Constructor Overload     "
        public WritableHeaderExtender(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            _Header = new WritableGridHeader();
        }
        #endregion

        #region "    Fields     "

        
        private clsWritableGrid _grid;
        private Control _currentParent;
        private WritableGridHeader _Header;
        private HeaderPosition _HeaderPosition = HeaderPosition.Top;
        private bool _autoAdjustGrid = false;
        private bool _initializing = false;

        #endregion

        #region "     Public Interfaces     "
        /// <summary>
        /// Gets and sets the poisiton of the filter GUI elements.
        /// </summary>
        [Browsable(true), DefaultValue(HeaderPosition.Top)]
        [Description("Gets and sets the position of the header GUI elements.")]
        public HeaderPosition GridHeaderPosition
        {
            get { return _HeaderPosition; }
            set
            {
                if (_HeaderPosition == value)
                    return;

                _HeaderPosition = value;
                AdjustHeaderControlToGrid();
            }
        }

        /// <summary>
        /// Gets and sets the text for the filter label.
        /// </summary>
        [Browsable(true), DefaultValue("Filter")]
        [Description("Gets and sets the text for the filter label.")]
        //public string HeaderMessageId
        //{
        //    get { return _Header.LabelHeaderMessageId; }
        //    set { _Header.LabelHeaderMessageId = value; }
        //}

        /// <summary>
        /// Gets and sets whether the filter label should be visible.
        /// </summary>
        //[Browsable(true), DefaultValue(true)]
        //[Description("Gets and sets whether the filter label should be visible.")]
        //public bool HeaderTextVisible
        //{
        //    get { return _Header.lblHeader.Visible; }
        //    set { _Header.lblHeader.Visible = value; }
        //}

        /// <summary>
        /// The bounds of the control with the GUI for filtering
        /// </summary>
        //[Browsable(false)]
        public Rectangle ControlBounds
        {
            get { return _Header == null ? Rectangle.Empty : _Header.Bounds; }
        }

        /// <summary>
        /// The Height of the control which is positioned for filtering
        /// </summary>
        [Browsable(false)]
        public int NeededControlHeight
        {
            get { return _Header == null ? 0 : _Header.Height; }
        }

        /// <summary>
        /// Gets and sets the grid which should be extended.
        /// </summary>
        [Browsable(true)]
        [Description("Gets and sets the grid which should be extended.")]
        public clsWritableGrid DataGridView
        {
            get { return _grid; }
            set
            {
                if (_grid == value)
                    return;

                RemoveHeaderControl();
                if (_grid != null)
                {
                    _grid.LocationChanged -= new EventHandler(OnGridLocationChanged);
                    _grid.Resize -= new EventHandler(OnGridResize);
                    _grid.ParentChanged -= new EventHandler(OnGridParentChanged);
                }

                _grid = value;

                _Header.DataGridView = _grid;

                AdjustHeaderControlToGrid();
                AddHeaderControl();

                if (_grid != null)
                {
                    _grid.LocationChanged += new EventHandler(OnGridLocationChanged);
                    _grid.Resize += new EventHandler(OnGridResize);
                    _grid.ParentChanged += new EventHandler(OnGridParentChanged);
                }
            }
        }


        public WritableGridHeader Header
        {
            get { return _Header; }
        }
        
        #endregion

        #region "     Privates    "

        private void AdjustGridPosition(HeaderPosition fromPosition, HeaderPosition toPosition)
        {
            if (_grid == null || _Header == null || fromPosition == toPosition)
                return;

            if (_initializing)
                return;

            int newTop = _grid.Top;
            int newHeight = _grid.Height;

            switch (fromPosition)
            {
                case HeaderPosition.Bottom:
                    newHeight += _Header.Height;
                    break;
                case HeaderPosition.Top:
                    newTop -= _Header.Height;
                    newHeight += _Header.Height;
                    break;
            }

            switch (toPosition)
            {
                case HeaderPosition.Bottom:
                    newHeight -= _Header.Height;
                    break;
                case HeaderPosition.Top:
                    newTop += _Header.Height;
                    newHeight -= _Header.Height;
                    break;
            }

            AnchorStyles oldStyle = _grid.Anchor;
            _grid.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            _grid.SetBounds(0, newTop, 0, newHeight, BoundsSpecified.Y | BoundsSpecified.Height);
            _grid.Anchor = oldStyle;
        }

        private void AddHeaderControl()
        {
            RemoveHeaderControl();

            if (_grid != null)
            {
                if (_grid.Parent != null)
                {
                    if (_currentParent != null)
                    {
                        _currentParent.BackColorChanged -= new EventHandler(OnColorsChanged);
                        _currentParent.ForeColorChanged -= new EventHandler(OnColorsChanged);
                    }

                    _currentParent = _grid.Parent;
                    _currentParent.BackColorChanged += new EventHandler(OnColorsChanged);
                    _currentParent.ForeColorChanged += new EventHandler(OnColorsChanged);
                    _grid.Parent.Controls.Add(_Header);
                    _Header.BringToFront();
                    //_filters.AfterFiltersChanged += new EventHandler(OnAfterFiltersChanged);
                    //_filters.BeforeFiltersChanging += new EventHandler(OnBeforeFiltersChanging);
                    //_filters.GridFilterBound += new GridFilterEventHandler(OnGridFilterBound);
                    //_filters.GridFilterUnbound += new GridFilterEventHandler(OnGridFilterUnbound);
                }

                AdjustHeaderControlToGrid();
            }
        }

        private void RemoveHeaderControl()
        {
            if (_currentParent != null)
            {
                //_filters.AfterFiltersChanged -= new EventHandler(OnAfterFiltersChanged);
                //_filters.BeforeFiltersChanging -= new EventHandler(OnBeforeFiltersChanging);
                //_filters.GridFilterBound -= new GridFilterEventHandler(OnGridFilterBound);
                //_filters.GridFilterUnbound -= new GridFilterEventHandler(OnGridFilterUnbound);
                _currentParent.Controls.Remove(_Header);
                _currentParent.BackColorChanged -= new EventHandler(OnColorsChanged);
                _currentParent.ForeColorChanged -= new EventHandler(OnColorsChanged);
                _currentParent = null;
            }
        }

        private void AdjustHeaderControlToGrid()
        {
            if (_grid == null || _Header == null || _grid.Parent == null)
                return;

            switch (_HeaderPosition)
            {
                case HeaderPosition.Top:
                    _Header.Top = _grid.Top - _Header.Height;
                    _Header.Left = _grid.Left+_grid.RowHeaderWidth;
                    _Header.Width = _grid.Width;
                    _Header.BackColor = _grid.Parent.BackColor;
                    _Header.ForeColor = _grid.Parent.ForeColor;
                    _Header.Visible = true;
                    break;
                case HeaderPosition.Bottom:
                    _Header.Top = _grid.Bottom + 1;
                    _Header.Left = _grid.Left + _grid.RowHeaderWidth;
                    _Header.Width = _grid.Width;
                    _Header.BackColor = _grid.Parent.BackColor;
                    _Header.ForeColor = _grid.Parent.ForeColor;
                    _Header.Visible = true;
                    break;
                default:
                    _Header.Visible = false;
                    break;
            }
        }

        #endregion

        #region "     Events     "

        private void OnGridLocationChanged(object sender, EventArgs e)
        {
            AdjustHeaderControlToGrid();
        }

        private void OnGridResize(object sender, EventArgs e)
        {
            AdjustHeaderControlToGrid();
        }

        private void OnGridParentChanged(object sender, EventArgs e)
        {
            AddHeaderControl();
        }

        private void OnColorsChanged(object sender, EventArgs e)
        {
            AdjustHeaderControlToGrid();
        }

        //private void OnAfterFiltersChanged(object sender, EventArgs e)
        //{
        //    OnAfterFiltersChanged(e);
        //}

        //private void OnBeforeFiltersChanging(object sender, EventArgs e)
        //{
        //    OnBeforeFiltersChanging(e);
        //}

        //private void OnGridFilterBound(object sender, GridFilterEventArgs e)
        //{
        //    OnGridFilterBound(e);
        //}

        //private void OnGridFilterUnbound(object sender, GridFilterEventArgs e)
        //{
        //    OnGridFilterUnbound(e);
        //}

        #endregion

        #region "     ISupportInitialize Members     "

        public void BeginInit()
        {
            _initializing = true;
            if (_Header != null)
                _Header.BeginInit();
        }

        public void EndInit()
        {
            _initializing = false;
            if (_Header != null)
                _Header.EndInit();
        }

        #endregion
    }
}
