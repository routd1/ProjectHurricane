using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsDataGridComboBox.
	/// </summary>
	public class DataGridComboBoxColumn : DataGridTextBoxColumn
	{
		public event EnableCellEventHandler CheckCellEnabled;
		//chang color
		public event EnableCellColorEventHandler CheckCellColor;
		//

		public NoKeyUpCombo ColumnComboBox;
		private System.Windows.Forms.CurrencyManager _source;
		private int _rowNum;
		private bool _isEditing;
		public static int _RowCount;
		
		private int _col = 0;

		private int xMargin;
		private int yMargin;

		public DataGridComboBoxColumn(int Column) : base()
		{
			_source = null;
			_isEditing = false;
			_RowCount = -1;
	
			xMargin = 2;
			yMargin = -5;

			ColumnComboBox = new NoKeyUpCombo();
			ColumnComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
		
			ColumnComboBox.Leave += new EventHandler(LeaveComboBox);
//			ColumnComboBox.Enter += new EventHandler(ComboMadeCurrent);
			ColumnComboBox.SelectedIndexChanged += new EventHandler(ComboStartEditing);
			ColumnComboBox .SelectionChangeCommitted +=new EventHandler(ColumnComboBox_SelectedIndexChanged);
		}

		private void ColumnComboBox_SelectedIndexChanged(object sender,EventArgs e)
		{
			this.TextBox.Text = ColumnComboBox.Text;
			if(TextChangedCombo != null)
			{
				TextChangedCombo(this,null);
			}
			_source.Position = _rowNum;
			SetColumnValueAtRow(_source, _rowNum, ColumnComboBox.Text);
			_isEditing = false;
			Invalidate();
			ColumnComboBox.Hide();
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
			base.Paint( g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);
		}


		private void ComboStartEditing(object sender, EventArgs e)
		{
			_isEditing = true;
			
			base.ColumnStartedEditing((Control) sender);
			
		}

		private void HandleScroll(object sender, EventArgs e)
		{
			if(ColumnComboBox.Visible)
				ColumnComboBox.Hide();
		}
		
		private void ComboMadeCurrent(object sender,EventArgs e)
		{
			if(_isEditing)
			{
				_source.Position = _rowNum;
				SetColumnValueAtRow(_source, _rowNum, ColumnComboBox.Text);
				_isEditing = false;
				Invalidate();
			}
			ColumnComboBox.Hide();
			this.DataGridTableStyle.DataGrid.Scroll -= new EventHandler(HandleScroll);
		}

		private void LeaveComboBox(object sender, EventArgs e)
		{
			if(_isEditing)
			{
				_source.Position = _rowNum;
				SetColumnValueAtRow(_source, _rowNum, ColumnComboBox.Text);
				_isEditing = false;
				Invalidate();
			}
			ColumnComboBox.Hide();
			this.DataGridTableStyle.DataGrid.Scroll += new EventHandler(HandleScroll);
		}

		private string GetText(object Value)
		{
			if(Value == null)
				return NullText;

			if(Value.ToString().Trim() != "")
				return Value.ToString();
			else
				return String.Empty;
		}

		private int DataGridTableGridLineWidth
		{
			get
			{
				if(this.DataGridTableStyle.GridLineStyle == DataGridLineStyle.Solid)
					return 1;
				else
					 return 0;
			}
		}

		protected override Size GetPreferredSize(Graphics g, object value)
		{
			Size Extends = Size.Ceiling(g.MeasureString(GetText(value),this.DataGridTableStyle.DataGrid.Font));
			Extends.Width += xMargin * 2 + DataGridTableGridLineWidth;
			Extends.Height += yMargin;
			return Extends;	
		}

		protected override int GetPreferredHeight(Graphics g, object value)
		{
			int NewLineIndex = 0, NewLines = 0;
			string ValueString = this.GetText(value);
			while(NewLineIndex != -1)
			{
				NewLineIndex = ValueString.IndexOf("r\n",NewLineIndex + 1);
				NewLines += 1;
			}
			return FontHeight * NewLines + yMargin;
		}


		protected override int GetMinimumHeight()
		{
			return ColumnComboBox.Height + yMargin;
		}


		protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
		{
			try
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
					base.Edit(source,rowNum, bounds, readOnly, instantText , cellIsVisible);

					_rowNum = rowNum;
					_source = source;
			
					bounds.Offset(xMargin,yMargin);
					bounds.Width -= xMargin * 2;
					bounds.Height -= yMargin;
					
					this.TextBox.TabStop = true;
					ColumnComboBox.Bounds = bounds;
					ColumnComboBox.TabStop = true;
					ColumnComboBox.Enabled = true;
					ColumnComboBox.Parent = this.TextBox.Parent;
					Rectangle rect = this.DataGridTableStyle.DataGrid.GetCurrentCellBounds();
					ColumnComboBox.Location = rect.Location;
					ColumnComboBox.Size = new Size(this.TextBox.Size.Width, ColumnComboBox.Size.Height);
					//ColumnComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
					ColumnComboBox.SelectedIndex = ColumnComboBox.FindStringExact(this.TextBox.Text);
					ColumnComboBox.Text =  this.TextBox.Text;
					this.TextBox.Visible = false;
					ColumnComboBox.Visible = true;
					
					ColumnComboBox.ReadOnly = this.TextBox.ReadOnly;

					this.DataGridTableStyle.DataGrid.Scroll += new EventHandler(HandleScroll);
				
					ColumnComboBox.BringToFront();
					ColumnComboBox.Focus();	
				}
				else
				{
					ColumnComboBox.Enabled = false;
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message + " - " + ex.Source);
			}

		}

		public event EventHandler TextChangedCombo;
		protected override bool Commit(System.Windows.Forms.CurrencyManager dataSource, int rowNum)
		{
			if(_isEditing)
			{
				_isEditing = false;
				dataSource.Position = rowNum;
				SetColumnValueAtRow(dataSource, rowNum, ColumnComboBox.Text);
				
				
			}
			return true;
		}

		protected override void ConcedeFocus()
		{
			base.ConcedeFocus();
		}

		protected override object GetColumnValueAtRow(System.Windows.Forms.CurrencyManager source, int rowNum)
		{
			object s =  base.GetColumnValueAtRow(source, rowNum);
			
			string[] dv = (string[]) this.ColumnComboBox.DataSource;
			
			if(dv != null)
			{
				int rowCount = dv.Length;
				int i = 0;

				//if things are slow, you could order your dataview
				//& use binary search instead of this linear one
				while (i < rowCount)
				{
					if( s.Equals( dv[i]))
						break;
					++i;
				}
			
				if(i < rowCount)
					return dv[i];
			}
			return DBNull.Value;
		}

		protected override void SetColumnValueAtRow(System.Windows.Forms.CurrencyManager source, int rowNum, object value)
		{
			
			source.Position = rowNum;
			object s = value;

			string[] dv = (string[])this.ColumnComboBox.DataSource;
			
			if(dv != null)
			{
				int rowCount = dv.Length;
				int i = 0;

				//if things are slow, you could order your dataview
				//& use binary search instead of this linear one
				while (i < rowCount)
				{
					if( s.Equals( dv[i]))
						break;
					++i;
				}
				if(i < rowCount)
					s =  dv[i];
				else
					s = DBNull.Value;
			}
			
			base.SetColumnValueAtRow(source, rowNum, s);
			
		}
	}

	public class NoKeyUpCombo : clsComboBox
	{
		private const int WM_KEYUP = 0x101;

		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			if(m.Msg == WM_KEYUP)
			{
				return;
			}
			base.WndProc(ref m);
		}
	}
}
