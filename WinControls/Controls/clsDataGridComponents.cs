using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsDataGridComponents.
	/// </summary>

	/// <summary>
	/// Set different Color for each Cell.
	/// </summary>
	public delegate void EnableCellColorEventHandler(object sender, DataGridCellColorEventArgs  e);
	public delegate Color delegateGetColorRowCol(int row, int col);
	public delegate void CustomPasteEventHandler(object sender);
	
	public class MyGridTextBoxColumn : DataGridTextBoxColumn
	{
		//in your handler, set the EnableValue to true or false, depending upon the row & col
		public event EnableCellEventHandler CheckCellEnabled;
		//chang color
		public event EnableCellColorEventHandler CheckCellColor;
		//
		private delegateGetColorRowCol _getColorRowCol;
		private bool _boolGetColor = false;

		private KeyTrapTextBox _keyTrapTextBox = null;
		private System.Windows.Forms.CurrencyManager _source = null;
		private int _rowNum;
		private bool _isEditing = false;

		private clsTxtBox.TypeEnum _txtType;
		private int _intPrecision;
		private int _decPrecision;
		private bool _isCurrency = false;

		public static int _RowCount = 0;
		
		private int _col;

		public bool IsCurrency
		{
			set 
			{
				_isCurrency = value;
				_keyTrapTextBox.isCurrency  = _isCurrency;
			}
		}
		public KeyTrapTextBox KeyTextBox
		{
			get
			{
				return _keyTrapTextBox;
			}
		}

		public delegateGetColorRowCol GetColorRowCol
		{
			set
			{
				_getColorRowCol = value;
				_boolGetColor = true;
			}
		}

		public clsTxtBox.TypeEnum TextType
		{
			set
			{
				_txtType = value;
				_keyTrapTextBox.Type = _txtType;
			}
		}

		public int IntPrecision
		{
			set
			{
				_intPrecision = value;
				_keyTrapTextBox.IntegerPrecision = _intPrecision;
			}
		}

		public int DecPrecision
		{
			set
			{
				_decPrecision = value;
				_keyTrapTextBox.DecimalPrecision = _decPrecision;
			}
		}

		public MyGridTextBoxColumn(int column) 
		{
			_col = column;

			this.TextBox.TabStop = true;
			_keyTrapTextBox = new KeyTrapTextBox();
            _keyTrapTextBox.BorderStyle = this.TextBox.BorderStyle;
			_keyTrapTextBox.AutoSize = false;
			_keyTrapTextBox.TabStop = true;
			_keyTrapTextBox.Leave += new EventHandler(LeaveKeyTrapTextBox);
			_keyTrapTextBox.KeyPress += new KeyPressEventHandler(TextBoxEditStarted);
			_keyTrapTextBox.KeyDown += new KeyEventHandler(TextBoxEdit);
			_keyTrapTextBox.TextChanged += new EventHandler(KeyTrapTextChange); 
		}


		private void KeyTrapTextChange(object sender,EventArgs e)
		{
			_isEditing = true;
			//this.TextBox.Text = _keyTrapTextBox.Text;
		}

		private void TextBoxEdit(object sender,KeyEventArgs e)
		{
			try
			{
				_isEditing = true;
				base.ColumnStartedEditing((Control) sender);
			}
			catch(Exception)
			{
				//string str = ex.Source + " - " + ex.Message + " - " + ex.StackTrace;
				return;
			}
		}

		private void TextBoxEditStarted(object sender, KeyPressEventArgs e)
		{
			_isEditing = true; 
			base.ColumnStartedEditing((Control) sender);
		}
		
		private void LeaveKeyTrapTextBox(object sender, EventArgs e)
		{
			try
			{
				if(_isEditing)
				{
					if(_source.Position == _rowNum)
					{
						if(_keyTrapTextBox.Text.Trim() != "")
						{
							SetColumnValueAtRow(_source, _rowNum, _keyTrapTextBox.Text.Trim());
						}
						else
						{
							SetColumnValueAtRow(_source,_rowNum, "");
						}

						_isEditing = false;
						Invalidate();
					}
				}
				_keyTrapTextBox.Hide();
			}
			catch(Exception)
			{
			//	MessageBox.Show(ex.Source + " - " + ex.Message);
				//return;
			}
		}

		protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Brush backBrush, System.Drawing.Brush foreBrush, bool alignToRight)
		{
			//can remove this if you don't want to vary the color of diabled cells
			bool enabled = true;
			if(CheckCellEnabled != null)
			{
				DataGridEnableEventArgs e = new DataGridEnableEventArgs(rowNum, _col, enabled);
				CheckCellEnabled(this, e);
				if(!e.EnableValue)
					backBrush = Brushes.LightGray;
				if(_boolGetColor)
					foreBrush = new SolidBrush(_getColorRowCol(rowNum,this._col));
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
			base.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);
		}

		protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
		{
			try
			{
                bool enabled = true;

                if (CheckCellEnabled != null)
                {
                    DataGridEnableEventArgs et = new DataGridEnableEventArgs(rowNum, _col, enabled);
                    CheckCellEnabled(this, et);
                    enabled = et.EnableValue;
                }

                if (enabled)//&& _keyTrapTextBox.Focused == false )
                {
                    _rowNum = rowNum;
                    _source = source;
                    _RowCount = source.Count;

                    try
                    {
                        base.Edit(source, rowNum, bounds, readOnly, instantText, cellIsVisible);
                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                    }

                    if (_txtType == clsTxtBox.TypeEnum.Float)
                    {
                        if (_keyTrapTextBox.isCurrency)
                        {
                            _keyTrapTextBox.MaxLength = _keyTrapTextBox.GetNoofCommas() + _intPrecision + _decPrecision + 1;
                        }
                        else
                        {
                            _keyTrapTextBox.MaxLength = _intPrecision + _decPrecision + 1;
                        }
                    }
                    else if (_txtType == clsTxtBox.TypeEnum.Integer)
                    {
                        if (_keyTrapTextBox.isCurrency)
                        {
                            _keyTrapTextBox.MaxLength = _keyTrapTextBox.GetNoofCommas() + _intPrecision;
                        }
                        else
                        {
                            _keyTrapTextBox.MaxLength = _intPrecision;
                        }
                    }
                    else
                        _keyTrapTextBox.MaxLength = this.TextBox.MaxLength;

                    _keyTrapTextBox.Parent = this.TextBox.Parent;
                    _keyTrapTextBox.Location = this.TextBox.Location;
                    _keyTrapTextBox.Size = this.TextBox.Size;
                    _keyTrapTextBox.Height = this.TextBox.Height;
                    _keyTrapTextBox.Width = this.TextBox.Width;

                    _keyTrapTextBox.Text = this.TextBox.Text;

                    if (_txtType == clsTxtBox.TypeEnum.Integer || _txtType == clsTxtBox.TypeEnum.Float)
                    {
                        _keyTrapTextBox.TextAlign = HorizontalAlignment.Right;
                        this.TextBox.TextAlign = HorizontalAlignment.Right;
                        this.Alignment = HorizontalAlignment.Right;
                    }

                    _keyTrapTextBox.ReadOnly = this.TextBox.ReadOnly;
                    _keyTrapTextBox.ForeColor = this.TextBox.ForeColor;

                    this.TextBox.Visible = false;
                    _keyTrapTextBox.Visible = true;
                    _keyTrapTextBox.BringToFront();
                    _keyTrapTextBox.Focus();
                    _keyTrapTextBox.SelectAll();

                    //if (_txtType == clsTxtBox.TypeEnum.Integer || _txtType == clsTxtBox.TypeEnum.Float)
                    //{
                    //    _keyTrapTextBox.TextAlign = HorizontalAlignment.Right;
                    //    this.TextBox.TextAlign = HorizontalAlignment.Right;
                    //    this.Alignment = HorizontalAlignment.Right;
                    //}
                }
            }
			catch
			{
				MessageBox.Show("EDIT");
			}
		}


		private string GetStringInCommaFormat(string strParamTextWithoutComma)
		{
			string strTextWithComma = strParamTextWithoutComma;
			bool flag = false;
			string integerPart = "";
			string DecimalPart = "";
			if(_keyTrapTextBox.Type == clsTxtBox.TypeEnum.Float )
			{
				#region Decimal

				int dotIndex = strTextWithComma.LastIndexOf ('.');
				if(dotIndex == -1)
				{
					dotIndex	= strTextWithComma.Length;
					DecimalPart = strTextWithComma.Substring (dotIndex);
					integerPart = strTextWithComma.Substring (0,dotIndex);
				}
				else
				{
					DecimalPart = strTextWithComma.Substring (dotIndex+1);
					if(_keyTrapTextBox.DecimalPrecision < DecimalPart .Length)
					{
						DecimalPart = DecimalPart .Substring (0,_keyTrapTextBox.DecimalPrecision);
					}
					integerPart = strTextWithComma.Substring (0,dotIndex);
					flag = true;
				}
				
				integerPart = integerPart.Replace (",","");
				
				string prefix = "";
				if( _keyTrapTextBox.AllowMinus && integerPart.IndexOf("-") == 0 )
				{
					integerPart = integerPart.TrimStart('-');
					prefix = "-";
				}

				if(integerPart.Length >= _keyTrapTextBox.CommaPrecision)
				{
				
					int temp = integerPart.Length ;
					for(int k=_keyTrapTextBox.CommaPrecision;k<temp;k+=_keyTrapTextBox.CommaPrecision)
					{
						integerPart = integerPart.Insert (temp-k,","); 
						
					
					}
				
				}
				if(flag)
					strTextWithComma = prefix + integerPart + "."+DecimalPart ;
				else
					strTextWithComma = prefix + integerPart;
				#endregion
			}
			else
			{
				#region Integer
				strTextWithComma = strTextWithComma.Replace (",","");
				if(strTextWithComma.Length >= _keyTrapTextBox.CommaPrecision)
				{
				
					int temp = strTextWithComma.Length ;
					for(int k=_keyTrapTextBox.CommaPrecision;k<temp;k+=_keyTrapTextBox.CommaPrecision)
					{
						strTextWithComma = strTextWithComma.Insert (temp-k,","); 
						//this.IntegerPrecision ++;
					
					}
				
				}
				#endregion
			}
			return strTextWithComma;
		}


		private string ExecuteDecimal(string DecimalNumber, int Precision)
		{
			try
			{
				string prefix = "";
				if( _keyTrapTextBox.AllowMinus && DecimalNumber.IndexOf("-") == 0 )
				{
					DecimalNumber = DecimalNumber.TrimStart('-');
					prefix = "-";
				}
				if(DecimalNumber .Trim () == "")
				{
					DecimalNumber = "0";
				}
				Decimal m_decTemp = Math.Round(Convert.ToDecimal(DecimalNumber),Precision);
				string m_strTemp = "0", m_strLocal = ".";

				for(int k=0; k < Precision; k++)
					m_strLocal += "0";
				
				m_strTemp += m_strLocal;
				string rep = m_decTemp.ToString("###" + m_strTemp + ";"); 
				string originalIntPortion;
				int index = DecimalNumber .LastIndexOf (".");
				if(index == -1)
				{

					originalIntPortion = DecimalNumber;
				}
				else
				{
					originalIntPortion = DecimalNumber .Substring (0,index);
				}
				if(originalIntPortion == "") originalIntPortion = "0";

				string nextIntPortion;
				index = rep .LastIndexOf (".");
				if(index == -1)
				{

					nextIntPortion = rep;
				}
				else
				{
					nextIntPortion = rep .Substring (0,index);
				}
				
				if(nextIntPortion == "") nextIntPortion = "0";
				rep = prefix + rep.Replace (nextIntPortion,originalIntPortion);

				if( _keyTrapTextBox.isCurrency )
					rep = GetStringInCommaFormat( rep );

				return rep;
			}
			catch(SystemException)
			{
				return "";
			}
		}

		protected override bool Commit(System.Windows.Forms.CurrencyManager dataSource, int rowNum)
		{
            if (_isEditing)
            {
                //_isEditing = false;
                //dataSource.Position = rowNum;
                string strWithComma = "";
                if (_keyTrapTextBox.Type == clsTxtBox.TypeEnum.Float && _keyTrapTextBox.AllowBlankInFloatValue == false)
                {
                    strWithComma = ExecuteDecimal(_keyTrapTextBox.Text, _keyTrapTextBox.DecimalPrecision);
                }
                else
                {
                    strWithComma = _keyTrapTextBox.Text;
                }
                //if (dataSource.Position == rowNum)//tirtho
                //    this.SetColumnValueAtRow(dataSource, rowNum, strWithComma);
            }

            return true;
        }

	}

	public class KeyTrapTextBox : clsTxtBox
	{
		public event EnableF2KeyHandler CellKeyDown;
		public event CustomPasteEventHandler CheckPasteContextMenu;
		
		private const int WM_KEYUP = 0x101; 
		private const int WM_PASTE = 0x302;
		private Keys _keyCode;

		public KeyTrapTextBox() : base()
		{
			this.Type = clsTxtBox.TypeEnum.Integer;
			this.TabStop=true;
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg == WM_KEYUP)
				return;
			if(m.Msg == WM_PASTE)
			{
				if(CheckPasteContextMenu  != null)
				{
					CheckPasteContextMenu(this);
					return;
				}
				base.WndProc (ref m);
			}

			base.WndProc (ref m);

			
		}

		public override bool PreProcessMessage(ref Message msg )
		{
			try
			{
				Keys keyCode = (Keys)(int)msg.WParam & Keys.KeyCode;
				
				_keyCode = keyCode;

				if(keyCode == Keys.F2)
				{
					if (this.ReadOnly == true)
					{
						DataGridF2KeyArgs e = new DataGridF2KeyArgs(keyCode);
						
						if(CellKeyDown != null)
							CellKeyDown(this,e);

						return false;
					}
				}
				return base.PreProcessMessage(ref msg);
			}
			catch(Exception ex)
			{
				string strErr = ex.Source + " - " + ex.Message + " - " + ex.StackTrace;
				return false;
			}
		}

		private void F2Handler(object sender,DataGridF2KeyArgs e)
		{
			e = new DataGridF2KeyArgs(_keyCode);
		}
	}

	public class DataGridCellDisableEnableEventArgs : EventArgs
	{
		private int _column;
		private int _row;
		

		public DataGridCellDisableEnableEventArgs(int row, int col)
		{
			_row = row;
			_column = col;
			
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
		
	}

	public class DataGridCellColorEventArgs : EventArgs
	{
		private int _column;
		private int _row;
		private Brush  _enableColor = null;

		public DataGridCellColorEventArgs(int row, int col,Brush colorVal)
		{
			_row = row;
			_column = col;
			_enableColor = colorVal;
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
		public Brush   EnableColor
		{
			get{ return _enableColor;}
			set{ _enableColor = value;}
		}
	}

}
