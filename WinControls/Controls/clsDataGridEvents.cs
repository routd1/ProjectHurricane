using System;
using System.Windows;
using System.Windows.Forms;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsEvents.
	/// </summary>

	public delegate void EnableCellEventHandler(object sender, DataGridEnableEventArgs e);

	public class DataGridEnableEventArgs : EventArgs
	{
		private int _column;
		private int _row;
		private bool _enablevalue;

		public DataGridEnableEventArgs(int row, int col, bool val)
		{
			_row = row;
			_column = col;
			_enablevalue = val;
		}

		public int Column
		{
			get{ return _column;}
			set{ _column = value;}
		}
		public int Row
		{
			get{ return _row;}
			set{ _row = value;}
		}
		public bool EnableValue
		{
			get{ return _enablevalue;}
			set{ _enablevalue = value;}
		}
	}

	public delegate void EnableF2KeyHandler(object sender, DataGridF2KeyArgs e);

	public class DataGridF2KeyArgs : EventArgs
	{
		private int _Row;
		private int _Col;
		private Keys _keyCode;

		public DataGridF2KeyArgs(Keys keyCode)
		{
			_keyCode = keyCode;
		}

		public DataGridF2KeyArgs(int Row, int Col, Keys keyCode)
		{
			_Row = Row;
			_Col = Col;
			_keyCode = keyCode;
		}
		
		public int Row
		{
			get{return _Row;}
			set{_Row = value;}
		}

		public int Col
		{
			get{return _Col;}
			set{_Col = value;}
		}

		public Keys KeyCode
		{
			get{return _keyCode;}
			set{_keyCode = value;}
		}
	}
}
