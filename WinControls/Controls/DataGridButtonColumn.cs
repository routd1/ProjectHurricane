using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for DataGridButtonColumn.
	/// </summary>
	public class DataGridButtonColumn	:	DataGridTextBoxColumn
	{
		public NoKeyUpButton ColumnButton;

		private CurrencyManager _source;
		private int _rowNum;
		private bool _isEditing;

		public static int _RowCount;

		public DataGridButtonColumn(int Column) : base()
		{
			_source = null;
			_isEditing = false;
			_RowCount = -1;

			ColumnButton = new NoKeyUpButton();
			//ColumnButton.Image = Image.FromFile(@"F:\Tvd2000\Bmp\Application.ico");

			ColumnButton.Leave += new EventHandler(BtnLeave);
			ColumnButton.Enter += new EventHandler(BtnStartEditing);
			ColumnButton.GotFocus += new EventHandler(BtnStartEditing);
			ColumnButton.LostFocus += new EventHandler(BtnLeave);
			
			ColumnButton.Visible = false;
		}

		protected override void ConcedeFocus()
		{
			try
			{
				this.ColumnButton.Visible = false;
				//base.ConcedeFocus ();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Source + " - " + ex.Message);
			}
		}


		private void BtnStartEditing(object sender,EventArgs e)
		{
			_isEditing = true;
			base.ColumnStartedEditing((Control) sender);
		}
		
		protected override void SetDataGrid(DataGrid value)
		{
			base.SetDataGrid (value);
		}

		private void BtnLeave(object sender,EventArgs e)
		{
			if(_isEditing)
			{
				_source.Position = _rowNum;
				SetColumnValueAtRow(_source,_rowNum,"");
				_isEditing = false;
				Invalidate();
			}
			
			this.DataGridTableStyle.DataGrid.Scroll += new EventHandler(HandleScroll);
		}

		protected override bool Commit(CurrencyManager dataSource, int rowNum)
		{
			if(_isEditing)
			{
				_isEditing = false;
				dataSource.Position = rowNum;
				SetColumnValueAtRow(dataSource,rowNum,"");	
				
			}
			return false;
		}

		protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
		{
			base.Edit(source,rowNum,bounds,readOnly,instantText,cellIsVisible);

			_rowNum = rowNum;
			_source = source;
			
			BtnSettings();
			ColumnButton.Focus();
		}

		private void BtnSettings()
		{
			ColumnButton.Parent = this.TextBox.Parent;
			ColumnButton.Location = this.TextBox.Location;
			ColumnButton.Size = new Size(this.TextBox.Size.Width,ColumnButton.Size.Height);
			this.TextBox.Visible = false;
			ColumnButton.Visible = false;
			this.DataGridTableStyle.DataGrid.Scroll += new EventHandler(HandleScroll);
			ColumnButton.BringToFront();
		}

		private void HandleScroll(object sender, EventArgs e)
		{
			if(ColumnButton.Visible)
				ColumnButton.Hide();
		}

		protected override object GetColumnValueAtRow(CurrencyManager source, int rowNum)
		{
			return base.GetColumnValueAtRow (source, rowNum);
		}

		protected override void SetColumnValueAtRow(CurrencyManager source, int rowNum, object value)
		{
			source.Position = rowNum;
			base.SetColumnValueAtRow (source, rowNum, value);
		}
	}

	public class NoKeyUpButton : clsButton
	{
		private const int WM_KEYUP = 0x101;

//		protected override void WndProc(ref Message m)
//		{
//			if(m.Msg == WM_KEYUP)
//			{
//				return;
//			}
//			base.WndProc (ref m);
//		}
	}

}
