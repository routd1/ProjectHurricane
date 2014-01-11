using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ICTEAS.WinForms.Controls
{
    public enum ToolBarPosition
    {
        Left,
        Right,
        Bottom
    };
    public class WritableGridToolBarExtender : Component ,ISupportInitialize
    {
        #region "     Designer.cs      "
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
        public WritableGridToolBarExtender()
        {
            InitializeComponent();
            _ToolBar = new WritableGridToolBar();
        }
        #endregion

        #region "     Constructor Overload     "
        public WritableGridToolBarExtender(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            _ToolBar = new WritableGridToolBar();
        }
        #endregion

        #region "    Fields     "


        private clsWritableGrid _grid;
        private Control _currentParent;
        private WritableGridToolBar _ToolBar;
        private ToolBarPosition _ToolBarPosition = ToolBarPosition.Bottom;
        private bool _autoAdjustGrid = false;
        private bool _initializing = false;

        #endregion

        #region "     Public Interfaces     "
        /// <summary>
        /// Gets and sets the poisiton of the filter GUI elements.
        /// </summary>
        [Browsable(true), DefaultValue(ToolBarPosition.Bottom)]
        [Description("Gets and sets the position of the header GUI elements.")]
        public ToolBarPosition GridToolBarPosition
        {
            get { return _ToolBarPosition; }
            set
            {
                if (_ToolBarPosition == value)
                    return;

                _ToolBarPosition = value;
                AdjustToolBarControlToGrid();
            }
        }


        /// <summary>
        /// The bounds of the control with the GUI for filtering
        /// </summary>
        [Browsable(false)]
        public Rectangle ControlBounds
        {
            get { return _ToolBar == null ? Rectangle.Empty : _ToolBar.Bounds; }
        }

        /// <summary>
        /// The Height of the control which is positioned for filtering
        /// </summary>
        [Browsable(false)]
        public int NeededControlHeight
        {
            get { return _ToolBar == null ? 0 : _ToolBar.Height; }
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

                _ToolBar.DataGridView = _grid;

                AdjustToolBarControlToGrid();
                AddHeaderControl();
                if (_grid != null)
                {
                    _grid.LocationChanged += new EventHandler(OnGridLocationChanged);
                    _grid.Resize += new EventHandler(OnGridResize);
                    _grid.ParentChanged += new EventHandler(OnGridParentChanged);
                }
            }
        }


        public WritableGridToolBar Header
        {
            get { return _ToolBar; }
        }


        private bool _IsVisible = true;

        /// <summary>
        /// Gets and sets the grid which should be extended.
        /// </summary>
        [Browsable(true)]
        [Description("Sets the toolbar to visible.")]
        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; _ToolBar.Visible = _IsVisible; _grid.Height = _IsVisible ? _grid.Height : _grid.Height + 24; }
        }

        #endregion

        #region "     Privates    "

        private void AdjustGridPosition(ToolBarPosition fromPosition, ToolBarPosition toPosition)
        {
            if (_grid == null || _ToolBar == null || fromPosition == toPosition)
                return;

            if (_initializing)
                return;

            int newTop = _grid.Top;
            int newHeight = _grid.Height;

            switch (fromPosition)
            {
                case ToolBarPosition.Left:
                    newHeight += _ToolBar.Height;
                    break;
                case ToolBarPosition.Right:
                    newTop -= _ToolBar.Height;
                    newHeight += _ToolBar.Height;
                    break;
            }

            switch (toPosition)
            {
                case ToolBarPosition.Left:
                    newHeight -= _ToolBar.Height;
                    break;
                case ToolBarPosition.Right:
                    newTop += _ToolBar.Height;
                    newHeight -= _ToolBar.Height;
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
                    _grid.Parent.Controls.Add(_ToolBar);
                    _ToolBar.BringToFront();
                    //_filters.AfterFiltersChanged += new EventHandler(OnAfterFiltersChanged);
                    //_filters.BeforeFiltersChanging += new EventHandler(OnBeforeFiltersChanging);
                    //_filters.GridFilterBound += new GridFilterEventHandler(OnGridFilterBound);
                    //_filters.GridFilterUnbound += new GridFilterEventHandler(OnGridFilterUnbound);
                }

                AdjustToolBarControlToGrid();
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
                _currentParent.Controls.Remove(_ToolBar);
                _currentParent.BackColorChanged -= new EventHandler(OnColorsChanged);
                _currentParent.ForeColorChanged -= new EventHandler(OnColorsChanged);
                _currentParent = null;
            }
        }

        private void AdjustToolBarControlToGrid()
        {
            if (_grid == null || _ToolBar == null || _grid.Parent == null)
                return;

            switch (_ToolBarPosition)
            {
                case ToolBarPosition.Left:
                    //_ToolBar.Top = _grid.Top - _ToolBar.Height;
                    //_ToolBar.Left = _grid.Left + _grid.RowHeaderWidth;
                    //_ToolBar.Width = _grid.Width;
                    //_ToolBar.BackColor = _grid.Parent.BackColor;
                    //_ToolBar.ForeColor = _grid.Parent.ForeColor;
                    //_ToolBar.Visible = true;
                    //_ToolBar.Width = 24;
                    //_ToolBar.Location = new Point(_grid.PointToScreen(_grid.Location).X - _ToolBar.Width, _grid.PointToClient(_grid.Location).Y);
                    ////_ToolBar.Location = new Point(_grid.Location.X - _ToolBar.Width, _grid.Location.Y);
                    //_ToolBar.Height = _grid.Height;

                    _ToolBar.Width = 24;
                    Point pt = _grid.Location;
                    _ToolBar.Location = pt;
                    bl = false;
                    _grid.Location = new Point(_grid.Location.X + _ToolBar.Width, _grid.Location.Y);
                    _ToolBar.Height = _grid.Height;
                    bl = true;
                    
                    break;
                case ToolBarPosition.Right:
                    //_grid.Width = _grid.Width - 25;
                    _ToolBar.Location =new Point( _grid.Location.X + _grid.Width,_grid.Location.Y);
                   
                    //_ToolBar.Location.X = _grid.Location.X + _grid.Width;
                    //_ToolBar.Location.Y = _grid.Location.Y;
                    //_ToolBar.Left = _grid.Left + _grid.RowHeaderWidth;
                    _ToolBar.Height = _grid.Height;
                    _ToolBar.Width = 25;
                    
                    //_ToolBar.BackColor = _grid.Parent.BackColor;
                    //_ToolBar.ForeColor = _grid.Parent.ForeColor;
                    _ToolBar.Visible = true;

                    //_ToolBar.Top = _grid.Bottom + 1;
                    //_ToolBar.Left = _grid.Left + _grid.RowHeaderWidth;
                    //_ToolBar.Width = _grid.Width;
                    //_ToolBar.BackColor = _grid.Parent.BackColor;
                    //_ToolBar.ForeColor = _grid.Parent.ForeColor;
                    //_ToolBar.Visible = true;
                    break;
                case ToolBarPosition.Bottom :
                   
                    //_grid.Height = _grid.Height - _ToolBar.Height;
                    //_grid.Height = _grid.Height - _ToolBar.Height;
                    _ToolBar.Location = new Point(_grid.Location.X, _grid.Location.Y + _grid.Height);
                    _ToolBar.Width = _grid.Width;
                    _ToolBar.Height = 23;
                    
                    break;
                default:
                    _ToolBar.Visible = false;
                    break;
            }
        }

        #endregion

        #region "     Events     "

        bool bl = true;
        private void OnGridLocationChanged(object sender, EventArgs e)
        {
            if (bl)
            {
                AdjustToolBarControlToGrid();
            }
        }

        private void OnGridResize(object sender, EventArgs e)
        {
            AdjustToolBarControlToGrid();
        }

        private void OnGridParentChanged(object sender, EventArgs e)
        {
            AddHeaderControl();
        }

        private void OnColorsChanged(object sender, EventArgs e)
        {
            AdjustToolBarControlToGrid();
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
            if (_ToolBar != null)
                _ToolBar.BeginInit();
        }

        public void EndInit()
        {
            _initializing = false;
            if (_ToolBar != null)
                _ToolBar.EndInit();
        }

        #endregion

    }
}
