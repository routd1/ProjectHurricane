using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using ICTEAS.WinForms.Common;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for DataGridDateTimePicker.
	/// </summary>
	public class DataGridDateTimePicker	: DataGridTextBoxColumn
	{
		public event EnableCellEventHandler CheckCellEnabled;
		//chang color
		public event EnableCellColorEventHandler CheckCellColor;
		//

		public NoKeyUpDTP ColumnDateTimePicker;
		
		private CurrencyManager _source;
		private int _rowNum;
		private bool _isEditing;

		private int _col = 0;

		public static int _RowCount;

		public DataGridDateTimePicker(int Column) : base()
		{
			_source = null;
			_isEditing = false;
			_RowCount = -1;
			_col=Column;

			ColumnDateTimePicker = new NoKeyUpDTP();

			ColumnDateTimePicker.Format = DateTimePickerFormat.Custom;
			ColumnDateTimePicker.CustomFormat = clsCommonInfo.DateFormat;
			
			this.TextBox.TabStop = true;
			ColumnDateTimePicker.TabStop = true;

			ColumnDateTimePicker.Leave += new EventHandler(DTPLeave);
			ColumnDateTimePicker.Enter += new EventHandler(DTPStartEditing);
			ColumnDateTimePicker.ValueChanged += new EventHandler(ColumnDateTimePicker_ValueChanged);
			
			//ColumnDateTimePicker.ValueChanged += new EventHandler(DTPStartEditing);
			
		}

		private void ColumnDateTimePicker_ValueChanged(object sender , EventArgs e)
		{

			if(ColumnDateTimePicker.CustomFormat != " ")
			{
				DataGridCell objCell = new DataGridCell(_rowNum,_col);
				this.DataGridTableStyle.DataGrid.CurrentCell = objCell;
				this.TextBox.Text = ColumnDateTimePicker.Value.ToString(ColumnDateTimePicker.CustomFormat);
				if(TextChangedDTP != null)
				{
					TextChangedDTP(this,null);
				}
				_source.Position = _rowNum;
				if(ColumnDateTimePicker.Value != DateTime.MinValue)
					SetColumnValueAtRow(_source,_rowNum, ColumnDateTimePicker.Value.ToString(ColumnDateTimePicker.CustomFormat));
				else
					SetColumnValueAtRow(_source,_rowNum, DBNull.Value);

				_isEditing = false;
				Invalidate();
				ColumnDateTimePicker.Hide();
			}
		}

		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
		{
			bool enabled = true;
			if(CheckCellEnabled != null)
			{
				DataGridEnableEventArgs e = new DataGridEnableEventArgs(rowNum, _col, enabled);
				CheckCellEnabled(this, e);
				if(!e.EnableValue)
					backBrush = Brushes.LightGray;
			}

			//changing for color
			Brush  enabledcolor = null;
			if(CheckCellColor != null)
			{
				DataGridCellColorEventArgs  e = new DataGridCellColorEventArgs(rowNum, _col, enabledcolor);
				CheckCellColor(this, e);
				if(e.EnableColor != null)
					backBrush = e.EnableColor  ;
			}

			//end

			base.Paint (g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);
		}


		private void DTPStartEditing(object sender,EventArgs e)
		{
			_isEditing = true;
			base.ColumnStartedEditing((Control) sender);
		}

		private void HandleScroll(object sender, EventArgs e)
		{
			if(ColumnDateTimePicker.Visible)
				ColumnDateTimePicker.Hide();
		}

		private void DTPLeave(object sender,EventArgs e)
		{
			if(_isEditing)
			{
				_source.Position = _rowNum;
				if(ColumnDateTimePicker.Value != DateTime.MinValue)
					SetColumnValueAtRow(_source,_rowNum, ColumnDateTimePicker.Value.ToString(ColumnDateTimePicker.CustomFormat));
				else
					SetColumnValueAtRow(_source,_rowNum, DBNull.Value);

				_isEditing = false;
				Invalidate();
				
				
			}
			ColumnDateTimePicker.Hide();
			this.DataGridTableStyle.DataGrid.Scroll += new EventHandler(HandleScroll);
		}
		
		protected override void Edit(CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
		{	
			bool enabled = true;
			
			if(CheckCellEnabled != null)
			{
				DataGridEnableEventArgs et = new DataGridEnableEventArgs(rowNum, _col, enabled);
				CheckCellEnabled(this, et);
				enabled = et.EnableValue;
			}
			
			if(enabled)
			{
				base.Edit(source,rowNum,bounds,readOnly,instantText,cellIsVisible);

				if(cellIsVisible)
				{
					_rowNum = rowNum;
					_source = source;

					ColumnDateTimePicker.Parent = this.TextBox.Parent;
					Rectangle rectTemp = this.DataGridTableStyle.DataGrid.GetCurrentCellBounds();
					ColumnDateTimePicker.Location = rectTemp.Location;

					ColumnDateTimePicker.Size = new Size(this.TextBox.Size.Width,this.TextBox.Height);
			
					if(this.TextBox.Text.Trim() != "")
						ColumnDateTimePicker.Value = Convert.ToDateTime(this.TextBox.Text);
					else
						ColumnDateTimePicker.Value = DateTime.MinValue;

					this.TextBox.Visible = false;
			
					ColumnDateTimePicker.Visible = true;
					ColumnDateTimePicker.ReadOnly = this.TextBox.ReadOnly;
                
					this.DataGridTableStyle.DataGrid.Scroll += new EventHandler(HandleScroll);
			
					ColumnDateTimePicker.Show();
					ColumnDateTimePicker.BringToFront();
					ColumnDateTimePicker.Focus();
				}
				else
					ColumnDateTimePicker.Hide();
			}
			else
				ColumnDateTimePicker.Enabled = false;
		}

		public event EventHandler TextChangedDTP;
		protected override bool Commit(CurrencyManager dataSource, int rowNum)
		{
			if(_isEditing)
			{
				
				_isEditing = false;
				dataSource.Position = rowNum;
				if(ColumnDateTimePicker.Value != DateTime.MinValue)
					SetColumnValueAtRow(dataSource,rowNum,ColumnDateTimePicker.Value.ToString(ColumnDateTimePicker.CustomFormat));	
				else
					SetColumnValueAtRow(dataSource,rowNum,"");	
				
				if(TextChangedDTP != null)
				{
					TextChangedDTP(this,null);
				}
			}
			ColumnDateTimePicker.Hide();
			return true;
		}

		protected override void ConcedeFocus()
		{
			this.ColumnDateTimePicker.Visible = false;
			//base.ConcedeFocus ();
		}

		protected override object GetColumnValueAtRow(CurrencyManager source, int rowNum)
		{
			return base.GetColumnValueAtRow (source, rowNum);
		}

		protected override void SetColumnValueAtRow(CurrencyManager source, int rowNum, object value)
		{
			try
			{
				source.Position = rowNum;
				base.SetColumnValueAtRow (source, rowNum, value);
			}
			catch(Exception) {return;}
		}
	}

	public class NoKeyUpDTP	: clsDateTimePicker	
	{
		private const int WM_KEYUP = 0x101;

		protected override void WndProc(ref Message m)
		{
			if(m.Msg == WM_KEYUP)
			{
				return;
			}
			base.WndProc (ref m);
		}
	}
}
